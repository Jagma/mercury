using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    protected Transform visual;
    private void Awake() {
        visual = transform.Find("Visual");
    }

    public virtual void Use () {
        Debug.Log("Weapon in use.");
    }
    
    private void Update() {
        // Visual look at camera
        visual.eulerAngles = new Vector3(45, 45, Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg + 45);
    }
}
