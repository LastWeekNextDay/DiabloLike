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

    public void Attack(Vector3 direction)
    {
        if (CheckIfBusy())
        {
            return;
        }
        if (_character.Mana >= _character.EquippedAbility.ManaCost)
        {
            if (_character.EquippedAbility.IsOnCooldown == false)
            {
                StopCharacterAndLook(direction);
                PlayAttackAnimation();
                InititateAbilityCast();
            }
        }
    }

    void InititateAbilityCast()
    {
        _isAttacking = true;
        _character.Mana -= _character.EquippedAbility.ManaCost;
        _character.EquippedAbility.Cast();
        _character.EquippedAbility.StartCooldown();
        StartCoroutine(AttackCastTime());
    }

    void StopCharacterAndLook(Vector3 direction)
    {
        _navMeshAgent.speed = 0;
        transform.LookAt(direction);
    }

    void PlayAttackAnimation()
    {
        _animator.Play(_character.EquippedAbility.AnimationState);
        _animator.speed = (float)(_animator.GetCurrentAnimatorStateInfo(0).length / _character.EquippedAbility.CastTime);
    }

    IEnumerator AttackCastTime()
    {
        yield return new WaitForSeconds(_character.EquippedAbility.CastTime);
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
