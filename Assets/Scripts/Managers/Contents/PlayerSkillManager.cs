using define;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    UI_Skill_Slot[] _skillSlots = new UI_Skill_Slot[2];
    PlayerStat _stat;
    public ESkillType[] _eCurrSkillSlotType = new ESkillType[2];
    public void Init()
    {
        if (_skillDict.Count == 0)
        {
            #region SkillInit
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            _stat = player.GetComponent<PlayerStat>();

            Dictionary<int, data.SkillInfo> skillDict = Managers.Data.SkillInfoDict;

            Skill_BaseController roll = Utill.GetComponentInChildrenOrNull<Skill_RollController>(player, GetSkillControllerObjectName(ESkillType.Roll));
            Skill_BaseController reaper = Inst<Skill_SpawnRepaerController>(skillDict[(int)ESkillType.Spawn_Reaper_LV1].controllerPrefabPath);
            Skill_BaseController panda = Inst<Skill_SpawnShooterController>(skillDict[(int)ESkillType.Spawn_Shooter_LV1].controllerPrefabPath);
            Skill_BaseController blackFlame = Inst<Skill_BlackFlameController>(skillDict[(int)ESkillType.Cast_BlackFlame_LV1].controllerPrefabPath);
            Skill_BaseController swordStrike = Inst<Skill_SwordStrikeController>(skillDict[(int)ESkillType.Cast_SwordStrike_LV1].controllerPrefabPath);

            reaper.gameObject.name = GetSkillControllerObjectName(ESkillType.Spawn_Reaper_LV1);
            panda.gameObject.name = GetSkillControllerObjectName(ESkillType.Spawn_Shooter_LV1);
            blackFlame.gameObject.name = GetSkillControllerObjectName(ESkillType.Cast_BlackFlame_LV1);
            swordStrike.gameObject.name = GetSkillControllerObjectName(ESkillType.Cast_SwordStrike_LV1);

            Object.DontDestroyOnLoad(reaper.gameObject);
            Object.DontDestroyOnLoad(panda.gameObject);
            Object.DontDestroyOnLoad(blackFlame.gameObject);
            Object.DontDestroyOnLoad(swordStrike.gameObject);

            _skillDict.Add(ESkillType.Roll, roll);
            _skillDict.Add(ESkillType.Spawn_Reaper_LV1, reaper);
            _skillDict.Add(ESkillType.Spawn_Shooter_LV1, panda);
            _skillDict.Add(ESkillType.Cast_BlackFlame_LV1, blackFlame);
            _skillDict.Add(ESkillType.Cast_SwordStrike_LV1, swordStrike);

            #endregion

            GameObject uiPlayerHud = GameObject.Find("UI_PlayerHUD");
            skillCoolTimer[(int)ESkillSlot.AKey] = Utill.GetComponentInChildrenOrNull<UI_SkillCoolTimer>(uiPlayerHud, "AKeySkillCoolTimer");
            skillCoolTimer[(int)ESkillSlot.SKey] = Utill.GetComponentInChildrenOrNull<UI_SkillCoolTimer>(uiPlayerHud, "SKeySkillCoolTimer");
            skillCoolTimer[(int)ESkillSlot.CKey] = Utill.GetComponentInChildrenOrNull<UI_SkillCoolTimer>(uiPlayerHud, "PlayerRollCoolTimer");

            _skillSlots[(int)ESkillSlot.AKey] = uiPlayerHud.transform.Find("AKeySkillSlot").gameObject.GetComponent<UI_Skill_Slot>();
            _skillSlots[(int)ESkillSlot.SKey] = uiPlayerHud.transform.Find("SKeySkillSlot").gameObject.GetComponent<UI_Skill_Slot>();
            _eCurrSkillSlotType[(int)ESkillSlot.AKey] = ESkillType.Count;
            _eCurrSkillSlotType[(int)ESkillSlot.SKey] = ESkillType.Count;

            Debug.Assert(skillCoolTimer[(int)ESkillSlot.AKey] != null && skillCoolTimer[(int)ESkillSlot.SKey] != null);
            Debug.Assert(_skillSlots[0] != null && _skillSlots[1] != null);


            #region EventSubscribe
            Managers.Input.KeyboardHandler -= OnSkillKeyDowned;
            Managers.Input.KeyboardHandler += OnSkillKeyDowned;
            UI_Skill_Slot.OnSkillIocnDropEventHandler -= OnSkillIconDroped;
            UI_Skill_Slot.OnSkillIocnDropEventHandler += OnSkillIconDroped;
            UI_Skill_Icon.OnSkillLevelUpEventHandler -= OnSkillLevelUp;
            UI_Skill_Icon.OnSkillLevelUpEventHandler += OnSkillLevelUp;
            #endregion
        }
        // TODO : 나중에는 필요할 때 메모리에 로드 하는 방식으로 바꿔야함.
        for (int i = 0; i < 2; ++i)
        {
            if (_eCurrSkillSlotType[i] != ESkillType.Count)
                _skillDict[_eCurrSkillSlotType[i]].InitForNextSceneLoad();
            skillCoolTimer[i].InitForNextSceneLoad();
        }
    }

    #region Event
    void OnSkillKeyDowned()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UseSkill(ESkillSlot.AKey, _eCurrSkillSlotType[(int)ESkillSlot.AKey]);
            return;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            UseSkill(ESkillSlot.SKey, _eCurrSkillSlotType[(int)ESkillSlot.SKey]);
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
        if (eSkillType != ESkillType.Count &&  GetSkillOrNull(eSkillType).TryUseSkill())
        {
            skillCoolTimer[(int)eSlot].StartCoolTime(GetSkillOrNull(eSkillType).SkillCoolTimeInSec);
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

    void OnSkillIconDroped(ESkillSlot eSlot, ESkillType eType)
    {
        Debug.Assert(eSlot <= ESkillSlot.SKey && eType != ESkillType.Count && eType != ESkillType.Roll);
        _eCurrSkillSlotType[(int)eSlot] = eType;
    }


    void OnSkillLevelUp(ESkillType eType)
    {
        switch (eType)
        {
            case ESkillType.Spawn_Reaper_LV1:
            case ESkillType.Spawn_Reaper_LV2:
            case ESkillType.Spawn_Reaper_LV3:
                _skillDict[ESkillType.Spawn_Reaper_LV1].LevelUpSkill(eType);
                break;
            case ESkillType.Spawn_Shooter_LV1:
            case ESkillType.Spawn_Shooter_LV2:
            case ESkillType.Spawn_Shooter_LV3:
                _skillDict[ESkillType.Spawn_Shooter_LV1].LevelUpSkill(eType);
                break;
            case ESkillType.Cast_BlackFlame_LV1:
            case ESkillType.Cast_BlackFlame_LV2:
            case ESkillType.Cast_BlackFlame_LV3:
                _skillDict[ESkillType.Cast_BlackFlame_LV1].LevelUpSkill(eType);
                break;
            case ESkillType.Cast_SwordStrike_LV1:
            case ESkillType.Cast_SwordStrike_LV2:
            case ESkillType.Cast_SwordStrike_LV3:
                _skillDict[ESkillType.Cast_SwordStrike_LV1].LevelUpSkill(eType);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    #endregion


    #region Helpers

    Skill_BaseController GetSkillOrNull(ESkillType eType)
    {
        switch (eType)
        {
            case ESkillType.Spawn_Reaper_LV1:
            case ESkillType.Spawn_Reaper_LV2:
            case ESkillType.Spawn_Reaper_LV3:
                return _skillDict[ESkillType.Spawn_Reaper_LV1];
            case ESkillType.Spawn_Shooter_LV1:
            case ESkillType.Spawn_Shooter_LV2:
            case ESkillType.Spawn_Shooter_LV3:
                return _skillDict[ESkillType.Spawn_Shooter_LV1];
            case ESkillType.Cast_BlackFlame_LV1:
            case ESkillType.Cast_BlackFlame_LV2:
            case ESkillType.Cast_BlackFlame_LV3:
                return _skillDict[ESkillType.Cast_BlackFlame_LV1];
            case ESkillType.Cast_SwordStrike_LV1:
            case ESkillType.Cast_SwordStrike_LV2:
            case ESkillType.Cast_SwordStrike_LV3:
                return _skillDict[ESkillType.Cast_SwordStrike_LV1];
            default:
                Debug.Assert(false);
                return null;
        }
    }

    public string GetSkillObjectName(ESkillType eType)
    {
        return Managers.Data.SkillInfoDict[(int)eType].objectPrefabPath.Substring(Managers.Data.SkillInfoDict[(int)eType].objectPrefabPath.LastIndexOf('/') + 1);
    }

    public bool IsAandSSlotUsingAnySkill()
    {
        bool isASlotUsingSkill = false;
        bool isSSlotUsingSkill = false;
        if (_eCurrSkillSlotType[0] != ESkillType.Count)
            isASlotUsingSkill = !_skillDict[_eCurrSkillSlotType[(int)ESkillSlot.AKey]].IsCanUseSkillByCoolTime;
        if (_eCurrSkillSlotType[1] != ESkillType.Count)
            isSSlotUsingSkill = !_skillDict[_eCurrSkillSlotType[(int)ESkillSlot.SKey]].IsCanUseSkillByCoolTime;
        Debug.Log($"A Slot using skill? {isASlotUsingSkill},\nS Slot using skill? {isSSlotUsingSkill}");
        if (isASlotUsingSkill == false && isSSlotUsingSkill == false)
            return false;
        else
            return true;
    }

    public bool SwapIfSameNextToSlot(ESkillSlot eSlot, ESkillType eSkillType)
    {
        if (eSlot == ESkillSlot.AKey)
        {
            if (_eCurrSkillSlotType[(int)ESkillSlot.SKey] == eSkillType)
            {
                SwapSlotSkillType();
                _skillSlots[(int)ESkillSlot.AKey].SwapIcon(_skillSlots[(int)ESkillSlot.SKey]);
                return true;
            }
            else
                return false;
        }
        else
        {
            if (_eCurrSkillSlotType[(int)ESkillSlot.AKey] == eSkillType)
            {
                SwapSlotSkillType();
                _skillSlots[(int)ESkillSlot.SKey].SwapIcon(_skillSlots[(int)ESkillSlot.AKey]);
                return true;
            }
            else
                return false;
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

    void SwapSlotSkillType()
    {
        ESkillType tmpSkillType = _eCurrSkillSlotType[(int)ESkillSlot.AKey];
        _eCurrSkillSlotType[(int)ESkillSlot.AKey] = _eCurrSkillSlotType[(int)ESkillSlot.SKey];
        _eCurrSkillSlotType[(int)ESkillSlot.SKey] = tmpSkillType;

        //Debug.Log($"A:{_eCurrSkillSlotType[(int)ESkillSlot.AKey]}, S:{_eCurrSkillSlotType[(int)ESkillSlot.SKey]}");
    }

    #endregion


    public void Clear()
    {

    }
}
