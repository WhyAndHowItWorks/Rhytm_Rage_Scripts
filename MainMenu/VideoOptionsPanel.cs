using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoOptionsPanel : MonoBehaviour
{
    public Toggle VerSins_toogle;
    public Toggle Fullscreen_toogle;
    public Toggle DoorsAnim_toogle;
    public OptionsHandler oh;
    public OptionsHandler OH
    {
        get
        {
            if (oh != null)
            {
                return oh;
            }
            else
            {
                oh = GameObject.Find("Хранитель настроек").GetComponent<OptionsHandler>();
                return oh;
            }
        }
        set { oh = value; }
    }

    public MainOptions mo;

    public void Start()
    {
        Invoke("LateStart", 0.1f);
    }
    public void LateStart()
    {
        VerSins_toogle.SetIsOnWithoutNotify(OH.VerSins);
        Fullscreen_toogle.SetIsOnWithoutNotify(OH.FullScreen);
        DoorsAnim_toogle.SetIsOnWithoutNotify(OH.so.DoorsAnim);
    }
    public void VerSinsToogleChanged()
    {
        OH.VerSins = VerSins_toogle.isOn;
    }
    public void FullScreenToggleChanged()
    {
        OH.FullScreen = Fullscreen_toogle.isOn;
    }
    public void DoorsAnimToogleChanged()
    {
        OH.so.DoorsAnim = DoorsAnim_toogle.isOn;
        mo.Doors.gameObject.SetActive(oh.so.DoorsAnim);
        mo.Doors.IsActive = oh.so.DoorsAnim;
        mo.Doors.an.Play("Idle_Открытые");
    }

}
