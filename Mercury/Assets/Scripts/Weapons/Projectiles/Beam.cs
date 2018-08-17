using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    // TODO: When we add different types of beams this class will need to be reworked.
    // Currently a lot of functionality that should be handled by the children classes lives here.

    public float width;
    public int damage = 1;
    protected Transform visual;

    public virtual void Init()
    {
        visual = transform.Find("Visual");
        Update();
    }

    public void Update()
    {
        CollisionCheck();
    }

    public void OnEnable()
    {
        CollisionCheck();
    }

    private void CollisionCheck()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(transform.position, transform.right));

        if (hits.Length <= 0)
        {
            return;
        }

        RaycastHit closest = hits[0];
        for (int i=0; i < hits.Length; i ++)
        {
            Wall wall = hits[i].collider.GetComponent<Wall>();
            Enemy enemy = hits[i].collider.GetComponent<Enemy>();

            if (wall != null)
            {
                wall.Damage(damage);
            }
            if (enemy != null)
            {
                enemy.Damage(damage);
            }

            // This is for selecting the closest qualifying collider. Since the RaycastHit[] array isn't ordered.
            if (wall != null || enemy != null)
            {
                if (Vector3.Distance(transform.position, hits[i].point) <= Vector3.Distance(transform.position, closest.point))
                {
                    closest = hits[i];
                }
            }
        }

        UpdateVisual(transform.position, closest.point);
    }

    public void UpdateVisual (Vector3 startPos, Vector3 endPos)
    {
        LineRenderer mainLR = visual.transform.Find("MainBeam").GetComponent<LineRenderer>();
        Vector3[] positions = {startPos, endPos};
        mainLR.positionCount = 2;
        mainLR.SetPositions(positions);
        mainLR.widthMultiplier = width;
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
}
