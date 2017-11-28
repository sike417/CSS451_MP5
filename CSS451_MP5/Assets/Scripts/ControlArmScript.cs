using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlArmScript : MonoBehaviour
{
	private Action<GameObject> _mouseDownAction;

	private Material _defaultMaterial;
	// Use this for initialization
	void Start ()
	{
		_defaultMaterial = this.gameObject.GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RegisterMouseDownAction(Action<GameObject> desiredAction)
	{
		_mouseDownAction = desiredAction;
	}

	private void OnMouseDown()
	{
		if (_mouseDownAction != null && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
		{
			_mouseDownAction(this.gameObject);
		}
	}

	public void ResetMaterial()
	{
		this.gameObject.GetComponent<MeshRenderer>().material = _defaultMaterial;
	}
}
