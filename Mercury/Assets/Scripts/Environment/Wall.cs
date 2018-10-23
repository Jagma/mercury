using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float health = 100;

    public void Damage (float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
            GameObject wallBreak = Factory.instance.CreateWallBreak(ProgressionState.environmentName);
            wallBreak.transform.position = gameObject.transform.position;
            GameProgressionManager.instance.IncreaseWallsDestroyed();
            Destroy(wallBreak, 2f);
        }
    }
}