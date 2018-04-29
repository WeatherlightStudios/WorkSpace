using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	
	public float speed = 1;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");
		float vertical = Input.GetAxis("Height");
		
		Vector3 movement = new Vector3(inputX, vertical, inputY).normalized;
		transform.position += movement * speed * Time.deltaTime;
	}
}
