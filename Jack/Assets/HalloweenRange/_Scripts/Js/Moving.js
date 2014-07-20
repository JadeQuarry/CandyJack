
// Used to move object according to simple list of rules
//Moving
class Movement
{
 var direction: Vector3;  	// Direction of movement, or axis along which it will be rotated (if isRotation = true)
 var speed: float;  		// Speed of movement/rotation
 var cycleTime: float;   	// How long will one cycle of movement/rotation
 var cycles: int = 0;		// How much cycles of this movement/rotation should be
 var isRotation: boolean; 	// Is it Rotation or movement
 var endTime: float;        // Internal variable - your input doesn't influence on it
}

/////////

var currentMovement: int;		// Current (or start) movement
var activated: boolean = false; // Is movement activated
var cycled: boolean = false;    // Is whole list of movements cycled (i.e. after las movement in list  it  starts play the  firsr)
var movements : Movement[];     // List of all movements

private var cycle : int;		// Internal variable


//=======================================================================================================

function Start () {
 ChangeMovement(currentMovement);
}


//---------------------------------------------------------------------------------------------------------	
// Change current movement according to movements list 

function Update () {

if (Time.timeScale == 1)
if ( movements[currentMovement].endTime > Time.time && (movements[currentMovement].cycles != 0))
 {
   if (! movements[currentMovement].isRotation)   transform.Translate( movements[currentMovement].direction * movements[currentMovement].speed * Time.deltaTime, Space.World);	
    else
      transform.Rotate(movements[currentMovement].direction * movements[currentMovement].speed * Time.deltaTime, Space.World);	
  }
 else
   {
   	   if (cycle > 0) 
	     {
	      cycle --;
	      ChangeMovement (currentMovement) ;
	     }
	     else 
	      {
	        if (movements[currentMovement].cycles < 0)  ChangeMovement (currentMovement) ;
	         	else
	         	  if  (currentMovement < (movements.Length-1))  ChangeMovement (currentMovement+1) ;
	         	     else
	         	       if  (cycled) ChangeMovement (0) ;
	   	  }
	 }

}

//---------------------------------------------------------------------------------------------------------	
// Change movement to another by index
function ChangeMovement (chMovement : int) 
{

  if (currentMovement != chMovement) cycle = movements[currentMovement].cycles; 
  
  currentMovement = chMovement;
  
  movements[currentMovement].endTime = Time.time + movements[currentMovement].cycleTime;

}

//---------------------------------------------------------------------------------------------------------	