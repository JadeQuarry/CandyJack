
var Point : float;  				// Points quantity
var additionalText: String;			// Additional text to show with points (for example "headshot")
var Text: String;					// Text added to points (for example "+ $")
var PointSkin : GUISkin;			// Skin used for all texts
var PointSkinShadow : GUISkin;		// Skin used for all texts shadows
var AnimationSpeed: float = 50;
var AnimationTime : float = 3;		// How long will be points visible
var Enabled: boolean = false;

private var targY : float;
private var PointPosition : Vector3;


//========================================================================================================
function Start() {
	targY = Screen.height/2 - 100;
 }
 
//---------------------------------------------------------------------------------------------------------		

function OnGUI() {

 if (Enabled)
   {
    PointPosition = transform.position;
    var screenPos2 : Vector3 = Camera.main.camera.WorldToScreenPoint (PointPosition);
	
	 // Update transparency
	AnimationTime -= Time.deltaTime;
	GUI.color = new Color (1.0f,1.0f,1.0f, AnimationTime);
	
	 // Reder texts
		GUI.skin = PointSkinShadow;
		GUI.Label (Rect (screenPos2.x+2 , targY+2, 200, 150), additionalText + "\n"+ Text + Point.ToString());
		
		GUI.skin = PointSkin;
		GUI.Label (Rect (screenPos2.x , targY, 200, 150), additionalText + "\n" + Text + Point.ToString());
	
	 // Update vertical position
 	 targY -= Time.deltaTime*AnimationSpeed;
   }
}

//---------------------------------------------------------------------------------------------------------		

function EnablePoints( isEnabled: boolean) {
    Enabled = isEnabled;	
    
}

//---------------------------------------------------------------------------------------------------------		

function SetPoints( point: int) {

    Point = point;
}

//---------------------------------------------------------------------------------------------------------		