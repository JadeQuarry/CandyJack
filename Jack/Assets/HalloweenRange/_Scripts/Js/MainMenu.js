//MainMenu
class Location
{
 var title: String;
 var cost: int;
 var completedLevels: int;
 var locked: boolean;
 var icon: Texture2D;
}

var skin_gui : GUISkin;
var windowLevelsSize : Vector2;
var winLevelsPos: Vector2;
var windowSize : Vector2;
var winStartPos: Vector2;
var winSpeed: float;


var Money: int = -1;
var moneyIcon : Texture2D;
var Locations: Location[];

private var fullScreenCaption;
private var winFinishPos: Vector2;
private var winPos: Vector2;
private var opening: boolean = true;
private var openingLevels: boolean = false;
private var oldMoney: int;
//var NonPurchased: Texture;
//----------------------------------------------------------------------------------

function Start()
{
  
    
  if (!Screen.fullScreen) 
	fullScreenCaption = "FULLSCREEN";
   else
	fullScreenCaption = "WINDOWED";
	
	
	winFinishPos = Vector2((Screen.width - windowSize.x)/2 , (Screen.height - windowSize.y)/2);
	winStartPos.x = (Screen.width - windowSize.x)/2;
	winPos = winStartPos;
	
	winLevelsPos = Vector2((Screen.width - windowLevelsSize.x)/2 , (Screen.height - windowLevelsSize.y)/2);
	
	if(PlayerPrefs.HasKey ("HalloweenRange_Money") ) Money = PlayerPrefs.GetInt("HalloweenRange_Money");
	 else
	  PlayerPrefs.SetInt("HalloweenRange_Money", Money);
	  
	 oldMoney = Money;
	 
   for ( var l = 0; l < (Locations.Length); l ++)
    { 
        if (PlayerPrefs.HasKey ("HalloweenRange_Location"+ l.ToString()))   Locations[l].cost = 0;//Locations[l].locked = false;
        if (PlayerPrefs.HasKey ("HalloweenRange"+ l.ToString()+"_Levels"))   Locations[l].completedLevels = PlayerPrefs.GetInt("HalloweenRange"+ l.ToString()+"_Levels");
    }
    
        
}


//----------------------------------------------------------------------------------

function Update () 
  {
  
  Time.timeScale = 1; 
  
 if (Input.GetKeyUp(KeyCode.Delete))  PlayerPrefs.DeleteAll();
	
	
	Screen.lockCursor = false;
  	Screen.showCursor = true;
  	
  	
  	 if (oldMoney != Money)
     { 
       oldMoney = Money;
       PlayerPrefs.SetInt("HalloweenRange_Money", Money);
      }
      
  }

			
//----------------------------------------------------------------------------------
			
function OnGUI () {

  GUI.skin = skin_gui;
  
 if (opening)
   {
   // if (winFinishPos.x != (Screen.width - windowSize.x)/2 || winFinishPos.y != (Screen.height - windowSize.y)/2) 
    //	winFinishPos = Vector2((Screen.width - windowSize.x)/2 , (Screen.height - windowSize.y)/2);
	//winPos = winFinishPos;
   }
   else
    if (GUI.Button (Rect ((Screen.width-100)/2 , winLevelsPos.y+windowLevelsSize.y+2, 100, 30), "CANCEL") ) 
       {
 	    opening = true;
		openingLevels = false;
		var helpGui = GameObject.Find("HelpGUI");
		if (helpGui.guiTexture.enabled) helpGui.guiTexture.enabled = false;
		   // else helpGui.guiTexture.enabled = true;
 	   }
 	   

  
  
 windowRect = GUI.Window (0, Rect( winPos.x, winPos.y, windowSize.x, windowSize.y) , WindowFunction, "MAIN MENU");
 if (openingLevels) 
 	{

  //  GUI.Button (Rect (0, Screen.height-40, 120, 40), GUIContent (" " + Money.ToString(), moneyIcon));
   	 GUI.Button (Rect (0, Screen.height-35, 85, 35),"");
  	  GUI.DrawTexture(Rect (0, Screen.height-30, 25, 25), moneyIcon );  
      GUI.Label (Rect (20, Screen.height-30, 80, 25), " " + Money.ToString());   
    
 	 windowLevelsRect = GUI.Window (0, Rect( winLevelsPos.x, winLevelsPos.y, windowLevelsSize.x, windowLevelsSize.y) , WindowLevelsFunction, "CHOOSE LOCATION");
 	/* if (GUI.Button (Rect ( (winLevelsPos.x+windowLevelsSize.x)/2-10, winLevelsPos.y+windowLevelsSize.y+2, 120, 30), "CANCEL")) 
 	   {
 	    opening = true;
		openingLevels = false;
 	   }*/
 	}
	
}
//----------------------------------------------------------------------------------

