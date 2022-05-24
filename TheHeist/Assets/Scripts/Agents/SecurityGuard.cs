using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecurityGuard : AIAgent
{

    bool m_RobberDetected;

    public bool RobberDetected { get => m_RobberDetected; set => m_RobberDetected = value; }

    void Start()
    {
        RobberDetected = false;

        if (m_NavMeshAgent != null)
        {
            ConstructBehaviorTree();
        }
        else
            Debug.LogError("Behaviour tree: Nav mesh agent is null");

    }

    void Update()
    {
        //Execute all nodes
        if (m_TopNode != null)
        {
            m_TopNode.Execute();
        }
        else
            Debug.LogError("Top node is null");

        //If all nodes failed
        if (m_TopNode.NodeState == NodeState.FAILURE)
        {
            Debug.LogError("All nodes failed!");
        }

        if (m_TargetTransform == null)
        {
            RobberDetected = false;
        }

    }

    void ConstructBehaviorTree()
    {
        //-------------------------PATROL----------------------------------------------------------------------------------------
        PatrolNode           patrolNode                   = new PatrolNode(m_NavMeshAgent,this);
        DetectedNode         detectedNode                 = new DetectedNode(this);
        InverterNode         detectedNodeInv              = new InverterNode(detectedNode);
        SequenceNode         patrolSequence               = new SequenceNode(new List<Node> {detectedNodeInv, patrolNode });

        //------------------------SHOOTING---------------------------------------------------------------------------------------
        IsCoverAvailableNode isCoverAvailableNode         = new IsCoverAvailableNode(this, m_TargetTransform, m_AvailableCovers);
        GoToCoverNode        goToCoverNode                = new GoToCoverNode(m_NavMeshAgent, this);
        HealthNode           healthNode                   = new HealthNode(m_LowHealthThreshold, this);
        IsCoveredNode        isCoveredNode                = new IsCoveredNode(m_TargetTransform, transform);

        ChaseNode            chaseNode                    = new ChaseNode(m_TargetTransform, m_NavMeshAgent, this);
        InRangeNode          chasingRangeNode             = new InRangeNode(m_ChasingRange, m_TargetTransform, transform);
            
        InRangeNode          shootingRangeNode            = new InRangeNode(m_ShootingRange, m_TargetTransform, transform);
        ShootNode            shootNode                    = new ShootNode(this, m_NavMeshAgent, m_TargetTransform);

        SequenceNode         chaseSequence                = new SequenceNode(new List<Node> { chasingRangeNode, chaseNode });
        SequenceNode         shootSequence                = new SequenceNode(new List<Node> { shootingRangeNode, shootNode });

        SequenceNode         goToCoverSequence            = new SequenceNode(new List<Node> { isCoverAvailableNode, goToCoverNode });
        SelectorNode         findCoverSelector            = new SelectorNode(new List<Node> { goToCoverSequence, chaseNode });
        SelectorNode         tryToTakeCoverSelector       = new SelectorNode(new List<Node> { isCoveredNode, findCoverSelector });
        SequenceNode         mainCoverSequence            = new SequenceNode(new List<Node> { healthNode, tryToTakeCoverSelector });

        SelectorNode         attackSelector               = new SelectorNode(new List<Node> { mainCoverSequence, shootSequence, chaseSequence });
        SequenceNode         detectedSequence             = new SequenceNode(new List<Node> { detectedNode, attackSelector });

        //m_TopNode = new SelectorNode(new List<Node> {mainCoverSequence, shootSequence, chaseSequence });

        m_TopNode = new SelectorNode(new List<Node> { patrolSequence,  detectedSequence});
        
    }




}
