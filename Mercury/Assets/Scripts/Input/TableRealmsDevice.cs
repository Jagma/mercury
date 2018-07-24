using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableRealmsDevice : TableRealmsPlayerActionBehavior {

    public override void Start() {
        base.Start();
        TableRealmsManager.instance.AddDevice(gameObject.name, this);
    }

    public override void OnDestroy() {
        base.OnDestroy();
        TableRealmsManager.instance.RemoveDevice(gameObject.name);
    }

    public string GetDeviceID() {
        return gameObject.name;
    }

    public string GetDeviceAccountName() {
        return TableRealmsModel.instance.GetData<string>(GetDeviceID() + ".name");
    }

    public void DisplayPage(string pageName) {
        TableRealmsModel.instance.SetData(GetDeviceID() + ".page", pageName);
    }

    public void SetPlayerColor(Color c) {
        TableRealmsModel.instance.SetData(GetDeviceID() + ".playerColor", ToLuaColor(c));
    }

    string ToLuaColor(Color c) {
        string colorString =
            "{" +
            "red=" + c.r + "," +
            "green=" + c.g + "," +
            "blue=" + c.b + "," +
            "alpha=" + c.a + "," +
            "}";
        return colorString;
    }

    // Device methods when the console is on the "Menu" screen
    public void StartGame() {
    }

    // Device methods when the console is on the "Game" screen

    // Device methods when the console is on the "GameOver" screen
 
    public void NavigateMenu() {
    }

    // Polling methods
    public Vector2 GetLeftStick() {
        Vector2 leftStick = Vector2.zero;
        leftStick.x = TableRealmsModel.instance.GetData<float>(GetDeviceID() + ".ThumbStick.StickLeft.x");
        leftStick.y = TableRealmsModel.instance.GetData<float>(GetDeviceID() + ".ThumbStick.StickLeft.y");
        return leftStick;
    }

    public bool GetFire() {
        return TableRealmsModel.instance.GetData<bool>(GetDeviceID() + ".Button.Fire.Pressed");
    }
}
