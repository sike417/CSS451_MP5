using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SquareMesh : BaseMesh {

    const float startingPoint = -2;
    const float endingPoint = 2;

    protected override void InitializeMesh()
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
        InitNormalSegments();
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

    protected override Vector3 CalculateFaceNormals(int point1,int point2, int point3)
    {
        var a = vertices[point2] - vertices[point1];
        var b = vertices[point3] - vertices[point1];
        return Vector3.Cross(a, b).normalized;
    }

    protected override void CalculateNormalVectors()
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

    protected override void UpdateNormals()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            mNormals[i].SetEndPoints(vertices[i], vertices[i] + 1.0f * normalVectors[i]);
        }
    }

    protected override void UpdateVerticesFromControlPoints()
    {        
        for(int verticeIndex = 0; verticeIndex < mControllers.Count; verticeIndex++)
        {
            vertices[verticeIndex] = mControllers[verticeIndex].transform.localPosition;
        }
    }
}
