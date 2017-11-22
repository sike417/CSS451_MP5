using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMeshScript : BaseMesh {
    private const float Radius = 1.5f;
    private const float Height = 4f;
    private float CylinderRotation = 360f;
    private Vector3 m_center = new Vector3();


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

        InitControllers();
    }

    void CalculateVertices()
    {
        var rotationPerVertex = CylinderRotation / m_desiredVertexCount;
        var offsetPerIndex = Height / m_desiredVertexCount;
        for (var index = 0; index < m_desiredVertexCount; index++)
        {
            var rotationAxis = Quaternion.AngleAxis(rotationPerVertex * index, Vector3.up);

            for (var zIndex = 0; zIndex < m_desiredVertexCount; zIndex++)
            {
                var verticalOffset = (Vector3.up * Height / 2) - (Vector3.up * offsetPerIndex * zIndex);
                var horizontalOffset = (rotationAxis * Vector3.right * Radius);
                vertices[(index * m_desiredVertexCount) + zIndex] = m_center + horizontalOffset + verticalOffset;
            }
        }
    }

    void CalculateTriangles()
    {
        for (var verticalVertices = 0; verticalVertices < m_desiredVertexCount - 1; verticalVertices++)
        {

            for (var horizontalVertices = 0; horizontalVertices < m_desiredVertexCount - 1; horizontalVertices++)
            {
                triangles.Add(new IndividualTriangle
                {
                    pointA = verticalVertices + (m_desiredVertexCount * horizontalVertices),
                    pointB = verticalVertices + (m_desiredVertexCount * horizontalVertices) + 1,
                    pointC = verticalVertices + (m_desiredVertexCount * horizontalVertices) + m_desiredVertexCount + 1
                });
                
                triangles.Add(new IndividualTriangle
                {
                    pointA = verticalVertices + (m_desiredVertexCount * horizontalVertices),
                    pointB = verticalVertices + (m_desiredVertexCount * horizontalVertices) + m_desiredVertexCount,
                    pointC = verticalVertices + (m_desiredVertexCount * horizontalVertices) + m_desiredVertexCount + 1
                });
            }
            
//            triangles.Add(new IndividualTriangle
//            {
//                pointA = verticalVertices,
//                pointB = verticalVertices + 1,
//                pointC = verticalVertices + m_desiredVertexCount + 1
//            });
//            
//            triangles.Add(new IndividualTriangle
//            {
//                pointA = verticalVertices,
//                pointB = verticalVertices + m_desiredVertexCount,
//                pointC = verticalVertices + m_desiredVertexCount + 1
//            });
        }
//        for (int verticesIndex = 0; verticesIndex < Mathf.Pow(m_desiredVertexCount, 2) - m_desiredVertexCount; verticesIndex++)
//        {
//            var adjustedIndexValue = (verticesIndex - (m_desiredVertexCount - 1));
//            if (verticesIndex == m_desiredVertexCount - 1 || (adjustedIndexValue >= 0 && (adjustedIndexValue % m_desiredVertexCount) == 0))
//            {
//                continue;
//            }
//            triangles.Add(new IndividualTriangle
//            {
//                pointA = verticesIndex,
//                pointB = verticesIndex + m_desiredVertexCount,
//                pointC = verticesIndex + m_desiredVertexCount + 1
//            });
//            triangles.Add(new IndividualTriangle
//            {
//                pointA = verticesIndex,
//                pointB = verticesIndex + m_desiredVertexCount + 1,
//                pointC = verticesIndex + 1
//            });
//        }
    }

    protected override Vector3 CalculateFaceNormals(int point1, int point2, int point3)
    {
        var a = vertices[point2] - vertices[point1];
        var b = vertices[point3] - vertices[point1];
        return Vector3.Cross(a, b).normalized;
    }

    protected override void CalculateNormalVectors()
    {
        for (int i = 0; i < mNormals.Count; i++)
        {
            
        }
    }

    protected override void UpdateVerticesFromControlPoints()
    {
    }
}
