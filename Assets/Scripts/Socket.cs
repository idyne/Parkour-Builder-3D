using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour
{
    public int index;

    public void Place(Selector.Device device)
    {
        Quaternion deviceRot = device.prefab.transform.rotation;
        Quaternion rot = Quaternion.Euler(deviceRot.eulerAngles.x, device.rotAngle, deviceRot.eulerAngles.z);
        Transform dvc = Instantiate(device.prefab, transform.position + device.offset, rot).transform;
    }
}
