using UnityEngine;
using System.Collections;

public class Point : MonoBehaviour 
{
	public float 	Point_;  				// Points quantity
	public string 	additionalText;			// Additional text to show with points (for example "headshot")
	public string 	Text;					// Text added to points (for example "+ $")
	public GUISkin 	PointSkin;				// Skin used for all texts
	public GUISkin 	PointSkinShadow;		// Skin used for all texts shadows
	public float 	AnimationSpeed 	= 50;	// How long animation speed
	public float 	AnimationTime 	= 3;	// How long will be points visible
	public bool 	Enabled 	= false;
	public Camera 	camera;
	
	private float 	targY;
	private Vector3 PointPosition;
	private Vector3 screenPos2;
	
	//========================================================================================================
	void Start() 
	{
		targY = Screen.height/2 - 100;
		screenPos2 = Camera.main.camera.WorldToScreenPoint (PointPosition);
	}
	
	//---------------------------------------------------------------------------------------------------------		
	
	void OnGUI() 
	{
		if (Enabled)
		{
			PointPosition = transform.position;
			//modify enemy position when shooting
			if(Camera.main == null)
				screenPos2 = camera.WorldToScreenPoint (PointPosition);
			else
				screenPos2 = Camera.main.camera.WorldToScreenPoint (PointPosition);
			// Update transparency
			AnimationTime -= Time.deltaTime;
			GUI.color = new Color (1.0f,1.0f,1.0f, AnimationTime);
			
			// Reder texts
			GUI.skin = PointSkinShadow;
			GUI.Label (new Rect (screenPos2.x+2 , targY+2, 200, 150), additionalText + "\n"+ Text + Point_.ToString());
			
			GUI.skin = PointSkin;
			GUI.Label (new Rect (screenPos2.x , targY, 200, 150), additionalText + "\n" + Text + Point_.ToString());
			
			// Update vertical position
			targY -= Time.deltaTime*AnimationSpeed;
		}
	}
	
	//---------------------------------------------------------------------------------------------------------		
	
	void EnablePoints(bool isEnabled) 
	{
		Enabled = isEnabled;	
		
	}
	
	//---------------------------------------------------------------------------------------------------------		
	
	void SetPoints(int point) 
	{
		Point_= point;
	}
	
	//---------------------------------------------------------------------------------------------------------	
}
