using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableRealmsPeerDisableComponentsIfNotHost : MonoBehaviour {

    public MonoBehaviour[] componentsToDisable;
    public MonoBehaviour[] componentsToEnable;

    void Start() {
        bool disableScripts = (TableRealmsGameNetwork.p2p && !TableRealmsGameNetwork.host);
        Debug.Log("TableRealms: Disable Scripts p2p="+TableRealmsGameNetwork.p2p+" host="+TableRealmsGameNetwork.host+ " disableScripts=" + disableScripts);
    }

    public void Update() {
        bool disableScripts = (TableRealmsGameNetwork.p2p && !TableRealmsGameNetwork.host);
        foreach (MonoBehaviour componentToDisable in componentsToDisable) {
            if (componentToDisable.enabled != disableScripts) {
                Debug.Log("TableRealms: " + (disableScripts? "Enabling " : "Disabling ") + componentToDisable.name + "." + componentToDisable.GetType().Name);
                componentToDisable.enabled = disableScripts;
            }
        }
        foreach (MonoBehaviour componentToEnable in componentsToEnable) {
            if (componentToEnable.enabled == disableScripts) {
                Debug.Log("TableRealms: " + (!disableScripts ? "Enabling " : "Disabling ") + componentToEnable.name + "." + componentToEnable.GetType().Name);
                componentToEnable.enabled = !disableScripts;
            }
        }
    }
}
