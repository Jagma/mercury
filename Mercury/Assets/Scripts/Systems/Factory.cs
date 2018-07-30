using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{

    // The factory is a singleton
    public static Factory instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    // Factory methods
    public GameObject CreatePlayer ()
    {
        GameObject player = GameObject.Instantiate(playerPrefab);
        return player;
    }
    
    public GameObject CreateDropShadow() {
        GameObject dropShadow = GameObject.Instantiate(dropShadowPrefab);
        return dropShadow;
    }


    public GameObject CreateBullet() {
        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        return bullet;
    }

    public GameObject CreatePistol () {
        GameObject pistol = GameObject.Instantiate(pistolPrefab);
        return pistol;
    }

    public GameObject CreateMachineGun()
    {
        GameObject machineGun = GameObject.Instantiate(machineGunPrefab);
        return machineGun;
    }

    public GameObject CreateRocketLauncher() {
        GameObject rocketLauncher = GameObject.Instantiate(rocketLauncherPrefab);
        return rocketLauncher;
    }

    public GameObject CreateRocketBullet() {
        GameObject rocketBullet = GameObject.Instantiate(rocketBulletPrefab);
        return rocketBullet;
    }

    public GameObject CreateFloor() {
        GameObject floor = GameObject.Instantiate(floorPrefab);
        return floor;
    }
 

    public GameObject CreateWall ()
    {
        GameObject wall = GameObject.Instantiate(wallPrefab);
        return wall;
    }

    public GameObject CreateEnemyWalker () {
        GameObject enemyWalker = GameObject.Instantiate(enemyWalkerPrefab);
        return enemyWalker;
    }

    // Factory objects
    public GameObject playerPrefab;
    public GameObject dropShadowPrefab;

    public GameObject bulletPrefab;
    public GameObject pistolPrefab;
    public GameObject machineGunPrefab;
    public GameObject rocketLauncherPrefab;
    public GameObject rocketBulletPrefab;

    public GameObject floorPrefab;
    public GameObject wallPrefab;

    public GameObject enemyWalkerPrefab;
}