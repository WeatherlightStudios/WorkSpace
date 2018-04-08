using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour {
	
	public bool D = true;
    public bool asd = false;

	public LayerMask mask;
	public float up;
	private Vector3 startingPos = Vector3.zero;
	private Vector3 lastPosHit = Vector3.zero;
	RaycastHit hit;
	
    private void OnMouseDrag()
    {
        if (!asd)
        {
            //transform.Translate(0, 0.3f, 0);
            asd = true;
			startingPos = transform.position;
        }
		
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(camRay, out hit, Mathf.Infinity, mask)){
			transform.position = hit.point + Vector3.up * up;
			lastPosHit = hit.point;
			Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
		}
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0) &&  asd)
        {
            transform.position = new Vector3(transform.position.x, lastPosHit.y + transform.localScale.y/2, transform.position.z);
			//transform.Translate(0, -0.3f, 0);
            asd = false;
        }
    }
}
