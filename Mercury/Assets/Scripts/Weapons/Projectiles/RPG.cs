using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : Projectile {
    protected override void Start() {
        base.Start();

        // Stats
        speed = 10f;
        damage = 5;
    }
}
