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
        //Get aim direction and rotate by 45 degrees
        Vector2 aimDirection = InputManager.instance.GetAimDirection(playerActor.model.playerID);
        
        if(aimDirection.magnitude <= 0.1)
        {
            aimDirection = aimDirection * 100f;
        }
        Vector3 normalizedAim = Quaternion.AngleAxis(45, Vector3.up) * new Vector3(aimDirection.x, 0, aimDirection.y);

        //Position to place wall
        Vector3 position = playerActor.transform.position + new Vector3(normalizedAim.x, normalizedAim.y, normalizedAim.z) * placementOffset;

        //Check for other walls at that position
        Collider[] hits = Physics.OverlapSphere(position, 0.2f);
        foreach(Collider hit in hits)
        {
            if(hit.gameObject != null && hit.gameObject.name.Equals("Wall"))
            {
                GameObject.Destroy(hit.gameObject, 0.1f);
            }
        }


        //Create wall and move to position with infinite HP
        GameObject wall = Factory.instance.CreateTrumpWall();
        wall.GetComponent<Wall>().health = int.MaxValue;
        wall.transform.position = position;
        BoxCollider wallBoxCollider = wall.GetComponent<BoxCollider>();
        
        //Destroy wall after x Seconds
        GameObject.Destroy(wall, 5f);

        AudioManager.instance.PlayAudio("Trump - Great great wall", 1, false);
    }
}
