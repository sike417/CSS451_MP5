using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public partial class BaseMesh : MonoBehaviour
{
    protected virtual void CalculateNormalVectors()
    {
        throw new NotImplementedException();
    }

    protected virtual void UpdateNormals()
    {
        for (int i = 0; i < Vertices.Length; i++)
        {
            MNormals[i].SetEndPoints(Vertices[i], Vertices[i] + 1.0f * NormalVectors[i]);
        }
    }

    protected virtual Vector3 CalculateFaceNormals(int point1, int point2, int point3)
    {
        throw new NotImplementedException();
    }
}
