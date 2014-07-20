using UnityEngine;
using System.Collections;

public class Fireworks : MonoBehaviour 
{

	QuestScript questScript; 
	
	void Start () 
	{
		questScript =  GameObject.Find("Quest").GetComponent<QuestScript>();
	}
	
	
	void Update () 
	{	
		particleEmitter.emit = questScript.isSuccess() ;

		float varX = transform.position.x;
		float varY = transform.position.y;

		varX += Random.insideUnitSphere.x ;
		varY += (Random.insideUnitSphere.y) / 5.0f;

		transform.position = new Vector2 (varX, varY);
	}
}
