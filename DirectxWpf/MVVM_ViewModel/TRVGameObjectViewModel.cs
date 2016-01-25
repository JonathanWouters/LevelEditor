using DirectxWpf.Behavior;
using DirectxWpf.Helpers;
using DirectxWpf.MVVM_Model;
using DirectxWpf.MVVM_Model.Managers;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DirectxWpf.MVVM_ViewModel
{
    public class TRVGameObjectViewModel : BaseViewModel
    {
        //*******************************************************//
        //                      FIELDS                           //
        //*******************************************************//
        private ObservableCollection<TRVGameObjectElementViewModel> _root;


        //*******************************************************//
        //                      PROPERTIES                       //
        //*******************************************************//
        public ObservableCollection<TRVGameObjectElementViewModel> Root
        {
            get
            {
                if (_root == null)
                {
                    _root = new ObservableCollection<TRVGameObjectElementViewModel>();
                }
                return _root;
            }
        }


        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public TRVGameObjectViewModel() 
        { 
        
        }

        private void ChangeSelectedObject(RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue as TRVGameObjectElementViewModel != null)
            {
                if (GameObjectManager.Instance().SelectedObject != (e.NewValue as TRVGameObjectElementViewModel).GameObject)
                    GameObjectManager.Instance().SelectedObject = (e.NewValue as TRVGameObjectElementViewModel).GameObject;
            }
        }
        
        private void ReassignElements(TRVGameObjectElementViewModel element, TRVGameObjectElementViewModel newParent)
        {
            // remove the game object from its old parrent
            if (element.Parent != null)
            {
                element.Parent.RemoveChild(element);
            }
            else if (Root.Contains(element))
            {
                Root.Remove(element);
            }
            else
            {
                // element not found
                return;
            }

            // add the game object to its new parent
            if (newParent != null)
            {
                newParent.AddChild(element);
                OnElementReassigning(element.GameObject, newParent.GameObject);
            }
            else
            {
                Root.Add(element);
                OnElementReassigning(element.GameObject, null);
            }

        }

        public void OnElementReassigning(object source,ElementReassigningEventArgs e) 
        {
            ReassignElements(e.Element, e.NewParent);
        }

        public void OnGameObectAdded(object sender,GameObjectEventArgs e) 
        {
            TRVGameObjectElementViewModel element = new TRVGameObjectElementViewModel(e.GameObject);
            element.ElementReassigning += OnElementReassigning;
            Root.Add(element);

            foreach (var child in e.GameObject.Children)
            {
                AddChildToParrent(child, element);
            }
        }

        private void AddChildToParrent(GameObject gameObject,TRVGameObjectElementViewModel parent)
        {
            TRVGameObjectElementViewModel element = new TRVGameObjectElementViewModel(gameObject);
            element.ElementReassigning += OnElementReassigning;
            parent.AddChild(element);

            foreach (var child in gameObject.Children)
            {
                AddChildToParrent(child, element);
            }
        }

        public void OnGameObectRemoved(object sender, GameObjectEventArgs e)
        {
            Root.RemoveAll(element => (element.GameObject == e.GameObject));
            foreach (var child in Root)
            {
                RemoveInChildren(child,e.GameObject);
            } 
        }

        private void RemoveInChildren(TRVGameObjectElementViewModel childElement,GameObject gameObject) 
        {
            childElement.Children.RemoveAll(element => (element.GameObject == gameObject));
            
            foreach (var child in childElement.Children)
            {
                RemoveInChildren(child, gameObject);
            }        
        }

        //*******************************************************//
        //                      COMMANDS                         //
        //*******************************************************//
        private RelayCommand<RoutedPropertyChangedEventArgs<object>> _changeSelecedObjectCommand;
        public RelayCommand<RoutedPropertyChangedEventArgs<object>> ChangeSelecedObjectCommand
        {
            get { return _changeSelecedObjectCommand ?? (_changeSelecedObjectCommand = new RelayCommand<RoutedPropertyChangedEventArgs<object>>(ChangeSelectedObject)); }
        }

        //*******************************************************//
        //                      EVENTS                           //
        //*******************************************************//
        public event EventHandler<GOReassigningEventArgs> GameObjectReassigning;
        protected virtual void OnElementReassigning(GameObject gameObject, GameObject newParent)
        {
            if (GameObjectReassigning != null)
                GameObjectReassigning(this, new GOReassigningEventArgs() { GameObject = gameObject, NewParent = newParent });
        }
    }
}
