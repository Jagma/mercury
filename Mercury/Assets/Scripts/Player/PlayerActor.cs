using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour
{

    public PlayerModel model;

    public Sprite forward;
    public Sprite facing;

    Transform visual;
    Transform dropShadow;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        visual = transform.Find("Visual");
        dropShadow = Factory.instance.CreateDropShadow().transform;
        dropShadow.parent = transform;
        dropShadow.localPosition = new Vector3(0, 0, -0.1f);
        dropShadow.localScale = new Vector3(0.6f, 0.2f, 1);
    }

    void Start ()
    {        
        transform.eulerAngles = new Vector3(0, 45, 0);
    }

	void Update ()
    {
        if (model.equippedWeapon)
        {
            model.equippedWeapon.transform.position = transform.position + model.equippedWeapon.transform.right * 0.5f;
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
        for (int i=0; i < colliders.Length; i ++) {
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
}
