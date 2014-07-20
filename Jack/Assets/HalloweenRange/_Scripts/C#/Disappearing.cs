using UnityEngine;
using System.Collections;

//----------------------------------------
// Material should be with Transparency!
//----------------------------------------
public class Disappearing : MonoBehaviour 
{
	public float animationTime 	= 3;  	// Time to dissapear
	public float transparency 	= 1;  	// Initial transparency


	void Update () 
	{
		if ( transparency > 0 )
		{
			Color color = renderer.material.color;
			transparency -= (1/animationTime) * Time.deltaTime;
			color.a = transparency;
			renderer.material.color = color;
		}
	}
}
