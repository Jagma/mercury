using UnityEngine;

public class TableRealmsSamplePlayer : TableRealmsPlayerActionBehavior, DataTokenChangeListener {

    public override void Start() {
        base.Start();
        Tokenizer.AddDataTokenChangeListener(this, "T{" + gameObject.name + ".ThumbStick.Stick.x}");
        Tokenizer.AddDataTokenChangeListener(this, "T{" + gameObject.name + ".ThumbStick.Stick.y}");
        Tokenizer.AddDataTokenChangeListener(this, "T{" + gameObject.name + ".Button.Fire.Pressed}");
    }

    public override void OnDestroy() {
        base.OnDestroy();
        Tokenizer.RemoveDataTokenChangeListener(this);
    }

    public void DataTokenChanged(string token, object newvalue) {
        Debug.Log(token+"="+newvalue);
    }

    public void Fire() {
        Debug.Log("Table realms client "+gameObject.name+" Fire called.");
    }
}
