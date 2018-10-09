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
    public GameObject CreatePlayerBase()
    {
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
        playerGO.AddComponent<SpriteRenderer>();

        /* GameObject playerHealthBarGO = new GameObject("Health Bar");
         playerHealthBarGO.transform.parent = playerVisualGO.transform;
         playerHealthBarGO.transform.localPosition = new Vector3(0,0.5f, -0.3f); // (-2.15f, 3, -0.3f);
         playerHealthBarGO.transform.localScale = new Vector3(1f, 0.1f, 1);
         SpriteRenderer srHB = playerHealthBarGO.AddComponent<SpriteRenderer>();
         playerHealthBarGO.AddComponent<HealthBar>();*/

        GameObject health = GameObject.FindGameObjectWithTag("HealthBars");
        health.transform.SetParent(playerGO.transform, true);
        PlayerActor playerActor = playerGO.AddComponent<PlayerActor>();
        playerActor.facing = sr.sprite = Resources.Load<Sprite>("Sprites/Characters/character_Trump");
        playerActor.forward = sr.sprite = Resources.Load<Sprite>("Sprites/Characters/character_TrumpB");
        playerActor.death = sr.sprite = Resources.Load<Sprite>("Sprites/Characters/ded");
        PlayerController playerController = playerGO.AddComponent<PlayerController>();
        playerController.actor = playerActor;

        PlayerModel playerModel = new PlayerModel();
        playerActor.model = playerModel;

        return playerGO;
    }

    public GameObject CreatePlayerTrump ()
    {
        GameObject playerGO = CreatePlayerBase();

        AbilityTrump abilityTrump = new AbilityTrump();
        abilityTrump.playerActor = playerGO.GetComponent<PlayerActor>();
        abilityTrump.Init();

        // Set all trump specific stats
        playerGO.GetComponent<PlayerActor>().model.ability = abilityTrump;

        playerGO.GetComponent<PlayerActor>().facing  = Resources.Load<Sprite>("Sprites/Characters/character_Trump");
        playerGO.GetComponent<PlayerActor>().forward  = Resources.Load<Sprite>("Sprites/Characters/character_TrumpB");

        return playerGO;
    }

    public GameObject CreatePlayerOprah()
    {
        GameObject playerGO = CreatePlayerBase();

        AbilityOprah abilityOprah = new AbilityOprah();
        abilityOprah.playerActor = playerGO.GetComponent<PlayerActor>();

        // Set all oprah specific stats
        playerGO.GetComponent<PlayerActor>().model.ability = abilityOprah;

        playerGO.GetComponent<PlayerActor>().facing  = Resources.Load<Sprite>("Sprites/Characters/character_Oprah");
        playerGO.GetComponent<PlayerActor>().forward  = Resources.Load<Sprite>("Sprites/Characters/character_OprahB");

        return playerGO;
    }

    public GameObject CreatePlayerBinLaden()
    {
        GameObject playerGO = CreatePlayerBase();

        AbilityBinLaden abilityBinLaden = new AbilityBinLaden();
        abilityBinLaden.playerActor = playerGO.GetComponent<PlayerActor>();

        // Set all oprah specific stats
        playerGO.GetComponent<PlayerActor>().model.ability = abilityBinLaden;

        playerGO.GetComponent<PlayerActor>().facing = Resources.Load<Sprite>("Sprites/Characters/character_1");
        playerGO.GetComponent<PlayerActor>().forward = Resources.Load<Sprite>("Sprites/Characters/character_Bin_LadenB");

        return playerGO;
    }

    public GameObject CreatePlayerPope()
    {
        GameObject playerGO = CreatePlayerBase();

        AbilityPope abilityPope = new AbilityPope();
        abilityPope.playerActor = playerGO.GetComponent<PlayerActor>();

        // Set all oprah specific stats
        playerGO.GetComponent<PlayerActor>().model.ability = abilityPope;

        playerGO.GetComponent<PlayerActor>().facing = Resources.Load<Sprite>("Sprites/Characters/character_The_PopeB");
        playerGO.GetComponent<PlayerActor>().forward = Resources.Load<Sprite>("Sprites/Characters/character_The_Pope");

        return playerGO;
    }


    public GameObject CreateDropShadow()
    {
        GameObject dropShadowGO = new GameObject("Drop Shadow");

        SpriteRenderer sr = dropShadowGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Random/DropShadow");
        sr.color = new Color(0, 0, 0, 0.5f);

        dropShadowGO.AddComponent<DropShadow>();
        return dropShadowGO;
    }

    public GameObject CreateTNTBag()
    {
        GameObject bagGO = new GameObject("TNTBag");

        SphereCollider bagCollider = bagGO.AddComponent<SphereCollider>();
        bagCollider.isTrigger = false;
        bagCollider.radius = 0.0f;

        Rigidbody bagRigid = bagGO.AddComponent<Rigidbody>();
        bagRigid.isKinematic = true;
        bagRigid.useGravity = false;

        GameObject bagVisualGO = new GameObject("Visual");
        bagVisualGO.transform.parent = bagGO.transform;

        GameObject bagVisualBodyGO = new GameObject("Body");
        bagVisualBodyGO.transform.parent = bagVisualGO.transform;
        SpriteRenderer sr = bagVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Random/TNTBag");

        Projectile p = bagGO.AddComponent<TNTBag>();
        p.Init();
        return bagGO;
    }

    public GameObject CreateBullet()
    {
        GameObject bulletGO = new GameObject("Bullet");

        SphereCollider bulletCollider = bulletGO.AddComponent<SphereCollider>();
        bulletCollider.isTrigger = true;
        bulletCollider.radius = 0.2f;

        Rigidbody bulletRigid = bulletGO.AddComponent<Rigidbody>();
        bulletRigid.isKinematic = true;
        bulletRigid.useGravity = false;

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

    public GameObject CreateRocket()
    {
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

        Projectile rock = rocketGO.AddComponent<RPG>();
        rock.Init();
        return rocketGO;
    }

    public GameObject CreateBeamNeon()
    {
        GameObject neonBeamGO = new GameObject("BeamNeon");

        GameObject neonBeamVisualGO = new GameObject("Visual");
        neonBeamVisualGO.transform.parent = neonBeamGO.transform;

        GameObject neonBeamVisualMainGO = new GameObject("MainBeam");
        neonBeamVisualMainGO.transform.parent = neonBeamVisualGO.transform;

        LineRenderer mainLR = neonBeamVisualMainGO.AddComponent<LineRenderer>();
        Material beamLine = new Material(Shader.Find("Particles/Additive"));
        beamLine.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Weapons/laserBeamMiddle"));
        mainLR.material = beamLine;

        Beam beam = neonBeamGO.AddComponent<BeamNeon>();
        beam.Init();
        return neonBeamGO;
    }

    public GameObject CreateBulletHit()
    {
        GameObject bulletHit = GameObject.Instantiate(Resources.Load<GameObject>("Effects/BulletHit"));
        return bulletHit;
    }

    public Material CreateHitFlash()
    {
        Material hitflash = new Material(Shader.Find("Particles/Additive"));
        hitflash.SetTexture("_MainTex", Resources.Load<Texture>("Materials/Flash"));

        return hitflash;
    }

    public GameObject CreateBeamHit()
    {
        GameObject beamHit = GameObject.Instantiate(Resources.Load<GameObject>("Effects/BulletHit"));
        return beamHit;
    }

    public GameObject CreateRocketHit()
    {
        GameObject rocketHit = GameObject.Instantiate(Resources.Load<GameObject>("Effects/Explosion"));
        ParticleSystem.ShapeModule shape = rocketHit.GetComponent<ParticleSystem>().shape;
        ParticleSystemRenderer test = rocketHit.GetComponent<ParticleSystemRenderer>();

        Material beamLine = new Material(Shader.Find("Particles/Additive"));
       // beamLine.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Weapons/laserBeamMiddle"));
        test.material = beamLine;


        shape.radius = 0.4f;
        return rocketHit;
    }
    public GameObject CreateRocketSmokeFlash()
    {
        GameObject smokeFlash = GameObject.Instantiate(Resources.Load<GameObject>("Effects/RocketSmokeFlash"));
        return smokeFlash;
    }

    public GameObject CreateRocketTrail()
    {
        GameObject smokeTrail = GameObject.Instantiate(Resources.Load<GameObject>("Effects/RocketTrail"));
        return smokeTrail;
    }

    public GameObject CreateBrokenWallEffect()
    {
        GameObject brokenWall = GameObject.Instantiate(Resources.Load<GameObject>("Effects/WallBreak"));
        return brokenWall;
    }

    public GameObject CreateMuzzleFlash()
    {
        GameObject muzzleFlash = GameObject.Instantiate(Resources.Load<GameObject>("Effects/MuzzleFlash"));
        return muzzleFlash;
    }

   public GameObject CreatePistol()
    {
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

    public GameObject CreateMachineGun()
    {
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

    public GameObject CreateLaserRifle()
    {
        GameObject laserRifleGO = new GameObject("Laser Rifle");

        SphereCollider laserRifleCollider = laserRifleGO.AddComponent<SphereCollider>();
        laserRifleCollider.radius = 0.1f;

        SphereCollider laserRifleColliderT = laserRifleGO.AddComponent<SphereCollider>();
        laserRifleColliderT.isTrigger = true;

        Rigidbody laserRifleRigid = laserRifleGO.AddComponent<Rigidbody>();
        laserRifleRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject laserRifleVisualGO = new GameObject("Visual");
        laserRifleVisualGO.transform.parent = laserRifleGO.transform;

        GameObject laserRifleVissualBodyGO = new GameObject("Body");
        laserRifleVissualBodyGO.transform.parent = laserRifleVisualGO.transform;
        SpriteRenderer sr = laserRifleVissualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/LaserRifle");

        laserRifleGO.AddComponent<LaserRifle>();

        return laserRifleGO;
    }
    public GameObject CreateRocketLauncher()
    {
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
    public GameObject CreateNormalChest()
    {
        GameObject chestGO = new GameObject("Normal Chest");

        SphereCollider chestCollider = chestGO.AddComponent<SphereCollider>();
        chestCollider.radius = 0.1f;

        SphereCollider chestColliderT = chestGO.AddComponent<SphereCollider>();
        chestColliderT.isTrigger = true;

        Rigidbody chestRigid = chestGO.AddComponent<Rigidbody>();
        chestRigid.constraints = RigidbodyConstraints.FreezeRotation;
     
        GameObject chestVisualGO = new GameObject("Visual");
        chestVisualGO.transform.parent = chestGO.transform;

        GameObject chestVisualBodyGO = new GameObject("Body");
        chestVisualBodyGO.transform.parent = chestVisualGO.transform;
        SpriteRenderer sr = chestVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Environment/NormalChest");

        chestGO.AddComponent<NormalChest>();

        return chestGO;
    }

    public GameObject CreateMedkit()
    {
        GameObject medkitGO = new GameObject("Medkit");

        SphereCollider medkitCollider = medkitGO.AddComponent<SphereCollider>();
        medkitCollider.radius = 0.1f;

        SphereCollider medkitColliderT = medkitGO.AddComponent<SphereCollider>();
        medkitColliderT.isTrigger = true;

        Rigidbody medkitRigid = medkitGO.AddComponent<Rigidbody>();
        medkitRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject medkitVisualGO = new GameObject("Visual");
        medkitVisualGO.transform.parent = medkitGO.transform;

        GameObject medkitVisualBodyGO = new GameObject("Body");
        medkitVisualBodyGO.transform.parent = medkitVisualGO.transform;
        SpriteRenderer sr = medkitVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Environment/heart");

        medkitGO.AddComponent<Medkit>();

        return medkitGO;
    }

    public GameObject CreateMedpack()
    {
        GameObject medpackGO = new GameObject("Medpack");

        SphereCollider medpackCollider = medpackGO.AddComponent<SphereCollider>();
        medpackCollider.radius = 0.1f;

        SphereCollider medpackColliderT = medpackGO.AddComponent<SphereCollider>();
        medpackColliderT.isTrigger = true;

        Rigidbody medpackRigid = medpackGO.AddComponent<Rigidbody>();
        medpackRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject medpackVisualGO = new GameObject("Visual");
        medpackVisualGO.transform.parent = medpackGO.transform;

        GameObject medpackVisualBodyGO = new GameObject("Body");
        medpackVisualBodyGO.transform.parent = medpackVisualGO.transform;
        SpriteRenderer sr = medpackVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Environment/heart1");

        medpackGO.AddComponent<Medpack>();

        return medpackGO;
    }

    public GameObject CreateFloor()
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";

        Material mf = new Material(Shader.Find("Mobile/Diffuse"));
        mf.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Environment/Mercury/Floor"));

        floor.GetComponent<Renderer>().material = mf;
        return floor;
    }

    Material m = null;
    public GameObject CreateWall()
    {
        GameObject wall = new GameObject("Wall");
        wall.AddComponent<BoxCollider>();
        wall.AddComponent<Wall>();
        wall.GetComponent<BoxCollider>().size = new Vector3(1, 50, 1);
        if (m == null)
        {
            m = new Material(Shader.Find("Mobile/Diffuse"));
            m.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Environment/Mercury/Voxel"));
        }

        Voxel wallVoxel = wall.AddComponent<Voxel>();
        wallVoxel.material = m;
        wallVoxel.Init();
        return wall;
    }

    public GameObject CreateEnemyWalker()
    {
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

        enemyWalkerGO.AddComponent<Walker>();
        return enemyWalkerGO;
    }

    public GameObject CreateMartianBoss()
    {
         GameObject martianBossGO = new GameObject("Martian Boss");

         CapsuleCollider martianBossCollider = martianBossGO.AddComponent<CapsuleCollider>();
         //Collider is double the size of a regular enemy
         martianBossCollider.radius = 0.5f;
         martianBossCollider.height = 1.6f;

         Rigidbody martianBossRigid = martianBossGO.AddComponent<Rigidbody>();
         martianBossRigid.interpolation = RigidbodyInterpolation.Interpolate;
         martianBossRigid.constraints = RigidbodyConstraints.FreezeRotation;

         GameObject martianBossrVisualGO = new GameObject("Visual");
         martianBossrVisualGO.transform.parent = martianBossGO.transform;

         GameObject martianBossVisualBodyGO = new GameObject("Body");
         martianBossVisualBodyGO.transform.parent = martianBossrVisualGO.transform;
         //Boss is double the size of a regular enemy 64x64
         martianBossVisualBodyGO.transform.localScale = new Vector3(2, 2);

         SpriteRenderer sr = martianBossVisualBodyGO.AddComponent<SpriteRenderer>();
         sr.sprite = Resources.Load<Sprite>("Sprites/Enemies/MartianBoss");

         martianBossGO.AddComponent<MartianBoss>();
         return martianBossGO;
    }

    public GameObject CreateRangedWalker()
    {
        GameObject enemyRangedGO = new GameObject("Ranged Enemy");

        CapsuleCollider enemyWalkerCollider = enemyRangedGO.AddComponent<CapsuleCollider>();
        enemyWalkerCollider.radius = 0.25f;
        enemyWalkerCollider.height = 0.8f;

        Rigidbody enemyWalkerRigid = enemyRangedGO.AddComponent<Rigidbody>();
        enemyWalkerRigid.interpolation = RigidbodyInterpolation.Interpolate;
        enemyWalkerRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject enemyWalkerVisualGO = new GameObject("Visual");
        enemyWalkerVisualGO.transform.parent = enemyRangedGO.transform;

        GameObject enemyWalkerVisualBodyGO = new GameObject("Body");
        enemyWalkerVisualBodyGO.transform.parent = enemyWalkerVisualGO.transform;

        SpriteRenderer sr = enemyWalkerVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Enemies/Ranged Walker");

        enemyRangedGO.AddComponent<RangedWalker>();
        return enemyRangedGO;
    }

    public GameObject CreatePortal()
    {
        GameObject portalGO = new GameObject("Portal");

        CapsuleCollider portalCollider = portalGO.AddComponent<CapsuleCollider>();
        portalCollider.radius = 0.25f;
        portalCollider.height = 0.8f;

        Rigidbody portalRigid = portalGO.AddComponent<Rigidbody>();
        portalRigid.interpolation = RigidbodyInterpolation.Interpolate;
        portalRigid.constraints = RigidbodyConstraints.FreezeRotation;

        //alternative method to clamp the portal to the ground:
        //portalRigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

        GameObject portalVisualGO = new GameObject("Visual");
        portalVisualGO.transform.parent = portalGO.transform;

        GameObject portalBodyVisualGO = new GameObject("Body");
        portalBodyVisualGO.transform.parent = portalVisualGO.transform;

        SpriteRenderer sr = portalBodyVisualGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Environment/Portal");

        portalGO.AddComponent<Portal>();
        return portalGO;
    }



    /* New stuff
     * ********************************************************
     * ********************************************************
     */

    public class ObjectConstructor
    {
        public virtual GameObject Construct()
        {
            return null;
        }
    }

    public class PlayerConstructorServer : ObjectConstructor
    {
        public override GameObject Construct()
        {
            base.Construct();
            GameObject playerGO = Factory.instance.CreatePlayerTrump();
            playerGO.AddComponent<NetworkIdentity>();
            playerGO.AddComponent<NetworkPlayerController>();
            return playerGO;
        }
    }
}