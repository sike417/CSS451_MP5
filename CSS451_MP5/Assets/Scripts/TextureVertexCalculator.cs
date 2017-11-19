using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureVertexCalculator : MonoBehaviour {

    List<GameObject> mControllers = new List<GameObject>();
    public int m_desiredVertexCount;
    const float startingPoint = -2;
    const float endingPoint = 2;
    Vector3[] vertices;
    int[] triangles;
    Vector3[] normalVectors;

    // Use this for initialization
    void Start () {
    }

    public void UpdateVertexCount(int vertexCount)
    {
        m_desiredVertexCount = vertexCount;
        ClearControllers();
        InitializeMesh();
    }

    void InitializeMesh()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        theMesh.Clear();

        vertices = new Vector3[(int)Mathf.Pow(m_desiredVertexCount, 2)];
        triangles = new int[(int)Mathf.Pow(m_desiredVertexCount - 1, 2) * 2 * 3];
        normalVectors = new Vector3[vertices.Length];

        CalculateVertices();
        CalculateTriangles();
        CalculateNormalVectors();

        theMesh.vertices = vertices;
        theMesh.triangles = triangles;
        theMesh.normals = normalVectors;

        InitControllers(vertices);
    }

    void CalculateVertices()
    {
        var delta = (Mathf.Abs(endingPoint) + Mathf.Abs(startingPoint)) / (m_desiredVertexCount - 1);
        int vertexIndex = 0;
        for (int zIndex = 0; zIndex < m_desiredVertexCount; zIndex++)
        {
            //calculate z position
            var zPosition = startingPoint + zIndex * delta;
            for (int xIndex = 0; xIndex < m_desiredVertexCount; xIndex++, vertexIndex++)
            {
                //calculates x position
                var xPosition = startingPoint + xIndex * delta;
                vertices[vertexIndex] = new Vector3(xPosition, 0, zPosition);
            }
        }
    }

    void CalculateTriangles()
    {
        int triangleIndex = 0;

        for (int verticesIndex = 0; verticesIndex < Mathf.Pow(m_desiredVertexCount, 2) - m_desiredVertexCount; verticesIndex++)
        {
            var adjustedIndexValue = (verticesIndex - (m_desiredVertexCount - 1));
            if (verticesIndex == m_desiredVertexCount - 1 || (adjustedIndexValue >= 0 && (adjustedIndexValue % m_desiredVertexCount) == 0))
            {
                continue;
            }

            triangles[triangleIndex] = verticesIndex; triangles[triangleIndex + 1] = verticesIndex + m_desiredVertexCount; triangles[triangleIndex + 2] = verticesIndex + m_desiredVertexCount + 1;
            triangles[triangleIndex + 3] = verticesIndex; triangles[triangleIndex + 4] = triangles[triangleIndex + 2]; triangles[triangleIndex + 5] = verticesIndex + 1;
            triangleIndex += 6;
        }
    }

    void CalculateNormalVectors()
    {
        for (int index = 0; index < normalVectors.Length; index++)
        {
            normalVectors[index] = new Vector3(0, 1, 0);
        }
    }

    void InitControllers(Vector3[] v)
    {
        for (int i = 0; i < v.Length; i++)
        {
            mControllers.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            mControllers[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            mControllers[i].transform.localPosition = v[i];
            mControllers[i].transform.parent = this.transform;
        }
    }

    void ClearControllers()
    {
        foreach(var gameObject in mControllers)
        {
            Destroy(gameObject);
        }
        mControllers.Clear();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
