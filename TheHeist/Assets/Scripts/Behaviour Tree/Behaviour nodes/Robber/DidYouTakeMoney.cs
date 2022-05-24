using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DidYouTakeMoney : Node
{
    BankRobber m_BankRobber;


    

    public DidYouTakeMoney(BankRobber bankRobber)
    {
        m_BankRobber = bankRobber;
    }


    public override NodeState Execute()
    {
        if (m_BankRobber != null)
        {
            //Debug.Log(m_BankRobber.ObtainedMoney ? NodeState.SUCCESS : NodeState.FAILURE);
            return  m_BankRobber.ObtainedMoney ?  NodeState.SUCCESS :  NodeState.FAILURE;
        }
        else
        {
            Debug.LogError("Navmeshposition or safe position is null");
            return NodeState.FAILURE;
        }
    }
}
