using UnityEngine;
using System.Collections;

public class LaserPistol : WeaponRanged
{
    GameObject beam;

    protected override void Start()
    {
        base.Start();

        beam = Factory.instance.CreateBeamPurple();
        beam.transform.parent = transform;

        // Stats
        cooldown = 0f;
        ammoOffset = 0.25f;
        ammoMaxInventory = 200;
        ammoInventory = 200;
        ammoMax = 100;
        ammoCount = 100;
    }

    protected override void Use()
    {
        base.Use();
        beam.transform.position = transform.position + transform.right * ammoOffset;
        beam.transform.right = transform.right;

        AudioManager.instance.PlayAudio("dsplasma", 0.05f, false);

        beam.SetActive(true);
        cooldownRemaining = 1;
    }


    protected override void Update()
    {
        base.Update();
        cooldownRemaining--;
        // This is to disable the beam once the weapon stops being used
        if (cooldownRemaining < 1)
        {
            beam.SetActive(false);
        }
    }
}
