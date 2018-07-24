using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput { 
    // A player input is a mapping between a player and a device.
    // The device can be a TableRealms device or a controller.

    public enum InputType { TableRealms, Controller}

    public InputType inputType = InputType.TableRealms;
    public string playerID = "-1";

    public PlayerInput(string playerID, InputType inputType) {
        this.playerID = playerID;
        this.inputType = inputType;
    }

    // ** Methods :
    public Vector2 GetMoveDirection () {
        if (inputType == InputType.TableRealms) {
            return TableRealmsManager.instance.GetDevice(playerID).GetLeftStick();
        }
        return Vector2.zero;
    }

    public bool GetFire () {
        if (inputType == InputType.TableRealms) {
            return TableRealmsManager.instance.GetDevice(playerID).GetFire();
        }
        return false; 
    }
}
