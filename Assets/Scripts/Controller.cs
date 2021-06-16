using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public Ball ball;
    public LayerMask layerMask;
    bool stopSlider = false;
    float sliderCons = 1;
    Cannon cannon;
    private bool isCannonFired = false;


    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
        cannon = FindObjectOfType<Cannon>();
    }

    void Update()
    {


        if (Singleton.GM.finished)
        {
            if (!stopSlider)
            {
                sliderCons += Time.deltaTime * 100 * 2;
                Singleton.SLIDER.value = Mathf.PingPong(sliderCons, 100);

            }
            if (Input.GetMouseButtonDown(0) && !isCannonFired)
            {
                isCannonFired = true;
                stopSlider = true;
                cannon.Fire((Singleton.SLIDER.value - 50) * (5.3f / 50f));
            }
        }

    }
}
