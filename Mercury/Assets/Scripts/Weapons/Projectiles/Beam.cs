using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public float speed = 100;
    public int damage = 1;
    public RaycastHit2D hitEffect;
    LineRenderer lineRend;
    protected Transform visual;

    public virtual void Init()
    {
        visual = transform.Find("Visual");
        lineRend = this.GetComponent<LineRenderer>();
        lineRend.useWorldSpace = true;
        lineRend.enabled = true;
        Destroy(gameObject, 5f);
        Update();
    }

    public void Update()
    {
        lineRend.SetPosition(0, transform.position);
        lineRend.SetPosition(1, visual.position);

        hitEffect = Physics2D.Raycast(transform.position, transform.right * Time.deltaTime * speed);
        visual.position = hitEffect.point;
    }

    private void OnTriggerEnter(Collider col)
    {
        Wall wall = col.GetComponent<Wall>();
        if (wall != null)
        {
            Destroy();
        }

        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null)
        {
            Destroy();
        }
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
}
