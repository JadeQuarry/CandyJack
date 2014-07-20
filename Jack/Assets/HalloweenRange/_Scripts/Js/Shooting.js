//Shooting	
   class CWeapon
	{
	 
	 var model: GameObject;     	// Weapon 3D model
	 var damage: float = 1; 		// Damage to enemy
	 var fireRate: float = 0.5; 	// Frequency of bullet generation
	 var ammo: int = 10 ;      		// Ammo quantity
	 var type: int = 0;				// Type of weapon. 0 - bullets, 1 - fire, 3 - grenades
	 var enabled: boolean = false;  
	 var WeaponKey: KeyCode;
	 var sound: AudioClip;
	}

var weapon: CWeapon[];
var activeWeapon: int = 0;
var grenade : Rigidbody;
var stone : Rigidbody;

var bulletTrace : Rigidbody;					// The clonned object 
var traceSpeed = 10.0;							// Initial speed of bullet
var fireKey: KeyCode = KeyCode.Mouse0; 			// Button to fire
var decal: Rigidbody[];							// Decal to show according to material


private var nextFire = 0.0;
private var questScript: QuestScript;
	
	
//=======================================================================================================

function Start () {
		
		questScript = GameObject.Find("Quest").GetComponent(QuestScript); 
		
		
		Screen.lockCursor = true;
		Screen.showCursor = false;

	// Load saved data	
	 for ( var w = 1; w < (weapon.Length); w ++)
	   {
		 if (PlayerPrefs.HasKey ("HalloweenRange_weapon" + w.ToString()))   weapon[w].enabled = true;
		 if (PlayerPrefs.HasKey ("HalloweenRange_weapon" + w.ToString()+"_ammo"))  weapon[w].ammo = PlayerPrefs.GetInt("HalloweenRange_weapon" + w.ToString()+"_ammo");
		 
		 weapon[w].model.SetActive (false);
	   }

  		weapon[activeWeapon].enabled = true;
		weapon[activeWeapon].model.SetActive (true);
		BroadcastMessage ("ChangeWeapon", activeWeapon); 
		
}

//---------------------------------------------------------------------------------------------------------	
	
function Fire () {

// Create new bullet with bulletSpeed to forward direction
   
  if (weapon[activeWeapon].type == 0) 
  {
    var hitInfo : RaycastHit;
    mainCamera = Camera.main;
    
    // Raycast object
    var ray = mainCamera.ScreenPointToRay ( Vector3(mainCamera.pixelWidth/2,mainCamera.pixelHeight/2,0));		
  
      if (Physics.Raycast (ray, hitInfo, Mathf.Infinity)) 
            {
             var material = hitInfo.collider.gameObject.GetComponent(MaterialType);
            
             if (material && material.materialIndex < decal.length)
              {	
    		 	var ShootClone : Rigidbody = Instantiate(decal[material.materialIndex], hitInfo.point, transform.rotation);
    			ShootClone.transform.rotation = Quaternion.FromToRotation (Vector3.up,hitInfo.normal);
    			ShootClone.velocity = transform.forward * traceSpeed;
    		//	if(!ShootClone.gameObject.active) ShootClone.gameObject.SetActiveRecursively(true);
    		  	ShootClone.gameObject.SetActive(true);
    		  }
    		}

    // Create random trace	
    if (Random.Range(-10, 10) > 0)
      {
       var TraceClone : Rigidbody = Instantiate(bulletTrace, transform.position+transform.forward, transform.rotation);
       TraceClone.velocity = transform.forward * traceSpeed;
//       if(!TraceClone.gameObject.active) TraceClone.gameObject.SetActiveRecursively(true);
      	TraceClone.gameObject.SetActive(true);
      }
      
      BroadcastMessage ("GenerateMuff");
   }
   
   
   // Instantiate throwable object-bullet
	  if (weapon[activeWeapon].type == 3) 
	  {
	       var grenadeInstance : Rigidbody = Instantiate(grenade, transform.position-transform.forward*1.2 , transform.rotation);
	       grenadeInstance.velocity = transform.forward * 15 + transform.up*2;
//	       if(!grenadeInstance.gameObject.active) grenadeInstance.gameObject.SetActiveRecursively(true);
			grenadeInstance.gameObject.SetActive(true);
	  }
	 
	  
	  if (weapon[activeWeapon].type == 4) 
	  {
	       var stoneInstance : Rigidbody = Instantiate(stone, transform.position-transform.forward*1.2 , transform.rotation);
	       stoneInstance.velocity = transform.forward * 15 + transform.up*2;
//	       if(!stoneInstance.gameObject.active) stoneInstance.gameObject.SetActiveRecursively(true);
			stoneInstance.gameObject.SetActive(true);
		
	  } 
}


//---------------------------------------------------------------------------------------------------------	

function Update () {

		
 if (Screen.showCursor == false)
 { 
  // Change weapon by pressing button
   for ( var i = 0; i < weapon.length; i++) 
   	  {
         if (Input.GetKey(weapon[i].WeaponKey)) 
            if (weapon[i].ammo > 0 && weapon[i].enabled) ChangeWeapon (i);
       }
           
   // Change weapon to previous if no ammo    
      if (weapon[activeWeapon].ammo == 0 )     
	       for ( i = activeWeapon; i >= 0; i--) 
	           		 if (weapon[i].ammo > 0 )
	           				   {
	           				    ChangeWeapon (i);
	           				    i = -1;
	           				   }
    
  // Show muzzle
     if ((Input.GetKey(fireKey) && Time.time > (nextFire-weapon[activeWeapon].fireRate+0.1)) || Input.GetKeyUp(fireKey))
		  BroadcastMessage ("SetVisibility", false, SendMessageOptions.DontRequireReceiver);

  // Fire - instantiate bullet, decrease ammo, play sound and animation
	   if ( Input.GetKey(fireKey) && Time.time > nextFire) 
			{
				
				nextFire = Time.time + weapon[activeWeapon].fireRate;
				
				if ( weapon[activeWeapon].ammo > 0 && weapon[activeWeapon].enabled)
				   {
				    weapon[activeWeapon].model.animation.Play("Shoot");
					weapon[activeWeapon].model.BroadcastMessage ("SetVisibility", true, SendMessageOptions.DontRequireReceiver);
					
					Fire ();
					
					audio.clip = weapon[activeWeapon].sound;
					audio.Play();
					
					weapon[activeWeapon].ammo--;
					questScript.SetUsedAmmo(questScript.GetUsedAmmo() + 1);
				   }
			  
			 }
	         
   }
    
} 

//---------------------------------------------------------------------------------------------------------	

function ChangeWeapon ( weaponIndex: int) 
{
   weapon[activeWeapon].model.SetActive (false);
   activeWeapon = weaponIndex;
   weapon[activeWeapon].model.SetActive (true);
}

//---------------------------------------------------------------------------------------------------------	