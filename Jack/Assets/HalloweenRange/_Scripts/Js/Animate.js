//Animate
var idleTime: float = 0.0f;				// Time to start Idle animation

private var exTime: float = 0.0f;
private var scriptShooting: Shooting;


//========================================================================================================
function Start () {
  
  exTime = Time.time + idleTime;
  scriptShooting = gameObject.GetComponent("Shooting");
  
}

//---------------------------------------------------------------------------------------------------------		
function Update () {

 // Switch between Shoot and Idle animations
  if(!scriptShooting.weapon[scriptShooting.activeWeapon].model.animation.IsPlaying("Shoot"))
   if (Time.time > exTime)
	{
	  scriptShooting.weapon[scriptShooting.activeWeapon].model.animation.Play("Idle");
	  exTime = Time.time + idleTime;
	}
}
//---------------------------------------------------------------------------------------------------------		