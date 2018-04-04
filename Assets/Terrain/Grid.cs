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
		float[,] noise = Noise.NoiseMapGenerator(size, size, scale, seed, octaves, persistance, lacunarity, offset, normalize);
		for(int y = 0; y < size+1; y++){
			for (int x = 0; x < size+1; x++)
			{
				float currentHeight = noise[x,y];
				Vector3 pos = new Vector3(x,currentHeight,y);
				verts.Add(pos);
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
			}
		}
		Create();
	}
	
	void Create(){
		mesh.vertices = verts.ToArray();
		mesh.triangles = tri.ToArray();
		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = mesh;
	}
	
	void Setup(){
		grid = new bool[size+1,size+1,height+1];
		
		verts = new List<Vector3>();
		tri = new List<int>();
		
		mesh = new Mesh();
	}
}
