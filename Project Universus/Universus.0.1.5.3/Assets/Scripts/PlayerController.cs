using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CollisionsController))]
public class PlayerController : MonoBehaviour {
	//TODO comment code
	private		Animator 				anim;
	public		Vector2					wallJump;
	public		bool					doInput;
	private 	bool					wallSliding					=	false;
	private		bool					grabbedLedge				=	false;
	private		bool					facingRight					=   true;
	private		float 					accelerationTimeAirbourne 	= 	0.2f;
	private		float 					acceleratinTimeGrounded 	=	0.1f;
	private		float 					gravity;
	private		float 					maxJumpVelocity;
	private		float					minJumpVelocity;
	private		float 					velocityXSmoothing; 
	private 	float					targetVelocityX;
	private 	int 					playerState;
	public		float					moveSpeed					=	5f;
	public 		float 					timeToJumpApex 				= 	0.4f;
	public 		float 					maxJumpHeight 				= 	4f;
	public		float					minJumpHeight				=	1f;
	public		float					wallSlideSpeedMax			= 	3f;
	public		float					direction					=	1;
	public 		float 					maxHp						= 	100f;
	public 		float 					currHp;
	public		float					maxStamina					=   100f;
	public		float					currStamina;
	public		float					staminaCooldown;
	public		bool					isStaggered					= false;
	public		Vector2 				velocity;
	public		GameObject				player;
	public		PlatformInteractions	platInt;
	public		LayerMask				collisionMask;
	public		string					moveLeft;
	public		string					moveRight;
	public		string					jump;
	public		string					punch;
				CollisionsController 	collisions;
				HitBox					wallInteractions;
	
	void Start () {
		anim 			 = GetComponent<Animator> ();
		collisions 	 	 = GetComponent<CollisionsController> ();
		wallInteractions = GetComponent<HitBox> ();
		currHp 			 = maxHp;
		currStamina 	 = maxStamina;
		gravity 	 	 = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity  = Mathf.Abs (gravity) * timeToJumpApex; 	
		minJumpVelocity  = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
		doInput = true;
	}
	
	void AnimationController () {
		if (isStaggered == false) {
			if (!collisions.collisionsInfo.below && !(collisions.collisionsInfo.left || collisions.collisionsInfo.right)) {     
				playerState = 2;
			}
			if (!collisions.collisionsInfo.below && (collisions.collisionsInfo.left || collisions.collisionsInfo.right) && (grabbedLedge || wallSliding)) {
				playerState = 3;
			}
			if (Input.GetKey (punch)) {
				playerState = 4;
			}
			if (playerState == 1) {                                       // if player is moving display moving sprite
				anim.Play ("Running_1");
			} else if (playerState == 2) {                                 // if player is in air display falling sprite                                             
				anim.Play ("Jumping");	
			} else if (playerState == 3) {                                // if player is sliding on wall display sliding sprite
				anim.Play ("Hanging");
			} else if (playerState == 4) {
				anim.Play ("Punching");
			} else {                                                      // if player is idle display idle sprite
				anim.Play ("Idle");     
			}
		} else if (isStaggered == true) {
				anim.Play ("Staggered");
		}
	}
	
