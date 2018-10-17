using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPope : Ability
{

    float reviveRadius;
    float reviveHP;
    float healHP;
    public override void Init()
    {
        base.Init();

        // Stats
        cooldown = 0f;
        reviveRadius = 10f;
        reviveHP = 50f;
        healHP = 10;
    }

    protected override void Use()
    {
        base.Use();
        Revive();
    }

    Collider[] colliders;
    private void Revive()
    {
        int layerId = LayerMask.NameToLayer("Player");
        int layerMask = 1 << layerId;
        colliders = Physics.OverlapSphere(playerActor.transform.position, reviveRadius, layerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerActor player = colliders[i].GetComponent<PlayerActor>();

            // Is this collider a player
            if (player != null && player.model.playerActive == false)
            {
                player.HealPlayer(reviveHP);
            }
            if (player != null && player.model.playerActive == true)
            {
                player.HealPlayer(healHP);
            }

        }
        AudioManager.instance.PlayAudio("Pope_peace", 1, false);
    }
}
