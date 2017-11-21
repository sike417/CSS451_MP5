using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorldController : MonoBehaviour {

    public BaseMesh SquareMesh;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateMeshVertices(float desiredVertexCount)
    {
        SquareMesh.UpdateVertexCount((int)desiredVertexCount);
    }
}
