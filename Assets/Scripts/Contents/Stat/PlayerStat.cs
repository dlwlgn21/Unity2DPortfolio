using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class PlayerStat : BaseStat
{
    static public UnityAction<int> LevelUpEventHandler;
    static public UnityAction<int, int> AddExpEventHandler;
    static public UnityAction<int, int> ManaChangedEventHandler;
    static public UnityAction PlayerDieHandler;
    static public UnityAction<int, int> PlayerHpIncreaseEventHandler;
    [SerializeField] int _level;
    [SerializeField] int _exp;
    [SerializeField] int _gold;
    [SerializeField] int _mana;
    // 24.10.3 Item 착용위해 추가.
    [SerializeField] int _swordPlusDamage;
    [SerializeField] int _helmetPlusDefence;
    [SerializeField] int _armorPlusDefence;

    public int Level { get { return _level; } set { _level = value; } }

    public int Mana 
    { 
        get { return _mana; }
        set 
        {
            int beforeMana = _mana;
            _mana = Mathf.Clamp(value, 0, MaxMana);
            if (ManaChangedEventHandler != null)
                ManaChangedEventHandler.Invoke(beforeMana, _mana);
        }
    }

    public int MaxMana { get; private set; }

    public int Exp 
    { 
        get { return _exp; } 
        set 
        {
            _exp = value;
            int currNeedLevelUpExp = Managers.Data.PlayerStatDict[Level].totalExp;
            bool isLevelUp = false;
            int levelUpCount = 0;
            int sentinelValue = 0; // ForDebuging
            while (_exp >= currNeedLevelUpExp)
            {
                ++Level;
                ++sentinelValue;
                if (sentinelValue > 1)
                {
                    Debug.DebugBreak();
                }
                ++levelUpCount;
                isLevelUp = true;
                _exp -= currNeedLevelUpExp;
                if (_exp <= 0)
                {
                    _exp = 0;
                    break;
                }
                currNeedLevelUpExp = Managers.Data.PlayerStatDict[Level].totalExp;
            }

            if (AddExpEventHandler != null)
            {
                if (isLevelUp)
                    AddExpEventHandler.Invoke(currNeedLevelUpExp, currNeedLevelUpExp);
                else
                    AddExpEventHandler.Invoke(Exp, currNeedLevelUpExp);
            }

            if (isLevelUp)
            {
                if (LevelUpEventHandler != null)
                    LevelUpEventHandler.Invoke(levelUpCount);
            }
        } 
    }

    public int Gold { get { return _gold; } set { _gold = value; } }



    // 24.10.3 Item 착용위해 추가.
    public int SwordPlusDamage { get { return _swordPlusDamage; } set { _swordPlusDamage = value; } }
    public int HelmetPlusDefence { get { return _helmetPlusDefence; } set { _helmetPlusDefence = value; } }
    public int ArmorPlusDefence { get { return _armorPlusDefence; } set { _armorPlusDefence = value; } }

    public override int DecreaseHpAndGetActualDamageAmount(int damage, out int beforeDamageHp, out int afterDamageHp)
    {
        if (HP <= 0)
        {
            beforeDamageHp = 0;
            afterDamageHp = 0;
            return 0;
        }

        if (damage == 0)
        {
            if (damage == 0)
            {
                beforeDamageHp = HP;
                afterDamageHp = HP;
                return 0;
            }
        }

        int actualDamage = Mathf.Max(1, damage - (Defence + HelmetPlusDefence + ArmorPlusDefence));
        beforeDamageHp = HP;
        HP -= actualDamage;
        afterDamageHp = HP;
        if (HP <= 0)
        {
            HP = 0;
            if (PlayerDieHandler != null)
                PlayerDieHandler.Invoke();
        }
        return actualDamage;
    }

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        var dict = Managers.Data.PlayerStatDict;
        Level = dict[1].level;
        HP = dict[1].maxHp;
        MaxHP = dict[1].maxHp;
        Attack = dict[1].attack;
        Defence = dict[1].defence;
        MoveSpeed = dict[1].moveSpeed;
        MaxMana = dict[1].maxMana;
        Mana = MaxMana;
        Exp = 0;
        Gold = 0;
        SwordPlusDamage = 0;
        HelmetPlusDefence = 0;
        ArmorPlusDefence = 0;
    }

    public void IncreaseHp(int amount)
    {
        if (HP >= MaxHP)
            return;
        PlayerHpIncreaseEventHandler.Invoke(HP, Mathf.Min(HP + amount, MaxHP));
        HP = Mathf.Min(HP + amount, MaxHP);
    }

    public void InitHP()
    {
        HP = MaxHP;
        PlayerHpIncreaseEventHandler.Invoke(HP, MaxHP);
    }

    private void OnDestroy()
    {
        LevelUpEventHandler = null;
        AddExpEventHandler = null;
        ManaChangedEventHandler = null;
        PlayerDieHandler = null;
    }
}
