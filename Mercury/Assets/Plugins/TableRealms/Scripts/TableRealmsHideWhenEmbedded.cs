using UnityEngine;

public class TableRealmsHideWhenEmbedded : MonoBehaviour {

    void Update() {
        if (TableRealmsGameNetwork.instance.IsEmbedded()) {
            gameObject.SetActive(false);
        }
    }

}
