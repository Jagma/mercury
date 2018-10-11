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
                    GameObject floorGO = Factory.instance.CreateFloor("Spaceship", 0);
                    floorGO.transform.parent = levelRoot;
                    floorGO.transform.position = new Vector3(x, 0, z);
                }
                if (col == new Color(0, 1, 0)) {
                    GameObject floorGO = Factory.instance.CreateFloor("Spaceship", 1);
                    floorGO.transform.localEulerAngles = new Vector3(0, Random.Range(0, 4) * 90, 0);
                    floorGO.transform.parent = levelRoot;
                    floorGO.transform.position = new Vector3(x, 0, z);
                }
                if (col == new Color(0, 1, 1)) {
                    GameObject floorGO = Factory.instance.CreateFloor("Spaceship", 2);
                    floorGO.transform.parent = levelRoot;
                    floorGO.transform.position = new Vector3(x, 0, z);
                    floorGO.transform.localEulerAngles = new Vector3(0, Random.Range(0, 4) * 90, 0);
                }

                if (col == new Color(1, 1, 1)) {
                    GameObject wallGO = Factory.instance.CreateWall("Spaceship", 0);
                    wallGO.transform.parent = levelRoot;
                    wallGO.transform.position = new Vector3(x, 1, z);                    
                }
                if (col == new Color(0, 0, 1)) {
                    GameObject wallGO = Factory.instance.CreateWall("Spaceship", 1);
                    wallGO.transform.parent = levelRoot;
                    wallGO.transform.position = new Vector3(x, 1, z);
                    wallGO.transform.localEulerAngles = new Vector3(0, Random.Range(1, 4) * 90, 0);
                }
            }
        }       

    }

    void Update() {
        
    }
}
