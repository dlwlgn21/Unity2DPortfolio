using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager
{
    // TODO : ���߿� ���Ͱ� ������ ������ ����� �־�� �Ѵ�.

    public void Init()
    {

    }


    public void DropItem(define.EMonsterNames eMonName)
    {
        switch (eMonName)
        {
            case define.EMonsterNames.Archer:
                break;
            case define.EMonsterNames.Blaster:
                break;
            case define.EMonsterNames.CagedShoker:
                break;
            case define.EMonsterNames.RedGhoul:
                break;
            case define.EMonsterNames.HeabySlicer:
                break;
            case define.EMonsterNames.Gunner:
                break;
            case define.EMonsterNames.Shielder:
                Debug.Assert(false);
                break;
            case define.EMonsterNames.Warden:
                break;
            case define.EMonsterNames.Flamer:
                break;
            case define.EMonsterNames.BossColossal:
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
}
