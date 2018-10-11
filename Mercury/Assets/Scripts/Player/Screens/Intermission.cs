using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intermission : MonoBehaviour {
    public Texture2D spaceshipDesign;
	void Start () {
        Transform levelRoot = new GameObject("LevelRoot").transform;
        AudioManager.instance.PlayAudio("Game_music_Magellanic_clouds", .4f, true);
        for (int z = 0; z < spaceshipDesign.height; z++) {
            for (int x = 0; x < spaceshipDesign.width; x++) {
                Color col = spaceshipDesign.GetPixel(x, z);
                if (col == new Color(1, 1, 0)) {
                    GameObject wallGO = Factory.instance.CreateFloor("Spaceship", 0);
                    wallGO.transform.parent = levelRoot;
                    wallGO.transform.position = new Vector3(x, 0, z);
                }

                if (col == Color.white) {
                    GameObject wallGO = Factory.instance.CreateWall("Spaceship", 0);
                    wallGO.transform.parent = levelRoot;
                    wallGO.transform.position = new Vector3(x, 1, z);
                }
            }
        }       

    }

    void Update() {
        
    }
}
