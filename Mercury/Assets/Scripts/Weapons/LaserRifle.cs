using UnityEngine;
using System.Collections;

public class LaserRifle : WeaponRanged
{
    GameObject beam;
    protected override void Start()
    {
        base.Start();
        // Stats
        cooldown = 0f;
        beamOffset = 0.65f;
    }

    protected override void Use()
    {
        base.Use();

        //Lazer rifle charge up effect.
        GameObject flash = Factory.instance.CreateMuzzleFlash();
        flash.transform.position = transform.position + transform.right * beamOffset;

        if (beam == null)
        {
            createBeam();
        }

        beam.transform.position = transform.position + transform.right * beamOffset;
        beam.transform.right = transform.right;
        beam.GetComponent<Beam>().Update();
    }

    private void createBeam()
    {
        beam = Factory.instance.CreateLaserBeam();
        beam.GetComponent<Beam>().speed *= 2;
    }
}
