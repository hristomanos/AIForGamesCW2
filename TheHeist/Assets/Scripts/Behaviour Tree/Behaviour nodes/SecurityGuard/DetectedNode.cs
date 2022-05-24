using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectedNode : Node
{
    SecurityGuard m_SecurityGuard;

    public DetectedNode(SecurityGuard securityGuard)
    {
        m_SecurityGuard = securityGuard;
    }

    public override NodeState Execute()
    {
        return m_SecurityGuard.RobberDetected ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
