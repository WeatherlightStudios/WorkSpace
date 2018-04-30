using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotate : MonoBehaviour {


	public float rotDamping = 1;
	
	private Vector3 lookingDir = Vector3.forward;
	private GameObject cam;
	
	void Start () {
		cam = Camera.main.gameObject;
	}
	
	void Update () {
		//get input
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		
		//projected vector of the camera on the plane
		Vector3 camProjForw = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up);
		
		//direction of movement based on inputs relative to camera/itself
		Vector3 direction = camProjForw * inputY + transform.right * inputX;
		
		//if the direction is not 0 change the currently looking direction
		//inputY is to not have the player turn around if he wants to go backwards
		if(direction.magnitude != 0 && inputY != 0)
			lookingDir = direction * inputY;
		
		//desired direction where the player should look
		Quaternion desiredLookDir = Quaternion.LookRotation(lookingDir, Vector3.up);
		//smooth lerp of the looking direction
		Quaternion lookDir = Quaternion.Lerp(transform.rotation, desiredLookDir, Time.deltaTime * rotDamping);
		//direction setting
		transform.rotation = lookDir;
	}
}
