using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {
	
	public int size;
	public int height;
	
	public float squaresSide;
	
	public bool[,,] grid;
	
	private Mesh mesh;
	private List<Vector3> verts;
	private List<int> tri;
	private List<Vector2> uvs;
	private List<Vector3> norm;
	private Vector3[] normals;
	
	//int mapWidth, int mapHeight, float scale, int seed, int octaves,float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode
	[Range(0.1f,100f)]
	public float scale;
	public int seed;
	[Range(1,5)]
	public int octaves;
	[Range(0.1f,10f)]
	public float persistance;
	[Range(0.001f,1f)]
	public float lacunarity;
	public Vector2 offset;
	public Noise.NormalizeMode normalize;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnDrawGizmos(){
		Setup();
		float[,] fullNoise = Noise.NoiseMapGenerator(size+2, size+2, scale, seed, octaves, persistance, lacunarity, offset, normalize);
		
		float[,] noise = new float[size+1,size+1];
		for(int y = 1; y <= size+1; y++){
			for(int x = 1; x <= size+1; x++){
				noise[x-1,y-1] = fullNoise[x,y];
			}
		}
		
		
		
		for(int y = 0; y <= size; y++){
			for (int x = 0; x <= size; x++)
			{
				float currentHeight = noise[x,y];
				
				//verts
				Vector3 pos = new Vector3(x,currentHeight,y);
				verts.Add(pos);
				
				//tris
				if(x < size && y < size){
					int t1 = x + y * size + y;
					int t2 = t1 + 1;
					int t3 = t1 + size + 1;
					int t4 = t3 + 1;
					tri.Add(t1);
					tri.Add(t3);
					tri.Add(t2);
					tri.Add(t3);
					tri.Add(t4);
					tri.Add(t2);
				}
				
				//uvs
				Vector2 uv = new Vector2(x / size, y / size);
				uvs.Add(uv);
			}
		}
		
		
		
		List<Vector3> fullVerts = new List<Vector3>();
		for(int y = 0; y <= size+2; y++){
			for (int x = 0; x <= size+2; x++)
			{
				float fullHeight = fullNoise[x,y];
				Vector3 pos = new Vector3(x,fullHeight,y);
				fullVerts.Add(pos);
			}
		}
		
		/*
		3-----4
		|	  |
		| 	  |
		1-----2
		 */
		
		Vector3[,] norms = new Vector3[size+3,size+3];
		for(int y = 0; y < size+2; y++){
			for (int x = 0; x < size+2; x++)
			{
				
				Vector3 pos1 = new Vector3(x,   fullNoise[x,  y],   y);
				Vector3 pos2 = new Vector3(x+1, fullNoise[x+1,y],   y);
				Vector3 pos3 = new Vector3(x,   fullNoise[x,  y+1], y+1);
				Vector3 pos4 = new Vector3(x+1, fullNoise[x+1,y+1], y+1);
				
				Vector3 norm1 = calculateNormal(pos1, pos3, pos2);
				Vector3 norm2 = calculateNormal(pos4, pos2, pos3);
				norms[x,   y   ] += norm1;
				norms[x+1, y   ] += norm1;
				norms[x,   y+1 ] += norm1;
				norms[x+1, y   ] += norm2;
				norms[x,   y+1 ] += norm2;
				norms[x+1, y+1 ] += norm2;
			}
		}
		
		
		int j = 0;
		for(int y = 1; y <= size+1; y++){
			for(int x = 1; x <= size+1; x++){
				normals[j] = norms[x,y];
				j++;
			}
		}
		
		for(int i = 0; i < normals.Length; i++){
			normals[i] = normals[i].normalized;
		}
		
		Create();
		
		/*
		for(int i = 0; i < fullNoise.Length-1; i++){
			int vertextIndexA = tri[i];
			int vertextIndexB = tri[i+1];
			int vertextIndexC = tri[i+2];
			Vector3 norm = calculateNormal(verts.ToArray(), vertextIndexA, vertextIndexB, vertextIndexC);
			normals[vertextIndexA] += norm;
			normals[vertextIndexB] += norm;
			normals[vertextIndexC] += norm;
		}
		 */
		
		// print("verts: " + verts.Count);
		// print("tri:" + tri.Count);
		// print("norm: " + normals.Length);
		
		
		// for(int i = 0; i < mesh.normals.Length; i++)
		// {
		// 	Gizmos.DrawRay(mesh.vertices[i] + transform.position, mesh.normals[i]);
		// }
	}
	
	/*
	//calculate normals for right area
		for(int i = 0; i < tri.Count; i+=3){
			int vertextIndexA = tri[i];
			int vertextIndexB = tri[i+1];
			int vertextIndexC = tri[i+2];
			Vector3 norm = calculateNormal(verts.ToArray(), vertextIndexA, vertextIndexB, vertextIndexC);
			normals[vertextIndexA] += norm;
			normals[vertextIndexB] += norm;
			normals[vertextIndexC] += norm;
		}
		
		//setup to calculate border normals
		List<Vector3> fullVerts = new List<Vector3>();
		for(int y = 0; y <= size+1; y++){
			for (int x = 0; x < size+1; x++)
			{
				float fullHeight = fullNoise[x,y];
				Vector3 pos = new Vector3(x,fullHeight,y);
				fullVerts.Add(pos);
			}
		}
		
		//for every vertex of the border calculate normals
		for(int y = 0; y <= size+1; y++){
			for(int x = 0; x <= size+1; x++){
				int indA = x + y*(size+2);
				int indB = indA + 1;
				int indC = indA + y*(size+2);
				int indD = indC + 1;
				
				Vector3 norm1 = calculateNormal(fullVerts.ToArray(), indA, indB, indC);
				Vector3 norm2 = calculateNormal(fullVerts.ToArray(), indD, indB, indC);
			}
		}
		
		for(int i = 0; i < normals.Length; i++){
			normals[i] = normals[i].normalized;
		}
	 */
	 
	Vector3 calculateNormal(Vector3 a, Vector3 b, Vector3 c){
		
		Vector3 sideAB = b - a;
		Vector3 sideAC = c - a;
		return Vector3.Cross(sideAB,sideAC).normalized;
	} 
	
	
	Vector3 calculateNormal(Vector3[] list, int a, int b, int c){
		Vector3 pointA = list[a];
		Vector3 pointB = list[b];
		Vector3 pointC = list[c];
		
		Vector3 sideAB = pointB - pointA;
		Vector3 sideAC = pointC - pointA;
		return Vector3.Cross(sideAB,sideAC).normalized;
	}
	
	void Create(){
		mesh.vertices = verts.ToArray();
		mesh.triangles = tri.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.normals = normals;
		mesh.RecalculateTangents();
		mesh.RecalculateBounds();
		GetComponent<MeshFilter>().mesh = mesh;
	}
	
	void Setup(){
		grid = new bool[size+1,size+1,height+1];
		
		verts = new List<Vector3>();
		tri = new List<int>();
		uvs = new List<Vector2>();
		normals = new Vector3[(size+1)*(size+1)];
		norm = new List<Vector3>();
		
		mesh = new Mesh();
		mesh.name = "Chunk";
	}
}
