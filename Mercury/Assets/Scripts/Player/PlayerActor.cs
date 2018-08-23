using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour
{
    public PlayerModel model;
    public static PlayerActor instance;
    public Sprite forward;
    public Sprite facing;
    public Sprite death;
    public double health = 99999;
    Transform visual;
    Rigidbody rigid;
    Weapon weapon;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        visual = transform.Find("Visual");
    }

    void Start ()
    {
        transform.eulerAngles = new Vector3(0, 45, 0);
        instance = this;
    }

	void Update ()
    {
        if (model.equippedWeapon)
        {
            model.equippedWeapon.transform.position = transform.position + model.equippedWeapon.transform.right * 0.5f - transform.up * 0.2f;
        }

        // Visual look at camera
        visual.eulerAngles = new Vector3(45, 45, visual.eulerAngles.z);
    }

    private void FixedUpdate()
    {
        Vector3 velocityMinusY = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
        velocityMinusY = Vector3.ClampMagnitude(velocityMinusY, model.moveMaxSpeed);
        rigid.velocity = new Vector3(velocityMinusY.x, rigid.velocity.y, velocityMinusY.z);

        rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(0, rigid.velocity.y, 0), model.moveDeceleration);
    }

    public void Move (Vector2 direction)
    {
        Vector2 moveDir = InputManager.instance.GetMoveDirection(model.playerID);
        rigid.velocity += transform.forward * moveDir.y * model.moveAcceleration;
        rigid.velocity += transform.right * moveDir.x * model.moveAcceleration;
    }

    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
        for (int i = 0; i < colliders.Length; i++)
        {
            Weapon weapon = colliders[i].GetComponent<Weapon>();
            if (weapon != null && weapon != model.equippedWeapon)
            {
                // Dequip current weapon
                if (model.equippedWeapon != null)
                {
                    model.equippedWeapon.Dequip();
                    model.equippedWeapon = null;
                }

                // Equip new weapon
                weapon.Equip();
                AudioManager.instance.PlayAudio("dsdbload",1,false);
                model.equippedWeapon = weapon;
                return;
            }
        }
    }

    public void Aim (Vector2 direction)
    {
        if (model.equippedWeapon)
        {
            model.equippedWeapon.transform.right = new Vector3(direction.x, 0, direction.y);
        }

        // TODO: Move this to a seperate animation script
        // This is for setting the sprites based on the aim/look direction
        Vector3 norm = Quaternion.AngleAxis(-45, Vector3.up) * new Vector3(direction.x, 0, direction.y);
        if (norm.x < 0)
        {
            Vector3 x = Quaternion.AngleAxis(180, visual.up) * visual.forward;
            visual.forward = x;
        }

        if (norm.z > 0)
        {
            visual.Find("Body").GetComponent<SpriteRenderer>().sprite = forward;
        } else
        {
            visual.Find("Body").GetComponent<SpriteRenderer>().sprite = facing;
        }
    }

    public void Attack ()
    {
        if (model.equippedWeapon)
        {
            model.equippedWeapon.UseWeapon();
        }
    }

    public void UseAbility ()
    {
        model.ability.UseAbility();
        Debug.Log("Ability used");
    }


    // Damage, health, and death
    private void OnTriggerEnter(Collider col)
    {
        Projectile projectile = col.GetComponent<Projectile>();
        Portal portal = col.GetComponent<Portal>();
        if (projectile != null)
        {
            Damage(projectile.damage);
            Debug.Log("Player took damage.");
        }
        if (portal != null)
        {
            Debug.Log("portal enter.");
            Portal.instance.EnterPortal();
        }

    }

    public void Damage(double damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        /*Destroy(gameObject);
        if (model.equippedWeapon)
        {
            model.equippedWeapon.Dequip();
            model.equippedWeapon = null;
        }*/
        AudioManager.instance.PlayAudio("death1",1,false);
        Debug.Log("Player is dead.");
        GameProgressionManager.instance.GameOver();
    }
}
