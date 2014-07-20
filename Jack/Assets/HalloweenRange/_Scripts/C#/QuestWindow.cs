using UnityEngine;
using System.Collections;

public class QuestWindow : MonoBehaviour 
{
	[System.Serializable]
	public class Task
	{
		public string caption;
		public float goalValue;
		public float currentValue;
		public string units;
	}
	
	
	public AudioClip win_sound;
	
	public GUISkin skin_gui;
	public GUISkin skin_hud;
	
	public Vector2 windowSize;
	public Vector2 winFinishPos;
	public float winSpeed;
	
	public Task [] tasks;  				// List of tasks
	QuestScript questScript;			// Quest script
	private BuyMenu buyMenuObj;   		// BuyMenu script
	public GameObject  player;			// Player
	
	private Vector2 winStartPos;
	private Vector2 winPos;
	private bool opening = true;
	private GUIText winText;
	private int reward = 0;
	private int Location0completedLevels = 0;
	

	//==============================================================================================================
	void Start () 
	{
		buyMenuObj = GameObject.Find ("GUI").GetComponent<BuyMenu> ();
		// Load data for 1 location
		if (PlayerPrefs.HasKey ("HalloweenRange0"+"_Levels"))   
			Location0completedLevels = PlayerPrefs.GetInt("HalloweenRange0_Levels");
		
		winText = GameObject.Find("WinText").guiText;
		
		
		Time.timeScale = 0; 
		
		winStartPos.x = (Screen.width - windowSize.x)/ 2; 
		winStartPos.y =  - windowSize.y; 
		winPos.x = (Screen.width - windowSize.x)/ 2; 
		winPos.y = winFinishPos.y;
		
//		player.SetActiveRecursively(false);
		player.SetActive (false);
		questScript = GetComponent<QuestScript>();
		
		tasks[0].goalValue 		= questScript.endTime;
		tasks[0].currentValue	= Time.deltaTime;

		tasks[1].goalValue 		= questScript.endPoints;
		tasks[1].currentValue 	= questScript.Points;

		tasks[2].goalValue 		= questScript.targetsToHit;
		tasks[2].currentValue 	= questScript.Hits;

		tasks[3].goalValue 		= questScript.missesAllowed;
		tasks[3].currentValue 	= questScript.Misses;

		tasks[4].goalValue 		= questScript.ammoLimit;
		tasks[4].currentValue 	= questScript.Ammo;

		tasks[5].goalValue 		= questScript.defenceLive;
		tasks[5].currentValue 	= questScript.defenceLive;

		tasks[6].goalValue 		= questScript.bossLive;
		tasks[6].currentValue 	= questScript.bossLive;
	}

	//------------------------------------------------------------------------------------------------------------------------
	void Update () 
	{
		tasks[0].currentValue = Time.time;
		tasks[1].currentValue = questScript.Points;
		tasks[2].currentValue = questScript.Hits;
		tasks[3].currentValue = questScript.Misses;
		tasks[4].currentValue = questScript.Ammo;
		tasks[5].currentValue = questScript.DefenceLife;
		tasks[6].currentValue = questScript.BossLife;
	}
	
	//------------------------------------------------------------------------------------------------------------------------
	void OnGUI ()
	{
		
		GUI.skin = skin_gui; 
		string ButtonText;

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
					GameObject.Find("GUI").GetComponent<BuyMenu>().AddMoney(reward);
					
					if (Location0completedLevels < 4) Location0completedLevels++;
					PlayerPrefs.SetInt("HalloweenRange0_Levels", Location0completedLevels);
					
					audio.clip = win_sound;
					if (!audio.isPlaying) audio.Play();
				}
				
				GUI.Label (new Rect ( windowSize.x/2 - 45, (windowSize.y)/2 + 170, 400, 30), "YOUR REWARD for quest IS: " + questScript.Reward + "$");
				
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
				if (GUI.Button (new Rect (winPos.x - 10, winPos.y+windowSize.y + 5, 120, 30), "< MENU")) 
				{
					Time.timeScale = 1; 
					Application.LoadLevel ("MainMenu");
				}
				
				// Start next level
				if (GUI.Button (new Rect (winPos.x + windowSize.x - 120, winPos.y+windowSize.y + 5, 130, 30), ButtonText + " >")) 
				{
					for ( int w = 1; w < (buyMenuObj.weapons.Length); w++) 
						PlayerPrefs.SetInt("HalloweenRange_weapon" + w.ToString() + "_ammo", buyMenuObj.Shooter_	.weapon[w].ammo);
					
					
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
			
			if (GUI.Button (new Rect (winPos.x - 10, winPos.y+windowSize.y + 5, 120, 30), "< CANCEL")) 
			{
				Time.timeScale = 1; 
				Application.LoadLevel ("MainMenu");
			}
			
			if (GUI.Button (new Rect (winPos.x + windowSize.x - 120, winPos.y+windowSize.y + 5, 130, 30), "START >")) 
			{
				Screen.lockCursor = true;
				Screen.showCursor = false;
				
				Time.timeScale = 1; 
				player.SetActive(true);
				opening = !opening;
			}

			Rect windowQuestRect = new Rect( winPos.x, winPos.y, windowSize.x, windowSize.y);
			windowQuestRect = GUI.Window (1, windowQuestRect, WindowQuestFunction, "QUEST");
		}
		
		else
		{
			tasks[0].goalValue = questScript.LevelEndTime;
			
			int yPos = 0;
			
			GUI.skin = skin_hud; 
			
			// Show active tasks and their status in HUD 
			for (int i = 0; i<tasks.Length ; i++)
				if (tasks[i].goalValue >= 0 ) 
				{
					int remainValue = (int)(tasks[i].goalValue - tasks[i].currentValue);
				
					if (i < 5)
					{
						if (remainValue <= 0) tasks[i].goalValue = -1;
						GUI.Label (new Rect (5, 5 + yPos*20, windowSize.x, 30), tasks[i].caption + remainValue + tasks[i].units);
					}else
					{
						if (remainValue < 0) tasks[i].goalValue = -1;
						GUI.Label (new Rect (5, 5 + yPos*20, windowSize.x, 30), tasks[i].units + " LIFE: " + tasks[i].currentValue + "/" + tasks[i].goalValue);
					}	
					yPos++;
				}
			}
	}
	
	//------------------------------------------------------------------------------------------------------------------------
	void WindowQuestFunction (int windowID) 
	{
		GUI.Label (new Rect (25, 15, windowSize.x, 30), "HERE ARE YOUR GOALS:");
		
		int yPos = 0;
		for (int i = 0; i<tasks.Length ; i++)
			if (tasks[i].goalValue >= 0 ) 
			{   
			if (i < 5)
				GUI.Label (new Rect (23, 45 + yPos*25, windowSize.x, 30), " - " +tasks[i].caption + tasks[i].goalValue + tasks[i].units);
			else
				GUI.Label (new Rect (23, 45 + yPos*25, windowSize.x, 30), " - " +tasks[i].caption + tasks[i].units); 
			
				yPos++;
			}
		
		
		GUI.Label (new Rect (25, windowSize.y-40, windowSize.x, 30), "REWARD WILL BE: " + questScript.Reward + "$");
		
	}
	//------------------------------------------------------------------------------------------------------------------------

}
