using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public float destroy =5f;
    protected Transform visual;
    private Vector3 targetPos;
    int count = 1;
    IEnumerator ChestDespawn()
    {
        yield return new WaitForSeconds(0.5f);
        Delete();
    }

    private void Awake()
    {
        visual = transform.Find("Visual");
    }

    public void OpenChest()
    {
       Use();
    }

    protected void Update()
    {
       transform.position = Vector3.Lerp(transform.position, transform.position, 0.3f);
    }

    protected virtual void Use()
    {
        //StartCoroutine(ChestDespawn());
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
            //OpenChest();
        }
    }
}
