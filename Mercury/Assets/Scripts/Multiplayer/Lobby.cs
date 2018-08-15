using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {

    public static string lobbyUniqueID;

    GameObject item;
	void Start () {
        NetworkManager.instance.Subscribe(ReceiveMessageLobby);

        item = GameObject.Find("Item");
        item.SetActive(false);

        NetworkMessages.RequestLobbyDetails requestLobbyDetails = new NetworkMessages.RequestLobbyDetails();
        requestLobbyDetails.lobbyUniqueID = lobbyUniqueID;
        NetworkManager.instance.Send(requestLobbyDetails);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            NetworkMessages.LeaveLobby leaveLobby = new NetworkMessages.LeaveLobby();
            leaveLobby.lobbyUniqueID = lobbyUniqueID;
            NetworkManager.instance.Send(leaveLobby);
        }

        if (Input.GetKeyUp(KeyCode.E)) {
            NetworkMessages.ExecuteLobby executeLobby = new NetworkMessages.ExecuteLobby();
            NetworkManager.instance.Send(executeLobby);
        }
    }

    public void ReceiveMessageLobby(NetworkMessages.MessageBase message) {
        if (message.GetType() == typeof(NetworkMessages.ReturnLobbyDetails)) {
            NetworkMessages.ReturnLobbyDetails lobbyDetails = (NetworkMessages.ReturnLobbyDetails)message;
            UpdateClientList(lobbyDetails.data.clientList);
        }
        if (message.GetType() == typeof(NetworkMessages.LobbyLeft)) {
            SceneManager.LoadScene("Multiplayer");
        }
        if (message.GetType() == typeof(NetworkMessages.LobbyExecuted)) {
            NetworkMessages.LobbyExecuted lobbyExecuted = (NetworkMessages.LobbyExecuted)message;
            NetworkManager.gameUniqueID = lobbyExecuted.lobbyUniqueID;
            SceneManager.LoadScene("GameMultiplayer");
        }
    }

    void UpdateClientList(List<string> clientList) {
        if (this == null || this.enabled == false) {
            return;
        }

        Transform root = GameObject.Find("Items").transform;
        while (root.childCount > 0) {
            Transform t = root.GetChild(0);
            Destroy(t.gameObject);
            t.parent = null;
        }

        for (int i = 0; i < clientList.Count; i++) {
            GameObject newItem = GameObject.Instantiate(item);
            newItem.transform.SetParent(root, false);
            newItem.SetActive(true);
            newItem.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.8f - i / 10f);
            newItem.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.9f - i / 10f);

            newItem.gameObject.name = i.ToString();

            newItem.transform.Find("PlayerCount").GetComponent<Text>().text = "";
            newItem.transform.Find("GameName").GetComponent<Text>().text = "Unique ID : " + clientList[i];
            newItem.transform.Find("PlayerCount").GetComponent<Text>().text = "";
        }
    }

}
