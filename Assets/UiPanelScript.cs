using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPanelScript : MonoBehaviour {
	public ImageScript[] images;

	public void setImage(int index, string spriteName) {
		images [index].setImage (spriteName);
	}
}
