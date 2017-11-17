﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureVertexCalculator : MonoBehaviour {

    GameObject[] mControllers;

    // Use this for initialization
    void Start () {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        theMesh.Clear();

        Vector3[] v = new Vector3[9];
        int[] t = new int[8 * 3];
        Vector3[] n = new Vector3[9];

        v[0] = new Vector3(-2, 0, -2);
        v[1] = new Vector3(0, 0, -2);
        v[2] = new Vector3(2, 0, -2);

        v[3] = new Vector3(-2, 0, 0);
        v[4] = new Vector3(0, 0, 0);
        v[5] = new Vector3(2, 0, 0);

        v[6] = new Vector3(-2, 0, 2);
        v[7] = new Vector3(0, 0, 2);
        v[8] = new Vector3(2, 0, 2);

        n[0] = new Vector3(0, 1, 0);
        n[1] = new Vector3(0, 1, 0);
        n[2] = new Vector3(0, 1, 0);
        n[3] = new Vector3(0, 1, 0);
        n[4] = new Vector3(0, 1, 0);
        n[5] = new Vector3(0, 1, 0);
        n[6] = new Vector3(0, 1, 0);
        n[7] = new Vector3(0, 1, 0);
        n[8] = new Vector3(0, 1, 0);

        // First triangle
        t[0] = 0; t[1] = 3; t[2] = 4;  // 0th triangle
        t[3] = 0; t[4] = 4; t[5] = 1;  // 1st triangle

        t[6] = 1; t[7] = 4; t[8] = 5;  // 2nd triangle
        t[9] = 1; t[10] = 5; t[11] = 2;  // 3rd triangle

        t[12] = 3; t[13] = 6; t[14] = 7;  // 4th triangle
        t[15] = 3; t[16] = 7; t[17] = 4;  // 5th triangle

        t[18] = 4; t[19] = 7; t[20] = 8;  // 6th triangle
        t[21] = 4; t[22] = 8; t[23] = 5;  // 7th triangle

        theMesh.vertices = v; //  new Vector3[3];
        theMesh.triangles = t; //  new int[3];
        theMesh.normals = n;

        InitControllers(v);
    }

    void InitControllers(Vector3[] v)
    {
        mControllers = new GameObject[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            mControllers[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            mControllers[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            mControllers[i].transform.localPosition = v[i];
            mControllers[i].transform.parent = this.transform;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
