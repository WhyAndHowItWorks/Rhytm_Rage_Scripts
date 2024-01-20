using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Column : MonoBehaviour
{
    public float minValue;
    public Text minValueText;
    public float maxValue;
    public Text maxValueText;
    public float tempValue;
   
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


     float currentvelocity;

    // Отображение
    public float TimeToChange;
     public float TargetValue;
    public Image image;
    public bool IsChanging;
    //


    
    
    public void UpdateColumn()
    {
        TargetValue = (tempValue - minValue) / (maxValue - minValue);
        IsChanging = true;
    }
    public void Update()
    {
        if (IsChanging)
        {
           //  image.fillAmount = Mathf.Lerp(oldValue, TargetValue, (TactSeconds.time-oldtime)/TimeToChange );
            image.fillAmount = Mathf.SmoothDamp(image.fillAmount, TargetValue, ref currentvelocity, TimeToChange);
            if (image.fillAmount  > TargetValue - 0.005f && image.fillAmount < TargetValue + 0.005f )
            {
                IsChanging = false;
            }
        }
    }
   
}
