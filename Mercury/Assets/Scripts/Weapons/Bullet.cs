using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    /* We will need to abstract this class once we add more bullet types. Similar to weapons.     * 
     */
    Transform visual;
    public float speed = 5;
    public int damage = 1;

    private void Awake()
    {
        visual = transform.Find("Visual");
    }

    private void Start()
    {
        Destroy(gameObject, 5f);
        visual.eulerAngles = new Vector3(45, 45, Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg + 45);
    }

    void Update () {
        transform.position += transform.right * Time.deltaTime * speed;
        visual.eulerAngles = new Vector3(45, 45, Mathf.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg + 45);
    }

    private void OnTriggerEnter(Collider col)
    {
        Wall wall = col.GetComponent<Wall>();
        if (wall != null)
        {
            Destroy(gameObject);
        }

        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null) {
            Destroy(gameObject);
        }
    }
}