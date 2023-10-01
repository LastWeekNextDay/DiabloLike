using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class CharacterController : GeneralController
{
    public GameObject Target;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
            if (distance > _character.EquippedAbility.Range)
            {
                Move(Target.transform.position);
            }
            if (distance <= _character.EquippedAbility.Range)
            {
                Attack(Target.transform.position);
            }
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
