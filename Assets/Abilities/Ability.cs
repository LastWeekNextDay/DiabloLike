using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool IsOnCooldown;
    public float CooldownTimer;
    protected UIController _uiController;
    public Character assignedCharacter;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _uiController = GameObject.Find("UIController").GetComponent<UIController>();
        InitParent();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        EvaluateCooldown();
    }

    public virtual void Cast()
    {
        
    }

    void InitParent()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        assignedCharacter = parent.GetComponent<Character>();
    }

    void EvaluateCooldown()
    {
        if (IsOnCooldown)
        {
            CooldownTimer -= Time.deltaTime;
            if (CooldownTimer <= 0)
            {
                IsOnCooldown = false;
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
        
    }
}
