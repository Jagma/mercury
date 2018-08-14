using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    // TODO : We will need to abstract this class once we add more enemy types. Similar to weapons. 

    public int health = 2;
    public float moveSpeed = 1f;

    Transform visual;
    Transform dropShadow;
    Rigidbody rigid;


    private void Awake() {
        visual = transform.Find("Visual");
        rigid = GetComponent<Rigidbody>();

        dropShadow = Factory.instance.CreateDropShadow().transform;
        dropShadow.parent = transform;
        dropShadow.localPosition = new Vector3(0, 0, -0.3f);
        dropShadow.localScale = new Vector3(0.6f, 0.2f, 1);
    }

    void Start () {
        transform.eulerAngles = new Vector3(0, 45, 0);
        visual.eulerAngles = new Vector3(45, 45, 0);
    }
	

	void FixedUpdate ()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerActor playerActor = colliders[i].GetComponent<PlayerActor>();
            if (playerActor != null)
            {
                MoveToPosition(playerActor.transform.position);
            }
        }

    }

    void MoveToPosition(Vector3 target)
    {
        Vector3 vec = (target - transform.position).normalized * moveSpeed;
        vec.y = rigid.velocity.y; // preserves physics Y movement

        rigid.velocity = vec;
    }

    private void OnTriggerEnter(Collider col) {
        Projectile projectile = col.GetComponent<Projectile>();
        if (projectile != null) {
            Damage(projectile.damage);
        }
    }

    void Damage (int damage) {
        health -= damage;

        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
