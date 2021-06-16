using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMortar : MonoBehaviour
{
    public Vector3 rot = Vector3.zero;
    public Transform head;

    private void Update()
    {
        if (rot.magnitude >= 0.1f)
        {
            Quaternion rot1 = Quaternion.LookRotation(rot);
            Quaternion rot2 = Quaternion.LookRotation(rot);
            head.rotation = Quaternion.Euler(rot1.eulerAngles.x, 0, 0);
            transform.rotation = Quaternion.Euler(0, rot2.eulerAngles.y, 0);
        }
    }
}
