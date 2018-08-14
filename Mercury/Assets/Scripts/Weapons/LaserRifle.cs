using UnityEngine;
using System.Collections;

public class LaserRifle : WeaponRanged
{
    GameObject beam;    
    protected override void Start()
    {
        base.Start();

        beam = Factory.instance.CreateBeamNeon();
        beam.transform.parent = transform;

        // Stats
        cooldown = 0f;
        ammoOffset = 0.2f;
    }

    protected override void Use()
    {
        base.Use();
        beam.transform.position = transform.position + transform.right * ammoOffset;
        beam.transform.right = transform.right;
        
        beam.SetActive(true);
        framesSinceUse = 0;
    }

    int framesSinceUse = 0;
    private void Update() {
        // This is to disable the beam once the weapon stops being used
        framesSinceUse++;
        if (framesSinceUse > 2) {
            beam.SetActive(false);
        }
    }
}
