using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {
    public int health = 2;

    private void OnTriggerEnter(Collider col) {
        Projectile p = col.GetComponent<Projectile>();
        if (p != null) {
            Damage(p.damage);
        }
    }

    public void Damage (int damage) {
        health -= damage;

        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}