using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
{
    public Transform target;
    public CameraPosition camPos;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public IEnumerator Jump()
    {
        Singleton.CAM.SetPosition(camPos);
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(1.5f);
        Singleton.CAM.Follow();
        
    }
}
