

   class Weapon
	{
	 var caption: String;
	 var cost: int;
	 var ammoCost: int;
	 var ammoAdd: int;
	 var icon : Texture2D;
	 var ammoIcon : Texture2D;
	}

var skin_gui : GUISkin;
var skin_hud : GUISkin;

var Shooter: Shooting;   // Shooter script
var weapons: Weapon[];	 // List of avaiable weapons

var windowSize : Vector2;
var winFinishPos: Vector2;
var winSpeed: float;

var moneyIcon : Texture2D;
var ESCKey: KeyCode;


private var winPos: Vector2;
private var opening: boolean = false;
private var winStartPos: Vector2;
private var Money: int;
private var oldMoney: int;


 
//=======================================================================================================
function AddMoney (additionalMoney : int) {
  Money += additionalMoney;
}


//---------------------------------------------------------------------------------------------------------	
function Start () {

 winStartPos.x = (Screen.width - windowSize.x)/ 2; 
 winStartPos.y =  - windowSize.y; 
 winPos.x = winStartPos.x;
 winPos.y = winStartPos.y;
 
 // Load saved moneys
 if (PlayerPrefs.HasKey ("HalloweenRange_Money") ) Money = PlayerPrefs.GetInt("HalloweenRange_Money");
 
 oldMoney = Money;
}

//---------------------------------------------------------------------------------------------------------	
function Update ()
{

 if (Input.GetKeyUp(ESCKey)) 
	 if (Time.timeScale > 0)
	  {
	 		Screen.lockCursor = !Screen.lockCursor ;
			Screen.showCursor = !Screen.showCursor ;
			opening = !opening;
	  }
	  
  // Save money
   if (oldMoney != Money)
     { 
       oldMoney = Money;
       PlayerPrefs.SetInt("HalloweenRange_Money", Money);
      }
}

//---------------------------------------------------------------------------------------------------------	

function OnGUI () {


   GUI.skin = skin_gui;
   
 
  if (opening)
  { 
  // Animate shopping window  
   if (winPos.x < winFinishPos.x) winPos.x += winSpeed * Time.deltaTime;
   if (winPos.y < winFinishPos.y) winPos.y += winSpeed * Time.deltaTime;
   
   if (GUI.Button (Rect (winPos.x - 10, winPos.y+windowSize.y + 5, 120, 30), "< MENU")) 
   {
     // Go to Main Menu
     for ( var w = 1; w < (weapons.length); w++) PlayerPrefs.SetInt("HalloweenRange_weapon" + w.ToString() + "_ammo", Shooter.weapon[w].ammo);
	 Application.LoadLevel ("MainMenu");				
   }
   
    // Close window
   if (GUI.Button (Rect (winPos.x + windowSize.x - 120, winPos.y+windowSize.y + 5, 130, 30), "RESUME >")) 
     {
 		Screen.lockCursor = !Screen.lockCursor ;
		Screen.showCursor = !Screen.showCursor ;
		opening = !opening;
     }
  }
   else
    { 
      if (winPos.x > winStartPos.x) winPos.x -= winSpeed * Time.deltaTime;
      if (winPos.y > winStartPos.y) winPos.y -= winSpeed * Time.deltaTime;
    }
  
  // Show shopping window
  	windowRect = Rect( winPos.x, winPos.y, windowSize.x, windowSize.y);
    windowRect = GUI.Window (0,  windowRect, WindowFunction, "GUN SHOP");


  // Show HUD with current money and ammo values
    GUI.DrawTexture(Rect (0, Screen.height-30, 30, 30), moneyIcon );  
    GUI.Label (Rect (20, Screen.height-30, 100, 30), " " + Money.ToString());
        
    GUI.DrawTexture(Rect (Screen.width-80, Screen.height-30, 30, 30), weapons[Shooter.activeWeapon].ammoIcon);  
    GUI.Label (Rect (Screen.width-60, Screen.height-30, 100, 30), " " + Shooter.weapon[Shooter.activeWeapon].ammo.ToString());

}



//---------------------------------------------------------------------------------------------------------	
// Shopping function
function WindowFunction (windowID : int) {

	GUI.Label (Rect (24, 20, windowSize.x, 30), "BUY NEW WEAPON:");
    GUI.Label (Rect (24, 185, windowSize.x, 30), "BUY AMMUNITION:");
			
		
	 	 
	 for ( var w = 1; w < (weapons.length); w++)	 
		 {
		 
		 	 		GUI.skin = skin_hud;
		 	 
			 GUI.Label (Rect (30 + 115 * (w-1), 148, windowSize.x, 30),  "Ammo: " + Shooter.weapon[w].ammo.ToString());
			  
			  
			// Buy ammo 	  
			if (GUI.Button (Rect (25 + 115 * (w-1), 215, 100, 35),  GUIContent ((weapons[w].ammoCost).ToString()+ " for "+ (weapons[w].ammoAdd).ToString(), weapons[w].ammoIcon, "buy Ammo")))  
			 {
			   if ((Money - weapons[w].ammoCost) >=0) 
			     {
			     
			      Shooter.weapon[w].ammo += weapons[w].ammoAdd;
			      Money -= weapons[w].ammoCost;
			              
				   PlayerPrefs.SetInt("HalloweenRange_weapon" + w.ToString() + "_ammo", Shooter.weapon[w].ammo);
			     }
			 }
			 
			 
			        GUI.skin = skin_gui;
			            
			  
			// Buy weapons 	  
			 if (!Shooter.weapon[w].enabled)	
			  {   
				if (BuyButton(Rect (25 + 115 * (w-1), 50, 100, 100), weapons[w].caption + "\n $", weapons[w].icon, weapons[w].cost )) 
					   if ((Money - weapons[w].cost) >=0) 
					     {
					      Shooter.weapon[w].enabled = true;
					      Money -= weapons[w].cost;
					      if (Shooter.weapon[w].enabled) PlayerPrefs.SetInt("HalloweenRange_weapon" + w.ToString(), 1);
					     }
			   }
			    else GUI.DrawTexture(Rect (25 + 115 * (w-1), 50, 100, 100), weapons[w].icon);
			   
			  
		   }
      
      
  
   GUI.Button (Rect (25 + 115*2,50,100,120), "COMING\nSOON");
}


//---------------------------------------------------------------------------------------------------------		

function BuyButton (screenRect : Rect, caption : String, icon : Texture2D, cost: int) : boolean 
{
  
  if ( GUI.Button (screenRect, GUIContent ("", icon, "Press to BUY")) ) return true;
  GUI.Label (Rect (screenRect.x+5, screenRect.y+5, screenRect.width, screenRect.height), caption + cost.ToString() );
}
//---------------------------------------------------------------------------------------------------------	