using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMeshScript : BaseMesh {
    
    private const float Radius = 1.5f;
    private const float Height = 4f;
    private float CylinderRotation = 360f;
    private Vector3 m_center = new Vector3();

    public void SetCylinderRotation(float rotation)
    {
        CylinderRotation = rotation;
        ClearControllers();
        InitializeMesh();
    }

    protected override void InitializeMesh()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        theMesh.Clear();
        Triangles.Clear();

        Vertices = new Vector3[(int)Mathf.Pow(MDesiredVertexCount, 2)];
        NormalVectors = new Vector3[Vertices.Length];

        CalculateVertices();
        CalculateTriangles();
        CalculateNormalVectors();
        
        theMesh.vertices = Vertices;
        theMesh.triangles = Triangles;

        InitControllers();
    }

    void CalculateVertices()
    {
        var rotationPerVertex = CylinderRotation / (MDesiredVertexCount - 1);
        var offsetPerIndex = Height / MDesiredVertexCount;
        for (var index = 0; index < MDesiredVertexCount; index++)
        {
            var rotationAxis = Quaternion.AngleAxis(rotationPerVertex * index, Vector3.up);

            for (var zIndex = 0; zIndex < MDesiredVertexCount; zIndex++)
            {
                var verticalOffset = (Vector3.up * Height / 2) - (Vector3.up * offsetPerIndex * zIndex);
                var horizontalOffset = (rotationAxis * Vector3.right * Radius);
                Vertices[(index * MDesiredVertexCount) + zIndex] = m_center + horizontalOffset + verticalOffset;
            }
        }
    }

    void CalculateTriangles()
    {
        for (var verticalVertices = 0; verticalVertices < MDesiredVertexCount - 1; verticalVertices++)
        {

            for (var horizontalVertices = 0; horizontalVertices < MDesiredVertexCount - 1; horizontalVertices++)
            {
                Triangles.Add(new IndividualTriangle
                {
                    pointA = verticalVertices + (MDesiredVertexCount * horizontalVertices),
                    pointB = verticalVertices + (MDesiredVertexCount * horizontalVertices) + 1,
                    pointC = verticalVertices + (MDesiredVertexCount * horizontalVertices) + MDesiredVertexCount + 1
                });
                
                Triangles.Add(new IndividualTriangle
                {
                    pointA = verticalVertices + (MDesiredVertexCount * horizontalVertices),
                    pointB = verticalVertices + (MDesiredVertexCount * horizontalVertices) + MDesiredVertexCount,
                    pointC = verticalVertices + (MDesiredVertexCount * horizontalVertices) + MDesiredVertexCount + 1
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
        var a = Vertices[point2] - Vertices[point1];
        var b = Vertices[point3] - Vertices[point1];
        return Vector3.Cross(a, b).normalized;
    }

    protected override void CalculateNormalVectors()
    {
        for (int i = 0; i < MNormals.Count; i++)
        {
            
        }
    }

    protected override void UpdateVerticesFromControlPoints()
    {
    }
}
