using UnityEngine;
using System.Collections;

public class TransPlacer3 : MonoBehaviour {

	RectTransform 					rt;
	private BackgroundCollisions 			mainScript;
	float width;
	float width1;
	float width2;
	
	void Start() {
		
		mainScript = this.gameObject.transform.root.GetComponent<BackgroundCollisions> ();
		rt = GetComponent<RectTransform> ();
		
	}
	
	void Update () {
		
		width = 1076 * mainScript.healthRatio1;
		width1 = 1076 * mainScript.healthRatio2;
		width2 = 1076 * mainScript.healthRatio3;
		
		if (GameObject.FindGameObjectWithTag ("Player3") != null) {
			rt.anchoredPosition = new Vector3 ((width + width1 + width2), 0, 0);
		} 
		else if(GameObject.FindGameObjectWithTag("Player3") == null && GameObject.FindGameObjectWithTag("Player2") != null) {
			rt.anchoredPosition = new Vector3 (width + width1, 0, 0);
		}
		else if(GameObject.FindGameObjectWithTag("Player3") == null && GameObject.FindGameObjectWithTag("Player2") == null) {
			rt.anchoredPosition = new Vector3 (width, 0, 0);
		}
	}
}
