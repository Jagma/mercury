using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour {

    public PlayerModel model;

    Transform visual;
    Rigidbody rigid;

    private void Awake() {
        rigid = GetComponent<Rigidbody>();
        visual = transform.Find("Visual");
    }

    void Start () {        
        transform.eulerAngles = new Vector3(0, 45, 0);
    }

	void Update () {
        if (model.equippedWeapon) {
            model.equippedWeapon.transform.position = transform.position + model.equippedWeapon.transform.right * 0.5f;
        }

        // Visual look at camera
        visual.eulerAngles = new Vector3(45, 45, visual.eulerAngles.z);
    }

    private void FixedUpdate() {
        Vector3 velocityMinusY = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
        velocityMinusY = Vector3.ClampMagnitude(velocityMinusY, model.moveMaxSpeed);
        rigid.velocity = new Vector3(velocityMinusY.x, rigid.velocity.y, velocityMinusY.z);

        rigid.velocity = Vector3.Lerp(rigid.velocity, new Vector3(0, rigid.velocity.y, 0), model.moveDeceleration);
    }

    public void Move (Vector2 direction) {
        Vector2 moveDir = InputManager.instance.GetMoveDirection(model.playerID);
        rigid.velocity += transform.forward * moveDir.y * model.moveAcceleration;
        rigid.velocity += transform.right * moveDir.x * model.moveAcceleration;
    }

    public void Aim (Vector2 direction) {
        if (model.equippedWeapon) {
            model.equippedWeapon.transform.right = new Vector3(direction.x, 0, direction.y);
        }
    }

    public void Attack () {
        if (model.equippedWeapon)   {
            model.equippedWeapon.Use();
        }
    }

    private void OnTriggerEnter(Collider col) {
        Weapon weapon = col.GetComponent<Weapon>();
        if (weapon != null) {
            model.equippedWeapon = weapon;
        }
    }
}
