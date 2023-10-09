using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int Level;
    public int Faction;
    public int MaxHealth;
    public int Health;
    public int MaxMana;
    public int Mana;
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int PhysicalResistance;
    public int FireResistance;
    public int ColdResistance;
    public int LightningResistance;
    public int ChaosResistance;
    public float MovementSpeed;
    public int HealthRegen;
    public int ManaRegen;
    public GameObject EquippedWeapon = null;
    public GameObject EquippedAbility = null;
    public GameObject EquippedMovementAbility = null;
    public Inventory Inventory;
    private float _healthRegenTimer;
    private float _manaRegenTimer;
    private Transform _propPos;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        FindPropR();
        WeaponCheck();
        AbilityCheck();
    }

    void FindPropR()
    {
        Transform[] arrTransform = transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in arrTransform)
        {
            if (t.name == "prop_R")
            {
                _propPos = t;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        Regen();
        WeaponCheck();
        AbilityCheck();
        SetHPToZeroIfNegative();
    }

    void SetHPToZeroIfNegative() 
    { 
        if (Health < 0)
        {
            Health = 0;
        }
    }

    void Regen()
    {
        if(Health <= 0)
        {
            return;
        }
        _healthRegenTimer += Time.deltaTime;
        _manaRegenTimer += Time.deltaTime;
        if (HealthRegen > 0)
        {
            if (Health < MaxHealth)
            {
                if (_healthRegenTimer >= 0.5)
                {
                    Health += HealthRegen;
                    _healthRegenTimer = 0;
                }
            }
        }
        if (ManaRegen > 0)
        {
            if (Mana < MaxMana)
            {
                if (_manaRegenTimer >= 0.5)
                {
                    Mana += ManaRegen;
                    _manaRegenTimer = 0;
                }
            }
        }
    }

    void WeaponCheck()
    {
        if (EquippedWeapon == null)
        {
            GameObject fist = Instantiate(Resources.Load<GameObject>("Weapons/Fist"), transform);
            EquippedWeapon = fist;
        }
        if (EquippedWeapon.transform.position != _propPos.transform.position)
        {
            EquippedWeapon.transform.position = _propPos.transform.position;
        }
    }

    void AbilityCheck()
    {
        if (EquippedAbility == null && EquippedWeapon != null)
        {
            GameObject ability = Instantiate(Resources.Load<GameObject>("Abilities/WeaponAttack"), transform);
            EquippedAbility = ability;
        }
    }
}
