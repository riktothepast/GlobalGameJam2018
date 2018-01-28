using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ImageScript : MonoBehaviour {

	public Sprite[] sprites;
	public Sprite[] gate;

	private Image image = null;
	private bool closing = true;
	private bool closed = true;

	void Start () {
		this.image = GetComponent<Image> ();
		this.image.sprite = sprites[0];
	}

	public void setImage(string imageName) {
		int index;
		switch (imageName) {
		case "item":
			index = 0;
			break;
		case "item_active":
			index = 1;
			break;
		case "neutral":
			index = 2;
			break;
		case "neutral_active":
			index = 3;
			break;
		default:
			index = 0;
			break;
		}
		this.image.sprite = sprites[index];
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
