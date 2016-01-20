using UnityEngine;
using System.Collections;

public class PlatformInteractions : MonoBehaviour {
	
	public	bool					isCollidingWithWall	 		= false;
	public	bool					isCollidingWithLedge		= false;
	public 	GameObject 				player;
	public	PlayerController		playerController;
	public	LayerMask				wallMask;
	public	LayerMask				ledgeMask;
			CollisionsController	collisions;
			Vector2					velocity;
	
	void Start () {
		collisions 	= GetComponent<CollisionsController> ();
	}
	
	void Update () {
		IsColliding ();
	}
	
	void IsColliding () {
		velocity.x  = playerController.direction * .005f;
		collisions.HitBox (velocity, wallMask);
		if (collisions.collisionsInfo.left || collisions.collisionsInfo.right) {
			isCollidingWithWall = true;
		} else {
			isCollidingWithWall = false;
		}
		collisions.HitBox (velocity, ledgeMask);
		if (collisions.collisionsInfo.left || collisions.collisionsInfo.right) {
			isCollidingWithLedge = true;
		} else {
			isCollidingWithLedge = false;
		}
	}
}