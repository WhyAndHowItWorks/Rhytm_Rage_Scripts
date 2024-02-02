using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Column : MonoBehaviour
{
    //Min Value
    public float MinValue
    {
        get { return minValue; }
        set
        {
            minValue = value;
            if (minValueText != null)
            {
                minValueText.text = value.ToString();
            }
            
            UpdateColumn();
        }
    }
    public float minValue;
    public Text minValueText;
    //Max Value
    public float MaxValue
    {
        get { return maxValue; }
        set
        {
            maxValue = value;
            if (maxValueText != null)
            {
                maxValueText.text = value.ToString();
            }
              
            UpdateColumn();
        }
    }
    public float maxValue;
    public Text maxValueText;
    //Temp Value
    public virtual float TempValue
    {
        get { return tempValue; }
        set
        {            
            if (value < minValue)
            {
                tempValue = minValue;
            }
            else if (value > maxValue)
            {
                tempValue = maxValue;
            }
            else
            {
                tempValue = value;
            }
            
            UpdateColumn();
        }
    }
    public float tempValue;
    //Displaying
    //Start Values
    public float TimeToChange;
    public Image FillImage;
    //UpdateValues
    float currentvelocity;
    public float TargetValue;
    public bool IsChanging;
    


    public void UpdateColumn()
    {
        TargetValue = (tempValue - minValue) / (maxValue - minValue);
        IsChanging = true;
    }
    public void Update()
    {
        if (IsChanging)
        {
            FillImage.fillAmount = Mathf.SmoothDamp(FillImage.fillAmount, TargetValue, ref currentvelocity, TimeToChange);
            if (FillImage.fillAmount  > TargetValue - 0.005f && FillImage.fillAmount < TargetValue + 0.005f )
            {
                IsChanging = false;
            }
        }
    }
   
}
