using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : Weapon
{
    protected bool attack = false;
    protected override void InitWeaponStats()
    {
        base.InitWeaponStats();
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

    public void OnDrawGizmos()
    { 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.right * 0.5f, 0.4f);
    }

    protected override void UpdateVisual()
    {
        if (attack==false)
        {
            base.UpdateVisual();
        }
    }

    void PlayMeleeSwingAudio()
    {
        int weaponIndex = Random.Range(0, 4);
        switch (weaponIndex)
        {
            case 0:
                AudioManager.instance.PlayAudio("Melee_use1", 1, false);
                break;
            case 1:
                AudioManager.instance.PlayAudio("Melee_use2", 1, false);
                break;
            case 2:
                AudioManager.instance.PlayAudio("Melee_use3", 1, false);
                break;
            case 3:
                AudioManager.instance.PlayAudio("Melee_use4", 1, false);
                break;
        }
    }
    IEnumerator Swing ()
    {
        attack = true;
        float swingTime = 0.2f;
        float attackTime = 0.08f;
        Vector3 angles = visual.transform.localEulerAngles;

        visual.transform.localEulerAngles += new Vector3(0, 0, 10);
        PlayMeleeSwingAudio();
        while (swingTime > 0) {
            swingTime -= Time.deltaTime;
            if (swingTime > 0.05) {
                visual.transform.localEulerAngles = 
                    new Vector3(angles.x, angles.y, Mathf.LerpAngle(visual.transform.localEulerAngles.z, angles.z - 120, 0.5f));
            }

            if (swingTime <= attackTime)
            {
                attackTime = -1f;
                MeleeDamage();
            }

            yield return new WaitForEndOfFrame ();
        }

        attack = false;
    }

    protected void MeleeDamage ()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.right * 0.5f, 0.4f);
        int randomClipInt;
        for (int i=0; i < hits.Length; i ++)
        {
            Enemy enemy = hits[i].GetComponent<Enemy>();
            Wall wall = hits[i].GetComponent<Wall>();
            PlayerActor playerA = hits[i].GetComponent<PlayerActor>();
            if (enemy != null)
            {                
                enemy.Damage(damage);
                randomClipInt = Random.Range(0, 1);
                switch (randomClipInt)
                {
                    case 0:
                        AudioManager.instance.PlayAudio("Melee_enemy_hit1", 1f, false);
                        break;

                    case 1:
                        AudioManager.instance.PlayAudio("Melee_enemy_hit2", 1f, false);
                        break;
                }
            }


            if (wall != null)
            {
                randomClipInt = Random.Range(0, 1);
                switch (randomClipInt)
                {
                    case 0:
                        AudioManager.instance.PlayAudio("Melee_wall_hit1", 1f, false);
                        break;

                    case 1:
                        AudioManager.instance.PlayAudio("Melee_wall_hit2", 1f, false);
                        break;
                }

                wall.Damage(damage);
            }
            
                if (playerA != null)
                { 
                    playerA.Damage(damage);
                }
        }
    }
}