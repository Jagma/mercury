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
    #region Characters
    public GameObject CreatePlayerBase(string playerName)
    {
        GameObject playerGO = new GameObject("Player");
        playerGO.layer = LayerMask.NameToLayer("Player");

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

        Animator animator = playerVisualBodyGO.AddComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Controller_" + playerName);

        Animation anim = playerGO.AddComponent<Animation>();
        anim.playerActor = playerActor;

        PlayerController playerController = playerGO.AddComponent<PlayerController>();
        playerController.actor = playerActor;

        PlayerModel playerModel = new PlayerModel();
        playerModel.equippedWeapon = CreateSniperRifle().GetComponent<Weapon>();
        playerModel.equippedWeapon.Equip();
        playerActor.model = playerModel;
        GameObject hud = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerHUD"));
        hud.transform.SetParent(playerGO.transform, true);
        hud.transform.Find("HealthBar").GetComponent<HealthBar>().playerModel = playerModel;
        hud.transform.Find("AmmoBar1").GetComponent<AmmoBarMag>().playerModel = playerModel;
        hud.transform.Find("AmmoBar2").GetComponent<AmmoBarInventory>().playerModel = playerModel;
        hud.transform.localEulerAngles = new Vector3(45, 0, 0);
        hud.transform.position = new Vector3(0, 1.5f, 0);
        return playerGO;
    }

    public GameObject CreatePlayerTrump ()
    {
        GameObject playerGO = CreatePlayerBase("Trump");

        AbilityTrump abilityTrump = new AbilityTrump();
        abilityTrump.playerActor = playerGO.GetComponent<PlayerActor>();
        abilityTrump.Init();

        // Set all trump specific stats
        playerGO.GetComponent<PlayerActor>().model.ability = abilityTrump;
        return playerGO;
    }

    public GameObject CreatePlayerOprah()
    {
        GameObject playerGO = CreatePlayerBase("Oprah");

        AbilityOprah abilityOprah = new AbilityOprah();
        abilityOprah.playerActor = playerGO.GetComponent<PlayerActor>();

        // Set all oprah specific stats
        playerGO.GetComponent<PlayerActor>().model.ability = abilityOprah;
        return playerGO;
    }

    public GameObject CreatePlayerBinLaden()
    {
        GameObject playerGO = CreatePlayerBase("BinLaden");

        AbilityBinLaden abilityBinLaden = new AbilityBinLaden();
        abilityBinLaden.playerActor = playerGO.GetComponent<PlayerActor>();

        // Set all oprah specific stats
        playerGO.GetComponent<PlayerActor>().model.ability = abilityBinLaden;
        return playerGO;
    }

    public GameObject CreatePlayerPope()
    {
        GameObject playerGO = CreatePlayerBase("Pope");

        AbilityPope abilityPope = new AbilityPope();
        abilityPope.playerActor = playerGO.GetComponent<PlayerActor>();

        // Set all oprah specific stats
        playerGO.GetComponent<PlayerActor>().model.ability = abilityPope;
        return playerGO;
    }
