using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTBag : MonoBehaviour
{
    Rigidbody rigid;
    float blastRadius;
    float damage;
    float explodeTime;
    float timer;
    bool explode;

    private void Init()
    {
        explodeTime = 0f;
        blastRadius = 2f;
        damage = 100;
        timer = 0.5f;
        explode = false;
    }

    private void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }
    void Start()
    {
        Init();
        AudioManager.instance.PlayAudio("Binladen_ability", 1, false);
    }

    void Update()
    {
        if( rigid.transform.position.y <= 0.7f && !explode)
        {
            rigid.velocity = Vector3.zero;
            rigid.position = new Vector3(rigid.position.x, 0.7f, rigid.position.z);
            rigid.useGravity = false;
            explodeTime = Time.time + timer;
            explode = true;
        }
        Debug.Log("Time " + Time.time + " EXLODE TIME " + explodeTime);
        if(Time.time >= explodeTime && explode)
        {
            Explode();
            Debug.Log("Exploded");
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
