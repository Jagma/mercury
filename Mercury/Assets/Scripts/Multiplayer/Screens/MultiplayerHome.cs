using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerHome : MonoBehaviour {

    private void Start() {
        NetworkManager.instance.Subscribe(ReceiveMessage);
    }

    public void ConnectGlobal () {
        NetworkManager.instance.SetServerAddress("mercury-multiplayer.herokuapp.com");
        NetworkManager.instance.Connect();        
    }

    public void ConnectLocal () {
        string address = GameObject.Find("InputField").GetComponent<InputField>().text;
        NetworkManager.instance.SetServerAddress(address);
        NetworkManager.instance.Connect();
    }

    public void ReceiveMessage (NetworkMessages.MessageBase message) {
        if (message.GetType() == typeof(NetworkMessages.ConnectionEstablished)) {
            SceneManager.LoadScene("LobbyManager");
        }
    }
}
