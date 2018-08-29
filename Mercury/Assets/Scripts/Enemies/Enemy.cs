﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public double health = 100;
    public float moveSpeed = 1f;
    public bool allowWalk = false;
    public Weapon equippedWeapon;
    Transform visual;
    Transform dropShadow;
    Rigidbody rigid;
    public Vector3 forwardDirection = Vector3.forward;

    private void Awake()
    {
        visual = transform.Find("Visual");
        rigid = GetComponent<Rigidbody>();
        dropShadow = Factory.instance.CreateDropShadow().transform;
        dropShadow.parent = visual;
        dropShadow.localPosition = new Vector3(0, 0, 0);
        dropShadow.localScale = new Vector3(0.6f, 0.2f, 1);
    }

    protected virtual void Start()
    {

    }

    protected virtual void FixedUpdate()
    {
        visual.eulerAngles = new Vector3(45, 45, 0);
        dropShadow.localPosition = new Vector3(0, 0, -0.5f);
    }

    // This method needs to be overriden for each enemy
    protected virtual void Attack()
    {

    }

    // AI interface methods
    protected void FaceDirection(Vector3 directionVector)
    {
        forwardDirection = directionVector;
    }

    protected void MoveForward()
    {
        Vector3 moveDir = new Vector3(forwardDirection.x, Mathf.Clamp(rigid.velocity.y, -100, 0), forwardDirection.z);
        rigid.velocity = moveDir * moveSpeed;
    }
    

    // Damage, health, and death
    private void OnTriggerEnter(Collider col)
    {
        Projectile projectile = col.GetComponent<Projectile>();
        if (projectile != null)
        {
            Damage(projectile.damage);
        }
    } 

    public void Damage (double damage)
    {
        health -= damage;
        allowWalk = true;
        if (health <= 0)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        if (equippedWeapon)
        {
            equippedWeapon.Dequip();
            equippedWeapon = null;
        }
        GameProgressionManager.instance.EnemyDead();
        Destroy(gameObject);
    }
}
