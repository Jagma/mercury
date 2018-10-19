using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour {

    public PlayerModel playerModel;
    public int weaponIndex = 0;
    Text ammoText;
    void Start () {
        ammoText = transform.Find("Text").GetComponent<Text>();
	}
	

	void Update () {
        Weapon weapon = null;
        if (weaponIndex == 1) {
            weapon = playerModel.equippedWeapon;
        } else {
            weapon = playerModel.secondaryWeapon;
        }

        if (weapon == null) {
            ammoText.text = "";
            return;
        }

        ammoText.text = weapon.ammoInventory.ToString();
    }
}
