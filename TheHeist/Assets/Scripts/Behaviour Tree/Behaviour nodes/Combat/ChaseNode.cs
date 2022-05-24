using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : Node
{
    Transform m_Target;
    NavMeshAgent m_NavMeshAgent;
    AIAgent m_Agent;

    public ChaseNode(Transform target,NavMeshAgent navMeshAgent,AIAgent agent)
    {
        m_Target = target;
        m_NavMeshAgent = navMeshAgent;
        m_Agent = agent;
    }


    public override NodeState Execute()
    {
            float distanceFromTarget = Vector3.Distance(m_Target.position,m_NavMeshAgent.transform.position);
            //Debug.Log("DistanceFromTarget: " + distanceFromTarget);
            m_Agent.Material.color = Color.yellow;
            if (distanceFromTarget >= 2f)
            {
                m_NavMeshAgent.isStopped = false;
                m_NavMeshAgent.SetDestination(m_Target.position);
                return NodeState.RUNNING;
            }
            else
            {
                m_NavMeshAgent.isStopped = true;
                return NodeState.SUCCESS;
            }
        
    }
}
