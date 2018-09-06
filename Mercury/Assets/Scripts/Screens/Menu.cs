using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    
    public EventSystem eventSystem;
    public GameObject[] buttonList;
    int selectionIndex = 0;

    GameObject previousSelection;
    void Start ()
    {
        //AudioManager.instance.PlayAudio("main_menu_loop", 1, true);
        InputManager.instance.GetPlayerInputDictionary().Clear();
    }

    private void Update()
    {
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
        AudioManager.instance.StopAudio("main_menu_loop");
        SceneManager.LoadScene("CampaignLobby");
    }

    public void NavigateTeamDeathmatch ()
    {
        // SceneManager.LoadScene("");
        Debug.LogError("Not implemented");
    }

    public void NavigateExit ()
    {
        Application.Quit();
    }
}