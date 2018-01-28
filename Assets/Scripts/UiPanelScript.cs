using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPanelScript : MonoBehaviour {
	public UiImageScript[] images;

	public void instructionAdded(int index, Instructions instruction) {
		images [index].closeGate ();
		images [index].setInstruction (instruction);
		if (index + 1 < images.Length) {
			images [index + 1].setControlImage ("neutral_active");
		}
	}

	public void turnStart() {
		for (int i = 0; i < images.Length; i++) {
			images [i].openGate (true);
		}
	}

	public void instructionExecute(int index) {
		images [index].setInstructionImage(true);
	}

	public void turnEnd() {
		for (int i = 0; i < images.Length; i++) {
			images [i].closeGate (true);
		}
	}

	public void reset() {
		for (int i = 1; i < images.Length; i++) {
			images [i].setControlImage ("neutral");
		}
		images [0].setControlImage ("neutral_active");
	}
}
