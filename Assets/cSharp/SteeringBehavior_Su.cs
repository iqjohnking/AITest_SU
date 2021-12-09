using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior_Su 
{
    static public void Move(AI_Data_Su data)
    {
        //�P�w�O�_��ʤ�
        if (data.m_bMove == false)
        {
            return;
        }
        //��m/����]��t
        Transform t = data.m_gObject.transform;
        //�{�b��m
        Vector3 cPos = data.m_gObject.transform.position;
        //steer�V�q(steer?)
        Vector3 vR = t.right;
        //"���"�e�i�V�q
        Vector3 vOriF = t.forward;
        //�e�i�V�q
        Vector3 vF = data.m_vCurrentVector;
        //�������O�D
        if (data.m_fTempTurnForce > data.m_fMaxRot)
        {
            data.m_fTempTurnForce = data.m_fMaxRot;
        }
        else if (data.m_fTempTurnForce < -data.m_fMaxRot)
        {
            data.m_fTempTurnForce = -data.m_fMaxRot;
        }
        //�e�i�V�q = �V�e + steer�V�q*��V�O�D
        vF = vF + vR * data.m_fTempTurnForce;
        //�e�i�V�q ���V�q��
        vF.Normalize();
        //���󥿫e�譱�� = �e�i�V�q
        t.forward = vF;

        //�t�� = �t�� + �e�i�O�D*���ɶ�
        data.m_Speed = data.m_Speed + data.m_fMoveForce * Time.deltaTime;
        //����̰��t�̧C�t
        if (data.m_Speed < 0.01f)
        {
            data.m_Speed = 0.01f;
        }
        else if (data.m_Speed > data.m_fMaxSpeed)
        {
            data.m_Speed = data.m_fMaxSpeed;
        }

        //��������ê����ƨg�ݰ�
        //�p�G�������ê���A���������V
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
        //�p�G�t�׹L�C�A����90�����s(f=r)
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
        //(�s)�{�b��m =  �{�b��m  +  �V�e�V�q*�V�e�t��
        cPos = cPos + t.forward * data.m_Speed * Time.deltaTime;
        //�q�{�b��m���ʨ�s��m
        t.position = cPos;
        //���ʪ�����O�¤@�ӦV�q���_�ܤƦ�m�C
    }

    //�I������
    static public bool CheckCollision(AI_Data_Su data)
    {
        //�Ы�List<Obstacle>�A�䤤�����󳣬O�j�ת��ؼ�
        List<Obstacle> m_AvoidTargets = Main.m_Instance.GetObstacles();
        if (m_AvoidTargets == null)
        {
            return false;
        }
        Transform ct = data.m_gObject.transform;
        //����{�b��m
        Vector3 cPos = ct.position;
        //���魱�ۤ�V
        Vector3 cForward = ct.forward;
        Vector3 vec;

        float fDist = 0.0f;
        float fDot = 0.0f;
        //���o��List�����X�Ӫ���
        int iCount = m_AvoidTargets.Count;


        for (int i = 0; i < iCount; i++)
        {
            //�j��A�������j�ץؼФ��V�q���n�]�@��
            //�ԲӹϧΨ����O
            vec = m_AvoidTargets[i].transform.position - cPos;
            vec.y = 0.0f;
            fDist = vec.magnitude;

            //�p�G�ۨ��P�ؼжZ�� > �ڪ��d�� + �ؼнd��A�h�L�I��
            if (fDist > data.m_fProbeLength + m_AvoidTargets[i].m_fRadius)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
                //�P�_�L�I��,�i�J�U�@���j��
            }

            //���ƦV�q
            vec.Normalize();
            //Dot �I���n
            fDot = Vector3.Dot(vec, cForward);
            if (fDot < 0)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                continue;
                //�P�_�L�I��,�i�J�U�@���j��
            }
            m_AvoidTargets[i].m_eState = Obstacle.eState.INSIDE_TEST;

            //�p�G��ê���Z�����w�����u�Z�� > ���� + �ؼХb�|�A�h�L�I��
            float fProjDist = fDist * fDot;
            float fDotDist = Mathf.Sqrt(fDist * fDist - fProjDist * fProjDist);
            if (fDotDist > m_AvoidTargets[i].m_fRadius + data.m_fRadius)
            {
                continue;
                //�P�_�L�I��,�i�J�U�@���j��
            }
            //�p�G�e����continue�A�N��o�@��true���|�Qreturn�A
            //�ӬO�j��~��false�Qreturn�C
            return true;
        }
        return false;
    }

    //�I���j��
    static public bool CollisionAvoid(AI_Data_Su data)
    {
        List<Obstacle> m_AvoidTargets = Main.m_Instance.GetObstacles();
        Transform ct = data.m_gObject.transform;
        //����{�b��m
        Vector3 cPos = ct.position;
        //���魱�ۤ�V
        Vector3 cForward = ct.forward;
        data.m_vCurrentVector = cForward;
        Vector3 vec;

        //�ԲӹϧΨ����O
        float fFinalDotDist;
        float fFinalProjDist;
        Vector3 vFinalVec = Vector3.forward;
        Obstacle oFinal = null;

        float fDist = 0.0f;
        float fDot = 0.0f;
        float fFinalDot = 0.0f;
        //���o��List�����X�Ӫ���
        int iCount = m_AvoidTargets.Count;

        //???
        float fMinDist = 10000.0f;
        for (int i = 0; i < iCount; i++)
        {
            vec = m_AvoidTargets[i].transform.position - cPos;
            vec.y = 0.0f;
            fDist = vec.magnitude;
            //����P�ؼжZ�� > ���� + �ؼХb�|
            if (fDist > data.m_fProbeLength + m_AvoidTargets[i].m_fRadius)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                //�P�_�L�I��,�i�J�U�@���j��
                continue;
            }

            vec.Normalize();
            fDot = Vector3.Dot(vec, cForward);
            if (fDot < 0)
            {
                m_AvoidTargets[i].m_eState = Obstacle.eState.OUTSIDE_TEST;
                //�P�_�L�I��,�i�J�U�@���j��
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
                //�P�_�L�I��,�i�J�U�@���j��
                continue;
            }

            //�W�z��continue������A�N��i�঳�I���A����H�U�{��
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
            //��~�n�A�M�w��V
            Vector3 vCross = Vector3.Cross(cForward, vFinalVec);
            //��X���s���O�D
            float fTurnMag = Mathf.Sqrt(1.0f - fFinalDot * fFinalDot);
            //�~�n�j��s�A�N����b�k��A�ҥH�n�k�V����
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
        //�p�G��"�ؼ�"�b���w���h��t
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
        //���n
        float fDotF = Vector3.Dot(vf, vec);

        //����
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
        //�V�ؼЪ��ޤO
        Vector3 vec = data.m_vTarget - cPos;
        //y�b�T�w
        vec.y = 0.0f;
        //�V�q����(�Z��)
        float fDist = vec.magnitude;
        //�p�G�V�q���� �p�� �t��+0.001f�A�������ʨ�ؼ�
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
        //��V�ؼЪ�����
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
            //�W�[�u�Z������V�O
            if (fDist < 3.0f)
            {
                fDotR *= (fDist / 3.0f + 1.0f);
            }
            data.m_fTempTurnForce = fDotR;
        }

        //�p�G�Z���ܵu�N��t
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
