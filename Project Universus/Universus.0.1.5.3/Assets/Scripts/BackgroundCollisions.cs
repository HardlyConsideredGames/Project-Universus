using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundCollisions : MonoBehaviour {
	
	 	   		GameObject 			background1;
	 	   		GameObject 			background2;
	 	   		GameObject 			background3;
				GameObject			background4;
	public	   	PlayerController 	player1;
	public	   	PlayerController 	player2;
	public	 	PlayerController	player3;
	public 		PlayerController 	player4;
	public		float				healthRatio1;
	public		float				healthRatio2;
	public		float				healthRatio3;
	public		float				healthRatio4;
	private		float				totalHealth;



	void Start() {

		background1 = this.gameObject.transform.GetChild (0).GetChild (0).gameObject;
		background2 = this.gameObject.transform.GetChild (1).GetChild (0).gameObject;
		background3 = this.gameObject.transform.GetChild (2).GetChild (0).gameObject;
		//background4 = this.gameObject.transform.GetChild (3).GetChild (0).gameObject;
		//totalHealth = 400;

	}


	void Update () {
		totalHealth = (player1.currHp + player2.currHp + player3.currHp + player4.currHp);
		backgroundUpdate ();

	}	
	
	public void backgroundUpdate () {
		healthRatio1 = (player1.currHp/totalHealth);
		healthRatio2 = (player2.currHp/totalHealth);
		healthRatio3 = (player3.currHp/totalHealth);
		healthRatio4 = (player4.currHp/totalHealth);
	}	
}