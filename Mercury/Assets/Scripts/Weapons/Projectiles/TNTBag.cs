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
        speed = 1f;
        damage = 200;
        blastRadius = 2f;
        timer = 3f;
        height = 0;
        initPosition = this.transform.position;
    }

    void  Update()
    {
        if(aimDirection != null)
        {
            distance = Mathf.Abs(Vector3.Distance(initPosition, this.transform.position));
            if(distance >= 200f)
            {
                Explode();
            }
            if(this.transform.position.z <= 0)
            {
                height = 1;
            }
            else if(this.transform.position.z > 3 )
            {
                height = -1;
            }
            //float heightPos = Mathf.Abs((Mathf.Pow(distance, 2) / 1000 + (distance) / 5) - transform.position.z);
            this.transform.position += new Vector3(aimDirection.x,0, aimDirection.y) * speed * Time.deltaTime;
            
           // Debug.Log("Dist: " + distance + "Height: " + heightPos);
        }
        else
        {
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
