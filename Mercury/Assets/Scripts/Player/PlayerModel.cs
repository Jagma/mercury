﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel {

    public string playerID = "-1";
    public float moveAcceleration = 1f; 
    public float moveDeceleration = 0.1f;
    public float moveMaxSpeed = 5f;

    public Weapon equippedWeapon;
    public Weapon secondaryWeapon;
}