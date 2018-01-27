using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManagerScript : MonoBehaviour {

	public List<UiPanelScript> uiPanels;
	public List<GameObject> uiPanelsPlaceholder;

	public void resetAll() {
		for (int i = 0; i < 4; i++) {
			uiPanelsPlaceholder [i].SetActive (true);
			uiPanels [i].gameObject.SetActive (false);
		}
	}

	public void receiveCreatedPlayer(int botNumber) {
		uiPanelsPlaceholder [botNumber].gameObject.SetActive (false);
		uiPanels [botNumber].gameObject.SetActive (true);
	}

	public void receiveInstruction(int botNumber, Queue<Instructions> instructions) {
		Instructions instruction = instructions.Peek ();
		int index = instructions.Count - 1;
		uiPanels [botNumber].images [index].setInstructionImage (instruction);
	}

	public void resetPanels() {
		
	}
}
