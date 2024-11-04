using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MainMenuButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] EMainMenuButtonType _eType;

    public void OnSelect(BaseEventData eventData)
    {
        Managers.MainMenu.ECurrentSelectedButtonType = _eType;
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_MENU_MOVE_PATH);
    }
}
