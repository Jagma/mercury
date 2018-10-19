using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public PlayerModel playerModel;
    Image healthbar;    
	void Start ()
    {
        healthbar = transform.Find("Inner").GetComponent<Image>();
	}	

	void Update ()
    {
        if (playerModel != null)
        {
            float maxHealth = playerModel.maxHealth; 
            float health = playerModel.health;
            healthbar.fillAmount = health / playerModel.maxHealth;
            if (health > maxHealth * 0.75 && health <= maxHealth)
            {
                healthbar.color = new Color(0, 255, 0);
            }
            if (health > maxHealth * 0.5 && health <= maxHealth * 0.75)
            {
                healthbar.color = new Color(200, 255, 0);
            }
            if (health > maxHealth * 0.25 && health <= maxHealth * 0.5)
            {
                healthbar.color = new Color(255, 167, 0);
            }
            if (health > maxHealth * 0 && health <= maxHealth * 0.25)
            {
                healthbar.color = new Color(255, 0, 0);
            }
        }
    }

    private float DeterminePos(float currentHealth)
    {
        // return currentHealth * 0.73f - 73f;
        return currentHealth * 1f - 150;
    }
}
