// Main script that tracks are goals achieved or not
//QuestScript
// (-1 = disabled)
var endTime : float = 0; 		// Time attack  
var endPoints : int = -1; 		// Points seeker 
var targetsToHit: int = -1;		// Sniper - not all targets allowed
var missesAllowed: int = -1; 	// Limited misses
var ammoLimit: int = -1;  		// Limited ammo
var defenceLive: float = -1; 	// Defence
var bossLive: float = -1;		// Boss
var Reward: int = 1000;	       	// Reward for quest

private var levelEndTime: float = 0;		
private var currentPoints: int = 0; 		
private var currentTargetsHit: int = 0; 	
private var currentMisses: int = 0; 		
private var currentUsedAmmo: int = 0; 		
private var currentDefenceLive: float = 0; 	
private var currentBossLive: float = 0; 	
private var successStatus: int = 0;


//========================================================================================================

function Start () {

  levelEndTime = Time.time + endTime;
  
  currentDefenceLive = defenceLive;
  currentBossLive = bossLive;

}


//---------------------------------------------------------------------------------------------------------------------------

function Update () {

     
      if (!CheckLimits ()) successStatus = -1;
      if (!CheckResults () && (Time.time > levelEndTime) && (endTime >=0) )  successStatus = -1;
      if (CheckResults () && (endTime < 0) && CheckLimits () ) successStatus = 1; 
      if (CheckResults () && (endTime >=0) && CheckLimits () ) if (Time.time < levelEndTime) successStatus = 1;  

}


//---------------------------------------------------------------------------------------------------------------------------

function CheckResults (): boolean {

      if (currentPoints - endPoints < 0  ) return false; 
      if (currentTargetsHit - targetsToHit < 0) return false; 
      if (currentBossLive > 0) return false;
    
    return true;
}

//---------------------------------------------------------------------------------------------------------------------------

function CheckLimits (): boolean {

     if (missesAllowed >= 0 && ( currentMisses - missesAllowed > 0 )) return false; 
     if (ammoLimit >= 0 &&  ( currentUsedAmmo - ammoLimit > 0) ) return false; 
     if (defenceLive >= 0 && currentDefenceLive <= 0) return false; 

     
    return true;
}

//---------------------------------------------------------------------------------------------------------------------------
// Use those functions
function isSuccess () : boolean {
  
  if (successStatus > 0) return true;
  if (successStatus < 0) return false;
}

//----
function GetStatus () : int {
  
  return successStatus;
}

//---------------------------------------------------------------------------------------------------------------------------
function SetLevelEndTime ( endTime: float) {
  
  levelEndTime = endTime;
}

//----
function GetLevelEndTime () : float {
  
  return levelEndTime;
}


//----
function SetPoints (points: int) {
  
  currentPoints = points;
  
}
function GetPoints (): int {
  
  return currentPoints;
  
}
//----

function SetTargetsHits (Hits: int) {
 
  currentTargetsHit = Hits;
  
}
function GetTargetsHits (): int {
 
  return currentTargetsHit;
  
}
//----

function SetMisses (misses: int) {
 
  currentMisses = misses;
  
}
function GetMisses (): int {
 
  return currentMisses;
  
}
//----

function SetUsedAmmo (ammo: int) {
 
  currentUsedAmmo = ammo;
  
}
function GetUsedAmmo (): int {
 
  return currentUsedAmmo;
  
}

//----
function SetDefenceLife (life: float) {
 
  currentDefenceLive = life;
  
}
function GetDefenceLife () : float {
 
  return currentDefenceLive;
  
}

//----
function SetBossLife (life: float) {
 
  currentBossLive = life;
  
}
function GetBossLife () : float {
 
  return currentBossLive;
  
}
//---------------------------------------------------------------------------------------------------------------------------
