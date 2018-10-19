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

    public int mapWidth = 128;
    public int mapDepth = 128;

    public string[,] terrain;
    public string[,] enemies;
    public string[,] pickups;

    Transform levelRoot;

    public void Generate ()
    {
        mapWidth = 64;
        mapDepth = 64;

        terrain = new string[mapWidth, mapDepth];
        enemies = new string[mapWidth, mapDepth];
        pickups = new string[mapWidth, mapDepth];

        levelRoot = new GameObject("Level Root").transform;

   //     Random.InitState(91142069);
        // Fill level with solids
        for (int z=0; z < terrain.GetLength(1); z ++)
        {
            for (int x = 0; x < terrain.GetLength(0); x++)
            {
                terrain[x, z] = "Solid";
            }
        }

        // Mine away solids
        List<Miner> minerList = new List<Miner>();

        int minerCount = Random.Range(3, 7);
        for (int i = 0; i < minerCount; i++)
        {
            Miner miner = new Miner();
            miner.levelGen = this;
            miner.posX = mapWidth / 2;
            miner.posZ = mapDepth / 2;

            minerList.Add(miner);
        }

        int runCount = Random.Range(300, 500);
        for (int i=0; i < runCount; i ++)
        {
            for (int j=0;j < minerList.Count; j ++)
            {
                minerList[j].Update();
            }
        }

        int playerSpawnX = minerList[0].posX;
        int playerSpawnZ = minerList[0].posZ;
        CreateRoom(playerSpawnX, playerSpawnZ, 2, 2);

        playerSpawnPosition = new Vector3(playerSpawnX, 4, playerSpawnZ);

        for (int z = playerSpawnZ - 8; z < playerSpawnZ + 8; z++) {
            for (int x = playerSpawnX - 8; x < playerSpawnX + 8; x++) {
                x = Mathf.Clamp(x, 0, terrain.GetLength(0)-1);
                z = Mathf.Clamp(z, 0, terrain.GetLength(1)-1);

                enemies[x, z] = "";
            }
        }


        pickups[minerList[0].posX, minerList[0].posZ] = "Normal Chest";
     //   pickups[minerList[0].posX, minerList[0].posZ] = "Rare Chest";

        // Build floor
        GameObject floor = Factory.instance.CreateFloor(ProgressionState.environmentName, 0);
        floor.transform.parent = levelRoot;
        floor.transform.localScale = new Vector3(mapWidth, 1, mapDepth);
        floor.transform.position = new Vector3(mapWidth / 2.0f, 0, mapDepth / 2.0f);
        floor.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(mapWidth, mapDepth));

        Random.InitState(System.DateTime.Now.Millisecond);

        // Build level from arrays
        for (int z = 0; z < terrain.GetLength(1); z++)
        {
            for (int x = 0; x < terrain.GetLength(0); x++)
            {
                if (terrain[x, z] == "Solid")
                {
                    GameObject wallGO = Factory.instance.CreateWall(ProgressionState.environmentName, 0);
                    wallGO.transform.parent = levelRoot;
                    wallGO.transform.position = new Vector3(x, 1, z);
                    if(x == 0 || x == mapWidth-1)
                        wallGO.GetComponent<Wall>().health = int.MaxValue;
                    if (z == 0 || z == mapDepth-1)
                        wallGO.GetComponent<Wall>().health = int.MaxValue;
                }

                if (enemies[x, z] == "Martian Boss")
                {
                   GameObject enemyGO = Factory.instance.CreateMartianBoss();
                   enemyGO.transform.parent = levelRoot;
                   enemyGO.transform.position = new Vector3(x, 2, z);
                   GameProgressionManager.instance.IncreaseEnemyCount();
                }

                if (enemies[x, z] == "Walker")
                {
                    GameObject enemyGO = Factory.instance.CreateEnemyWalker();
                    enemyGO.transform.parent = levelRoot;
                    enemyGO.transform.position = new Vector3(x, 2, z);
                    GameProgressionManager.instance.IncreaseEnemyCount();
                    EnemyManager.instance.AddEnemy(enemyGO.GetComponent<Enemy>());
                }

                if (enemies[x, z] == "Ranged Walker")
                {
                    GameObject enemyGO = Factory.instance.CreateRangedWalker();
                    enemyGO.transform.parent = levelRoot;
                    enemyGO.transform.position = new Vector3(x, 2, z);
                    GameProgressionManager.instance.IncreaseEnemyCount();
                    EnemyManager.instance.AddEnemy(enemyGO.GetComponent<Enemy>());
                }

                if (enemies[x, z] == "Overlord Walker")
                {
                    GameObject enemyGO = Factory.instance.CreateOverlordWalker();
                    enemyGO.transform.parent = levelRoot;
                    enemyGO.transform.position = new Vector3(x, 2, z);
                    GameProgressionManager.instance.IncreaseEnemyCount();
                    EnemyManager.instance.AddEnemy(enemyGO.GetComponent<Enemy>());
                }

                if (pickups[x, z] == "Normal Chest")
                {
                    GameObject chestGO = Factory.instance.CreateNormalChest();
                    chestGO.transform.parent = levelRoot;
                    chestGO.transform.position = new Vector3(x, 2, z);
                }

                if (pickups[x, z] == "Rare Chest")
                {
                    GameObject chestGO = Factory.instance.CreateRareChest();
                    chestGO.transform.parent = levelRoot;
                    chestGO.transform.position = new Vector3(x, 2, z);
                }

                if (pickups[x, z] == "Medkit")
                {
                    GameObject medkitGO = Factory.instance.CreateMedkit();
                    medkitGO.transform.parent = levelRoot;
                    medkitGO.transform.position = new Vector3(x, 2, z);
                }

                if (pickups[x, z] == "Medpack")
                {
                    GameObject medpackGO = Factory.instance.CreateMedpack();
                    medpackGO.transform.parent = levelRoot;
                    medpackGO.transform.position = new Vector3(x, 2, z);
                }
            }
        }
    }

    void CreateRoom(int posX, int posZ, int sizeX, int sizeZ) {
        for (int x = -sizeX; x < sizeX; x++) {
            for (int z = -sizeZ; z < sizeZ; z++) {
                int compositeX = Mathf.Clamp(posX + x, 1, mapWidth - 2);
                int compositeZ = Mathf.Clamp(posZ + z, 1, mapDepth - 2);

                terrain[compositeX, compositeZ] = "";
            }
        }
    }

    public void SpawnMartianBoss(Vector3 playerPosition) {
        GameObject enemyGO = Factory.instance.CreateMartianBoss();
        enemyGO.transform.parent = levelRoot;
        enemyGO.transform.position = new Vector3(playerPosition.x, 5, playerPosition.z);
        EnemyManager.instance.AddEnemy(enemyGO.GetComponent<Enemy>());
    }
}

