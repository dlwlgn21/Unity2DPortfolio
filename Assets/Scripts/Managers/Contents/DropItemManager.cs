using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemManager
{

    public void Init()
    {
        monster_states.Die.DieEventEnterStateHandler -= DropConsumableItem;
        monster_states.Die.DieEventEnterStateHandler += DropConsumableItem;
    }

    public void DropConsumableItem(NormalMonsterController mc)
    {
        if (Managers.Scene.ECurrentScene == define.ESceneType.Tutorial)
            return;

        if (Random.Range(0, 3) == 0)
        {
            GameObject go = Managers.Resources.Instantiate<GameObject>("Prefabs/Item/Comsumable/ConsumablePotion");
            Vector3 pos = mc.transform.position;
            go.transform.position = new Vector3(pos.x, pos.y + 0.6f, pos.z);
            Managers.Sound.Play(DataManager.SFX_UI_POINTER_ENTER);
            Managers.Tween.StartDoPunchPos(go.transform);
        }
    }

    public void Clear()
    {

    }

}
