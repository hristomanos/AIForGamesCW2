using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] float m_lifeTime;
    
    [SerializeField] float m_Velocity;

    [SerializeField] float m_Damage;

    Rigidbody              m_RigidBody;



    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        Destroy(gameObject, m_lifeTime);
    }

    private void FixedUpdate()
    {
        m_RigidBody.velocity = transform.up * m_Velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            AIAgent agent = other.GetComponent<AIAgent>();
            agent.ReceiveDamage(m_Damage);
            Debug.Log("Health: " + agent.Health);
        }
            Destroy(gameObject);
    }


}
