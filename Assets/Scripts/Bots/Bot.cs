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

    private void Awake()
    {
        instructions = new Queue<Instructions>();
        AddInstruction(Instructions.left);
        AddInstruction(Instructions.forward);
        AddInstruction(Instructions.left);
        AddInstruction(Instructions.forward);
        AddInstruction(Instructions.right);
        AddInstruction(Instructions.forward);
        AddInstruction(Instructions.backwards);
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
            instructionsAdded(instructions);
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

    IEnumerator Move(float angle, float units)
    {
        busy = true;
        Vector3 desiredRotation = transform.rotation.eulerAngles;
        desiredRotation.y += angle;
        while (transform.rotation.eulerAngles != desiredRotation)
        {
            transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.rotation.eulerAngles, desiredRotation, Time.deltaTime * rotationSpeed));
            yield return null;
        }
        yield return null;
        Vector3 desiredDestination = transform.position + transform.forward * units;
        while (transform.position != desiredDestination)
        {
            transform.position = Vector3.MoveTowards(transform.position, desiredDestination, Time.deltaTime * movementSpeed);
            yield return null;
        }
        busy = false;
    }
}
