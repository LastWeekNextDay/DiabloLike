using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public float PhysicalDamage;
    public float FireDamage;
    public float ColdDamage;
    public float LightningDamage;
    public float ChaosDamage;
    public string AnimationState;
    public float CastTime;
    public float Cooldown;
    public float Range;
    public float AoeRadius;
    public bool IsEquipped;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Type = "Weapon";
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }


}
