using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSlideDot : MonoBehaviour
{
    public int WhatLine;
    public Router rt;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Slider Dot")
        {
            rt.nt.NoteLines_sl[WhatLine].Add(collision.gameObject);
        }
        else if (collision.tag == "SliderMiddle")
        {
           
            rt.nt.IsSlider[WhatLine] = true;
            rt.nt.sliderInfo[WhatLine] = collision.transform.parent.gameObject.GetComponent<SliderForGame>();
            rt.ng.SliderTapCheck(WhatLine);
            rt.nt.DoSliderEvent(true, collision.transform.parent.gameObject.GetComponent<SliderForGame>());
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "SliderMiddle")
        {
            
            rt.nt.IsSlider[WhatLine] = false;
            rt.nt.sliderInfo[WhatLine] = null;
            rt.ng.SliderTapCheck(WhatLine);
            rt.nt.DoSliderEvent(false, collision.transform.parent.gameObject.GetComponent<SliderForGame>());
        }
    }

}
