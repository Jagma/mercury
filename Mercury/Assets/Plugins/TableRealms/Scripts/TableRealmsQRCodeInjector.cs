using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TableRealmsQRCodeInjector : MonoBehaviour {

    public Image image;

    private Texture2D oldTexture = null;

    private void Start(){
        if (image == null) {
            image = gameObject.GetComponentInChildren<Image>();
        }

        if (image == null) {
            Debug.LogError("TableRealms: Unable to find imaghe to render QR Code into.");
        }
    }

    void Update() {
        if (TableRealmsGameNetwork.instance == null) {
            Debug.LogError("TableRealms: Unable to find TableRealmsGameNetwork instance, this must be added in your first scene.");
        } else {
            if (TableRealmsGameNetwork.instance.joinQRCodeTexture!=null && !Object.ReferenceEquals(oldTexture, TableRealmsGameNetwork.instance.joinQRCodeTexture)) {
                oldTexture = TableRealmsGameNetwork.instance.joinQRCodeTexture;
                image.sprite = Sprite.Create(oldTexture, new Rect(0,0, oldTexture.width, oldTexture.height), new Vector2(oldTexture.width/2, oldTexture.height/2));
                image.sprite.texture.filterMode = FilterMode.Point;
            }
        }
    }

}
