using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {

    public GameObject tilePrefab;
    public Vector2 boardSize;
    public int tileSize;
    public List<Bot> bots;

    enum GameStates {
        initialization,
        input,
        movement,
        boardActions,
    }

    public void CreateBaseBoard() {
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

    public void AddHazards() {

    }

    public void InstatiateBots() {
        Queue<Vector2> botPosition = new Queue<Vector2>();
        botPosition.Enqueue(new Vector2(0, 0));
        botPosition.Enqueue(new Vector2(boardSize.x, 0));
        botPosition.Enqueue(new Vector2(0, boardSize.y));
        botPosition.Enqueue(new Vector2(boardSize.x, boardSize.y));

    }

	void Start () {
        bots = new List<Bot>();
        CreateBaseBoard();
        InstatiateBots();
    }
	
}
