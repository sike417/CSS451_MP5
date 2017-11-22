using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public partial class BaseMesh : MonoBehaviour
{
    protected List<GameObject> mControllers = new List<GameObject>();
    protected int m_desiredVertexCount;

    protected TriangleCollection triangles = new TriangleCollection();

    protected Vector3[] vertices;
    protected Vector3[] normalVectors;
    protected List<LineSegment> mNormals = new List<LineSegment>();

    public virtual void UpdateVertexCount(int vertexCount)
    {
        m_desiredVertexCount = vertexCount;
        ClearControllers();
        InitializeMesh();
    }

    protected virtual void InitializeMesh()
    {
        throw new NotImplementedException();
    }

    protected virtual void ClearControllers()
    {
        foreach (var gameObject in mControllers)
        {
            Destroy(gameObject);
        }
        mControllers.Clear();
    }

    protected virtual void ClearNormalSegments()
    {
        foreach (var lineSegment in mNormals)
        {
            Destroy(lineSegment.gameObject);
        }

        mNormals.Clear();
    }

    protected virtual void InitNormalSegments()
    {
        ClearNormalSegments();
        for (int index = 0; index < vertices.Length; index++)
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            mNormals.Add(o.AddComponent<LineSegment>());
            mNormals[index].SetWidth(0.05f);
            mNormals[index].transform.SetParent(this.transform);
            mNormals[index].gameObject.layer = 8;
        }
    }

    protected virtual void InitControllers()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            mControllers.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            mControllers[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            mControllers[i].transform.localPosition = vertices[i];
            mControllers[i].transform.parent = this.transform;
            mControllers[i].layer = 8;
        }
    }

    protected virtual void UpdateVerticesFromControlPoints()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        //Mesh theMesh = GetComponent<MeshFilter>().mesh;
        //UpdateVerticesFromControlPoints();
        //theMesh.vertices = vertices;
        //CalculateNormalVectors();
        //theMesh.normals = normalVectors;

        //UpdateNormals();
    }
}
