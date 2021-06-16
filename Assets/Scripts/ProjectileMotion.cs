using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotion : MonoBehaviour
{

    private static float timePassed = 0;
    private static float t;
    private static Vector3 targetPos;
    private static bool fired = false;
    private static Vector3 force;
    private static Transform obj;
    private static Callback callback;

    private void Update()
    {
        if (fired)
        {
            float deltaTime = timePassed;
            timePassed = Mathf.Clamp(timePassed + Time.deltaTime, 0, t);
            deltaTime = timePassed - deltaTime;
            Vector3 newPos = obj.position;
            newPos += (new Vector3(force.x, 0, force.z)) * deltaTime;
            force.y += Physics.gravity.y * deltaTime;
            newPos.y += (force.y * deltaTime);
            //obj.position = Vector3.MoveTowards(obj.position, newPos, deltaTime);
            //if (timePassed >= t && Vector3.Distance(obj.position, newPos) <= 0.000001f)
            obj.position = newPos;
            if (timePassed >= t)
            {
                fired = false;
                obj.position = targetPos;
                timePassed = 0;
                callback();
            }
        }
    }

    public static void Fire(float t, Vector3 targetPos, Transform obj, Callback callback)
    {
        ProjectileMotion.callback = callback;
        ProjectileMotion.obj = obj;
        ProjectileMotion.targetPos = targetPos;
        Vector3 dif = targetPos - obj.position;
        Vector3 force = new Vector3(dif.x, 0, dif.z) / t;
        force.y = dif.y / t - Physics.gravity.y * t / 2;
        ProjectileMotion.force = force;
        fired = true;
        ProjectileMotion.t = t;
    }
}
