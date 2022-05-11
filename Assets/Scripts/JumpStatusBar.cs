using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpStatusBar : MonoBehaviour
{
    public PlayerController PlayerController;
    public Image fillImage;
    private Slider power_bar;

    void Awake() 
    {
        power_bar = GetComponent<Slider>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (power_bar.value <= power_bar.minValue) {
            fillImage.enabled = false;
        }

        if(power_bar.value > power_bar.minValue && !fillImage.enabled) {
            fillImage.enabled = true;
        }
        float fillValue = PlayerController.jump_power / 1;
        if(fillValue <= 0.6)
        {
            fillImage.color = Color.green;
        }

        if(fillValue > 0.6 && fillValue <= 0.85)
        {
            fillImage.color = Color.yellow;
        }

        if(fillValue > 0.85)
        {
            fillImage.color = Color.red;
        }
        power_bar.value = fillValue;
    }
}
