﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneNode : MonoBehaviour {

    protected Matrix4x4 mCombinedParentXform;
    
    public Vector3 Pivot = Vector3.zero;
    public List<NodePrimitive> PrimitiveList;

    public Transform AxisFrame = null;
    private Vector3 kDefaultTreeTip = new Vector3(0.19f, 12.69f, 3.88f);

	// Use this for initialization
	protected void Start () {
        InitializeSceneNode();
        // Debug.Log("PrimitiveList:" + PrimitiveList.Count);
	}

    private void InitializeSceneNode()
    {
        mCombinedParentXform = Matrix4x4.identity;
    }

    // This must be called _BEFORE_ each draw!! 
    public void CompositeXform(ref Matrix4x4 parentXform)
    {
        Matrix4x4 pivot = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);  // Pivot translation
        Matrix4x4 invPivot = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);  // inv Pivot
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        
        mCombinedParentXform = parentXform * pivot * trs;

        // propagate to all children
        foreach (Transform child in transform)
        {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                cn.CompositeXform(ref mCombinedParentXform);
            }
        }
        
        // disenminate to primitives
        foreach (NodePrimitive p in PrimitiveList)
        {
            p.LoadShaderMatrix(ref mCombinedParentXform);
        }

        // Compute AxisFrame 
        if (AxisFrame != null)
        {
            AxisFrame.localPosition = mCombinedParentXform .MultiplyPoint(kDefaultTreeTip);
        }
    }
}