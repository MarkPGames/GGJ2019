using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerResoureType
{
    HUNGER,
    THIRST,
    STAMINA
}

[System.Serializable]
public struct PlayerResource
{
    [SerializeField]
    private float m_fMax;
    public float Max { get { return m_fMax; } }
    [SerializeField]
    private float m_fCurrent;
    public float Current { get { return m_fCurrent; } }

    public float Progress { get { return m_fCurrent / m_fMax; } }

    public void Deplete(float a_fDepleteAmount)
    {
        m_fCurrent -= a_fDepleteAmount;
    }

    public void Reset()
    {
        m_fCurrent = m_fMax;
    }
}

public class PlayerResources : MonoBehaviour
{
    [SerializeField]
    private PlayerResource[] m_resources;

    [SerializeField]
    private int m_iTimesFainted;

    [SerializeField]
    private int m_iMaxFaints;

    private void Start()
    {
        //initialize all the resources to their max
        for (int i = 0; i < m_resources.Length; i++)
        {
            m_resources[i].Reset();
        }
    }

    public void DepleteResource(PlayerResoureType a_resourceType, float a_fDepletionAmount)
    {
        m_resources[(int)a_resourceType].Deplete(a_fDepletionAmount);
    }
}
