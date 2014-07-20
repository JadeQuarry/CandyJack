using UnityEngine;
using System.Collections;

public class MuffGenerator : MonoBehaviour 
{
	public Rigidbody 	muff;	    					// Object to instanciate
	public float 		muffSpeed 			= 1.0f;		// Initial speed
	public float 		RotationCorrector 	= -90f;		// Correcting muff rotation (set 0 to keep rotation unchanged)

	void GenerateMuff () 
	{
		// Create muff and push it forward
		Rigidbody muffClone = Instantiate(muff, transform.position, transform.rotation)as Rigidbody;
		muffClone.transform.Rotate(Vector3.up, RotationCorrector);
		muffClone.velocity = transform.forward * muffSpeed;
		if(!muffClone.gameObject.active) 
			muffClone.gameObject.SetActiveRecursively(true);
	}
}
