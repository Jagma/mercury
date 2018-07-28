using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

    public static LevelGeneration instance;
    private void Awake() {
        instance = this;        
    }

    string[,] terrain = new string[64, 64];
    public void Generate () {
        Random.InitState(91142069);

        for (int y=0; y < terrain.GetLength(1); y ++) {
            for (int x = 0; x < terrain.GetLength(0); x++) {
                if (x==0 || y == 0 || x == terrain.GetLength(0)-1 || y == terrain.GetLength(1)-1) {
                    terrain[x, y] = "Solid";
                }
                if (Random.Range(0, 100) < 2) {
                    terrain[x, y] = "Solid";
                }
            }
        }


        // Build level from arrays
        for (int y = 0; y < terrain.GetLength(1); y++) {
            for (int x = 0; x < terrain.GetLength(0); x++) {
                if (terrain[x, y] == "Solid") {
                    GameObject wallGO = Factory.instance.CreateWall();
                    wallGO.transform.position = new Vector3(x, 1, y);
                }
            }
        }
    }
}
