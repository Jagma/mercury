using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public virtual void Use () {
        Debug.Log("Weapon in use.");
    }
}
