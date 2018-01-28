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

	private Sprite neutral;
	private Sprite neutralActive;

	void Start () {
		this.image = GetComponent<Image> ();
		this.neutral = Resources.Load<Sprite>("Sprites/neutral");
		this.neutralActive = Resources.Load<Sprite>("Sprites/neutral_active");
		this.image.sprite = this.neutral;
	}

	public void setInstruction(Instructions instruction) {
		this.instruction = instruction;
	}

	public void setControlImage(bool glow = false) {
		if (glow) {
			this.image.sprite = this.neutralActive;
		} else {
			this.image.sprite = this.neutral;
		}  
	}

	public void setInstructionImage(bool glow = false) {
		string active = glow ? "_active" : "";
		switch (this.instruction) {
			case Instructions.attack:
				this.image.sprite = Resources.Load<Sprite> ("Sprites/Attack" + active);
			break;
			case Instructions.backwards:
				this.image.sprite = Resources.Load<Sprite> ("Sprites/ArrowDown" + active);
			break;
			case Instructions.forward:
				this.image.sprite = Resources.Load<Sprite> ("Sprites/ArrowUp" + active);
			break;
			case Instructions.left:
				this.image.sprite = Resources.Load<Sprite> ("Sprites/ArrowLeft" + active);
			break;
			case Instructions.right:
				this.image.sprite = Resources.Load<Sprite> ("Sprites/ArrowRight" + active);
			break;
			default:
				this.image.sprite = Resources.Load<Sprite> ("Sprites/Error" + active);
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
			this.setControlImage ();
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
