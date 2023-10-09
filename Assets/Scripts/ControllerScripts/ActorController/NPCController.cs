using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class NPCController : GeneralController
{
    public GameObject Target;
    public bool WalkAround = false;
    private float _originalSpeed;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _originalSpeed = _navMeshAgent.speed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Activity();
    }

    void Activity()
    {
        if (_character.Health <= 0)
        {
            return;
        }
        PatrolAround();
        LookForEnemies();
    }

    void LookForEnemies()
    {
        if (CheckIfCanEngage() == false)
        {
            return;
        }
        if (Target != null)
        {
            if (Target.GetComponent<Character>().Health <= 0)
            {
                Target = null;
            } else {
                Engage();
            }
        } else
        {
            if (CreateAoeHitCheckerAndReturn(out List<Collider> colliders))
            {
                float closestDistance = 50f;
                foreach (Collider collider in colliders)
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        Target = collider.gameObject;
                    }
                }
            }
        } 
    }

    bool CheckIfCanEngage()
    {
        if (CanAttack == false || CanMove == false)
        {
            return false;
        }
        return true;
    }

    void Engage()
    {
        if (CheckIfCanEngage() == false)
        {
            return;
        }
        if (Target != null)
        {
            float distance = Vector3.Distance(transform.position, Target.transform.position);
            if (distance > _character.EquippedAbility.GetComponent<Ability>().Range)
            {
                Move(Target.transform.position);
            }
            if (distance <= _character.EquippedAbility.GetComponent<Ability>().Range)
            {
                Attack(Target.transform.position);
            }
        }
    }

    void PatrolAround()
    {
        if ((Target != null) || (WalkAround == false))
        {
            _navMeshAgent.speed = _originalSpeed;
            return;
        }
        if (_navMeshAgent.speed > 2)
        {
            _navMeshAgent.speed = 2;
        }
        
        if (Vector3.Distance(transform.position, _navMeshAgent.destination) <= 10f)
        {
            CreateRandomDestination();
        } else
        {
            return;
        }
    }

    private void CreateRandomDestination()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * 20f;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
        {
            Move(hit.position);
        }
    }

    bool CreateAoeHitCheckerAndReturn(out List<Collider> colliders)
    {
        colliders = new List<Collider>();
        Vector3 pos = _character.gameObject.transform.position;
        pos.y += _character.gameObject.GetComponent<BoxCollider>().size.y/2f;
        Collider[] hitColliders = Physics.OverlapSphere(pos, 10f);
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                Character character = collider.GetComponent<Character>();
                if (character != null)
                {
                    if (character.Faction != _character.Faction)
                    {
                        if (character.Health > 0)
                        {
                            colliders.Add(collider);
                        } 
                    }
                }
            }
        }
        if (colliders.Count > 0)
        {
            return true;
        } else
        {
            colliders = null;
            return false;
        }
    }
}
