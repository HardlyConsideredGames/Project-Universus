using UnityEngine;
using System.Collections;

public class TransPlacer1 : MonoBehaviour {

	RectTransform 					rt;
	private BackgroundCollisions 			mainScript;
	float width;

	void Start() {

		mainScript = this.gameObject.transform.root.GetComponent<BackgroundCollisions> ();
		rt = GetComponent<RectTransform> ();

	}

	void Update () {

		if (GameObject.FindGameObjectWithTag ("Player1") != null) {

			width = 1076 * mainScript.healthRatio1;
			rt.anchoredPosition = new Vector3 (width, 0, 0);

		} else
			rt.anchoredPosition = new Vector3 (0, 0, 0);
	}
}