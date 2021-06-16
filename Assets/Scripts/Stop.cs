using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop : MonoBehaviour
{
    public int selectionIndex;
    public CameraPosition cameraPosition;


    public void Selection()
    {
        Singleton.CAM.SetPosition(cameraPosition);

        FindObjectOfType<Selector>().selections.transform.GetChild(selectionIndex).gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
