using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTrump : Ability {
    public override void Init() {
        base.Init();

        // Stats
        cooldown = 5f;
    }

    protected override void Use() {
        base.Use();        

        Vector3 aimDirection = InputManager.instance.GetAimDirection(playerActor.model.playerID);
        Vector3 placePos = playerActor.transform.position + new Vector3(aimDirection.x, 0, aimDirection.y) * 1f;

        //Check for other walls at that position
        Collider[] hits = Physics.OverlapSphere(placePos, 0.2f);
        foreach(Collider hit in hits)
        {
            if(hit.gameObject != null && hit.gameObject.name.Equals("Wall"))
            {
                GameObject.Destroy(hit.gameObject, 0.1f);
            }
        }


        //Create trump wall
        GameObject wall = Factory.instance.CreateTrumpWall();        
        wall.transform.position = placePos;
        BoxCollider wallBoxCollider = wall.GetComponent<BoxCollider>();
        //Destroy wall after x Seconds
        GameObject.Destroy(wall, 5f);
    }
}
