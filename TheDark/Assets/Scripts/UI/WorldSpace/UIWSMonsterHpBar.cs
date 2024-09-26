using DG.Tweening;
using UnityEngine;

public class UIWSMonsterHpBar : UIHealthBar
{
    private const float SCALE_TW_DURATION = 0.2f;
    private Vector3 _originalLocalScale;
    private RectTransform _rectTransform;
    private Vector3 _originalRectTransformScale;
    private BaseMonsterController _mc;

    private void Start()
    {
        SetFullHpBarRatio();
        if (_rectTransform == null)
        {
            AssginComponentsAndInitVariables();
        }
    }

    private void OnEnable()
    {
        BaseMonsterController.HittedByNormalAttackWSUIEventHandler += OnMonsterHittedByPlayerNormalAttack;
        monster_states.Die.MonsterDieEventHandelr += OnMonsterDie;
    }


    private void OnDestroy()
    {
        BaseMonsterController.HittedByNormalAttackWSUIEventHandler -= OnMonsterHittedByPlayerNormalAttack;
        monster_states.Die.MonsterDieEventHandelr -= OnMonsterDie;
    }
    public void OnMonsterInit()
    {
        Init();
        InitScale();
    }

    public override void Init()
    {
        if (_rectTransform == null)
        {
            AssginComponentsAndInitVariables();
        }
        SetFullHpBarRatio();
    }

    private void Update()
    {
        if (transform.parent.localRotation.eulerAngles.y > 0f)
        {
            transform.localScale = new Vector3(-1f, _originalLocalScale.y, _originalLocalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(1f, _originalLocalScale.y, _originalLocalScale.z);
        }
    }

    public void OnMonsterDie(BaseMonsterController mc)
    {
        if (mc == _mc)
        {
            _rectTransform.DOScale(0f, SCALE_TW_DURATION).SetEase(Ease.OutElastic);
        }
    }
    public void OnMonsterHittedByPlayerNormalAttack(int damage, int beforeDamageHP, int afterDamageHP)
    {
        if (_mc.IsHittedByPlayerNormalAttack)
        {
            DecraseHP(beforeDamageHP, afterDamageHP);
        }
    }

    private void AssginComponentsAndInitVariables()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalLocalScale = transform.localScale;
        _originalRectTransformScale = _rectTransform.localScale;
        _rectTransform.localScale = _originalRectTransformScale;
        _mc = transform.parent.gameObject.GetComponent<BaseMonsterController>();
    }

    private void InitScale()
    {
        transform.localScale = _originalLocalScale;
    }
}
