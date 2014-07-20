using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour 
{
	[System.Serializable]
	public class Movement
	{
		public Vector3 	direction;  	// Direction of movement, or axis along which it will be rotated (if isRotation = true)
		public float 	speed;  		// Speed of movement/rotation
		public float 	cycleTime;   	// How long will one cycle of movement/rotation
		public float 	endTime;       	// Internal variable - your input doesn't influence on it
		public int 		cycles = 0;		// How much cycles of this movement/rotation should be
		public bool 	isRotation; 	// Is it Rotation or movement

	}
	
	/////////
	
	public int 		currentMovement;			// Current (or start) movement
	public bool 	activated 		= false; 	// Is movement activated
	public bool 	cycled 			= false;    // Is whole list of movements cycled (i.e. after las movement in list  it  starts play the  firsr)
	public Movement[] movements;    			// List of all movements
	
	private int cycle;				// Internal variable
	//=======================================================================================================
	
	void Start () 
	{
		ChangeMovement(currentMovement);
	}
	
	
	//---------------------------------------------------------------------------------------------------------	
	// Change current movement according to movements list 

	void Update () 
	{
		if (Time.timeScale == 1)
			if ( movements[currentMovement].endTime > Time.time && (movements[currentMovement].cycles != 0))
			{
				if (! movements[currentMovement].isRotation)  
				{
					transform.Translate( movements[currentMovement].direction * 
				                    movements[currentMovement].speed * Time.deltaTime, Space.World);	
				}
				else
				{
					transform.Rotate(movements[currentMovement].direction * 
				                 movements[currentMovement].speed * Time.deltaTime, Space.World);	

				}
			}else
			{
				if (cycle > 0) 
				{
					cycle --;
					ChangeMovement (currentMovement) ;
				}
				else 
				{
					if (movements[currentMovement].cycles < 0)  
						ChangeMovement (currentMovement) ;
					else if  (currentMovement < (movements.Length-1))  
							ChangeMovement (currentMovement+1) ;
					else if  (cycled) ChangeMovement (0) ;
				}
		}
		
	}
	
	//---------------------------------------------------------------------------------------------------------	
	// Change movement to another by index
	void ChangeMovement (int chMovement) 
	{
		
		if (currentMovement != chMovement) 
			cycle = movements[currentMovement].cycles; 
		
		currentMovement = chMovement;
		
		movements[currentMovement].endTime = Time.time + movements[currentMovement].cycleTime;
		
	}
	//---------------------------------------------------------------------------------------------------------
}
