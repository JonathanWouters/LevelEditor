using DirectxWpf.Behavior;
using DirectxWpf.Helpers;
using DirectxWpf.MVVM_Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectxWpf.MVVM_ViewModel
{
    public class TRVGameObjectElementViewModel:BaseViewModel, IDropable,IDragable
    {
        //*******************************************************//
        //                      FIELDS                           //
        //*******************************************************//
        private GameObject _gameObject;
        private TRVGameObjectElementViewModel _parent;
        private ObservableCollection<TRVGameObjectElementViewModel> _children;


        //*******************************************************//
        //                      PROPERTIES                       //
        //*******************************************************//
        public TRVGameObjectElementViewModel Parent { get { return _parent; } private set { _parent = value; } }
        public GameObject GameObject { get { return _gameObject; } private set { _gameObject = value; OnPropertyChanged("GameObject"); } }
        public ObservableCollection<TRVGameObjectElementViewModel> Children
        {
            get
            {
                if (_children == null)
                    _children = new ObservableCollection<TRVGameObjectElementViewModel>();
                
                return _children;
            }
            private set
            {
                _children = value;
                OnPropertyChanged("Children");
            }
        }


        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public TRVGameObjectElementViewModel(GameObject obj,TRVGameObjectElementViewModel parent = null) 
        {
            _parent = parent;
            GameObject = obj;
        }

        public void AddChild(TRVGameObjectElementViewModel element) 
        {
            if (element.Parent != null)
            {
                if (element.Parent == this)
                    Console.WriteLine("GOElemenetView::AddChild > Element to add is already attached to this parent");
                else
                    Console.WriteLine("GOElemenetView::AddChild > Element to add is already attached to another GameObject. Detach it from it's current parent before attaching it to another one.");
                return;
            }
            Children.Add(element);
            element.Parent = this;
        }

        public void RemoveChild(TRVGameObjectElementViewModel element, bool searchChildren = true)
        {
            if (Children.Contains(element) )
            {
                Children.Remove(element);
                element.Parent = null;
                return;
            }

            if (searchChildren)
            {
                TryRemoveChild(element);
            }

        }

        private void TryRemoveChild(TRVGameObjectElementViewModel element)
        {
            foreach (TRVGameObjectElementViewModel child in Children)
            {
                if (child.Children.Contains(element))
                {
                    RemoveChild(element,false);
                    return;
                }

                child.TryRemoveChild(element);
            }
            return;
        }

        private List<TRVGameObjectElementViewModel> GetAllChildren() 
        {
            List<TRVGameObjectElementViewModel> allChildren = new List<TRVGameObjectElementViewModel>();

            foreach (var child in Children)
            {
                allChildren.Add(child);
                foreach (var child2 in child.GetAllChildren())
	            {
                    allChildren.Add(child2);
	            }
            }

            return allChildren;
        }


        // IDropable interface
        Type IDropable.DataType
        {
            get { return typeof(TRVGameObjectElementViewModel); }
        }
        void IDropable.Drop(object data)
        {
            TRVGameObjectElementViewModel dropedElement = data as TRVGameObjectElementViewModel;

            if (dropedElement != null)
            {
                //if dragged and dropped yourself, don't do anything
                if (dropedElement == this) 
                    return;
                
                //if dragged onto own Children, don't do anything
                if (dropedElement.GetAllChildren().Contains(this)) 
                    return;

                // Dragging on parent opbject
                if (this == dropedElement.Parent)
                {
                    OnElementReassigning(dropedElement, Parent);
                }
                // Dragging
                else 
                { 
                    OnElementReassigning(dropedElement, this);
                }
            }
        }

        // IIDragable interface
        Type IDragable.DataType
        {
            get { return typeof(TRVGameObjectElementViewModel); }
        }
        void IDragable.Remove(object i)
        {

        }


        //*******************************************************//
        //                      EVENTS                           //
        //*******************************************************//
        public event EventHandler<ElementReassigningEventArgs> ElementReassigning;
        protected virtual void OnElementReassigning(TRVGameObjectElementViewModel element, TRVGameObjectElementViewModel newParent)
        {
            if (ElementReassigning != null)
                ElementReassigning(this, new ElementReassigningEventArgs() { Element = element, NewParent = newParent });
        }
    }
}
