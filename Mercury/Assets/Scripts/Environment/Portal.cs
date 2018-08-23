using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public static Portal instance;
    private Vector3 targetPos;
    // Use this for initialization
    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Vector3 playerPos = PlayerActor.instance.transform.position;
        Vector3 playerDirection = PlayerActor.instance.transform.forward;
        Quaternion playerRotation = PlayerActor.instance.transform.rotation;
        float spawnDistance = 3;
        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
        transform.position = spawnPos;
        targetPos = transform.position;
    }
    // Update is called once per frame
    void Update ()
    {
        CollisionDetection();
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.3f);
    }

    private void OnTriggerEnter(Collider col)
    {

    }

    private void CollisionDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.2f);

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerActor playerActor = colliders[i].GetComponent<PlayerActor>();

            // Is this collider a player
            if (playerActor != null)
            {
                Debug.Log("Player collide with portal.");
                EnterPortal();
            }

        }
    }

    public void EnterPortal()
    {
       GameProgressionManager.instance.LevelComplete();
    }
}
