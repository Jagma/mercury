using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : Projectile
{
    float blastRadius;
    public override void Init()
    {
        base.Init();
        // Stats
        speed = 10f;
        damage = 100;
        blastRadius = 1f;
    }

    public override void Destroy()
    {
        base.Destroy();
        GameObject a = Factory.instance.CreateRocketHit();
        AudioManager.instance.PlayAudio("r_exp3", .5f, false);
        a.transform.position = transform.position;
        Destroy(a, 1f);
    }

    private void OnTriggerEnter(Collider col)
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, blastRadius);
        foreach (Collider hit in hits)
        {
            Debug.Log(hit.gameObject.name);
            if (hit.gameObject.name.Equals("Enemy Walker") || hit.gameObject.name.Equals("Ranged Enemy"))
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


        Wall wallCheck = col.GetComponent<Wall>();
        Enemy enemy = col.GetComponent<Enemy>();
        PlayerActor playerA = col.GetComponent<PlayerActor>();
        if (wallCheck != null || playerA != null || enemy != null)
        {
            Destroy();
        }
    }
}
