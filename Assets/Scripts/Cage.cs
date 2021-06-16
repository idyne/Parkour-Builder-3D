using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : Activable
{

    public Animator anim;

    public override void Activate()
    {
        anim.SetTrigger("Up");
    }

    public override void Deactivate()
    {
        anim.SetTrigger("Down");
    }
}
