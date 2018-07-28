using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour {

    public PlayerModel model;
    public static float bulletAmount = 0;
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
        if (model.equippedWeapon)   {
            if (bulletAmount > 0) {
                bulletAmount -= 1;
                model.equippedWeapon.Use();
            }
            else { Debug.Log("Weapon clip empty!"); }
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {

        if (col.GetComponent<MachineGun>() == true) {
                model.equippedWeapon = col.GetComponent<MachineGun>();
                bulletAmount = 60;
        }
        else if (col.GetComponent<Pistol>() == true)
        {
            model.equippedWeapon = col.GetComponent<Pistol>();
            bulletAmount = 25;
        }


    }

}
