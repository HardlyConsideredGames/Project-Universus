using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour {
	
	public	bool					isColliding		= false;
	public 	GameObject 				player;
	public	PlayerController		playerController;
	public	LayerMask				collisionsMask;
			CollisionsController	collisions;
			Vector2					velocity;
	
	void Start () {
		collisions 	= GetComponent<CollisionsController> ();
	}
	
	void Update () {
		IsColliding ();
	}
	
	void IsColliding () {
		velocity.x  = playerController.direction * playerController.moveSpeed * Time.deltaTime;
		collisions.HitBox (velocity, collisionsMask);
		if (collisions.collisionsInfo.left || collisions.collisionsInfo.right) {
			isColliding = true;
		} else {
			isColliding = false;
		}
		
	}
}