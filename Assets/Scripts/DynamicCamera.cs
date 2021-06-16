using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    public Transform target;
    public float speed = 1;
    [SerializeField] private Transform cameraPosition;
    public CameraState state = CameraState.TAKING_POSITION;
    private bool follow = false;
    private bool ease = false;
    private float initialSpeed;
    public float easeSpeed = 1;
    public CameraPosition easePosA, easePosB;
    public Transform easeTarget;

    private void Awake()
    {
        initialSpeed = speed;
    }



    private void Update()
    {
        ModifySpeed();
        if (target)
        {
            switch (state)
            {
                case CameraState.FOLLOWING:
                    Follow(target);
                    break;
                case CameraState.TAKING_POSITION:
                    if (follow)
                        Follow(target);
                    else
                        TakePosition();
                    break;
            }
        }
    }

    public void Follow()
    {
        state = CameraState.TAKING_POSITION;
        follow = true;
    }

    public IEnumerator FollowWithDelay(float t)
    {
        yield return new WaitForSeconds(t);
        state = CameraState.TAKING_POSITION;
        follow = true;
    }


    private void ModifySpeed()
    {
        if (state == CameraState.TAKING_POSITION)
            speed = initialSpeed * 2;
        else if (state == CameraState.FOLLOWING)
            speed = initialSpeed;
    }
    private void Follow(Transform target)
    {

        Ball ball = target.GetComponent<Ball>();
        if (ball)
        {
            Vector3 desiredPos = target.position - target.forward.normalized * 1.5f + Vector3.up * 1.2f;
            Quaternion desiredRot = Quaternion.LookRotation(ball.transform.forward + Vector3.down * 0.5f);
            float ballSpeed = Mathf.Clamp(ball.GetComponent<Rigidbody>().velocity.magnitude, 1, Mathf.Infinity);
            float distance = Vector3.Distance(transform.position, desiredPos);
            float angle = Quaternion.Angle(transform.rotation, desiredRot);
            float movingSpeed = Time.deltaTime * speed * (ballSpeed > 3 ? ballSpeed : 1);

            float rotatingSpeed = angle * movingSpeed / distance;
            if (state == CameraState.TAKING_POSITION && distance <= 0.1f && angle <= 1f)
                state = CameraState.FOLLOWING;
            transform.position = Vector3.MoveTowards(transform.position, desiredPos, movingSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotatingSpeed);
        }
        else
        {
            Vector3 offset = -target.forward * 3;
            if (state == CameraState.TAKING_POSITION && Vector3.Distance(transform.position, target.position + offset) <= 0.1f)
                state = CameraState.FOLLOWING;
            transform.position = Vector3.MoveTowards(transform.position, target.position + offset, Time.deltaTime * speed * 10);
        }

    }

    private void TakePosition()
    {
        float distance = Vector3.Distance(transform.position, cameraPosition.position);
        float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(cameraPosition.forward));
        float movingSpeed = Time.deltaTime * speed;
        float rotatingSpeed = angle * movingSpeed / distance;
        if (state == CameraState.TAKING_POSITION && distance <= 0.1f && angle <= 1f)
            state = CameraState.STOPPED;
        transform.position = Vector3.MoveTowards(transform.position, cameraPosition.position, movingSpeed);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(cameraPosition.forward), rotatingSpeed);
    }

    public void SetPosition(CameraPosition camPos)
    {
        cameraPosition = camPos.transform;
        state = CameraState.TAKING_POSITION;
        follow = false;
    }

    public IEnumerator SetPositionWithDelay(CameraPosition camPos, float t)
    {
        yield return new WaitForSeconds(t);
        cameraPosition = camPos.transform;
        state = CameraState.TAKING_POSITION;
        follow = false;
    }

    private void Ease()
    {
        if (!easeTarget)
        {
            if (state != CameraState.EASING_WITHOUT_TARGET)
            {
                transform.position = easePosA.transform.position;
            }
            float distance = Vector3.Distance(transform.position, easePosB.transform.position);
            float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(easePosB.transform.forward));
            float movingSpeed = Time.deltaTime * speed;
            float rotatingSpeed = angle * movingSpeed / distance;
            if (state == CameraState.TAKING_POSITION && distance <= 0.1f && angle <= 1f)
                state = CameraState.STOPPED;
            transform.position = Vector3.MoveTowards(transform.position, cameraPosition.position, movingSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(cameraPosition.forward), rotatingSpeed);
        }
    }

    public enum CameraState { STOPPED, FOLLOWING, TAKING_POSITION, EASING_WITH_TARGET, EASING_WITHOUT_TARGET };
}

