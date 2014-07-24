using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour 
{
	[System.Serializable]
	public class CWeapon
	{
		public GameObject model;     		// Weapon 3D model
		public float damage = 1; 			// Damage to enemy
		public float fireRate = 0.5f; 		// Frequency of bullet generation
		public int ammo = 10 ;      		// Ammo quantity
		public int type = 0;				// Type of weapon. 0 - bullets, 1 - fire, 3 - grenades
		public bool  enabled = false;  
		public KeyCode WeaponKey;
		public AudioClip sound;
	}

	public CWeapon[] weapon;
	public int activeWeapon = 0;
	public Rigidbody grenade;
	public Rigidbody stone;
	
	public Rigidbody bulletTrace;					// The clonned object 
	public float traceSpeed = 10.0f;				// Initial speed of bullet
	public KeyCode fireKey = KeyCode.Mouse0; 		// Button to fire
	public Rigidbody[] decal;						// c to show according to material
	public float decalscaleRate = .5f;
	
	
	private float nextFire = 0.0f;
	private QuestScript questScript;
	private static RaycastHit hitInfo;
	void Awake()
	{
		questScript = GameObject.Find("Quest").GetComponent<QuestScript>(); 
	}
	//=======================================================================================================
	void Start () 
	{	

		
		Screen.lockCursor = true;
		Screen.showCursor = false;
		
		// Load saved data	
		for ( int w = 1; w < (weapon.Length); w ++)
		{
			//Returns true if key exists in the preferences.
			if (PlayerPrefs.HasKey ("HalloweenRange_weapon" + w.ToString()))   
				weapon[w].enabled = true;
			if (PlayerPrefs.HasKey ("HalloweenRange_weapon" + w.ToString()+"_ammo"))  
				weapon[w].ammo = PlayerPrefs.GetInt("HalloweenRange_weapon" + w.ToString()+"_ammo");
			
			weapon[w].model.SetActive (false);
		}
		
		weapon[activeWeapon].enabled = true;
		weapon[activeWeapon].model.SetActive (true);
		BroadcastMessage ("ChangeWeapon", activeWeapon); 
		
	}
	
	//---------------------------------------------------------------------------------------------------------	
	void Fire () 
	{	
		// Create new bullet with bulletSpeed to forward direction
		if (weapon[activeWeapon].type == 0) 
		{
	//		RaycastHit hitInfo;
			Camera mainCamera = Camera.main;

			Ray ray = mainCamera.ScreenPointToRay (new Vector3(mainCamera.pixelWidth/2,mainCamera.pixelHeight/2,0));		
			// Raycast object
			if (Physics.Raycast (ray,out hitInfo, Mathf.Infinity)) 
			{
				MaterialType material = hitInfo.collider.gameObject.GetComponent<MaterialType>();
				if(material == null)
				{
					Debug.LogWarning("Should have material in targat object");
					return;
				}
				if(material.materialIndex != -1)
				{	
					ProjectingDecal();
					EjectShellParticleFire();
				}else
				{
					EjectShellParticleFire();
				}
			}
			
			// Create random trace	
			if (Random.Range(-10, 10) > 0)
			{
				Rigidbody TraceClone = Instantiate(bulletTrace, transform.position+transform.forward, transform.rotation)as Rigidbody;
				TraceClone.velocity = transform.forward * traceSpeed;
				if(!TraceClone.gameObject.active) 
					TraceClone.gameObject.SetActiveRecursively(true);
				Debug.Log("Create Trace");
			}

			BroadcastMessage ("GenerateMuff");
		}
		
		
		// Instantiate throwable object-bullet
		if (weapon[activeWeapon].type == 3) 
		{
			Rigidbody grenadeInstance = Instantiate(grenade, transform.position-transform.forward*1.2f , transform.rotation)as Rigidbody;
			grenadeInstance.velocity = transform.forward * 15 + transform.up*2;
			if(!grenadeInstance.gameObject.active) 
				grenadeInstance.gameObject.SetActiveRecursively(true);
	//		grenadeInstance.gameObject.SetActiveRecursively(true);
			
		}
		
		
		if (weapon[activeWeapon].type == 4) 
		{
			Rigidbody stoneInstance = Instantiate(stone, transform.position-transform.forward*1.2f , transform.rotation)as Rigidbody;
			stoneInstance.velocity = transform.forward * 15 + transform.up*2;
			if(!stoneInstance.gameObject.active) 
				stoneInstance.gameObject.SetActiveRecursively(true);
			stoneInstance.gameObject.SetActiveRecursively(true);
		} 
	}
	
	
	//---------------------------------------------------------------------------------------------------------	

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			Screen.showCursor = !Screen.showCursor;
		}

		if (Screen.showCursor == false)
		{ 
			// Change weapon by pressing button
			int i;
			for (i = 0; i < weapon.Length; i++) 
			{
				if (Input.GetKey(weapon[i].WeaponKey)) 
					if (weapon[i].ammo > 0 && weapon[i].enabled) 
						ChangeWeapon (i);
			}
			
			// Change weapon to previous if no ammo    
			if (weapon[activeWeapon].ammo == 1 )     
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
					questScript.Ammo ++;
//					questScript.SetUsedAmmo((int)questScript.GetUsedAmmo() + 1);
				}
				
			}
			
		}
		
	} 
	
	//---------------------------------------------------------------------------------------------------------	
	
	void ChangeWeapon (int weaponIndex = 0) 
	{
		weapon[activeWeapon].model.SetActiveRecursively (false);
		activeWeapon = weaponIndex;
		weapon[activeWeapon].model.SetActiveRecursively (true);
		Debug.Log ("O");
	}
	
	//---------------------------------------------------------------------------------------------------------	
	void EjectShellParticleFire()
	{
		Rigidbody particleFire = Instantiate(decal[1], hitInfo.point, transform.rotation)as Rigidbody;
		particleFire.transform.rotation = Quaternion.FromToRotation (Vector3.up,hitInfo.normal);
		particleFire.velocity = transform.forward * traceSpeed;
		if(!particleFire.gameObject.active) 
			particleFire.gameObject.SetActiveRecursively(true);
	}

	void ProjectingDecal()
	{
		Rigidbody ShootClone = Instantiate(decal[0], hitInfo.point, transform.rotation)as Rigidbody;
		ShootClone.transform.localScale = new Vector3(decalscaleRate, decalscaleRate, decalscaleRate);
		ShootClone.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
		ShootClone.velocity = transform.forward * traceSpeed;
		if(!ShootClone.gameObject.active) 
			ShootClone.gameObject.SetActiveRecursively(true);
	}
}
