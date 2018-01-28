using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManagerScript : MonoBehaviour {

	public List<UiPanelScript> uiPanels;
	public List<GameObject> uiPanelsPlaceholder;
	private int maxPlayers;

	void Start () {
		if (uiPanels.Count != uiPanelsPlaceholder.Count) {
			Debug.LogError ("placeHolders must match panels");
		}
		maxPlayers = uiPanels.Count;
		// resetAll ();
	}

	public void resetAll() {
		for (int botNumber = 0; botNumber < maxPlayers; botNumber++) {
			uiPanelsPlaceholder [botNumber].SetActive (true);
			uiPanels [botNumber].reset ();
			uiPanels [botNumber].gameObject.SetActive (false);
		}
	}

	public void receiveCreatedPlayer(int botNumber) {
		uiPanelsPlaceholder [botNumber].gameObject.SetActive (false);
		uiPanels [botNumber].gameObject.SetActive (true);
		uiPanels [botNumber].reset ();
	}

	public void receiveInstruction(int botNumber, Queue<Instructions> instructions) {
		Instructions instruction = instructions.Peek ();
		int index = instructions.Count - 1;
		uiPanels [botNumber].instructionAdded(index, instruction);
	}

	public void turnStart() {
		for (int botNumber = 0; botNumber < maxPlayers; botNumber++) {
			uiPanels [botNumber].turnStart ();
		}
	}
		
	public void turnEnd() {
		for (int botNumber = 0; botNumber < maxPlayers; botNumber++) {
			uiPanels [botNumber].turnEnd ();
		}
	}

	public void executeInstruction(int botNumber, int instructionNumber) {
		uiPanels [botNumber].instructionExecute(instructionNumber);
	}
}
