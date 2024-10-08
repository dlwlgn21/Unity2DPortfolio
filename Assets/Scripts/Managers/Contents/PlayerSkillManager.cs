using define;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSkillManager
{
    enum ECoolTimer
    {
        AKey,
        SKey,
        CKey,
        Count
    }

    Dictionary<ESkillType, BasePlayerSkillController> _skillDict = new();
    UI_SkillCoolTimer[] skillCoolTimer = new UI_SkillCoolTimer[(int)ECoolTimer.Count];
    BasePlayerSkillController[] _currSkill = new BasePlayerSkillController[2];
    GameObject[] _skillSlots = new GameObject[2];
    public void Init()
    {
        // TODO : 나중에는 필요할 때 메모리에 로드 하는 방식으로 바꾸긴 해야함.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        BasePlayerSkillController roll = Utill.GetComponentInChildrenOrNull<PlayerSkillRollController>(player, "PlayerSkillRoll");
        BasePlayerSkillController reaper = Managers.Resources.Instantiate<PlayerSkillSpawnRepaerController>("Prefabs/Player/Skills/PlayerSkillSpawnReaper");
        BasePlayerSkillController panda = Managers.Resources.Instantiate<PlayerSkillSpawnPandaController>("Prefabs/Player/Skills/PlayerSkillSpawnPanda");
        reaper.gameObject.name = "PlayerSkillSpawnReaper";
        panda.gameObject.name = "PlayerSkillSpawnPanda";
        Object.DontDestroyOnLoad(reaper.gameObject);
        Object.DontDestroyOnLoad(panda.gameObject);

        _skillDict.Add(ESkillType.Roll, roll);
        _skillDict.Add(ESkillType.Spawn_Reaper, reaper);
        _skillDict.Add(ESkillType.Spawn_Panda, panda); ;
        GameObject uiPlayerHud = GameObject.Find("UI_PlayerHUD");
        skillCoolTimer[(int)ECoolTimer.AKey] = Utill.GetComponentInChildrenOrNull<UI_SkillCoolTimer>(uiPlayerHud, "AKeySkillCoolTimer"); 
        skillCoolTimer[(int)ECoolTimer.SKey] = Utill.GetComponentInChildrenOrNull<UI_SkillCoolTimer>(uiPlayerHud, "SKeySkillCoolTimer");
        skillCoolTimer[(int)ECoolTimer.CKey] = Utill.GetComponentInChildrenOrNull<UI_SkillCoolTimer>(uiPlayerHud, "PlayerRollCoolTimer");

        _skillSlots[(int)ECoolTimer.AKey] = uiPlayerHud.transform.Find("AKeySkillSlot").gameObject;
        _skillSlots[(int)ECoolTimer.SKey] = uiPlayerHud.transform.Find("SKeySkillSlot").gameObject;

        Managers.Input.KeyboardHandler -= OnSkillKeyDowned;
        Managers.Input.KeyboardHandler += OnSkillKeyDowned;
        Debug.Assert(skillCoolTimer[(int)ECoolTimer.AKey] != null && skillCoolTimer[(int)ECoolTimer.SKey] != null);
        Debug.Assert(_skillSlots[0] != null && _skillSlots[1] != null);
    }


    public BasePlayerSkillController GetSkill(ESkillType eType)
    {
        Debug.Assert(_skillDict[eType] != null);
        return _skillDict[eType];
    }

    public void Clear()
    {
        Managers.Input.KeyboardHandler -= OnSkillKeyDowned;
    }


    void OnSkillKeyDowned()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (_skillDict[ESkillType.Spawn_Panda].TryUseSkill())
            {
                skillCoolTimer[(int)ECoolTimer.AKey].StartCoolTime(_skillDict[ESkillType.Spawn_Panda].SkillCoolTimeInSec);
                Managers.Tween.StartUIScaleTW(_skillSlots[(int)ECoolTimer.AKey].transform, OnAKeyScaleTWEnd);
            }
            else
            {
                Managers.Tween.StartUIDoPunchPos(_skillSlots[(int)ECoolTimer.AKey].transform);
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_skillDict[ESkillType.Spawn_Reaper].TryUseSkill())
            {
                skillCoolTimer[(int)ECoolTimer.SKey].StartCoolTime(_skillDict[ESkillType.Spawn_Reaper].SkillCoolTimeInSec);
                Managers.Tween.StartUIScaleTW(_skillSlots[(int)ECoolTimer.SKey].transform, OnSKeyScaleTWEnd);
            }
            else
            {
                Managers.Tween.StartUIDoPunchPos(_skillSlots[(int)ECoolTimer.SKey].transform);
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_skillDict[ESkillType.Roll].TryUseSkill())
            {
                skillCoolTimer[(int)ECoolTimer.CKey].StartCoolTime(_skillDict[ESkillType.Roll].SkillCoolTimeInSec);
            }
            return;
        }
    }

    void OnAKeyScaleTWEnd()
    {
        Managers.Tween.EndToOneUIScaleTW(_skillSlots[(int)ECoolTimer.AKey].transform);
    }
    void OnSKeyScaleTWEnd()
    {
        Managers.Tween.EndToOneUIScaleTW(_skillSlots[(int)ECoolTimer.SKey].transform);
    }
}
