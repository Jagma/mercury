using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour {

    Rigidbody rigid;
    SpriteRenderer bloodVisual;
    float time;
    //ParticleSystem ps;
    private void Awake()
    {
        time = -1;
        rigid = gameObject.GetComponent<Rigidbody>();
        bloodVisual = gameObject.GetComponent<SpriteRenderer>();
        //ps = gameObject.GetComponent<ParticleSystem>();
    }
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (rigid.position.y <= 1f && rigid.useGravity ==  true)
        {
            time = 0;
            bloodVisual.sprite = Resources.Load<Sprite>("Sprites/Environment/Blood");
            bloodVisual.transform.Rotate(axis: Vector3.left, angle: 90f);
            rigid.useGravity = false;
            rigid.position = new Vector3(rigid.position.x, 0.6f, rigid.position.z);
        }

        if (time > -1)
            time += Time.deltaTime;
        if (time > 5f)
            HideBlood();

    }

    private void HideBlood()
    {
        this.gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }
}
