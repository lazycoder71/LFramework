﻿using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace LFramework
{
    public class UIButtonOpenPopup : UIButtonBase
    {
        [Title("Config")]
        [SerializeField] GameObject _popup;

        public event Action<UIPopupBehaviour> eventSpawnPopup;

        public override void Button_OnClick()
        {
            base.Button_OnClick();

            SpawnPopup();
        }

        protected virtual void HandleSpawnPopup(UIPopupBehaviour popupBehaviour)
        {

        }

        public UIPopupBehaviour SpawnPopup()
        {
            UIPopupBehaviour popup = UIPopupHelper.Create(_popup);

            eventSpawnPopup?.Invoke(popup);

            HandleSpawnPopup(popup);

            return popup;
        }
    }
}