using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intermission : MonoBehaviour {
    public Texture2D spaceshipDesign;
	void Start () {
        Transform levelRoot = new GameObject("LevelRoot").transform;

        for (int z = 0; z < spaceshipDesign.height; z++) {
            for (int x = 0; x < spaceshipDesign.width; x++) {
                Color col = spaceshipDesign.GetPixel(x, z);
                if (col == Color.red) {
                    GameObject wallGO = Factory.instance.CreateFloor("Spaceship");
                    wallGO.transform.parent = levelRoot;
                    wallGO.transform.position = new Vector3(x, 0, z);
                }

                if (col == Color.white) {
                    GameObject wallGO = Factory.instance.CreateWall("Spaceship");
                    wallGO.transform.parent = levelRoot;
                    wallGO.transform.position = new Vector3(x, 1, z);
                }
            }
        }       

    }

    void Update() {
        
    }
}
