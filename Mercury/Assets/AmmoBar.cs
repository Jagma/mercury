using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    public PlayerModel playerModel;
    public int weaponIndex = 0;
    protected Text ammoText;
    protected Weapon weapon;
    protected virtual void Start ()
    {
        ammoText = transform.Find("Text").GetComponent<Text>();
	}


    protected virtual void Update ()
    {
        if (weaponIndex == 1)
            weapon = playerModel.equippedWeapon;
        else
            weapon = playerModel.secondaryWeapon;
        if (weapon == null)
        {
            ammoText.text = "";
            return;
        }
    }
}
