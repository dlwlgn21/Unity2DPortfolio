using define;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public enum ESkillSlot
{
    AKey,
    SKey,
    CKey,
    Count
}
public class PlayerSkillManager
{


    Dictionary<ESkillType, Skill_BaseController> _skillDict = new();
    UI_SkillCoolTimer[] skillCoolTimer = new UI_SkillCoolTimer[(int)ESkillSlot.Count];
    Skill_BaseController[] _currSkill = new Skill_BaseController[2];
    GameObject[] _skillSlots = new GameObject[2];
    public void Init()
    {
        // TODO : ���߿��� �ʿ��� �� �޸𸮿� �ε� �ϴ� ������� �ٲ����.
        #region SkillInit
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Dictionary<int, data.SkillInfo> skillDict = Managers.Data.SkillInfoDict;

        Skill_BaseController roll = Utill.GetComponentInChildrenOrNull<Skill_RollController>(player, GetSkillControllerObjectName(ESkillType.Roll));
        Skill_BaseController reaper = Inst<Skill_SpawnRepaerController>(skillDict[(int)ESkillType.Spawn_Reaper].controllerPrefabPath);
        Skill_BaseController panda = Inst<Skill_SpawnPandaController>(skillDict[(int)ESkillType.Spawn_Panda].controllerPrefabPath);
        Skill_BaseController blackFlame = Inst<Skill_BlackFlameController>(skillDict[(int)ESkillType.Cast_BlackFlame].controllerPrefabPath);
        Skill_BaseController swordStrike = Inst<Skill_SwordStrikeController>(skillDict[(int)ESkillType.Cast_SwordStrike].controllerPrefabPath);

        reaper.gameObject.name = GetSkillControllerObjectName(ESkillType.Spawn_Reaper);
        panda.gameObject.name = GetSkillControllerObjectName(ESkillType.Spawn_Panda);
        blackFlame.gameObject.name = GetSkillControllerObjectName(ESkillType.Cast_BlackFlame);
        swordStrike.gameObject.name = GetSkillControllerObjectName(ESkillType.Cast_SwordStrike);

        Object.DontDestroyOnLoad(reaper.gameObject);
        Object.DontDestroyOnLoad(panda.gameObject);
        Object.DontDestroyOnLoad(blackFlame.gameObject);
        Object.DontDestroyOnLoad(swordStrike.gameObject);

        _skillDict.Add(ESkillType.Roll, roll);
        _skillDict.Add(ESkillType.Spawn_Reaper, reaper);
        _skillDict.Add(ESkillType.Spawn_Panda, panda);
        _skillDict.Add(ESkillType.Cast_BlackFlame, blackFlame); 
        _skillDict.Add(ESkillType.Cast_SwordStrike, swordStrike);

        #endregion

        GameObject uiPlayerHud = GameObject.Find("UI_PlayerHUD");
        skillCoolTimer[(int)ESkillSlot.AKey] = Utill.GetComponentInChildrenOrNull<UI_SkillCoolTimer>(uiPlayerHud, "AKeySkillCoolTimer");
        skillCoolTimer[(int)ESkillSlot.SKey] = Utill.GetComponentInChildrenOrNull<UI_SkillCoolTimer>(uiPlayerHud, "SKeySkillCoolTimer");
        skillCoolTimer[(int)ESkillSlot.CKey] = Utill.GetComponentInChildrenOrNull<UI_SkillCoolTimer>(uiPlayerHud, "PlayerRollCoolTimer");

        _skillSlots[(int)ESkillSlot.AKey] = uiPlayerHud.transform.Find("AKeySkillSlot").gameObject;
        _skillSlots[(int)ESkillSlot.SKey] = uiPlayerHud.transform.Find("SKeySkillSlot").gameObject;

        Managers.Input.KeyboardHandler -= OnSkillKeyDowned;
        Managers.Input.KeyboardHandler += OnSkillKeyDowned;
        Debug.Assert(skillCoolTimer[(int)ESkillSlot.AKey] != null && skillCoolTimer[(int)ESkillSlot.SKey] != null);
        Debug.Assert(_skillSlots[0] != null && _skillSlots[1] != null);
    }


    public Skill_BaseController GetSkill(ESkillType eType)
    {
        Debug.Assert(_skillDict[eType] != null);
        return _skillDict[eType];
    }

    public void Clear()
    {
        Managers.Input.KeyboardHandler -= OnSkillKeyDowned;
    }
    public string GetSkillObjectName(ESkillType eType)
    {
        return Managers.Data.SkillInfoDict[(int)eType].objectPrefabPath.Substring(Managers.Data.SkillInfoDict[(int)eType].objectPrefabPath.LastIndexOf('/') + 1);
    }

    void OnSkillKeyDowned()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UseSkill(ESkillSlot.AKey, ESkillType.Cast_BlackFlame);
            return;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            UseSkill(ESkillSlot.SKey, ESkillType.Cast_SwordStrike);
            return;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_skillDict[ESkillType.Roll].TryUseSkill())
            {
                skillCoolTimer[(int)ESkillSlot.CKey].StartCoolTime(_skillDict[ESkillType.Roll].SkillCoolTimeInSec);
            }
            return;
        }
    }

    void UseSkill(ESkillSlot eSlot, ESkillType eSkillType)
    {
        if (_skillDict[eSkillType].TryUseSkill())
        {
            skillCoolTimer[(int)eSlot].StartCoolTime(_skillDict[eSkillType].SkillCoolTimeInSec);
            if (eSlot == ESkillSlot.AKey)
                Managers.Tween.StartUIScaleTW(_skillSlots[(int)eSlot].transform, OnAKeyScaleTWEnd);
            else
                Managers.Tween.StartUIScaleTW(_skillSlots[(int)eSlot].transform, OnSKeyScaleTWEnd);
        }
        else
        {
            Managers.Tween.StartUIDoPunchPos(_skillSlots[(int)eSlot].transform);
        }
    }

    void OnAKeyScaleTWEnd()
    {
        Managers.Tween.EndToOneUIScaleTW(_skillSlots[(int)ESkillSlot.AKey].transform);
    }
    void OnSKeyScaleTWEnd()
    {
        Managers.Tween.EndToOneUIScaleTW(_skillSlots[(int)ESkillSlot.SKey].transform);
    }

    T Inst<T>(string path) where T : Object
    {
        return Managers.Resources.Instantiate<T>(path);
    }

    string GetSkillControllerObjectName(ESkillType eType)
    {
        return Managers.Data.SkillInfoDict[(int)eType].controllerPrefabPath.Substring(Managers.Data.SkillInfoDict[(int)eType].controllerPrefabPath.LastIndexOf('/') + 1);
    }


}
