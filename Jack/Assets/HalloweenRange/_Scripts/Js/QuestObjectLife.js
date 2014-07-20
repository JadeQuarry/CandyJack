//QuestObjectLife
var isBoss: boolean = false;

private var Damage: float = 1;
private var questScript;
private var playerShooter;
private var enemyScript;

//---------------------------------------------------------------------------------------------------------	

function Start () {

 questScript = GameObject.Find("Quest").GetComponent(QuestScript); 
 
 enemyScript = GetComponent(EnemyScript);
    
 if (isBoss) 
  {
   enemyScript.SetLife(questScript.GetBossLife());
   playerShooter = GetComponent(EnemyScript).player.GetComponentInChildren(Shooting);
  }
   else
      enemyScript.SetLife(questScript.GetDefenceLife());
 
  
}

//---------------------------------------------------------------------------------------------------------	

function Update () {
  if (isBoss)  
    {
     Damage = playerShooter.weapon[playerShooter.activeWeapon].damage;
     enemyScript.SetLife(questScript.GetBossLife());
    }
     else
      enemyScript.SetLife(questScript.GetDefenceLife());
}

//---------------------------------------------------------------------------------------------------------	

function OnCollisionEnter(collision : Collision) {
  
   if (collision.gameObject.tag == "bullet" || collision.gameObject.tag == "enemy")
    if (isBoss)
         questScript.SetBossLife( questScript.GetBossLife() - Damage);
      else
         questScript.SetDefenceLife( questScript.GetDefenceLife() - Damage);
   
}  


//---------------------------------------------------------------------------------------------------------	

function SetDamaging(damage : float) {

  Damage = damage;
  
}
//---------------------------------------------------------------------------------------------------------	