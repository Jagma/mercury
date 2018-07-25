using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerModel {

    public string playerID = "-1";
    public float moveAcceleration = 1f; 
    public float moveDeceleration = 0.1f; // Range between 0 and 1, with one being instant stop.
    public float moveMaxSpeed = 5f;

    public Weapon equippedWeapon;
    public Weapon secondaryWeapon;


}