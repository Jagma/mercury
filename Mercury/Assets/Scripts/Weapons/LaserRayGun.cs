using UnityEngine;
using System.Collections;

public class LaserRayGun : WeaponRanged
{
    GameObject beam;
    protected override void Start()
    {
        base.Start();
        beam = Factory.instance.CreateBeamNeon();
        beam.transform.parent = transform;

        // Stats
        cooldown = 0f;
        ammoOffset = 0.25f;
        ammoMaxInventory = 200;
        ammoInventory = 200;
        ammoMax = 100;
        ammoCount = 100;
    }


    public void setDamage(int damageA)
    {
        beam.GetComponent<BeamNeon>().setDamage(damageA);
    }

    protected override void Use()
    {
        base.Use();
        beam.transform.position = transform.position + transform.right * ammoOffset;
        beam.transform.right = transform.right;

        AudioManager.instance.PlayAudio("dsplasma", 0.05f, false);

        beam.SetActive(true);
        framesSinceUse = 0;
    }

    int framesSinceUse = 0;
    protected override void Update()
    {
        base.Update();
        // This is to disable the beam once the weapon stops being used
        framesSinceUse++;
        if (framesSinceUse > 2)
        {
            beam.SetActive(false);
        }
    }
}
