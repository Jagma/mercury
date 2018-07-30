using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

    public static LevelGeneration instance;
    private void Awake() {
        instance = this;        
    }

    int mapWidth = 64;
    int mapDepth = 64;

    string[,] terrain;
    string[,] enemies;
    Transform levelRoot;
    public void Generate () {
        terrain = new string[mapWidth, mapDepth];
        enemies = new string[mapWidth, mapDepth];

        levelRoot = new GameObject("Level Root").transform;

        Random.InitState(91142069);

        for (int z=0; z < terrain.GetLength(1); z ++) {
            for (int x = 0; x < terrain.GetLength(0); x++) {
                if (x==0 || z == 0 || x == mapWidth-1 || z == mapDepth-1) {
                    terrain[x, z] = "Solid";
                }
                if (Random.Range(0, 100) < 2) {
                    terrain[x, z] = "Solid";
                }
                if (terrain[x, z] != "Solid" && Random.Range(0, 100) < 1) {
                    enemies[x, z] = "Walker";
                }
            }
        }

        // Build floor
        GameObject floor = Factory.instance.CreateFloor();
        floor.transform.parent = levelRoot;
        floor.transform.localScale = new Vector3(mapWidth, 1, mapDepth);
        floor.transform.position = new Vector3(mapWidth / 2.0f, 0, mapDepth / 2.0f);

        // Build level from arrays
        for (int z = 0; z < terrain.GetLength(1); z++) {
            for (int x = 0; x < terrain.GetLength(0); x++) {
                if (terrain[x, z] == "Solid") {
                    GameObject wallGO = Factory.instance.CreateWall();
                    wallGO.transform.parent = levelRoot;
                    wallGO.transform.position = new Vector3(x, 1, z);
                }

                if (enemies[x, z] == "Walker") {
                    GameObject enemyGO = Factory.instance.CreateEnemyWalker();
                    enemyGO.transform.parent = levelRoot;
                    enemyGO.transform.position = new Vector3(x, 2, z);
                }
            }
        }        
    }
}
