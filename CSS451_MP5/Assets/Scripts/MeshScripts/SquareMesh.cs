using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMesh : BaseMesh {

    const float startingPoint = -2;
    const float endingPoint = 2;

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
        theMesh.normals = NormalVectors;

        InitControllers();
        InitNormalSegments();
    }

    void CalculateVertices()
    {
        var delta = (Mathf.Abs(endingPoint) + Mathf.Abs(startingPoint)) / (MDesiredVertexCount - 1);
        int vertexIndex = 0;
        for (int zIndex = 0; zIndex < MDesiredVertexCount; zIndex++)
        {
            //calculate z position
            var zPosition = startingPoint + zIndex * delta;
            for (int xIndex = 0; xIndex < MDesiredVertexCount; xIndex++, vertexIndex++)
            {
                //calculates x position
                var xPosition = startingPoint + xIndex * delta;
                Vertices[vertexIndex] = new Vector3(xPosition, 0, zPosition);
            }
        }
    }

    void CalculateTriangles()
    {
        for (int verticesIndex = 0; verticesIndex < Mathf.Pow(MDesiredVertexCount, 2) - MDesiredVertexCount; verticesIndex++)
        {
            var adjustedIndexValue = (verticesIndex - (MDesiredVertexCount - 1));
            if (verticesIndex == MDesiredVertexCount - 1 || (adjustedIndexValue >= 0 && (adjustedIndexValue % MDesiredVertexCount) == 0))
            {
                continue;
            }
            Triangles.Add(new IndividualTriangle
            {
                pointA = verticesIndex,
                pointB = verticesIndex + MDesiredVertexCount,
                pointC = verticesIndex + MDesiredVertexCount + 1
            });
            Triangles.Add(new IndividualTriangle
            {
                pointA = verticesIndex,
                pointB = verticesIndex + MDesiredVertexCount + 1,
                pointC = verticesIndex + 1
            });
        }
    }

    protected override Vector3 CalculateFaceNormals(int point1,int point2, int point3)
    {
        var a = Vertices[point2] - Vertices[point1];
        var b = Vertices[point3] - Vertices[point1];
        return Vector3.Cross(a, b).normalized;
    }

    protected override void CalculateNormalVectors()
    {
        var triNormalVectors = new Vector3[(int)(Triangles.Count)];

        for (int index = 0; index < triNormalVectors.Length; index++)
        {
            if(index % 2 == 0)
            {
                triNormalVectors[index] = CalculateFaceNormals(Triangles[index].pointB, Triangles[index].pointC, Triangles[index].pointA);
            }
            else
            {
                triNormalVectors[index] = CalculateFaceNormals(Triangles[index].pointA, Triangles[index].pointB, Triangles[index].pointC);
            }
        }

        var trianglesPerRow = (MDesiredVertexCount - 1) * 2;

        for (int verticesIndex = 0; verticesIndex < NormalVectors.Length; verticesIndex++)
        {
            var adjustedIndexValue = (verticesIndex - (MDesiredVertexCount - 1));
            var layer = verticesIndex / MDesiredVertexCount;

            //first column
            if (verticesIndex % MDesiredVertexCount == 0)
            {
                if(verticesIndex < MDesiredVertexCount)
                {
                    //first row
                    NormalVectors[verticesIndex] = (triNormalVectors[verticesIndex] + triNormalVectors[verticesIndex + 1]).normalized; 
                }
                else if(verticesIndex >= (MDesiredVertexCount * (MDesiredVertexCount - 1)))
                {
                    NormalVectors[verticesIndex] = (triNormalVectors[trianglesPerRow * (MDesiredVertexCount - 2)]).normalized;
                    //last row
                }
                else
                {
                    var sectionOne = trianglesPerRow * (layer - 1);
                    var sectionTwo = trianglesPerRow * (layer);

                    NormalVectors[verticesIndex] = (triNormalVectors[sectionOne] + triNormalVectors[sectionTwo] + triNormalVectors[sectionTwo + 1]).normalized;
                    
                }
            }
            else if(verticesIndex == MDesiredVertexCount - 1 || (adjustedIndexValue >= 0 && (adjustedIndexValue % MDesiredVertexCount) == 0))
            {
                //last column
                if (verticesIndex < MDesiredVertexCount)
                {
                    //first row
                    NormalVectors[verticesIndex] = (triNormalVectors[trianglesPerRow - 1]).normalized;
                }
                else if (verticesIndex == (Mathf.Pow(MDesiredVertexCount, 2) - 1))
                {
                    var finalPosition = trianglesPerRow * (MDesiredVertexCount - 1);
                    NormalVectors[verticesIndex] = (triNormalVectors[finalPosition - 1] + triNormalVectors[finalPosition - 2]).normalized;
                    //last row
                }
                else
                {
                    var sectionOne = trianglesPerRow * (layer);
                    var sectionTwo = trianglesPerRow * (layer + 1);

                    NormalVectors[verticesIndex] = (triNormalVectors[sectionOne - 1] + triNormalVectors[sectionOne - 2] + triNormalVectors[sectionTwo - 1]).normalized;
                }
            }
            else
            {
                if(layer == 0)
                {
                    var startingIndex = (verticesIndex - 1) * 2 + 1;
                    NormalVectors[verticesIndex] = (triNormalVectors[startingIndex] + triNormalVectors[startingIndex + 1] + triNormalVectors[startingIndex + 2]).normalized;
                }
                else if(layer == (MDesiredVertexCount - 1))
                {
                    var startingIndex = ((verticesIndex % MDesiredVertexCount) - 1) + (layer - 1) * trianglesPerRow;
                    NormalVectors[verticesIndex] = (triNormalVectors[startingIndex] + triNormalVectors[startingIndex + 1] + triNormalVectors[startingIndex + 2]).normalized; 
                }
                else
                {
                    var sectionOne = (layer - 1) * trianglesPerRow + ((verticesIndex % MDesiredVertexCount) - 1) * 2;
                    var sectionTwo = (layer) * trianglesPerRow + ((verticesIndex % MDesiredVertexCount) - 1) * 2;

                    NormalVectors[verticesIndex] = (triNormalVectors[sectionOne] + triNormalVectors[sectionOne + 1] + triNormalVectors[sectionTwo] + triNormalVectors[sectionTwo + 1] + triNormalVectors[sectionTwo + 2]).normalized;
                }
            }
            //normalVectors[verticesIndex] = new Vector3(0, 1, 0);
        }
    }

    protected override void UpdateVerticesFromControlPoints()
    {        
        for(int verticeIndex = 0; verticeIndex < MControllers.Count; verticeIndex++)
        {
            Vertices[verticeIndex] = MControllers[verticeIndex].transform.localPosition;
        }
    }
}
