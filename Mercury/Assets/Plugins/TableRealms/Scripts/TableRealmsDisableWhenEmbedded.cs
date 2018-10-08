using UnityEngine;

public class TableRealmsDisableWhenEmbedded : MonoBehaviour {

    public MonoBehaviour[] enableWhenNotEmbeded;
    public MonoBehaviour[] enableWhenEmbeded;

    private bool lastValue = false;

    private void Start() {
        // Foirce the first execution
        lastValue = !TableRealmsGameNetwork.embeded;
    }

    void Update() {
        if (lastValue != TableRealmsGameNetwork.embeded) {
            lastValue = TableRealmsGameNetwork.embeded;

            foreach (MonoBehaviour behaviour in enableWhenNotEmbeded) {
                behaviour.enabled = !lastValue;
            }
            foreach (MonoBehaviour behaviour in enableWhenEmbeded) {
                behaviour.enabled = lastValue;
            }
        }
    }

}
