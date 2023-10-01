using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool isPlayer;
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
    public Weapon EquippedWeapon = null;
    public Ability EquippedAbility = null;
    public Ability EquippedMovementAbility = null;
    private float _healthRegenTimer;
    private float _manaRegenTimer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        WeaponInit();
        AbilityInit();
    }



    // Update is called once per frame
    void Update()
    {
        Regen();
        WeaponInit();
        AbilityInit();
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

    void WeaponInit()
    {
        if (EquippedWeapon == null)
        {
            GameObject fist = Instantiate(Resources.Load<GameObject>("Weapons/Fist"), transform);
            EquippedWeapon = fist.GetComponent<Fist>();
        }
    }

    void AbilityInit()
    {
        if (EquippedAbility == null && EquippedWeapon != null)
        {
            GameObject ability = Instantiate(Resources.Load<GameObject>("WeaponAttack"), transform);
            EquippedAbility = ability.GetComponent<WeaponAttack>();
            EquippedAbility.assignedCharacter = this;
        }
    }
}
