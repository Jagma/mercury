using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * SUMMARY:
 * The factory class was created in order to help reduce any merging problems that may occur when multiple members of our team works on the same classes and scenes in this project.
 * The factory class is a singleton class, it contains most if not all of the code to create our objects manually through code, as we encountered previous problems within
 * GitHub that will merge different member's work together and causes it to break.
 * 
 * In order to prevent comment "redundancy", the following in-built unity components that are used various time in our code will be explained below:
 * > SpriteRenderer: Use to...
 * > RigidBody: Use to...
 * > LineRenderer: Use to...
 * > Capsule Collider: Use to...
 * > PhysicMaterial: Use to...
 * > Material: Use to...
 * > ParticleSystem: Use to...
 * > SphereCollider: Use to...
 */

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

        GameObject smokeTrail = GameObject.Instantiate(Resources.Load<GameObject>("Effects/RocketTrail"));
        smokeTrail.name = "SmokeTrail";
        smokeTrail.transform.parent = bulletVisualGO.transform;
        smokeTrail.transform.localPosition = Vector3.zero;

        Projectile rock = rocketGO.AddComponent<RPG>();
        rock.Init();
        return rocketGO;
    }

    public GameObject CreateBeamNeon() //code is used in order to create/spawn the laser beam used for the laser rifle weapon.
    {
        GameObject neonBeamGO = new GameObject("BeamNeon");

        GameObject neonBeamVisualGO = new GameObject("Visual");
        neonBeamVisualGO.transform.parent = neonBeamGO.transform;

        GameObject neonBeamVisualMainGO = new GameObject("MainBeam");
        neonBeamVisualMainGO.transform.parent = neonBeamVisualGO.transform;

        LineRenderer mainLR = neonBeamVisualMainGO.AddComponent<LineRenderer>();
        Material beamLine = new Material(Shader.Find("Particles/Additive")); //material is used in order to set the beam's material to a custom made one.
        beamLine.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Weapons/laserBeamMiddle"));
        mainLR.material = beamLine;

        Beam beam = neonBeamGO.AddComponent<BeamNeon>(); //the beam makes use of the BeamNeon script which has its own functionalities.
        beam.Init();
        return neonBeamGO;
    }

    public GameObject CreateBulletHit() 
    {
        GameObject bulletHit = GameObject.Instantiate(Resources.Load<GameObject>("Effects/BulletHit"));
        return bulletHit;
    }

    public Material CreateHitFlash() //adds a hit flash effect ("white flash") on the enemies if the player shoots them.
    {
        Material hitflash = new Material(Shader.Find("Particles/Additive"));
        hitflash.SetTexture("_MainTex", Resources.Load<Texture>("Materials/Additive"));

        return hitflash;
    }

    public GameObject CreateBeamHit()
    {
        GameObject beamHit = GameObject.Instantiate(Resources.Load<GameObject>("Effects/BulletHit"));
        return beamHit;
    }

    public GameObject CreateRocketHit() //adds an explosion effect onto the rocket when it hits/collides with an object.
    {
        GameObject rocketHit = GameObject.Instantiate(Resources.Load<GameObject>("Effects/Explosion"));
        ParticleSystem.ShapeModule shape = rocketHit.GetComponent<ParticleSystem>().shape;
        ParticleSystemRenderer test = rocketHit.GetComponent<ParticleSystemRenderer>();

        Material beamLine = new Material(Shader.Find("Particles/Additive"));
        test.material = beamLine;


        shape.radius = 0.4f;
        return rocketHit;
    }
    public GameObject CreateRocketSmokeFlash()  //adds an smoke flash effect that travels with the rocket itself.
    {
        GameObject smokeFlash = GameObject.Instantiate(Resources.Load<GameObject>("Effects/RocketSmokeFlash"));
        return smokeFlash;
    }

    public GameObject CreateBrokenWallEffect()
    {
        GameObject brokenWall = GameObject.Instantiate(Resources.Load<GameObject>("Effects/WallBreak"));
        return brokenWall;
    }

    public GameObject CreateMuzzleFlash() //adds an muzzle flash effect for the weapon.
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

    public GameObject CreateMachineGun() //code use to create the machine gun weapon for the players to use.
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

    public GameObject CreateLaserRifle()  //code use to create the laser rifle weapon for the players to use.
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
    public GameObject CreateRocketLauncher() //code use to create the rocket launcher weapon for the players to use.
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
    public GameObject CreateNormalChest()  //code use to create the normal chest.
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

        chestGO.AddComponent<NormalChest>(); //makes usse of the NormalChest script which has its own functionalities.

        return chestGO;
    }

    public GameObject CreateAmmoChest() //code use to create the ammo chest.
    {
        GameObject chestGO = new GameObject("Ammo Chest");

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
        sr.sprite = Resources.Load<Sprite>("Sprites/Environment/AmmoChest");

        chestGO.AddComponent<AmmoChest>();  //makes usse of the AmmoChest script which has its own functionalities.

        return chestGO;
    }

    public GameObject CreateMedkit() //code use to create the medkit for players to pickup.
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

        medkitGO.AddComponent<Medkit>(); //makes use of the Medkit script which has its own functionalities.

        return medkitGO;
    }

    public GameObject CreateMedpack() //code use to create the medpack for players to pickup.
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

        medpackGO.AddComponent<Medpack>(); //makes use of the Medpack script which has its own functionalities.

        return medpackGO;
    }

    public GameObject CreateRocketAmmoPack() //code use to create the rocket ammo pack for players to pickup.
    {
        GameObject rocketAmmoGO = new GameObject("RocketAmmo");

        SphereCollider rocketAmmoCollider = rocketAmmoGO.AddComponent<SphereCollider>();
        rocketAmmoCollider.radius = 0.1f;

        SphereCollider rocketAmmoColliderT = rocketAmmoGO.AddComponent<SphereCollider>();
        rocketAmmoColliderT.isTrigger = true;

        Rigidbody rocketAmmoRigid = rocketAmmoGO.AddComponent<Rigidbody>();
        rocketAmmoRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject rocketAmmoVisualGO = new GameObject("Visual");
        rocketAmmoVisualGO.transform.parent = rocketAmmoGO.transform;

        GameObject rocketAmmoVisualBodyGO = new GameObject("Body");
        rocketAmmoVisualBodyGO.transform.parent = rocketAmmoVisualGO.transform;
        SpriteRenderer sr = rocketAmmoVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Environment/rocketammo");

        rocketAmmoGO.AddComponent<RockettAmmoPack>(); //makes use of the RockettAmmoPack script which has its own functionalities.

        return rocketAmmoGO;
    }


    public GameObject CreateBulletAmmoPack() //code use to create the bullet ammo pack for players to pickup.
    {
        GameObject bulletAmmoGO = new GameObject("BulletAmmo");

        SphereCollider bulletAmmoCollider = bulletAmmoGO.AddComponent<SphereCollider>();
        bulletAmmoCollider.radius = 0.1f;

        SphereCollider bulletAmmoColliderT = bulletAmmoGO.AddComponent<SphereCollider>();
        bulletAmmoColliderT.isTrigger = true;

        Rigidbody bulletAmmoRigid = bulletAmmoGO.AddComponent<Rigidbody>();
        bulletAmmoRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject bulletAmmoVisualGO = new GameObject("Visual");
        bulletAmmoVisualGO.transform.parent = bulletAmmoGO.transform;

        GameObject bulletAmmoVisualBodyGO = new GameObject("Body");
        bulletAmmoVisualBodyGO.transform.parent = bulletAmmoVisualGO.transform;
        SpriteRenderer sr = bulletAmmoVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Environment/bulletammo");

        bulletAmmoGO.AddComponent<BulletAmmoPack>();  //makes use of the BulletAmmoPack script which has its own functionalities.

        return bulletAmmoGO;
    }


    public GameObject CreateBeamAmmoPack() //code use to create the beam ammo pack for players to pickup.
    { 
        GameObject BeamAmmoGO = new GameObject("BeamAmmo");

        SphereCollider BeamAmmoCollider = BeamAmmoGO.AddComponent<SphereCollider>();
        BeamAmmoCollider.radius = 0.1f;

        SphereCollider BeamAmmoColliderT = BeamAmmoGO.AddComponent<SphereCollider>();
        BeamAmmoColliderT.isTrigger = true;

        Rigidbody BeamAmmoRigid = BeamAmmoGO.AddComponent<Rigidbody>();
        BeamAmmoRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject BeamAmmoVisualGO = new GameObject("Visual");
        BeamAmmoVisualGO.transform.parent = BeamAmmoGO.transform;

        GameObject BeamAmmoVisualBodyGO = new GameObject("Body");
        BeamAmmoVisualBodyGO.transform.parent = BeamAmmoVisualGO.transform;
        SpriteRenderer sr = BeamAmmoVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Environment/beamammo"); 

        BeamAmmoGO.AddComponent<BeamAmmoPack>();  //makes use of the BeamAmmoPack script which has its own functionalities.

        return BeamAmmoGO;
    }

    public GameObject CreateTrumpWall() {
        GameObject wall = new GameObject("Wall");
        wall.AddComponent<BoxCollider>();
        wall.AddComponent<Wall>();
        wall.GetComponent<Wall>().health = int.MaxValue;
        wall.GetComponent<BoxCollider>().size = new Vector3(1, 50, 1);

        Material trumpWallmat = new Material(Shader.Find("Mobile/Diffuse"));
        trumpWallmat.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Environment/Mercury/Voxel"));

        Voxel wallVoxel = wall.AddComponent<Voxel>();
        wallVoxel.material = trumpWallmat;
        wallVoxel.Init();        
        return wall;
    }

    // Mars environment
    public GameObject CreateMarsFloor()
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";

        Material mf = new Material(Shader.Find("Mobile/Diffuse"));
        mf.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Environment/Mercury/Floor"));

        floor.GetComponent<Renderer>().material = mf;
        return floor;
    }

    Material marsWallMat = null;
    public GameObject CreateMarsWall()
    {
        GameObject wall = new GameObject("Wall");
        wall.AddComponent<BoxCollider>();
        wall.AddComponent<Wall>();
        wall.GetComponent<BoxCollider>().size = new Vector3(1, 50, 1);
        if (marsWallMat == null)
        {
            marsWallMat = new Material(Shader.Find("Mobile/Diffuse"));
            marsWallMat.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Environment/Mercury/Voxel"));
        }

        Voxel wallVoxel = wall.AddComponent<Voxel>();
        wallVoxel.material = marsWallMat;
        wallVoxel.Init();
        return wall;
    }

    // Spaceship environment
    public GameObject CreateSpaceshipFloor() {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";

        Material mf = new Material(Shader.Find("Mobile/Diffuse"));
        mf.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Environment/Spaceship/Floor"));

        floor.GetComponent<Renderer>().material = mf;
        return floor;
    }

    Material spaceshipWallmat = null;
    public GameObject CreateSpaceshipeWall() {
        GameObject wall = new GameObject("Wall");
        wall.AddComponent<BoxCollider>();
        wall.AddComponent<Wall>();
        wall.GetComponent<BoxCollider>().size = new Vector3(1, 50, 1);
        if (spaceshipWallmat == null) {
            spaceshipWallmat = new Material(Shader.Find("Mobile/Diffuse"));
            spaceshipWallmat.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Environment/Spaceship/Voxel"));
        }

        Voxel wallVoxel = wall.AddComponent<Voxel>();
        wallVoxel.material = spaceshipWallmat;
        wallVoxel.Init();
        return wall;
    }

    public GameObject CreateEnemyWalker()  //code use to create the walker enemy within the game.
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

        enemyWalkerGO.AddComponent<Walker>();  //makes use of the Walker script which has its own functionalities.
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

    public GameObject CreateRangedWalker() //code use to create the ranged walker enemy within the game.
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

        enemyRangedGO.AddComponent<RangedWalker>();  //makes use of the RangedWalker script which has its own functionalities.
        return enemyRangedGO;
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