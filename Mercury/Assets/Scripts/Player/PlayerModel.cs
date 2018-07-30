using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel {

    public string playerID = "-1";
    public float moveAcceleration = 1.2f; 
    public float moveDeceleration = 0.2f;
    public float moveMaxSpeed = 5f;

    public Weapon equippedWeapon;
    public Weapon secondaryWeapon;
}