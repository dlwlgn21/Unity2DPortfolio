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


    Dictionary<ESkillType, BasePlayerSkillController> _skillDict = new();
    UI_SkillCoolTimer[] skillCoolTimer = new UI_SkillCoolTimer[(int)ESkillSlot.Count];
    BasePlayerSkillController[] _currSkill = new BasePlayerSkillController[2];
    GameObject[] _skillSlots = new GameObject[2];
    public void Init()
    {
        // TODO : 나중에는 필요할 때 메모리에 로드 하는 방식으로 바꾸긴 해야함.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        BasePlayerSkillController roll = Utill.GetComponentInChildrenOrNull<PlayerSkillRollController>(player, "PlayerSkillRoll");
        BasePlayerSkillController reaper = Managers.Resources.Instantiate<PlayerSkillSpawnRepaerController>("Prefabs/Player/Skills/PlayerSkillSpawnReaper");
        BasePlayerSkillController panda = Managers.Resources.Instantiate<PlayerSkillSpawnPandaController>("Prefabs/Player/Skills/PlayerSkillSpawnPanda");
        BasePlayerSkillController blackFlame = Managers.Resources.Instantiate<PlayerSkillBlackFlameController>("Prefabs/Player/Skills/PlayerSkillBlackFlame");
        BasePlayerSkillController swordStrike = Managers.Resources.Instantiate<PlayerSkillSwordStrikeController>("Prefabs/Player/Skills/PlayerSkillSwordStrike");
        reaper.gameObject.name = "PlayerSkillSpawnReaper";
        panda.gameObject.name = "PlayerSkillSpawnPanda";
        blackFlame.gameObject.name = "PlayerSkillBlackFlame";
        swordStrike.gameObject.name = "PlayerSkillSwordStrike";
        Object.DontDestroyOnLoad(reaper.gameObject);
        Object.DontDestroyOnLoad(panda.gameObject);
        Object.DontDestroyOnLoad(blackFlame.gameObject);
        Object.DontDestroyOnLoad(swordStrike.gameObject);

        _skillDict.Add(ESkillType.Roll, roll);
        _skillDict.Add(ESkillType.Spawn_Reaper, reaper);
        _skillDict.Add(ESkillType.Spawn_Panda, panda);
        _skillDict.Add(ESkillType.Cast_BlackFlame, blackFlame); 
        _skillDict.Add(ESkillType.Cast_SwordStrike, swordStrike); 

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
}
