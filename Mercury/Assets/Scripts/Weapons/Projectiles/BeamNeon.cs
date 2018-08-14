using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamNeon : Beam {
    // TODO: When we add different types of beams this class will need to be reworked.
    // Currently a lot of functionality that should be handled by this child class lives in the base Beam class.

    public override void Init() {
        base.Init();

        // Stats
        width = 0.25f;
        damage = 2;
    }
}