public class Miner
{
    public LevelGeneration levelGen;
    public int posX = 3;
    public int posZ = 3;

    public int dirX = 1;
    public int dirZ = 0;

    public void Update ()
    {
        // Movement
        if (Random.Range(0, 1f) > 0.6f) {
            // Keep same direction
        } else {
            int random = Random.Range(0, 4);

            if (random == 0) {
                dirX = 1;
                dirZ = 0;
            }
            if (random == 1) {
                dirX = 0;
                dirZ = 1;
            }
            if (random == 2) {
                dirX = -1;
                dirZ = 0;
            }
            if (random == 3) {
                dirX = 0;
                dirZ = -1;
            }

            if (Random.Range(0, 1f) > 0.95f) {
                CreateRoom(Random.Range(1, 3), Random.Range(1, 3));
            }
        }

        posX += dirX;
        posZ += dirZ;

        posX = Mathf.Clamp(posX, 1, levelGen.mapWidth - 2);
        posZ = Mathf.Clamp(posZ, 1, levelGen.mapDepth - 2);

        // Death
        if (posX <= 1 || posX >= levelGen.mapWidth-2 ||
            posZ <= 1 || posZ >= levelGen.mapDepth-2)
        {
            return;            
        }



        // Create empty
        levelGen.terrain[posX, posZ] = "";

        // Create rooms
        if (Random.Range(0, 1f) > 0.990f)
        {
            CreateRoom(Random.Range(1, 5), Random.Range(1, 5));
        }

        // Enemies 
        if (Random.Range(0, 1f) > 0.99f)
        {
            levelGen.enemies[posX, posZ] = "Walker";
        }
        if (Random.Range(0, 1f) > 0.995f)
        {
        //    levelGen.enemies[posX, posZ] = "Martian Boss";
        }
        if (Random.Range(0, 1f) > 0.995f)
        {
            levelGen.enemies[posX, posZ] = "Ranged Walker";
        }
        if (Random.Range(0, 1f) > 0.995f)
        {
            levelGen.enemies[posX, posZ] = "Overlord Walker";
        }
        // Pickups
        if (Random.Range(0, 1f) > 0.9990f)
        {
            levelGen.pickups[posX, posZ] = "Normal Chest";
        }
        if (Random.Range(0, 1f) > 0.9996f)
        {
            levelGen.pickups[posX, posZ] = "Rare Chest";
        }
        if (Random.Range(0, 1f) > 0.998f)
        {
            levelGen.pickups[posX, posZ] = "Medkit";
        }
        if (Random.Range(0, 1f) > 0.998f)
        {
            levelGen.pickups[posX, posZ] = "Medpack";
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
