using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour {

	public float moveSpeed;
	public float waitWinnerTimer;
	public float resetTime;
	public Text creditsText;
	private bool showCredits;
	private bool resets;

	public GameObject creditsPanel;
	private float timer;
	private float resetTimer;
	// Use this for initialization
	void Start () {
		showCredits = false;
		timer = 0;
		resets = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (resets) {
			resetTimer += Time.deltaTime;
			creditsPanel.transform.localPosition = Vector3.zero;
			creditsText.text = "Restart in " + (resetTime - resetTimer).ToString("00.00") +"\n Winner" ;
			if (resetTimer > resetTime) {
				UnityEngine.SceneManagement.SceneManager.LoadScene ("theAllMightyScene");
			}
		} else {
			timer += Time.deltaTime;
			if (timer > waitWinnerTimer) {
				showCredits = true;
			}
			if (showCredits) {
				if (creditsPanel.transform.localPosition.y < 3588.317f) {
					creditsPanel.transform.position += Vector3.up * moveSpeed;
				} else {
					resetTimer += Time.deltaTime;
					if (resetTimer > 1.3f) {
						resets = true;
						resetTimer = 0;
				
					}

				}
			}
		}
	}
}
