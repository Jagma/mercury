using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public float destroy =5f;
    protected Transform visual;
    private Vector3 targetPos;

    private void Awake()
    {
        visual = transform.Find("Visual");

        transform.position = Vector3.Lerp(transform.position, transform.position, 0.3f);
        visual.eulerAngles = new Vector3(45, 45, visual.eulerAngles.z);
    }

    public void OpenChest()
    {
       Use();
       Destroy();
    }

    protected virtual void Use()
    {

    }

    protected virtual void Destroy()
    {
       Destroy(gameObject);
    }
}
