using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarrior : Character
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Level = 1;
        MaxHealth = 100;
        Health = MaxHealth;
        MaxMana = 0;
        Mana = MaxMana;
        Strength = 10;
        Dexterity = 10;
        Intelligence = 10;
        MovementSpeed = 3.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
