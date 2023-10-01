using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Axe : Weapon
{
    protected override void Start()
    {
        base.Start();
        Name = "Axe";
        Texture = Resources.Load<Texture>("weapon_axe");
        AnimationState = "OneHandedSwing";
        PhysicalDamage = 15;
        CastTime = 0.35f;
        Cooldown = 0.35f;
        Range = 1.5f;
        AoeRadius = 1.5f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
