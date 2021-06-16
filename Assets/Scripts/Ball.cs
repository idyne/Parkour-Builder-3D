using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Callback();
public delegate IEnumerator CallbackCoroutine();
public class Ball : MonoBehaviour
{
    Animator anim;
    public float speed = 1;
    public bool stop = false;
    public LayerMask layerMask;
    public LayerMask roadLayerMask;
    private TrailRenderer trail;
    public GameObject dieEffect;
    private Vector3 previousFramePosition;
    private float velocityMagnitude = 0;
    private bool goDown = false;
    private Vector3 holeTarget;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        trail = GetComponent<TrailRenderer>();
        anim.SetBool("Stop", stop);
        previousFramePosition = transform.position;
    }

    void Update()
    {
        velocityMagnitude = Vector3.Distance(transform.position, previousFramePosition) / Time.deltaTime;
        previousFramePosition = transform.position;

        CheckBottom();
        AdjustPosition();
        if (goDown)
            GoDown();

    }

    private void GoDown()
    {
        if (Vector3.Distance(transform.position, holeTarget) > 0.01f)
            transform.position = Vector3.MoveTowards(transform.position, holeTarget, Time.deltaTime * 5);
        else
        {
            transform.position = holeTarget;
            goDown = false;
            Invoke("StartMove", 0.3f);
        }
    }

    public void ToggleTrail()
    {
        trail.emitting = !trail.emitting;
    }


    private void AdjustPosition()
    {
        if (!stop)
        {
            transform.position = transform.position + transform.forward * 0.025f * Time.deltaTime * 10 * speed;
            Debug.DrawRay(transform.position, Vector3.down, Color.green);
            bool left = false, right = false, down = false, forward = false;
            bool leftDirect = false;
            Vector3 newPosition = transform.position;



            RaycastHit rightHit;
            if (Physics.Raycast(transform.position, transform.right, out rightHit, 1))
                right = true;
            RaycastHit leftHit;
            if (Physics.Raycast(transform.position, -transform.right, out leftHit, 1f))
                left = true;
            RaycastHit downHit;
            if (Physics.Raycast(transform.position, Vector3.down, out downHit, 0.4f))
                down = true;
            Debug.DrawRay(transform.position, transform.forward, Color.black);
            RaycastHit forwardHit;
            if (Physics.Raycast(transform.position, transform.forward, out forwardHit, 1f, roadLayerMask))
                forward = true;
            if (left && right)
            {
                if (forward)
                {
                    if (forwardHit.transform.tag == "Hole")
                    {
                        if (Vector3.Distance(transform.position, forwardHit.point) <= 0.165f)
                        {
                            holeTarget = forwardHit.point + Vector3.down * 2;
                            goDown = true;
                            StopMove();
                            return;
                        }
                    }
                }
                if (leftHit.transform.tag == "Road" && rightHit.transform.tag == "Road")
                {

                    newPosition = (rightHit.point + leftHit.point) / 2;
                }
                else if (leftHit.transform.tag == "LeftRightSwitch")
                {
                    newPosition = leftHit.point + leftHit.normal * 0.165f;
                    leftDirect = true;
                }
                else if (rightHit.transform.tag == "LeftRightSwitch")
                {
                    newPosition = rightHit.point + rightHit.normal * 0.165f;
                    leftDirect = false;
                }

            }
            else if (left)
                leftDirect = true;
            else if (right)
                leftDirect = false;


            if (!leftDirect)
            {
                Vector3 newDirection = Quaternion.Euler(0, -90, 0) * -rightHit.normal;
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            else
            {
                Vector3 newDirection = Quaternion.Euler(0, 90, 0) * -leftHit.normal;
                transform.rotation = Quaternion.LookRotation(newDirection);
            }

            if (down)
            {
                newPosition.y = downHit.point.y + 0.1f;
            }
            transform.position = newPosition;
        }
    }

    public void StopMove()
    {
        stop = true;
        anim.SetBool("Stop", stop);
    }

    public void StartMove()
    {
        stop = false;
        anim.SetBool("Stop", stop);
    }

    private void CheckBottom()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1, layerMask))
        {

            Transform other = hit.transform;
            if (other.tag == "Stop")
            {
                StopMove();
                Stop stop = other.GetComponent<Stop>();
                stop.Selection();
            }
            else if (other.tag == "Piston")
            {
                other.GetComponent<Collider>().enabled = false;
                StopMove();
                Piston piston = other.GetComponent<Piston>();
                float t = 1.3f;
                StartCoroutine(piston.Jump());
                ToggleTrail();
                ProjectileMotion.Fire(t, piston.target.position, transform, ToggleTrail);
                Invoke("StartMove", t + 0.1f);
            }
            else if (other.tag == "Mortar Stop")
            {
                float t = 0.9f;
                Mortar mortar = other.GetComponentInParent<Mortar>();
                Singleton.CAM.SetPosition(mortar.camPositions[0]);
                anim.SetTrigger("Shrink");
                StopMove();
                Vector3 targetPos = mortar.mouth.position;
                targetPos.y -= 0.068f;
                ToggleTrail();
                ProjectileMotion.Fire(t, targetPos, transform, ToggleTrail);
                StartCoroutine(Singleton.CAM.SetPositionWithDelay(mortar.camPositions[1], t + 0.3f));
                StartCoroutine(mortar.Fire(transform, 1.5f, t));
                Invoke("Expand", t + 2.1f);
                Invoke("StartMove", t + 3.6f);
                StartCoroutine(Singleton.CAM.FollowWithDelay(t + 3.6f));
                other.GetComponent<Collider>().enabled = false;
            }
            else if (other.tag == "Switch")
            {
                Switch sw = other.GetComponent<Switch>();
                sw.On();
            }
            else if (other.tag == "Finish" && !Singleton.GM.finished)
            {
                StopMove();
                Singleton.GM.Finish();
            }
        }
    }


    public void Shrink()
    {
        anim.SetTrigger("Shrink");
    }

    public void Expand()
    {
        anim.SetTrigger("Expand");
    }


    public IEnumerator Die(float delay)
    {
        yield return new WaitForSeconds(delay);
        FindObjectOfType<Controller>().ball = null;
        FindObjectOfType<DynamicCamera>().target = null;
        Instantiate(dieEffect, transform.position, dieEffect.transform.rotation);
        Singleton.GM.EndGame(2, 0);
        Destroy(gameObject);
    }


}
