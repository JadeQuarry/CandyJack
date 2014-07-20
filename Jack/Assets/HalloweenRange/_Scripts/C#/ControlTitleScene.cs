using UnityEngine;
using System.Collections;

public class ControlTitleScene : MonoBehaviour 
{
	public Transform targetPosition;
	public GameObject titleText,pressStartBtn,mainMeau;
//	public AudioSource bgmSound;

	private int titlePattern = 0;
	private float alpha = 0.5f;
	private float movinfSpeed = 2.1f;

	void Start()
	{
		alpha = 0.5f;
	}

	void Update () 
	{
		if(titlePattern == 0)
		{
			if(transform.position != targetPosition.position)
			{
				if(transform.position.z > -10)
					titleText.SetActive(true);
				transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, movinfSpeed * Time.deltaTime);
			}else
			{
				pressStartBtn.SetActive(true);
				titlePattern = 1;
			}
		}else if(titlePattern == 1)
		{
			if(Input.anyKey)
			{
				pressStartBtn.SetActive(false);
				mainMeau.SetActive(true);
			}
		}
			



		

	}
}
