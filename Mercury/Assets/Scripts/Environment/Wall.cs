using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int health = 100;

    private void OnTriggerEnter(Collider col)
    {
        Projectile p = col.GetComponent<Projectile>();
        Beam b = col.GetComponent<Beam>();
        if (p != null)
        {
            Damage(p.damage);
        }
        else if (b != null)
        {
            Damage(b.damage);
        }
    }

    public void Damage (int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
            GameObject wallBreak = Factory.instance.CreateBrokenWallEffect();
            wallBreak.transform.position = gameObject.transform.position; 
            Destroy(wallBreak, 0.7f);
        }
    }
}