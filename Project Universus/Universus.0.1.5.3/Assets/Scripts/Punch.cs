using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CollisionsController))]
public class Punch : MonoBehaviour {

	public	bool					isColliding			= 	false;
	public 	GameObject 				player;
	public	PlayerController		playerController;
	public 	PlayerController		player1Controller;
	public 	PlayerController		player2Controller;
	public 	PlayerController		player3Controller;
	public 	PlayerController		player4Controller;
	public 	LayerMask 				player1CMask;
	public 	LayerMask 				player2CMask;
	public 	LayerMask 				player3CMask;
	public 	LayerMask 				player4CMask;
			CollisionsController	collisions;
			Vector2					velocity;
	
	void Start () {
		collisions 	= GetComponent<CollisionsController> ();
	}
	
	void Update () {
		Vanquish ();
		Punching ();
	}
	
	void IsColliding (LayerMask cMask) {
		velocity.x  = playerController.direction * playerController.moveSpeed * Time.deltaTime;
		collisions.HitBox (velocity, cMask);
		if (collisions.collisionsInfo.left || collisions.collisionsInfo.right) {
			isColliding = true;
		} else {
			isColliding = false;
		}
	}
	
	void Vanquish () {
		if (Input.GetKeyDown (playerController.punch) || Input.GetKeyDown(KeyCode.S)) { //Replace S with crounching state
			IsColliding (player1CMask);
			if (isColliding && player1Controller.isStaggered == true) {
				player1Controller.DmgIntake(10);
				playerController.HpIntake(10);			
			}
			IsColliding (player2CMask);
			if (isColliding && player2Controller.isStaggered == true) {
				player2Controller.DmgIntake(10);
				playerController.HpIntake(10);	
			}
			IsColliding (player3CMask);
			if (isColliding && player3Controller.isStaggered == true) {
				player3Controller.DmgIntake(10);
				playerController.HpIntake(10);	
			}
			IsColliding (player4CMask);
			if (isColliding && player4Controller.isStaggered == true) {
				player4Controller.DmgIntake(10);
				playerController.HpIntake(10);	
			}
		}
	}
	void Punching () {
		if (Input.GetKeyDown (playerController.punch)) {
			IsColliding (player1CMask);
			if (isColliding) {
				player1Controller.staminaDmg(25);
			}
			IsColliding (player2CMask);
			if (isColliding) {
				player2Controller.staminaDmg(25);
			}
			IsColliding (player3CMask);
			if (isColliding) {
				player3Controller.staminaDmg(25);
			}
			IsColliding (player4CMask);
			if (isColliding) {
				player4Controller.staminaDmg(25);
			}
		}
	}
}