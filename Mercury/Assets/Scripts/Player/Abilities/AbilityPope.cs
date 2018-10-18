using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPope : Ability
{

    float abilityRadius;
    float reviveHP;
    float healHP;
    float reviveCount;
    public override void Init()
    {
        base.Init();

        // Stats
        cooldown = 10f;
        abilityRadius = 20f;
        healHP = 20;
        reviveHP = 20;
        reviveCount = 1;
    }

    protected override void Use()
    {
        base.Use();
        Init();
        Revive();
    }

    Collider[] colliders;
    private void Revive()
    {
        int layerId = LayerMask.NameToLayer("Player");
        int layerMask = 1 << layerId;
        colliders = Physics.OverlapSphere(playerActor.transform.position, abilityRadius);

        foreach (Collider hit in colliders)
        {
            PlayerActor playerHit = hit.GetComponent<PlayerActor>();
            //Heal players that is not downed
            if (playerHit != null && playerHit.model.playerActive == true)
            {
                playerHit.HealPlayer(healHP);
            }
            //Revive players that is downed
            if (playerHit != null && playerHit.model.playerActive == false)
            {
                if (reviveCount == 0)
                {
                    reviveCount++;
                    playerHit.HealPlayer(reviveHP);
                }  
                if (reviveCount > 0)
                    playerHit.HealPlayer(reviveHP);
                
            }
        }
        AudioManager.instance.PlayAudio("Pope_peace", 1, false);
    }
}
