using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class vn_QuestWindow : MonoBehaviour 
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
	
	public Task [] tasks;  				// List of tasks
	QuestScript questScript;			// Quest script
	private BuyMenu buyMenuObj;   		// BuyMenu script
	public GameObject  player;			// Player
	public UIFont font;
	public UILabel questContents;
	public UILabel[] tips;

	public GameObject questWindow;
	public GameObject questLable;
	public GameObject startButton;
	public GameObject cancleButton;

	private Vector2 winStartPos;
	private Vector2 winPos;
	private bool opening = true;
	private GUIText winText;
	private int reward = 0;
	private int Location0completedLevels = 0;
	private int yPos = 0;
	List <UILabel> mList = new List<UILabel>();

	void Awake()
	{
		UIEventListener.Get(startButton).onClick = StartButtonClick;
		UIEventListener.Get(cancleButton).onClick = CancleButtonClick;
	}

	void Start()
	{
		//---------------------------------------------
		player.SetActive (false);
		questScript = GameObject.Find("Quest").GetComponent<QuestScript>();

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

		questContents.text = "Here are your goals :"+"\n";
		//---------------------------------------------
		for (int i = 0; i<tasks.Length ; i++)
		{
			if (tasks[i].goalValue >= 0 ) 
			{
				int _remainValue = (int)(tasks[0].goalValue - tasks[0].currentValue);
				if(i<5)
					questContents.text += tasks[i].caption+"[99ff00]"+tasks[i].goalValue+"[-]"+ tasks[i].units+"\n" ;

				else
					questContents.text += tasks[i].caption + tasks[i].units+"\n";
			}
		}
		//---------------------------------------------
	}

	void Update () 
	{
		tasks[0].currentValue = Time.time;
		tasks[1].currentValue = questScript.Points;
		tasks[2].currentValue = questScript.Hits;
		tasks[3].currentValue = questScript.Misses;
		tasks[4].currentValue = questScript.Ammo;
		tasks[5].currentValue = questScript.DefenceLife;
		tasks[6].currentValue = questScript.BossLife;


		for(int i=0; i<tips.Length; i++)
		{
			int _remainValue = (int)(tasks[i].goalValue - tasks[i].currentValue);
			if (_remainValue <= 0) tasks[i].goalValue = -1;
			tips[i].text = tasks[i].caption+"[FF6333]"+_remainValue+"[-]"+" "+tasks[i].units;

		}
	}
	
	void StartButtonClick(GameObject button)
	{
		Screen.lockCursor = true;
		Screen.showCursor = false;

		player.SetActive(true);
		tasks[0].goalValue += tasks[0].currentValue;
		questLable.SetActive(true);
		Time.timeScale = 1;
		Debug.Log("Click Start Button");
	}
	
	void CancleButtonClick(GameObject button)
	{
		Time.timeScale = 1;
		Application.LoadLevel ("Opening");
	}
}
