
var lifeTime: float = 3;  			// After this time object will be destroyed


//=======================================================================================================

function Start () {

// Time when object will be destroyed
	lifeTime = lifeTime + Time.time;  
	
 } 
	
//---------------------------------------------------------------------------------------------------------	

function Update () {

// Destroy the object if it lifeTIme has expired
	if (Time.time > lifeTime) {
	
	    if (gameObject.tag == "bullet") 
	     {
	       var questScript = GameObject.Find("Quest").GetComponent(QuestScript);
	       questScript.SetMisses(questScript.GetMisses()+ 1);
	     }
	     
		Destroy(gameObject); 
	}
}

//---------------------------------------------------------------------------------------------------------
