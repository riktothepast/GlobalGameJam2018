using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiImageScript : MonoBehaviour {
	public Sprite[] gate;

	private Image image = null;
	private bool closing = false;
	private bool closed = false;
	private Instructions instruction;
	private bool showInstructionAfterOpen = false;
	private bool openAfterClose = false;

	private Sprite neutral;
	private Sprite neutralActive;

	private int currentGateFrame = 0;
	private float frameTimer;

	void Start () {
		this.image = GetComponent<Image> ();
		this.neutral = Resources.Load<Sprite>("Sprites/neutral");
		this.neutralActive = Resources.Load<Sprite>("Sprites/neutral_active");
		this.image.sprite = this.neutral;
		currentGateFrame = 0;
		gateFrameTimeIndex = 0;
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

	private int[] gateFrameTimings = new int[]{2, 1, 3, 1};
	[System.NonSerialized]
	private int gateFrameTimeIndex = 0;

	void Update () {
		if (this.closing) {
			frameTimer += Time.deltaTime;
			if (frameTimer > 1/60) {
				frameTimer = 0;
				if (currentGateFrame < gateFrameTimings.Length && gateFrameTimeIndex > gateFrameTimings [currentGateFrame]) {
					gateFrameTimeIndex = 0;
					if (currentGateFrame == gate.Length - 1) {
						currentGateFrame = 0;
						this.closing = false;
						this.closed = !this.closed;
						if (this.closed) {
							this.onClose ();
						} else {
							this.onOpen ();
						}
					} else {
						currentGateFrame += 1;
						this.image.sprite = gate [!this.closed ? currentGateFrame : ((gate.Length - 1) - currentGateFrame)];
					}
				} else {
					gateFrameTimeIndex += 1;								
				}
			}
		}
	}
}
