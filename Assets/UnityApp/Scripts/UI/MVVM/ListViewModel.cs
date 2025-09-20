using Loxodon.Framework.Commands;
using Loxodon.Framework.Observables;
using Loxodon.Framework.ViewModels;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class ListViewModel<T> : ViewModelBase where T : SelectableItemViewModel
    {
        protected SimpleCommand<T> ItemSelectCommand;

        public ObservableList<T> Items
        {
            get { return this.m_Items; }
            set { this.Set(ref m_Items, value); }
        }
        private ObservableList<T> m_Items;

        public T SelectedItem
        {
            get
            {
                return m_SelectedItem;
            }
            set
            {
                Set(ref m_SelectedItem, value);
            }
        }
        private T m_SelectedItem;

        public ListViewModel()
        {
            Items = new ObservableList<T>();
            ItemSelectCommand = new SimpleCommand<T>(OnItemSelect);
        }

        protected virtual void OnItemSelect(T item)
        {
            item.IsSelected = !item.IsSelected;
            if (item.IsSelected)
            {
                foreach (var i in Items)
                {
                    if (i == item)
                        continue;
                    i.IsSelected = false;
                }
            }

            if (item.IsSelected)
                this.SelectedItem = item;
        }

        public void SetSelectItem(T item)
        {
            foreach (var i in Items)
            {
                i.IsSelected = false;
            }
            if (item != null)
            {
                item.IsSelected = true;
            }
            SelectedItem = item;
        }
    }
}
