using UnityEngine;
using System.Collections;

public class TimeDestruction : MonoBehaviour 
{
	public float lifeTime;			// After this time object will be destroyed
	void Start () 
	{
		lifeTime = lifeTime + Time.time;	
	}

	void Update () 
	{
		if (Time.time > lifeTime) 
		{
			
			if (gameObject.tag == "bullet") 
			{
				var questScript = GameObject.Find("Quest").GetComponent<QuestScript>();
				questScript.Misses += 1;
			//	questScript.SetMisses((int)questScript.GetMisses()+ 1);
			}
			
			Destroy(gameObject); 
		}	
	}
}
