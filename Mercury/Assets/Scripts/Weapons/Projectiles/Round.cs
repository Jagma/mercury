using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : Projectile {
    protected override void Start() {
        base.Start();

        // Stats
        speed = 20f;
        damage = 1;
    }
}