//MuffGenerator
var muff : Rigidbody;	    			// Object to instanciate
var muffSpeed: float = 1.0;				// Initial speed
var RotationCorrector: float = -90;		// Correcting muff rotation (set 0 to keep rotation unchanged)

private var nextFire = 0.0;


//============================================================================================

function GenerateMuff () {

 // Create muff and push it forward
   var muffClone : Rigidbody = Instantiate(muff, transform.position, transform.rotation);
   muffClone.transform.Rotate(Vector3.up, RotationCorrector);
   muffClone.velocity = transform.forward * muffSpeed;
}

//----------------------------------------------------------------------------------------------