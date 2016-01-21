using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundController : MonoBehaviour {
	
	public 	   	Image 				background1;
	public 	   	Image 				background2;
	public 	   	Image 				background3;
	public		PlayerController	player1;
	public		PlayerController	player2;
	public		PlayerController	player3;
	public		PlayerController	player4;
	private		float				healthRatio1;
	private		float				healthRatio2;
	private		float				healthRatio3;


	void Update () {
		backgroundUpdate ();
	}	
	
	public void backgroundUpdate () {
		healthRatio3 = (1 - (player4.currHp / (player1.currHp + player2.currHp + player3.currHp + player4.currHp)));
		healthRatio2 = (healthRatio3 - (player3.currHp / (player1.currHp + player2.currHp + player3.currHp + player4.currHp))); 
		healthRatio1 = player1.currHp / (player1.currHp + player2.currHp + player3.currHp + player4.currHp);
		background1.fillAmount 	= healthRatio1;
		background2.fillAmount 	= healthRatio2;
		background3.fillAmount 	= healthRatio3;
	}	
}