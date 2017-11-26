using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CylinderMeshScript : BaseMesh {
    
    private const float Radius = 1.5f;
    private const float Height = 4f;
    private float m_cylinderRotation = 360f;
    private readonly Vector3 m_center = new Vector3();

    public void SetCylinderRotation(float rotation)
    {
        m_cylinderRotation = rotation;
        ClearControllers();
        InitializeMesh();
    }

    protected override void InitializeMesh()
    {
        var theMesh = GetComponent<MeshFilter>().mesh;
        theMesh.Clear();
        M_Triangles.Clear();

        M_Vertices = new Vector3[(int)Mathf.Pow(M_DesiredVertexCount, 2)];
        M_NormalVectors = new Vector3[M_Vertices.Length];

        CalculateVertices();
        CalculateTriangles();
        CalculateNormalVectors();
        
        theMesh.vertices = M_Vertices;
        theMesh.triangles = M_Triangles;

        InitControllers();
        InitNormalSegments();
    }

    void CalculateVertices()
    {
        var rotationPerVertex = m_cylinderRotation / (M_DesiredVertexCount - 1);
        var offsetPerIndex = Height / M_DesiredVertexCount;
        for (var index = 0; index < M_DesiredVertexCount; index++)
        {
            var rotationAxis = Quaternion.AngleAxis(rotationPerVertex * index, Vector3.up);
            var horizontalOffset = (rotationAxis * Vector3.right * Radius);

            for (var zIndex = 0; zIndex < M_DesiredVertexCount; zIndex++)
            {
                var verticalOffset = (Vector3.up * Height / 2) - (Vector3.up * offsetPerIndex * zIndex);
                M_Vertices[(index * M_DesiredVertexCount) + zIndex] = m_center + horizontalOffset + verticalOffset;
            }
        }
    }

    void CalculateTriangles()
    {
        for (var horizontalVertices = 0; horizontalVertices < M_DesiredVertexCount - 1; horizontalVertices++)
        {

            for (var verticalVertices = 0; verticalVertices < M_DesiredVertexCount - 1; verticalVertices++)
            {
                M_Triangles.Add(new IndividualTriangle
                {
                    pointA = verticalVertices + (M_DesiredVertexCount * horizontalVertices),
                    pointB = verticalVertices + (M_DesiredVertexCount * horizontalVertices) + 1,
                    pointC = verticalVertices + (M_DesiredVertexCount * horizontalVertices) + M_DesiredVertexCount + 1
                });
                
                M_Triangles.Add(new IndividualTriangle
                {
                    pointA = verticalVertices + (M_DesiredVertexCount * horizontalVertices),
                    pointB = verticalVertices + (M_DesiredVertexCount * horizontalVertices) + M_DesiredVertexCount,
                    pointC = verticalVertices + (M_DesiredVertexCount * horizontalVertices) + M_DesiredVertexCount + 1
                });
            }
        }
    }

    protected override Vector3 CalculateFaceNormals(int point1, int point2, int point3)
    {
        var sideA = M_Vertices[point2] - M_Vertices[point1];
        var sideB = M_Vertices[point3] - M_Vertices[point1];
        return Vector3.Cross(sideA, sideB).normalized;
    }

    protected override void CalculateNormalVectors()
    {
        var triNormalVectors = new Vector3[(int)(M_Triangles.Count)];

        for (var index = 0; index < triNormalVectors.Length; index++)
        {
            if (index % 2 == 0)
            {
                triNormalVectors[index] = CalculateFaceNormals(M_Triangles[index].pointA, M_Triangles[index].pointB, M_Triangles[index].pointC);
            }
            else
            {
                triNormalVectors[index] = CalculateFaceNormals(M_Triangles[index].pointC, M_Triangles[index].pointB, M_Triangles[index].pointA);
            }
        }

        for (var verticesIndex = 0; verticesIndex < M_NormalVectors.Length; verticesIndex++)
        {
            if (verticesIndex < M_DesiredVertexCount)
            {
                if (verticesIndex % M_DesiredVertexCount == 0)
                {
                    M_NormalVectors[verticesIndex] = (triNormalVectors[verticesIndex] + triNormalVectors[verticesIndex + 1]).normalized;
                }
                else if (verticesIndex % M_DesiredVertexCount == M_DesiredVertexCount - 1)
                {
                    M_NormalVectors[verticesIndex] = triNormalVectors[(M_DesiredVertexCount - 2) * 2].normalized;
                }
                else
                {
                    var startingPoint = (verticesIndex - 1) * 2;
                    M_NormalVectors[verticesIndex] = (triNormalVectors[startingPoint] + triNormalVectors[startingPoint + 2] + triNormalVectors[startingPoint + 3]).normalized;
                }
            }
            else if (verticesIndex > Mathf.Pow(M_DesiredVertexCount, 2) - M_DesiredVertexCount - 1)
            {
                if (verticesIndex % M_DesiredVertexCount == 0)
                {
                    var startingPoint = ((M_DesiredVertexCount - 1) * 2) * (M_DesiredVertexCount - 2) + 1;
                    M_NormalVectors[verticesIndex] = triNormalVectors[startingPoint].normalized;
                }
                else if (verticesIndex % M_DesiredVertexCount == M_DesiredVertexCount - 1)
                {
                    M_NormalVectors[verticesIndex] = (triNormalVectors[triNormalVectors.Length - 1] + triNormalVectors[triNormalVectors.Length - 2]).normalized;
                }
                else
                {
                    var startingPoint = ((verticesIndex - 1) % M_DesiredVertexCount) * 2 + ((M_DesiredVertexCount - 1) * 2) * (M_DesiredVertexCount - 2);
                    M_NormalVectors[verticesIndex] = (triNormalVectors[startingPoint] + triNormalVectors[startingPoint + 1] + triNormalVectors[startingPoint + 2]).normalized;
                }
            }
            else
            {
                var column = verticesIndex / M_DesiredVertexCount;
                var row = verticesIndex % M_DesiredVertexCount;

                if(verticesIndex % M_DesiredVertexCount == 0)
                {
                    var startingPoint = (column - 1) * (M_DesiredVertexCount - 1) * 2 + 1;
                    var endingPoint = startingPoint + (M_DesiredVertexCount - 1) * 2;
                    M_NormalVectors[verticesIndex] = (triNormalVectors[startingPoint] + triNormalVectors[endingPoint - 1] + triNormalVectors[endingPoint]).normalized;
                }
                else if(verticesIndex % M_DesiredVertexCount == M_DesiredVertexCount - 1)
                {
                    var startingPoint = (column - 1) * (M_DesiredVertexCount - 1) * 2 + (verticesIndex % M_DesiredVertexCount) + 1;
                    var endingPoint = startingPoint + (M_DesiredVertexCount - 1) * 2;
                    M_NormalVectors[verticesIndex] = (triNormalVectors[startingPoint] + triNormalVectors[startingPoint + 1] + triNormalVectors[endingPoint]).normalized;
                }
                else
                {
                    var startingPoint = (column - 1) * (M_DesiredVertexCount - 1) * 2 + ((verticesIndex - 1) % M_DesiredVertexCount);
                    var endingPoint = startingPoint + (M_DesiredVertexCount - 1) * 2 + 2;
                    M_NormalVectors[verticesIndex] = (triNormalVectors[startingPoint] + triNormalVectors[startingPoint + 3] + triNormalVectors[endingPoint] + triNormalVectors[endingPoint - 1] + triNormalVectors[endingPoint - 2]).normalized;
                }
            }
        }
        //        M_NormalVectors[verticesIndex] = (triNormalVectors[verticesIndex] + triNormalVectors[verticesIndex + 1]).normalized; 
    }

    protected override void UpdateVerticesFromControlPoints()
    {
    }
}
