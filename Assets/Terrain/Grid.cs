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
	
	[Range(0f,1000f)]
	public float dimension = 1;
	
	[Range(0,50)]
	public int LOD = 1;
	
	private Mesh mesh;
	private List<Vector3> verts;
	private List<int> tri;
	private List<Vector2> uvs;
	private List<Vector3> norm;
	private Vector3[] normals;
	
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
	
	void MeshCreation(){
		Setup();
		
		//noise della griglia estesa
		float[,] fullNoise = Noise.NoiseMapGenerator(GetLOD+2, GetLOD+2, Spacing, scale, seed, octaves, persistance, lacunarity, offset, normalize);
		//cut-out della griglia renderizzata
		float[,] noise = new float[GetLOD+1,GetLOD+1];
		for(int y = 1; y <= GetLOD+1; y++){
			for(int x = 1; x <= GetLOD+1; x++){
				noise[x-1,y-1] = fullNoise[x,y];
			}
		}
		
		//calcoli principali mesh
		CreateMesh(noise);
		
		
		//calcolo posizioni di tutti i vertici
		//TODO: ora parte di questi vengono ricalcolati, meglio fare un cut-out come il noise
		List<Vector3> fullVerts = new List<Vector3>();
		for(int y = 0; y <= GetLOD+2; y++){
			for (int x = 0; x <= GetLOD+2; x++)
			{
				float fullHeight = fullNoise[x,y];
				Vector3 pos = new Vector3(x,fullHeight,y);
				fullVerts.Add(pos);
			}
		}
		
		//calcolo normali
		Normals(fullNoise);
		
		
		//passa informazioni alla mesh
		Create();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	float Spacing{	
		get{
			return dimension/GetLOD;
		}
	}
	
	int GetLOD{	
		get{
			return LOD+1;
		}
	}
	
	void CreateMesh(float[,] noise){

		for(int y = 0; y <= GetLOD; y++){
			for (int x = 0; x <= GetLOD; x++)
			{
				float currentHeight = noise[x,y];
				
				//verts
				Vector3 pos = new Vector3(x * Spacing,currentHeight,y * Spacing);
				verts.Add(pos);
				
				//tris
				if(x < GetLOD && y < GetLOD){
					int t1 = x + y * (GetLOD + 1);
					int t2 = t1 + 1;
					int t3 = t1 + GetLOD + 1;
					int t4 = t3 + 1;
					tri.Add(t1);
					tri.Add(t3);
					tri.Add(t2);
					tri.Add(t3);
					tri.Add(t4);
					tri.Add(t2);
				}
				
				
				//uvs
				Vector2 uv = new Vector2(x / GetLOD, y / GetLOD);
				uvs.Add(uv);
			}
		}
		
	}
	
	void Normals(float[,] fullNoise){
		
		Vector3[,] norms = new Vector3[GetLOD+3,GetLOD+3];
		for(int y = 0; y  < GetLOD+2; y++){
			for (int x = 0; x < GetLOD+2; x++)
			{
			  //Vector3 pos  = new Vector3(x * Spacing/GetLOD,currentHeight,y * Spacing/GetLOD);
				Vector3 pos1 = new Vector3( x    * Spacing, fullNoise[ x    ,  y    ],  y    * Spacing);
				Vector3 pos2 = new Vector3((x+1) * Spacing, fullNoise[(x+1) ,  y    ],  y    * Spacing);
				Vector3 pos3 = new Vector3( x    * Spacing, fullNoise[ x    , (y+1) ], (y+1) * Spacing);
				Vector3 pos4 = new Vector3((x+1) * Spacing, fullNoise[(x+1) , (y+1) ], (y+1) * Spacing);
				
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
		
		//rinormalizzazione
		int j = 0;
		for(int y = 1; y <= GetLOD+1; y++){
			for(int x = 1; x <= GetLOD+1; x++){
				normals[j] = norms[x,y].normalized;
				j++;
			}
		}
		
	}
	
	void OnDrawGizmos(){
		MeshCreation();
		
	}
	
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
		normals = new Vector3[(GetLOD+1)*(GetLOD+1)];
		norm = new List<Vector3>();
		
		mesh = new Mesh();
		mesh.name = "Chunk";
	}
}
