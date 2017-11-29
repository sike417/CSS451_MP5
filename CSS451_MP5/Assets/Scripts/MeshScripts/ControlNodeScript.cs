using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNodeScript : MonoBehaviour
{
    private GameObject _manipulator;
    
    public Vector3 _previousPosition;
    
    public bool IsTransformDirty
    {
        get
        {
            if(this.gameObject.transform.hasChanged == true && _previousPosition != this.gameObject.transform.localPosition)
            {
                _previousPosition = this.gameObject.transform.localPosition;
                return true;
            }
            return false;
        }
        set
        {
            this.gameObject.transform.hasChanged = value;
        }
    }

    public bool IsSelectable = true;

    public BaseMesh ParentMesh;

	// Use this for initialization
	void Start () {
        _previousPosition = this.gameObject.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
	    if (_manipulator != null)
	    {
	        this.transform.localPosition = _manipulator.transform.localPosition;
	    }
	}

    public void SetPreviousPosition()
    {
        _previousPosition = this.transform.localPosition;
    }

    private void OnMouseDown()
    {
        if (_manipulator == null && IsSelectable)
        {
            _manipulator = Instantiate(Resources.Load("DirectManipulator", typeof(GameObject))) as GameObject;
            _manipulator.transform.localPosition = this.transform.localPosition;
            ParentMesh.ObservableController = this.gameObject;
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
        }
    }

    public void DestoryManipulator()
    {
        if (_manipulator == null) return;
        Destroy(_manipulator);
        _manipulator = null;
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public bool HasManipulator()
    {
        return _manipulator != null;
    }
}
