using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : Weapon
{
    protected bool attack = false;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Use()
    {
        base.Use();

        StartCoroutine(Swing());
    }

    protected override void Update()
    {
        base.Update();
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.right * 0.5f, 0.4f);
    }

    protected override void UpdateVisual() {
        if (attack==false) {
            base.UpdateVisual();
        }
    }

    IEnumerator Swing () {
        attack = true;
        float swingTime = 0.2f;
        float attackTime = 0.08f;
        Vector3 angles = visual.transform.localEulerAngles;

        visual.transform.localEulerAngles += new Vector3(0, 0, 10);

        while (swingTime > 0) {
            swingTime -= Time.deltaTime;
            if (swingTime > 0.05) {
                visual.transform.localEulerAngles = 
                    new Vector3(angles.x, angles.y, Mathf.LerpAngle(visual.transform.localEulerAngles.z, angles.z - 120, 0.5f));
            }

            if (swingTime <= attackTime) {
                attackTime = -1f;
                MeleeDamage();
            }

            yield return new WaitForEndOfFrame ();
        }
        attack = false;
    }

    protected void MeleeDamage () {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.right * 0.5f, 0.4f);

        for (int i=0; i < hits.Length; i ++) {
            Enemy enemy = hits[i].GetComponent<Enemy>();
            Wall wall = hits[i].GetComponent<Wall>();

            if (enemy != null) {
                enemy.Damage(damage);
            }

            if (wall != null) {
                wall.Damage(damage);
            }

        }
    }
}