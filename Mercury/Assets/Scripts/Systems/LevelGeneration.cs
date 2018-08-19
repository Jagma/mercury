using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{

    public static LevelGeneration instance;
    private void Awake()
    {
        instance = this;        
    }

    public Vector3 playerSpawnPosition = new Vector3(3, 3, 3);

    public int mapWidth = 64;
    public int mapDepth = 64;

    public string[,] terrain;
    public string[,] enemies;
    public string[,] pickups;

    Transform levelRoot;

    public void Generate ()
    {
        terrain = new string[mapWidth, mapDepth];
        enemies = new string[mapWidth, mapDepth];
        pickups = new string[mapWidth, mapDepth];

        levelRoot = new GameObject("Level Root").transform;

        // Random.InitState(91142069);
        for (int z=0; z < terrain.GetLength(1); z ++)
        {
            for (int x = 0; x < terrain.GetLength(0); x++)
            {
                terrain[x, z] = "Solid";
            }
        }

        List<Miner> minerList = new List<Miner>();
        for (int i = 0; i < 5; i++)
        {
            Miner miner = new Miner();
            miner.levelGen = this;
            miner.posX = mapWidth / 2;
            miner.posZ = mapDepth / 2;

            minerList.Add(miner);
        }

        for (int i=0; i < 10000; i ++)
        {
            bool exit = true;
            for (int j=0;j < minerList.Count; j ++)
            {
                minerList[j].Update();

                if (minerList[j].lifetime > 0)
                {
                    exit = false;
                }
            }

            if (exit)
            {
                break;
            }
        }

        pickups[minerList[0].posX, minerList[0].posZ] = "Pistol";
        pickups[minerList[0].posX, minerList[0].posZ+1] = "MachineGun";
        pickups[minerList[0].posX, minerList[0].posZ+2] = "RocketLauncher";
        pickups[minerList[0].posX, minerList[0].posZ + 3] = "LaserRifle";
        playerSpawnPosition = new Vector3(minerList[0].posX, 4, minerList[0].posZ);

        // Build floor
        GameObject floor = Factory.instance.CreateFloor();
        floor.transform.parent = levelRoot;
        floor.transform.localScale = new Vector3(mapWidth, 1, mapDepth);
        floor.transform.position = new Vector3(mapWidth / 2.0f, 0, mapDepth / 2.0f);
        floor.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(mapWidth, mapDepth));

        // Build level from arrays
        for (int z = 0; z < terrain.GetLength(1); z++)
        {
            for (int x = 0; x < terrain.GetLength(0); x++)
            {
                if (terrain[x, z] == "Solid") {
                    GameObject wallGO = Factory.instance.CreateWall();
                    wallGO.transform.parent = levelRoot;
                    wallGO.transform.position = new Vector3(x, 1, z);
                }

                if (enemies[x, z] == "Walker")
                {
                    GameObject enemyGO = Factory.instance.CreateEnemyWalker();
                    enemyGO.transform.parent = levelRoot;
                    enemyGO.transform.position = new Vector3(x, 2, z);
                }

                if (enemies[x, z] == "Ranged Walker")
                {
                    GameObject enemyGO = Factory.instance.CreateRangedWalker();
                    enemyGO.transform.parent = levelRoot;
                    enemyGO.transform.position = new Vector3(x, 2, z);
                }
                if (pickups[x, z] == "Pistol")
                {
                     GameObject pistolGO = Factory.instance.CreatePistol();
                     pistolGO.transform.parent = levelRoot;
                     pistolGO.transform.position = new Vector3(x, 2, z);
                 }
                if (pickups[x, z] == "MachineGun") {
                    GameObject machineGunGO = Factory.instance.CreateMachineGun();
                    machineGunGO.transform.parent = levelRoot;
                    machineGunGO.transform.position = new Vector3(x, 2, z);
                }
                if (pickups[x, z] == "RocketLauncher") {
                    GameObject rocketLauncherGO = Factory.instance.CreateRocketLauncher();
                    rocketLauncherGO.transform.parent = levelRoot;
                    rocketLauncherGO.transform.position = new Vector3(x, 2, z);
                }
                if (pickups[x, z] == "LaserRifle")
                {
                    GameObject laserRifleGGO = Factory.instance.CreateLaserRifle();
                    laserRifleGGO.transform.parent = levelRoot;
                    laserRifleGGO.transform.position = new Vector3(x, 2, z);
                }
            }
        }
    }
}


public class Miner
{

    public LevelGeneration levelGen;
    public int posX = 3;
    public int posZ = 3;
    public int lifetime = 400;
    public int roomChance = 98;

    public void Update ()
    {
        // Movement
        int random = Random.Range(0, 4);
        if (random == 0)
        {
            posX++;
        }
        if (random == 1)
        {
            posX--;
        }
        if (random == 2)
        {
            posZ++;
        }
        if (random == 3)
        {
            posZ--;
        }
        
        // Death
        lifetime--;
        if (posX <= 1 || posX >= levelGen.mapWidth-2 ||
            posZ <= 1 || posZ >= levelGen.mapDepth-2)
        {
            lifetime = 0;
        }

        if (lifetime <= 0)
        {
            CreateRoom(2, 2);
            return;
        }

        // Create empty
        levelGen.terrain[posX, posZ] = "";

        // Create rooms
        if (Random.Range(0, 100) > roomChance)
        {
            CreateRoom(Random.Range(1, 5), Random.Range(1, 5));
        }

        // Enemies 
        if (Random.Range(0, 1000) > 995)
        {
            levelGen.enemies[posX, posZ] = "Walker";
        }
        if (Random.Range(0, 1000) > 995)
        {
            levelGen.enemies[posX, posZ] = "Ranged Walker";
        }
        // Pickups
        if (Random.Range(0, 10000) > 9990)
        {
            levelGen.pickups[posX, posZ] = "Pistol";
        }
        if (Random.Range(0, 10000) > 9990)
        {
            levelGen.pickups[posX, posZ] = "MachineGun";
        }
        if (Random.Range(0, 10000) > 9990)
        {
            levelGen.pickups[posX, posZ] = "RocketLauncher";
        }
        if (Random.Range(0, 10000) > 9990)
        {
            levelGen.pickups[posX, posZ] = "LaserRifle";
        }
    }

    void CreateRoom (int sizeX, int sizeZ)
    {
        for (int x= -sizeX; x < sizeX; x ++)
        {
            for (int z= -sizeZ; z < sizeZ; z++)
            {
                int compositeX = Mathf.Clamp(posX +x, 1, levelGen.mapWidth-2);
                int compositeZ = Mathf.Clamp(posZ +z, 1, levelGen.mapDepth-2);

                levelGen.terrain[compositeX, compositeZ] = "";
            }
        }
    }
}
