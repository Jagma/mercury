using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour {
    //TODO: Create a reusable system

    public PlayerActor playerActor;

    SpriteRenderer spriteRenderer;
    Rigidbody rigid;
    Transform visual;
    Animator animController;

    private void Awake() {
        visual = transform.Find("Visual");
        animController = visual.Find("Body").GetComponent<Animator>();
        spriteRenderer = visual.Find("Body").GetComponent<SpriteRenderer>();
    }

    void Start () {
        rigid = playerActor.GetComponent<Rigidbody>();
    }

	void Update () {
        // This is for setting the sprites based on the aim/look direction
        Vector2 aim = playerActor.model.lookDirection;
        Vector3 norm = Quaternion.AngleAxis(0, Vector3.up) * new Vector3(aim.x, 0, aim.y);

        // Back
        if (norm.z <= 0) {
            animController.SetBool("Front", true);
        } else {
            animController.SetBool("Front", false);
        }

        if (norm.x <= 0) {
            spriteRenderer.flipX = true;
        }
        else {
            spriteRenderer.flipX = false;
        }

        animController.SetFloat("Speed", rigid.velocity.magnitude);
        Debug.Log(norm);
    }
}
