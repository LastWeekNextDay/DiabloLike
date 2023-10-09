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
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        UpdateWeaponStats();
    }

    void UpdateWeaponStats()
    {
        if (GetComponentInParent<Character>().EquippedWeapon != null)
        {
            RecalculateDamages();
            FixWrongs();
        }
    }

    void RecalculateDamages()
    {
        PhysicalDamage = GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().PhysicalDamage + ((float)GetComponentInParent<Character>().Strength / 10f);
        FireDamage = GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().FireDamage + ((float)GetComponentInParent<Character>().Intelligence / 30f);
        ColdDamage = GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().ColdDamage + ((float)GetComponentInParent<Character>().Intelligence / 30f);
        LightningDamage = GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().LightningDamage + ((float)GetComponentInParent<Character>().Intelligence / 30f);
        ChaosDamage = GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().ChaosDamage + ((float)GetComponentInParent<Character>().Intelligence / 40f) + (GetComponentInParent<Character>().Dexterity / 30f);
    }

    void FixWrongs() { 
        if (GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().CastTime != CastTime)
        {
            CastTime = GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().CastTime;
        }
        if (GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().Cooldown != Cooldown)
        {
            Cooldown = GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().Cooldown;
        }
        if (GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().Range != Range)
        {
            Range = GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().Range;
        }
        if (GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().AoeRadius != AoeRadius)
        {
            AoeRadius = GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().AoeRadius;
        }
        if (GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().AnimationState != AnimationState)
        {
            AnimationState = GetComponentInParent<Character>().EquippedWeapon.GetComponent<Weapon>().AnimationState;
        }
    }

    public override void Cast(Vector3 target)
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
    
    bool CreateHitCheckerAndReturnHit(out List<Collider> colliders)
    {
        colliders = new List<Collider>();
        Vector3 pos = GetComponentInParent<Character>().gameObject.transform.position;
        pos.y += GetComponentInParent<Character>().gameObject.GetComponent<BoxCollider>().size.y / 2;
        pos += GetComponentInParent<Character>().gameObject.transform.forward * Range;
        Collider[] collidersHit = Physics.OverlapSphere(pos, AoeRadius*2);
        if (collidersHit.Length > 0)
        {
            Collider[] relevantColliders = Array.FindAll(collidersHit, collider => collider.gameObject.GetComponent<Character>() != null);
            if (relevantColliders.Length > 0)
            {
                for (int i = 0; i < relevantColliders.Length; i++)
                {
                    if (relevantColliders[i].gameObject.GetComponent<Character>().Faction != GetComponentInParent<Character>().Faction)
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
