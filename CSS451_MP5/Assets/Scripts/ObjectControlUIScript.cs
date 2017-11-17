using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectControlUIScript : MonoBehaviour {

    public Toggle T, S, R;
    public ControlScript XSlider, YSlider, ZSlider;
    private Vector3 _previousSliderValue;
    private SceneNode _controlledGameObject;

    public SceneNode ControlledGameObject
    {
        get { return _controlledGameObject; }
        set
        {
            if (_controlledGameObject != value)
            {
                _controlledGameObject = value;
                Init();
            }
        }
    }

	// Use this for initialization
	void Start () {

        Invoke("Init", .1f);
        SetNormalVector();
        //SetToTranslation(true);
    }

    private void Init()
    {
        //T.onValueChanged.AddListener((bool isChecked) => {

        //    if (isChecked)
        //    {
        //        XSlider.InitSliderControl(-20, 20, controlledGameObject.transform.loca);
        //        YSlider.InitSliderControl(-20, 20, Barrier.transform.localPosition.y);
        //        ZSlider.InitSliderControl(-20, 20, Barrier.transform.localPosition.z);
        //    }
        //});
        //S.onValueChanged.AddListener((bool isChecked) => {
        //    if (isChecked)
        //    {
        //        XSlider.InitSliderControl(.1f, 20, Barrier.transform.localScale.x);
        //        YSlider.InitSliderControl(.1f, 20, Barrier.transform.localScale.y);
        //        ZSlider.InitSliderControl(.1f, 20, Barrier.transform.localScale.z);
        //    }
        //});
        //R.onValueChanged.AddListener((bool isChecked) => {
        //    if (isChecked)
        //    {
        //        _previousSliderValue = Vector3.zero;
        //        XSlider.InitSliderControl(-180, 180, 0);
        //        YSlider.InitSliderControl(-180, 180, 0);
        //        ZSlider.InitSliderControl(-180, 180, 0);
        //    }
        //});

        //XSlider.onValueChanged.AddListener()

        XSlider.AddValueChanged((float value) => 
        {
            if (T.isOn)
            {
                //var localPosition = Barrier.transform.localPosition;
                //localPosition.x = value;
                //Barrier.transform.localPosition = localPosition;
            }
            else if (S.isOn)
            {
                //var localScale = Barrier.transform.localScale;
                //localScale.x = value;
                //Barrier.transform.localScale = localScale;
            }
            else
            {
                //float dy = value - _previousSliderValue.x;
                //_previousSliderValue.x = value;
                //Quaternion q = Quaternion.AngleAxis(dy, Vector3.right);
                //Barrier.transform.localRotation *= q;
            }
            SetNormalVector();

        });
        YSlider.AddValueChanged((float value) =>
        {
            if (T.isOn)
            {
                //var localPosition = Barrier.transform.localPosition;
                //localPosition.y = value;
                //Barrier.transform.localPosition = localPosition;

            }
            else if (S.isOn)
            {
                //var localScale = Barrier.transform.localScale;
                //localScale.y = value;
                //Barrier.transform.localScale = localScale;

            }
            else
            {
                //float dy = value - _previousSliderValue.y;
                //_previousSliderValue.y = value;
                //Quaternion q = Quaternion.AngleAxis(dy, Vector3.up);
                //Barrier.transform.localRotation *= q;
            }
            SetNormalVector();

        });

        ZSlider.AddValueChanged((float value) =>
        {
            if (T.isOn)
            {
                //var localPosition = Barrier.transform.localPosition;
                //localPosition.z = value;
                //Barrier.transform.localPosition = localPosition;

            }
            else if (S.isOn)
            {
                //var localScale = Barrier.transform.localScale;
                //localScale.z = value;
                //Barrier.transform.localScale = localScale;
            }
            else
            {
                //float dy = value - _previousSliderValue.z;
                //_previousSliderValue.z = value;
                //Quaternion q = Quaternion.AngleAxis(dy, Vector3.forward);
                //Barrier.transform.localRotation *= q;
            }
            SetNormalVector();
        });

        T.isOn = true;
        R.isOn = false;
        S.isOn = false;
    }

    private void SetNormalVector()
    {
        //NormalVector.transform.up = Barrier.transform.forward;
        //NormalVector.transform.localPosition = Barrier.transform.localPosition - NormalVector.transform.up * 5f;

    }


    // Update is called once per frame
    void Update () {
		
	}
}
