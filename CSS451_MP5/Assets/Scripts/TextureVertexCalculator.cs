using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TextureVertexCalculator : MonoBehaviour {

    List<GameObject> mControllers = new List<GameObject>();
    public int m_desiredVertexCount;
    const float startingPoint = -1;
    const float endingPoint = 1;

    Vector3[] vertices;
    TriangleCollection triangles = new TriangleCollection();
    Vector3[] normalVectors;
    List<LineSegment> mNormals = new List<LineSegment>();


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
        triangles.Clear();

        vertices = new Vector3[(int)Mathf.Pow(m_desiredVertexCount, 2)];
        normalVectors = new Vector3[vertices.Length];

        CalculateVertices();
        CalculateTriangles();
        CalculateNormalVectors();

        theMesh.vertices = vertices;
        theMesh.triangles = triangles;
        theMesh.normals = normalVectors;

        InitControllers();
        InitNormalSegment();
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
        for (int verticesIndex = 0; verticesIndex < Mathf.Pow(m_desiredVertexCount, 2) - m_desiredVertexCount; verticesIndex++)
        {
            var adjustedIndexValue = (verticesIndex - (m_desiredVertexCount - 1));
            if (verticesIndex == m_desiredVertexCount - 1 || (adjustedIndexValue >= 0 && (adjustedIndexValue % m_desiredVertexCount) == 0))
            {
                continue;
            }
            triangles.Add(new IndividualTriangle
            {
                pointA = verticesIndex,
                pointB = verticesIndex + m_desiredVertexCount,
                pointC = verticesIndex + m_desiredVertexCount + 1
            });
            triangles.Add(new IndividualTriangle
            {
                pointA = verticesIndex,
                pointB = verticesIndex + m_desiredVertexCount + 1,
                pointC = verticesIndex + 1
            });
        }
    }

    Vector3 CalculateFaceNormals(int point1,int point2, int point3)
    {
        var a = vertices[point2] - vertices[point1];
        var b = vertices[point3] - vertices[point1];
        return Vector3.Cross(a, b).normalized;
    }

    void CalculateNormalVectors()
    {
        var triNormalVectors = new Vector3[(int)(triangles.Count)];

        for (int index = 0; index < triNormalVectors.Length; index++)
        {
            if(index % 2 == 0)
            {
                triNormalVectors[index] = CalculateFaceNormals(triangles[index].pointB, triangles[index].pointC, triangles[index].pointA);
            }
            else
            {
                triNormalVectors[index] = CalculateFaceNormals(triangles[index].pointA, triangles[index].pointB, triangles[index].pointC);
            }
        }

        var trianglesPerRow = (m_desiredVertexCount - 1) * 2;

        for (int verticesIndex = 0; verticesIndex < normalVectors.Length; verticesIndex++)
        {
            var adjustedIndexValue = (verticesIndex - (m_desiredVertexCount - 1));
            var layer = verticesIndex / m_desiredVertexCount;

            //first column
            if (verticesIndex % m_desiredVertexCount == 0)
            {
                if(verticesIndex < m_desiredVertexCount)
                {
                    //first row
                    normalVectors[verticesIndex] = (triNormalVectors[verticesIndex] + triNormalVectors[verticesIndex + 1]).normalized; 
                }
                else if(verticesIndex >= (m_desiredVertexCount * (m_desiredVertexCount - 1)))
                {
                    normalVectors[verticesIndex] = (triNormalVectors[trianglesPerRow * (m_desiredVertexCount - 2)]).normalized;
                    //last row
                }
                else
                {
                    var sectionOne = trianglesPerRow * (layer - 1);
                    var sectionTwo = trianglesPerRow * (layer);

                    normalVectors[verticesIndex] = (triNormalVectors[sectionOne] + triNormalVectors[sectionTwo] + triNormalVectors[sectionTwo + 1]).normalized;
                    
                }
            }
            else if(verticesIndex == m_desiredVertexCount - 1 || (adjustedIndexValue >= 0 && (adjustedIndexValue % m_desiredVertexCount) == 0))
            {
                //last column
                if (verticesIndex < m_desiredVertexCount)
                {
                    //first row
                    normalVectors[verticesIndex] = (triNormalVectors[trianglesPerRow - 1]).normalized;
                }
                else if (verticesIndex == (Mathf.Pow(m_desiredVertexCount, 2) - 1))
                {
                    var finalPosition = trianglesPerRow * (m_desiredVertexCount - 1);
                    normalVectors[verticesIndex] = (triNormalVectors[finalPosition - 1] + triNormalVectors[finalPosition - 2]).normalized;
                    //last row
                }
                else
                {
                    var sectionOne = trianglesPerRow * (layer);
                    var sectionTwo = trianglesPerRow * (layer + 1);

                    normalVectors[verticesIndex] = (triNormalVectors[sectionOne - 1] + triNormalVectors[sectionOne - 2] + triNormalVectors[sectionTwo - 1]).normalized;
                }
            }
            else
            {
                if(layer == 0)
                {
                    var startingIndex = (verticesIndex - 1) * 2 + 1;
                    normalVectors[verticesIndex] = (triNormalVectors[startingIndex] + triNormalVectors[startingIndex + 1] + triNormalVectors[startingIndex + 2]).normalized;
                }
                else if(layer == (m_desiredVertexCount - 1))
                {
                    var startingIndex = ((verticesIndex % m_desiredVertexCount) - 1) + (layer - 1) * trianglesPerRow;
                    normalVectors[verticesIndex] = (triNormalVectors[startingIndex] + triNormalVectors[startingIndex + 1] + triNormalVectors[startingIndex + 2]).normalized; 
                }
                else
                {
                    var sectionOne = (layer - 1) * trianglesPerRow + ((verticesIndex % m_desiredVertexCount) - 1) * 2;
                    var sectionTwo = (layer) * trianglesPerRow + ((verticesIndex % m_desiredVertexCount) - 1) * 2;

                    normalVectors[verticesIndex] = (triNormalVectors[sectionOne] + triNormalVectors[sectionOne + 1] + triNormalVectors[sectionTwo] + triNormalVectors[sectionTwo + 1] + triNormalVectors[sectionTwo + 2]).normalized;
                }
            }
            //normalVectors[verticesIndex] = new Vector3(0, 1, 0);
        }
    }

    void InitControllers()
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

    void InitNormalSegment()
    {
        ClearNormalSegments();
        for(int index = 0; index < vertices.Length; index++)
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            mNormals.Add(o.AddComponent<LineSegment>());
            mNormals[index].SetWidth(0.05f);
            mNormals[index].transform.SetParent(this.transform);
            mNormals[index].gameObject.layer = 8;
        }

    }

    void UpdateNormals()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            mNormals[i].SetEndPoints(vertices[i], vertices[i] + 1.0f * normalVectors[i]);
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

    void ClearNormalSegments()
    {
        foreach(var lineSegment in mNormals)
        {
            Destroy(lineSegment.gameObject);
        }

        mNormals.Clear();
    }

    void UpdateVerticesFromControlPoints()
    {        
        for(int verticeIndex = 0; verticeIndex < mControllers.Count; verticeIndex++)
        {
            vertices[verticeIndex] = mControllers[verticeIndex].transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update () {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        UpdateVerticesFromControlPoints();
        theMesh.vertices = vertices;

        CalculateNormalVectors();
        theMesh.normals = normalVectors;

        UpdateNormals();
    }
}
