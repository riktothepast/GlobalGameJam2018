using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiImageScript : MonoBehaviour {
	public Sprite[] gate;

	private Image image = null;
	private bool closing = true;
	private bool closed = true;
	private Instructions instruction;
	private bool showInstructionAfterOpen = false;
	private bool openAfterClose = false;

	void Start () {
		this.image = GetComponent<Image> ();
		this.image.sprite = Resources.Load<Sprite>("Sprites/neutral");
	}

	public void setInstruction(Instructions instruction) {
		this.instruction = instruction;
	}

	public void setControlImage(string spriteName) {
		this.image.sprite = Resources.Load<Sprite>("Sprites/" + spriteName); 
	}

	public void setInstructionImage(bool glow = false) {
		switch (this.instruction) {
			case Instructions.attack:
			break;
		}
	}

	public void closeGate(bool openAfterClose = false) {
		this.closed = false;
		this.closing = true;
		this.openAfterClose = openAfterClose;
	}

	public void openGate(bool showInstructionAfterOpen = false) {
		this.closed = true;
		this.closing = true;
		this.showInstructionAfterOpen = showInstructionAfterOpen;
	}

	public void toggleGate() {
		this.closing = true;
	}

	private void onClose() {
		if (this.openAfterClose) {
			this.openGate ();
		}
	}

	private void onOpen() {
		if (this.showInstructionAfterOpen) {
			this.setInstructionImage ();
		} else {
			this.setControlImage ("neutral");
		}
	}

	void Update () {
		if (this.closing) {
			int index = (int)(Time.timeSinceLevelLoad * 15);
			index = index % gate.Length;
			this.image.sprite = gate[!this.closed ? index : ((gate.Length-1) - index)];
			if (index == (gate.Length-1)) {
				this.closing = false;
				this.closed = !this.closed;

				if (this.closed) {
					this.onClose ();
				} else {
					this.onOpen ();
				}
			}
		}
	}
}
