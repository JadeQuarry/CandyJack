// Script to show/update quest tasks and current status
//QuestWindow
	class Task
	{
	 var caption: String;
	 var goalValue: float;
	 var currentValue: float;
	 var units: String;
	 
	}


var win_sound : AudioClip;

var skin_gui : GUISkin;
var skin_hud : GUISkin;

var windowSize : Vector2;
var winFinishPos: Vector2;
var winSpeed: float;

var tasks : Task [];  		// List of tasks
var questScript;			// Quest script
var buyMenuObj : BuyMenu;   // BuyMenu script
var  player: GameObject;	// Player

private var winStartPos: Vector2;
private var winPos: Vector2;
private var opening: boolean = true;
private var winText: GUIText;
private var reward: int = 0;
private var Location0completedLevels : int = 0;


//==============================================================================================================
function Start () {
 
 // Load data for 1 location
  if (PlayerPrefs.HasKey ("HalloweenRange0"+"_Levels"))   
  				Location0completedLevels = PlayerPrefs.GetInt("HalloweenRange0_Levels");
  
 winText = GameObject.Find("WinText").guiText;
      
      
 Time.timeScale = 0; 
 
 winStartPos.x = (Screen.width - windowSize.x)/ 2; 
 winStartPos.y =  - windowSize.y; 
 winPos.x = (Screen.width - windowSize.x)/ 2; 
 winPos.y = winFinishPos.y;
 
 player.SetActive(false);
 
 questScript = GetComponent(QuestScript);
 
    tasks[0].goalValue = questScript.endTime;
    tasks[1].goalValue = questScript.endPoints;
    tasks[2].goalValue = questScript.targetsToHit;
    tasks[3].goalValue = questScript.missesAllowed;
    tasks[4].goalValue = questScript.ammoLimit;
    tasks[5].goalValue = questScript.defenceLive;
    tasks[6].goalValue = questScript.bossLive;
 
    
    tasks[0].currentValue = Time.time;
    tasks[1].currentValue = questScript.GetPoints();
    tasks[2].currentValue = questScript.GetTargetsHits ();
    tasks[3].currentValue = questScript.GetMisses ();
    tasks[4].currentValue = questScript.GetUsedAmmo ();
    tasks[5].currentValue = questScript.defenceLive;
    tasks[6].currentValue = questScript.bossLive;

  
  
}


//------------------------------------------------------------------------------------------------------------------------

function Update () {

 	tasks[0].currentValue = Time.time;
 	tasks[1].currentValue = questScript.GetPoints();
    tasks[2].currentValue = questScript.GetTargetsHits ();
    tasks[3].currentValue = questScript.GetMisses ();
    tasks[4].currentValue = questScript.GetUsedAmmo ();
    tasks[5].currentValue = questScript.GetDefenceLife ();
    tasks[6].currentValue = questScript.GetBossLife ();
	
}

//------------------------------------------------------------------------------------------------------------------------

