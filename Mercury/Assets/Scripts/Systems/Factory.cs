using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {

    // The factory is a singleton
    public static Factory instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    // Factory methods
    public GameObject CreatePlayer () {
        GameObject player = GameObject.Instantiate(playerPrefab);
        return player;
    }

    public GameObject CreatePistol () {
        GameObject pistol = GameObject.Instantiate(pistolPrefab);
        return pistol;
    }

    public GameObject CreateBullet() {
        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        return bullet;
    }

    // Factory objects
    public GameObject playerPrefab;
    public GameObject pistolPrefab;
    public GameObject bulletPrefab;
}
