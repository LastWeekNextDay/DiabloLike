using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : Ability
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Texture = Resources.Load<Texture>("Icons/weapon_fist");
        Name = "Weapon Attack";
        ManaCost = 0;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        UpdateWeaponStats();
    }

    

    void UpdateWeaponStats()
    {
        if (assignedCharacter.EquippedWeapon != null)
        {
            RecalculateDamages();
            FixWrongs();
        }
    }

    void RecalculateDamages()
    {
        PhysicalDamage = assignedCharacter.EquippedWeapon.PhysicalDamage + ((float)assignedCharacter.Strength / 10f);
        FireDamage = assignedCharacter.EquippedWeapon.FireDamage + ((float)assignedCharacter.Intelligence / 30f);
        ColdDamage = assignedCharacter.EquippedWeapon.ColdDamage + ((float)assignedCharacter.Intelligence / 30f);
        LightningDamage = assignedCharacter.EquippedWeapon.LightningDamage + ((float)assignedCharacter.Intelligence / 30f);
        ChaosDamage = assignedCharacter.EquippedWeapon.ChaosDamage + ((float)assignedCharacter.Intelligence / 40f) + (assignedCharacter.Dexterity / 30f);
    }

    void FixWrongs() { 
        if (assignedCharacter.EquippedWeapon.CastTime != CastTime)
        {
            CastTime = assignedCharacter.EquippedWeapon.CastTime;
        }
        if (assignedCharacter.EquippedWeapon.Cooldown != Cooldown)
        {
            Cooldown = assignedCharacter.EquippedWeapon.Cooldown;
        }
        if (assignedCharacter.EquippedWeapon.Range != Range)
        {
            Range = assignedCharacter.EquippedWeapon.Range;
        }
        if (assignedCharacter.EquippedWeapon.AoeRadius != AoeRadius)
        {
            AoeRadius = assignedCharacter.EquippedWeapon.AoeRadius;
        }
        if (assignedCharacter.EquippedWeapon.AnimationState != AnimationState)
        {
            AnimationState = assignedCharacter.EquippedWeapon.AnimationState;
        }
    }

    public override void Cast()
    {
        if(CreateHitCheckerAndReturnHit(out List<Collider> colliders))
        {
            foreach (Collider collider in colliders)
            {
                Character character = collider.GetComponent<Character>();
                if (character != null)
                {
                    DealDamage(character);
                }
            }
            return;
        }
    }

    protected override void DealDamage(Character character)
    {
        float damage = CalculatePhysicalDamage(PhysicalDamage, character.PhysicalResistance) + 
                        CalculateFireDamage(FireDamage, character.FireResistance) +
                        CalculateColdDamage(ColdDamage, character.ColdResistance) +
                        CalculateLightningDamage(LightningDamage, character.LightningResistance) +
                        CalculateChaosDamage(ChaosDamage, character.ChaosResistance);
        character.Health -= (int)damage;
        _uiController.ShowDamageNumber((int)damage, character.gameObject, assignedCharacter);
    }

    private float CalculatePhysicalDamage(float physicalDamage, int physicalResistance)
    {
        return physicalDamage - physicalDamage * ((float)physicalResistance / 100);
    }

    private float CalculateFireDamage(float fireDamage, int fireResistance)
    {
        return fireDamage - fireDamage * ((float)fireResistance / 100);
    }

    private float CalculateColdDamage(float coldDamage, int coldResistance)
    {
        return coldDamage - coldDamage * ((float)coldResistance / 100);
    }

    private float CalculateLightningDamage(float lightningDamage, int lightningResistance)
    {
        return lightningDamage - lightningDamage * ((float)lightningResistance / 100);
    }

    private float CalculateChaosDamage(float chaosDamage, int chaosResistance)
    {
        return chaosDamage - chaosDamage * ((float)chaosResistance / 100);
    }

    bool CreateHitCheckerAndReturnHit(out List<Collider> colliders)
    {
        colliders = new List<Collider>();
        Vector3 pos = assignedCharacter.gameObject.transform.position;
        pos.y += assignedCharacter.gameObject.GetComponent<BoxCollider>().size.y / 2;
        pos += assignedCharacter.gameObject.transform.forward * Range;
        Collider[] collidersHit = Physics.OverlapSphere(pos, AoeRadius*2);
        if (collidersHit.Length > 0)
        {
            Collider[] relevantColliders = Array.FindAll(collidersHit, collider => collider.gameObject.GetComponent<Character>() != null);
            if (relevantColliders.Length > 0)
            {
                for (int i = 0; i < relevantColliders.Length; i++)
                {
                    if (relevantColliders[i].gameObject.GetComponent<Character>().Faction != assignedCharacter.Faction)
                    {
                        if (relevantColliders[i].gameObject.GetComponent<Character>().Health > 0)
                        {
                            colliders.Add(relevantColliders[i]);
                        }  
                    }                   
                }
                if (colliders.Count > 0)
                {
                    return true;
                }
            }
        }
        colliders = null;
        return false;
    }
}