function WindowFunction (windowID : int) {
   
   
  if (opening)
  { 
   if (winPos.x < winFinishPos.x) winPos.x += winSpeed * Time.deltaTime;
   if (winPos.y < winFinishPos.y) winPos.y += winSpeed * Time.deltaTime;
  }
   else
    { 
      if (winPos.x > winStartPos.x) winPos.x -= winSpeed * Time.deltaTime;
      if (winPos.y > winStartPos.y) winPos.y -= winSpeed * Time.deltaTime;
    }
    
    
    
	
	if (GUI.Button (Rect (50,35,150,40), "START")) 
	{
		opening = false;
		//Application.LoadLevel ("1");
		openingLevels = true;
	}
	
	if (GUI.Button (Rect (50,85,150,40), "HELP")) 
	{
	    opening = false;
	    
		var helpGui = GameObject.Find("HelpGUI");
		if (helpGui.guiTexture.enabled) helpGui.guiTexture.enabled = false;
		    else helpGui.guiTexture.enabled = true;
	}
	
	if (GUI.Button (Rect (50,135,150,40), fullScreenCaption)) 
	{
		 if (Screen.fullScreen) 
		  		fullScreenCaption = "FULLSCREEN";
		  	else
		  	    fullScreenCaption = "WINDOWED";
		  

		  Screen.fullScreen = !Screen.fullScreen;
		  
	}
	
	
	if (GUI.Button (Rect (50,185,150,40), "EXIT")) 
	{
	     PlayerPrefs.Save();
		 Application.Quit();
	}
	
	

}


//----------------------------------------------------------------------------------

function WindowLevelsFunction (windowID : int) {

        
 for ( var l = 0; l < (Locations.Length); l ++)
  {
  	
  	
  	
  if (Locations[l].locked)
  {
   	if (GUI.Button (Rect (20 + 125*l,30,115,160), Locations[l].title) )
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
	       GUI.Label (Rect(65 + l*125, 70, 115, 30), "BUY");
	       GUI.Label (Rect(35 + l*125, 122, 115, 30), "For $"+ Locations[l].cost.ToString());
	      }
	      else
	        {
	          GUI.DrawTexture(Rect (28 + 125*l,40,100,139), Locations[l].icon);
	          GUI.Label (Rect(27 + l*125, 155, 115, 35), Locations[l].title);
	        }
	    }
	    
	    
  }
	
	
	GUI.Label (Rect (70 + 125*l, 190, 115, 30), ""+ (Locations[l].completedLevels).ToString()+ "/4");
  
  
	 if (!Locations[l].locked)
	   for	( var i = 0; i < 4; i ++)
	  	 {
	  	 
	  	  if ((i-1) < Locations[l].completedLevels)
		    {
		     if(GUI.Button (Rect (24+ l*125 ,30+i*40,115,40), "Level " + ((i+1) + l*4).ToString())) 
		      {
		       PlayerPrefs.Save();
		       Application.LoadLevel ( ((l*4)+(i+1)).ToString() );
		      }
		    }
		   else
		     if(GUI.Button (Rect (24 + l*125, 30+i*40,115,40), "LOCKED"/*+ ((i+1) + l*4).ToString()*/ ));
		  }
	  
  }	
		

	GUI.Button (Rect (20 + 125,30,115,160), "COMING\nSOON");
	GUI.Button (Rect (20 + 125*2,30,115,160), "COMING\nSOON");
}