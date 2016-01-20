using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class CollisionsController : MonoBehaviour {

	const		float 				skinWidth 				= 		0.015f;
	const		int					horRayCount 			= 		20;
	const		int					vertRayCount 			= 		20;
	private		float 				jumpCoolDown 			= 		.1f;
	private 	float 				maxDescendAngle 		= 		75f;
	private 	float 				maxClimbAngle 			= 		80f;
	private 	float 				timeStamp;
	private 	float				horRaySpacing;
	private 	float				vertRaySpacing;	
	public		CollisionInfo		collisionsInfo;
				BoxCollider2D 		collider;
				RayCastOrigin		rayCastOrigins;

	void Start () {
		collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
	}

	public void Move(Vector3 velocity, LayerMask cMask){
		UpdateRayCastOrigins ();                                                                                                // calls UpdateRayCastOrigins () method which recreates origins for the rays
		collisionsInfo.Reset ();																						        // calls Reset() method from collisionsInfo which resets all info
		HorizontalCollisions (ref velocity, cMask);																	// calls HorizontalCollisions() method with a reference to velocity which checks collisions in the x axis
		if(velocity.y < 0){                                                                                                     // checks if object is falling
			DescendSlope(ref velocity, cMask);                                                                      // calls DescendSlope() method with a reference to velocity which checks collisions on slopes
		}
		if (velocity.y != 0) {			    																				    // if object is moving in the y axis
			VerticalCollisions (ref velocity, cMask);	    														// calls VerticalCollisions() method with a reference to velocity which checks collisions in the y axis
		}   
		transform.Translate (velocity);																					        // moves object with velocity
	}

	public void HitBox (Vector3 velocity, LayerMask cMask) {
		UpdateRayCastOrigins ();
		collisionsInfo.Reset ();
		HorizontalCollisions (ref velocity, cMask);
	}

	 void HorizontalCollisions (ref Vector3 velocity, LayerMask cMask) {
		float dirX 		= Mathf.Sign (velocity.x);																		        // direction of velocity in the x component
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;															        // length of raycast ray, equal to velocity in the x component
		for (int i = 0; i < horRayCount; i++) { 
			Vector2 	 rayOrigin	 = (dirX == -1)?rayCastOrigins.botLeft:rayCastOrigins.botRight;						        // checks direction of velocity in the x component and assigns an origin respectively
			rayOrigin 				+= Vector2.up * (horRaySpacing * i);												        // adds origin for each ray along boundry 
			RaycastHit2D hit		 = Physics2D.Raycast (rayOrigin, Vector2.right * dirX, rayLength, cMask);	        		// creates ray at each ray origin in the direction of velocity with the length of velocity while ignoring objects not in the layer mask collisionMask
			Debug.DrawRay(rayOrigin, Vector2.right * dirX * rayLength, Color.red);
			if (hit) {																									        // if ray collides with object in layer mask collisionMask
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if (i == 0 && slopeAngle <= maxClimbAngle){
					float distanceToSlopeStart = 0;                                                                             // temp variable for distance to slope
					if (slopeAngle != collisionsInfo.slopeAngleOld) {                                                           // checks change in slope
						distanceToSlopeStart = hit.distance - skinWidth;                                                        // assigns distance minus skin width to temp variable
						velocity.x 			-= distanceToSlopeStart * dirX;                                                     // subtracts temp value from velocity in the x axis with respect to direction
					}
					ClimbSlope (ref velocity, slopeAngle);                                                                      // calls ClimbSlope () method which checks collisions on slopes
					velocity.x += distanceToSlopeStart * dirX;                                                                  // adds temp value to velocity in the x axis with respect to direction
				}
				if (!collisionsInfo.climbingSlope || slopeAngle > maxClimbAngle){
				velocity.x  = (hit.distance - skinWidth) * dirX;												                // if hit assigns distance from boundry to object collided to velocity in the x component
				rayLength  	= hit.distance;																	                    // assigns distance from boundry to object collided to rayLegth, as to not get confused if not all rays are colliding with same object distance
				if (collisionsInfo.climbingSlope){
					velocity.y = Mathf.Tan (collisionsInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);                 // Trig! 
				}
				collisionsInfo.left  = dirX == -1;                                                                      
				collisionsInfo.right = dirX == 1;
				} 
			}		
		}
	}

	void VerticalCollisions(ref Vector3 velocity, LayerMask cMask){
		float dirY 		= Mathf.Sign (velocity.y);																		        // direction of velocity in the y component
		float rayLength = Mathf.Abs(velocity.y) + skinWidth;															        // length of raycast ray, equal to velocity in the y component
		for (int i = 0; i < vertRayCount; i++) {
			Vector2 	 rayOrigin = (dirY == -1)?rayCastOrigins.botLeft:rayCastOrigins.topLeft;						        // checks direction of velocity in the y component and assigns an origin respectively 
			rayOrigin 	     	  += Vector2.right * (vertRaySpacing * i + velocity.x);									        // adds origin for each ray along boundry 
			RaycastHit2D hit 	   = Physics2D.Raycast(rayOrigin,Vector2.up * dirY,rayLength, cMask);			       		    // creates ray at each ray origin in the direction of velocity with the length of velocity while ignoring objects not in the layer mask collisionMask
			Debug.DrawRay(rayOrigin, Vector2.up * dirY * rayLength,Color.red);
			if (hit) {																									        // if ray collides with object in layer mask collisionMask
				velocity.y 	= (hit.distance - skinWidth) * dirY;												                // if hit assigns distance from boundry to object collided to velocity in the y component
				rayLength   = hit.distance;																	                    // assigns distance from boundry to object collided to rayLegth, as to not get confused if not all rays are colliding with same object distance
				if (collisionsInfo.climbingSlope){
					velocity.x = velocity.y / Mathf.Tan (collisionsInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);   // more Trig!
				}
				collisionsInfo.below = dirY == -1;
				collisionsInfo.above = dirY == 1;
				if( timeStamp <= Time.time) {                                                                                   
					timeStamp = Time.time + jumpCoolDown;
				}
			}
		}
		if (collisionsInfo.climbingSlope){
			float directionX    = Mathf.Sign (velocity.x);
			rayLength           = Mathf.Abs (velocity.x) + skinWidth;
			Vector2 rayOrigin   = ((directionX == -1)?rayCastOrigins.botLeft:rayCastOrigins.botRight) + Vector2.up * velocity.y;
			RaycastHit2D hit    = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, cMask);
			if (hit) {
				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
				if (slopeAngle  != collisionsInfo.slopeAngle){
					velocity.x   = (hit.distance - skinWidth) * directionX;
					collisionsInfo.slopeAngle = slopeAngle;
				}
			}			
		}
	}		

	void ClimbSlope(ref Vector3 velocity, float slopeAngle) {
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
		if (velocity.y <= climbVelocityY){
			velocity.y                      = climbVelocityY;
			velocity.x                      = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
			collisionsInfo.below            = true;
			collisionsInfo.climbingSlope    = true;
			collisionsInfo.slopeAngle       = slopeAngle;
		}
	}
	void DescendSlope (ref Vector3 velocity, LayerMask cMask){
		float directionX = Mathf.Sign (velocity.x);
		Vector2 rayOrigin = (directionX == -1)?rayCastOrigins.botRight:rayCastOrigins.botLeft;
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, cMask);			
		if (hit) {
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle){
				if (Mathf.Sign(hit.normal.x) == directionX){
					if (hit.distance - skinWidth <= Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)){
						float moveDistance = Mathf.Abs (velocity.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
						velocity.y -= descendVelocityY;							
						collisionsInfo.slopeAngle = slopeAngle;
						collisionsInfo.descendingSlope = true;
						collisionsInfo.below = true;
					}
				}
			}
		}
	}
	
	void UpdateRayCastOrigins () {
		Bounds bounds 			= collider.bounds;																		// instatiate boundries of BoxCollider2D
		bounds.Expand (skinWidth * -2);																					// shrinks boundries by skinWidth
		rayCastOrigins.botLeft 	= new Vector2 (bounds.min.x, bounds.min.y);
		rayCastOrigins.botRight	= new Vector2 (bounds.max.x, bounds.min.y);
		rayCastOrigins.topLeft 	= new Vector2 (bounds.min.x, bounds.max.y);
		rayCastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	void CalculateRaySpacing () {
		Bounds bounds 			= collider.bounds;																		// instatiate boundries of BoxCollider2D
		bounds.Expand (skinWidth * -2);																					// shrinks boundries by skinWidth
		horRaySpacing 			= bounds.size.y / (horRayCount - 1);													// calculates distance between rays along horizontal bound
		vertRaySpacing			= bounds.size.x / (vertRayCount - 1);   												// calculates distance between rays along vertical bound
	}	
	
	struct RayCastOrigin {
		public Vector2 topLeft, topRight;																				// top corners of BoxCollider2D
		public Vector2 botLeft, botRight;																				// bottom corners of BoxCOllider2D
	}

	public struct CollisionInfo {
		public bool  below, above;																						// bool to check if collision is happening above or below;
		public bool  left,  right;																						// bool to check if collision is happening right or left
		public bool  climbingSlope;                                                                                     // bool to check if moving up a slope
		public bool  descendingSlope;                                                                                   // bool to check if moving down a slope
		public float maxClimbAngle;                                                                                     // max value angle of a slope can be to climb treated as a wall after this amount
		public float maxDescendAngle;                                                                                   // max value angle of a slope can be to descend treated as a wall after this amount
		public float slopeAngle, slopeAngleOld;                                                                         // variable to hold angle of slope, and old angle of slope from last update

		public void Reset() {																							// resets collision booleans, and assigns slope angle to old slope angle then resets slope angle
			below = above = left = right 	= false;
			climbingSlope = descendingSlope = false;
			slopeAngleOld = slopeAngle;
			slopeAngle 	  					= 0;
		}
	}
}