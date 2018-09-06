using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{

    [Header("Unity")]
    public Image healthbar;

    Vector3 localScale;
	// Use this for initialization
	void Start ()
    {
        //localScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float health = PlayerActor.instance.GetHealthInformation();
        healthbar.fillAmount = health / 100;

        //this.transform.localScale = new Vector3(test, 1f, 1f);
    }
}
