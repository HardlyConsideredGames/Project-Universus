using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collisions))]
public class PlayerController : MonoBehaviour {
	Collisions collisions;
	Vector2 velocity;
	private		float 					accelerationTimeAirbourne 	= 	0.2f;
	private		float 					acceleratinTimeGrounded 	=	0.1f;
	private		float 					velocityXSmoothing; 
	private 	float					targetVelocityX;

	void Start () {
		collisions = GetComponent<Collisions> ();
	}
	
	void Update () {
		Move();
		Debug.Log("Left: "+collisions.collisionInfo.left+"  Right: "+collisions.collisionInfo.right);
	}

	void Move(){
		collisions.Collision();
		if (Input.GetKey (KeyCode.A)){
			targetVelocityX = -5;
		} else if (Input.GetKey (KeyCode.D)){
			targetVelocityX = 5;
		} else {
			targetVelocityX = 0;
		}
		if (Input.GetKey (KeyCode.W)){
			velocity.y = 5;
		} else if (Input.GetKey (KeyCode.S)){
			velocity.y = -5;
		} else {
			velocity.y = 0;
		}
		velocity.x  = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisions.collisionInfo.below)?acceleratinTimeGrounded:accelerationTimeAirbourne);
		transform.Translate(velocity * Time.deltaTime);
	}
}
