using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public partial class BaseMesh : MonoBehaviour
{
    public struct IndividualTriangle
    {
        public int pointA;
        public int pointB;
        public int pointC;

        public List<int> ReturnPoints()
        {
            return new List<int> { pointA, pointB, pointC };
        }
    }

    public class TriangleCollection : List<IndividualTriangle>
    {
        public static implicit operator int[] (TriangleCollection rhs)
        {
            var FinalProduct = new int[rhs.Count * 3];
            int FinalProductIndex = 0;
            for (int i = 0; i < rhs.Count; i++, FinalProductIndex += 3)
            {
                FinalProduct[FinalProductIndex] = rhs[i].pointA;
                FinalProduct[FinalProductIndex + 1] = rhs[i].pointB;
                FinalProduct[FinalProductIndex + 2] = rhs[i].pointC;
            }
            return FinalProduct;
        }
    }
}
