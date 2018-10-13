using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamNeon : Beam
{
    public override void Init()
    {
        base.Init();

        // Stats
        width = 0.25f;
        damage = 2;
    }

    public void setDamage(int damageA)
    {
        damage = damageA;
    }
}
