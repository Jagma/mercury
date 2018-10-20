using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerHome : MonoBehaviour {

    public void ConnectGlobal () {
        NetworkManager.instance.SetServerAddress("ws://127.0.0.1:3000/lobby");
        NetworkManager.instance.Connect();

        SceneManager.LoadScene("LobbyManager");
    }
}
