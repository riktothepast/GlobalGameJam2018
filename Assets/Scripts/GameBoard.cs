using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{

    public MultiplayerBasicExample.MultiPlayerManager mpManager;
    public GameObject tilePrefab;
    public Vector3 boardSize;
    public int tileSize;
    public List<Bot> bots;
    GameStates currentState = GameStates.initialization;
    public delegate void TurnStartedDelegate();
    public delegate void TurnEndedDelegate();
    public TurnEndedDelegate turnStarted;
    public TurnEndedDelegate turnEnded;
    public UiManagerScript uiManager;

    Queue<Vector3> botPosition = new Queue<Vector3>();

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

    }

    public void CreateBotPosition()
    {
        botPosition.Enqueue(new Vector3(0, 0, 0));
        botPosition.Enqueue(new Vector3(0, 0, (boardSize.y - 1) * tileSize));
        botPosition.Enqueue(new Vector3((boardSize.x - 1) * tileSize, 0, 0));
        botPosition.Enqueue(new Vector3((boardSize.x - 1) * tileSize, 0, (boardSize.y - 1) * tileSize));
    }

    public Vector3 GetPosition()
    {
        return botPosition.Dequeue();
    }

    void Start()
    {
        turnStarted += uiManager.turnStart;
        turnEnded += uiManager.turnEnd;
        bots = new List<Bot>();
        CreateBaseBoard();
        CreateBotPosition();
        currentState = GameStates.initialization;
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameStates.initialization:
                mpManager.CheckForControllers();
                break;
            case GameStates.input:
                break;
            case GameStates.movement:
                break;
            case GameStates.boardActions:
                break;
            default:
                break;
        }
    }

}
