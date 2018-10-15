using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPope : Ability
{

    float reviveRadius;
    float reviveHP;
    public override void Init()
    {
        base.Init();

        // Stats
        cooldown = 0f;
        reviveRadius = 10f;
        reviveHP = 50f;
    }

    protected override void Use()
    {
        AudioManager.instance.PlayAudio("Pope_peace", 1, false);
        base.Use();
        Revive();
    }

    private void Revive()
    {
        Collider[] hits = Physics.OverlapSphere(playerActor.transform.position, reviveRadius );
        foreach (Collider hit in hits)
        {
            if (hit.GetComponent<PlayerActor>() !=null)
            {
                hit.GetComponent<PlayerActor>().HealPlayer(reviveHP);
            }
        }
    }
}
