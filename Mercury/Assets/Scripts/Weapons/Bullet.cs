using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 10;	
	void Update () {
        transform.position += transform.right * Time.deltaTime * speed;
    }
}
