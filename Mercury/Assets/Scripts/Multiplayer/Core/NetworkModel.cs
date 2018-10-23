using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkModel : MonoBehaviour {

    public string uniqueID = "global";
    Dictionary<string, object> modelDictionary = new Dictionary<string, object>();
    Dictionary<string, object> updateDictionary = new Dictionary<string, object>();

    public static NetworkModel instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {
        NetworkManager.instance.Subscribe(ReceiveMessage);
    }

    public void SetModel(string key, object value) {
        updateDictionary[key] = value;
    }

    public object GetModel(string key) {
        if (modelDictionary.ContainsKey(key)) {
            return modelDictionary[key];
        }
        return null;
    }
    
    private void LateUpdate() {
        Dictionary<string, object> diffDictionary = new Dictionary<string, object>();
        foreach (KeyValuePair<string, object> kvp in updateDictionary) {
            if (modelDictionary.ContainsKey(kvp.Key) == false) {
                diffDictionary[kvp.Key] = kvp.Value;
            }
            else if (kvp.Value.Equals(modelDictionary[kvp.Key]) == false) {
                diffDictionary[kvp.Key] = kvp.Value;
            }
        }

        if (diffDictionary.Count > 0) {
            NetworkMessages.UpdateModel updateModel = new NetworkMessages.UpdateModel();
            updateModel.gameUniqueID = uniqueID;
            updateModel.uniqueID = uniqueID;
            updateModel.model = diffDictionary;

            NetworkManager.instance.Send(updateModel);
        }

        updateDictionary = new Dictionary<string, object>();
    }

    private void ReceiveMessage(NetworkMessages.MessageBase message) {
        if (message.GetType() == typeof(NetworkMessages.ModelUpdated)) {
            NetworkMessages.ModelUpdated modelUpdated = (NetworkMessages.ModelUpdated)message;
            if (modelUpdated.uniqueID == uniqueID) {                
                foreach(KeyValuePair<string, object> kvp in modelUpdated.model) {
                    modelDictionary[kvp.Key] = kvp.Value;
                }
            }
        }
    }
}
