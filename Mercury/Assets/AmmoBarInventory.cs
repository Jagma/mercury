using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBarInventory : AmmoBar
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (weapon != null) {
            ammoText.text = weapon.ammoInventory.ToString();
        }        
    }
}
