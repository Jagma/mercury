using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{  
    public float speed = 20;
    public float damage = 1;
    protected Transform visual;
    
    public virtual void Init()
    {
        visual = transform.Find("Visual");

        visual.GetChild(0).localPosition = new Vector3(0, 0, -0.1f);

        Destroy(gameObject, 5f);
        Update();
        
    }
    public void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
        visual.eulerAngles = new Vector3(45, 45, Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg + 45);
    }

    public virtual void Destroy() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        Wall wall = col.GetComponent<Wall>();
        if (wall != null)
        {
            wall.Damage(damage);
            Destroy();
        }

        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Damage(damage);
            Destroy();
        }

        PlayerActor player = col.GetComponent<PlayerActor>();
        if (player != null) {
            player.Damage(damage);
            Destroy();
        }
    }
}
