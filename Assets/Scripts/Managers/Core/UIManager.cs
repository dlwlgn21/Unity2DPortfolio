using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    UI_Statinfo _stat;
    UI_Inventory _inven;
    public void Init()
    {
        {
            // Stat
            GameObject go = Managers.Resources.Instantiate<GameObject>("Prefabs/UI/UI_Statinfo");
            Debug.Assert(go != null);
            Object.DontDestroyOnLoad(go);
            _stat = go.GetComponent<UI_Statinfo>();
            _stat.gameObject.SetActive(false);
        }
        {
            // Inven
            GameObject go = Managers.Resources.Instantiate<GameObject>("Prefabs/UI/UI_Inventory");
            Debug.Assert(go != null);
            Object.DontDestroyOnLoad(go);
            _inven = go.GetComponent<UI_Inventory>();
            _inven.gameObject.SetActive(false);
        }

        Managers.Input.KeyboardHandler += OnIKeyDowned;
    }
    
    public void OnIKeyDowned()
    {
        if (Managers.Scene.ECurrentScene != define.ESceneType.MAIN_MENU)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ShowStatIvenUI();
            }
        }
    }
    public void Clear()
    {
        Managers.Input.KeyboardHandler -= OnIKeyDowned;
    }
    void ShowStatIvenUI()
    {
        if (!_stat.gameObject.activeSelf)
        {
            _stat.gameObject.SetActive(true);
            _inven.gameObject.SetActive(true);
            _stat.RefreshUI();
            _inven.RefreshUI();

        }
        else
        {
            _stat.gameObject.SetActive(false);
            _inven.gameObject.SetActive(false);
        }
    }
}
