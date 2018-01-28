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
    Vector3 lastDir;

    private void Awake()
    {
        instructions = new Queue<Instructions>();
    }

    public void SetGameBoard(GameBoard board) {
        gameBoard = board;
        lastDir = Vector3.forward;
    }

    public void CheckInstructionInput() {
        if (Device.DPadRight.WasPressed)
        {
            AddInstruction(Instructions.right);
        } else 
        if (Device.DPadLeft.WasPressed)
        {
            AddInstruction(Instructions.left);
        } else 
        if (Device.DPadUp.WasPressed)
        {
            AddInstruction(Instructions.forward);
        } else 
        if (Device.DPadDown.WasPressed)
        {
            AddInstruction(Instructions.backwards);
        }
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
            busy = true;
            ExecuteInstruction(instructions.Dequeue());
        }
    }

    void ExecuteInstruction(Instructions inst)
    {
        switch (inst)
        {
            case Instructions.left:
                StartCoroutine(Move(Vector3.left, displacementUnit));
                break;
            case Instructions.right:
                StartCoroutine(Move(Vector3.right, displacementUnit));
                break;
            case Instructions.forward:
                StartCoroutine(Move(Vector3.forward, displacementUnit));
                break;
            case Instructions.backwards:
                StartCoroutine(Move(Vector3.back, displacementUnit));
                break;
            default:
                break;
        }
    }

    Vector3 TargetPositionInsideGameBoard(Vector3 target) {
        Vector2 mapSize = new Vector2((gameBoard.boardSize.x - 1) * gameBoard.tileSize, (gameBoard.boardSize.y - 1) * gameBoard.tileSize);
        target.x = Mathf.Clamp(target.x, 0, mapSize.x);
        target.z = Mathf.Clamp(target.z, 0, mapSize.y);
        return target;
    }

    IEnumerator Move(Vector3 dir, float units)
    {
        Vector3 desiredRotation = transform.GetChild(0).rotation.eulerAngles;
        if (Vector3.Distance(dir, Vector3.forward) < 0.1f)
        {
            desiredRotation.y = 0;
        } else if (Vector3.Distance(dir, Vector3.back) < 0.1f)
        {
            desiredRotation.y = 180;
        } else if (Vector3.Distance(dir, Vector3.left) < 0.1f)
        {
            desiredRotation.y = -90;
        } else if (Vector3.Distance(dir, Vector3.right) < 0.1f)
        {
            desiredRotation.y = 90;
        }
        float timerVal = 0;
        Quaternion startRot = transform.GetChild(0).transform.rotation;
        Quaternion endRot = Quaternion.Euler(desiredRotation);
        while (timerVal < 1f)
        {
            timerVal += Time.deltaTime * rotationSpeed;
            transform.GetChild(0).rotation = Quaternion.Slerp(startRot, endRot, timerVal);
            yield return new WaitForEndOfFrame();
        }
        transform.GetChild(0).transform.rotation = endRot;
        Vector3 desiredDestination = transform.position + (dir * units);
        desiredDestination = TargetPositionInsideGameBoard(desiredDestination);
        float currentTime = 0;
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime * movementSpeed;
            transform.position = Vector3.Lerp(transform.position, desiredDestination, Time.deltaTime * movementSpeed);
            yield return null;
        }
        lastDir = dir;
        transform.position = desiredDestination;
        busy = false;
    }

    public bool Finished() {
        return busy;
    }

    public bool HasInstructionsLeft() {
        return instructions.Count > 0 ? true : false;
    }
}
