//DrawGizmo
var iconName: String;
var gizmoGolor: Color = Color (1,0,0,.5);

//========================================================================================================

function OnDrawGizmos()
{
	// Draw debug Zone-gizmo with icon in Editor viewport
	    Gizmos.DrawIcon(transform.position, iconName);
		Gizmos.color = gizmoGolor;
		Gizmos.DrawCube (transform.position, transform.localScale);
	
}
//---------------------------------------------------------------------------------------------------------		