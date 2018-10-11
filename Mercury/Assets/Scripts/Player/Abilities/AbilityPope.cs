using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPope : Ability
{

    float reviveRadius;
    //double reviveHP;
    public override void Init()
    {
        base.Init();

        // Stats
        cooldown = 0f;
        reviveRadius = 10f;
        //reviveHP = 50f;
    }

    protected override void Use()
    {
        base.Use();
        Revive();
    }

    private void Revive()
    {
        Collider[] hits = Physics.OverlapSphere(playerActor.transform.position, reviveRadius );
        foreach (Collider hit in hits)
        {
            if (hit.gameObject.name.Equals("Player") && playerActor.model.playerID != hit.GetComponent<PlayerModel>().playerID)
            {
                hit.GetComponent<PlayerActor>().Revive(50);
            }

        }
    }
}
