using UnityEngine;
using System.Collections;

public class EnemyEmmiter : MonoBehaviour 
{
	[System.Serializable]
	public class Enemy
	{
		public GameObject model;
		public float probability = 1;	
	}

	
	public Enemy[] Enemies;						// List of enemies
	public bool randomEnemy = false;			// Randomize enemies?
	public bool singleEnemy  = true;			// Only one live Enemy allowed
	public float generationTime = 1;			// How offten enemy should be generated ( if not singleEnemy)
	
	private GameObject singleton = null;
	private float createTime;
	private int currentEnemyType = 0;
	

	//========================================================================================================
	
	void Start () 
	{
		createTime = Time.time + generationTime;
	}
	

	//---------------------------------------------------------------------------------------------------------	
	
	void Update () 
	{
		if (Time.time > createTime)
		{
			bool created = false;
			
			// Create enemy if !singleEnemy and/or singleton object not created
			if (singleEnemy) 
			{
				if (!singleton) 
				{ 
					singleton = CreateEnemy (Enemies[currentEnemyType].model);
					created = true;
				}
			}
			else
			{
				CreateEnemy (Enemies[currentEnemyType].model);
				created = true;
			}
			
			// Randomize enemy
			if (created)    
				if(randomEnemy)
			{
				int rv = Random.Range (0, Enemies.Length);
				
				if (Random.value < Enemies[rv].probability) 
					currentEnemyType = rv;
			}    
			else
				if (currentEnemyType < (Enemies.Length-1)) currentEnemyType++; else currentEnemyType = 0;

			createTime = Time.time + generationTime;
		} 
	}

	//---------------------------------------------------------------------------------------------------------	
	// Instantiate enemy object in current coordinates
	GameObject CreateEnemy (GameObject enemyObject) 
	{
		GameObject EnemyClone = Instantiate(enemyObject, transform.position, enemyObject.transform.rotation)as GameObject;
		EnemyClone.SetActive(true);
		return EnemyClone;
	}

	//---------------------------------------------------------------------------------------------------------	
	// Draw debug Zone-gizmo in Editor viewport
	void OnDrawGizmos()
	{
		Gizmos.color = new Color (0,0,1,.5f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}
	//---------------------------------------------------------------------------------------------------------	

}
