using UnityEngine;
using System.Collections;

public class BG_wall : MonoBehaviour {

	Vector3 						myPosition;
	Vector3 						screenPos;
	RectTransform 					rt;
	public GameObject 				gameCamera;
	GameObject						myPlayer;
	Camera 							camera;
	float							width;
	float 							height;
	
	
	
	
	void Start () {
		myPosition = this.gameObject.transform.position;
		
		
		rt = GetComponent<RectTransform> ();						//Accessing the RectTransform of the object, used to modify width and height values
		camera = gameCamera.GetComponent<Camera> ();				//Getting game Camera
		screenPos = camera.WorldToScreenPoint (myPosition);			//Declaring objects position on screen based off its transform in the scene
		height = 500;

		if (transform.parent.tag == "BG2") {
			myPlayer = GameObject.FindGameObjectWithTag ("Player2");
		} else if (transform.parent.tag == "BG3") {
			myPlayer = GameObject.FindGameObjectWithTag("Player3");
		}
		else if (transform.parent.tag == "BG4") {
			myPlayer = GameObject.FindGameObjectWithTag("Player4");
		}
		
		
	}
	
	
	void Update () {

		
		width = 20;				//whatever looks good...
		rt.sizeDelta = new Vector2 (width,height);

		if (myPlayer == null) {

			Destroy (gameObject);
		} 
		
	}
	

}