#endregion

    #region Bullets
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

    public GameObject CreateSniperBullet()
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
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/Bullet_2");

        Projectile p = bulletGO.AddComponent<Round>();
        p.Init();
        return bulletGO;
    }

    Material lazerBulletMat;
    public GameObject CreateLaserBullet()
    {
        if (lazerBulletMat == null) {
            lazerBulletMat = new Material(Shader.Find("Unlit/Transparent"));
            lazerBulletMat.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Weapons/laserBeamPurpleO"));
        }
        GameObject LbulletGO = new GameObject("Laser Bullet");

        SphereCollider lbulletCollider = LbulletGO.AddComponent<SphereCollider>();
        lbulletCollider.isTrigger = true;
        lbulletCollider.radius = 0.2f;

        Rigidbody lbulletRigid = LbulletGO.AddComponent<Rigidbody>();
        lbulletRigid.isKinematic = true;
        lbulletRigid.useGravity = false;

        GameObject lbulletVisualGO = new GameObject("Visual");
        lbulletVisualGO.transform.parent = LbulletGO.transform;

        GameObject lbulletVisualBodyGO = new GameObject("Body");
        lbulletVisualBodyGO.transform.parent = lbulletVisualGO.transform;
        TrailRenderer tr = lbulletVisualBodyGO.AddComponent<TrailRenderer>();
        tr.time = 0.08f;
        tr.widthMultiplier = 0.2f;
        tr.material = lazerBulletMat;
        tr.numCapVertices = 2;
        tr.textureMode = LineTextureMode.Tile;

        Keyframe[] keys = new Keyframe[3];
        keys[0] = new Keyframe(0, 0.5f);
        keys[1] = new Keyframe(0.5f, 1);
        keys[2] = new Keyframe(1f, 0.5f);

        AnimationCurve curve = new AnimationCurve(keys);
        tr.widthCurve = curve;

        Projectile p = LbulletGO.AddComponent<Round>();
        p.Init();
        return LbulletGO;
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

    public GameObject CreateFlame()
    {
        //flame code.
        return null;
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
        beamLine.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Weapons/laserBeamNeon"));
        mainLR.material = beamLine;

        Beam beam = neonBeamGO.AddComponent<BeamNeon>(); //the beam makes use of the BeamNeon script which has its own functionalities.
        beam.Init();
        return neonBeamGO;
    }

    public GameObject CreateBeamPurple() //code is used in order to create/spawn the laser beam used for the laser pisol weapon.
    {
        GameObject purpleBeamGO = new GameObject("BeamNeon");

        GameObject purpleBeamVisualGO = new GameObject("Visual");
        purpleBeamVisualGO.transform.parent = purpleBeamGO.transform;

        GameObject neonBeamVisualMainGO = new GameObject("MainBeam");
        neonBeamVisualMainGO.transform.parent = purpleBeamVisualGO.transform;

        LineRenderer mainLR = neonBeamVisualMainGO.AddComponent<LineRenderer>();
        Material beamLine = new Material(Shader.Find("Particles/Additive")); //material is used in order to set the beam's material to a custom made one.
        beamLine.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Weapons/laserBeamPurple"));
        mainLR.material = beamLine;

        Beam beam = purpleBeamGO.AddComponent<BeamPurple>(); //the beam makes use of the BeamNeon script which has its own functionalities.
        beam.Init();
        return purpleBeamGO;
    }
#endregion

    #region Effects & Abilities
    public GameObject CreateBulletHit() 
    {
        GameObject bulletHit = GameObject.Instantiate(Resources.Load<GameObject>("Effects/BulletHit"));
        return bulletHit;
    }

    public Material CreateHitFlash() //adds a hit flash effect ("white flash") on the enemies if the player shoots them.
    {
        Material hitflash = new Material(Shader.Find("HitFlash"));
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
        GameObject rocketHit = GameObject.Instantiate(Resources.Load<GameObject>("Effects/RocketExplosion"));
        ParticleSystem.ShapeModule shape = rocketHit.GetComponent<ParticleSystem>().shape;
        shape.radius = 0.8f;
        return rocketHit;
    }

    public GameObject CreateFlameEffect() //adds an flame effect onto the flamethower
    {
        GameObject flame = GameObject.Instantiate(Resources.Load<GameObject>("Effects/FlameEffect"));
        return flame;
    }

    public GameObject CreateBagExplosion() //adds an explosion effect onto the rocket when it hits/collides with an object.
    {
        GameObject exlosion = GameObject.Instantiate(Resources.Load<GameObject>("Effects/BagExplosion"));
        return exlosion;
    }

    public GameObject CreateRocketSmokeFlash()  //adds an smoke flash effect that travels with the rocket itself.
    {
        GameObject smokeFlash = GameObject.Instantiate(Resources.Load<GameObject>("Effects/RocketSmokeFlash"));
        return smokeFlash;
    }

    public GameObject CreateMuzzleFlash() //adds an muzzle flash effect for the weapon.
    {
        GameObject muzzleFlash = GameObject.Instantiate(Resources.Load<GameObject>("Effects/MuzzleFlash"));
        return muzzleFlash;
    }

    public GameObject CreateLaserMuzzleFlash() //adds an muzzle flash effect for the weapon.
    {
        GameObject LmuzzleFlash = GameObject.Instantiate(Resources.Load<GameObject>("Effects/LaserMuzzleFlash"));
        return LmuzzleFlash;
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

        Rigidbody bagRigid = bagGO.AddComponent<Rigidbody>();
        bagRigid.useGravity = true;
        bagRigid.freezeRotation = true;
        bagRigid.transform.position = bagGO.transform.position;

        SphereCollider bagCollider = bagGO.AddComponent<SphereCollider>();
        bagCollider.isTrigger = false;
        bagCollider.radius = 0.0f;
        bagCollider.transform.parent = bagRigid.transform;

        SpriteRenderer sr = bagGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Random/TNTBag");
        sr.transform.parent = bagRigid.transform;
        sr.transform.localEulerAngles = new Vector3(45, 0, 0);

        TNTBag bag = bagGO.AddComponent<TNTBag>();
        return bagGO;
    }

    public GameObject CreatePopeEffect()
    {
        GameObject popeEffectGO = GameObject.Instantiate(Resources.Load<GameObject>("Effects/PopeHeal"));

        return popeEffectGO;
    }

    #endregion

    #region Weapons
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

    public GameObject CreateRevolver()
    {
        GameObject revolverGO = new GameObject("Revolver");

        SphereCollider revolverCollider = revolverGO.AddComponent<SphereCollider>();
        revolverCollider.radius = 0.1f;

        SphereCollider revolverColliderT = revolverGO.AddComponent<SphereCollider>();
        revolverColliderT.isTrigger = true;

        Rigidbody revolverRigid = revolverGO.AddComponent<Rigidbody>();
        revolverRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject revolverVisualGO = new GameObject("Visual");
        revolverVisualGO.transform.parent = revolverGO.transform;

        GameObject revolverVisualBodyGO = new GameObject("Body");
        revolverVisualBodyGO.transform.parent = revolverVisualGO.transform;
        SpriteRenderer sr = revolverVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/Revolver");

        revolverGO.AddComponent<Revolver>();

        return revolverGO;
    }

    public GameObject CreateBurstAssaultRifle()
    {
        GameObject burstARGO = new GameObject("Burst Assault Rifle");

        SphereCollider burstARCollider = burstARGO.AddComponent<SphereCollider>();
        burstARCollider.radius = 0.1f;

        SphereCollider burstARColliderT = burstARGO.AddComponent<SphereCollider>();
        burstARColliderT.isTrigger = true;

        Rigidbody burstARRigid = burstARGO.AddComponent<Rigidbody>();
        burstARRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject burstARVisualGO = new GameObject("Visual");
        burstARVisualGO.transform.parent = burstARGO.transform;

        GameObject burstARVisualBodyGO = new GameObject("Body");
        burstARVisualBodyGO.transform.parent = burstARVisualGO.transform;
        SpriteRenderer sr = burstARVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/BurstAssaultRifle");

        burstARGO.AddComponent<BurstAssaultRifle>();

        return burstARGO;
    }
    public GameObject CreateSniperRifle()
    {
        GameObject sniperGO = new GameObject("Sniper Rifle");

        SphereCollider sniperCollider = sniperGO.AddComponent<SphereCollider>();
        sniperCollider.radius = 0.1f;

        SphereCollider sniperColliderT = sniperGO.AddComponent<SphereCollider>();
        sniperColliderT.isTrigger = true;

        Rigidbody sniperRigid = sniperGO.AddComponent<Rigidbody>();
        sniperRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject sniperVisualGO = new GameObject("Visual");
        sniperVisualGO.transform.parent = sniperGO.transform;

        GameObject sniperVisualBodyGO = new GameObject("Body");
        sniperVisualBodyGO.transform.parent = sniperVisualGO.transform;
        SpriteRenderer sr = sniperVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/SniperRifle");

        sniperGO.AddComponent<SniperRifle>();

        return sniperGO;
    }

    public GameObject CreateSword()
    {
        GameObject swordGO = new GameObject("Sword");

        SphereCollider swordCollider = swordGO.AddComponent<SphereCollider>();
        swordCollider.radius = 0.1f;

        SphereCollider swordColliderT = swordGO.AddComponent<SphereCollider>();
        swordColliderT.isTrigger = true;

        Rigidbody swordRigid = swordGO.AddComponent<Rigidbody>();
        swordRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject swordVisualGO = new GameObject("Visual");
        swordVisualGO.transform.parent = swordGO.transform;

        GameObject swordVisualBodyGO = new GameObject("Body");
        swordVisualBodyGO.transform.parent = swordVisualGO.transform;
        SpriteRenderer sr = swordVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/Sword");

        swordGO.AddComponent<Sword>();

        return swordGO;
    }

    public GameObject CreateStrongAxe()
    {
        GameObject saxeGO = new GameObject("Strong Axe");

        SphereCollider saxeCollider = saxeGO.AddComponent<SphereCollider>();
        saxeCollider.radius = 0.1f;

        SphereCollider saxeColliderT = saxeGO.AddComponent<SphereCollider>();
        saxeColliderT.isTrigger = true;

        Rigidbody saxeRigid = saxeGO.AddComponent<Rigidbody>();
        saxeRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject saxeVisualGO = new GameObject("Visual");
        saxeVisualGO.transform.parent = saxeGO.transform;

        GameObject axeVisualBodyGO = new GameObject("Body");
        axeVisualBodyGO.transform.parent = saxeVisualGO.transform;
        SpriteRenderer sr = axeVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/StrongAxe");

        saxeGO.AddComponent<StrongAxe>();

        return saxeGO;
    }

    public GameObject CreateWeakAxe()
    {
        GameObject waxeGO = new GameObject("Weak Axe");

        SphereCollider waxeCollider = waxeGO.AddComponent<SphereCollider>();
        waxeCollider.radius = 0.1f;

        SphereCollider waxeColliderT = waxeGO.AddComponent<SphereCollider>();
        waxeColliderT.isTrigger = true;

        Rigidbody waxeRigid = waxeGO.AddComponent<Rigidbody>();
        waxeRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject waxeVisualGO = new GameObject("Visual");
        waxeVisualGO.transform.parent = waxeGO.transform;

        GameObject waxeVisualBodyGO = new GameObject("Body");
        waxeVisualBodyGO.transform.parent = waxeVisualGO.transform;
        SpriteRenderer sr = waxeVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/WeakAxe");

        waxeGO.AddComponent<WeakAxe>();

        return waxeGO;
    }

    public GameObject CreateShotgun()
    {
        GameObject shotgunGO = new GameObject("Shotgun");

        SphereCollider shotgunCollider = shotgunGO.AddComponent<SphereCollider>();
        shotgunCollider.radius = 0.1f;

        SphereCollider shotgunColliderT = shotgunGO.AddComponent<SphereCollider>();
        shotgunColliderT.isTrigger = true;

        Rigidbody shotgunRigid = shotgunGO.AddComponent<Rigidbody>();
        shotgunRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject shotgunVisualGO = new GameObject("Visual");
        shotgunVisualGO.transform.parent = shotgunGO.transform;

        GameObject shotgunVisualBodyGO = new GameObject("Body");
        shotgunVisualBodyGO.transform.parent = shotgunVisualGO.transform;
        SpriteRenderer sr = shotgunVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/Shotgun");

        shotgunGO.AddComponent<Shotgun>();

        return shotgunGO;
    }

    public GameObject CreateFlamethrower()  //code use to create the flamethower weapon for the players to use.
    {
        GameObject flamethrowerGO = new GameObject("Flamethrower");

        SphereCollider flameThrowerCollider = flamethrowerGO.AddComponent<SphereCollider>();
        flameThrowerCollider.radius = 0.1f;

        SphereCollider flameThrowerColliderT = flamethrowerGO.AddComponent<SphereCollider>();
        flameThrowerColliderT.isTrigger = true;

        Rigidbody flameThrowerRigid = flamethrowerGO.AddComponent<Rigidbody>();
        flameThrowerRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject flamethrowerVisualGO = new GameObject("Visual");
        flamethrowerVisualGO.transform.parent = flamethrowerGO.transform;

        GameObject flamethrowerVisualBodyGO = new GameObject("Body");
        flamethrowerVisualBodyGO.transform.parent = flamethrowerVisualGO.transform;
        SpriteRenderer sr = flamethrowerVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/Flamethrower");

        flamethrowerGO.AddComponent<Flamethrower>();

        return flamethrowerGO;
    }

    public GameObject CreateLaserMachineGun() //code use to create the laser machine gun weapon for the players to use.
    {
        GameObject LmachineGunGO = new GameObject("Laser Machine Gun");
        SphereCollider LmachineGunCollider = LmachineGunGO.AddComponent<SphereCollider>();
        LmachineGunCollider.radius = 0.1f;

        SphereCollider LmachineGunColliderT = LmachineGunGO.AddComponent<SphereCollider>();
        LmachineGunColliderT.isTrigger = true;

        Rigidbody LmachineGunRigid = LmachineGunGO.AddComponent<Rigidbody>();
        LmachineGunRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject LmachineGunVisualGO = new GameObject("Visual");
        LmachineGunVisualGO.transform.parent = LmachineGunGO.transform;

        GameObject LmachineGunVisualBodyGO = new GameObject("Body");
        LmachineGunVisualBodyGO.transform.parent = LmachineGunVisualGO.transform;
        SpriteRenderer sr = LmachineGunVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/LaserMachineGun");

        LmachineGunGO.AddComponent<LaserMachineGun>();

        return LmachineGunGO;
    }
    public GameObject CreateLaserPistol()  //code use to create the laser pistol weapon for the players to use.
    {
        GameObject laserPistolGO = new GameObject("Laser Pistol");

        SphereCollider laserPistolCollider = laserPistolGO.AddComponent<SphereCollider>();
        laserPistolCollider.radius = 0.1f;

        SphereCollider laserPistolColliderT = laserPistolGO.AddComponent<SphereCollider>();
        laserPistolColliderT.isTrigger = true;

        Rigidbody laserPistolRigid = laserPistolGO.AddComponent<Rigidbody>();
        laserPistolRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject laserPistolVisualGO = new GameObject("Visual");
        laserPistolVisualGO.transform.parent = laserPistolGO.transform;

        GameObject laserPistolVisualBodyGO = new GameObject("Body");
        laserPistolVisualBodyGO.transform.parent = laserPistolVisualGO.transform;
        SpriteRenderer sr = laserPistolVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/LaserPistol");

        laserPistolGO.AddComponent<LaserPistol>();

        return laserPistolGO;
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

    public GameObject CreateLaserRayGun()  //code use to create the laser rifle weapon for the players to use.
    {
        GameObject laserRifleGO = new GameObject("Laser RayGun");

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
        sr.sprite = Resources.Load<Sprite>("Sprites/Weapons/LaserRayGun");

        laserRifleGO.AddComponent<LaserRayGun>();

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
#endregion 

    #region Blood
    public GameObject CreateBlood()
    {
        GameObject bloodGO = new GameObject("Blood");

        Blood behavior = bloodGO.AddComponent<Blood>();

        return bloodGO;
    }

    /// <summary>
    /// Creates chunks and places them around a point
    /// </summary>
    /// <param name="center"> Center around which chunks is placed </param>
    /// <param name="radius"> The radius of the cirle </param>
    /// <returns> List of GameObjects</returns>
        public List<GameObject> CreateChunks(Vector3 center, float radius)
        {
        int chunkCount = Random.Range(5, 9);
        List<GameObject> chunkList = new List<GameObject>();
        //Creates a game object 
        for (int i = 0; i < chunkCount; i++)
        { 
            GameObject chunk = new GameObject();
            chunk.transform.position = center;

            Rigidbody rigid = chunk.AddComponent<Rigidbody>();
            rigid.useGravity = true;

            //Position around circle
            float angle = Random.Range(0,361);
            Vector3 pos = new Vector3(
                x: center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad),
                y: center.y,
                z: center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad)
            );
            rigid.transform.position = pos;
            rigid.mass = Random.Range(2f, 5f);

            SpriteRenderer visual = chunk.AddComponent<SpriteRenderer>();
            visual.sprite = Resources.Load<Sprite>("Effects/Blood/Splat_" + Random.Range(1, 5));
            visual.transform.parent = rigid.transform;
            visual.transform.rotation = Quaternion.Euler(45, 45, 0);
            visual.enabled = false;

            GameObject trail = GameObject.Instantiate(Resources.Load<GameObject>("Effects/Blood/BloodTrail"));
            trail.transform.parent = rigid.transform;
            trail.transform.localPosition = Vector3.zero;
            GameObject.Destroy(trail, 5f);

            Chunk behavior = chunk.AddComponent<Chunk>();

            chunkList.Add(chunk);
        };
        return chunkList;
    }
    #endregion

    #region Chests
    public GameObject CreateNormalChest()  //code use to create the normal chest.
    {
        GameObject chestGO = new GameObject("Normal Chest");

        SphereCollider chestCollider = chestGO.AddComponent<SphereCollider>();
        chestCollider.radius = 0.3f;

        SphereCollider chestColliderT = chestGO.AddComponent<SphereCollider>();
        chestColliderT.isTrigger = true;

        Rigidbody chestRigid = chestGO.AddComponent<Rigidbody>();
        chestRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject chestVisualGO = new GameObject("Visual");
        chestVisualGO.transform.parent = chestGO.transform;

        GameObject chestVisualBodyGO = new GameObject("Body");
        chestVisualBodyGO.transform.parent = chestVisualGO.transform;
        SpriteRenderer sr = chestVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Environment/Chest1Closed");

        Chest chest = chestGO.AddComponent<NormalChest>(); //makes usse of the NormalChest script which has its own functionalities.
        chest.openSprite = Resources.Load<Sprite>("Sprites/Environment/Chest1Open");
        chest.closedSprite = Resources.Load<Sprite>("Sprites/Environment/Chest1Closed");

        return chestGO;
    }

    public GameObject CreateRareChest() //code use to create the rare chest.
    {
        GameObject chestGO = new GameObject("Rare Chest");

        SphereCollider chestCollider = chestGO.AddComponent<SphereCollider>();
        chestCollider.radius = 0.3f;

        SphereCollider chestColliderT = chestGO.AddComponent<SphereCollider>();
        chestColliderT.isTrigger = true;

        Rigidbody chestRigid = chestGO.AddComponent<Rigidbody>();
        chestRigid.constraints = RigidbodyConstraints.FreezeRotation;

        GameObject chestVisualGO = new GameObject("Visual");
        chestVisualGO.transform.parent = chestGO.transform;

        GameObject chestVisualBodyGO = new GameObject("Body");
        chestVisualBodyGO.transform.parent = chestVisualGO.transform;
        SpriteRenderer sr = chestVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Environment/Chest2Closed");

        Chest chest = chestGO.AddComponent<RareChest>(); //makes usse of the NormalChest script which has its own functionalities.
        chest.openSprite = Resources.Load<Sprite>("Sprites/Environment/Chest2Open");
        chest.closedSprite = Resources.Load<Sprite>("Sprites/Environment/Chest2Closed");

        return chestGO;
    }
    #endregion

    #region Health Pickups
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
        medkitVisualBodyGO.transform.localPosition = new Vector3(0, 0.05f, -0.05f);
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
        medpackVisualBodyGO.transform.localPosition = new Vector3(0, 0.05f, -0.05f);
        SpriteRenderer sr = medpackVisualBodyGO.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/Environment/heart1");

        medpackGO.AddComponent<Medpack>(); //makes use of the Medpack script which has its own functionalities.

        return medpackGO;
    }
