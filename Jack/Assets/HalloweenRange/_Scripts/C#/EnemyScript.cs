using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour 
{
	public GameObject 		player;							// Player object
	public GameObject 		wreck;							// Wreck object
	public float 			life 		= 3;				// Life
	public int 				EnemyClass 	= 0; 				// Type of enemy. Influence on equipment, life and points
	public int 				enemyCost 	= 50;				// Enemy cost (how much points  will be  earned)
	public bool 			isDead 		= false;			// Is enemy dead
	
	private float 			currentLife;
	private float 			playerDamage 	= 0;
	private Shooting 		playerShooter;
	private Rigidbody 		previousBullet 	= null;

	//=======================================================================================================
	void Start () 
	{	
		if (EnemyClass>0)
		{
			GameObject highlightClone = Instantiate (GameObject.Find("highlight"), transform.position, transform.rotation)as GameObject;
			highlightClone.transform.parent = transform;
		}		   

		playerShooter = player.transform.FindChild ("Main Camera/Shooter").GetComponent<Shooting> ();
		playerDamage =  playerShooter.weapon[playerShooter.activeWeapon].damage;
		
		currentLife = life;
	}
	

	//---------------------------------------------------------------------------------------------------------	
	void OnCollisionEnter(Collision collision) 
	{	
		// Decrease life, if object hitted by bullet. 
		if (collision.gameObject.tag == "bullet") 
			if ( collision.collider.gameObject.rigidbody != previousBullet)
			{
				previousBullet = collision.collider.gameObject.rigidbody;
				ChangeWeapon();
			
				if (currentLife <= playerDamage) 
				{
					if (!isDead)
					{
					// Change quest variables  
					var questScript = GameObject.Find("Quest").GetComponent<QuestScript>();
					questScript.Points = questScript.Points + enemyCost;
					
					if (EnemyClass>0)
					{
						questScript.Hits ++; 
					}
					//	questScript.SetTargetsHits ((int)questScript.GetTargetsHits() +1); 
					GameObject.Find("GUI").GetComponent<BuyMenu>().AddMoney(enemyCost);
					
					// Instantiate wrec obhject and  destroy enemy  			   
					GameObject wreckClone = Instantiate (wreck, transform.position, transform.rotation)as GameObject;
					wreckClone.SetActiveRecursively(true);
					wreckClone.GetComponent<Point>().Point_ = enemyCost;
					Destroy(gameObject);
					isDead = true;
				}
			}
			else 
				currentLife = currentLife - playerDamage;
		}	
	}   

	//---------------------------------------------------------------------------------------------------------		
	void ChangeWeapon () 
	{
		// Update damaging
		playerDamage = playerShooter.weapon[playerShooter.activeWeapon].damage;
	}


	//---------------------------------------------------------------------------------------------------------	
	public void SetLife (int newLife) 
	{
		currentLife = newLife;
		
	}
	//---------------------------------------------------------------------------------------------------------	
}
