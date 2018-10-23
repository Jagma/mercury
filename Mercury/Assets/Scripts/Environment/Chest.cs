using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite openSprite;
    public Sprite closedSprite;

    protected Transform visual;
    protected int result;
    protected int[] itemWeights;
    private Vector3 targetPos;
    int count = 1;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        visual = transform.Find("Visual");
        transform.position = Vector3.Lerp(transform.position, transform.position, 0.3f);
        visual.eulerAngles = new Vector3(45, 45, visual.eulerAngles.z);
        spriteRenderer = visual.Find("Body").GetComponent<SpriteRenderer>();
    }

    public void OpenChest()
    {
       Use();
   //    Destroy();
    }

    protected virtual void Use()
    {
        if (count >= 1)
        {
            count--;

            GameObject[] chestObjects = GetRandomObjects();

            for (int i = 0; i < chestObjects.Length; i++)
            {
                chestObjects[i].transform.position = transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0.5f, UnityEngine.Random.Range(-1f, 1f));
                chestObjects[i].GetComponent<Rigidbody>().AddForce(Vector3.up * 5 + UnityEngine.Random.onUnitSphere * 2, ForceMode.Impulse);
            }
        }

        spriteRenderer.sprite = openSprite;
    }

    protected virtual void Destroy()
    {
       DisplayEmptyChest();
       Destroy(gameObject);
    }

    private void DisplayEmptyChest()
    {
        visual.transform.parent = null;
        visual.transform.position = new Vector3(visual.transform.position.x, 0.7f, visual.transform.position.z);
        spriteRenderer.sprite = openSprite;
    }

    protected virtual GameObject[] GetRandomObjects()
    {
        int objectCount = 1;

        if (UnityEngine.Random.Range(0, 100) > 30)
        {
            objectCount += 1;
        }
        else
        if (UnityEngine.Random.Range(0, 100) > 50)
        {
            objectCount += 2;
        }
        else
        if (UnityEngine.Random.Range(0, 100) > 80)
        {
            objectCount += 3;
        }

        List<GameObject> objectList = new List<GameObject>();
        for (int i = 0; i < objectCount; i++)
        {
            objectList.Add(GetRandomItem());
        }

        return objectList.ToArray();
    }

    protected virtual GameObject GetRandomItem()
    {
        int total = 0;
        for (int i = 0; i < itemWeights.Length; i++)
        {
            total += itemWeights[i];
        }

        int random = UnityEngine.Random.Range(0, total);

        total = 0;

        for (int i = 0; i < itemWeights.Length; i++)
        {
            total += itemWeights[i];
            if (random >= total)
            {
                result = i;
                break;
            }
        }
        return null;
    }
}
