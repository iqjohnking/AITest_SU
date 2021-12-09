using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AI_Data_Su
{
    //半徑
    public float m_fRadius;
    //探針長度
    public float m_fProbeLength;
    //速度
    public float m_Speed;
    //最高速
    public float m_fMaxSpeed;
    //轉向
    public float m_fRot;
    //最大轉向
    public float m_fMaxRot;
    public GameObject m_gObject;

    //視野
    public float m_fSight;
    //攻擊距離
    public float m_fAttackRange;

    [HideInInspector]
    //攻擊時間(暫時未用
    public float m_fAttackTime;
    //生命值(暫時未用
    public float m_fHp;
    [HideInInspector]
    //目標
    public GameObject m_TargetObject;

    [HideInInspector]
    //目標之向量
    public Vector3 m_vTarget;
    [HideInInspector]
    //目標之現在向量(?
    public Vector3 m_vCurrentVector;
    [HideInInspector]
    //暫時轉向力
    public float m_fTempTurnForce;
    [HideInInspector]
    //前進力道
    public float m_fMoveForce;
    [HideInInspector]
    //bool 是否移動(??
    public bool m_bMove;

    [HideInInspector]
    //障礙物
    public bool m_bCol;

    //[HideInInspector]
    //有限狀態機
    //public FSMSystem m_FSMSystem;
    //暫時未用
    //public BT.cBTSystem m_BTSystem;
}