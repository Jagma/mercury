using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour {

    // TODO: Check controller input and handle navigation

    public static EventSystemManager instance;

    EventSystem eventSystem;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        eventSystem = GetComponent<EventSystem>();
    }
	
	void Update () {
        // This ensures that there is always a selected UI component
		if (eventSystem.currentSelectedGameObject == null) {
            eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
        }
	}
}
