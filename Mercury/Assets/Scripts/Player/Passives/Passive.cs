using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Passive
{
    public PlayerActor player;

    public float cooldown;
    public float lastActivatedTime;

    public virtual void Init()
    {
        
    }
    public virtual void SingleAffect()
    {

    }
    public virtual void RecurringAffect()
    {
        
    }
}

public class PassiveHPRegen : Passive
{
    float hpRegen = 0.1f;

    public override void Init()
    {
        base.Init();
        cooldown = 5f;
        hpRegen = 10f;
        lastActivatedTime = 0f;
    }
    public override void SingleAffect()
    {
        base.SingleAffect();
        Init();
    }

    public override void RecurringAffect()
    {
        if (Time.time >= lastActivatedTime + cooldown)
        {
            if (player.model.playerActive == true && player.model.health != player.model.maxHealth)
            {
                player.HealPlayer(hpRegen);
                lastActivatedTime = Time.time;
                Debug.Log(Time.time);
            }
        }
    }
}

public class PassiveIncreasedMaxHP : Passive
{
    float hpIncrease = 50f;

    public override void SingleAffect()
    {
        player.model.maxHealth += hpIncrease;
        player.model.health = player.model.maxHealth;
    }
    public override void RecurringAffect() {}

}

public class PassiveMovementSpeed : Passive
{
    float maxMovementSpeedIncrease = 0.5f;
    public override void SingleAffect()
    {
        player.model.moveMaxSpeed += player.model.moveMaxSpeed * maxMovementSpeedIncrease;
        player.model.moveAcceleration += 0.5f;
    }
    public override void RecurringAffect() { }
}

public class PassiveMaxAmmoIncrease : Passive
{
    int ammoIncrease = 50;

    public override void SingleAffect()
    {
        player.model.equippedWeapon.SetMaxAmmoCount(ammoIncrease);
        player.model.secondaryWeapon.SetMaxAmmoCount(ammoIncrease);
    }
    public override void RecurringAffect() { }
}

public class PassiveDegenAura : Passive
{
    float damage;

    public override void Init()
    {
        base.Init();
        cooldown = 5;
        damage = 5f;
        lastActivatedTime = 0f;
    }
    public override void SingleAffect()
    {
        Init();
    }
    public override void RecurringAffect()
    {
        if (player != null)
        {
            if (Time.time >= lastActivatedTime + cooldown)
            {
                Collider[] hits = Physics.OverlapSphere(player.transform.position, 3f);
                int hitCount = 0;
                foreach (Collider hit in hits)
                {
                    Enemy enemyHit = hit.GetComponent<Enemy>();
                    if (enemyHit != null)
                    {
                        enemyHit.Damage(damage);
                        hitCount++;
                    }
                }
                if (hitCount != 0)
                {
                    lastActivatedTime = Time.time;
                }
            }
        }
        }
    }


public class PassiveRandomBullet : Passive
{
    GameObject bullet;
    float radius;
    public override void Init()
    {
        base.Init();
        cooldown = 5f;
        radius = 5f;
        lastActivatedTime = 0f;
    }
    public override void SingleAffect()
    {
        base.SingleAffect();
        Init();
    }

    public override void RecurringAffect()
    {
        if (Time.time >= lastActivatedTime + cooldown)
        {
            Vector3 dir = EnemyDirection();
            if (dir != Vector3.zero)
            {
                //Move bullet
                bullet = Factory.instance.CreateBullet();
                bullet.transform.position = player.transform.position + dir;
                bullet.transform.right = new Vector3(dir.x, dir.y, dir.z);
                bullet.GetComponent<Projectile>().Update();
                lastActivatedTime = Time.time;


            }
        }
    }

    public Vector3 GetDirection(Vector3 enemyPos)// Return direction between enemy and player
    {
        Vector3 heading = enemyPos - player.transform.position;
        return heading / radius;
    }

    private Vector3 EnemyDirection()// Returns a random enemy in a certain radius
    {
        Collider[] hits = Physics.OverlapSphere(player.transform.position, radius);
        List<Enemy> allEnemies = new List<Enemy>();
        for(int i = 0; i < hits.Length; i++)
        {
            Enemy enemy = hits[i].GetComponent<Enemy>();

            if(enemy != null)
            {
                Vector3 direction = GetDirection(enemy.transform.position);
                if (IsPossibleToHit(direction))
                {
                    return direction;
                }
            }
        }
        return Vector3.zero;

    }

    private bool IsPossibleToHit(Vector3 dir)
    {
        RaycastHit hit;
        int layerId = LayerMask.NameToLayer("Environment");
        int layerMask = 1 << layerId;
        //Raycasts to check if it is possible to hit an enemy
        if (Physics.Raycast(new Ray(player.transform.position, dir),out hit, radius,layerMask))
        {
            Collider collider = hit.collider;
            if(collider.GetComponent<Wall>() !=  null)
            {
                return false;
            }
            return true;
        }
        return true;
    }

}
