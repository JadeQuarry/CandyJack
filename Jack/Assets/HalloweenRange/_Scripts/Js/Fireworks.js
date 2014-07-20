//Fireworks
var questScript; 

function Start () {
 questScript =  GameObject.Find("Quest").GetComponent(QuestScript);
}


function Update () 
{

  particleEmitter.emit = questScript.isSuccess() ;
  
  transform.position.x += Random.insideUnitSphere.x ;
  transform.position.y += Random.insideUnitSphere.y/5 ;
}