	void Update(){
		AnimationController ();                                       // handles sprite changes 
		Die ();                                                       // if players health is zero or less destroys player
		Stamina ();

		if(staminaCooldown >= 0) {
			staminaCooldown -= 1 * Time.deltaTime;
			}

		if (staminaCooldown <= 0) {
			staminaCooldown = 0;
			currStamina = 100f;
			if(isStaggered = true) {
				isStaggered = false;
			}
		}

		if (doInput == true || doInput == false) {
			Move ();												// handles all player movement
		}
	}
	void Move () {
		print (currStamina);
		wallSliding  = false;
		grabbedLedge = false;

		if (platInt.isCollidingWithLedge && !collisions.collisionsInfo.below) {

			if((Input.GetKey (moveLeft) || Input.GetKey (moveRight))) {
			velocity.y 	 = 0;
			velocity.x 	 = 0;
			grabbedLedge = true;
			}
			else if(!collisions.collisionsInfo.below && grabbedLedge == false && wallSliding == false) {

				velocity.y += gravity * Time.deltaTime;
			}
		}
		if (!collisions.collisionsInfo.below && velocity.y < 0) {                                                                                   // if not grounded and falling
			if (!grabbedLedge && (collisions.collisionsInfo.left && Input.GetKey (moveLeft)) || (collisions.collisionsInfo.right && Input.GetKey (moveRight))) {     // if colliding to the left while left key is down or colliding to the right while right key is down
				wallSliding = true;
				if (velocity.y < -wallSlideSpeedMax) {                                                                                              // if falling faster than slide speed max
					velocity.y = -wallSlideSpeedMax;                                                                                                // sets speed falling to slide speed max
				}
			}
		}
		if (collisions.collisionsInfo.below || collisions.collisionsInfo.above) {                                                                   // if grounded or hitting wall overhead
			velocity.y = 0;                                                                                                                         // velocity in the y axis stops
		}
		if (Input.GetKey (moveLeft) && doInput == true) { 
			if(facingRight == true) {
				Flip ();
			}
			playerState = 1;
			targetVelocityX = -moveSpeed;
			direction = -1;
		} else if (Input.GetKey (moveRight) && doInput == true) {
			if(facingRight == false) {
				Flip();
			}
			playerState = 1;
			targetVelocityX = moveSpeed;
			direction = 1;
		} else {
			playerState = 0;
			targetVelocityX = 0;
		}
		if ((Input.GetKeyDown (jump))) {
			if (!collisions.collisionsInfo.below) {
				if (grabbedLedge) {
					if (!collisions.collisionsInfo.below) {
						velocity.y	 = 12f;
						if (direction == 1) { 
							velocity.x	 = 15;
						} else if (direction == -1) {
							velocity.x	 = -15;
						}
					}
					grabbedLedge = false; 
				} else if (!grabbedLedge) {                           
					if (collisions.collisionsInfo.left) {                                                                                               		
						velocity.x 	= wallJump.x;
						velocity.y 	= wallJump.y;
						wallSliding = false;
						doInput = false;
						StartCoroutine("inputReset",(0.26f));
					} else if (collisions.collisionsInfo.right){
						velocity.x 	= -wallJump.x;
						velocity.y 	= wallJump.y;
						wallSliding = false;
						doInput = false;
						StartCoroutine("inputReset",(0.26f));
					}
				}
			}
			if (collisions.collisionsInfo.below){
				velocity.y = maxJumpVelocity;
			}
		}
		if (Input.GetKeyUp (jump)) {
			if (velocity.y > minJumpVelocity) {
				velocity.y = minJumpVelocity;
			}
		}
		if (!platInt.isCollidingWithLedge) {
			velocity.y += gravity * Time.deltaTime;
		}
		velocity.x  = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisions.collisionsInfo.below)?acceleratinTimeGrounded:accelerationTimeAirbourne);
		collisions.Move(velocity * Time.deltaTime, collisionMask);
	}
	
	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		
	}
	
	public void DmgIntake (float dmg) {
		if (currHp > 0) {
			currHp -= dmg;
		}
	}

	public void staminaDmg (float Sdmg) {
		if (currStamina > 0) {
			currStamina -= Sdmg;
			staminaCooldown = 2f;
		}
	}

	public void HpIntake (float hp) {
		if (currHp > 0) {
			currHp += hp;
		}
	}

	void Stamina () {
		if (currStamina <= 0) {
			isStaggered = true;
			playerState = 5;
			doInput = false;
			StartCoroutine("inputReset",(1f));
		}

	}
	
	void Die () {
		if (currHp <= 0) {
			Destroy (player);
		}
	}

	IEnumerator inputReset(float resetTimer) {
		yield return new WaitForSeconds (resetTimer);
		doInput = true;
	}
}