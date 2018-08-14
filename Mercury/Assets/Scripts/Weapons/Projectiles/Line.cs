using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : Beam
{

    public override void Init()
    {
        base.Init();

        // Stats
        speed = 20f;
        damage = 666;
    }

    public override void Destroy()
    {
        base.Destroy();
        GameObject a = Factory.instance.CreateBeamHit();
        a.transform.position = transform.position;
    }
}
