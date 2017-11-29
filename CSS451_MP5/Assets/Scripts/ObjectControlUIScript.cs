using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectControlUIScript : MonoBehaviour {

    public Toggle T, S, R;
    public ControlScript XSlider, YSlider, ZSlider;
    public TexturePlacement controlledObject;

	// Use this for initialization
	void Start () {

        Invoke("Init", .1f);
    }

    private void Init()
    {
        T.onValueChanged.AddListener((bool isChecked) => {

            if (isChecked)
            {
                XSlider.InitSliderControl(-4, 4, 0);
                YSlider.InitSliderControl(-4, 4, 0);
                ZSlider.InitSliderControl(0, 0, 0);
            }
        });
        S.onValueChanged.AddListener((bool isChecked) => {
            if (isChecked)
            {
                XSlider.InitSliderControl(.1f, 10, 1);
                YSlider.InitSliderControl(.1f, 10, 1);
                ZSlider.InitSliderControl(1, 1, 1);
            }
        });
        R.onValueChanged.AddListener((bool isChecked) => {
            if (isChecked)
            {
                XSlider.InitSliderControl(0, 0, 0);
                YSlider.InitSliderControl(0, 0, 0);
                ZSlider.InitSliderControl(-180, 180, 0);
            }
        });

        //XSlider.onValueChanged.AddListener()

        XSlider.AddValueChanged((float value) => 
        {
            if (T.isOn)
            {
                controlledObject.Offset.x = value;
            }
            else if (S.isOn)
            {
                controlledObject.Scale.x = value;
            }
        });
        YSlider.AddValueChanged((float value) =>
        {
            if (T.isOn)
            {
                controlledObject.Offset.y = value;

            }
            else if (S.isOn)
            {
                controlledObject.Scale.y = value;

            }

        });

        ZSlider.AddValueChanged((float value) =>
        {
            if(R.isOn)
            {
                controlledObject.Rotation = value;
            }
        });

        T.isOn = true;
        R.isOn = false;
        S.isOn = false;
    }
}
