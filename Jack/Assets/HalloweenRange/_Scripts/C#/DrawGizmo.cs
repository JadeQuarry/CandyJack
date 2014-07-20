using UnityEngine;
using System.Collections;

public class DrawGizmo : MonoBehaviour 
{
	public string 	iconName;
	public Color 	gizmoGolor;
	
	//========================================================================================================
	void OnDrawGizmos()
	{
		// Draw debug Zone-gizmo with icon in Editor viewport
		Gizmos.DrawIcon(transform.position, iconName);
		Gizmos.color = gizmoGolor;
		Gizmos.DrawCube (transform.position, transform.localScale);
	}
	//---------------------------------------------------------------------------------------------------------		
}
