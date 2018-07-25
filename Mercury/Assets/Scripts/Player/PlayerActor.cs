using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour {

    public PlayerModel model;

    Rigidbody2D rigid;
    void Start () {
        rigid = GetComponent<Rigidbody2D>();
    }

	void Update () {
        rigid.velocity = Vector2.ClampMagnitude(rigid.velocity, model.moveMaxSpeed);
        rigid.velocity = Vector2.Lerp(rigid.velocity, Vector2.zero, model.moveDeceleration);

        if (model.equippedWeapon) {
            model.equippedWeapon.transform.position = transform.position + model.equippedWeapon.transform.right;
        }
    }

    public void Move (Vector2 direction) {
        rigid.velocity += InputManager.instance.GetMoveDirection(model.playerID) * model.moveAcceleration;
    }

    public void Aim (Vector2 direction) {
        if (model.equippedWeapon) {
            model.equippedWeapon.transform.right = direction;
        }
    }

    public void Attack () {
        if (model.equippedWeapon) {
            model.equippedWeapon.Use();
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Weapon weapon = col.GetComponent<Weapon>();
        if (weapon) {
            model.equippedWeapon = weapon;
        }        
    }


}
