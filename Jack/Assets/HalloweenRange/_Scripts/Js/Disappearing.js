
var animationTime : float = 3;  // Time to dissapear
var transparency: float = 1;  	// Initial transparency

// Material should be with Transparency!
//Disappering
//========================================================================================================

function Update () {

   if ( transparency > 0 )
    {
	 transparency -= (1/animationTime) * Time.deltaTime;
	 renderer.material.color.a = transparency;
	}
}

//---------------------------------------------------------------------------------------------------------		