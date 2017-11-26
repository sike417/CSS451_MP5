using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNodeScripts : MonoBehaviour {

    private Vector3 _previousPosition;
    public bool isTransformDirty
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
}
