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
        protected SimpleCommand<T> ItemClickCommand;

        public ObservableList<T> Items
        {
            get { return this.m_Items; }
            set { this.Set(ref m_Items, value); }
        }
        private ObservableList<T> m_Items;

        public ListViewModel()
        {
            Items = new ObservableList<T>();
            ItemSelectCommand = new SimpleCommand<T>(OnItemSelect);
            ItemClickCommand = new SimpleCommand<T>(OnItemClick);
        }



        public T SelectedItem
        {
            get
            {
                return m_SelectedItem;
            }
            set
            {
                foreach (var i in Items)
                {
                    i.IsSelected = false;
                }
                Set(ref m_SelectedItem, value);
                if (m_SelectedItem != null)
                {
                    m_SelectedItem.IsSelected = true;
                }
                OnSelectedItemChanged();
            }
        }
        private T m_SelectedItem;


        protected virtual void OnItemClick(T item)
        {

        }

        private void OnItemSelect(T item)
        {
            Debug.Log(222); 
            SelectedItem = item;
            //item.IsSelected = !item.IsSelected;
            //if (item.IsSelected)
            //{
            //    foreach (var i in Items)
            //    {
            //        if (i == item)
            //            continue;
            //        i.IsSelected = false;
            //    }
            //}

            //if (item.IsSelected)
            //    this.SelectedItem = item;
        }

        protected virtual void OnSelectedItemChanged()
        {
        }
    }
}
