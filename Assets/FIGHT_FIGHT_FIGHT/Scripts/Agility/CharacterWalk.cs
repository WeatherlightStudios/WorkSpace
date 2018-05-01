using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWalk : MonoBehaviour {
	
	
	public float speed;
	public GameObject cam;
	
	private Animator anim;
	
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		anim = GetComponent<Animator>();
	}
	
	void Update () {
		//get input
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		
		//projected vector of the camera on the plane
		Vector3 camProjForw = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up);
		
		//direction of movement based on inputs relative to camera/itself
		Vector3 direction = camProjForw * inputY + transform.right * inputX;
		
		anim.SetFloat("Speed", direction.magnitude * inputY);
		
		//translate
		transform.position += direction * speed * Time.deltaTime;
	}
}
