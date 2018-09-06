using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
public class ControllerManger : MonoBehaviour {
    
    public Dictionary<string, InputDevice> deviceList = new Dictionary<string, InputDevice>();

    public static ControllerManger instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            return;
        }

        foreach (InputDevice d in InControl.InputManager.Devices) {
            deviceList.Add(d.GUID.ToString(), d);
        }

        InControl.InputManager.OnDeviceAttached += inputDevice => DeviceConnect(inputDevice);
        InControl.InputManager.OnDeviceDetached += inputDevice => DeviceDisconnect(inputDevice);
    }

    private void DeviceDisconnect (InputDevice device) {

    }

    private void DeviceConnect (InputDevice device) {
        
    }

    public string GetDeviceID (InputDevice device) {
        foreach(KeyValuePair<string, InputDevice> kvp in deviceList) {
            if (device == kvp.Value) {
                return kvp.Key;
            }
        }

        return "";
    }

    public InputDevice GetDevice (string deviceID) {
        if (deviceList.ContainsKey(deviceID) == false) {
            Debug.LogError("Device not found");
            return null;
        }

        return deviceList[deviceID];
    }
}
