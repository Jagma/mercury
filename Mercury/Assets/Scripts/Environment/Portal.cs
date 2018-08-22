using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider col)
    {
        PlayerActor player = col.GetComponent<PlayerActor>();
        if (player != null)
        {
            GameProgressionManager.instance.LevelComplete();
        }

    }
}
