using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Activable
{
    public ParticleSystem[] effects;
    public GameObject[] flamethrowers;
    public CameraPosition camPos;
    public override void Activate()
    {
        Singleton.CAM.SetPosition(camPos);
        StartCoroutine(Play());
        Ball ball = FindObjectOfType<Ball>();
        ball.StopMove();
        StartCoroutine(ball.Die(3));
    }

    public override void Deactivate()
    {
        foreach (ParticleSystem effect in effects)
            effect.Stop();
    }

    IEnumerator Play()
    {
        yield return new WaitUntil(() => Singleton.CAM.state == DynamicCamera.CameraState.STOPPED);
        foreach (GameObject flamethrower in flamethrowers)
            flamethrower.SetActive(true);
        foreach (ParticleSystem effect in effects)
            effect.Play();
    }

}
