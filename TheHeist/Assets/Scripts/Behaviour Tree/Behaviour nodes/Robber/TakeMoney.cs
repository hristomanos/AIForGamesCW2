using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TakeMoney : Node
{
    BankRobber m_BankRobber;


    public TakeMoney(BankRobber bankRobber)
    {
        m_BankRobber = bankRobber;
    }


    public override NodeState Execute()
    {
        m_BankRobber.Material.color = Color.green;
        m_BankRobber.ObtainedMoney = true;
        Debug.Log("Obtained money: " + m_BankRobber.ObtainedMoney);
        return NodeState.SUCCESS;
    }
}
