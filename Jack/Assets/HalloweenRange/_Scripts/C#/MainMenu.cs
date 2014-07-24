using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	[System.Serializable]
	public class Location
	{
		public string title;
		public int cost;
		public int completedLevels;
		public bool locked;
		public Texture2D icon;
	}
	
	public GUISkin 		skin_gui;
	public Vector2 		windowLevelsSize;
	public Vector2 		winLevelsPos;
	public Vector2 		windowSize;
	public Vector2 		winStartPos;
	public float 		winSpeed;
	
	
	public int 			Money 			= -1;
	public Texture2D 	moneyIcon;
	public Location[] 	Locations;
	
	private string 		fullScreenCaption;
	private Vector2 	winFinishPos;
	private Vector2 	winPos;
	private bool 		opening 		= true;
	private bool 		openingLevels 	= false;
	private int 		oldMoney;

	//----------------------------------------------------------------------------------
	
	void Start()
	{
		if (!Screen.fullScreen) 
			fullScreenCaption = "FULLSCREEN";
		else
			fullScreenCaption = "WINDOWED";

		winFinishPos = new Vector2((Screen.width - windowSize.x)/2 , (Screen.height - windowSize.y)/2);
		winStartPos.x = (Screen.width - windowSize.x)/2;
		winPos = winStartPos;
		
		winLevelsPos = new Vector2((Screen.width - windowLevelsSize.x)/2 , (Screen.height - windowLevelsSize.y)/2);
		
		if(PlayerPrefs.HasKey ("HalloweenRange_Money") ) 
			Money = PlayerPrefs.GetInt("HalloweenRange_Money");
		else
			PlayerPrefs.SetInt("HalloweenRange_Money", Money);
		
		oldMoney = Money;
		
		for ( int l = 0; l < (Locations.Length); l ++)
		{ 
			if (PlayerPrefs.HasKey ("HalloweenRange_Location"+ l.ToString()))   
				Locations[l].cost = 0;
			if (PlayerPrefs.HasKey ("HalloweenRange"+ l.ToString()+"_Levels"))   
				Locations[l].completedLevels = PlayerPrefs.GetInt("HalloweenRange"+ l.ToString()+"_Levels");
		}
	}
	//----------------------------------------------------------------------------------
	void Update () 
	{
		Time.timeScale = 1; 
		
		if (Input.GetKeyUp(KeyCode.Delete))  
			PlayerPrefs.DeleteAll();

		Screen.lockCursor = false;
		Screen.showCursor = true;

		if (oldMoney != Money)
		{ 
			oldMoney = Money;
			PlayerPrefs.SetInt("HalloweenRange_Money", Money);
		}
	}
	//----------------------------------------------------------------------------------
	
	void OnGUI () 
	{
		GUI.skin = skin_gui;
		
		if (opening)
		{
		}
		else if (GUI.Button (new Rect ((Screen.width-100)/2 , winLevelsPos.y+windowLevelsSize.y+2, 100, 30), "CANCEL") ) 
		{
			opening = true;
			openingLevels = false;
			GameObject helpGui = GameObject.Find("HelpGUI");
			if (helpGui.guiTexture.enabled) 
				helpGui.guiTexture.enabled = false;
		}

		Rect windowRect = new Rect (winPos.x, winPos.y, windowSize.x, windowSize.y);
		windowRect = GUI.Window (0, windowRect , WindowFunction, "MAIN MENU");
		if (openingLevels) 
		{
			//  GUI.Button (Rect (0, Screen.height-40, 120, 40), GUIContent (" " + Money.ToString(), moneyIcon));
			GUI.Button (new Rect (0, Screen.height-35, 85, 35),"");
			GUI.DrawTexture(new Rect (0, Screen.height-30, 25, 25), moneyIcon );  
			GUI.Label (new Rect (20, Screen.height-30, 80, 25), " " + Money.ToString());   

			Rect windowLevelsRect = new Rect( winLevelsPos.x, winLevelsPos.y, windowLevelsSize.x, windowLevelsSize.y);
			windowLevelsRect = GUI.Window (0, windowLevelsRect , WindowLevelsFunction, "CHOOSE LOCATION");
		}
	}
	//----------------------------------------------------------------------------------
	//Mian windown
	void WindowFunction (int windowID) 
	{	
		if (opening)
		{ 
			if (winPos.x < winFinishPos.x) 
				winPos.x += winSpeed * Time.deltaTime;

			if (winPos.y < winFinishPos.y) 
				winPos.y += winSpeed * Time.deltaTime;
		}
		else
		{ 
			if (winPos.x > winStartPos.x) 
				winPos.x -= winSpeed * Time.deltaTime;

			if (winPos.y > winStartPos.y) 
				winPos.y -= winSpeed * Time.deltaTime;
		}

		//must to modify button position (can not lock position)
		if (GUI.Button (new Rect (50,35,150,40), "START")) 
		{
			opening = false;
			//Application.LoadLevel ("1");
			openingLevels = true;
		}
		
		if (GUI.Button (new Rect (50,85,150,40), "HELP")) 
		{
			opening = false;
			
			GameObject helpGui = GameObject.Find("HelpGUI");
			if (helpGui.guiTexture.enabled) 
				helpGui.guiTexture.enabled = false;
			else 
				helpGui.guiTexture.enabled = true;
		}
		
		if (GUI.Button (new Rect (50,135,150,40), fullScreenCaption)) 
		{
			if (Screen.fullScreen) 
				fullScreenCaption = "FULLSCREEN";
			else
				fullScreenCaption = "WINDOWED";

			Screen.fullScreen = !Screen.fullScreen;
		}

		if (GUI.Button (new Rect (50,185,150,40), "EXIT")) 
		{
			PlayerPrefs.Save();
			Application.Quit();
		}
	}
	//----------------------------------------------------------------------------------
	
	void WindowLevelsFunction (int windowID ) 
	{
		for ( int l = 0; l < (Locations.Length); l ++)
		{
			if (Locations[l].locked)
			{
				if (GUI.Button (new Rect (20 + 125*l,30,115,160), Locations[l].title) )
				{    
					if (Locations[l].cost == 0) Locations[l].locked = false ;
					
					if ((Money - Locations[l].cost) >=0 ) 
					{
						Money -= Locations[l].cost;
						Locations[l].cost = 0;
						PlayerPrefs.SetInt("HalloweenRange_Location"+ l.ToString(), 1);
					}
				}
				else
				{
					if (Locations[l].cost>0) 
					{
						// GUI.DrawTexture(Rect(20 + l*125, 30, 115, 160), NonPurchased);
						GUI.Label (new Rect(65 + l*125, 70, 115, 30), "BUY");
						GUI.Label (new Rect(35 + l*125, 122, 115, 30), "For $"+ Locations[l].cost.ToString());
					}
					else
					{
						GUI.DrawTexture(new Rect (28 + 125*l,40,100,139), Locations[l].icon);
						GUI.Label (new Rect(27 + l*125, 155, 115, 35), Locations[l].title);
					}
				}
			}
			
			
	//		GUI.Label (new Rect (70 + 125*l, 190, 115, 30), ""+ (Locations[l].completedLevels).ToString()+ "/4");
			GUI.Label (new Rect (70 + 125*l, 190, 115, 30), ""+ "1"+ "/4");
			
			if (!Locations[l].locked)
				for	( int i = 0; i < 4; i ++)
				{
					//default on;y one level can play
			//		if ((i-1) < Locations[l].completedLevels)
					if( i == 0)
					{
						if(GUI.Button (new Rect (24+ l*125 ,30+i*40,115,40), "Level " + ((i+1) + l*4).ToString())) 
						{
							PlayerPrefs.Save();
							Application.LoadLevel ("Level 1");
						//	Application.LoadLevel ( ((l*4)+(i+1)).ToString() );
						}
					}else
					{
						if(GUI.Button (new Rect (24 + l*125, 30+i*40,115,40), "LOCKED"/*+ ((i+1) + l*4).ToString()*/ ))
						{
						}
					}
			}
			
		}	

		GUI.Button (new Rect (20 + 125,30,115,160), "COMING\nSOON");
		GUI.Button (new Rect (20 + 125*2,30,115,160), "COMING\nSOON");
	}
}
