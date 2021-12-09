using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AI_Data_Su
{
    //�b�|
    public float m_fRadius;
    //���w����
    public float m_fProbeLength;
    //�t��
    public float m_Speed;
    //�̰��t
    public float m_fMaxSpeed;
    //��V
    public float m_fRot;
    //�̤j��V
    public float m_fMaxRot;
    public GameObject m_gObject;

    //����
    public float m_fSight;
    //�����Z��
    public float m_fAttackRange;

    [HideInInspector]
    //�����ɶ�(�Ȯɥ���
    public float m_fAttackTime;
    //�ͩR��(�Ȯɥ���
    public float m_fHp;
    [HideInInspector]
    //�ؼ�
    public GameObject m_TargetObject;

    [HideInInspector]
    //�ؼФ��V�q
    public Vector3 m_vTarget;
    [HideInInspector]
    //�ؼФ��{�b�V�q(?
    public Vector3 m_vCurrentVector;
    [HideInInspector]
    //�Ȯ���V�O
    public float m_fTempTurnForce;
    [HideInInspector]
    //�e�i�O�D
    public float m_fMoveForce;
    [HideInInspector]
    //bool �O�_����(??
    public bool m_bMove;

    [HideInInspector]
    //��ê��
    public bool m_bCol;

    //[HideInInspector]
    //�������A��
    //public FSMSystem m_FSMSystem;
    //�Ȯɥ���
    //public BT.cBTSystem m_BTSystem;
}