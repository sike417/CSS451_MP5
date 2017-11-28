using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNodeScript : MonoBehaviour
{
    private GameObject _manipulator;
    
    private Vector3 _previousPosition;
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

	// Use this for initialization
	void Start () {
        _previousPosition = this.gameObject.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        _manipulator = Instantiate(Resources.Load("DirectManipulator", typeof(GameObject))) as GameObject;
        _manipulator.transform.localPosition = this.transform.localPosition;
    }
}
