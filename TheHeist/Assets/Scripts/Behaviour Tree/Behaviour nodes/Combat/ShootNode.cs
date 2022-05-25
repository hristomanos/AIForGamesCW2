using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootNode : Node
{
    AIAgent m_Agent;
    NavMeshAgent m_NavMeshAgent;
    Transform m_Target;

    float m_FireRate = 1;

    public ShootNode(AIAgent agent, NavMeshAgent navMeshAgent, Transform target)
    {
        m_NavMeshAgent = navMeshAgent;
        m_Agent = agent;
        m_Target = target;
    }


    public override NodeState Execute()
    {
        m_Agent.transform.LookAt(m_Target); //Only if I can see him
        if (TargetIsVisible(m_Target))
        {

            m_NavMeshAgent.isStopped = true;
            m_Agent.Material.color = Color.red;
                                                // Debug.Log("Shooting");
            Shoot();
            return NodeState.SUCCESS;
        }
        else
            return NodeState.FAILURE;

    }

    void Shoot()
    {
        m_FireRate -= Time.deltaTime;
        if (m_FireRate <= 0)
        {
            m_Agent.SpawnBullet();
            m_FireRate = 1;
        }
    }


    private bool TargetIsVisible(Transform target)
    {
        RaycastHit hit;
        Vector3 direction = m_Target.position - m_Agent.transform.position;

        if (Physics.Raycast(m_Agent.transform.position, direction, out hit))
        {
            if (hit.collider.transform == m_Target)
            {
                return true;
            }
        }

        return false;

    }

}
