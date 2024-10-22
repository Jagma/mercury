﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    Rigidbody rigid;
    SpriteRenderer sr;
    void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
        rigid = gameObject.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if(rigid != null)
        {
            if (rigid.transform.position.y < 0f)
            {
                sr.enabled = true;
                sr.transform.position = new Vector3(rigid.transform.position.x, 0.6f, rigid.transform.position.z);
                Destroy(rigid);
                Destroy(gameObject, 5f);
            }
        }
	}
}
