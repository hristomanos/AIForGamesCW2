using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToTarget : Node
{
    //Nav mesh agent's location
    NavMeshAgent m_NavMeshAgent;

    //Safe's location
    Vector3 m_TargetPosition;

    public GoToTarget(NavMeshAgent navMeshAgent, Vector3 TargetPosition)
    {
        m_NavMeshAgent = navMeshAgent;

        m_TargetPosition = TargetPosition;
    }


    public override NodeState Execute()
    {
        //Check the distance between you and the safe
        float distanceFromTarget = Vector3.Distance(m_TargetPosition, m_NavMeshAgent.transform.position);
        m_NavMeshAgent.GetComponent<MeshRenderer>().material.color = Color.red;
        //If it is greater than (amount) then set its destination
        if (distanceFromTarget > 3f)
        {
            //Debug.Log(distanceFromTarget);
            m_NavMeshAgent.isStopped = false;
            m_NavMeshAgent.SetDestination(m_TargetPosition);
            return NodeState.FAILURE;
        }
        //Otherwise, you are close to the target
        else
        {
            m_NavMeshAgent.isStopped = true;
            return NodeState.SUCCESS;
        }
    }
}
