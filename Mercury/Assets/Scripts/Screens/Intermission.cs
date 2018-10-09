using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intermission : MonoBehaviour {

    string[,] terrain = new string[32, 32];
	void Start () {
        // Nose
        terrain[5, 0] = "Wall";

        terrain[4, 1] = "Wall";
        terrain[6, 1] = "Wall";

        terrain[3, 2] = "Wall";
        terrain[7, 2] = "Wall";

        terrain[3, 3] = "Wall";
        terrain[7, 3] = "Wall";

        terrain[3, 4] = "Wall";
        terrain[7, 4] = "Wall";

        terrain[3, 4] = "Wall";
        terrain[7, 4] = "Wall";

        terrain[2, 4] = "Wall";
        terrain[8, 4] = "Wall";

        terrain[1, 5] = "Wall";
        terrain[9, 5] = "Wall";

        terrain[1, 6] = "Wall";
        terrain[9, 6] = "Wall";

        // Body
        for (int z=7; z < 30; z ++) {
            terrain[0, z] = "Wall";
            terrain[10, z] = "Wall";
        }

        terrain[1, 29] = "Wall";
        terrain[9, 29] = "Wall";

        terrain[2, 29] = "Wall";
        terrain[8, 29] = "Wall";

        terrain[3, 29] = "Wall";
        terrain[7, 29] = "Wall";

        terrain[3, 28] = "Wall";
        terrain[7, 28] = "Wall";

        terrain[3, 27] = "Wall";
        terrain[7, 27] = "Wall";

        terrain[3, 26] = "Wall";
        terrain[7, 26] = "Wall";

        terrain[4, 26] = "Wall";
        terrain[6, 26] = "Wall";

        terrain[5, 26] = "Wall";

        Transform levelRoot = new GameObject("LevelRoot").transform;

        for (int z = 0; z < terrain.GetLength(1); z++) {
            for (int x = 0; x < terrain.GetLength(0); x++) {
                if (terrain[x, z] == "Wall") {
                    GameObject wallGO = Factory.instance.CreateWall();
                    wallGO.transform.parent = levelRoot;
                    wallGO.transform.position = new Vector3(x, 1, z);
                }
            }
        }
    }

    void Update() {
        
    }
}
