using DirectxWpf.Effects;
using DirectxWpf.MVVM_Model.Components;
using SharpDX.Direct3D10;
using Format = SharpDX.DXGI.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SharpDX;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DirectxWpf.Helpers;
using Microsoft.Win32;
using System.IO;
using System.Xml;
using GalaSoft.MvvmLight.Command;

namespace DirectxWpf.MVVM_Model.Managers
{
    public class GameObjectManager:INotifyPropertyChanged
    {
        //**********************************//
        //            FIELDS                //
        //**********************************//
        private static GameObjectManager self;
        private GameObject _selectedObject;
        private ObservableCollection<GameObject> _objectList;

        //**********************************//
        //            PROPERTIES            //
        //**********************************//
        public bool IsInitialized { get; private set; }
        public GameObject SelectedObject
        {
            get
            {
                return _selectedObject;
            }
            set
            {
                if (_selectedObject != null)
                    _selectedObject.IsSelected = false;

                if (value != null)
                    value.IsSelected = true;

                _selectedObject = value;

                OnPropertyChanged("SelectedObject"); 
            }
        }
        public ObservableCollection<GameObject> ObjectList 
        { 
            get 
            { 
                return _objectList; 
            } 
            set 
            { 
                _objectList = value; OnPropertyChanged("ObjectList"); 
            }
        }
        public IEffect SelectedShader { get; set; }

        //**********************************//
        //            METHODS               //
        //**********************************//
        public static GameObjectManager Instance()
        {
            if (self == null)
                self = new GameObjectManager();
            return self;
        }

        private GameObjectManager() { IsInitialized = false; }

        public void Initialize(Device1 device)
        {
            if (IsInitialized) return;

            ObjectList = new ObservableCollection<GameObject>();

            SelectedShader = new PosNormColEffect();
            SelectedShader.Initialize(device);
            SelectedShader.Technique = SelectedShader.Effect.GetTechniqueByName("WireTech");

            IsInitialized = true;
        }
        
        public void AddGameObject(GameObject gameObject)
        {
            if (!IsInitialized) return;
            ObjectList.Add(gameObject);
            SelectedObject = gameObject;
            OnObjectAdded(gameObject);

            var UndoCommand = new RelayCommand<GameObject>(UndoAddGameObject);
            UndoRedoStack.AddUndoCommand(UndoCommand, gameObject);
        }

        private void UndoAddGameObject(GameObject gameObject)
        {
            if (!IsInitialized) return;

            if (ObjectList.Contains(gameObject))
            {
                ObjectList.Remove(gameObject);
                OnObjectRemoved(gameObject);
                SelectedObject = null;
                var RedoCommand = new RelayCommand<GameObject>(AddGameObject);
                UndoRedoStack.AddRedoCommand(RedoCommand, gameObject);
                return;
            }
            else
            {
                foreach (var obj in ObjectList)
                {
                    if (obj.RemoveChild(gameObject, true))
                    {
                        OnObjectRemoved(gameObject);
                        SelectedObject = null;
                        var RedoCommand = new RelayCommand<GameObject>(AddGameObject);
                        UndoRedoStack.AddRedoCommand(RedoCommand, gameObject);
                        return;
                    }
                }
            }
        }

        private void UndoRemoveObject(GameObject gameObject) 
        {
            if (!IsInitialized) return;
            ObjectList.Add(gameObject);
            SelectedObject = gameObject;
            OnObjectAdded(gameObject);

            var RedoCommand = new RelayCommand<GameObject>(RemoveGameObject);
            UndoRedoStack.AddRedoCommand(RedoCommand, gameObject);
        }
        
        public void RemoveGameObject(GameObject gameObject)
        {
            if (!IsInitialized) return;

            if (ObjectList.Contains(gameObject))
            {
                ObjectList.Remove(gameObject);
                OnObjectRemoved(gameObject);
                SelectedObject = null;

                var UndoCommand = new RelayCommand<GameObject>(UndoRemoveObject);
                UndoRedoStack.AddUndoCommand(UndoCommand, gameObject);
                return;
            }
            else
            {
                foreach (var obj in ObjectList)
                {
                    if (obj.RemoveChild(gameObject, true)) 
                    {
                        OnObjectRemoved(gameObject);
                        SelectedObject = null;
                        var UndoCommand = new RelayCommand<GameObject>(UndoRemoveObject);
                        UndoRedoStack.AddUndoCommand(UndoCommand, gameObject);
                        return;
                    }
                }
            }


        }

