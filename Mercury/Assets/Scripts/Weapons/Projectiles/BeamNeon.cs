using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamNeon : Beam
{
    public override void Init()
    {
        base.Init();

        // Stats
        damage = 2f;
        width = 0.25f;
    }

}
