using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectManipulatorScript : MonoBehaviour
{

	public ControlArmScript Yaxis;
	public ControlArmScript Xaxis;
	public ControlArmScript Zaxis;
	public Material SelectedMaterial;

	private bool IsArmSelected;
	
	// Use this for initialization
	void Start () {
		Xaxis.RegisterMouseDownAction(SelectControlArm);
		Zaxis.RegisterMouseDownAction(SelectControlArm);
		Yaxis.RegisterMouseDownAction(SelectControlArm);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void SelectControlArm(GameObject selectedArm)
	{
		selectedArm.gameObject.GetComponent<MeshRenderer>().material = SelectedMaterial;
	}
}
