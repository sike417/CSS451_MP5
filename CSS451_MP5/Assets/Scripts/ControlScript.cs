using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ControlScript : MonoBehaviour
{

	public Text BoundText;
	public Slider BoundSlider;
	
	// Use this for initialization
	private void Start ()
	{
		BoundText = this.transform.FindChild("BoundValue").GetComponent<Text>();
		BoundSlider = this.transform.FindChild("Slider").GetComponent<Slider>();
		UpdateValue(BoundSlider.value);
		BoundSlider.onValueChanged.AddListener(UpdateValue);
	}

	private void UpdateValue(float value)
	{
		BoundText.text = value.ToString(CultureInfo.CurrentCulture);
	}

    public void InitSliderControl(float minValue, float maxValue, float currentValue)
    {
        BoundSlider.minValue = minValue;
        BoundSlider.maxValue = maxValue;
        BoundSlider.value = currentValue;
    }

    public void AddValueChanged(UnityAction<float> callback)
    {
        BoundSlider.onValueChanged.AddListener(callback);
    }

    public void RemoveListener(UnityAction<float> callback)
    {
        BoundSlider.onValueChanged.RemoveListener(callback);
    }
}
