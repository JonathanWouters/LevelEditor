using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.DirectInput;
using System.Windows;
using System.Windows.Interop;
using SharpDX;
using System.Windows.Input;
using System.Windows.Controls;

namespace DirectxWpf.MVVM_Model.Managers
{
    public static class InputManager
    {
        //*******************************************************//
        //                      FIELDS                           //
        //*******************************************************//
        private static DirectInput _DirectInput;
        private static SharpDX.DirectInput.Mouse _Mouse;
        private static MouseState _MouseState;
        private static bool _LeftMouseDownOnLastFrame;
        private static bool _RightMouseDownOnLastFrame;
        private static bool _OnLeftMouseDown;
        private static bool _OnRightMouseDown;

        //*******************************************************//
        //                      PROPERTIES                       //
        //*******************************************************//
        public static bool IsInitialized { get; private set; }


        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public static void Initialize()
        {
            if (IsInitialized) return;
            
            _DirectInput = new DirectInput();
            _Mouse = new SharpDX.DirectInput.Mouse(_DirectInput);
            _Mouse.Properties.AxisMode = DeviceAxisMode.Relative;
            try
            {
                _Mouse.Acquire();
            }
            catch (SharpDX.SharpDXException)
            {
                Console.WriteLine("Error: Failed to aquire mouse !");
                return;
            }

            IsInitialized = true;
        }

        public static void Update()
        {
            if (!IsInitialized) return;

            _MouseState = _Mouse.GetCurrentState();


            if (IsLeftMouseButtonDown() && _LeftMouseDownOnLastFrame == false)
            {
                _OnLeftMouseDown = true;
                _LeftMouseDownOnLastFrame = true;
            }
            else if(IsLeftMouseButtonDown())
            {
                _OnLeftMouseDown = false;
                _LeftMouseDownOnLastFrame = true;
            }
            else
            {
                _OnLeftMouseDown = false;
                _LeftMouseDownOnLastFrame = false;
            }


            if (IsRightMouseButtonDown() && _RightMouseDownOnLastFrame == false)
            {
                _OnRightMouseDown = true;
                _RightMouseDownOnLastFrame = true;
            }
            else if (IsRightMouseButtonDown())
            {
                _OnRightMouseDown = false;
                _RightMouseDownOnLastFrame = true;
            }
            else
            {
                _OnRightMouseDown = false;
                _RightMouseDownOnLastFrame = false;
            }
        }

        public static  Vector2 GetMouseDirection()
        {
            if (!IsInitialized) return new Vector2(0,0);
            return new Vector2(_MouseState.X, _MouseState.Y);
        }
       
        public static bool IsLeftMouseButtonDown()
        {
            return _Mouse.GetCurrentState().Buttons[0];
        }
      
        public static bool OnLeftMouseButtonDown() 
        {
            return _OnLeftMouseDown;
        }
      
        public static bool OnRightMouseButtonDown()
        {
            return _OnRightMouseDown;
        }        
      
        public static bool IsRightMouseButtonDown() 
        {
            return _Mouse.GetCurrentState().Buttons[1];
        }
     
        public static System.Windows.Point GetMousePositionOnWindow()
        {
            return System.Windows.Input.Mouse.GetPosition(null);
        }
     
        public static System.Windows.Point GetMousePosition()
        {
            return new System.Windows.Point(System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y) ;
        }        
      
        public static bool IsKeyDown(System.Windows.Input.Key k)
        {
            return System.Windows.Input.Keyboard.IsKeyDown(k);
        }
      
        public static bool IsKeyUp(System.Windows.Input.Key k)
        {
            return System.Windows.Input.Keyboard.IsKeyUp(k);
        }
      
        public static bool IsKeyToggled(System.Windows.Input.Key k)
        {
            return System.Windows.Input.Keyboard.IsKeyToggled(k);
        }   



    }
}
