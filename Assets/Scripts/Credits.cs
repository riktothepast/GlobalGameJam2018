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
	public Text winner;
	private float timer;
	private float resetTimer;
    public GameObject referencePoint;
    public List<GameObject> bots;
	// Use this for initialization
	void Start () {
		showCredits = false;
		timer = 0;
		resets = false;
		if (PlayerPrefs.HasKey ("Winner")) {
			int winnerInt = PlayerPrefs.GetInt ("Winner");
			if (winnerInt >= 0) {
				winner.text = "Player " + (winnerInt + 1);
                GameObject.Instantiate(bots[winnerInt], referencePoint.transform, false);
			} else {
				winner.text = "Draw";
			}
		} else {
			winner.text = "Draw";
		}
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
