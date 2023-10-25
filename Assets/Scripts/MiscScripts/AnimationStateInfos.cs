using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class AnimationStateInfos : MonoBehaviour
{
    private float _walk = 1.067f;
    private float _run = 0.667f;
    private float _idle = 1.733f;
    private float _death = 2.433f;
    private float _fistPunch = 0.967f;
    private float _oneHandedSwing = 0.867f;
    private float _twoHandedSwing = 1.933f;
    private float _cast1 = 0.2f;
    private float _cast2 = 2.133f;
    private float _cast3 = 0.933f;

    public float AnimationSpeed(string animationState)
    {
        switch (animationState)
        {
            case "Walk":
                return _walk;
            case "Run":
                return _run;
            case "Idle":
                return _idle;
            case "Death":
                return _death;
            case "FistPunch":
                return _fistPunch;
            case "OneHandedSwing":
                return _oneHandedSwing;
            case "TwoHandedSwing":
                return _twoHandedSwing;
            case "Cast1":
                return _cast1;
            case "Cast2":
                return _cast2;
            case "Cast3":
                return _cast3;
            default:
                return 1;
        }
    }
}
