using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Ability : MonoBehaviour
{   
    public Texture Texture;
    public string Name;
    public float PhysicalDamage;
    public float FireDamage;
    public float ColdDamage;
    public float LightningDamage;
    public float ChaosDamage;
    public float CastTime;
    public float Cooldown;
    public float Range;
    public float AoeRadius;
    public int ManaCost;
    public string AnimationState;
    public bool IsOnCooldown = false;
    public float CooldownTimer;
    protected UIController _uiController;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _uiController = GameObject.Find("UIController").GetComponent<UIController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        EvaluateCooldown();
    }

    public virtual void Cast(Vector3 target)
    {
        
    }

    void EvaluateCooldown()
    {
        if (IsOnCooldown)
        {
            CooldownTimer -= Time.deltaTime;
            if (CooldownTimer <= 0)
            {
                IsOnCooldown = false;
                CooldownTimer = 0;
            }
        }
    }

    public void StartCooldown()
    {
        IsOnCooldown = true;
        CooldownTimer = Cooldown;
    }

    protected virtual void DealDamage(Character receiver)
    {
        float damage = CalculatePhysicalDamage(PhysicalDamage, receiver.PhysicalResistance) +
                        CalculateFireDamage(FireDamage, receiver.FireResistance) +
                        CalculateColdDamage(ColdDamage, receiver.ColdResistance) +
                        CalculateLightningDamage(LightningDamage, receiver.LightningResistance) +
                        CalculateChaosDamage(ChaosDamage, receiver.ChaosResistance);
        receiver.Health -= (int)damage;
        _uiController.ShowDamageNumber((int)damage, receiver.gameObject, transform.parent.gameObject);
    }

    protected float CalculatePhysicalDamage(float physicalDamage, int physicalResistance)
    {
        return physicalDamage - physicalDamage * ((float)physicalResistance / 100);
    }

    protected float CalculateFireDamage(float fireDamage, int fireResistance)
    {
        return fireDamage - fireDamage * ((float)fireResistance / 100);
    }

    protected float CalculateColdDamage(float coldDamage, int coldResistance)
    {
        return coldDamage - coldDamage * ((float)coldResistance / 100);
    }

    protected float CalculateLightningDamage(float lightningDamage, int lightningResistance)
    {
        return lightningDamage - lightningDamage * ((float)lightningResistance / 100);
    }

    protected float CalculateChaosDamage(float chaosDamage, int chaosResistance)
    {
        return chaosDamage - chaosDamage * ((float)chaosResistance / 100);
    }
}
