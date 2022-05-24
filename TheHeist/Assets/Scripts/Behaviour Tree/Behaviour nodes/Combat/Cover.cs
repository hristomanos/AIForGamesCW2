using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{

    [SerializeField] Transform[] m_CoverSpots;

    public Transform[] GetCoverSpots() { return m_CoverSpots; }

}
