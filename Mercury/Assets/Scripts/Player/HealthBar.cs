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

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (healthbar != null)
        {
            float health = PlayerActor.instance.GetHealthInformation();
            healthbar.fillAmount = health / 100;
            if (health > 75 && health <= 100)
            {
                healthbar.color = new Color(0, 255, 0);
            }
            if (health > 50 && health <= 75)
            {
                healthbar.color = new Color(200, 255, 0);
            }
            if (health > 25 && health <= 50)
            {
                healthbar.color = new Color(255, 167, 0);
            }
            if (health > 0 && health <= 25)
            {
                healthbar.color = new Color(255, 0, 0);
            }
        }

    }

    private float DeterminePos(float currentHealth)
    {
        return currentHealth * 0.73f - 73f;
    }
}
