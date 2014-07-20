using UnityEngine;
using System.Collections;

public class AlphaText : MonoBehaviour 
{
	public float FadeSpeed;
	private float count;

	// Update is called once per frame
	void Update () 
	{
		count += FadeSpeed * Time.deltaTime;
		guiText.color = new Color(0.5f,0.5f,0.5f,Mathf.Sin(count)*0.5f);
	}
}
