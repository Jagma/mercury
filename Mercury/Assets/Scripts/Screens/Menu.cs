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
    int selectionIndex = 0;

    GameObject previousSelection;

    void Start ()
    {
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
        previousSelection = eventSystem.currentSelectedGameObject;
    }

    public void Select(GameObject go)
    {
        eventSystem.SetSelectedGameObject(go);
    }

    public void NavigateCampaign ()
    {
        AudioManager.instance.PlayAudio("sfx_sounds_button5", 1, false);
        AudioManager.instance.StopAudio("main_menu_loop");
        SceneManager.LoadScene("CampaignLobby");
    }

    public void NavigateTeamDeathmatch ()
    {
        AudioManager.instance.PlayAudio("sfx_sounds_button5", 1, false);
        // SceneManager.LoadScene("");
        Debug.LogError("Not implemented");
    }

    public void NavigateExit ()
    {
        AudioManager.instance.PlayAudio("sfx_sounds_button5", 1, false);
        Application.Quit();
    }
}