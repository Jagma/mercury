using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public double health = 100;
    public float moveSpeed = 1f;
    public Weapon equippedWeapon;
    Transform visual;
    Transform dropShadow;
    Rigidbody rigid;

    public float viewRadius;
    public float viewAngle;
    public Vector3 DirFromAngle (float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    private void Awake()
    {
        visual = transform.Find("Visual");
        rigid = GetComponent<Rigidbody>();
        dropShadow = Factory.instance.CreateDropShadow().transform;
        dropShadow.parent = transform;
        dropShadow.localPosition = new Vector3(0, 0, -0.3f);
        dropShadow.localScale = new Vector3(0.6f, 0.2f, 1);
    }

    protected virtual void Start()
    {
        transform.eulerAngles = new Vector3(0, 45, 0);
        visual.eulerAngles = new Vector3(45, 45, 0);
    }
	
	void FixedUpdate()
    {
        //TODO: Add Line of Sight for enemy.

        Collider[] colliders = Physics.OverlapSphere(transform.position, 7.5f);
        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerActor playerActor = colliders[i].GetComponent<PlayerActor>();

            Weapon weapon = colliders[i].GetComponent<Weapon>();

            // Ek het gedink dat die walker moet eers hardloop vir 'n weapon voor hy na die player to beweeg en skiet.
            if (weapon != null && weapon.equipped == false)
            {
                //float distance = Vector3.Distance(weapon.transform.position, transform.position); - wou die gebruik om die afstand te vind tussen die weapon en die enemy sodat hy so deur determine om op te tel.
                if (weapon.transform.position.z == this.transform.position.z)
                {
                    weapon.Equip();
                    equippedWeapon = weapon;
                    Debug.Log("Walker equipped weapon.");
                }
                MoveToPosition(weapon.transform.position);
            }
            if (playerActor != null)
            {
                float distance = Vector3.Distance(playerActor.transform.position, transform.position);
                MoveToPosition(playerActor.transform.position); //moves towards player's position.
                AimOnPlayer(playerActor.transform.position); //aims weapon towards player.
                AttackPlayer(); //shoots player.
            }
        }
    }

    public void AimOnPlayer(Vector3 direction)
    {
        if (equippedWeapon)
        {
            equippedWeapon.transform.right = new Vector3(direction.x, 0, direction.y);
        }
    }

    public void AttackPlayer()
    {
        if (equippedWeapon)
        {
            equippedWeapon.UseWeapon();
        }
    }

    void MoveToPosition(Vector3 target)
    {
        if (WallCollisionCheck() == true)
        {
            //TODO: move away from wall
            Vector3 vec = (target - transform.position).normalized * moveSpeed;
            vec.y = rigid.velocity.y; // preserves physics Y movement
            rigid.velocity = vec;
        }
        else
        {
            Vector3 vec = (target - transform.position).normalized * moveSpeed;
            vec.y = rigid.velocity.y; // preserves physics Y movement
            rigid.velocity = vec;
        }
    }

    private bool WallCollisionCheck()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(transform.position, transform.right));

        if (hits.Length <= 0)
        {
            return false;
        }

        for (int i = 0; i < hits.Length; i++)
        {
            Wall walls = hits[i].collider.GetComponent<Wall>();

            if (walls != null)
            {
                //Debug.Log("Walker collided with wall.");
                return true;
            }
        }
        return false;
    }
    
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
        if (health <= 0)
        {
            Destroy(gameObject);
            if (equippedWeapon)
            {
                equippedWeapon.Dequip();
                equippedWeapon = null;
            }
        }
    }
}
