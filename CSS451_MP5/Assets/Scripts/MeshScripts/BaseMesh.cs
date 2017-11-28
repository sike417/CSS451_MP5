using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public partial class BaseMesh : MonoBehaviour
{
    protected List<GameObject> M_Controllers = new List<GameObject>();
    protected int M_DesiredVertexCount;

    protected TriangleCollection M_Triangles = new TriangleCollection();

    protected Vector3[] M_Vertices;
    protected Vector3[] M_NormalVectors;
    protected List<LineSegment> M_Normals = new List<LineSegment>();

    public virtual void UpdateVertexCount(int vertexCount)
    {
        M_DesiredVertexCount = vertexCount;
        ClearControllers();
        InitializeMesh();
    }

    protected virtual void InitializeMesh()
    {
        throw new NotImplementedException();
    }

    protected virtual void ClearControllers()
    {
        foreach (var currentController in M_Controllers)
        {
            Destroy(currentController);
        }
        M_Controllers.Clear();
    }

    protected virtual void ClearNormalSegments()
    {
        foreach (var lineSegment in M_Normals)
        {
            Destroy(lineSegment.gameObject);
        }

        M_Normals.Clear();
    }

    protected virtual void InitNormalSegments()
    {
        ClearNormalSegments();
        for (var index = 0; index < M_Vertices.Length; index++)
        {
            var o = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            M_Normals.Add(o.AddComponent<LineSegment>());
            M_Normals[index].SetWidth(0.05f);
            M_Normals[index].transform.SetParent(this.transform);
            M_Normals[index].gameObject.layer = 8;
        }
    }

    protected virtual void InitControllers()
    {
        for (var i = 0; i < M_Vertices.Length; i++)
        {
            var instance = Instantiate(Resources.Load("Controller", typeof(GameObject))) as GameObject;
            M_Controllers.Add(instance);

            M_Controllers[i].transform.localPosition = M_Vertices[i];
            M_Controllers[i].transform.parent = this.transform;
        }
    }

    protected virtual void UpdateVerticesFromControlPoints()
    {
        throw new NotImplementedException();
    }

    protected virtual void CalculateTriangles()
    {
        throw new NotImplementedException();
    }

    protected virtual void CalculateVertices()
    {
        throw new NotImplementedException();
    }

    protected virtual void Update()
    {
        var theMesh = GetComponent<MeshFilter>().mesh;
        UpdateVerticesFromControlPoints();
        theMesh.vertices = M_Vertices;
        CalculateNormalVectors();
        theMesh.normals = M_NormalVectors;

        UpdateNormals();
    }
}
