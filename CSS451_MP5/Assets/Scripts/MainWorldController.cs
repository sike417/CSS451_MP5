using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorldController : MonoBehaviour {

    public TextureVertexCalculator mesh;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateMeshVertices(float desiredVertexCount)
    {
        mesh.UpdateVertexCount((int)desiredVertexCount);
    }
}
