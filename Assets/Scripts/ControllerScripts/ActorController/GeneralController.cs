using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeneralController : MonoBehaviour
{
    protected Animator _animator;
    protected Character _character;
    protected NavMeshAgent _navMeshAgent;
    protected bool _isAttacking = false;
    public bool CanMove = true;
    public bool CanAttack = true;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
        _character = GetComponent<Character>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckIfDead();
        MovementAnimationHandling();
    }

    public Character Character
    {
        get { return _character; }
        set { _character = value; }
    }

    public void Move(Vector3 point)
    {
        if (CheckIfBusy())
        {
            return;
        }
        _navMeshAgent.SetDestination(point);
        _navMeshAgent.speed = _character.MovementSpeed;
    }

    private bool CheckIfBusy()
    {
        if (_isAttacking)
        {
            return true;
        }
        if (_navMeshAgent == null)
        {
            return true;
        }
        return false;
    }

    public void Attack(Vector3 target)
    {
        if (CheckIfBusy())
        {
            return;
        }
        if (_character.Mana >= _character.EquippedAbility.GetComponent<Ability>().ManaCost)
        {
            if (_character.EquippedAbility.GetComponent<Ability>().IsOnCooldown == false)
            {
                StopCharacterAndLook(target);
                PlayAttackAnimation();
                InititateAbilityCast(target);
            }
        }
    }

    public void MovementAbility(Vector3 target)
    {
        if (CheckIfBusy())
        {
            return;
        }
        if (_character.Mana >= _character.EquippedMovementAbility.GetComponent<Ability>().ManaCost)
        {
            if (_character.EquippedMovementAbility.GetComponent<Ability>().IsOnCooldown == false)
            {
                StopCharacterAndLook(target);
                InititateMovementAbilityCast(target);
            }
        }
    }

    private void InititateMovementAbilityCast(Vector3 target)
    {
        _isAttacking = true;
        _character.Mana -= _character.EquippedMovementAbility.GetComponent<Ability>().ManaCost;
        _character.EquippedMovementAbility.GetComponent<Ability>().Cast(target);
        _character.EquippedMovementAbility.GetComponent<Ability>().StartCooldown();
        StartCoroutine(MovementAbilityCastTime());
    }

    void InititateAbilityCast(Vector3 target)
    {
        _isAttacking = true;
        _character.Mana -= _character.EquippedAbility.GetComponent<Ability>().ManaCost;
        _character.EquippedAbility.GetComponent<Ability>().Cast(target);
        _character.EquippedAbility.GetComponent<Ability>().StartCooldown();
        StartCoroutine(AttackCastTime());
    }

    void StopCharacterAndLook(Vector3 target)
    {
        _navMeshAgent.speed = 0;
        transform.LookAt(target);
    }

    void PlayAttackAnimation()
    {
        if (_character.EquippedAbility.GetComponent<Ability>().AnimationState == "")
        {
            return;
        }
        _animator.Play(_character.EquippedAbility.GetComponent<Ability>().AnimationState);
        _animator.speed = (float)(_animator.GetCurrentAnimatorStateInfo(0).length / _character.EquippedAbility.GetComponent<Ability>().CastTime);
    }

    IEnumerator AttackCastTime()
    {
        yield return new WaitForSeconds(_character.EquippedAbility.GetComponent<Ability>().CastTime);
        _isAttacking = false;
        _animator.Play("Idle");
        _animator.speed = 1;
    }

    IEnumerator MovementAbilityCastTime()
    {
        yield return new WaitForSeconds(_character.EquippedMovementAbility.GetComponent<Ability>().CastTime);
        _isAttacking = false;
        _animator.Play("Idle");
        _animator.speed = 1;
    }

    void CheckIfDead()
    {
        if (_character.Health <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        _animator.Play("Death");
        Destroy(_navMeshAgent);
    }

    void MovementAnimationHandling()
    {
        if (_navMeshAgent == null)
        {
            return;
        }
        if (_isAttacking)
        { 
            _animator.SetBool("isMoving", false);
            if (_navMeshAgent.velocity.magnitude > 0)
            {
                _navMeshAgent.velocity = Vector3.zero;
            }
            return;
        }
        if (_navMeshAgent.velocity.magnitude > 0.5f)
        {
            if (_animator.GetBool("isMoving") == false)
            {
                _animator.SetBool("isMoving", true);
            }
            _animator.speed = _navMeshAgent.velocity.magnitude / 5;
            return;
        }
        else
        {
            _animator.SetBool("isMoving", false);
            _animator.speed = 1;
        }
    }
}
