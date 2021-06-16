using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public GameObject selections;
    public Device[] devices;
    public Option[] options;

    [System.Serializable]
    public class Option
    {
        public Socket socket;
        public int deviceIndex;
        public float rotAngle;

    }

    [System.Serializable]
    public class Device
    {
        public Vector3 offset;
        public GameObject prefab;
        [HideInInspector] public float rotAngle;
    }

    public void Select(int optionIndex)
    {
        for (int i = 0; i < selections.transform.childCount; i++)
        {
            selections.transform.GetChild(i).gameObject.SetActive(false);
        }
        Option option = options[optionIndex];
        Socket socket = option.socket;
        Device device = devices[option.deviceIndex];
        device.rotAngle = option.rotAngle;
        socket.Place(device);

        
        StartCoroutine(StartMove());
    }

    IEnumerator StartMove()
    {
        yield return new WaitForSeconds(1);
        Singleton.CAM.Follow();
        yield return new WaitUntil(() => Singleton.CAM.state == DynamicCamera.CameraState.FOLLOWING);
        FindObjectOfType<Ball>().StartMove();
    }
}
