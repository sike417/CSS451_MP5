﻿using System.Collections;
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

    public void ClearHasChangedFlag()
    {
        foreach(var controller in M_Controllers)
        {
            controller.transform.hasChanged = false;
        }
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

    protected override void CalculateVertices()
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

    protected override void CalculateTriangles()
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
    
    protected override void InitControllers()
    {
        for (var i = 0; i < M_Vertices.Length; i++)
        {
            var instance = Instantiate(Resources.Load("Controller", typeof(GameObject))) as GameObject;
            M_Controllers.Add(instance);

            M_Controllers[i].transform.localPosition = M_Vertices[i];
            if (i >= M_DesiredVertexCount)
            {
                M_Controllers[i].gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
                M_Controllers[i].GetComponent<ControlNodeScript>().IsSelectable = false;
            }
            M_Controllers[i].transform.parent = this.transform;
            M_Controllers[i].GetComponent<ControlNodeScript>().ParentMesh = this;
        }
    }

    protected override void UpdateVerticesFromControlPoints()
    {
        var manipulationList = Enumerable.Range(0, M_Controllers.Count)
            .Where(index => M_Controllers[index].GetComponent<ControlNodeScript>().IsSelectable == true).ToList();
        var rotationPerVertex = m_cylinderRotation / (M_DesiredVertexCount - 1);

        for (var index = 0; index < manipulationList.Count; index++)
        {
            var offset = M_Controllers[index].transform.position -
                         M_Controllers[index].GetComponent<ControlNodeScript>()._previousPosition;
            var offsetX = Vector3.Project(offset, Vector3.right).magnitude;
            var offsetXAxis = Vector3.Project(offset, Vector3.right).normalized;
            var offsetY = Vector3.Project(offset, Vector3.up).magnitude;
            var offsetYAxis = Vector3.Project(offset, Vector3.up).normalized;
            var offsetZ = Vector3.Project(offset, Vector3.forward).magnitude;
            var offsetZAxis = Vector3.Project(offset, Vector3.forward).normalized;
            M_Controllers[index].GetComponent<ControlNodeScript>().SetPreviousPosition();

            for (var i = 0; i < M_DesiredVertexCount; i++)
            {
                var currentController = M_Controllers[index + M_DesiredVertexCount * i];
                var rotationAxis = Quaternion.AngleAxis(rotationPerVertex * i, Vector3.up);
                currentController.transform.localPosition += rotationAxis * offsetXAxis * offsetX;
                currentController.transform.localPosition += offsetYAxis * offsetY;
                currentController.transform.localPosition += offsetZAxis * offsetZ;
            }
        }

        for(var verticeIndex = 0; verticeIndex < M_Controllers.Count; verticeIndex++)
        {
            M_Vertices[verticeIndex] = M_Controllers[verticeIndex].transform.localPosition;
        }
//        var changeList = Enumerable.Range(0, M_Controllers.Count).Where(index => M_Controllers[index].GetComponent<ControlNodeScript>().IsTransformDirty == true).ToList();
//        
//        if(changeList.Count > 0)
//        {
//            foreach(var index in changeList)
//            {
//                M_Controllers[index].transform.hasChanged = false;
//            }
//        }
    }
}
