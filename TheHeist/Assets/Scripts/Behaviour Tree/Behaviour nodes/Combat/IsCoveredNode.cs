using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsCoveredNode : Node
{
    Transform m_Target;
    Transform m_Origin;

    public IsCoveredNode(Transform target,Transform origin)
    {
        
        m_Target = target;
        m_Origin = origin;
    }


    public override NodeState Execute()
    {
        RaycastHit hit;

        if (Physics.Raycast(m_Origin.position, m_Target.position - m_Origin.position, out hit))
        {
            if (hit.collider.transform != m_Target)
            {
                Debug.Log("I am covered already!");
                return NodeState.SUCCESS;
            }
        }

        return NodeState.FAILURE;
    }
}
