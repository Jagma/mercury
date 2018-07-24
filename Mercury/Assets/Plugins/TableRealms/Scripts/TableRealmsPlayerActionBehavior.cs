using UnityEngine;

public abstract class TableRealmsPlayerActionBehavior : ActionBehaviour {

    public override string GetSourceName() {
        return gameObject.name;
    }

    public bool GetButtonPressed(string buttonName) {
        return TableRealmsModel.instance.GetData<bool>(gameObject.name + ".Button." + buttonName + ".Pressed");
    }

    public Vector2 GetThumbStickVector(string stickName) {
        return new Vector2((float)TableRealmsModel.instance.GetData<double>(gameObject.name + ".ThumbStick." + stickName + ".x"), (float)TableRealmsModel.instance.GetData<double>(gameObject.name + ".ThumbStick." + stickName + ".y"));
    }

    public void SetPlayerPage(string page) {
        TableRealmsModel.instance.SetData(gameObject.name + ".page", page);
    }
}
