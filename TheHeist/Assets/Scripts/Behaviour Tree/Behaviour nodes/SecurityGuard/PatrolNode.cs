using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNode : Node
{
    NavMeshAgent m_NavMeshAgent;
    AIAgent m_Agent;

   

    //The total time we wait at each node.
    [SerializeField] float m_TotalWaitTime = 3.0f;

    
    ConnectedWaypoint m_PreviousWaypoint;
    ConnectedWaypoint m_CurrentWaypoint;

   
    bool m_Travelling;
    bool m_Waiting;
   
    float m_WaitTimer;

    ConnectedWaypoint m_StartingWaypoint;
    
    public PatrolNode(NavMeshAgent navMeshAgent,AIAgent agent)
    {
        m_NavMeshAgent = navMeshAgent;
        m_Agent = agent;

        //If you don't find the nav mesh agent, then log an error
        if (m_NavMeshAgent == null)
        {
            Debug.LogError("Nav mesh agent is not attached to " + m_Agent.gameObject.name);
        }
        else
        {
            if (m_CurrentWaypoint == null)
            {
                GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

                if (allWaypoints.Length > 0)
                {

                    while (m_CurrentWaypoint == null)
                    {
                        int randomWaypoint = Random.Range(0, allWaypoints.Length);
                        m_StartingWaypoint = allWaypoints[randomWaypoint].GetComponent<ConnectedWaypoint>();

                        if (m_StartingWaypoint != null)
                        {
                            m_CurrentWaypoint = m_StartingWaypoint;

                        }

                    }
                }
                else
                {
                    Debug.LogError("Failed to find any waypoints for use in the scene");
                }
            }

            SetDestination();
        }

    }


    public override NodeState Execute()
    {
        // Debug.Log("In patrol: " + m_NavMeshAgent.pathStatus);


        if (m_Travelling && m_NavMeshAgent.isStopped == true)
        {
            //m_NavMeshAgent.isStopped = true;
            Debug.Log("Path reset");
            m_NavMeshAgent.ResetPath();

            if (m_StartingWaypoint != null)
            {
                m_CurrentWaypoint = m_StartingWaypoint;

            }

            return NodeState.SUCCESS;
        }

        //Debug.Log(m_Travelling);
        //If the agent have arrived to waypoint
        if (m_Travelling && m_NavMeshAgent.remainingDistance <= 0.1f)
        {
            m_Agent.Material.color = Color.blue;
            
            m_Travelling = false;

            
            //Otherwise set a new destination
            SetDestination();
            return NodeState.SUCCESS;
            
        }


        if (m_Waiting)
        {
            //Count while waiting
            m_WaitTimer += Time.deltaTime;
            if (m_WaitTimer >= m_TotalWaitTime)
            {
                m_Waiting = false;

                //Set new destination when done waiting
                SetDestination();
                return NodeState.SUCCESS;
            }
        }



        return NodeState.FAILURE;
    }

    private void SetDestination()
    {
        ConnectedWaypoint nextWaypoint = m_CurrentWaypoint.NextWayPoint(m_PreviousWaypoint);
        m_PreviousWaypoint = m_CurrentWaypoint;
        m_CurrentWaypoint = nextWaypoint;

        Vector3 targetVector = m_CurrentWaypoint.transform.position;
        m_NavMeshAgent.SetDestination(targetVector);
        m_Travelling = true;
    }


}
