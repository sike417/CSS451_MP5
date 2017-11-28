using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlArmScript : MonoBehaviour
{
	private bool m_isSelected = false;
	private Vector3 m_initialPoint;

	private Material m_defaultMaterial;
	public Material SelectedMaterial;
	
	
	// Use this for initialization
	public void Start ()
	{
		m_defaultMaterial = this.gameObject.GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	public void Update () {
		if (Input.GetMouseButtonUp(0) && m_isSelected)
		{
			ResetMaterial();
			m_isSelected = false;
		}
	}

	private void OnMouseDown()
	{
		if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl)) return;
		this.gameObject.GetComponent<MeshRenderer>().material = SelectedMaterial;
		m_isSelected = true;
		m_initialPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
	}

	private void OnMouseDrag()
	{
		if (!m_isSelected) return;
		var currentPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		var offset = currentPoint - m_initialPoint;

		var movementRight = Camera.main.transform.right * offset.x;
		var movementUp = Camera.main.transform.up * offset.y;
		var movementVector = Vector3.Project(movementRight + movementUp, this.transform.up) * 4f;

		this.transform.parent.localPosition += movementVector;

		m_initialPoint = currentPoint;
	}

	private void ResetMaterial()
	{
		this.gameObject.GetComponent<MeshRenderer>().material = m_defaultMaterial;
	}
}