function OnGUI () {

   GUI.skin = skin_gui; 
   var ButtonText: String;
  
  
    if (questScript.GetStatus() != 0)
     {
       // Show WIN message and  save params
       if(questScript.isSuccess())
       {
         
         if (winText.fontSize < 120) winText.fontSize += 3; 
         winText.text = "You WON!";

         ButtonText = "NEXT"; 
         
         if (reward <= 0) 
           { 
            reward = questScript.Reward;
         	GameObject.Find("GUI").GetComponent(BuyMenu).AddMoney(reward);
         	
         	if (Location0completedLevels < 4) Location0completedLevels++;
		    PlayerPrefs.SetInt("HalloweenRange0_Levels", Location0completedLevels);
		    
		    audio.clip = win_sound;
         	if (!audio.isPlaying) audio.Play();
           }
                 
         GUI.Label (Rect ( windowSize.x/2 - 45, (windowSize.y)/2 + 170, 400, 30), "YOUR REWARD for quest IS: " + questScript.Reward + "$");

       }
        else
         {
           // Show LOSE message and  save params
          if (winText.fontSize < 120) winText.fontSize += 3;
           winText.text = "You LOSE!";
           ButtonText = "RETRY"; 
          if (!audio.isPlaying) audio.Play();
         }
        
      
        // Show buttons to exit or continue
		   if (winText.fontSize >= 120)
		   {
		   
		     player.SetActive(false);
		     Screen.lockCursor = false;
			 Screen.showCursor = true;
			  			
			  			
		      // Move to main menu
			   if (GUI.Button (Rect (winPos.x - 10, winPos.y+windowSize.y + 5, 120, 30), "< MENU")) 
			    {
			     Time.timeScale = 1; 
			     Application.LoadLevel ("MainMenu");
			    }
			    
			   // Start next level
			   if (GUI.Button (Rect (winPos.x + windowSize.x - 120, winPos.y+windowSize.y + 5, 130, 30), ButtonText + " >")) 
			     {
			     
			     
			        for ( var w = 1; w < (buyMenuObj.weapons.length); w++) PlayerPrefs.SetInt("HalloweenRange_weapon" + w.ToString() + "_ammo", buyMenuObj.Shooter.weapon[w].ammo);
					
					
					if (questScript.isSuccess())
					  { 
					     if ( Application.loadedLevel < (Application.levelCount)) Application.LoadLevel (Application.loadedLevel+1);
					     else Application.LoadLevel (0);
					   }
						else
						  Application.LoadLevel (Application.loadedLevel);
			     }
		   }
      }

 // Show window  with tasks       
  if (opening)
  { 
  
    Time.timeScale = 0;
   
    Screen.lockCursor = false;
    Screen.showCursor = true ;

   if (GUI.Button (Rect (winPos.x - 10, winPos.y+windowSize.y + 5, 120, 30), "< CANCEL")) 
    {
     Time.timeScale = 1; 
     Application.LoadLevel ("MainMenu");
    }
    
   if (GUI.Button (Rect (winPos.x + windowSize.x - 120, winPos.y+windowSize.y + 5, 130, 30), "START >")) 
     {
 			Screen.lockCursor = true;
  			Screen.showCursor = false;
  			
		Time.timeScale = 1; 
		player.SetActive(true);
		opening = !opening;
     }

  
     windowQuestRect = GUI.Window (1, Rect( winPos.x, winPos.y, windowSize.x, windowSize.y) , WindowQuestFunction, "QUEST");

  }
  
  else
  {
      tasks[0].goalValue = questScript.GetLevelEndTime ();
      
	 var yPos: int = 0;
	 
	 GUI.skin = skin_hud; 
	  
	 // Show active tasks and their status in HUD 
	 for (var i = 0; i<tasks.Length ; i++)
	   if (tasks[i].goalValue >= 0 ) 
	   {
	   
	      var remainValue: int = (tasks[i].goalValue - tasks[i].currentValue);
	      
	       if (i < 5)
	       {
	        if (remainValue <= 0) tasks[i].goalValue = -1;
	        GUI.Label (Rect (5, 5 + yPos*20, windowSize.x, 30), tasks[i].caption + remainValue + tasks[i].units);
	       }
	          else
	           {
	            if (remainValue < 0) tasks[i].goalValue = -1;
	             GUI.Label (Rect (5, 5 + yPos*20, windowSize.x, 30), tasks[i].units + " LIFE: " + tasks[i].currentValue + "/" + tasks[i].goalValue);
	           }
	           
	         yPos++;
	   }
  
  
  }
  
  
}

//------------------------------------------------------------------------------------------------------------------------

function WindowQuestFunction (windowID : int) {

 GUI.Label (Rect (25, 15, windowSize.x, 30), "HERE ARE YOUR GOALS:");
 
 var yPos: int = 0;
 for (var i = 0; i<tasks.Length ; i++)
   if (tasks[i].goalValue >= 0 ) 
   {   
        if (i < 5)
         GUI.Label (Rect (23, 45 + yPos*25, windowSize.x, 30), " - " +tasks[i].caption + tasks[i].goalValue + tasks[i].units);
           else
         GUI.Label (Rect (23, 45 + yPos*25, windowSize.x, 30), " - " +tasks[i].caption + tasks[i].units); 
         
         yPos++;
   }
  
  
  GUI.Label (Rect (25, windowSize.y-40, windowSize.x, 30), "REWARD WILL BE: " + questScript.Reward + "$");
    
}

//------------------------------------------------------------------------------------------------------------------------