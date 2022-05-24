using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InRangeNode : Node
{
   
    float m_Range;
    Transform m_Target;
    Transform m_Origin;

    public InRangeNode(float range, Transform target,Transform origin)
    {
        m_Range = range;
        m_Target = target;
        m_Origin = origin;
    }


    public override NodeState Execute()
    {
        if (m_Target != null)
        {

        float distanceFromTarget = Vector3.Distance(m_Target.position,m_Origin.position);
        //Debug.Log("DistanceFromTarget: " + distanceFromTarget);
        return distanceFromTarget <= m_Range ? NodeState.SUCCESS : NodeState.FAILURE;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
