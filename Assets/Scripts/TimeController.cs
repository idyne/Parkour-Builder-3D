using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{

    private bool slow = false;
    private float modifier = 1;

    private void Update()
    {
        if (slow)
        {
            if (Mathf.Abs(Time.timeScale - 1 / modifier) <= .1f)
                slow = false;
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1 / modifier, Time.deltaTime);
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
    }
    public void DoSlowMotion(float slowMotionModifier)
    {
        Time.timeScale = 1 / slowMotionModifier;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    public void UndoSlowMotion()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    public void DoSmoothSlowMotion(float modifier)
    {
        slow = true;
        this.modifier = modifier;
    }
}
