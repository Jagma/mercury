using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int health = 100;

    public void Damage (int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
            GameObject wallBreak = Factory.instance.CreateBrokenWallEffect();
            wallBreak.transform.position = gameObject.transform.position; 
            Destroy(wallBreak, 0.7f);
        }
    }
}