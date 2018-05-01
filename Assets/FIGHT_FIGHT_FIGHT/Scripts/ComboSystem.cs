using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour {
	
	public GameObject leftHand;
	public GameObject rightHand;
	
	private Animator anim;
	
	private BoxCollider col_LH;
	private BoxCollider col_RH;
	
	void Start () {
		anim = GetComponent<Animator>();
		col_RH = rightHand.GetComponent<BoxCollider>();
		col_RH.enabled = false;
		col_LH = leftHand.GetComponent<BoxCollider>();
		col_LH.enabled = false;
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			anim.SetTrigger("Punch");
		}
	}
	
	void EnableTrigger(string s){
		if(s == "L")
			col_LH.enabled = true;
		else if(s == "R")
			col_RH.enabled = true;
	}
	
	void DisableTrigger(string s){
		if(s == "L")
			col_LH.enabled = false;
		else if(s == "R")
			col_RH.enabled = false;
	}
	
}
