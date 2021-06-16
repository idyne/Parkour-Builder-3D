using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Singleton : MonoBehaviour
{
    public static GameManager GM;
    public static DynamicCamera CAM;
    public static Slider SLIDER;
    public static Ball BALL;
    public static TimeController TC;
    public static GameObject LOADING;
    public static GameObject TAPTOPLAY;

    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        CAM = FindObjectOfType<DynamicCamera>();
        SLIDER = FindObjectOfType<Slider>();
        SLIDER.gameObject.SetActive(false);
        BALL = FindObjectOfType<Ball>();
        TC = FindObjectOfType<TimeController>();
        LOADING = GameObject.Find("LoadingScreen");
        TAPTOPLAY = GameObject.Find("TapToPlay");
        LOADING.SetActive(false);
    }
}
