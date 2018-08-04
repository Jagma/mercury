using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : Projectile {
    public override void Init() {
        base.Init();

        // Stats
        speed = 10f;
        damage = 5;
    }
}
