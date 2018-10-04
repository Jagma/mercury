using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTBag : Projectile
{

    float blastRadius, timer, height, distance;
    Vector3 aimDirection, initPosition;
    public override void Init()
    {
        base.Init();
        // Stats
        speed = 2f;
        damage = 100;
        blastRadius = 2f;
        timer = 2f;
        height = 0;
        initPosition = this.transform.position;
    }

    void Update()
    {
        if(aimDirection != null)
        {
            distance = Mathf.Abs(Vector3.Distance(initPosition, this.transform.position));
            if(distance >= 200f)
            {
                Explode();
            }
            this.transform.position += new Vector3(aimDirection.x,0, aimDirection.y) * speed * Time.deltaTime;
            if (timer <= 0)
            {
                Explode();
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }       
    }

    public void Move(Vector3 aAimDirection)
    {
        aimDirection = aAimDirection;
    }

    public void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, blastRadius);
        foreach(Collider hit in hits)
        {
            Debug.Log(hit.gameObject.name);
            if(hit.GetComponent<Enemy>() != null)
            {
                Enemy enemyHit = hit.GetComponent<Enemy>();
                enemyHit.Damage(damage);
            }
            if (hit.gameObject.name.Equals("Player"))
            {
                PlayerActor player = hit.GetComponent<PlayerActor>();
                player.Damage(damage);
            }
            if (hit.GetComponent<Wall>() != null)
            {
                Wall wall = hit.GetComponent<Wall>();
                wall.Damage((int)damage);
            }
        }
        GameObject explosion = Factory.instance.CreateRocketHit();
        explosion.transform.position = this.transform.position;
        this.Destroy();
    }
}
