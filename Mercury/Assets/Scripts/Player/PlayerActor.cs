using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActor : MonoBehaviour
{
    public PlayerModel model;
    public static PlayerActor instance;
    public Sprite forward;
    public Sprite facing;
    public Sprite death;
    private float startHealth = 100;
    private bool playerActive = true;
    public float health = 100;
    Transform visual;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        visual = transform.Find("Visual");
    }

    void Start ()
    {
        transform.eulerAngles = new Vector3(0, 45, 0);
        instance = this;
        model.equippedWeapon = Factory.instance.CreateMachineGun().GetComponent<Weapon>();
        model.equippedWeapon.Equip();
        model.secondaryWeapon = null;
    }

	void Update ()
    {
        if (playerActive)
        {
            if (model.equippedWeapon)
            {
                model.equippedWeapon.transform.position = transform.position + model.equippedWeapon.transform.right * 0.5f - transform.up * 0.2f;
            }
            if (model.secondaryWeapon)
            {
                model.secondaryWeapon.transform.position = transform.position + new Vector3(0.4f, 0, 0.5f);

            }

            // Visual look at camera
            visual.eulerAngles = new Vector3(45, 45, visual.eulerAngles.z);
        }
    }

    private void FixedUpdate()
    {
        if (playerActive)
        {
            Vector3 velocityMinusY = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
            velocityMinusY = Vector3.ClampMagnitude(velocityMinusY, model.moveMaxSpeed);
            rigid.velocity = new Vector3(velocityMinusY.x, rigid.velocity.y, velocityMinusY.z);

            rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(0, rigid.velocity.y, 0), model.moveDeceleration);
        }
    }

    public Weapon GetPlayerEquippedWeapon()
    {
        return model.equippedWeapon;
    }

    public void Move (Vector2 direction)
    {
        Vector2 moveDir = InputManager.instance.GetMoveDirection(model.playerID);
        rigid.velocity += transform.forward * moveDir.y * model.moveAcceleration;
        rigid.velocity += transform.right * moveDir.x * model.moveAcceleration;
    }
    
    public void SwitchWeapons()
    {
        if (playerActive)
        {
            if (model.secondaryWeapon == null)
            {
                return;
            }
            Weapon sw = model.equippedWeapon;
            model.equippedWeapon.Dequip();
            model.equippedWeapon.equipped = false;
            model.secondaryWeapon.Equip();
            model.secondaryWeapon.equipped = true;
            model.equippedWeapon = model.secondaryWeapon;
            model.secondaryWeapon = sw;
            model.secondaryWeapon.gameObject.SetActive(false);
            model.equippedWeapon.gameObject.SetActive(true);
        }
    }

    public void Interact()
    {
        if (playerActive)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
            for (int i = 0; i < colliders.Length; i++)
            {
                Weapon weapon = colliders[i].GetComponent<Weapon>();
                Chest chest = colliders[i].GetComponent<Chest>();
                if (weapon != null && weapon.equipped == false && weapon.name.Equals(model.equippedWeapon.name))
                {
                    model.equippedWeapon.SetAmmoCount(20);
                    Destroy(weapon, 0.1f);
                }
                else if (weapon != null && model.secondaryWeapon != null && weapon.equipped == false && weapon.name.Equals(model.secondaryWeapon.name))
                {
                    model.secondaryWeapon.SetAmmoCount(20);
                    Destroy(weapon, 0.1f);
                }
                else
                if (weapon != null && weapon != model.equippedWeapon && weapon != model.secondaryWeapon && weapon.equipped == false)
                {
                    // Dequip current weapon
                    //Both slots full
                    if (model.equippedWeapon != null && model.secondaryWeapon != null)
                    {
                        model.equippedWeapon.Dequip();
                        model.equippedWeapon.equipped = false;
                        weapon.Equip();
                        weapon.equipped = true;
                        model.equippedWeapon = weapon;
                    }
                    //Inventory empty
                    if (model.equippedWeapon != null && model.secondaryWeapon == null)
                    {
                        model.equippedWeapon.Dequip();
                        model.equippedWeapon.equipped = false;
                        model.secondaryWeapon = model.equippedWeapon;
                        weapon.Equip();
                        weapon.equipped = true;
                        model.equippedWeapon = weapon;
                        model.secondaryWeapon.gameObject.SetActive(false);
                    }

                    // Equip new weapon
                    if (model.equippedWeapon == null && model.secondaryWeapon == null)
                    {
                        weapon.Equip();
                        weapon.equipped = true;
                        AudioManager.instance.PlayAudio("dsdbload", 1, false);
                        model.equippedWeapon = weapon;
                    }
                }

                if (chest != null)
                {
                    chest.OpenChest();
                }
            }
        }
    }

    public void Aim (Vector2 direction)
    {
        if (playerActive)
        {
            if (model.equippedWeapon)
            {
                model.equippedWeapon.transform.right = Quaternion.AngleAxis(45, Vector3.up) * new Vector3(direction.x, 0, direction.y);
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
            }
            else
            {
                visual.Find("Body").GetComponent<SpriteRenderer>().sprite = facing;
            }
        }
    }

    public void Attack ()
    {
        if (playerActive)
        {
            if (model.equippedWeapon)
            {
                model.equippedWeapon.UseWeapon();
            }
        }
    }

    public void UseAbility ()
    {
        model.ability.UseAbility();
    }

    public float GetHealthInformation()
    {
        return health;
    }

    public float GetStartHealth()
    {
        return startHealth;
    }

    public void Damage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Down();
        }
    }

    public void Revive(float hp)
    {
        health = hp;
        playerActive = true;
    }

    private void DisplayPlayerDown()
    {
        Debug.Log("Player is down.");
        visual.transform.parent = null;
        if (Random.Range(0, 100) >= 50)
        {
            visual.transform.localEulerAngles = new Vector3(45, 45, -90);
        }
        else
        {
            visual.transform.localEulerAngles = new Vector3(45, 45, 90);
        }
        visual.transform.position = new Vector3(visual.transform.position.x, 0.7f, visual.transform.position.z);
    }


    public void Down()
    {
        if (GameProgressionManager.instance.getPlayerCount() > 1)
        {
            DisplayPlayerDown();
            playerActive = false;
        }
        else
        {
            Death();
        }
    }

    public void Death()
    {
        AudioManager.instance.PlayAudio("death1", 1, false);
        Debug.Log("Player died.");
        GameProgressionManager.instance.GameOver();
    }

    private void OnTriggerEnter(Collider col)
    {

        PlayerActor player = col.GetComponent<PlayerActor>();
        if (player != null)
        {
            Revive(100);
        }
    }
}
