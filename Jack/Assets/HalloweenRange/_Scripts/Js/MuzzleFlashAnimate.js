//MuzzleFlashAnimate
#pragma strict
var scaleParam: float = 0.5;			 // Scale parameter
var scaleVector: Vector3 = Vector3.one;  // In which direction is muzzle scaling 

//============================================================================================

function Start()
{
  SetVisibility (false);
}

//----------------------------------------------------------------------------------------------
// Animate muzzle

function Update () {
	transform.localScale = scaleVector * Random.Range(scaleParam, scaleParam*3);
	transform.localEulerAngles.z = Random.Range(0,90.0);
	 if (light) light.enabled = false;
}

//----------------------------------------------------------------------------------------------

function SetVisibility (isVisible : boolean) {
    renderer.enabled = isVisible;
    if (light) light.enabled = isVisible;
}

//----------------------------------------------------------------------------------------------