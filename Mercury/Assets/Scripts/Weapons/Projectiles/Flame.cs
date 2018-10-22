using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    public float speed = 20;
    public float damage = 1;
    Transform visual;

    public void Init()
    {
        // Stats     
        speed = 10f;
        damage = 1f;
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }

    //Destroying object and applying damage to colliding players ect.
    public void Burn()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, 10f);
        foreach (Collider hit in hits)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy enemyHit = hit.GetComponent<Enemy>();
                enemyHit.Damage(damage);
            }

            if (hit.GetComponent<Wall>() != null)
            {
                Wall wall = hit.GetComponent<Wall>();
                wall.Damage((int)damage);
            }
        }
        GameObject explosion = Factory.instance.CreateBagExplosion();//Explosion effect
        //explosion.transform.position = rigid.transform.position;
        GameObject.Destroy(gameObject);
    }

}
