using UnityEngine;
using System.Collections;

public class Animate : MonoBehaviour 
{
	public 	float 		idleTime 	= 0.0f;				// Time to start Idle animation
	
	private float 		exTime		= 0.0f;				// Tiem to shoot animation
	private Shooting 	scriptShooting;
	
	
	//========================================================================================================
	void Start () 
	{	
		exTime = Time.time + idleTime;
		scriptShooting = gameObject.GetComponent<Shooting>();
	}
	
	//---------------------------------------------------------------------------------------------------------		
	void Update ()
	{
		// Switch between Shoot and Idle animations
		if(!scriptShooting.weapon[scriptShooting.activeWeapon].model.animation.IsPlaying("Shoot"))
			if (Time.time > exTime)
			{
				scriptShooting.weapon[scriptShooting.activeWeapon].model.animation.Play("Idle");
				exTime = Time.time + idleTime;
			}
	}
	//---------------------------------------------------------------------------------------------------------	
}
