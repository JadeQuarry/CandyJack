
var dust : GameObject;

// Instantiate dust object
function OnCollisionEnter(collision : Collision) {
  if (dust)
    var dustClone : GameObject = Instantiate(dust, transform.position, transform.rotation);

}