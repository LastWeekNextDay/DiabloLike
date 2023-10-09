using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Ability
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Cooldown = 5f;
        CastTime = 0.1f;
        Range = 20f;
        ManaCost = 10;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Cast(Vector3 target)
    {
        base.Cast(target);
        var distance = Vector3.Distance(transform.position, target);
        if (distance > Range)
        {
            target = transform.position + (target - transform.position).normalized * Range;
        }
        transform.parent.position = target;
    }
}
