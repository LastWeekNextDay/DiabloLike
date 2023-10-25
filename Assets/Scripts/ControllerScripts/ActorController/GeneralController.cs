using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeneralController : MonoBehaviour
{
    protected AnimationController _animationController;
    protected Character _character;
    protected NavMeshAgent _navMeshAgent;
    public bool Casting = false;
    public bool CanMove = true;
    public bool CanAttack = true;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _animationController = GameObject.Find("AnimationController").GetComponent<AnimationController>();
        _character = GetComponent<Character>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckIfDead();
        _animationController.MovementAnimationHandling(gameObject);
    }

    public void Move(Vector3 point)
    {
        if (Busy()) return;
        _navMeshAgent.SetDestination(point);
        _navMeshAgent.speed = _character.MovementSpeed;
    }

    public void Cast(Vector3 target, Ability ability)
    {
        if (Busy() || _character.Mana < ability.ManaCost || ability.IsOnCooldown) return;
        StopCharacter();
        transform.LookAt(target);
        InititateAbilityCast(target, ability);
    }

    bool Busy()
    {
        return Casting || !NavAgentExists();
    }

    void InititateAbilityCast(Vector3 target, Ability ability)
    {
        Casting = true;
        _character.Mana -= ability.ManaCost;
        ability.Cast(target);
        _animationController.PlayCastAnimation(ability, gameObject);
        ability.StartCooldown();
        StartCoroutine(AbilityCastTime(ability.CastTime));
    }

    IEnumerator AbilityCastTime(float time)
    {
        yield return new WaitForSeconds(time);
        Casting = false;
    }

    bool StopCharacter()
    {
        if (!NavAgentExists()) return false;
        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();
        return _navMeshAgent.velocity == Vector3.zero;
    }

    void CheckIfDead()
    {
        if (_character.Health <= 0) Die();
    }

    void Die()
    {
        _animationController.PlayDeathAnimation(gameObject);
        CanMove = CanAttack = false;
        Destroy(_navMeshAgent);
    }

    public bool NavAgentExists()
    {
        return _navMeshAgent != null;
    }
}