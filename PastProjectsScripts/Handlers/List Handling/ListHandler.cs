using UnityEngine;
using NS_CustomInspector;
using System.Collections.Generic;
using NS_ArrayExtensions;

namespace NS_Handlers.NS_ListHandling
{
    public interface IListHandler
    {
        void SelectSelectable(ListSelectableObject index);
        void UnSelectSelectable(ListSelectableObject index);
    }

    public class ListHandler<T> : EditorButtons, IListHandler where T : ListSelectableObject
    {
        protected List<T> m_selectables = new List<T>();
        protected List<T> m_selectablePooled = new List<T>();

        protected T m_selectedObject;
        [SerializeField]
        protected GameObject m_content;
        [SerializeField]
        GameObject m_itemPrefab;

        public event System.Action<T> OnSelect;

        public virtual void SelectSelectable(ListSelectableObject selectedObject)
        {
            if (m_selectedObject != null)
                m_selectedObject.interactable = true;

            if (selectedObject == m_selectedObject)
                m_selectedObject = null;
            else
            {
                m_selectedObject = (T)selectedObject;
                selectedObject.interactable = false;
            }

            if (OnSelect != null)
                OnSelect((T)m_selectedObject);
        }

        public void UnSelectSelectable(ListSelectableObject index)
        {
            if (index == m_selectedObject)
                m_selectedObject = null;
        }

        void UpdateSelectables()
        {
            m_selectables.Clear();

            if (m_content != null)
                m_selectables.AddRange(m_content.GetComponentsInChildren<T>());
        }

        public void CreateNewItem()
        {
            AddNewItem();
        }

        public T AddNewItem()
        {
            if (m_selectablePooled.Count > 0)
            {
                int lastIndex = m_selectablePooled.Count - 1;

                m_selectablePooled[lastIndex].gameObject.SetActive(true);
                m_selectables.Add(m_selectablePooled[lastIndex]);
                m_selectablePooled.RemoveAt(lastIndex);
            }
            else
            {
                T selectable = Instantiate(m_itemPrefab, Vector3.zero, Quaternion.identity, m_content.transform).GetComponent<T>();

                if (DebugTestLogger.TestValue(selectable))
                {               
                    m_selectables.Add(selectable);
                    selectable.SetListHandler(this, m_selectables.FindIndex( o => o == selectable));
                }
            }

            return m_selectables[m_selectables.Count - 1];
        }


        void DestroySelectables()
        {
            for (int i = 0; i < m_selectables.Count; i++)
                DestroyImmediate(m_selectables[i].gameObject);

            m_selectables.Clear();
        }

        public void DisableSelectables()
        {
            for (int i = 0; i < m_selectables.Count; i++)
                m_selectables[i].gameObject.SetActive(false);

            m_selectablePooled.AddRange(m_selectables.GetRange(0, m_selectables.Count));
            m_selectables.Clear();
        }

        public int GetSelectableCount()
        {
            return m_selectables.Count;
        }

        public override ButtonData[] GetButtonData()
        {
            return new ButtonData[]
            {
                new ButtonData(UpdateSelectables,"Update Selectables"),
                new ButtonData(CreateNewItem,"Create New Item"),
                new ButtonData(DisableSelectables,"Disable Selectables"),
                new ButtonData(DestroySelectables,"Destroy Selectables"),
            };
        }    
    }
}