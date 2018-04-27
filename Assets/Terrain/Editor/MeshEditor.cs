using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class MeshEditor : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		if(GUILayout.Button("Generate")){
			Grid script = (Grid)target;
			script.MeshCreation();
		}
	}
}
