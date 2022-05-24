using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : Node
{
    AIAgent m_Agent;
    float m_Threshold;

    public HealthNode(float threshold, AIAgent agent)
    {
        m_Agent = agent;
        m_Threshold = threshold;
    }


    public override NodeState Execute()
    {
        return m_Agent.Health <= m_Threshold ? NodeState.SUCCESS : NodeState.FAILURE; 
    }
}
