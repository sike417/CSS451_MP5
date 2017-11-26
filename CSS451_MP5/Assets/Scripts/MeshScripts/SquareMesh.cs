using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMesh : BaseMesh {

    const float StartingPoint = -2;
    const float EndingPoint = 2;

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
        theMesh.normals = M_NormalVectors;

        InitControllers();
        InitNormalSegments();
    }

    protected override void CalculateVertices()
    {
        var delta = (Mathf.Abs(EndingPoint) + Mathf.Abs(StartingPoint)) / (M_DesiredVertexCount - 1);
        var vertexIndex = 0;
        for (var zIndex = 0; zIndex < M_DesiredVertexCount; zIndex++)
        {
            //calculate z position
            var zPosition = StartingPoint + zIndex * delta;
            for (var xIndex = 0; xIndex < M_DesiredVertexCount; xIndex++, vertexIndex++)
            {
                //calculates x position
                var xPosition = StartingPoint + xIndex * delta;
                M_Vertices[vertexIndex] = new Vector3(xPosition, 0, zPosition);
            }
        }
    }

    protected override void CalculateTriangles()
    {
        for (var verticesIndex = 0; verticesIndex < Mathf.Pow(M_DesiredVertexCount, 2) - M_DesiredVertexCount; verticesIndex++)
        {
            var adjustedIndexValue = (verticesIndex - (M_DesiredVertexCount - 1));
            if (verticesIndex == M_DesiredVertexCount - 1 || (adjustedIndexValue >= 0 && (adjustedIndexValue % M_DesiredVertexCount) == 0))
            {
                continue;
            }
            M_Triangles.Add(new IndividualTriangle
            {
                pointA = verticesIndex,
                pointB = verticesIndex + M_DesiredVertexCount,
                pointC = verticesIndex + M_DesiredVertexCount + 1
            });
            M_Triangles.Add(new IndividualTriangle
            {
                pointA = verticesIndex,
                pointB = verticesIndex + M_DesiredVertexCount + 1,
                pointC = verticesIndex + 1
            });
        }
    }

    protected override void CalculateNormalVectors()
    {
        var triNormalVectors = new Vector3[(int)(M_Triangles.Count)];

        for (var index = 0; index < triNormalVectors.Length; index++)
        {
            if(index % 2 == 0)
            {
                triNormalVectors[index] = CalculateFaceNormals(M_Triangles[index].pointB, M_Triangles[index].pointC, M_Triangles[index].pointA);
            }
            else
            {
                triNormalVectors[index] = CalculateFaceNormals(M_Triangles[index].pointA, M_Triangles[index].pointB, M_Triangles[index].pointC);
            }
        }

        var trianglesPerRow = (M_DesiredVertexCount - 1) * 2;

        for (var verticesIndex = 0; verticesIndex < M_NormalVectors.Length; verticesIndex++)
        {
            var adjustedIndexValue = (verticesIndex - (M_DesiredVertexCount - 1));
            var layer = verticesIndex / M_DesiredVertexCount;

            //first column
            if (verticesIndex % M_DesiredVertexCount == 0)
            {
                if(verticesIndex < M_DesiredVertexCount)
                {
                    //first row
                    M_NormalVectors[verticesIndex] = (triNormalVectors[verticesIndex] + triNormalVectors[verticesIndex + 1]).normalized; 
                }
                else if(verticesIndex >= (M_DesiredVertexCount * (M_DesiredVertexCount - 1)))
                {
                    M_NormalVectors[verticesIndex] = (triNormalVectors[trianglesPerRow * (M_DesiredVertexCount - 2)]).normalized;
                    //last row
                }
                else
                {
                    var sectionOne = trianglesPerRow * (layer - 1);
                    var sectionTwo = trianglesPerRow * (layer);

                    M_NormalVectors[verticesIndex] = (triNormalVectors[sectionOne] + triNormalVectors[sectionTwo] + triNormalVectors[sectionTwo + 1]).normalized;
                    
                }
            }
            else if(verticesIndex == M_DesiredVertexCount - 1 || (adjustedIndexValue >= 0 && (adjustedIndexValue % M_DesiredVertexCount) == 0))
            {
                //last column
                if (verticesIndex < M_DesiredVertexCount)
                {
                    //first row
                    M_NormalVectors[verticesIndex] = (triNormalVectors[trianglesPerRow - 1]).normalized;
                }
                else if (verticesIndex == (Mathf.Pow(M_DesiredVertexCount, 2) - 1))
                {
                    var finalPosition = trianglesPerRow * (M_DesiredVertexCount - 1);
                    M_NormalVectors[verticesIndex] = (triNormalVectors[finalPosition - 1] + triNormalVectors[finalPosition - 2]).normalized;
                    //last row
                }
                else
                {
                    var sectionOne = trianglesPerRow * (layer);
                    var sectionTwo = trianglesPerRow * (layer + 1);

                    M_NormalVectors[verticesIndex] = (triNormalVectors[sectionOne - 1] + triNormalVectors[sectionOne - 2] + triNormalVectors[sectionTwo - 1]).normalized;
                }
            }
            else
            {
                if(layer == 0)
                {
                    var startingIndex = (verticesIndex - 1) * 2 + 1;
                    M_NormalVectors[verticesIndex] = (triNormalVectors[startingIndex] + triNormalVectors[startingIndex + 1] + triNormalVectors[startingIndex + 2]).normalized;
                }
                else if(layer == (M_DesiredVertexCount - 1))
                {
                    var startingIndex = ((verticesIndex % M_DesiredVertexCount) - 1) + (layer - 1) * trianglesPerRow;
                    M_NormalVectors[verticesIndex] = (triNormalVectors[startingIndex] + triNormalVectors[startingIndex + 1] + triNormalVectors[startingIndex + 2]).normalized; 
                }
                else
                {
                    var sectionOne = (layer - 1) * trianglesPerRow + ((verticesIndex % M_DesiredVertexCount) - 1) * 2;
                    var sectionTwo = (layer) * trianglesPerRow + ((verticesIndex % M_DesiredVertexCount) - 1) * 2;

                    M_NormalVectors[verticesIndex] = (triNormalVectors[sectionOne] + triNormalVectors[sectionOne + 1] + triNormalVectors[sectionTwo] + triNormalVectors[sectionTwo + 1] + triNormalVectors[sectionTwo + 2]).normalized;
                }
            }
        }
    }

    protected override void UpdateVerticesFromControlPoints()
    {        
        for(var verticeIndex = 0; verticeIndex < M_Controllers.Count; verticeIndex++)
        {
        }
    }
}
