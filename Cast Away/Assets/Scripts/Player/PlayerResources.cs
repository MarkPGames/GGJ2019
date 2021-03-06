﻿using System.Collections;
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
    private PlayerResoureType mResourceType;
    [SerializeField]
    private float m_fMax;
    public float Max { get { return m_fMax; } }
    [SerializeField]
    private float m_fCurrent;
    public float Current { get { return m_fCurrent; } }

    public bool depleteViaDelta;
    public bool Depleted
    {
        get
        {
            if (Progress <= 0)
            {
                return true;
            }
            return false;
        }
    }
    public float Progress { get { return m_fCurrent / m_fMax; } }

    [SerializeField]
    private float ReductionRate;

    public void Deplete(float a_fDepleteAmount)
    {
        m_fCurrent -= a_fDepleteAmount;
    }

    public void Deplete(bool doubleRate = false, bool reducedRate = false)
    {
        //Really hacky but outta time
        float multiplier = 100;
        float reductionRate = 0.5f;

        float tempResourceReductionRate = ReductionRate;
        if (doubleRate)
        {
            tempResourceReductionRate *= multiplier;
        }
        if (reducedRate)
        {
            tempResourceReductionRate *= reductionRate;
        }
        if (!depleteViaDelta)
        {
            m_fCurrent -=tempResourceReductionRate;
        }
        else
        {
            m_fCurrent -= tempResourceReductionRate * Time.deltaTime;
        }

        if (m_fCurrent <= 0)
        {
            m_fCurrent = 0;
        }
    }


    public void Replenish(float replenishAmount)
    {
        m_fCurrent += replenishAmount;
        if (m_fCurrent >= m_fMax)
        {
            m_fCurrent = m_fMax;
        }
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
    public PlayerResource[] Resources { get { return m_resources; } }

    [SerializeField]
    private int m_iTimesFainted;

    [SerializeField]
    private int m_iMaxFaints;

    private PlayerController _playerController;

    public bool GameOver
    {
        get
        {
            if (m_iTimesFainted > m_iMaxFaints)
            {
                return true;
            }
            return false;
        }
    }
    private void Start()
    {
        _playerController = this.GetComponent<PlayerController>();
        //initialize all the resources to their max
        for (int i = 0; i < m_resources.Length; i++)
        {
            m_resources[i].Reset();
        }
    }

    private void Update()
    {
        if (m_resources[(int)PlayerResoureType.HUNGER].Depleted || m_resources[(int)PlayerResoureType.THIRST].Depleted)
        {
            m_resources[(int)PlayerResoureType.STAMINA].Deplete(false, true);
        }
    }


    public void OnFainted()
    {
        m_iTimesFainted++;
    }

    public void ReplenishResource(PlayerResoureType a_resourceType, float a_fReplenishAmount)
    {
        m_resources[(int)a_resourceType].Replenish(a_fReplenishAmount);
    }

    public void DepleteResource(PlayerResoureType a_resourceType, bool doubleRate = false)
    {
        bool hungerDepleted = m_resources[(int)PlayerResoureType.HUNGER].Depleted;
        bool thirstDepletd = m_resources[(int)PlayerResoureType.THIRST].Depleted;

        if (a_resourceType == PlayerResoureType.STAMINA)
        {
            m_resources[(int)a_resourceType].Deplete(doubleRate);
        }
        else
        {
            m_resources[(int)a_resourceType].Deplete(doubleRate);
        }
        if (_playerController.CurrentState == PlayerState.FAINT)
        {
            return;
        }
        if (m_resources[(int)PlayerResoureType.STAMINA].Depleted)
        {
            _playerController.ChangeState(PlayerState.FAINT, Vector3.zero);
        }
    }
}
