using UnityEngine;
using System.Collections;

public class BuyMenu : MonoBehaviour 
{
	[System.Serializable]
	public class Weapon
	{
		public string caption;
		public int cost;
		public int ammoCost;
		public int ammoAdd;
		public Texture2D icon;
		public Texture2D ammoIcon;
	}
	public GameObject 	player;			//player 
	public GUISkin 		skin_gui;		//gui skin
	public GUISkin 		skin_hud;		//hud skin
	
	public Shooting 	Shooter_;   	// Shooter script
	public Weapon[] 	weapons;	 	// List of avaiable weapons
	
	public Vector2 		windowSize;		//gui windowsize
	public Vector2 		winFinishPos;	//gui finished position
	public float 		winSpeed;		//gui move speed
	
	public Texture2D 	moneyIcon;	
	public KeyCode 		ESCKey;

	private bool 		opening;
	private Vector2 	winPos;
	private Vector2 	winStartPos;
	private int 		Money;
	private int 		oldMoney;

	//=======================================================================================================
	public void AddMoney (int additionalMoney) 
	{
		Money += additionalMoney;
	}
	//---------------------------------------------------------------------------------------------------------
	public bool bOpen
	{
		get{return opening;}
	}
	void Awake()
	{
		opening = false;
		Shooter_ = player.transform.FindChild ("Main Camera/Shooter").GetComponent<Shooting> ();
		Debug.Log ("hello");
	}

	void Start () 
	{	
		winStartPos.x = (Screen.width - windowSize.x)/ 2; 
		winStartPos.y =  - windowSize.y; 
		winPos.x = winStartPos.x;
		winPos.y = winStartPos.y;
		
		// Load saved moneys
		if (PlayerPrefs.HasKey ("HalloweenRange_Money") ) 
			Money = PlayerPrefs.GetInt("HalloweenRange_Money");
		
		oldMoney = Money;
	}
	
	//---------------------------------------------------------------------------------------------------------	
	//---------------------------------------------------------------------------------------------------------	
	void Update ()
	{
		
		if (Input.GetKeyUp(ESCKey)) 
			if (Time.timeScale > 0)
			{
				Screen.lockCursor = !Screen.lockCursor ;
				Screen.showCursor = !Screen.showCursor ;
				opening = !opening;

				//lock camera
				
			}
		// Save money
		if (oldMoney != Money)
		{ 
			oldMoney = Money;
			PlayerPrefs.SetInt("HalloweenRange_Money", Money);
		}
	}
	
	//---------------------------------------------------------------------------------------------------------	

	void OnGUI () 
	{
		GUI.skin = skin_gui;

		if (opening)
		{ 
			// Animate shopping window  
			if (winPos.x < winFinishPos.x) winPos.x += winSpeed * Time.deltaTime;
			if (winPos.y < winFinishPos.y) winPos.y += winSpeed * Time.deltaTime;
			
			if (GUI.Button (new Rect (winPos.x - 10, winPos.y+windowSize.y + 5, 120, 30), "< MENU")) 
			{
				// Go to Main Menu
				for ( int w = 1; w < (weapons.Length); w++) 
					PlayerPrefs.SetInt("HalloweenRange_weapon" + w.ToString() + "_ammo", Shooter_.weapon[w].ammo);
				Application.LoadLevel ("MainMenu");				
			}

			// Close window
			if (GUI.Button (new Rect (winPos.x + windowSize.x - 120, winPos.y+windowSize.y + 5, 130, 30), "RESUME >")) 
			{
				Screen.lockCursor = !Screen.lockCursor ;
				Screen.showCursor = !Screen.showCursor ;
				opening = !opening;
			}
		}
		else
		{ 
			if (winPos.x > winStartPos.x) 
				winPos.x -= winSpeed * Time.deltaTime;
			if (winPos.y > winStartPos.y) 
				winPos.y -= winSpeed * Time.deltaTime;
		}

		// Show shopping window
		Rect windowRect = new Rect (winPos.x, winPos.y, windowSize.x, windowSize.y);
		windowRect = GUI.Window (0, windowRect,WindowFunction, "GUN SHOP");
		
		
		// Show HUD with current money and ammo values
		GUI.DrawTexture(new Rect (0, Screen.height-30, 30, 30), moneyIcon );  
		GUI.Label (new Rect (20, Screen.height-30, 100, 30), " " + Money.ToString());
		
		GUI.DrawTexture(new Rect (Screen.width-80, Screen.height-30, 30, 30), weapons[Shooter_.activeWeapon].ammoIcon);  
		GUI.Label (new Rect (Screen.width-60, Screen.height-30, 100, 30), " " + 
		           Shooter_.weapon[Shooter_.activeWeapon].ammo.ToString());
	}
	
	

	//---------------------------------------------------------------------------------------------------------	
	// Shopping function
	void WindowFunction (int windowID) 
	{

		GUI.Label (new Rect (24, 20, windowSize.x, 30), "BUY NEW WEAPON:");
		GUI.Label (new Rect (24, 185, windowSize.x, 30), "BUY AMMUNITION:");

		for ( int w = 1; w < (weapons.Length); w++)	 
		{
			GUI.skin = skin_hud;

			GUI.Label (new Rect (30 + 115 * (w-1), 148, windowSize.x, 30),  "Ammo: " + Shooter_.weapon[w].ammo.ToString());

			// Buy ammo 	  
			if (GUI.Button (new Rect (25 + 115 * (w-1), 215, 100, 35), new GUIContent ((weapons[w].ammoCost).ToString()+ " for "+ (weapons[w].ammoAdd).ToString(), weapons[w].ammoIcon, "buy Ammo")))  
			{
				if ((Money - weapons[w].ammoCost) >=0) 
				{
					Shooter_.weapon[w].ammo += weapons[w].ammoAdd;
					Money -= weapons[w].ammoCost;
					
					PlayerPrefs.SetInt("HalloweenRange_weapon" + w.ToString() + "_ammo", Shooter_.weapon[w].ammo);
				}
			}
			
			GUI.skin = skin_gui;

			// Buy weapons 	  
			if (!Shooter_.weapon[w].enabled)	
			{   
				if (BuyButton(new Rect (25 + 115 * (w-1), 50, 100, 100), weapons[w].caption + "\n $", weapons[w].icon, weapons[w].cost )) 
					if ((Money - weapons[w].cost) >=0) 
				{
					Shooter_.weapon[w].enabled = true;
					Money -= weapons[w].cost;
					if (Shooter_.weapon[w].enabled) PlayerPrefs.SetInt("HalloweenRange_weapon" + w.ToString(), 1);
				}
			}
			else GUI.DrawTexture(new Rect (25 + 115 * (w-1), 50, 100, 100), weapons[w].icon);
		}
		GUI.Button (new Rect (25 + 115*2,50,100,120), "COMING\nSOON");
	}
	

	//---------------------------------------------------------------------------------------------------------		
	
	bool BuyButton (Rect screenRect,string caption,Texture2D icon,int cost)
	{
		
		if (GUI.Button (screenRect, new GUIContent ("", icon, "Press to BUY"))) 
		{
			return true;
		} else 
		{
			GUI.Label (new Rect (screenRect.x+5, screenRect.y+5, screenRect.width, screenRect.height), caption + cost.ToString() );
						return false;
		}

	}
	//---------------------------------------------------------------------------------------------------------	

}
