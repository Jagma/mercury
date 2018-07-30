using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    /* We will need to abstract this class once we add more enemy types. Similar to weapons.     * 
    */

    public int health = 2;
    Transform visual;
    Transform dropShadow;

    private void Awake() {
        visual = transform.Find("Visual");

        dropShadow = Factory.instance.CreateDropShadow().transform;
        dropShadow.parent = transform;
        dropShadow.localPosition = new Vector3(0, 0, -0.3f);
        dropShadow.localScale = new Vector3(0.6f, 0.2f, 1);
    }

    void Start () {
        transform.eulerAngles = new Vector3(0, 45, 0);
        visual.eulerAngles = new Vector3(45, 45, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider col) {
        Bullet bullet = col.GetComponent<Bullet>();
        if (bullet != null) {
            Damage(bullet.damage);
        }
    }

    void Damage (int damage) {
        health -= damage;

        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