        public void Reassign(GameObject gameObject, GameObject newParent)
        {
            if (!IsInitialized) return;

            var oldParent = gameObject.ParentObject;
            // remove the game object from its old parrent
            if (gameObject.ParentObject != null)
            {
                gameObject.ParentObject.RemoveChild(gameObject);
            }
            else if (ObjectList.Contains(gameObject))
            {
                ObjectList.Remove(gameObject);
            }
            else
            {
                Console.WriteLine("GameObjectManager reassign: game object not found in manager");
                return;
            }

            // add the game object to its new parent
            if (newParent != null)
            {
                newParent.AddChild(gameObject);
            }
            else 
            {
                ObjectList.Add(gameObject);
            }

            SelectedObject = gameObject;

            GOReassigningEventArgs e = new GOReassigningEventArgs();
            e.GameObject = gameObject;
            e.NewParent = oldParent;
        }

        public void Clear() 
        {
            SelectedObject = null;
            foreach (var gameObject in ObjectList)
            {
                OnObjectRemoved(gameObject);
            }
            ObjectList.Clear();
        }

        public void Update(float deltaTime, Camera camera)
        {
            if (!IsInitialized) return;

            if (InputManager.IsKeyDown(Key.Delete))
            {
                if (SelectedObject != null)
                {                    
                    UndoRedoStack.ClearRedoStack();
                    RemoveGameObject(SelectedObject);
                }
            }

            foreach (var obj in ObjectList)
            {
                obj.Update(deltaTime, camera);
            }


            if (SelectedObject != null && SelectedShader != null)
            {

            }
        }

        public void Render(Device1 device,Camera camera)
        {
            if (!IsInitialized) return;

            foreach (var obj in ObjectList)
            {
                obj.Render(device);
            }

            RenderSelectedGameObject(SelectedObject,camera,device);
        }

        private void RenderSelectedGameObject(GameObject selectedObject,Camera camera,Device1 device) 
        {
            //Render the selected object
            if (selectedObject != null && SelectedShader != null)
            {
                if (!SelectedShader.IsInitialized)
                    SelectedShader.Initialize(device);

                SelectedShader.SetWorld(SelectedObject.Transform.World);
                SelectedShader.SetWorldViewProjection(selectedObject.Transform.World * camera.ViewMatrix * camera.ProjectionMatrix);

                ModelComponent modelComponent = selectedObject.GetComponent<ModelComponent>();
                if (modelComponent != null)
                {
                    device.InputAssembler.InputLayout = SelectedShader.InputLayout;
                    device.InputAssembler.PrimitiveTopology = modelComponent.PrimitiveTopology;
                    device.InputAssembler.SetIndexBuffer(modelComponent.IndexBuffer, Format.R32_UInt, 0);
                    device.InputAssembler.SetVertexBuffers(0,
                    new VertexBufferBinding(modelComponent.VertexBuffer, modelComponent.VertexStride, 0));

                    for (int i = 0; i < SelectedShader.Technique.Description.PassCount; ++i)
                    {
                        SelectedShader.Technique.GetPassByIndex(i).Apply();
                        device.DrawIndexed(modelComponent.IndexCount, 0, 0);
                    }
                }

                foreach (var child in selectedObject.Children)
                {
                    RenderSelectedGameObject(child, camera, device);
                }

            }
        }

        public void IsHitByMouseClick(System.Windows.Point mousePos, ViewportF viewFrustum, Camera camera)
        {
            if (!IsInitialized) return;
            
            float closestHit = float.MaxValue;
            foreach (GameObject obj in ObjectList)
            {
                float distance = float.MaxValue;


                List<ModelComponent> modelComponents = obj.GetAllComponentOfType<ModelComponent>();
                foreach (var modelComponent in modelComponents)
                {
                    if (modelComponent != null)
                    {
                        GameObject temp = modelComponent.IsHitByMouseClick(mousePos, viewFrustum, camera, out distance);
                        if (closestHit > distance)
                        {
                            closestHit = distance;
                            distance = float.MaxValue;
                            SelectedObject = temp;
                        }
                    }                    
                }

            }

        }

        public void OnReassigning(object source, GOReassigningEventArgs e)
        {
            Reassign(e.GameObject, e.NewParent);
        }

        public void OnAddingObject(object source, GameObjectEventArgs e)
        {
            UndoRedoStack.ClearRedoStack();
            AddGameObject(e.GameObject);
        }
        
        //**********************************//
        //            EVENTS                //
        //**********************************//
        
        public event EventHandler<GameObjectEventArgs> ObjectAdded;
        protected virtual void OnObjectAdded(GameObject gameObject)
        {
            if (ObjectAdded != null)
                ObjectAdded(this, new GameObjectEventArgs() { GameObject = gameObject });
        }


        public event EventHandler<GameObjectEventArgs> ObjectRemoved;
        protected virtual void OnObjectRemoved(GameObject gameObject)
        {
            if (ObjectRemoved != null)
                ObjectRemoved(this, new GameObjectEventArgs() { GameObject = gameObject });
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
