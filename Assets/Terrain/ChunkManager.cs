using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour {
	
	public GameObject chunk;
	
	public float viewDist;
	public Vector3 viewPos;
	
	public int quadSize;
	
	private List<GameObject> spawned;
	
	// Use this for initialization
	void Start () {
		spawned = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		for(int y = (int)-viewDist; y <= viewDist; y += quadSize){
			for (int x = (int)-viewDist; x <= viewDist; x += quadSize)
			{
				float roundedX = viewPos.x - viewPos.x % quadSize;
				float roundedZ = viewPos.z - viewPos.z % quadSize;
				Vector3 roundedPos = new Vector3(roundedX,0,roundedZ);
				Vector3 currentPos = roundedPos + new Vector3(x - x % quadSize,0,y - y % quadSize);
				float dist = Vector3.Distance(currentPos, roundedPos);
				if(dist<viewDist){
					//check if quad exist
					GameObject obj = FindObjAtPos(currentPos);
					if(obj == null){
						//if it doesn't create it and set offset
						GameObject chunked = Instantiate(chunk, currentPos, Quaternion.identity);
						spawned.Add(chunked);
						chunked.GetComponent<Grid>().offset = new Vector2(currentPos.x, currentPos.z);
					}
					else{
						//if it does, activate it
						obj.SetActive(true);
					}
				}
			}
		}
	}
	
	GameObject FindObjAtPos(Vector3 pos){
		foreach (GameObject obj in spawned)
		{
			if(obj.transform.position == pos){
				return obj;
			}
		}
		return null;
	}
	
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(viewPos, viewDist);
	}
	
}