using UnityEngine;
using System.Collections;

public class Dust : MonoBehaviour 
{
	public GameObject dust;			//dust gameObject
	void OnCollisionEnter(Collision collision) 
	{
		if (dust) 
		{
			GameObject dustClone = Instantiate(dust, transform.position, transform.rotation)as GameObject;
			Destroy(dustClone, 1.0f);
		}
	}
}
