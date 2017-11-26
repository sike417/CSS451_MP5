using HelperNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorldController : MonoBehaviour {

    public BaseMesh SquareMesh;
    public CylinderMeshScript CylinderMesh;

    private void Start()
    {
        SquareMesh.gameObject.SetActive(true);
        CylinderMesh.gameObject.SetActive(false);
    }

    public void UpdateMeshVertices(float desiredVertexCount)
    {
        SquareMesh.UpdateVertexCount((int)desiredVertexCount);
    }

    public void UpdateCylinderResolution(float desiredVertexCount)
    {
        CylinderMesh.UpdateVertexCount((int)desiredVertexCount);
    }

    public void UpdateCylinderRotation(float desiredRotation)
    {
        CylinderMesh.SetCylinderRotation(desiredRotation);
    }

    public void SetActiveMode(Mode activeMode)
    {
        switch (activeMode)
        {
            case Mode.Mesh:
                SquareMesh.gameObject.SetActive(true);
                CylinderMesh.gameObject.SetActive(false);
                break;
            case Mode.Cylinder:
                SquareMesh.gameObject.SetActive(false);
                CylinderMesh.gameObject.SetActive(true);
                CylinderMesh.ClearHasChangedFlag();
                break;
        }
    }
}
