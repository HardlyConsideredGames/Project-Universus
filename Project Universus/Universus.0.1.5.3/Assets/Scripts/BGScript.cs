using UnityEngine;
using System.Collections;

public class BGScript : MonoBehaviour {

			Vector3 						myPosition;
			Vector3 						screenPos;
			RectTransform 					rt;
	public GameObject 						gameCamera;
			Camera 							camera;
			float							width;
			float 							height;
			float							myRatio;
	private BackgroundCollisions 			mainScript;




	void Start () {
		mainScript = this.gameObject.transform.root.GetComponent<BackgroundCollisions> ();
		myPosition = this.gameObject.transform.position;


		rt = GetComponent<RectTransform> ();						//Accessing the RectTransform of the object, used to modify width and height values
		camera = gameCamera.GetComponent<Camera> ();				//Getting game Camera
		screenPos = camera.WorldToScreenPoint (myPosition);			//Declaring objects position on screen based off its transform in the scene


	//	height = Screen.height;
		height = 490;

	
	}
	

	void Update () {

		Identify ();

		width = 1076 * myRatio;				//Setting the objects width as the percent of screen it should cover
		rt.sizeDelta = new Vector2 (width,height);
	
	}

	void Identify() {
		
		if (this.gameObject.tag == ("BG1")) {
			
			myRatio = mainScript.healthRatio1;
		} else if (this.gameObject.tag == ("BG2")) {
			
			myRatio = mainScript.healthRatio2;
			
		} else if (this.gameObject.tag == ("BG3")) {
			
			myRatio = mainScript.healthRatio3;
			
		} else if (this.gameObject.tag == ("BG4")) {

			myRatio = mainScript.healthRatio4;
		}
		
	}
}
