using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using RocketGM;
//using RocketFacebook;
public class Starter : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private AudioSource audioSource;
    private bool isStarted = false;

    private void Update()
    {
        if (!isStarted && Input.GetMouseButtonDown(0))
            StartGame();

    }

    IEnumerator CallCamera(float delay)
    {
        yield return new WaitForSeconds(delay);
        Singleton.CAM.Follow();
    }

    void StartGame()
    {
        Singleton.TAPTOPLAY.SetActive(false);
        //RocGm.PlayerProgress.StartProgress(Singleton.GM.level);
        effect.Play();
        audioSource.Play();
        anim.SetTrigger("Punch");
        isStarted = true;
        StartCoroutine(CallCamera(0.4f));
        Ball ball = FindObjectOfType<Ball>();
        if (ball)
            ball.StartMove();
    }
}
