using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour {

    GameObject item;
    void Start () {
        NetworkManager.instance.Subscribe(ReceiveMessage);

        item = GameObject.Find("Item");
        item.SetActive(false);

        if (NetworkManager.instance.GetConnectionStatus() == NetworkManager.ConnectionStatus.Connected) {
            NetworkMessages.RequestServerDetails requestServerDetails = new NetworkMessages.RequestServerDetails();
            NetworkManager.instance.Send(requestServerDetails);
        }
    }

    public void ReceiveMessage (NetworkMessages.MessageBase message) {
        if (message.GetType() == typeof(NetworkMessages.ConnectionEstablished)) {
            NetworkMessages.RequestServerDetails requestServerDetails = new NetworkMessages.RequestServerDetails();
            NetworkManager.instance.Send(requestServerDetails);
        }

        if (message.GetType() == typeof(NetworkMessages.ReturnServerDetails)) {
            NetworkMessages.ReturnServerDetails returnServerDetails = (NetworkMessages.ReturnServerDetails)message;

            lobbyList.Clear();
            for (int i = 0; i < returnServerDetails.lobbyList.Count; i++) {
                lobbyList.Add(returnServerDetails.lobbyList[i]);
            }

            UpdateLobbyList();
        }

        if (message.GetType() == typeof(NetworkMessages.LobbyJoined)) {
            NetworkMessages.LobbyJoined lobbyJoined = (NetworkMessages.LobbyJoined)message;
            Lobby.lobbyUniqueID = lobbyJoined.lobbyUniqueID;
            SceneManager.LoadScene("Lobby");
        }
    }

    public List<NetworkMessages.LobbyListing> lobbyList = new List<NetworkMessages.LobbyListing>();
    void UpdateLobbyList () {
        if (this == null || this.enabled == false) {
            return;
        }

        Transform root = GameObject.Find("Items").transform;
        while(root.childCount > 0) {
            Transform t = root.GetChild(0);
            Destroy(t.gameObject);
            t.parent = null;
        }

        for (int i=0; i < lobbyList.Count; i ++) {
            GameObject newItem = GameObject.Instantiate(item);
            newItem.transform.SetParent(root, false);
            newItem.SetActive(true);
            newItem.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.8f - i / 10f);
            newItem.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.9f - i / 10f);

            newItem.gameObject.name = i.ToString();
            newItem.GetComponent<Button>().onClick.AddListener(delegate { JoinLobbyClick(newItem.gameObject.name);});

            newItem.transform.Find("PlayerCount").GetComponent<Text>().text = "";
            newItem.transform.Find("GameName").GetComponent<Text>().text = "Unique ID : " + lobbyList[i].uniqueID;
            newItem.transform.Find("PlayerCount").GetComponent<Text>().text = "";
        }
    }
    
	void Update () {
		if (Input.GetKeyDown(KeyCode.H)) {
            NetworkMessages.HostLobby hostLobby = new NetworkMessages.HostLobby();
            NetworkMessages.LobbyDetails data = new NetworkMessages.LobbyDetails();
            data.name = Random.Range(0, 100).ToString() + "My Random Game Name" + Random.Range(0, 100).ToString();
            hostLobby.data = data;

            NetworkManager.instance.Send(hostLobby);            
        }
    }

    public void JoinLobbyClick (string index) {
        NetworkMessages.JoinLobby joinLobby = new NetworkMessages.JoinLobby();
        joinLobby.lobbyUniqueID = lobbyList[int.Parse(index)].uniqueID;

        NetworkManager.instance.Send(joinLobby);
    }
}
