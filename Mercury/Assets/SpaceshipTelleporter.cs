using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipTelleporter : MonoBehaviour {
    
    private void OnTriggerEnter(Collider col) {
        PlayerActor actor = col.GetComponent<PlayerActor>();        
        if (actor != null) {
            CameraSystem.instance.UnsubscribeFromTracking(actor.transform);
            Intermission.instance.PlayerTeleport(actor);

            Destroy(actor.gameObject);
        }
    }
}
