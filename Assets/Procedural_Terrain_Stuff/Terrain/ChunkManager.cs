using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour {
	
	public GameObject chunk;
	
	public float viewDist;
	public Vector3 viewPos;
	
	[Range(0f,100f)]
	public float midRadiusPercentage;
	[Range(0f,100f)]
	public float highRadiusPercentage;
	
	public int quadSize;
	
	private List<GameObject> spawned;

	// Use this for initialization
	void Start () {
		spawned = new List<GameObject>();
		CheckBetweenRadius(0,viewDist);
	}
	
	// Update is called once per frame
	void Update () {
		//CheckBetweenRadius(viewDist-quadSize, viewDist);
	}
	
	void CheckBetweenRadius(float minRadius, float maxRadius){
		for(int y = (int)- maxRadius; y <= maxRadius; y += quadSize){
			for (int x = (int)-maxRadius; x <= maxRadius; x += quadSize)
			{
				float roundedX = viewPos.x - viewPos.x % quadSize;
				float roundedZ = viewPos.z - viewPos.z % quadSize;
				Vector3 roundedPos = new Vector3(roundedX,0,roundedZ);
				Vector3 currentPos = roundedPos + new Vector3(x - x % quadSize,0,y - y % quadSize);
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
	
	/*
	
	for(int y = (int)- maxRadius; y <= maxRadius; y += quadSize){
			for (int x = (int)-maxRadius; x <= maxRadius; x += quadSize)
			{
				float roundedX = viewPos.x - viewPos.x % quadSize;
				float roundedZ = viewPos.z - viewPos.z % quadSize;
				Vector3 roundedPos = new Vector3(roundedX,0,roundedZ);
				Vector3 currentPos = roundedPos + new Vector3(x - x % quadSize,0,y - y % quadSize);
				Vector3 distance = roundedPos - currentPos;//Vector3.Distance(currentPos, roundedPos);
				float dist = distance.x * distance.x + distance.z * distance.z;
				if(dist>= minRadius*minRadius && dist<maxRadius*maxRadius){
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
	 */
	
	GameObject FindObjAtPos(Vector3 pos){
		foreach (GameObject obj in spawned)
		{
			if(obj.transform.position == pos){
				return obj;
			}
		}
		return null;
	}
	
	float MidRadius{
		get{
			return viewDist * midRadiusPercentage / 100;
		}
	}
	
	float HighRadius{
		get{
			return MidRadius * highRadiusPercentage / 100;
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(viewPos, viewDist);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(viewPos, MidRadius);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(viewPos, HighRadius);
	}
	
}