using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTBag : MonoBehaviour
{
    Rigidbody rigid;
    float blastRadius;
    float damage;

    private void Init()
    {
        blastRadius = 2f;
        damage = 100;
    }

    private void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        Init();
        AudioManager.instance.PlayAudio("abra", 1, false);
    }

    void Update()
    {
        if( rigid.transform.position.y <= 0.6f)
        {
            Explode();
        }
    }

    public void Throw(float power, Vector3 position)
    {
        rigid.AddExplosionForce(power, position, 10f,0.2f, ForceMode.Impulse);
    }

    //Destroying object and applying damage to colliding players ect.
    public void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, blastRadius);
        foreach(Collider hit in hits)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                Enemy enemyHit = hit.GetComponent<Enemy>();
                enemyHit.Damage(damage);
            }
            if (hit.GetComponent<PlayerActor>() != null)
            {
                PlayerActor player = hit.GetComponent<PlayerActor>();
                player.Damage(damage);
            }
            if (hit.GetComponent<Wall>() != null)
            {
                Wall wall = hit.GetComponent<Wall>();
                wall.Damage((int)damage);
            }
        }
        GameObject explosion = Factory.instance.CreateBagExplosion();//Explosion effect
        explosion.transform.position = rigid.transform.position;
        GameObject.Destroy(gameObject);
    }
}
