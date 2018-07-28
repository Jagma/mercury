using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponRanged {
    
    public override void Use()
    {
        base.Use();
        GameObject bullet = Factory.instance.CreateBullet();
        bullet.transform.position = transform.position + transform.right * 0.2f;
        bullet.transform.right = transform.right;
    }
}
