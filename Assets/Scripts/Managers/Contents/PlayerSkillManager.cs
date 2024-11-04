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
public sealed class PlayerSkillManager
{
    Dictionary<EActiveSkillType, Skill_BaseController> _skillControllerDict = new();
    UI_SkillCoolTimer[] skillCoolTimer = new UI_SkillCoolTimer[(int)ESkillSlot.Count];
    UI_Skill_Slot[] _skillSlots = new UI_Skill_Slot[2];
    PlayerStat _stat;
    public EActiveSkillType[] _eCurrSkillSlotType = new EActiveSkillType[2];
    public void Init()
    {
        if (_skillControllerDict.Count == 0)
        {
            // TODO : 나중에는 필요할 때 메모리에 로드 하는 방식으로 바꿔야함.
            #region SkillInit
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            _stat = player.GetComponent<PlayerStat>();


            Skill_BaseController roll = Utill.GetComponentInChildrenOrNull<Skill_RollController>(player, GetSkillControllerObjectName(EActiveSkillType.Roll));
            Skill_BaseController reaper = Inst<Skill_SpawnRepaerController>(Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Spawn_Reaper][0].controllerPrefabPath);
            Skill_BaseController panda = Inst<Skill_SpawnShooterController>(Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Spawn_Shooter][0].controllerPrefabPath);
            Skill_BaseController blackFlame = Inst<Skill_BlackFlameController>(Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Cast_BlackFlame][0].controllerPrefabPath);
            Skill_BaseController swordStrike = Inst<Skill_SwordStrikeController>(Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Cast_SwordStrike][0].controllerPrefabPath);

            reaper.gameObject.name = GetSkillControllerObjectName(EActiveSkillType.Spawn_Reaper);
            panda.gameObject.name = GetSkillControllerObjectName(EActiveSkillType.Spawn_Shooter);
            blackFlame.gameObject.name = GetSkillControllerObjectName(EActiveSkillType.Cast_BlackFlame);
            swordStrike.gameObject.name = GetSkillControllerObjectName(EActiveSkillType.Cast_SwordStrike);

            Object.DontDestroyOnLoad(reaper.gameObject);
            Object.DontDestroyOnLoad(panda.gameObject);
            Object.DontDestroyOnLoad(blackFlame.gameObject);
            Object.DontDestroyOnLoad(swordStrike.gameObject);

            _skillControllerDict.Add(EActiveSkillType.Roll, roll);
            _skillControllerDict.Add(EActiveSkillType.Spawn_Reaper, reaper);
            _skillControllerDict.Add(EActiveSkillType.Spawn_Shooter, panda);
            _skillControllerDict.Add(EActiveSkillType.Cast_BlackFlame, blackFlame);
            _skillControllerDict.Add(EActiveSkillType.Cast_SwordStrike, swordStrike);
            #endregion

            GameObject uiPlayerHud = GameObject.Find("UI_PlayerHUD");
            skillCoolTimer[(int)ESkillSlot.AKey] = Utill.GetComponentInChildrenOrNull<UI_SkillCoolTimer>(uiPlayerHud, "AKeySkillCoolTimer");
            skillCoolTimer[(int)ESkillSlot.SKey] = Utill.GetComponentInChildrenOrNull<UI_SkillCoolTimer>(uiPlayerHud, "SKeySkillCoolTimer");
            skillCoolTimer[(int)ESkillSlot.CKey] = Utill.GetComponentInChildrenOrNull<UI_SkillCoolTimer>(uiPlayerHud, "PlayerRollCoolTimer");

            _skillSlots[(int)ESkillSlot.AKey] = uiPlayerHud.transform.Find("AKeySkillSlot").gameObject.GetComponent<UI_Skill_Slot>();
            _skillSlots[(int)ESkillSlot.SKey] = uiPlayerHud.transform.Find("SKeySkillSlot").gameObject.GetComponent<UI_Skill_Slot>();
            _eCurrSkillSlotType[(int)ESkillSlot.AKey] = EActiveSkillType.Count;
            _eCurrSkillSlotType[(int)ESkillSlot.SKey] = EActiveSkillType.Count;

            Debug.Assert(skillCoolTimer[(int)ESkillSlot.AKey] != null && skillCoolTimer[(int)ESkillSlot.SKey] != null);
            Debug.Assert(_skillSlots[0] != null && _skillSlots[1] != null);


            #region EventSubscribe
            Managers.Input.KeyboardHandler -= OnSkillKeyDowned;
            Managers.Input.KeyboardHandler += OnSkillKeyDowned;
            UI_Skill_Slot.SkillIconDropEventHandler -= OnSkillIconDroped;
            UI_Skill_Slot.SkillIconDropEventHandler += OnSkillIconDroped;
            UI_Skill_Icon.SkillLevelUpEventHandler -= OnSkillLevelUp;
            UI_Skill_Icon.SkillLevelUpEventHandler += OnSkillLevelUp;
            #endregion
        }
        for (int i = 0; i < (int)ESkillSlot.CKey; ++i)
        {
            if (_eCurrSkillSlotType[i] != EActiveSkillType.Count)
                _skillControllerDict[_eCurrSkillSlotType[i]].InitForNextSceneLoad();
            skillCoolTimer[i].InitForNextSceneLoad();
        }
        _skillControllerDict[EActiveSkillType.Roll].InitForNextSceneLoad();
        skillCoolTimer[(int)ESkillSlot.CKey].InitForNextSceneLoad();
    }

    public data.SkillInfo GetCurrSkillLevelSkillInfo(EActiveSkillType eType)
    {
        return _skillControllerDict[eType].GetCurrLevelSkillInfo();
    }

    #region Event
    void OnSkillKeyDowned()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TryUseSkill(ESkillSlot.AKey, _eCurrSkillSlotType[(int)ESkillSlot.AKey]);
            return;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TryUseSkill(ESkillSlot.SKey, _eCurrSkillSlotType[(int)ESkillSlot.SKey]);
            return;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_skillControllerDict[EActiveSkillType.Roll].TryUseSkill())
            {
                skillCoolTimer[(int)ESkillSlot.CKey].StartCoolTime(_skillControllerDict[EActiveSkillType.Roll].SkillCoolTimeInSec);
            }
            return;
        }
    }

    bool TryUseSkill(ESkillSlot eSlot, EActiveSkillType eSkillType)
    {
        if (eSkillType != EActiveSkillType.Count && _skillControllerDict[eSkillType].TryUseSkill())
        {
            skillCoolTimer[(int)eSlot].StartCoolTime(_skillControllerDict[eSkillType].SkillCoolTimeInSec);
            Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_PLAYER_FLY_USING_SKILL);
            if (eSlot == ESkillSlot.AKey)
                Managers.Tween.StartUIScaleTW(_skillSlots[(int)eSlot].transform, OnAKeyScaleTWEnd);
            else
                Managers.Tween.StartUIScaleTW(_skillSlots[(int)eSlot].transform, OnSKeyScaleTWEnd);
            return true;
        }
        else
        {
            Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_UI_DENIED);
            Managers.Tween.StartUIDoPunchPos(_skillSlots[(int)eSlot].transform);
            return false;
        }
    }

    void OnSkillIconDroped(ESkillSlot eSlot, EActiveSkillType eType)
    {
        Debug.Assert(eSlot <= ESkillSlot.SKey && eType != EActiveSkillType.Count && eType != EActiveSkillType.Roll);
        _eCurrSkillSlotType[(int)eSlot] = eType;
    }


    void OnSkillLevelUp(EActiveSkillType eType, int skillLevel)
    {
        _skillControllerDict[eType].LevelUpSkill();
    }
    #endregion


    #region Helpers


    public string GetSkillObjectName(EActiveSkillType eType)
    {
        string path = Managers.Data.ActiveSkillInfoDict[eType][0].objectPrefabPath;
        return path.Substring(Managers.Data.ActiveSkillInfoDict[eType][0].objectPrefabPath.LastIndexOf('/') + 1);
    }

    public bool IsAandSSlotUsingAnySkill()
    {
        bool isASlotUsingSkill = false;
        bool isSSlotUsingSkill = false;
        if (_eCurrSkillSlotType[0] != EActiveSkillType.Count)
            isASlotUsingSkill = !_skillControllerDict[_eCurrSkillSlotType[(int)ESkillSlot.AKey]].IsCanUseSkillByCoolTime;
        if (_eCurrSkillSlotType[1] != EActiveSkillType.Count)
            isSSlotUsingSkill = !_skillControllerDict[_eCurrSkillSlotType[(int)ESkillSlot.SKey]].IsCanUseSkillByCoolTime;
        if (isASlotUsingSkill == false && isSSlotUsingSkill == false)
            return false;
        else
            return true;
    }

    public bool SwapIfSameNextToSlot(ESkillSlot eSlot, EActiveSkillType eSkillType)
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

    string GetSkillControllerObjectName(EActiveSkillType eType)
    {
        return Managers.Data.ActiveSkillInfoDict[eType][0].controllerPrefabPath.Substring(Managers.Data.ActiveSkillInfoDict[eType][0].controllerPrefabPath.LastIndexOf('/') + 1);
    }

    void SwapSlotSkillType()
    {
        EActiveSkillType tmpSkillType = _eCurrSkillSlotType[(int)ESkillSlot.AKey];
        _eCurrSkillSlotType[(int)ESkillSlot.AKey] = _eCurrSkillSlotType[(int)ESkillSlot.SKey];
        _eCurrSkillSlotType[(int)ESkillSlot.SKey] = tmpSkillType;
    }

    #endregion


    public void Clear()
    {

    }
}
