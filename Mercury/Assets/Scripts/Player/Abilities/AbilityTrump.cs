using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTrump : Ability {
    float wallLifetime = 4f;
    int wallCount = 0;
    public override void Init() {
        base.Init();

        // Stats
        cooldown = 0.2f;
        
    }

    protected override void Use() {
        base.Use();
        wallCount++;

        //Get aim direction and rotate by 45 degrees
        Vector2 aimDirection = InputManager.instance.GetAimDirection(playerActor.model.playerID);
        
        if(aimDirection.magnitude <= 0.1)
        {
            aimDirection = aimDirection * 100f;
        }
        Vector3 normalizedAim = Quaternion.AngleAxis(45, Vector3.up) * new Vector3(aimDirection.x, 0, aimDirection.y);

        //Position to place wall
        Vector3 position = playerActor.transform.position + new Vector3(normalizedAim.x, 0, normalizedAim.z) * placementOffset;
        position.y = 1;

        //Check for other walls at that position
        int environmentLayerID = LayerMask.NameToLayer("Environment");
        int environmentLayerMask = 1 << environmentLayerID;
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f, environmentLayerMask);

        foreach(Collider col in colliders)
        {
            Wall wallScript = col.GetComponent<Wall>();
            if (wallScript != null)
            {
                wallScript.Damage(1000);
            }
        }


        //Create wall and move to position with infinite HP
        GameObject wall = Factory.instance.CreateWall("Trump", 0);
        wall.GetComponent<Wall>().health = int.MaxValue;
        
        wall.transform.position = position;
        wall.transform.eulerAngles = new Vector3(0, -Mathf.Atan2(normalizedAim.z, normalizedAim.x) * Mathf.Rad2Deg, 0);
  
        //Destroy wall after x Seconds
        GameObject.Destroy(wall, wallLifetime);


        int randomClipInt;
        randomClipInt = UnityEngine.Random.Range(0,3);
        switch (randomClipInt)
            {
                case 0:
                    AudioManager.instance.PlayAudio("Trump - Great wall", .4f, false);
                    break;

                case 1:
                    AudioManager.instance.PlayAudio("Trump - Need a wall", .4f, false);
                    break;

                case 2:
                    AudioManager.instance.PlayAudio("Trump - Great great wall", .4f, false);
                    break;

                case 3:
                    AudioManager.instance.PlayAudio("Trump - Wall", .6f, false);
                    break;
        }
            

        
    }
}
