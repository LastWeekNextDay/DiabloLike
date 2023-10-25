using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationController : MonoBehaviour
{
    private AnimationStateInfos _animationStateInfos;
    // Start is called before the first frame update
    void Start()
    {
        _animationStateInfos = GetComponent<AnimationStateInfos>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCastAnimation(Ability ability, GameObject initiator)
    {
        if (string.IsNullOrEmpty(ability.AnimationState)) return;
        Animator animator = initiator.GetComponent<Animator>();
        animator.Play(ability.AnimationState);
        float animationLength = _animationStateInfos.AnimationSpeed(ability.AnimationState);
        Debug.Log(animationLength);
        animator.SetFloat("CastMult", (float)(animationLength / ability.CastTime));
    }

    public void MovementAnimationHandling(GameObject initiator)
    {
        Animator animator = initiator.GetComponent<Animator>();
        NavMeshAgent navMeshAgent = initiator.GetComponent<NavMeshAgent>();
        GeneralController generalController = initiator.GetComponent<GeneralController>();
        if (!generalController.NavAgentExists()) return;
        if (generalController.Casting) navMeshAgent.velocity = Vector3.zero;
        float velocityMagnitude = navMeshAgent.velocity.magnitude;
        float speedMultiplier = velocityMagnitude > 3f ? velocityMagnitude / 5 : (velocityMagnitude > 0.5f ? velocityMagnitude / 2 : 1);
        animator.SetFloat("Speed", velocityMagnitude);
        animator.SetFloat("SpeedMult", speedMultiplier);
    }

    public void PlayDeathAnimation(GameObject initiator)
    {
        initiator.GetComponent<Animator>().SetTrigger("Death");
    }
}
