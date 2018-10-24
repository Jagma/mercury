using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject[] buttonList;
    GameObject previousSelection;

    void Start ()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            GameObject a = buttonList[0];
            GameObject b = buttonList[1];

            Destroy(buttonList[2]);

            buttonList = new GameObject[2];
            buttonList[0] = a;
            buttonList[1] = b;
        }

        AudioManager.instance.PlayAudio("main_menu_loop", 1, true);
        InputManager.instance.GetPlayerInputDictionary().Clear();
    }

    private void Update()
    {
        for (int i=0; i < buttonList.Length; i ++) {
            if (eventSystem.currentSelectedGameObject == buttonList[i]) {
                buttonList[i].transform.Find("Text").GetComponent<Text>().color = Color.red;
                buttonList[i].transform.localScale = new Vector3(1.1f, 1.1f, 1);
            } else {
                buttonList[i].transform.Find("Text").GetComponent<Text>().color = new Color(1, 0.7f, 0);
                buttonList[i].transform.localScale = new Vector3(1f, 1, 1);
            }
        }

        // This ensures that there is always a selected UI component
        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(previousSelection);
        }
        if (previousSelection != eventSystem.currentSelectedGameObject) {
            AudioManager.instance.PlayAudio("menu1", 1, false);
        }

        previousSelection = eventSystem.currentSelectedGameObject;
    }

    public void Select(GameObject go)
    {
        eventSystem.SetSelectedGameObject(go);
    }

    public void NavigateCampaign ()
    {
        AudioManager.instance.PlayAudio("sfx_sounds_button5", .4f, false);
        AudioManager.instance.StopAudio("main_menu_loop");

        SceneManager.LoadScene("CampaignLobby");
    }

    public void NavigateTeamDeathmatch ()
    {
        AudioManager.instance.PlayAudio("sfx_sounds_button5", .4f, false);
        AudioManager.instance.StopAudio("main_menu_loop");
        SceneManager.LoadScene("MultiplayerHome");
    }

    public void NavigateExit ()
    {
        AudioManager.instance.PlayAudio("sfx_sounds_button5", .4f, false);
        Application.Quit();
    }
}