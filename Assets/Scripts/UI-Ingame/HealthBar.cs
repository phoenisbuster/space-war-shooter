using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slide;
    public Gradient gradient;
    public Image fill;
    public bool isScale;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaxHealth(float health)
    {
        slide.maxValue = health;
        if(isScale)
        {
            float scale = health/20 <= 2.5f? health/20 : 2.5f;
            transform.localScale = new Vector3(scale, 1, 1);
            transform.GetChild(2).localScale = new Vector3(1/scale, 1, 1);
        }
        else
        {
            slide.value = health;
        }        
        // slide.value = health;
        // fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        slide.value = health;
        fill.color = gradient.Evaluate(slide.normalizedValue);
    }
}
