using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AtSafe : Node
{
    //Nav mesh agent's location
    NavMeshAgent m_NavMeshAgent;

    //Safe's location
    Vector3 m_SafePosition;

    public AtSafe(NavMeshAgent navMeshAgent, Vector3 safePosition)
    {
        m_NavMeshAgent = navMeshAgent;

        m_SafePosition = safePosition;
    }


    public override NodeState Execute()
    {
        if (m_NavMeshAgent != null && m_SafePosition != null)
        {

            float distanceFromSafe = Vector3.Distance(m_NavMeshAgent.transform.position, m_SafePosition);
            if (distanceFromSafe >= 3f)
            {
                return NodeState.FAILURE;
            }
            else
            {
                Debug.Log("Distance from safe: " + distanceFromSafe);
                m_NavMeshAgent.SetDestination(m_NavMeshAgent.transform.position);
                return NodeState.SUCCESS;
            }

        }
        else
        {
            Debug.LogError("Navmeshposition or safe position is null");
            return NodeState.FAILURE;
        }
    }
}
