﻿using System.Collections;
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
        GameObject closestGO = hits[0].collider.gameObject;
        for (int i=0; i < hits.Length; i ++)
        {
<<<<<<< HEAD
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

=======
>>>>>>> ba4a18346e28c4df3c1d033594a6e3798539f248
            // This is for selecting the closest qualifying collider. Since the RaycastHit[] array isn't ordered.
            if (hits[i].collider.GetComponent<Wall>() ||
                hits[i].collider.GetComponent<Enemy>())
            {
                if (Vector3.Distance(transform.position, hits[i].point) <= Vector3.Distance(transform.position, closest.point))
                {
                    closest = hits[i];
                    closestGO = hits[i].collider.gameObject;
                }
            }
        }

        UpdateVisual(transform.position, closest.point);

        Wall wall = closestGO.GetComponent<Wall>();
        Enemy enemy = closestGO.GetComponent<Enemy>();

        if (wall != null) {
            wall.Damage(damage);
        }
        if (enemy != null) {
            enemy.Damage(damage);
        }
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
