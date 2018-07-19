using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NS_Handlers.NS_ListHandling
{
    public class ListSelectableObject : UnityEngine.UI.Selectable
    {
        IListHandler m_listHandler;
        [SerializeField]
        int m_index;

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (eventData.IsScrolling() == false && eventData.dragging == false && eventData.button == PointerEventData.InputButton.Left)
            {
                if (m_listHandler != null)
                    m_listHandler.SelectSelectable(this);
            }
        }

        internal void SetListHandler(IListHandler listHandler, int i) 
        {
            m_listHandler = listHandler;
            m_index = i;
        }

        protected override void OnDisable()
        {
            m_listHandler.UnSelectSelectable(this);
        }

        protected override void OnDestroy()
        {
            m_listHandler.UnSelectSelectable(this);
            base.OnDestroy();
        }

        public void SetInteractable(bool isInteractable)
        {
            this.interactable = isInteractable;
        }
    }
}