using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool on = false;
    public Activable[] activables;
    public CameraPosition camPos;
    public float showTime = 1;
    public Color onColor;

    public void On()
    {
        if (!on)
        {
            GetComponent<Renderer>().materials[0].color = onColor;
            on = true;
            if (camPos)
                StartCoroutine(Show());
            else
            {
                Singleton.BALL.StopMove();
                foreach (Activable activable in activables)
                    activable.Activate();
            }
        }
    }

    IEnumerator Show()
    {
        Singleton.BALL.StopMove();
        Singleton.CAM.SetPosition(camPos);
        yield return new WaitUntil(() => Singleton.CAM.state == DynamicCamera.CameraState.STOPPED);
        foreach (Activable activable in activables)
            activable.Activate();
        yield return new WaitForSeconds(showTime);
        Singleton.CAM.Follow();
        yield return new WaitUntil(() => Singleton.CAM.state == DynamicCamera.CameraState.FOLLOWING);
        Singleton.BALL.StartMove();
    }
}
