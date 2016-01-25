using DirectxWpf.MVVM_Model.Components;
using SharpDX;
using SharpDX.Direct3D10;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DirectxWpf.MVVM_Model
{
    public class GameObject : INotifyPropertyChanged, ICloneable
    {
        
        //*******************************************************//
        //                      FIELDS                           //
        //*******************************************************//
        private string _Name;
        private bool _IsSelected;
        private ObservableCollection<GameObject> _Children;
        private GameObject _ParentObject;
        private TransformComponent _Transform;
        private List<IBaseComponent> _Components;


        //*******************************************************//
        //                      PROPERTIES                       //
        //*******************************************************//
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        public ObservableCollection<GameObject> Children
        {
            get{return _Children; }
            private set{_Children = value; }
        }
        public GameObject ParentObject{
            get
            {
                return _ParentObject;    
            }
            set 
            {
                _ParentObject = value;
            }
        }
        public TransformComponent Transform 
        {
            get { return _Transform; } 
            set{ _Transform = value; }
        
        }
        public List<IBaseComponent> Components 
        { 
            get { return _Components; } 
            private set { _Components = value; } 
        }


        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//     
        public GameObject() 
        {
            _Children = new ObservableCollection<GameObject>();
            Name = "Unnamed";
            _Components = new List<IBaseComponent>();

            
            _Transform = new TransformComponent();
            AddComponent(_Transform);
        }

        public void AddComponent(IBaseComponent component)
        {
            component.GameObject = this;
            _Components.Add(component);
        }

        public void RemoveComponent() 
        {
            throw new NotImplementedException();
        }
        
        public T GetComponent<T>() where T : class
        {
            foreach (var component in _Components)
	        {
		        if( component.GetType() == typeof(T))
                {
                    return (T)component;   
                }
	        }

            return null;
        }

        public List<T> GetAllComponentOfType<T>(bool searchInChildren = true) where T : class
        {
            List<T> list = new List<T>();
            
            foreach (var component in _Components)
            {
                if (component.GetType() == typeof(T))
                {
                    list.Add( (T)component );
                }
            }

            if (searchInChildren)
            {
                foreach (var child in Children)
                {
                    list.AddRange( child.GetAllComponentOfType<T>() );
                }                
            }

            return list;
        }

        public void AddChild(GameObject obj, bool convertToParentSpace = true) 
        {
	        if(obj.ParentObject != null)
	        {
                if (obj.ParentObject == this)
                    Console.WriteLine("GameObject::AddChild > GameObject to add is already attached to this parent");
                    //Logger::LogWarning(L"GameObject::AddChild > GameObject to add is already attached to this parent");
                else
                    Console.WriteLine("GameObject::AddChild > GameObject to add is already attached to another GameObject. Detach it from it's current parent before attaching it to another one.");
                    //Logger::LogWarning(L"GameObject::AddChild > GameObject to add is already attached to another GameObject. Detach it from it's current parent before attaching it to another one.");
                return;
	        }
            _Children.Add(obj);

            if (convertToParentSpace)
            {
                obj.Transform.ConvertToParentSpace(Transform);
            }
            obj.ParentObject = this;
        }

        public bool RemoveChild(GameObject obj, bool searchChildren = true) 
        {
            if (_Children.Contains(obj))
            {
                _Children.Remove(obj);
                obj.Transform.ConvertToLocalSpace(Transform);
                obj.ParentObject = null;
                return true;
            }
 
            if (searchChildren)
            {
                foreach (var child in _Children)
                {
                    if (child.RemoveChild(obj))
                        return true;
                }
            }

            return false;
        }

        private bool TryRemoveChild(GameObject gameobject) 
        {
            foreach (GameObject child in Children)
            {
                if (child.Children.Contains(gameobject)) 
                {
                    
                    return RemoveChild(gameobject,false);
                }

                if (child.TryRemoveChild(gameobject)) 
                {
                    return true;
                };               
            }
            return false;
        }

        public void Update(float deltaTime,Camera camera) 
        {
            //Update Components
            foreach (var component in _Components)
            {
                component.Update(deltaTime,camera);
            }
            //Update Children
            foreach (var child in _Children) 
            {
                child.Update(deltaTime,camera);
            }
        }

        public void Render(Device1 device) 
        {
            //Render Components
            foreach (var component in _Components)
	        {
		        component.Render(device);
	        }

            //Render Children
            foreach (var child in _Children)
            {
                child.Render(device);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        //*******************************************************//
        //                      EVENTS                           //
        //*******************************************************//
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
