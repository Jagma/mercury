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
    Material temp;
    public Vector3 forwardDirection = Vector3.forward;
    System.Random ran = new System.Random(85466248);

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.15f);
        transform.Find("Visual").Find("Body").GetComponent<SpriteRenderer>().material = temp;
    }

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
        temp = transform.Find("Visual").Find("Body").GetComponent<SpriteRenderer>().material;
    }

    //Forces enemy to look at camera
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

    public void Damage(double damage)
    {
        health -= damage;
        Material hit = Factory.instance.CreateHitFlash();
        transform.Find("Visual").Find("Body").GetComponent<SpriteRenderer>().material = hit;
        allowWalk = true;
        if (health <= 0)
        {
            Death();
        }
        StartCoroutine(Wait());
    }

    protected virtual void Death()
    {
        if (equippedWeapon) //has a chance to drop weapon or ammo pack.
        {
            DropItems();
        }
        DisplayEnemyCorpse();
        Destroy(gameObject);
    }

    private void DisplayEnemyCorpse()
    {
        Transform enemyVisual = transform.Find("Visual");
        enemyVisual.transform.parent = null;
        float speed = 75.0f;
        float angle = Mathf.PingPong(Time.time * speed, 90.0f);
        enemyVisual.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //enemyVisual.eulerAngles = new Vector3(transform.eulerAngles.x, 90f, transform.eulerAngles.z); --alternative method to rotate 90 degrees.
    }

    private void DropItems()
    {
        int randomNum = ran.Next(0, 100);
        if (randomNum >= 20 && randomNum < 61) //40% Drop ammo pack - still needs to be implemented.
        {
            GameObject randomAmmoPack;
            randomNum = ran.Next(0, 100);
            if (randomNum >= 0 && randomNum < 11)//10% Laser Rifle ammo
            {
                randomAmmoPack = Factory.instance.CreateBeamAmmoPack();
            }
            else if (randomNum > 20 && randomNum < 61) //40% rocket launcher ammo
            {
                randomAmmoPack = Factory.instance.CreateBulletAmmoPack();
            }
            else //if the random value is else.
            {
                randomAmmoPack = Factory.instance.CreateRocketAmmoPack();
            }
            randomAmmoPack.transform.position = transform.position;

            //removes weapon from being used.
            equippedWeapon.gameObject.SetActive(false);
            equippedWeapon.Dequip();
            equippedWeapon.equipped = false;
            equippedWeapon = null;
            Destroy(equippedWeapon);
        }
        else
        {
            equippedWeapon.Dequip();
            equippedWeapon.equipped = false;
            equippedWeapon = null;
        }

    }
}
