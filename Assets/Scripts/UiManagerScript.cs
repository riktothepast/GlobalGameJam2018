using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UiManagerScript : MonoBehaviour
{
    public GameObject startCanvas;
    public List<UiPanelScript> uiPanels;
    public List<GameObject> uiPanelsPlaceholder;
    private int maxPlayers;
    private int numPlayers;

    void Start()
    {
        if (uiPanels.Count != uiPanelsPlaceholder.Count)
        {
            Debug.LogError("placeHolders must match panels");
        }
        maxPlayers = uiPanels.Count;
        // resetAll ();
        numPlayers = 0;
    }

    public void resetAll()
    {
        for (int botNumber = 0; botNumber < maxPlayers; botNumber++)
        {
            uiPanelsPlaceholder[botNumber].SetActive(true);
            uiPanels[botNumber].reset();
            uiPanels[botNumber].gameObject.SetActive(false);
        }
    }

    public void receiveCreatedPlayer(int botNumber)
    {
        uiPanelsPlaceholder[botNumber].gameObject.SetActive(false);
        uiPanels[botNumber].gameObject.SetActive(true);
        startCanvas.SetActive(false);
        // uiPanels [botNumber].reset ();
        numPlayers++;

        // StartCoroutine(this.methodTester());
    }

    public void receiveInstruction(int botNumber, Queue<Instructions> instructions)
    {
        int index = instructions.Count - 1;
        uiPanels[botNumber].instructionAdded(index, instructions.LastOrDefault());
    }

    public void gameStart()
    {
        for (int deadBot = numPlayers; deadBot < maxPlayers; deadBot++)
        {
            uiPanelsPlaceholder[deadBot].gameObject.SetActive(false);
        }
    }

    public void turnStart()
    {
        for (int botNumber = 0; botNumber < numPlayers; botNumber++)
        {
            uiPanels[botNumber].turnStart();
        }
    }

    public void turnEnd()
    {
        for (int botNumber = 0; botNumber < maxPlayers; botNumber++)
        {
            uiPanels[botNumber].turnEnd();
        }
    }

    public void executeInstruction(int botNumber, int instructionNumber)
    {
        uiPanels[botNumber].instructionExecute(instructionNumber);
    }

    private IEnumerator methodTester()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            int botNumber = 0;
            Queue<Instructions> demoInstructions = new Queue<Instructions>();

            demoInstructions.Enqueue(Instructions.left);
            this.receiveInstruction(0, demoInstructions);
            yield return new WaitForSeconds(1);

            demoInstructions.Enqueue(Instructions.right);
            this.receiveInstruction(0, demoInstructions);
            yield return new WaitForSeconds(1);

            demoInstructions.Enqueue(Instructions.forward);
            this.receiveInstruction(0, demoInstructions);
            yield return new WaitForSeconds(1);

            demoInstructions.Enqueue(Instructions.attack);
            this.receiveInstruction(0, demoInstructions);
            yield return new WaitForSeconds(1);

            this.turnStart();
            yield return new WaitForSeconds(1);

            this.executeInstruction(0, 0);
            yield return new WaitForSeconds(1);

            this.executeInstruction(0, 1);
            yield return new WaitForSeconds(1);

            this.executeInstruction(0, 2);
            yield return new WaitForSeconds(1);

            this.executeInstruction(0, 3);
            yield return new WaitForSeconds(1);

            this.turnEnd();
            yield return new WaitForSeconds(1);

            break;
        }
    }
}
