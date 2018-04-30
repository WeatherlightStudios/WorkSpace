using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	
    public GameObject target;
    public float damping = 1;
    
	public float lookDist;
	
	Vector3 offset;
    
	
    void Start() {
        //save offset at start
        offset = target.transform.position - transform.position;
    }
    
    void LateUpdate() {
        //get current and desired angles
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = target.transform.eulerAngles.y;
        //smooth lerp
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);
        
        //set rotation and position of camera
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = target.transform.position - (rotation * offset);
        //projected  forward of the camera on the plane
		Vector3 camForwProj = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
		
        //look somewhat in front of the player
        transform.LookAt(target.transform.position + camForwProj * lookDist);
    }
}