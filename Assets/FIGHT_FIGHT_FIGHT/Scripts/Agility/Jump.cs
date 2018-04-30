using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Jump : MonoBehaviour {
	
	public float jumpForce = 100;
	[Range(1,5)]
	public int jumpsNumber = 1;
	public LayerMask mask;
	
	public float halfBody;
	
	private Rigidbody rb;
	private bool jumped = false;
	private bool canJump = true;
	
	private int jumps;
	
	private RaycastHit hit;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		jumps = jumpsNumber;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) && canJump){
			canJump = false;
			jumps--;
			ExecuteJump();
		}
		else if(Input.GetKeyDown(KeyCode.Space) && jumps > 0){
			jumps--;
			ExecuteJump();
		}
	}
	
	/// <summary>
	/// OnCollisionEnter is called when this collider/rigidbody has begun
	/// touching another rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Ground"){
			canJump = true;
			jumps = jumpsNumber;
		}
	}
	
	void ExecuteJump(){
		rb.velocity = Vector3.zero;
		rb.AddForce(Vector3.up * jumpForce * 10);
	}
}
