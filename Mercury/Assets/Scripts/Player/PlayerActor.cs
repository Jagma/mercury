using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour
{
    public PlayerModel model;
    public static PlayerActor instance;
    public Sprite forward;
    public Sprite facing;

    Transform visual;
    Rigidbody rigid;
    Weapon weapon;
    bool weaponCol = false;
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
        for (int i = 0; i < colliders.Length; i++) {
            Weapon weapon = colliders[i].GetComponent<Weapon>();
            if (weapon != null && weapon != model.equippedWeapon) {
                // Dequip current weapon
                if (model.equippedWeapon != null) {
                    model.equippedWeapon.Dequip();
                    model.equippedWeapon = null;
                }

                // Equip new weapon
                weapon.Equip();
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
        if (norm.x < 0) {
            Vector3 x = Quaternion.AngleAxis(180, visual.up) * visual.forward;
            visual.forward = x;
        }
        if (norm.z > 0) {
            visual.Find("Body").GetComponent<SpriteRenderer>().sprite = forward;
        } else {
            visual.Find("Body").GetComponent<SpriteRenderer>().sprite = facing;
        }
    }

    public void Attack () {
        if (model.equippedWeapon)   {
            model.equippedWeapon.UseWeapon();
        }
    }

    public void UseAbility () {
        model.ability.UseAbility();
        Debug.Log("used");
    }
}
