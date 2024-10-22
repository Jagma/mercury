﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActor : MonoBehaviour
{
    public PlayerModel model;
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
        
        //DELETE
        /*AddPassive(new PassiveDegenAura());
        AddPassive(new PassiveHPRegen());
        AddPassive(new PassiveMovementSpeed());
        AddPassive(new PassiveIncreasedMaxHP());
        AddPassive(new PassiveIncreasedMaxHP());
        //AddPassive(new PassiveRandomBullet());
        AddPassive(new PassiveMaxAmmoIncrease());
        */

        SingleUsePassives();//Activate passives with single affect
    }

	void Update ()
    {
        if (model.playerActive)
        {
            if (model.equippedWeapon)
            {
                model.equippedWeapon.transform.localScale = new Vector3(1, 1, 1);
                model.equippedWeapon.transform.position = transform.position + model.equippedWeapon.transform.right * 0.5f - Vector3.up * 0.2f;
            }
            if (model.secondaryWeapon)
            {
                model.secondaryWeapon.transform.localEulerAngles = new Vector3(45, 45, 0);
                model.secondaryWeapon.transform.Find("Visual").Find("Body").localPosition = Vector3.zero;
                if (model.lookDirection.y < 0) {
                    model.secondaryWeapon.transform.position = transform.position + new Vector3(0.6f, -0.2f, 0.6f);                                    
                    model.secondaryWeapon.transform.localScale = new Vector3(1, 1, 1);
                } else {
                    model.secondaryWeapon.transform.position = transform.position + new Vector3(-0.1f, 0, -0.1f);
                    model.secondaryWeapon.transform.localScale = new Vector3(-1, 1, 1);
                }
                
            }
            
            // Visual look at camera
            visual.eulerAngles = new Vector3(45, 45, visual.eulerAngles.z);

            RecurringPassives();//Apply recurring affects of passives
        }
    }

    private void FixedUpdate()
    {
        if (model.playerActive)
        {
            Vector3 velocityMinusY = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
            velocityMinusY = Vector3.ClampMagnitude(velocityMinusY, model.moveMaxSpeed);
            rigid.velocity = new Vector3(velocityMinusY.x, rigid.velocity.y, velocityMinusY.z);
            rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(0, rigid.velocity.y, 0), model.moveDeceleration);
        }
        else
            PlayerCollisionDetection();
    }

    public Weapon GetPlayerEquippedWeapon()
    {
        return model.equippedWeapon;
    }

    public void Move (Vector2 direction)
    {
        if (model.playerActive)
        {
            Vector2 moveDir = direction;
            rigid.velocity += transform.forward * moveDir.y * model.moveAcceleration;
            rigid.velocity += transform.right * moveDir.x * model.moveAcceleration;
        }
    }
    
    public void SwitchWeapons()
    {
        if (model.playerActive)
        {
            if (model.secondaryWeapon == null)
            {
                return;
            }
            Weapon sw = model.equippedWeapon;

            model.equippedWeapon = model.secondaryWeapon;
            model.secondaryWeapon = sw;
        }
    }

    private void PickUpWeapon(Weapon weapon)
    {
        model.equippedWeapon = weapon;
        model.equippedWeapon.Equip();
        foreach (Passive pas in model.passives)
        {
            if (pas.name.Equals("Ammo Increase"))
            {
                pas.SingleEffect();
            }
        }
    }

    private void SwapEquipedWeapon(Weapon weapon)
    {
        foreach (Passive pas in model.passives)
        {
            if (pas.name.Equals("Ammo Increase"))
            {
                pas.RevertEffect();
            }
        }
        model.equippedWeapon.Dequip();
        PickUpWeapon(weapon);
    }

    private void PickUpSecondaryWeapon(Weapon weapon)
    {
        model.secondaryWeapon = model.equippedWeapon;
        PickUpWeapon(weapon);
        foreach (Passive pas in model.passives)
        {
            if (pas.name.Equals("Ammo Increase"))
            {
                pas.SingleEffect();
            }
        }
    }

    public void Interact()
    {
        if (model.playerActive)
        {
            float nearestDistance = float.MaxValue;
            float distance;
            int weaponIndex = -1;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

            //Used to find nearest weapon or chest
            for (int i = 0; i < colliders.Length; i++)
            {
                Weapon weapon = colliders[i].GetComponent<Weapon>();
                if (weapon != null && weapon.equipped == false)
                {
                    distance = (transform.position - weapon.transform.position).sqrMagnitude;

                    if(distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        weaponIndex = i;
                    }
                }

                Chest chest = colliders[i].GetComponent<Chest>();

                if (chest != null)
                {
                    chest.OpenChest();
                }
            }

            if (weaponIndex != -1)
            {
                Weapon nearestWeapon = colliders[weaponIndex].GetComponent<Weapon>();
                // Dequip current weapon
                //Both slots full
                if (model.equippedWeapon != null && model.secondaryWeapon != null)
                {
                    SwapEquipedWeapon(nearestWeapon);
                }
                //Inventory empty
                if (model.equippedWeapon != null && model.secondaryWeapon == null)
                {
                    PickUpSecondaryWeapon(nearestWeapon);
                }
                // Equip new weapon
                if (model.equippedWeapon == null && model.secondaryWeapon == null)
                {
                    PickUpWeapon(nearestWeapon);
                    AudioManager.instance.PlayAudio("dsdbload", 1, false);
                }
            }
        }
    }

    public void Aim (Vector2 direction)
    {
        model.lookDirection = direction;
        if (model.playerActive)
        {
            if (model.equippedWeapon)
            {
                Vector3 norm = Quaternion.AngleAxis(45, Vector3.up) * new Vector3(direction.x, 0, direction.y);
                model.equippedWeapon.transform.right = norm;
            }
        }
    }

    public void Attack ()
    {
        if (model.playerActive)
        {
            if (model.equippedWeapon)
            {
                model.equippedWeapon.UseWeapon();
            }
        }
    }

    public void UseAbility ()
    {
        if (model.playerActive)
            model.ability.UseAbility();
    }

    public void Damage(float damage)
    {
        if (model.godMode == true) {
            return;
        }

        model.health -= damage;

        if (model.health <= 0)
        {
            if (model.playerActive == true) {
                Down();
            }            
        }

        GameObject blood = Factory.instance.CreateBlood();
        blood.transform.position = this.transform.position;
        GameObject.Destroy(blood, 5);
        GameProgressionManager.instance.IncreasePlayerDamageTaken(damage);
    }


    public void Down()
    {
        GameProgressionManager.instance.SetPlayerDown(model.playerID, true);
        bool allDown = GameProgressionManager.instance.getPlayerDownCount();

        if (GameProgressionManager.instance.getPlayerCount() > 1 && allDown == false)
        {
            DisplayPlayerDown();
            model.playerActive = false;
        }
        else
        {
            Death();
        }
    }

    public void HealPlayer(float hp)
    {
        model.health += hp;

        if (model.health + hp > model.maxHealth)
        {
            model.health = model.maxHealth;
        }            
  
        if (model.playerActive == false)
        {
            model.playerActive = true;
            model.health = hp;
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            visual.transform.parent = transform;
            visual.eulerAngles = new Vector3(45, 45, 0);
            GameProgressionManager.instance.SetPlayerDown(model.playerID, false);
        }

    }


    private void DisplayPlayerDown()
    {
        //visual.transform.parent = null;
        if (Random.Range(0, 100) >= 50)
        {
            visual.transform.localEulerAngles = new Vector3(45, 45, -90);
        }
        else
        {
            visual.transform.localEulerAngles = new Vector3(45, 45, 90);
        }
        visual.transform.position = new Vector3(visual.transform.position.x, 0.7f, visual.transform.position.z);
        rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    }


    public void Death()
    {
        AudioManager.instance.PlayAudio("Player_death", 1, false);
        GameProgressionManager.instance.GameOver();
    }

    public void AddPassive(Passive passive)
    {
        passive.player = this;
        model.passives.Add(passive);
    }
    private void RecurringPassives()// Apply passive's recurring affect
    {
        if(model.passives.Count > 0)
        {
            foreach (Passive passive in model.passives)
            {
                passive.RecurringEffect();
            }
        }
    }
    
    private void SingleUsePassives()// Apply passive's single affect
    {
        if (model.passives.Count > 0)
        {
            foreach (Passive passive in model.passives)
            {
                passive.SingleEffect();
            }
        }
    }



    Collider[] colliders;
    protected virtual void PlayerCollisionDetection()
    {
        int playerLayerID = LayerMask.NameToLayer("Player");
        int playerLayerMask = 1 << playerLayerID;
        colliders = Physics.OverlapSphere(transform.position, 0.25f, playerLayerMask);

        PlayerActor playerActor = null;
        for (int i = 1; i < colliders.Length; i++)
        {
            playerActor = colliders[i].GetComponent<PlayerActor>();

            // Is this collider a player
            if (playerActor != null)
            {
                Debug.Log("Player revived.");
                HealPlayer(50);
            }
        }
    }
}
