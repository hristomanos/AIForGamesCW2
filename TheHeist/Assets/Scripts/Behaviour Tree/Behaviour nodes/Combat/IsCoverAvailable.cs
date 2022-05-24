using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoverAvailableNode : Node
{
    Transform m_Target;
    
    AIAgent m_Agent;

    Cover[] m_AvailableCovers;

    public IsCoverAvailableNode(AIAgent agent, Transform target, Cover[] covers)
    {
        m_AvailableCovers = covers;
        m_Target = target;
        m_Agent = agent;
    }


    public override NodeState Execute()
    {
        Transform bestSpot = FindBestCoverSpot();
        m_Agent.SetBestCoverSpot(bestSpot);
       // Debug.Log("BestSpot: " + bestSpot.position);
        return bestSpot != null ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    Transform FindBestCoverSpot()
    {
        if (m_Agent.GetBestCoverSpot() != null)
        {
            if (CheckIfSpotIsValid(m_Agent.GetBestCoverSpot()))
            {
                return m_Agent.GetBestCoverSpot();
            }
        }

        float minAngle = 90;
        Transform bestSpot = null;

        for (int i = 0; i < m_AvailableCovers.Length; i++)
        {
            Transform bestSpotInCover = FindBestSpotInCover(m_AvailableCovers[i], ref minAngle);

            if (bestSpotInCover != null)
            {
                bestSpot = bestSpotInCover;
            }
        }

        return bestSpot;
    }

    Transform FindBestSpotInCover(Cover cover, ref float minAngle)
    {
        Transform[] availableSpots = cover.GetCoverSpots();

        Transform bestSpot = null;


        for (int i = 0; i < availableSpots.Length; i++)
        {
            Vector3 direction = m_Target.position - availableSpots[i].position;
            if (CheckIfSpotIsValid(availableSpots[i]))
            {
                float angle = Vector3.Angle(availableSpots[i].forward, direction);

                if (angle < minAngle)
                {
                    minAngle = angle;
                    bestSpot = availableSpots[i];
                }
            }
        }

        return bestSpot;
    }

    private bool CheckIfSpotIsValid(Transform spot)
    {
        RaycastHit hit;
        Vector3 direction = m_Target.position - spot.position;

        if (Physics.Raycast(spot.position, direction, out hit))
        {
            if (hit.collider.transform != m_Target)
            {
                return true;
            }
        }

        return false;

    }
}
