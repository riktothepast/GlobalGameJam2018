using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManagerScript : MonoBehaviour {

	public List<UiPanelScript> uiPanels;
	public List<UiPanelScript> uiPanelsPlaceholder;

	public void resetAll() {
		for (int i = 0; i < 4; i++) {
			uiPanelsPlaceholder [i].gameObject.SetActive (true);
			uiPanels [i].gameObject.SetActive (false);
		}
	}

	public void receiveCreatedPlayer(int botNumber) {
		uiPanelsPlaceholder [botNumber].gameObject.SetActive (false);
		uiPanels [botNumber].gameObject.SetActive (true);
	}

	public void receiveInstruction(int botNumber, Queue<>) {
		
	}

}
