using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    protected Transform visual;
    protected int ammoAmount = 0;

    private void Awake()
    {
        visual = transform.Find("Visual");
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position, 0.3f);
        visual.eulerAngles = new Vector3(45, 45, visual.eulerAngles.z);
    }


    protected virtual void Use(PlayerActor player)
    {

    }

    protected virtual void Delete()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {

        PlayerActor player = col.GetComponent<PlayerActor>();
        if (player != null)
        {
            Use(player);
        }
    }
}
