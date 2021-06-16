using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public Dart dart;
    public bool stop = true;
    private Cannon cannon;

    private void Awake()
    {
        cannon = GetComponentInParent<Cannon>();
    }

    private void Update()
    {
        if (!stop)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 0.4f))
            {
                stop = true;
                cannon.warpEffect.Stop();
                Singleton.CAM.SetPosition(cannon.dart.camPos);
                Singleton.TC.DoSmoothSlowMotion(1);
                //rb.isKinematic = true;
                Transform parent = hit.transform;
                parent.GetChild(0).gameObject.SetActive(true);
                parent.GetComponent<Renderer>().enabled = false;
                parent.GetComponent<Collider>().enabled = false;
                for (int i = 0; i < parent.GetChild(0).childCount; i++)
                {
                    Transform child = parent.GetChild(0).GetChild(i);
                    child.GetComponent<Rigidbody>().isKinematic = false;
                }
                Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
                for (int i = 0; i < colliders.Length; i++)
                {
                    Collider collider = colliders[i];
                    if(collider.transform.parent.parent == parent)
                    {
                        Rigidbody rb = collider.GetComponent<Rigidbody>();
                        //rb.isKinematic = false;
                        rb.AddForce((collider.transform.position - transform.position).normalized * 50, ForceMode.Impulse);
                    }
                }

            }
        }
        

    }

    IEnumerator SelfDestroy()
    {
        yield return new WaitUntil(() => Singleton.CAM.state == DynamicCamera.CameraState.STOPPED);
        Destroy(gameObject);
    }
}
