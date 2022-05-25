using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToCoverNode : Node
{
 
    NavMeshAgent m_NavMeshAgent;
    AIAgent m_Agent;

    public GoToCoverNode (NavMeshAgent navMeshAgent, AIAgent agent)
    {
        m_NavMeshAgent = navMeshAgent;
        m_Agent = agent;
    }


    public override NodeState Execute()
    {
        Transform bestCover = m_Agent.GetBestCoverSpot();
        //Debug.Log("GotoCover - Best cover: " + bestCover.position);
        m_Agent.Material.color = Color.green;
        if (bestCover == null)
        {
            Debug.Log("GotoCover: BestCover is null");
            return NodeState.FAILURE;
        }

        float distanceFromTarget = Vector3.Distance(bestCover.position,m_NavMeshAgent.transform.position);
        if (distanceFromTarget >= 0.5f)
        {
            m_NavMeshAgent.isStopped = false;
            Debug.Log("Moving to cover! " + distanceFromTarget);
            m_NavMeshAgent.SetDestination(bestCover.position);
            return NodeState.RUNNING;
        }
        else
        {
            m_NavMeshAgent.isStopped = true;
            Debug.Log("I am covered");
            return NodeState.SUCCESS;
        }
    }
}
