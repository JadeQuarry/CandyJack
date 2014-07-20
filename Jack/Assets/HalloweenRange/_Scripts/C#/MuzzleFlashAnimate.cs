using UnityEngine;
using System.Collections;

public class MuzzleFlashAnimate : MonoBehaviour 
{
	public float scaleParam = 0.5f	;			 	// Scale parameter
	public Vector3 scaleVector = Vector3.one;  		// In which direction is muzzle scaling 
	
	//============================================================================================
	void Start()
	{
		SetVisibility (false);
	}
	
	//----------------------------------------------------------------------------------------------
	// Animate muzzle	
	void Update () 
	{
		transform.localScale = scaleVector * Random.Range(scaleParam, scaleParam*3);
		float tempZ = Random.Range(0.0f,90.0f);
		Vector3 temp = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, tempZ);
		transform.localEulerAngles = temp;
		if (light) 
			light.enabled = false;
	}
	//----------------------------------------------------------------------------------------------
	
	void SetVisibility (bool isVisible) 
	{
		renderer.enabled = isVisible;
		if (light) 
			light.enabled = isVisible;
	}
	
	//----------------------------------------------------------------------------------------------
}
