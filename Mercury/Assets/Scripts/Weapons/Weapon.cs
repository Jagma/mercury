using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float cooldown = 0.1f;
    public float missChance = 0f;
    public bool equipped = false;
    protected Transform visual;
    protected float cooldownRemaining = 0f;

    private void Awake()
    {
        visual = transform.Find("Visual");
    }

    protected virtual void Start()
    {

    }

    public void Equip()
    {
        GetComponent<Rigidbody>().useGravity = false;
        equipped = true;
    }

    public void setMissChance(float value)
    {
        missChance = value;
    }
    public void Dequip()
    {
        GetComponent<Rigidbody>().useGravity = true;
        equipped = false;
    }

    public void UseWeapon()
    {
        if (cooldownRemaining > 0)
        {
            return;
        }
        else
        {
            cooldownRemaining = cooldown;
            Use();
        }
    }

    protected virtual void Use()
    {
    }
    
    protected virtual void Update()
    {
        // Cooldown
        if (cooldownRemaining >= 0)
        {
            cooldownRemaining -= Time.deltaTime;
        }


        // Visual look at camera
        visual.eulerAngles = new Vector3(45, 45, -transform.eulerAngles.y + 45);

        Vector2 norm = Quaternion.AngleAxis(-45, Vector3.up) * transform.right;
        if (norm.x < 0)
        {
            Vector3 x = Quaternion.AngleAxis(180, visual.right) * visual.forward;
            visual.forward = x;
            visual.eulerAngles = new Vector3(visual.eulerAngles.x, visual.eulerAngles.y, 180+ transform.eulerAngles.y -45);
        }
    }
}
