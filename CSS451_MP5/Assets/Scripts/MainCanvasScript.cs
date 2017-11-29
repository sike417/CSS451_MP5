using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HelperNamespace;
using UnityEngine.Events;
using UnityEngine.UI;


public class MainCanvasScript : MonoBehaviour
{
	public GameObject M_CylinderResolutionSlider;
	public GameObject M_CylinderRotationSlider;
	public GameObject M_MeshResolutionSlider;
    public GameObject M_XFormControl;
	public Dropdown M_ModeDropdown;
    public MainWorldController M_MainWorld;
    private List<string> m_dropdownOptionList;
	
	// Use this for initialization
	void Start ()
	{
        var meshControl = M_MeshResolutionSlider.GetComponent<ControlScript>();
        var cylinderResolutionControl = M_CylinderResolutionSlider.GetComponent<ControlScript>();
        var cylinderRotationControl = M_CylinderRotationSlider.GetComponent<ControlScript>();

        //MeshControl.AddValueChanged()
        meshControl.AddValueChanged(M_MainWorld.UpdateMeshVertices);
        cylinderResolutionControl.AddValueChanged(M_MainWorld.UpdateCylinderResolution);
		cylinderRotationControl.AddValueChanged(M_MainWorld.UpdateCylinderRotation);
        meshControl.InitSliderControl(2, 20, 5);
        cylinderResolutionControl.InitSliderControl(4, 20, 10);
        cylinderRotationControl.InitSliderControl(10, 360, 275);


        m_dropdownOptionList = Enum.GetNames(typeof(Mode)).ToList();
        M_ModeDropdown.AddOptions(m_dropdownOptionList);
        M_ModeDropdown.onValueChanged.AddListener((int value) =>
        {
            SetMode((Mode)value);
        });
		
        SetMode((Mode)M_ModeDropdown.value);
	}

	public void SetMode(Mode nextMode)
	{
		switch (nextMode)
		{
			case Mode.Mesh:
				M_MeshResolutionSlider.SetActive(true);
                M_XFormControl.SetActive(true);
				M_CylinderResolutionSlider.SetActive(false);
				M_CylinderRotationSlider.SetActive(false);
				break;
			case Mode.Cylinder:
				M_MeshResolutionSlider.SetActive(false);
                M_XFormControl.SetActive(false);
				M_CylinderResolutionSlider.SetActive(true);
				M_CylinderRotationSlider.SetActive(true);
				break;
			default:
				break;
		}
        M_MainWorld.SetActiveMode(nextMode);
	}
}
