﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Bot : MonoBehaviour
{
    public delegate void InstructionAddedDelegate(int botNumber, Queue<Instructions> actions);
    public delegate void InstructionExecutedDelegate(int botNuber, int instructionIndex);
    public InstructionAddedDelegate instructionsAdded;
    public InstructionExecutedDelegate instructionExecuted;
    public float rotationSpeed = 100f;
    public float movementSpeed = 10f;
    public int displacementUnit = 1;
    public int maxInstructionCount = 4;
    public GameObject projectile;
    [HideInInspector]
    public InputDevice Device { get; set; }
    [HideInInspector]
    public int playerNumber;
    UiManagerScript uiManager;
    [SerializeField]
    public Queue<Instructions> instructions;
    bool busy;
    GameBoard gameBoard;
    Vector3 lastDir;
    bool disabled;
    [HideInInspector]
    public EffectsService effectService;

    private void Awake()
    {
        instructions = new Queue<Instructions>();
    }

    public void SetGameBoard(GameBoard board)
    {
        gameBoard = board;
        lastDir = Vector3.forward;
    }

    public void SetUIManager(UiManagerScript uiManager)
    {
        this.uiManager = uiManager;
        instructionsAdded += this.uiManager.receiveInstruction;
        instructionExecuted += this.uiManager.executeInstruction;
    }

    public void Disable()
    {
        effectService.PlaceSmoke(transform.position);
        effectService.PlayExplosionSound();
        disabled = true;
        gameBoard.disabledPlayers++;
    }

    public void CheckInstructionInput()
    {
        if (Device.DPadRight.WasPressed)
        {
            AddInstruction(Instructions.right);
        }
        else
        if (Device.DPadLeft.WasPressed)
        {
            AddInstruction(Instructions.left);
        }
        else
        if (Device.DPadUp.WasPressed)
        {
            AddInstruction(Instructions.forward);
        }
        else
        if (Device.DPadDown.WasPressed)
        {
            AddInstruction(Instructions.backwards);
        }
        else
        if (Device.Action1.WasReleased)
        {
            AddInstruction(Instructions.attack);
        }
        else
        if (Device.Action2.WasReleased)
        {
            AddInstruction(Instructions.skip);
        }
    }

    public void AddInstruction(Instructions inst)
    {
        if (instructions.Count < maxInstructionCount && !disabled)
        {
            instructions.Enqueue(inst);
            if (instructionsAdded != null)
            {
                instructionsAdded(playerNumber, instructions);
            }
        }
    }

    public void DoNextInstruction()
    {
        if (instructions.Count > 0 && !busy && !disabled)
        {
            busy = true;
            instructionExecuted(playerNumber, 4 - instructions.Count);
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
            case Instructions.attack:
                StartCoroutine(Attack());
                break;
            case Instructions.skip:
                busy = false;
                break;
            default:
                break;
        }
    }


    Vector3 TargetPositionInsideGameBoard(Vector3 target)
    {
        Vector2 mapSize = new Vector2((gameBoard.boardSize.x - 1) * gameBoard.tileSize, (gameBoard.boardSize.y - 1) * gameBoard.tileSize);
        target.x = Mathf.Clamp(target.x, 0, mapSize.x);
        target.z = Mathf.Clamp(target.z, 0, mapSize.y);
        return target;
    }

    IEnumerator Move(Vector3 dir, float units)
    {
        effectService.PlayMoveSound();
        Vector3 desiredRotation = transform.GetChild(0).rotation.eulerAngles;
        if (Vector3.Distance(dir, Vector3.forward) < 0.1f)
        {
            desiredRotation.y = 0;
        }
        else if (Vector3.Distance(dir, Vector3.back) < 0.1f)
        {
            desiredRotation.y = 180;
        }
        else if (Vector3.Distance(dir, Vector3.left) < 0.1f)
        {
            desiredRotation.y = -90;
        }
        else if (Vector3.Distance(dir, Vector3.right) < 0.1f)
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

    IEnumerator Attack()
    {
        yield return null;
        GameObject.Instantiate(projectile, transform.position + transform.GetChild(0).transform.forward * displacementUnit, Quaternion.identity);
        busy = false;
    }

    public bool Finished()
    {
        return busy;
    }

    public bool IsDisabled()
    {
        return disabled;
    }

    public bool HasInstructionsLeft()
    {
        return instructions.Count > 0 ? true : false;
    }

    public void StartEngine()
    {
        effectService.PlaceEngine();
    }
}
