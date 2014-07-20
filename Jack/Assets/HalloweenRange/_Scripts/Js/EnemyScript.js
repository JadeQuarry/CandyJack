
// Main script for enemy with damage/score updating functions included
//EnemyScript
var player: GameObject;							// Player object
var wreck: GameObject;							// Wreck object
var life : float = 3;							// Life
var EnemyClass: int = 0; 					    // Type of enemy. Influence on equipment, life and points
var enemyCost: int = 50;						// Enemy cost (how much points  will be  earned)
var isDead : boolean = false;					// Is enemy dead

private var currentLife : float;
private var playerDamage : float = 0;
private var enemyAnimation: Animation;
private var playerShooter : Shooting;
private var previousBullet : Rigidbody = null;


//=======================================================================================================


function Start () {
 
  if (EnemyClass>0)
   {
    var highlightClone : GameObject = Instantiate (GameObject.Find("highlight"), transform.position, transform.rotation);
    highlightClone.transform.parent = transform;
   }		   

   enemyAnimation = GetComponentInChildren(Animation);

  playerShooter = player.GetComponentInChildren(Shooting);
  playerDamage =  playerShooter.weapon[playerShooter.activeWeapon].damage;
 
  currentLife = life;
  

}


//---------------------------------------------------------------------------------------------------------	

function OnCollisionEnter(collision : Collision) {
	
	     
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
  			   var questScript = GameObject.Find("Quest").GetComponent(QuestScript);
  			   questScript.SetPoints(questScript.GetPoints() + enemyCost);
  			  
  			   if (EnemyClass>0) questScript.SetTargetsHits (questScript.GetTargetsHits() +1); 
  			   GameObject.Find("GUI").GetComponent(BuyMenu).AddMoney(enemyCost);
  			 
  		    // Instantiate wrec obhject and  destroy enemy  			   
  			   var wreckClone : GameObject = Instantiate (wreck, transform.position, transform.rotation);
  			//   if(!wreckClone.active) wreckClone.SetActiveRecursively(true);
  				wreckClone.SetActive(true);
  			  	wreckClone.GetComponent(Point).Point = enemyCost;
  			  	Destroy(gameObject);
  			  	isDead = true;
  			  }
    			     
    	}
    	 else 
    	   	currentLife = currentLife - playerDamage;
 
    }
 
 }   
 
 
//---------------------------------------------------------------------------------------------------------	

function ChangeWeapon () 
{
   
   // Update damaging
    playerDamage = playerShooter.weapon[playerShooter.activeWeapon].damage;

}


//---------------------------------------------------------------------------------------------------------	

function SetLife (newLife: int) 
{
   
   currentLife = newLife;
       	
}
//---------------------------------------------------------------------------------------------------------	