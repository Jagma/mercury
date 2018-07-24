using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TableRealmsManager : MonoBehaviour {

    public static TableRealmsManager instance;
    Dictionary<string, TableRealmsDevice> devices = new Dictionary<string, TableRealmsDevice>();
    
    void Awake() {
        if (instance == null) {
            instance = this;
        }
        //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
    }


    public void AddDevice (string id, TableRealmsDevice device) {
        devices.Add(id, device);
    }

    public void RemoveDevice (string id) {
        devices.Remove(id);
    }

    // Fallback for when there are no connected devices
    private void Update() {   
    }

    public TableRealmsDevice GetDevice (string id) {
        if (DeviceExists(id)) {
            return devices[id];
        }
        return null;
    }

    public bool DeviceExists (string id) {
        if (devices.ContainsKey(id) == true) {
            return true;
        }
        return false;
    }

    public Dictionary<string, TableRealmsDevice> GetDeviceDictionary () {
        return devices;
    }
}
