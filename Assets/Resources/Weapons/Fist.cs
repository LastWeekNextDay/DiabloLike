using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fist : Weapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Name = "Fist";
        Texture = Resources.Load<Texture>("weapon_fist");
        AnimationState = "FistPunch";
        PhysicalDamage = 1;
        CastTime = 0.25f;
        Cooldown = 0.25f;
        Range = 1f;
        AoeRadius = 1f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
