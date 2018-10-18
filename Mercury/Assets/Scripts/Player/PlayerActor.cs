using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActor : MonoBehaviour
{
    public PlayerModel model;
    public Sprite forward;
    public Sprite facing;
    public Sprite death;
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
        model.equippedWeapon = Factory.instance.CreateSword().GetComponent<Weapon>();
        model.equippedWeapon.Equip();
        model.secondaryWeapon = null;
    }

	void Update ()
    {
        if (model.playerActive)
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
        if (model.playerActive)
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
        if (model.playerActive)
        {
            Vector2 moveDir = InputManager.instance.GetMoveDirection(model.playerID);
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
        if (model.playerActive)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
            for (int i = 0; i < colliders.Length; i++)
            {
                Weapon weapon = colliders[i].GetComponent<Weapon>();
                Chest chest = colliders[i].GetComponent<Chest>();
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
                        model.secondaryWeapon.gameObject.SetActive(false);//Hides the secondary weapon
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
        if (model.playerActive)
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
            Down();
        }

        GameObject blood = Factory.instance.CreateBlood();
        blood.transform.position = this.transform.position;
    }


    public void Down()
    {
        GameProgressionManager.instance.SetPlayerDown(model.playerID, true);
        bool allDown = GameProgressionManager.instance.getPlayerDownCount();

        if (GameProgressionManager.instance.getPlayerCount() > 1 && allDown ==false)
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

        if (model.health + hp > 100)
        {
            model.health = 100;
        }            
  
        if (model.playerActive == false)
        {
            model.playerActive = true;
            model.health = hp;
            rigid.constraints = RigidbodyConstraints.FreezeRotation;
            visual.transform.parent = transform;
            visual.eulerAngles = new Vector3(45, 45, visual.eulerAngles.z);
            GameProgressionManager.instance.SetPlayerDown(model.playerID, false);
        }

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
        rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
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
            Debug.Log("Player collided with another player.");
            HealPlayer(100);
        }
    }
}
