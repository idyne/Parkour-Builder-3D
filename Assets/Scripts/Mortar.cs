using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour
{
    public Transform head;
    public Transform target;
    public Transform mouth;
    public Transform body;
    public ParticleSystem effect;
    public CameraPosition[] camPositions;
    public AudioSource audioSource;
    private Quaternion headRotation, bodyRotation;
    private bool rotate = false;
    private Transform ball;

    private void Update()
    {

        if (rotate)
        {
            float headAngle = Quaternion.Angle(head.rotation, headRotation);
            float bodyAngle = Quaternion.Angle(body.rotation, bodyRotation);
            if (headAngle < 1 && bodyAngle < 1)
                rotate = false;
            else
            {
                head.rotation = Quaternion.RotateTowards(head.rotation, headRotation, Time.deltaTime * 50);
                body.rotation = Quaternion.RotateTowards(body.rotation, bodyRotation, Time.deltaTime * 50);
                Vector3 newBallPos = mouth.position;
                newBallPos.y -= 0.068f;
                ball.position = newBallPos;
            }
        }
    }

    public void RotateHead(Vector3 rot)
    {
        rotate = true;
        Quaternion rotation = Quaternion.LookRotation(rot);
        headRotation = Quaternion.Euler(rotation.eulerAngles.x, 0, 0);
        bodyRotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }

    public IEnumerator Fire(Transform obj, float t, float delay)
    {
        ball = obj;
        yield return new WaitForSeconds(delay);
        Vector3 dif = target.position - obj.position;
        Vector3 force = new Vector3(dif.x, 0, dif.z) / t;
        force.y = dif.y / t - Physics.gravity.y * t / 2;
        RotateHead(force.normalized);
        yield return new WaitUntil(() => !rotate);
        effect.Play();
        audioSource.Play();
        Ball ballObject = obj.GetComponent<Ball>();
        Callback callback;
        if (ballObject)
        {
            ballObject.ToggleTrail();
            callback = ballObject.ToggleTrail;
        }
        else
            callback = () => { return; };
        ProjectileMotion.Fire(t, target.position, obj, callback);
    }

    IEnumerator Stop(Rigidbody rb, float t)
    {
        yield return new WaitForSeconds(t);
        rb.isKinematic = true;
        rb.isKinematic = false;
    }

    IEnumerator Col(Collider col)
    {
        col.isTrigger = true;
        yield return new WaitForSeconds(0.2f);
        col.isTrigger = false;
    }
}
