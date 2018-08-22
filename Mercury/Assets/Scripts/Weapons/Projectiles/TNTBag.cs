using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTBag : Projectile
{

    float blastRadius, timer;
    public override void Init()
    {
        base.Init();
        // Stats
        speed = 5f;
        damage = 200;
        blastRadius = 2f;
        timer = 3f;
    }

    void  Update()
    {
        if(timer <= 0)
        {
            Explode();
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, blastRadius);
        foreach(Collider hit in hits)
        {
            Debug.Log(hit.gameObject.name);
            if(hit.gameObject.name.Equals("Enemy Walker")|| hit.gameObject.name.Equals("Ranged Enemy"))
            {
                Enemy enemyHit = hit.GetComponent<Enemy>();
                enemyHit.Damage(damage);
            }
            if (hit.gameObject.name.Equals("Player"))
            {
                PlayerActor player = hit.GetComponent<PlayerActor>();
                player.Damage(damage);
            }
            if (hit.gameObject.name.Equals("Wall"))
            {
                Wall wall = hit.GetComponent<Wall>();
                wall.Damage((int)damage);
            }
        }
        this.Destroy();
    }
}
