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
	public GameObject CylinderResolutionSlider;
	public GameObject CylinderRotationSlider;
	public GameObject MeshResolutionSlider;
    public GameObject XFormControl;
	public Dropdown ModeDropdown;
    public MainWorldController MainWorld;
    private List<string> _dropdownOptionList;
	
	// Use this for initialization
	void Start ()
	{
        var meshControl = MeshResolutionSlider.GetComponent<ControlScript>();
        var cylinderResolutionControl = CylinderResolutionSlider.GetComponent<ControlScript>();
        var cylinderRotationControl = CylinderRotationSlider.GetComponent<ControlScript>();

        //MeshControl.AddValueChanged()
        meshControl.AddValueChanged(MainWorld.UpdateMeshVertices);
        cylinderResolutionControl.AddValueChanged(MainWorld.UpdateCylinderResolution);
		cylinderRotationControl.AddValueChanged(MainWorld.UpdateCylinderRotation);
        meshControl.InitSliderControl(2, 20, 3);
        cylinderResolutionControl.InitSliderControl(4, 20, 10);
        cylinderRotationControl.InitSliderControl(10, 360, 275);


        _dropdownOptionList = Enum.GetNames(typeof(Mode)).ToList();
        ModeDropdown.AddOptions(_dropdownOptionList);
        ModeDropdown.onValueChanged.AddListener((int value) =>
        {
            SetMode((Mode)value);
        });
		
        SetMode((Mode)ModeDropdown.value);
	}

	public void SetMode(Mode nextMode)
	{
		switch (nextMode)
		{
			case Mode.Mesh:
				MeshResolutionSlider.SetActive(true);
                XFormControl.SetActive(true);
				CylinderResolutionSlider.SetActive(false);
				CylinderRotationSlider.SetActive(false);
				break;
			case Mode.Cylinder:
				MeshResolutionSlider.SetActive(false);
                XFormControl.SetActive(false);
				CylinderResolutionSlider.SetActive(true);
				CylinderRotationSlider.SetActive(true);
				break;
			default:
				break;
		}
        MainWorld.SetActiveMode(nextMode);
	}
}
