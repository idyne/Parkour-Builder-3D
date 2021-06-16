using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using RocketGM;
//using RocketFacebook;

public class LevelManager : MonoBehaviour
{
    public static LevelManager INSTANCE;

    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(gameObject);
            return;
        }
        INSTANCE = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNextLevel()
    {
        int index = Singleton.GM.level;
        if(index == 1 && PlayerPrefs.GetInt("firstLevelReported") == 0)
        {
            PlayerPrefs.SetInt("firstLevelReported", 1);
            //RocFacebookController.ReportFirstLevelCompleted();
            print("reported");
        }
        if (index >= SceneManager.sceneCountInBuildSettings)
            index = 0;
        //SceneManager.LoadScene(index);
        //RocGm.PlayerProgress.StopProgress(1);
        StartCoroutine(LoadAsynchronously(index));
    }

    public void LoadCurrentLevel()
    {
        int index = Singleton.GM.level - 1;
        //SceneManager.LoadScene(index);
        //RocGm.PlayerProgress.StopProgress(0);
        StartCoroutine(LoadAsynchronously(index));
    }

    

    public IEnumerator LoadCurrentLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        LoadCurrentLevel();
    }

    public IEnumerator LoadNextLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        LoadNextLevel();
    }

    private IEnumerator LoadAsynchronously (int sceneIndex)
    {
        Singleton.LOADING.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            yield return null;
        }
        Singleton.LOADING.SetActive(false);
    }
}
