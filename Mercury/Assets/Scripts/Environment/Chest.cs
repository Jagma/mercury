using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public float destroy =5f;
    protected Transform visual;
    private Vector3 targetPos;

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
       visual.eulerAngles = new Vector3(45, 45, visual.eulerAngles.z);
    }

    private void LookAtPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 7.5f);

        PlayerActor closestPlayerActor = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerActor playerActor = colliders[i].GetComponent<PlayerActor>();

            // Is this collider a player
            if (playerActor != null)
            {
                if (closestPlayerActor == null)
                {
                    closestPlayerActor = playerActor;
                }
                // Is this player closer than the current closest player
                if (Vector3.Distance(playerActor.transform.position, transform.position) <
                    Vector3.Distance(closestPlayerActor.transform.position, transform.position))
                {
                    closestPlayerActor = playerActor;
                }
            }
        }

        if (closestPlayerActor != null)
        {
            visual.eulerAngles = new Vector3(45, 45, closestPlayerActor.transform.eulerAngles.z);
        }
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
