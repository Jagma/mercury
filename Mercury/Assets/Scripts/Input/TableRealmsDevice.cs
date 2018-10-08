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

    // SendAction(model.id .. ".JoinGame")

    // Select Trump
    long selectTrumpFrame = 0;
    bool selectTrumpState = false;

    public bool GetSelectTrump() {
        return TableRealmsModel.instance.GetData<bool>(GetDeviceID() + ".Button.BTNSelectTrump.Pressed");
    }
    public bool GetSelectTrumpWasPressed() {
        bool result = false;
        if (selectTrumpFrame == Time.frameCount && GetSelectTrump()) {
            result = true;
        }
        if (selectTrumpState == false && GetSelectTrump()) {
            selectTrumpFrame = Time.frameCount;
            result = true;
        }

        selectTrumpState = GetSelectTrump();
        return result;
    }

    // Select BinLaden
    long selectBinLadenFrame = 0;
    bool selectBinLadenState = false;

    public bool GetSelectBinLaden() {
        return TableRealmsModel.instance.GetData<bool>(GetDeviceID() + ".Button.BTNSelectBinLaden.Pressed");
    }
    public bool GetSelectBinLadenWasPressed() {
        bool result = false;
        if (selectBinLadenFrame == Time.frameCount && GetSelectBinLaden()) {
            result = true;
        }
        if (selectBinLadenState == false && GetSelectBinLaden()) {
            selectBinLadenFrame = Time.frameCount;
            result = true;
        }

        selectBinLadenState = GetSelectBinLaden();
        return result;
    }

    // Select Oprah
    long selectOprahFrame = 0;
    bool selectOprahState = false;

    public bool GetSelectOprah() {
        return TableRealmsModel.instance.GetData<bool>(GetDeviceID() + ".Button.BTNSelectOprah.Pressed");
    }
    public bool GetSelectOprahWasPressed() {
        bool result = false;
        if (selectOprahFrame == Time.frameCount && GetSelectOprah()) {
            result = true;
        }
        if (selectOprahState == false && GetSelectOprah()) {
            selectOprahFrame = Time.frameCount;
            result = true;
        }

        selectOprahState = GetSelectOprah();
        return result;
    }

    // Select Pope
    long selectPopeFrame = 0;
    bool selectPopeState = false;

    public bool GetSelectPope() {
        return TableRealmsModel.instance.GetData<bool>(GetDeviceID() + ".Button.BTNSelectPope.Pressed");
    }
    public bool GetSelectPopeWasPressed() {
        bool result = false;
        if (selectPopeFrame == Time.frameCount && GetSelectPope()) {
            result = true;
        }
        if (selectPopeState == false && GetSelectPope()) {
            selectPopeFrame = Time.frameCount;
            result = true;
        }

        selectPopeState = GetSelectPope();
        return result;
    }

    public bool GetCharacterSelect() {
        return TableRealmsModel.instance.GetData<bool>(GetDeviceID() + ".Button.BTNCharacterSelect.Pressed");
    }

    public bool GetJoinLobby() {
        return TableRealmsModel.instance.GetData<bool>(GetDeviceID() + ".Button.BTNJoinLobby.Pressed");
    }
    public bool GetLeaveLobby() {
        return TableRealmsModel.instance.GetData<bool>(GetDeviceID() + ".Button.BTNLeaveLobby.Pressed");
    }
    public bool GetLeaveReady() {
        return TableRealmsModel.instance.GetData<bool>(GetDeviceID() + ".Button.BTNLeaveReady.Pressed");
    }

    public Vector2 GetLeftStick() {
        Vector2 leftStick = Vector2.zero;
        leftStick.x = TableRealmsModel.instance.GetData<float>(GetDeviceID() + ".ThumbStick.StickLeft.x");
        leftStick.y = TableRealmsModel.instance.GetData<float>(GetDeviceID() + ".ThumbStick.StickLeft.y");
        return leftStick;
    }

    public Vector2 GetRightStick() {
        Vector2 leftStick = Vector2.zero;
        leftStick.x = TableRealmsModel.instance.GetData<float>(GetDeviceID() + ".ThumbStick.StickRight.x");
        leftStick.y = TableRealmsModel.instance.GetData<float>(GetDeviceID() + ".ThumbStick.StickRight.y");

        return leftStick;
    }

    public bool GetInteract() {
        return TableRealmsModel.instance.GetData<bool>(GetDeviceID() + ".Button.BTNInteract.Pressed");
    }
    public bool GetSwapWeapon() {
        return TableRealmsModel.instance.GetData<bool>(GetDeviceID() + ".Button.BTNSwapWeapon.Pressed");
    }
    public bool GetAbility() {
        return TableRealmsModel.instance.GetData<bool>(GetDeviceID() + ".Button.BTNAbility.Pressed");
    }
}
