using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{

    public Dart dart;
    Rigidbody ballRb;
    public Transform head;
    public Vector3 force;
    public ParticleSystem effect;
    public AudioSource audioSource;
    CannonBall cannonBall;
    public ParticleSystem warpEffect;

    private void Awake()
    {
        cannonBall = transform.GetComponentInChildren<CannonBall>();
        ballRb = cannonBall.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Singleton.GM.finished)
        {
            head.rotation = Quaternion.Euler(head.rotation.eulerAngles.x, head.rotation.eulerAngles.y, Singleton.SLIDER.value / 4);

        }

    }
    public void Fire(float h)
    {
        warpEffect.Play();
        audioSource.Play();
        Singleton.SLIDER.gameObject.SetActive(false);
        Singleton.CAM.target = ballRb.transform;
        Singleton.CAM.Follow();
        float t = 1.5f;
        Vector3 targetPos = dart.transform.position;
        targetPos.y += h;
        Vector3 dif = targetPos - transform.position;
        Vector3 force = new Vector3(dif.x, 0, dif.z) / t;
        force.y = dif.y / t - Physics.gravity.y * t / 2;
        cannonBall.stop = false;
        ballRb.isKinematic = false;
        ballRb.AddForce(force, ForceMode.Impulse);
        effect.Play();
        Singleton.TC.DoSlowMotion(3);
        Singleton.GM.EndGame(6, 1);
    }
}
