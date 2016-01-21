using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class Collisions : MonoBehaviour {
	private		BoxCollider2D 		collider;
	private		float				skinWidth = 0.02f;
	private		float				horRaySpacing;
	private		float				vertRaySpacing;
	private		float				rayCount;
	private		float				rayLengthLeft;
	private		float				rayLengthRight;
	private		float				rayLengthTop;
	private		float				rayLengthBot;
	private		RayCastOrigin		rayCastOrigin;
	public		CollisionInfo		collisionInfo;
	public		LayerMask			cMask;
	
	void Start () {
		rayCount = 10;
		collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing();
	}

	public void Collision (){
		UpdateRayCastOrigins();
		collisionInfo.Reset();
		for(int i = 0; i < rayCount; i++){
			Vector2 rayOriginLeft  		 = rayCastOrigin.botLeft;
			Vector2 rayOriginRight 		 = rayCastOrigin.botRight;
			Vector2 rayOriginTop		 = rayCastOrigin.topLeft;
			Vector2 rayOriginBot		 = rayCastOrigin.botLeft;
			rayOriginLeft 		  		+= Vector2.up * (horRaySpacing * i);
			rayOriginRight		  		+= Vector2.up * (horRaySpacing * i);
			rayOriginTop				+= Vector2.right * (horRaySpacing * i);
			rayOriginBot				+= Vector2.right * (horRaySpacing * i);
			RaycastHit2D hitLeft		 = Physics2D.Raycast (rayOriginLeft, Vector2.left, rayLengthLeft, cMask);
			RaycastHit2D hitRight		 = Physics2D.Raycast (rayOriginRight, Vector2.right, rayLengthRight, cMask);
			RaycastHit2D hitTop			 = Physics2D.Raycast (rayOriginTop, Vector2.up, rayLengthTop, cMask);
			RaycastHit2D hitBot			 = Physics2D.Raycast (rayOriginBot, Vector2.down, rayLengthBot, cMask);
			Debug.DrawRay(rayOriginLeft, Vector2.left  * rayLengthLeft, Color.red);
			Debug.DrawRay(rayOriginRight, Vector2.right  * rayLengthRight, Color.red);
			Debug.DrawRay(rayOriginTop, Vector2.up  * rayLengthTop, Color.red);
			Debug.DrawRay(rayOriginBot, Vector2.down  * rayLengthBot, Color.red);
			if(hitLeft){
				collisionInfo.left 	= true;
				rayLengthLeft 		= (hitLeft.distance - skinWidth); 
			}
			if(hitRight){
				collisionInfo.right = true;
				rayLengthRight 		= (hitRight.distance - skinWidth); 
			}
			if(hitTop){
				collisionInfo.above = true;
				rayLengthTop		= (hitTop.distance - skinWidth);
			}
			if(hitBot){
				collisionInfo.below = true;
				rayLengthBot		= (hitBot.distance - skinWidth);
			}
		}
	}

	public void UpdateRayCastOrigins () {
		rayLengthBot = rayLengthLeft = rayLengthRight = rayLengthTop = 2;
		Bounds bounds  = collider.bounds;
		bounds.Expand(skinWidth * -2);
		rayCastOrigin.botLeft 	= new Vector2 (bounds.min.x, bounds.min.y);
		rayCastOrigin.botRight	= new Vector2 (bounds.max.x, bounds.min.y);
		rayCastOrigin.topLeft 	= new Vector2 (bounds.min.x, bounds.max.y);
		rayCastOrigin.topRight  = new Vector2 (bounds.max.x, bounds.max.y);
	}

	public void CalculateRaySpacing (){
		Bounds bounds  = collider.bounds;
		bounds.Expand(skinWidth * -2);
		horRaySpacing  = bounds.size.x / (rayCount - 1);
		vertRaySpacing = bounds.size.y / (rayCount -1);
	}

	private struct RayCastOrigin {
		public Vector2 botLeft, botRight;
		public Vector2 topLeft, topRight;
	}

	public struct CollisionInfo {
		public bool below, above;
		public bool left, right;
		public void Reset(){
			below = above = left = right = false;
		}
	}
}
