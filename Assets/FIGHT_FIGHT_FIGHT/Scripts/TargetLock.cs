using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLock : MonoBehaviour {
	
	public GameObject target;
	
	private Animator anim;
	private CharacterRotate rotate;
	private bool locked = false;
	
	// Use this for initialization
	void Start () {
		rotate = GetComponent<CharacterRotate>();
		locked = false;
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetMouseButtonDown(0)){
			GetTarget();
			locked = !locked;
		}
		
		if(target != null && locked){
			anim.SetBool("Locked", true);
			rotate.enabled = false;
			SetRotation();
		}
		else{
			anim.SetBool("Locked", false);
			rotate.enabled = true;
		}
	}
	
	void SetRotation(){
		//get direction of target
		Vector3 direction = (target.transform.position - transform.position).normalized;
		//project onto plane
		direction = Vector3.ProjectOnPlane(direction, Vector3.up);
		
		//set direction
		transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
	}
	
	void GetTarget(){
		GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
		if(targets.Length == 0){
			//no targets
			target = null;
		}
		else if(targets.Length == 1){
			//1 target
			target = targets[0];
		}
		else{
			//many targets, find closest
			int index = 0;
			float minDistance = Vector3.Distance(transform.position, targets[0].transform.position);
			for(int i = 1; i < targets.Length; i++){
				float distance = Vector3.Distance(transform.position, targets[i].transform.position);
				if(distance < minDistance){
					minDistance = distance;
					index = i;
				}
			}
			target = targets[index];
		}
	}
	
}
