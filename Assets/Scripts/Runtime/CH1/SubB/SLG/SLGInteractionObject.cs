using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLGDefines;
using Runtime.CH1.Main.Interface;
using System.Transactions;
using Yarn.Unity;
using Runtime.CH1.Main.Controller;
using Runtime.CH1.Main.Object;

public class SLGInteractionObject : InteractionObject
{
    private SLGObjectType type = SLGObjectType.WOOD;
    private SLGActionComponent _SLGAction;
    public bool _isActive = false;

    void Start()
    {
        _SLGAction = FindAnyObjectByType<SLGActionComponent>().GetComponent<SLGActionComponent>();
    }

    public void InitInteractionData(SLGObjectType InType)
    {
        type = InType;
        RefreshPopupIcon();
        _isActive = true;
    }
    private void RefreshPopupIcon()
    {
        if (_SLGAction != null)
        {
            Sprite currentSprite = _SLGAction.GetInteractionSprite(type);
            if (currentSprite != null)
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = currentSprite;
            }
        }
    }

    public bool Interact(Vector2 direction = default)
    {
        onInteract?.Invoke();
        if (_SLGAction != null)
        {
            _SLGAction.ProcessObjectInteraction(type);
        }
        return true;
    }
}
