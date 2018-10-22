using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public double health = 100;
    public float moveSpeed = 1f;
    public Weapon equippedWeapon;
    Transform visual;
    Transform dropShadow;
    Rigidbody rigid;

    public Vector3 forwardDirection = Vector3.forward;
    System.Random ran = new System.Random(85466248);

    SpriteRenderer spriteRenderer;
    Material hitMaterial;
    Material normalMaterial;

    private void Awake()
    {
        visual = transform.Find("Visual");
        rigid = GetComponent<Rigidbody>();
        dropShadow = Factory.instance.CreateDropShadow().transform;
        dropShadow.parent = visual;
        dropShadow.localPosition = new Vector3(0, 0, 0);
        dropShadow.localScale = new Vector3(0.6f, 0.2f, 1);

        spriteRenderer = visual.Find("Body").GetComponent<SpriteRenderer>();
        hitMaterial = Factory.instance.CreateHitFlash();
        normalMaterial = spriteRenderer.material;
    }

    protected virtual void Start()
    {
    }

    //Forces enemy to look at camera
    protected virtual void FixedUpdate()
    {
        visual.eulerAngles = new Vector3(45, 45, 0);
        dropShadow.localPosition = new Vector3(0, 0, -0.5f);
    }

    // This method needs to be overriden for each enemy
    protected virtual void Attack()
    {

    }

    // AI interface methods
    protected void FaceDirection(Vector3 directionVector)
    {
        forwardDirection = directionVector;
    }

    protected void MoveForward()
    {
        Vector3 moveDir = new Vector3(forwardDirection.x, Mathf.Clamp(rigid.velocity.y, -100, 0), forwardDirection.z);
        rigid.velocity = moveDir * moveSpeed;
    }


    // Damage, health, and death
    private void OnTriggerEnter(Collider col)
    {
        Projectile projectile = col.GetComponent<Projectile>();
        if (projectile != null)
        {        
            Damage(projectile.damage);
        }
    }

    public void Damage(double damage)
    {
        GameObject blood = Factory.instance.CreateBlood();
        blood.transform.position = this.transform.position;
        GameObject.Destroy(blood, 5);

        health -= damage;
        if (health <= 0)
        {
            Death();
        } else {
            StartCoroutine(HitFlash());
        }
        
    }

    IEnumerator HitFlash() {
        spriteRenderer.material = hitMaterial;

        yield return new WaitForSeconds(0.05f);

        spriteRenderer.material = normalMaterial;
    }

    protected virtual void Death()
    {
        if (equippedWeapon) //has a chance to drop weapon or ammo pack.
        {
            DropItems();
        }
        DisplayEnemyCorpse();
        GameProgressionManager.instance.IncreaseEnemyKills();
        GameProgressionManager.instance.DecreaseEnemiesLeft();
        Destroy(gameObject);
    }

    private void DisplayEnemyCorpse()
    {
        visual.transform.parent = null;
        if (Random.Range(0, 100) >= 50) {
            visual.transform.localEulerAngles = new Vector3(45, 45, -90);
        } else {
            visual.transform.localEulerAngles = new Vector3(45, 45, 90);
        }

        visual.transform.position = new Vector3(visual.transform.position.x, 0.7f, visual.transform.position.z);

        Destroy(visual.Find("Drop Shadow").gameObject);

        spriteRenderer.material = normalMaterial;
        Color deadColor = spriteRenderer.color;
        deadColor *= 0.6f;
        deadColor.a = 1;
        spriteRenderer.color = deadColor;
        Destroy(spriteRenderer, 10);
    }

    private void DropItems()
    {
        int randomNum = ran.Next(0, 100);
        if (randomNum >= 20 && randomNum < 61)
        {
            GameObject randomAmmoPack;
            randomNum = ran.Next(0, 100);
            if (randomNum >= 0 && randomNum < 11)//10% Laser Rifle ammo
            {
                randomAmmoPack = Factory.instance.CreateBeamAmmoPack();
            }
            else if (randomNum > 20 && randomNum < 61) //40% rocket launcher ammo
            {
                randomAmmoPack = Factory.instance.CreateBulletAmmoPack();
            }
            else //if the random value is else.
            {
                randomAmmoPack = Factory.instance.CreateRocketAmmoPack();
            }
            randomAmmoPack.transform.position = transform.position;

            //removes weapon from being used.
            equippedWeapon.Dequip();
            equippedWeapon.equipped = false;
            equippedWeapon = null;
        }
        else
        {
            equippedWeapon.Dequip();
            equippedWeapon.equipped = false;
            equippedWeapon = null;
        }

    }
}
