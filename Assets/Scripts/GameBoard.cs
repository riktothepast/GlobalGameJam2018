using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{

    public MultiplayerBasicExample.MultiPlayerManager mpManager;
    public GameObject tilePrefab;
    public GameObject trapPrefab;
    public Vector3 boardSize;
    public int maxNumberOfTraps;
    public Timer timer;
    public int tileSize;
    GameStates currentState = GameStates.initialization;
    public delegate void TurnStartedDelegate();
    public delegate void TurnEndedDelegate();
    public TurnEndedDelegate turnStarted;
    public TurnEndedDelegate turnEnded;
    public UiManagerScript uiManager;

    Queue<Vector3> botPosition = new Queue<Vector3>();
    List<Transform> traps = new List<Transform>();

    enum GameStates
    {
        initialization,
        input,
        movement,
        boardActions,
    }

    public void CreateBaseBoard()
    {
        for (int x = 0; x < boardSize.x; x += 1)
        {
            for (int y = 0; y < boardSize.y; y += 1)
            {
                GameObject tile = Instantiate(tilePrefab);
                tile.transform.parent = transform;
                tile.transform.position = new Vector3(x * tileSize, 0, y * tileSize);
            }
        }
    }

    public void AddHazards()
    {
        int numberOfTraps = Random.Range(2, maxNumberOfTraps + 1);
        int placedTraps = 0;
        while (placedTraps <= numberOfTraps)
        {
            float xPosition = (int)Random.Range(0, boardSize.x - 1);
            float yPosition = (int)Random.Range(0, boardSize.y - 1);
            if (!IsPlayerInThere(xPosition, yPosition))
            {
                Vector3 trapPosition = new Vector3(xPosition * tileSize, 0f, yPosition * tileSize);
                GameObject trap = Instantiate(trapPrefab, trapPosition, Quaternion.identity);
                traps.Add(trap.transform);
                placedTraps++;
            }
        }
    }

    private bool IsPlayerInThere(float xPosition, float yPosition)
    {
        Vector3 trapPosition = new Vector3(xPosition * tileSize, 0f, yPosition * tileSize);
        bool foundAPlayerInThatPosition = false;
        foreach (Vector3 playerPosition in botPosition)
        {
            if (Vector3.Distance(trapPosition, playerPosition) < Mathf.Epsilon)
            {
                foundAPlayerInThatPosition = true;
                break;
            }
        }

        return foundAPlayerInThatPosition;
    }

    public void CreateBotPosition()
    {
        botPosition.Enqueue(new Vector3(0, 0, (boardSize.y - 1) * tileSize));
        botPosition.Enqueue(new Vector3((boardSize.x - 1) * tileSize, 0, (boardSize.y - 1) * tileSize));
        botPosition.Enqueue(new Vector3(0, 0, 0));
        botPosition.Enqueue(new Vector3((boardSize.x - 1) * tileSize, 0, 0));
    }

    public Vector3 GetPosition()
    {
        return botPosition.Dequeue();
    }

    void Start()
    {
        CreateBaseBoard();
        CreateBotPosition();
        AddHazards();
        StartCoroutine(Initialization());
        turnStarted += uiManager.turnStart;
        turnEnded += uiManager.turnEnd;
    }

    void InitializeApplication()
    {
        if (mpManager.players.Count >= 1)
        {
            foreach (Bot bot in mpManager.players)
            {
                if (bot.Device.CommandWasPressed)
                {
                    currentState = GameStates.input;
                    Debug.Log("Starting game with " + mpManager.players.Count + " players");
                    StopAllCoroutines();
                    StartCoroutine(InputCheck());
                }
            }
        }
    }

    bool CheckForPlayerInput()
    {
        bool instructionsReady = true;
        foreach (Bot player in mpManager.players)
        {
            player.CheckInstructionInput();
            if (player.instructions.Count < player.maxInstructionCount && !player.IsDisabled())
            {
                instructionsReady = false;
            }
        }
        return instructionsReady;
    }

    IEnumerator Initialization()
    {
        while (currentState == GameStates.initialization)
        {
            mpManager.CheckForControllers();
            InitializeApplication();
            yield return null;
        }
    }

    IEnumerator HazardCheck()
    {
        for (int i = mpManager.players.Count - 1; i >= 0; i--)
        {
            foreach (Transform trap in traps)
            {
                Transform player = mpManager.players[i].transform;
				bool isBotDisabled = player.GetComponent<Bot> ().IsDisabled();

				if (!isBotDisabled) {
					Vector3 playerPosition = player.position;
					if (Vector3.Distance(playerPosition, trap.position) < Mathf.Epsilon && !isBotDisabled)
					{
						trap.GetComponent<Trap>().DoDamage(player.gameObject);
						break;
					}
				}
            }
            yield return null;
        }
        StartCoroutine(InputCheck());
    }

    IEnumerator InputCheck()
    {
        currentState = GameStates.input;
        while (!CheckForPlayerInput())
        {
            yield return null;
        }
        StopAllCoroutines();
        turnStarted();
        StartCoroutine(Movement(0));
    }

    IEnumerator Movement(int currentIndex)
    {
        currentState = GameStates.movement;
        mpManager.players[currentIndex].DoNextInstruction();
        while (mpManager.players[currentIndex].Finished())
        {
            yield return null;
        }
        if (currentIndex + 1 < mpManager.players.Count)
        {
            StopAllCoroutines();
            StartCoroutine(Movement(currentIndex + 1));
        }
        else
        {
            bool instructionsLeft = false;
            foreach (Bot player in mpManager.players)
            {
                if (player.HasInstructionsLeft() && !player.IsDisabled())
                {
                    instructionsLeft = true;
                }
                yield return null;
            }
            if (instructionsLeft)
            {
                StopAllCoroutines();
                StartCoroutine(Movement(0));
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(HazardCheck());
                turnEnded();
            }
        }
    }

    void OnGUI()
    {
        const float h = 22.0f;
        var y = 80.0f;

        GUI.Label(new Rect(10, y, 300, y + h), "Current game state: " + currentState);
        y += h;
    }

}
