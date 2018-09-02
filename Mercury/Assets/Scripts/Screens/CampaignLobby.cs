using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampaignLobby : MonoBehaviour {

    public Color[] playerColors;

    List<GameObject> portraitList = new List<GameObject>();
    GameObject portraitPrefab;

    private void Awake() {
        portraitPrefab = GameObject.Find("Portrait_Prefab");
        portraitPrefab.SetActive(false);
    }
	
	void Update () {

        // Keyboard
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (InputManager.instance.GetPlayerInput("Keyboard|0001") != null) {
                SceneManager.LoadScene("GameCOOP");
            } else {
                PlayerJoin("Keyboard|0001", PlayerInput.InputType.Keyboard);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (InputManager.instance.GetPlayerInput("Keyboard|0001") == null) {
                SceneManager.LoadScene("Menu");
            } else {
                PlayerLeave("Keyboard|0001");
            }
        }     
        
        // TODO: Add controller join

        // TODO: Add table realms join

    }

    void PlayerJoin (string id, PlayerInput.InputType type) {
        Dictionary<string, PlayerInput> inputDictionary = InputManager.instance.GetPlayerInputDictionary();
        if (inputDictionary.ContainsKey(id) == false) {
            PlayerInput pi = new PlayerInput(id, type);
            InputManager.instance.AddPlayerInput(pi);

            CreatePortrait(id);
        }
    }

    void PlayerLeave(string id) {
        PlayerInput pi = InputManager.instance.GetPlayerInput(id);
        InputManager.instance.RemovePlayerInput(pi);

        for (int i = 0; i < portraitList.Count; i ++) {
            if (portraitList[i].name == id) {
                Destroy(portraitList[i]);
                portraitList.RemoveAt(i);

                break;
            }
        }
    }

    void StartGame () {
        if (InputManager.instance.GetPlayerInputDictionary().Count >= 1) {
            SceneManager.LoadScene("GameCOOP");
        }
    }

    void CreatePortrait (string id) {
        GameObject p = Instantiate(portraitPrefab);
        p.transform.SetParent(portraitPrefab.transform.parent, false);

        p.name = id;
        p.GetComponent<Outline>().effectColor = playerColors[portraitList.Count];
        p.SetActive(true);

        portraitList.Add(p);
    }
}
