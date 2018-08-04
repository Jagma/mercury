﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    // The factory is a singleton
    public static Factory instance;
    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    // Factory methods
    public GameObject CreatePlayer () {
        GameObject playerGO = new GameObject("Player");

        CapsuleCollider playerCollider = playerGO.AddComponent<CapsuleCollider>();
        playerCollider.radius = 0.25f;
        playerCollider.height = 0.8f;

        PhysicMaterial pm = new PhysicMaterial();
        pm.frictionCombine = PhysicMaterialCombine.Minimum;
        pm.dynamicFriction = 0;
        pm.staticFriction = 0;
        playerCollider.material = pm;

        Rigidbody playerRigid = playerGO.AddComponent<Rigidbody>();
        playerRigid.interpolation = RigidbodyInterpolation.Interpolate;
        playerRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject playerDropShadowGO = Factory.instance.CreateDropShadow();
        playerDropShadowGO.transform.parent = playerGO.transform;
        playerDropShadowGO.transform.localPosition = new Vector3(0, 0, -0.3f);
        playerDropShadowGO.transform.localScale = new Vector3(0.6f, 0.2f, 1);

        GameObject playerVisualGO = new GameObject("Visual");
        playerVisualGO.transform.parent = playerGO.transform;

        GameObject playerVisualBodyGO = new GameObject("Body");
        playerVisualBodyGO.transform.parent = playerVisualGO.transform;
        SpriteRenderer sr = playerVisualBodyGO.AddComponent<SpriteRenderer>();

        PlayerActor playerActor = playerGO.AddComponent<PlayerActor>();
        playerActor.facing = sr.sprite = Resources.Load<Sprite>("Sprites/Characters/character_1");
        playerActor.forward = sr.sprite = Resources.Load<Sprite>("Sprites/Characters/character_1B");

        playerGO.AddComponent<PlayerController>();
        return playerGO;
    }
    
    public GameObject CreateDropShadow() {
        GameObject dropShadowGO = new GameObject("Drop Shadow");

        SpriteRenderer sr = dropShadowGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Random/DropShadow");
        sr.color = new Color(0, 0, 0, 0.5f);

        dropShadowGO.AddComponent<DropShadow>();
        return dropShadowGO;
    }


    public GameObject CreateBullet()
    {
        GameObject bulletGO = new GameObject("Bullet");

        SphereCollider bulletCollider = bulletGO.AddComponent<SphereCollider>();
        bulletCollider.isTrigger = true;
        bulletCollider.radius = 0.2f;

        Rigidbody bulletRigid = bulletGO.AddComponent<Rigidbody>();
        bulletRigid.isKinematic = true;

        GameObject bulletVisualGO = new GameObject("Visual");
        bulletVisualGO.transform.parent = bulletGO.transform;

        GameObject bulletVisualBodyGO = new GameObject("Body");
        bulletVisualBodyGO.transform.parent = bulletVisualGO.transform;
        SpriteRenderer sr = bulletVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/Bullet_1");

        Projectile p = bulletGO.AddComponent<Round>();
        p.Init();
        return bulletGO;
    }

    public GameObject CreateBulletHit ()
    {
        GameObject bulletHit = GameObject.Instantiate(Resources.Load<GameObject>("Effects/BulletHit"));
        return bulletHit;
    }

    public GameObject CreateRocketHit()
    {
        GameObject rocketHit = GameObject.Instantiate(Resources.Load<GameObject>("Effects/RocketHit"));
        return rocketHit;
    }
    public GameObject CreateRocketSmokeFlash()
    {
        GameObject smokeFlash = GameObject.Instantiate(Resources.Load<GameObject>("Effects/RocketSmokeFlash"));
        return smokeFlash;
    }

    public GameObject CreateMuzzleFlash()
    {
        GameObject muzzleFlash = GameObject.Instantiate(Resources.Load<GameObject>("Effects/MuzzleFlash"));
        return muzzleFlash;
    }

    public GameObject CreatePistol () {
        GameObject pistolGO = new GameObject("Pistol");

        SphereCollider pistolCollider = pistolGO.AddComponent<SphereCollider>();
        pistolCollider.radius = 0.1f;

        SphereCollider pistolColliderT = pistolGO.AddComponent<SphereCollider>();
        pistolColliderT.isTrigger = true;

        Rigidbody pistolRigid = pistolGO.AddComponent<Rigidbody>();
        pistolRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject pistolVisualGO = new GameObject("Visual");
        pistolVisualGO.transform.parent = pistolGO.transform;

        GameObject pistolVisualBodyGO = new GameObject("Body");
        pistolVisualBodyGO.transform.parent = pistolVisualGO.transform;
        SpriteRenderer sr = pistolVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/Pistol");

        pistolGO.AddComponent<Pistol>();

        return pistolGO;
    }

    public GameObject CreateMachineGun() {
        GameObject machineGunGO = new GameObject("Machine Gun");

        SphereCollider machineGunCollider = machineGunGO.AddComponent<SphereCollider>();
        machineGunCollider.radius = 0.1f;

        SphereCollider machineGunColliderT = machineGunGO.AddComponent<SphereCollider>();
        machineGunColliderT.isTrigger = true;

        Rigidbody machineGunRigid = machineGunGO.AddComponent<Rigidbody>();
        machineGunRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject machineGunVisualGO = new GameObject("Visual");
        machineGunVisualGO.transform.parent = machineGunGO.transform;

        GameObject machineGunVisualBodyGO = new GameObject("Body");
        machineGunVisualBodyGO.transform.parent = machineGunVisualGO.transform;
        SpriteRenderer sr = machineGunVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/MachineGun");

        machineGunGO.AddComponent<MachineGun>();

        return machineGunGO;
    }

    public GameObject CreateRocketLauncher() {
        GameObject rocketLauncherGO = new GameObject("Rocket Launcher");

        SphereCollider rocketLauncherCollider = rocketLauncherGO.AddComponent<SphereCollider>();
        rocketLauncherCollider.radius = 0.1f;

        SphereCollider rocketLauncherColliderT = rocketLauncherGO.AddComponent<SphereCollider>();
        rocketLauncherColliderT.isTrigger = true;

        Rigidbody rocketLauncherRigid = rocketLauncherGO.AddComponent<Rigidbody>();
        rocketLauncherRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject rocketLauncherVisualGO = new GameObject("Visual");
        rocketLauncherVisualGO.transform.parent = rocketLauncherGO.transform;

        GameObject rocketLauncherVisualBodyGO = new GameObject("Body");
        rocketLauncherVisualBodyGO.transform.parent = rocketLauncherVisualGO.transform;
        SpriteRenderer sr = rocketLauncherVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/RocketLauncher");

        rocketLauncherGO.AddComponent<RocketLauncher>();

        return rocketLauncherGO;
    }

    public GameObject CreateRocket() {
        GameObject rocketGO = new GameObject("Rocket");

        SphereCollider rocketCollider = rocketGO.AddComponent<SphereCollider>();
        rocketCollider.isTrigger = true;
        rocketCollider.radius = 0.2f;

        Rigidbody bulletRigid = rocketGO.AddComponent<Rigidbody>();
        bulletRigid.isKinematic = true;

        GameObject bulletVisualGO = new GameObject("Visual");
        bulletVisualGO.transform.parent = rocketGO.transform;

        GameObject bulletVisualBodyGO = new GameObject("Body");
        bulletVisualBodyGO.transform.parent = bulletVisualGO.transform;
        SpriteRenderer sr = bulletVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/Bullet_Rocket");

        rocketGO.AddComponent<RPG>();
        return rocketGO;
    }

    public GameObject CreateFloor() {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";

        Material mf = new Material(Shader.Find("Mobile/Diffuse"));
        mf.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Environment/Mercury/Floor"));

        floor.GetComponent<Renderer>().material = mf;
        return floor;
    }

    Material m = null;
    public GameObject CreateWall ()
    {
        GameObject wall = new GameObject("Wall");
        wall.AddComponent<BoxCollider>();
        wall.AddComponent<Wall>();

        if (m == null) {
            m = new Material(Shader.Find("Mobile/Diffuse"));
            m.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Environment/Mercury/Voxel"));
        }

        Voxel wallVoxel = wall.AddComponent<Voxel>();
        wallVoxel.material = m;
        wallVoxel.Init();
        return wall;
    }

    public GameObject CreateEnemyWalker () {
        GameObject enemyWalkerGO = new GameObject("Enemy Walker");

        CapsuleCollider enemyWalkerCollider = enemyWalkerGO.AddComponent<CapsuleCollider>();
        enemyWalkerCollider.radius = 0.25f;
        enemyWalkerCollider.height = 0.8f;

        Rigidbody enemyWalkerRigid = enemyWalkerGO.AddComponent<Rigidbody>();
        enemyWalkerRigid.interpolation = RigidbodyInterpolation.Interpolate;
        enemyWalkerRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject enemyWalkerVisualGO = new GameObject("Visual");
        enemyWalkerVisualGO.transform.parent = enemyWalkerGO.transform;

        GameObject enemyWalkerVisualBodyGO = new GameObject("Body");
        enemyWalkerVisualBodyGO.transform.parent = enemyWalkerVisualGO.transform;

        SpriteRenderer sr = enemyWalkerVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Enemies/Walker");

        enemyWalkerGO.AddComponent<Enemy>();
        return enemyWalkerGO;
    }
}