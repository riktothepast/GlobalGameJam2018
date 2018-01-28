using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiImageScript : MonoBehaviour {
	public Sprite[] gate;

	private Image image = null;
	private bool closing = true;
	private bool closed = true;

	void Start () {
		this.image = GetComponent<Image> ();
		this.image.sprite = Resources.Load<Sprite>("Sprites/neutral");
	}

	public void setControlImage(string spriteName) {
		this.image.sprite = Resources.Load<Sprite>("Sprites/" + spriteName); 
	}

	public void setInstructionImage(Instructions instruction) {
		switch (instruction) {
			case Instructions.attack:
			break;
		}
	}

	public void toggleGate() {
		this.closing = true;
	}

	private void onClose() {
		Debug.Log ("onClose()");
	}

	private void onOpen() {
		Debug.Log ("onOpen()");
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
