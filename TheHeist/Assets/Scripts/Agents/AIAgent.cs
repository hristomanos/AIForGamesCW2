using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] protected float      m_StartingHealth;
    [SerializeField] protected float      m_LowHealthThreshold;

    [Header("Range")]
    [SerializeField] protected float      m_ShootingRange;
    [SerializeField] protected float      m_ChasingRange;

    [Header("Combat")]
    [SerializeField] protected Transform  m_TargetTransform;
    [SerializeField] protected GameObject m_BulletPrefab;
    [SerializeField] protected Transform  m_SpawnBulletTransform;

    //Cover
    [SerializeField] protected Cover[]    m_AvailableCovers;
    protected Transform                   m_BestCoverSpot;
    public void SetBestCoverSpot(Transform bestSpot)
    {
        m_BestCoverSpot = bestSpot;
    }
    public Transform GetBestCoverSpot()
    {
        return m_BestCoverSpot;
    }

    //Move the game object arround without input
    protected NavMeshAgent                m_NavMeshAgent;

    //Declare the behavior tree's root node
    protected SelectorNode                m_TopNode;

    Material m_Material;
    public Material Material { get => m_Material; set => m_Material = value; }

    float m_Health;
    public float Health { get => m_Health; set => m_Health = value; }

    // Start is called before the first frame update
    void Awake()
    {
        m_Health = m_StartingHealth;

        m_Material = GetComponent<MeshRenderer>().material;

        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SpawnBullet()
    {
        GameObject bullet = Instantiate(m_BulletPrefab, m_SpawnBulletTransform.position, m_SpawnBulletTransform.rotation);
    }

    public void ReceiveDamage(float damage)
    {
        m_Health -= damage;

        if (m_Health <= 0)
        {
            m_Health = 0;
            Destroy(gameObject);
        }
    }
}
