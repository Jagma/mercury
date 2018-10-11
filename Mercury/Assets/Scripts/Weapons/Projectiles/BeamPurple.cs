﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamPurple : Beam
{
    public override void Init()
    {
        base.Init();

        // Stats
        width = 0.25f;
        damage = 35;
    }

}