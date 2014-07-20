using UnityEngine;
using System.Collections;

public class QuestObjectLife : MonoBehaviour 
{

	public bool isBoss = false;
	
	private float Damage = 1;
	private QuestScript questScript;
	private Shooting playerShooter;
	private EnemyScript enemyScript;

	//---------------------------------------------------------------------------------------------------------	
	void Start () 
	{
		
		questScript = GameObject.Find("Quest").GetComponent<QuestScript>();
		enemyScript = GetComponent<EnemyScript>();
		
		if (isBoss) 
		{
			enemyScript.SetLife((int)questScript.BossLife);
			playerShooter = GetComponent<EnemyScript>().player.GetComponentInChildren<Shooting>();
		}
		else
		{
			enemyScript.SetLife((int)questScript.DefenceLife);
		}
		
		
	}

	//---------------------------------------------------------------------------------------------------------	
	void Update () 
	{
		if (isBoss)  
		{
			Damage = playerShooter.weapon[playerShooter.activeWeapon].damage;
			enemyScript.SetLife((int)questScript.BossLife);
		}
		else
			enemyScript.SetLife((int)questScript.DefenceLife);
	}

	//---------------------------------------------------------------------------------------------------------	
	
	void OnCollisionEnter(Collision collision) 
	{
		
		if (collision.gameObject.tag == "bullet" || collision.gameObject.tag == "enemy")
		{
			if (isBoss)
				questScript.BossLife -= Damage;
			else
				questScript.DefenceLife -= Damage;
		}
	}
	//---------------------------------------------------------------------------------------------------------	
	
	void SetDamaging(float damage) 
	{
		Damage = damage;
		
	}
	//---------------------------------------------------------------------------------------------------------	
}
