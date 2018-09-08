using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public static Ammo instance;
    public int currentEquipedAmount = 0;
    public int maxAmmoAmount = 0;
    public int ammoCarryAmount = 0;
    public float reloadDelay = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
        // Use this for initialization
    void Start ()
    {
        if (currentEquipedAmount == -1)
            currentEquipedAmount = maxAmmoAmount;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void setAmmouAmount(int amount)
    {
        ammoCarryAmount += amount;
    }

    public int GetCurrentAmmoAmount()
    {
        return currentEquipedAmount;
    }

    public int GetAmmoCarryAmount()
    {
        return ammoCarryAmount;
    }
}