#endregion

    #region Ammo Pickups
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
    #endregion

    #region Walls & Floor
    public GameObject CreateWallBreak(string environmentName) {
        GameObject brokenWall = GameObject.Instantiate(Resources.Load<GameObject>("Effects/" + environmentName + "/WallBreak"));
        return brokenWall;
    }

    public GameObject CreateWall(string environmentName, int textureID) {
        GameObject wall = new GameObject("Wall");
        wall.layer = LayerMask.NameToLayer("Environment");

        wall.AddComponent<BoxCollider>();
        wall.AddComponent<Wall>();
        wall.GetComponent<BoxCollider>().size = new Vector3(1, 50, 1);

        Material mat = new Material(Shader.Find("Mobile/Diffuse"));
        mat.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Environment/" + environmentName + "/Voxel" + textureID.ToString()));

        Voxel wallVoxel = wall.AddComponent<Voxel>();
        wallVoxel.material = mat;
        wallVoxel.Init();

        return wall;
    }

    public GameObject CreateFloor(string environmentName, int textureID) {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";

        Material mat = new Material(Shader.Find("Mobile/Diffuse"));
        mat.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/Environment/" + environmentName + "/Floor" + textureID.ToString()));

        floor.GetComponent<Renderer>().material = mat;
        return floor;
    }
    #endregion

    #region Enemies & Bosses
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

    public GameObject CreateMartianBoss() //Creates the martian boss
    {
         GameObject martianBossGO = new GameObject("Martian Boss");

         CapsuleCollider martianBossCollider = martianBossGO.AddComponent<CapsuleCollider>();
         //Collider is double the size of a regular enemy
         martianBossCollider.radius = 0.6f;
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

    public GameObject CreateCorruptedWalker() //code use to create the ranged walker enemy within the game.
    {
        GameObject enemyRangedGO = new GameObject("Corrupted Enemy");

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
        sr.sprite = Resources.Load<Sprite>("Sprites/Enemies/Corrupted Walker");

        enemyRangedGO.AddComponent<CorruptedWalker>();  //makes use of the RangedWalker script which has its own functionalities.
        return enemyRangedGO;
    }


    public GameObject CreateArcWalker() //code use to create the ranged walker enemy within the game.
    {
        GameObject enemyRangedGO = new GameObject("Arc Enemy");

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
        sr.sprite = Resources.Load<Sprite>("Sprites/Enemies/Arc Walker");

        enemyRangedGO.AddComponent<ArcWalker>();  //makes use of the RangedWalker script which has its own functionalities.
        return enemyRangedGO;
    }
    public GameObject CreateOverlordWalker() //code use to create the overlord walker enemy within the game.
    {
        GameObject enemyRangedGO = new GameObject("Overlord Walker");

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
        sr.sprite = Resources.Load<Sprite>("Sprites/Enemies/Overlord Walker");

        enemyRangedGO.AddComponent<OverlordWalker>();  //makes use of the OverlordRangedWalker script which has its own functionalities.
        return enemyRangedGO;
    }
    #endregion

    // Multiplayer
    public class ObjectConstructor {
        public string objectUniqueID = "-1";
        public virtual GameObject Construct() {
            return new GameObject();
        }
    }

    public class BasePlayerConstructor : ObjectConstructor {
        public override GameObject Construct() {
            GameObject playerGO = base.Construct();
            playerGO.layer = LayerMask.NameToLayer("Player");

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

            PlayerActor playerActor = playerGO.AddComponent<PlayerActor>();

            PlayerModel playerModel = new PlayerModel();
            playerActor.model = playerModel;
            playerActor.model.playerActive = true;

            GameObject hud = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerHUD"));
            hud.transform.SetParent(playerGO.transform, true);
            hud.transform.Find("HealthBar").GetComponent<HealthBar>().playerModel = playerModel;
            hud.transform.Find("AmmoBar1").GetComponent<AmmoBarMag>().playerModel = playerModel;
            hud.transform.Find("AmmoBar2").GetComponent<AmmoBarInventory>().playerModel = playerModel;
            hud.transform.localEulerAngles = new Vector3(45, 0, 0);
            hud.transform.position = new Vector3(0, 1.5f, 0);
            return playerGO;
        }
    }

    public class HostPlayerConstructor : BasePlayerConstructor {
        public override GameObject Construct() {
            GameObject playerGO = base.Construct();
            playerGO.name = "PlayerHost";

            playerGO.AddComponent<ServerPlayer>();
            playerGO.GetComponent<ServerPlayer>().clientUniqueID = objectUniqueID;
            return playerGO;
        }
    }

    public class ClientPlayerConstructor : BasePlayerConstructor {
        public override GameObject Construct() {
            GameObject playerGO =  base.Construct();
            playerGO.name = "PlayerClient";

            playerGO.AddComponent<ClientPlayer>();
            playerGO.GetComponent<ClientPlayer>().clientUniqueID = objectUniqueID;

            return playerGO;
        }
    }
}