using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using RocketGM;
//using RocketFacebook;

public class GameManager : MonoBehaviour
{
    Cannon cannon;
    public bool finished = false;
    public int level = 1;

    private void Awake()
    {
        cannon = FindObjectOfType<Cannon>();
        if (LevelManager.INSTANCE == null)
        {
            GameObject levelManager = new GameObject("LevelManager");
            levelManager.AddComponent<LevelManager>();
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            PlayerPrefs.DeleteAll();
        if (Input.GetKeyDown(KeyCode.K))
            ScreenCapture.CaptureScreenshot("D:\\parkur.png");
    }

    public void Finish()
    {
        finished = true;
        Singleton.CAM.SetPosition(cannon.GetComponentInChildren<CameraPosition>());
        Singleton.SLIDER.gameObject.SetActive(true);
    }

    public void EndGame(float delay, int status)
    {
        if (status == 0) // fail
        {
            StartCoroutine(LevelManager.INSTANCE.LoadCurrentLevelWithDelay(delay));
        }
        else if (status == 1)
        {
            StartCoroutine(LevelManager.INSTANCE.LoadNextLevelWithDelay(delay));

        }
    }
}
