using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public partial class BaseMesh : MonoBehaviour
{
    protected List<GameObject> MControllers = new List<GameObject>();
    protected int MDesiredVertexCount;

    protected TriangleCollection Triangles = new TriangleCollection();

    protected Vector3[] Vertices;
    protected Vector3[] NormalVectors;
    protected List<LineSegment> MNormals = new List<LineSegment>();

    public virtual void UpdateVertexCount(int vertexCount)
    {
        MDesiredVertexCount = vertexCount;
        ClearControllers();
        InitializeMesh();
    }

    protected virtual void InitializeMesh()
    {
        throw new NotImplementedException();
    }

    protected virtual void ClearControllers()
    {
        foreach (var currentController in MControllers)
        {
            Destroy(currentController);
        }
        MControllers.Clear();
    }

    protected virtual void ClearNormalSegments()
    {
        foreach (var lineSegment in MNormals)
        {
            Destroy(lineSegment.gameObject);
        }

        MNormals.Clear();
    }

    protected virtual void InitNormalSegments()
    {
        ClearNormalSegments();
        for (var index = 0; index < Vertices.Length; index++)
        {
            var o = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            MNormals.Add(o.AddComponent<LineSegment>());
            MNormals[index].SetWidth(0.05f);
            MNormals[index].transform.SetParent(this.transform);
            MNormals[index].gameObject.layer = 8;
        }
    }

    protected virtual void InitControllers()
    {
        for (var i = 0; i < Vertices.Length; i++)
        {
            MControllers.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            MControllers[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            MControllers[i].transform.localPosition = Vertices[i];
            MControllers[i].transform.parent = this.transform;
            MControllers[i].layer = 8;
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
