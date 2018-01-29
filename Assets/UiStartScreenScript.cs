using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiStartScreenScript : MonoBehaviour {
	public Sprite[] frames;

	private Image image = null;
	private int currentFrame = 0;
	private float frameTimer;

	private int[] frameTimings = new int[]{2, 1, 3, 1};
	[System.NonSerialized]
	private int frameTimeIndex = 0;

	void Start() {
		this.image = GetComponent<Image> ();
		currentFrame = 0;
		frameTimeIndex = 0;
	}

	void Update () {
		frameTimer += Time.deltaTime;
		if (frameTimer > 1/60) {
			frameTimer = 0;
			if (currentFrame < frameTimings.Length && frameTimeIndex > frameTimings [currentFrame]) {
				frameTimeIndex = 0;
				if (currentFrame == frames.Length - 1) {
					currentFrame = 0;
				} else {
					currentFrame += 1;
					this.image.sprite = frames [currentFrame];
				}
			} else {
				frameTimeIndex += 1;
			}
		}
	}
}
