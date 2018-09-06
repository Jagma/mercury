using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class MartianBoss : Enemy
{
    private Timer timer;
    private PlayerActor closestPlayerActor = null;
    int i = 0;
    protected override void Start()
    {
        base.Start();

        health = 1000;
        moveSpeed = 15f;

        timer = new Timer();
        timer.Interval = 2000; //2 seconds
        timer.Enabled = true;
        timer.Elapsed += new ElapsedEventHandler(AttackDelay);
    }

    private void AttackDelay(object source, ElapsedEventArgs e)
    {
        timer.Stop();  
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Collider[] colliders = Physics.OverlapSphere(transform.position, 7.5f);

        PlayerActor closestPlayerActor = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerActor playerActor = colliders[i].GetComponent<PlayerActor>();

            // Is this collider a player
            if (playerActor != null)
            {
                if (closestPlayerActor == null)
                {
                    closestPlayerActor = playerActor;
                }
                // Is this player closer than the current closest player
                if (Vector3.Distance(playerActor.transform.position, transform.position) <
                    Vector3.Distance(closestPlayerActor.transform.position, transform.position))
                {
                    closestPlayerActor = playerActor;
                }
            }

        }
        //going on 60FPS meaning i==120 means 2sec
        // If we found a player move towards it
        if (closestPlayerActor != null)
        {
            if (i == 120)
            {
                base.FaceDirection((closestPlayerActor.transform.position - transform.position).normalized);
                i = -1;
            }
            i++;
            timer.Start();
            MoveForward();
        }
    }
}
