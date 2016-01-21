using UnityEngine;
using System.Collections;

public class TransPlacer2 : MonoBehaviour {

	RectTransform 					rt;
	private BackgroundCollisions 			mainScript;
	float width;
	float width1;
	
	void Start() {
		
		mainScript = this.gameObject.transform.root.GetComponent<BackgroundCollisions> ();
		rt = GetComponent<RectTransform> ();
		
	}
	
	void Update () {

		width = 1076 * mainScript.healthRatio1;
		width1 = 1076 * mainScript.healthRatio2;
		
		if (GameObject.FindGameObjectWithTag ("Player2") != null) {
			rt.anchoredPosition = new Vector3 ((width + width1), 0, 0);
			} 
		if(GameObject.FindGameObjectWithTag("Player2") == null) {
			rt.anchoredPosition = new Vector3 (width, 0, 0);
		}
	}
}