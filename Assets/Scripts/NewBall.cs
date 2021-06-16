using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBall : MonoBehaviour
{
    public Rigidbody rb;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AdjustPosition();
        //AdjustRotation();
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime);
    }

    private void AdjustRotation()
    {
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, transform.right, Color.black);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.right, out hit, 1))
        {
            //transform.position = hit.point + hit.normal * 0.165f;
            Vector3 newDirection = Quaternion.Euler(0, -90, 0) * -hit.normal;
            //transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(transform.position + newDirection);
            Debug.DrawRay(transform.position, newDirection, Color.blue);
        }
    }

    private void AdjustPosition()
    {
        transform.position = transform.position + transform.forward * 0.025f;
        Debug.DrawRay(transform.position, Vector3.down, Color.green);
        RaycastHit rightHit;
        if (Physics.Raycast(transform.position, transform.right, out rightHit, 1))
        {
            Vector3 newDirection = Quaternion.Euler(0, -90, 0) * -rightHit.normal;
            transform.rotation = Quaternion.LookRotation(newDirection);
            Debug.DrawRay(transform.position, newDirection, Color.blue);
            RaycastHit leftHit;
            if (Physics.Raycast(transform.position, -transform.right, out leftHit, 1))
            {
                Vector3 newPosition = (rightHit.point + leftHit.point) / 2;
                RaycastHit downHit;
                if (Physics.Raycast(transform.position, Vector3.down, out downHit, 1))
                {
                    newPosition.y = downHit.point.y + 0.1f;
                    transform.position = newPosition;
                }
            }
        }
    }
}
