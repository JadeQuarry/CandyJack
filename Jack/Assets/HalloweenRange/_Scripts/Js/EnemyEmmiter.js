//EnemyEmmiter
// Generate enemies according to attached list and probability

   class Enemy
	{
	 var model : GameObject;
	 var probability : float = 1;
	}


var Enemies : Enemy[];					// List of enemies
var randomEnemy : boolean = false;		// Randomize enemies?
var singleEnemy : boolean = true;		// Only one live Enemy allowed
var generationTime: float = 1;			// How offten enemy should be generated ( if not singleEnemy)

private var singleton : GameObject = null;
private var createTime: float;
private var currentEnemyType: int = 0;


//========================================================================================================

function Start () {
  createTime = Time.time + generationTime;
}


//---------------------------------------------------------------------------------------------------------	

function Update () {

  
  if (Time.time > createTime)
   {
    
    var created: boolean = false;
      
      // Create enemy if !singleEnemy and/or singleton object not created
      if (singleEnemy) 
	        {
	          if (!singleton) 
	          { 
	           singleton = CreateEnemy (Enemies[currentEnemyType].model);
	           created = true;
	          }
	        }
	       else
	        {
	          CreateEnemy (Enemies[currentEnemyType].model);
	          created = true;
	        }
	         
	  // Randomize enemy
	 if (created)    
	    if(randomEnemy)
	      {
	       
	        var rv : int = Random.Range (0, Enemies.Length);
	
	           if (Random.value < Enemies[rv].probability) 
	                currentEnemyType = rv;
	
	      }    
	        else
	           if (currentEnemyType < (Enemies.Length-1)) currentEnemyType++; else currentEnemyType = 0;
	         

	         
     createTime = Time.time + generationTime;
  } 
}

//---------------------------------------------------------------------------------------------------------	
// Instantiate enemy object in current coordinates
function CreateEnemy (enemyObject : GameObject) : GameObject {
  
  var EnemyClone : GameObject = Instantiate(enemyObject, transform.position, enemyObject.transform.rotation);
//  if(!EnemyClone.active) EnemyClone.SetActiveRecursively(true);
	EnemyClone.SetActive(true);
  return EnemyClone;
}

//---------------------------------------------------------------------------------------------------------	
// Draw debug Zone-gizmo in Editor viewport
function OnDrawGizmos()
{
		Gizmos.color = Color (0,0,1,.5);
		Gizmos.DrawCube (transform.position, transform.localScale);
}

//---------------------------------------------------------------------------------------------------------	