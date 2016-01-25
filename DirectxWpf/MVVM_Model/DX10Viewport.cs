using System.Windows;
using DaeSharpWpf;
using DirectxWpf.Effects;
using DirectxWpf.MVVM_Model.Models;
using SharpDX;
using SharpDX.Direct3D10;
using Format = SharpDX.DXGI.Format;
using System;
using System.Windows.Interop;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DirectxWpf.MVVM_Model;
using DirectxWpf.MVVM_Model.Components;
using GalaSoft.MvvmLight.Command;
using DirectxWpf.MVVM_ViewModel;
using DirectxWpf.MVVM_Model.Managers;
using InputManager = DirectxWpf.MVVM_Model.Managers.InputManager;

namespace DirectxWpf.MVVM_Model
{
    public class DX10Viewport : IDX10Viewport, INotifyPropertyChanged
    {
        //*******************************************************//
        //                      FIELDS                           //
        //*******************************************************//
        private Device1 _Device;
        private RenderTargetView _RenderTargetView;
        private DX10RenderCanvas _RenderControl;
        private Camera _Camera;
        private Gizmo _Gizmo;


        //*******************************************************//
        //                     PROPERTIES                        //
        //*******************************************************//
        public Camera Camera { get { return _Camera; } private set { _Camera = value; } }
        public Gizmo Gizmo { get { return _Gizmo; } private set { _Gizmo = value; } }


        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public void Initialize(Device1 device, RenderTargetView renderTarget, DX10RenderCanvas canvasControl)
        {
            _Device = device;

            _RenderTargetView = renderTarget;
            _RenderControl = canvasControl;
            _Camera = new Camera();
            _Gizmo = new Gizmo(_Device);
            
            GameObjectManager.Instance().Initialize(device);
            InputManager.Initialize();

        }

        public void Deinitialize()
        {

        }

        public void Update(float deltaT)
        {            
            InputManager.Update();
            _Camera.Update(deltaT,(float)_RenderControl.ActualWidth, (float)_RenderControl.ActualHeight);

            //Get the position of the mouse on the rendercanvas
            System.Windows.Point mousePos = _RenderControl.PointFromScreen(InputManager.GetMousePosition());
            
            // Create a viewportfrustum for Ray.GetPickRay
            ViewportF viewFrustum = new ViewportF(0, 0, (float)(_RenderControl.ActualWidth), (float)(_RenderControl.ActualHeight), Camera.NearPlane, 2);

            HandlePicking(viewFrustum, mousePos);

            if (GameObjectManager.Instance().SelectedObject != null)
            {
                _Gizmo.Update(_Camera, viewFrustum, mousePos, GameObjectManager.Instance().SelectedObject);
                _Gizmo.Position = GameObjectManager.Instance().SelectedObject.Transform.WorldPosition.Vector3;                
            }

            GameObjectManager.Instance().Update(deltaT, Camera);

        }

        public void Render(float deltaT)
        {
            if (_Device == null)
                return;

            GameObjectManager.Instance().Render(_Device,Camera);
  
            // render the Gizmo
            if (GameObjectManager.Instance().SelectedObject != null)
            {
                _RenderControl.ClearDepthStensil();
                _Gizmo.Render(_Device);
            }
        }

        private bool IsMouseInViewport() 
        {
            var pos = _RenderControl.PointFromScreen( InputManager.GetMousePosition() );
            if( pos.X >= 0 && 
                pos.X <= _RenderControl.ActualWidth &&
                pos.Y >= 0 &&
                pos.Y <= _RenderControl.ActualHeight
                )
            {
                return true;
            }
            return false;
            
        }

        private void HandlePicking(ViewportF viewFrustum, System.Windows.Point mousePos)
        {
            if (InputManager.OnLeftMouseButtonDown() && IsMouseInViewport())
            {
                //Check if the gizmo is picked
                if (GameObjectManager.Instance().SelectedObject != null)
                {
                   if(_Gizmo.IsHit(_Camera, viewFrustum, mousePos)) 
                       return;
                }

                GameObjectManager.Instance().IsHitByMouseClick(mousePos, viewFrustum, Camera);
            }
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
