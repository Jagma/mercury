using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    protected Transform visual;
    protected float healAmount = 100f;

    protected virtual void Start()
    {

    }

    private void Awake()
    {
        visual = transform.Find("Visual");
    }

    protected void Update()
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
        if (player != null && player.model.health < player.model.maxHealth)
        {
            Use(player);
        }
    }
}
