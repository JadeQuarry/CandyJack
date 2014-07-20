using UnityEngine;
using System.Collections;

public class QuestScript : MonoBehaviour 
{
	public float 	endTime 		= 0;		// Time attack  
	public int 		endPoints 		= -1; 		// Points seeker 
	public int 		targetsToHit 	= -1;		// Sniper - not all targets allowed
	public int 		missesAllowed 	= -1; 		// Limited misses
	public int 		ammoLimit 		= -1;  		// Limited ammo
	public float 	defenceLive 	= -1; 		// Defence
	public float 	bossLive 		= -1;		// Boss
	public int 		Reward 			= 1000;	    // Reward for quest
	
	private float 	levelEndTime 	= 0;		
	private int 	currentPoints 	= 0; 		
	private int 	currentTargetsHit = 0; 	
	private int 	currentMisses 	= 0; 		
	private int 	currentUsedAmmo = 0; 		
	private float 	currentDefenceLive = 0; 	
	private float 	currentBossLive = 0; 	
	private int 	successStatus 	= 0;
	
	void Start () 
	{
		levelEndTime = Time.time + endTime;
		
		currentDefenceLive = defenceLive;
		currentBossLive = bossLive;
	}

	void Update () 
	{
		if (!CheckLimits ()) successStatus = -1;
		if (!CheckResults () && (Time.time > levelEndTime) && (endTime >=0) )  successStatus = -1;
		if (CheckResults () && (endTime < 0) && CheckLimits () ) successStatus = 1; 
		if (CheckResults () && (endTime >=0) && CheckLimits () ) if (Time.time < levelEndTime) successStatus = 1; 
	}

	public bool CheckResults ()
	{
		if (currentPoints - endPoints < 0  ) return false; 
		if (currentTargetsHit - targetsToHit < 0) return false; 
		if (currentBossLive > 0) return false;
		
		return true;
	}

	public bool CheckLimits ()
	{
		if (missesAllowed >= 0 && ( currentMisses - missesAllowed > 0 )) return false; 
		if (ammoLimit >= 0 &&  ( currentUsedAmmo - ammoLimit > 0) ) return false; 
		if (defenceLive >= 0 && currentDefenceLive <= 0) return false; 
			
		return true;
	}

	public bool isSuccess()
	{
		if (successStatus > 0) 
			return true;
		else 
			return false;
	}

	public int GetStatus()
	{
		return successStatus;
	}
	//------------------------------------------------
	public float LevelEndTime
	{
		get{return levelEndTime;}
		set{levelEndTime = value;}
	}
	//------------------------------------------------
	public int Points
	{
		get{return currentPoints;}
		set{currentPoints = value;}
	}
	//------------------------------------------------
	public int Hits
	{
		get{return currentTargetsHit;}
		set{currentTargetsHit = value;}
	}
	//------------------------------------------------
	public int Misses
	{
		get {return currentMisses;}
		set {currentMisses = value;}
	}
	//------------------------------------------------
	public int Ammo
	{
		get{return currentUsedAmmo;}
		set{currentUsedAmmo = value;}
	}
	//------------------------------------------------
	public float DefenceLife
	{
		get{return currentDefenceLive;}
		set{currentDefenceLive = value;}
	}
	//------------------------------------------------
	public float BossLife
	{
		get {return currentBossLive;}
		set {currentBossLive = value;}
	} 
}
