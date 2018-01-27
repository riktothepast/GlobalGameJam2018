using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Bot : MonoBehaviour
{
    public delegate void InstructionAddedDelegate(Queue<Instructions> actions);
    public InstructionAddedDelegate instructionsAdded;
    public float rotationSpeed = 100f;
    public float movementSpeed = 10f;
    public int displacementUnit = 1;
    public int maxInstructionCount = 4;
    [HideInInspector]
    public InputDevice Device { get; set; }
    [SerializeField]
    public Queue<Instructions> instructions;
    bool busy;
    GameBoard gameBoard;

    private void Awake()
    {
        instructions = new Queue<Instructions>();
        AddInstruction(Instructions.forward);
        AddInstruction(Instructions.left);
        AddInstruction(Instructions.left);
        AddInstruction(Instructions.left);
        AddInstruction(Instructions.left);
    }

    public void SetGameBoard(GameBoard board) {
        gameBoard = board;
    }

    private void Update() // just for test, the game manager will manage these insts.
    {
        DoNextInstruction();
    }

    public void AddInstruction(Instructions inst)
    {
        if (instructions.Count <= maxInstructionCount)
        {
            instructions.Enqueue(inst);
            if (instructionsAdded != null)
            {
                instructionsAdded(instructions);
            }
        }
    }

    public void DoNextInstruction()
    {
        if (instructions.Count > 0 && !busy)
        {
            ExecuteInstruction(instructions.Dequeue());
        }
    }

    void ExecuteInstruction(Instructions inst)
    {
        switch (inst)
        {
            case Instructions.left:
                StartCoroutine(Move(90, displacementUnit));
                break;
            case Instructions.right:
                StartCoroutine(Move(-90, displacementUnit));
                break;
            case Instructions.forward:
                StartCoroutine(Move(0, displacementUnit));
                break;
            case Instructions.backwards:
                StartCoroutine(Move(180, displacementUnit));
                break;
            default:
                break;
        }
    }

    bool TargetPositionInsideGameBoard(Vector3 position, Vector3 units) {
        Vector2 target = new Vector2(position.x, position.z);
        target.x = target.x / gameBoard.tileSize + units.x;
        target.y = target.y / gameBoard.tileSize + units.z;
        if (target.x > -1 && target.x < gameBoard.boardSize.x 
            && target.y > -1 && target.y < gameBoard.boardSize.y) {
            return true;
        }
        return false;
    }

    IEnumerator Move(float angle, float units)
    {
        busy = true;
        Vector3 desiredRotation = transform.rotation.eulerAngles;
        desiredRotation.y += angle;
        while (transform.rotation != Quaternion.Euler(desiredRotation))
        {
            transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.rotation.eulerAngles, desiredRotation, Time.deltaTime * rotationSpeed));
            yield return null;
        }
        Vector3 desiredDestination = transform.position + transform.forward * units;
        if (TargetPositionInsideGameBoard(transform.position, transform.forward))
        {
            while (transform.position != desiredDestination)
            {
                transform.position = Vector3.MoveTowards(transform.position, desiredDestination, Time.deltaTime * movementSpeed);
                yield return null;
            }
        }
        busy = false;
    }
}
