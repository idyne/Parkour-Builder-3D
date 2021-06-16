using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Activable
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public override void Activate()
    {
        anim.SetTrigger("Up");
    }

    public override void Deactivate()
    {
        anim.SetTrigger("Down");
    }
}
