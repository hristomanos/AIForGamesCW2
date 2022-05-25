using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//The script is responsible for implementing the bank robber's behaviour.
//His main objective is to steal the money from the bank's safe and if he encounters the security guard while doing so to fall into a gun fight.

public class BankRobber : AIAgent
{

    //Declare safe position. This makes it easier to point to it straight away. Maybe try searching for it instead of going straight to it.
    [SerializeField] Transform m_SafeTranform; // <--

    //Declare Exit position.
    [SerializeField] Transform m_ExitTranform; // <--

    //Separates the infiltration from fleeing strategy
    bool m_ObtainedMoney; // <--
    public bool ObtainedMoney { get => m_ObtainedMoney; set => m_ObtainedMoney = value; }
    
    

    void Start()
    {
        ObtainedMoney = false;  

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
    }

    void ConstructBehaviorTree()
    {

        AtSafe               atSafeNode              = new AtSafe(m_NavMeshAgent,m_SafeTranform.position);
        AtSafe               atExitNode              = new AtSafe(m_NavMeshAgent, m_ExitTranform.position);

        //Search for safe
        GoToTarget           goToSafeNode            = new GoToTarget(m_NavMeshAgent, m_SafeTranform.position);

        //Take money
        TakeMoney            TakeMoneyNode           = new TakeMoney(this);

        //Did you take the money?
        DidYouTakeMoney      didYouTakeMoneyNode     = new DidYouTakeMoney(this);
        
        //Run to the exit
        GoToTarget           goToExitNode            = new GoToTarget(m_NavMeshAgent, m_ExitTranform.position);

        //Have you taken the money yet?
        InverterNode         didYouTakeMoneyInverter = new InverterNode(didYouTakeMoneyNode);



        //You are either already there or you need to go there
        SelectorNode         safeSelector            = new SelectorNode(new List<Node> { atSafeNode, goToSafeNode });
        SelectorNode         exitSelector            = new SelectorNode(new List<Node> { atExitNode, goToExitNode });

      

        //------------------------SHOOTING-----------------------------------------------------------------------------

        IsCoverAvailableNode isCoverAvailableNode    = new IsCoverAvailableNode(this, m_TargetTransform, m_AvailableCovers);
        GoToCoverNode        goToCoverNode           = new GoToCoverNode(m_NavMeshAgent, this);
        HealthNode           healthNode              = new HealthNode(m_LowHealthThreshold, this);
        IsCoveredNode        isCoveredNode           = new IsCoveredNode(m_TargetTransform, transform);
                                                     
        InverterNode         healthNodeInv           = new InverterNode(healthNode);
                                                     
        ChaseNode            chaseNode               = new ChaseNode(m_TargetTransform, m_NavMeshAgent, this);
        InRangeNode          chasingRangeNode        = new InRangeNode(m_ChasingRange, m_TargetTransform, transform);
                                                     
        InRangeNode          shootingRangeNode       = new InRangeNode(m_ShootingRange, m_TargetTransform, transform);
        ShootNode            shootNode               = new ShootNode(this, m_NavMeshAgent, m_TargetTransform);
                                                     
        SequenceNode         chaseSequence           = new SequenceNode(new List<Node> { chasingRangeNode, chaseNode });
        SequenceNode         shootSequence           = new SequenceNode(new List<Node> {shootingRangeNode, shootNode });
                                                     
        SequenceNode         goToCoverSequence       = new SequenceNode(new List<Node> { isCoverAvailableNode, goToCoverNode });
        SelectorNode         findCoverSelector       = new SelectorNode(new List<Node> { goToCoverSequence, chaseSequence });
        SelectorNode         tryToTakeCoverSelector  = new SelectorNode(new List<Node> { isCoveredNode, findCoverSelector });
        SequenceNode         mainCoverSequence       = new SequenceNode(new List<Node> { healthNode, tryToTakeCoverSelector });
                                                     
                                                     
        //Go to the safe and get the money           
        SequenceNode         goToSafeSequence        = new SequenceNode(new List<Node> {healthNodeInv, didYouTakeMoneyInverter, safeSelector, TakeMoneyNode });

        //Run to the exit if you have obtained the money
        SequenceNode         goToExitSequence        = new SequenceNode(new List<Node> {healthNodeInv, didYouTakeMoneyNode, exitSelector });


        m_TopNode = new SelectorNode(new List<Node> { mainCoverSequence, shootSequence, chaseSequence, goToSafeSequence, goToExitSequence });

        //m_TopNode = new SelectorNode(new List<Node> {mainCoverSequence, shootSequence, chaseSequence});
    }
}
