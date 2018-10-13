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
    public void setDamage(int damageA)
    {
        damage = damageA;
    }
    public override void Destroy()
    {
        base.Destroy();

        Collider[] hits = Physics.OverlapSphere(this.transform.position, blastRadius);

        foreach (Collider hit in hits) {
            Enemy enemyHit = hit.GetComponent<Enemy>();
            PlayerActor playerHit = hit.GetComponent<PlayerActor>();
            Wall wallHit = hit.GetComponent<Wall>();

            if (enemyHit) {
                enemyHit.Damage(damage);
            }
            if (playerHit) {
                playerHit.Damage(damage);
            }
            if (wallHit) {
                wallHit.Damage(damage);
            }
        }

        GameObject a = Factory.instance.CreateRocketHit();
        AudioManager.instance.PlayAudio("r_exp3", .5f, false);
        a.transform.position = transform.position;
        Destroy(a, 1f);

        Transform smokeTrail = visual.Find("SmokeTrail");
        if (smokeTrail != null)
        {
            smokeTrail.transform.parent = null;
            Destroy(smokeTrail.gameObject, 2f);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        Enemy enemyHit = col.GetComponent<Enemy>();
        PlayerActor playerHit = col.GetComponent<PlayerActor>();
        Wall wallHit = col.GetComponent<Wall>();

        if (enemyHit != null || playerHit != null || wallHit != null)
        {
            Destroy();
        }
    }
}
