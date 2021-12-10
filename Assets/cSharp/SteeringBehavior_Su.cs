using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior_Su
{
    static public void Move(AI_Data_Su data)
    {
        //判定是否行動中
        if (data.m_bMove == false)
        {
            return;
        }
        //位置/旋轉設為t
        Transform t = data.m_gObject.transform;
        //現在位置
        Vector3 cPos = data.m_gObject.transform.position;
        //steer向量(steer?)
        Vector3 vR = t.right;
        //"原先"前進向量
        Vector3 vOriF = t.forward;
        //前進向量
        Vector3 vF = data.m_vCurrentVector;
        //限制旋轉力道
        if (data.m_fTempTurnForce > data.m_fMaxRot)
        {
            data.m_fTempTurnForce = data.m_fMaxRot;
        }
        else if (data.m_fTempTurnForce < -data.m_fMaxRot)
        {
            data.m_fTempTurnForce = -data.m_fMaxRot;
        }
        //前進向量 = 向前 + steer向量*轉向力道
        vF = vF + vR * data.m_fTempTurnForce;
        //前進向量 單位向量化
        vF.Normalize();
        //物件正前方面相 = 前進向量
        t.forward = vF;

        //速度 = 速度 + 前進力道*單位時間
        data.m_Speed = data.m_Speed + data.m_fMoveForce * Time.deltaTime;
        //限制最高速最低速
        if (data.m_Speed < 0.01f)
        {
            data.m_Speed = 0.01f;
        }
        else if (data.m_Speed > data.m_fMaxSpeed)
        {
            data.m_Speed = data.m_fMaxSpeed;
        }

        //防止偵測到障礙物後瘋狂抖動
        //如果偵測到障礙物，維持原先方向
        if (data.m_bCol == false)
        {
            Debug.Log("CheckCollision");
            if (SteeringBehavior_Su.CheckCollision(data))
            {
                Debug.Log("CheckCollision true");
                t.forward = vOriF;
            }
            else
            {
                Debug.Log("CheckCollision true");
            }
        }
        //如果速度過慢，直接90度轉彎(f=r)
        else
        {
            if (data.m_Speed < 0.02f)
            {
                if (data.m_fTempTurnForce > 0)
                {
                    t.forward = vR;
                }
                else
                {
                    t.forward = -vR;
                }

            }
        }
        //(新)現在位置 =  現在位置  +  向前向量*向前速度
        cPos = cPos + t.forward * data.m_Speed * Time.deltaTime;
        //從現在位置移動到新位置
        t.position = cPos;
        //移動的本質是朝一個向量不斷變化位置。
    }

    //碰撞偵測
    static public bool CheckCollision(AI_Data_Su data)
    {
        //創建List<Obstacle>，其中的物件都是迴避的目標
        List<Obstacle> m_AvoidTargets = Main.m_Instance.GetObstacles();
        if (m_AvoidTargets == null)
        {
            return false;
        }
        Transform ct = data.m_gObject.transform;
        //物體現在位置
        Vector3 cPos = ct.position;
        //物體面相方向
        Vector3 cForward = ct.forward;
        Vector3 vec;

        float fDist = 0.0f;
        float fDot = 0.0f;
        //取得該List中有幾個物件
        int iCount = m_AvoidTargets.Count;


        for (int i = 0; i < iCount; i++)
        {
            //迴圈，全部的迴避目標之向量都要跑一輪
            //詳細圖形見筆記
            vec = m_AvoidTargets[i].transform.position - cPos;
            vec.y = 0.0f;
            fDist = vec.magnitude;

            //如果自身與目標距離 > 我的範圍 + 目標範圍，則無碰撞
            if (fDist > data.m_fProbeLength + m_AvoidTargets[i].m_fRadius)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
                //判斷無碰撞,進入下一輪迴圈
            }

            //單位化向量
            vec.Normalize();
            //Dot 點內積
            fDot = Vector3.Dot(vec, cForward);
            if (fDot < 0)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
                //判斷無碰撞,進入下一輪迴圈
            }
            m_AvoidTargets[i].m_eState = Obstacle.eState.INSIDE_TEST;

            //如果障礙物距離探針的直線距離 > 本體 + 目標半徑，則無碰撞
            float fProjDist = fDist * fDot;
            float fDotDist = Mathf.Sqrt(fDist * fDist - fProjDist * fProjDist);
            if (fDotDist > m_AvoidTargets[i].m_fRadius + data.m_fRadius)
            {
                continue;
                //判斷無碰撞,進入下一輪迴圈
            }
            //如果前面有continue，代表這一行true不會被return，
            //而是迴圈外的false被return。
            return true;
        }
        return false;
    }

    //碰撞迴避
    static public bool CollisionAvoid(AI_Data_Su data)
    {
        List<Obstacle> m_AvoidTargets = Main.m_Instance.GetObstacles();
        Transform ct = data.m_gObject.transform;
        //物體現在位置
        Vector3 cPos = ct.position;
        //物體面相方向
        Vector3 cForward = ct.forward;
        data.m_vCurrentVector = cForward;
        Vector3 vec;

        //詳細圖形見筆記
        float fFinalDotDist;
        float fFinalProjDist;
        Vector3 vFinalVec = Vector3.forward;
        Obstacle oFinal = null;

        float fDist = 0.0f;
        float fDot = 0.0f;
        float fFinalDot = 0.0f;
        //取得該List中有幾個物件
        int iCount = m_AvoidTargets.Count;

        //???
        float fMinDist = 10000.0f;
        for (int i = 0; i < iCount; i++)
        {
            vec = m_AvoidTargets[i].transform.position - cPos;
            vec.y = 0.0f;
            fDist = vec.magnitude;
            //本體與目標距離 > 本體 + 目標半徑
            if (fDist > data.m_fProbeLength + m_AvoidTargets[i].m_fRadius)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                //判斷無碰撞,進入下一輪迴圈
                continue;
            }

            vec.Normalize();
            fDot = Vector3.Dot(vec, cForward);
            if (fDot < 0)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                //判斷無碰撞,進入下一輪迴圈
                continue;
            }
            else if (fDot > 1.0f)
            {
                fDot = 1.0f;
            }

            m_AvoidTargets[i].m_eState = Obstacle.eState.INSIDE_TEST;
            float fProjDist = fDist * fDot;
            float fDotDist = Mathf.Sqrt(fDist * fDist - fProjDist * fProjDist);
            if (fDotDist > m_AvoidTargets[i].m_fRadius + data.m_fRadius)
            {
                //判斷無碰撞,進入下一輪迴圈
                continue;
            }

            //上述都continue未執行，代表可能有碰撞，執行以下程式
            if (fDist < fMinDist)
            {
                fMinDist = fDist;
                fFinalDotDist = fDotDist;
                fFinalProjDist = fProjDist;
                vFinalVec = vec;
                oFinal = m_AvoidTargets[i];
                fFinalDot = fDot;
            }
        }

        if (oFinal != null)
        {
            //算外積，決定方向
            Vector3 vCross = Vector3.Cross(cForward, vFinalVec);
            //算出轉彎的力道
            float fTurnMag = Mathf.Sqrt(1.0f - fFinalDot * fFinalDot);
            //外積大於零，代表物體在右方，所以要逃向左方
            if (vCross.y > 0.0f)
            {
                fTurnMag = -fTurnMag;
            }
            data.m_fTempTurnForce = fTurnMag;

            float fTotalLen = data.m_fProbeLength + oFinal.m_fRadius;
            float fRatio = fMinDist / fTotalLen;
            if (fRatio > 1.0f)
            {
                fRatio = 1.0f;
            }
            fRatio = 1.0f - fRatio;
            data.m_fMoveForce = -fRatio;
            oFinal.m_eState = Obstacle.eState.COL_TEST;
            data.m_bCol = true;
            data.m_bMove = true;
            return true;
        }
        data.m_bCol = false;
        return false;
    }

    static public bool Flee(AI_Data_Su data)
    {
        Vector3 cPos = data.m_gObject.transform.position;
        Vector3 vec = data.m_vTarget - cPos;
        vec.y = 0.0f;
        float fDist = vec.magnitude;
        data.m_fTempTurnForce = 0.0f;
        //如果有"目標"在探針內則減速
        if (data.m_fProbeLength < fDist)
        {
            if (data.m_Speed > 0.01f)
            {
                data.m_fMoveForce = -1.0f;
            }

            data.m_bMove = true;
            return false;
        }

        Vector3 vf = data.m_gObject.transform.forward;
        Vector3 vr = data.m_gObject.transform.right;
        data.m_vCurrentVector = vf;
        vec.Normalize();
        //內積
        float fDotF = Vector3.Dot(vf, vec);

        //死區
        if (fDotF < -0.995f)
        {
            fDotF = -1.0f;
            data.m_vCurrentVector = -vec;
            //  data.m_gObject.transform.forward = -vec;
            data.m_fTempTurnForce = 0.0f;
            data.m_fRot = 0.0f;
        }
        else
        {
            if (fDotF > 1.0f)
            {
                fDotF = 1.0f;
            }
            float fDotR = Vector3.Dot(vr, vec);

            if (fDotF > 0.0f)
            {

                if (fDotR > 0.0f)
                {
                    fDotR = 1.0f;
                }
                else
                {
                    fDotR = -1.0f;
                }
            }
            Debug.Log(fDotR);
            data.m_fTempTurnForce = -fDotR;
            // data.m_fTempTurnForce *= 0.1f;
        }
        data.m_fMoveForce = -fDotF;
        data.m_bMove = true;
        return true;
    }



    static public bool Seek(AI_Data_Su data)
    {
        Vector3 cPos = data.m_gObject.transform.position;
        //向目標的引力
        Vector3 vec = data.m_vTarget - cPos;
        //y軸固定
        vec.y = 0.0f;
        //向量長度(距離)
        float fDist = vec.magnitude;
        //如果向量長度 小於 速度+0.001f，直接移動到目標
        if (fDist < data.m_Speed * Time.deltaTime + 0.001f)
        {
            Vector3 vFinal = data.m_vTarget;
            vFinal.y = cPos.y;
            data.m_gObject.transform.position = vFinal;
            data.m_fMoveForce = 0.0f;
            data.m_fTempTurnForce = 0.0f;
            data.m_Speed = 0.0f;
            data.m_bMove = false;
            return false;
        }
        Vector3 vf = data.m_gObject.transform.forward;
        Vector3 vr = data.m_gObject.transform.right;
        data.m_vCurrentVector = vf;
        vec.Normalize();

        float fDotF = Vector3.Dot(vf, vec);
        //轉向目標的死區
        if (fDotF > 0.995f)
        {
            fDotF = 1.0f;
            data.m_vCurrentVector = vec;
            data.m_fTempTurnForce = 0.0f;
            data.m_fRot = 0.0f;
        }
        else
        {
            if (fDotF < -1.0f)
            {
                fDotF = -1.0f;
            }
            float fDotR = Vector3.Dot(vr, vec);

            if (fDotF < 0.0f)
            {

                if (fDotR > 0.0f)
                {
                    fDotR = 1.0f;
                }
                else
                {
                    fDotR = -1.0f;
                }
            }
            //增加短距離的轉向力
            if (fDist < 3.0f)
            {
                fDotR *= (fDist / 3.0f + 1.0f);
            }
            data.m_fTempTurnForce = fDotR;
        }

        //如果距離很短就減速
        if (fDist < 3.0f)
        {
            Debug.Log(data.m_Speed);
            if (data.m_Speed > 0.1f)
            {
                data.m_fMoveForce = -(1.0f - fDist / 3.0f) * 5.0f;
            }
            else
            {
                data.m_fMoveForce = fDotF * 100.0f;
            }
        }
        else
        {
            data.m_fMoveForce = 100.0f;
        }

        data.m_bMove = true;
        return true;
    }


}