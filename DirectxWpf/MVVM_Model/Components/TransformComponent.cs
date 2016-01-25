using GalaSoft.MvvmLight.Command;
using SharpDX;
using SharpDX.Direct3D10;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectxWpf.MVVM_Model.Components
{
    public class TransformComponent : IBaseComponent
    {

        //**********************************//
        //            FIELDS                //
        //**********************************//
        private MyVector3 _position, _worldPosition,_scale, _worldScale, _forward, _up, _right;
        private Quaternion _rotation, _worldRotation;
        private Matrix _world;

        //**********************************//
        //            PROPERTIES            //
        //**********************************//
        public GameObject GameObject { get; set; }
        public MyVector3 Position 
        { 
            get
            {
                return _position;
            } 
            set
            {
                _position = value;
            } 
        }
        public MyVector3 WorldPosition
        {
            get
            {
                return _worldPosition;
            }
            private set
            {
                _worldPosition = value;
            }
        }
        public MyVector3 Scale 
        { 
            get
            {
                return _scale; 
            } 
            set
            { 
                _scale = value;
            } 
        }
        public MyVector3 WorldScale
        {
            get 
            { 
                return _worldScale; 
            }
            private set 
            { 
                _worldScale = value; 
            }
        }
        public Quaternion Rotation 
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }
        public MyVector3 RotationEuler{get; set;}
        public MyVector3 WorldRotationEuler { get; set; }
        public Quaternion WorldRotation
        {
            get
            {
                return _worldRotation;
            }
            set
            {
                _worldRotation = value;
            }
        }
        public MyVector3 Forward
        {
            get { return _forward; }
            set { _forward = value; }
        }
        public MyVector3 Up 
        {
            get { return _up; }
            set { _up = value; }        
        }
        public MyVector3 Right
        {
            get { return _right; }
            set { _right = value; }
        }
        public Matrix World 
        {
            get {return _world; }
            private set{ _world = value; }
        }

        //**********************************//
        //            METHODS               //
        //**********************************//
        public TransformComponent() 
        {
            Position = new MyVector3(0, 0, 0);
            WorldPosition = new MyVector3(0, 0, 0);

            Scale = new MyVector3(1, 1, 1);
            WorldScale = new MyVector3(1, 1, 1);
            
            Rotation =  new Quaternion(0,0,0,1);
            WorldRotation = new Quaternion(0, 0, 0,1);
            RotationEuler = new MyVector3(0, 0, 0);
            WorldRotationEuler = new MyVector3(0, 0, 0);

            Forward = new MyVector3(0, 0, 1);
            Up =  new MyVector3(0, 1, 0);
            Right = new MyVector3(1, 0, 0);

            //Update();
        }

        public TransformComponent GetTransform()
        {
	        if(GameObject == null)
	        {
		        Console.WriteLine("BaseComponent::GetTransform() > Failed to retrieve the TransformComponent. GameObject is NULL.");
		        return null;
	        }
            return GameObject.Transform;
        }

        public void Update(float deltaTime, Camera camera)
        {
            UpdateTransforms();
        }

        private void UpdateTransforms() 
        {
            //Calculate World Matrix
            //**********************
            Rotation = Quaternion.RotationYawPitchRoll(MathUtil.DegreesToRadians(RotationEuler.Y),
                                                       MathUtil.DegreesToRadians(RotationEuler.X),
                                                       MathUtil.DegreesToRadians(RotationEuler.Z));

            World = Matrix.Scaling(Scale.Vector3) *
                    Matrix.RotationQuaternion(Rotation) *
                    Matrix.Translation(Position.Vector3);

            WorldRotationEuler.Vector3 = RotationEuler.Vector3;

            if (GameObject != null)
            {
                if (GameObject.ParentObject != null)
                {
                    var parentWorld = GameObject.ParentObject.Transform.World;
                    World *= parentWorld;

                    var parentWorldRotationEuler = GameObject.ParentObject.Transform.World;
                    WorldRotationEuler.Vector3 += GameObject.ParentObject.Transform.WorldRotationEuler.Vector3;
                }                
            }


            //Get World Transform
            Vector3 pos, scale;
            Quaternion rot;
            if ( World.Decompose(out scale, out rot, out pos) )
            {
                WorldPosition.Vector3 = pos;
                WorldScale.Vector3 = scale;
                WorldRotation = rot;
            }

            Matrix rotMat = Matrix.RotationQuaternion(rot);
            Forward.Vector3 = Vector3.TransformCoordinate(Vector3.ForwardLH, rotMat);
            Right.Vector3 = Vector3.TransformCoordinate(Vector3.Right, rotMat);
            Up.Vector3 = Vector3.TransformCoordinate(Vector3.Up, rotMat);        
        }

        public void Render(Device1 device)
        {
            //Noting to draw
        }

        public void ConvertToParentSpace(TransformComponent parent)
        {
            var invParentWorld = Matrix.Invert(parent.World);
            Scale.Vector3 /= parent.WorldScale.Vector3;
            RotationEuler.Vector3 -= parent.WorldRotationEuler.Vector3;
            Position.Vector3 = Vector3.TransformCoordinate(Position.Vector3, invParentWorld);
        }

        public void ConvertToLocalSpace(TransformComponent parent)
        {
            Scale.Vector3 *= parent.WorldScale.Vector3;
            RotationEuler.Vector3 += parent.WorldRotationEuler.Vector3;
            Position.Vector3 = Vector3.TransformCoordinate(Position.Vector3, parent.World);
        }

        public void SetPosition(MyVector3 position) 
        {
            var UndoCommand = new RelayCommand<MyVector3>(undoSetPosition);
            MyVector3 parameter = new MyVector3();
            parameter.Vector3 = Position.Vector3;
            UndoRedoStack.AddRedoCommand(UndoCommand, parameter);
            Position.Vector3 = position.Vector3;
        }

        private void undoSetPosition(MyVector3 position) 
        {
            var redoCommand = new RelayCommand<MyVector3>(SetPosition);
            MyVector3 parameter = new MyVector3();
            parameter.Vector3 = Position.Vector3;
            UndoRedoStack.AddUndoCommand(redoCommand, parameter);
            Position.Vector3 = position.Vector3;
        }

        public void SetScale(MyVector3 scale)
        {
            var UndoCommand = new RelayCommand<MyVector3>(undoSetScale);
            MyVector3 parameter = new MyVector3();
            parameter.Vector3 = Scale.Vector3;
            UndoRedoStack.AddRedoCommand(UndoCommand, parameter);
            Scale.Vector3 = scale.Vector3;
        }

        private void undoSetScale(MyVector3 scale)
        {
            var redoCommand = new RelayCommand<MyVector3>(SetScale);
            MyVector3 parameter = new MyVector3();
            parameter.Vector3 = Scale.Vector3;
            UndoRedoStack.AddUndoCommand(redoCommand, parameter);
            Scale.Vector3 = scale.Vector3;
        }

        public void SetRotation(MyVector3 rotation)
        {
            var UndoCommand = new RelayCommand<MyVector3>(undoSetRotation);
            MyVector3 parameter = new MyVector3();
            parameter.Vector3 = RotationEuler.Vector3;
            UndoRedoStack.AddRedoCommand(UndoCommand, parameter);
            RotationEuler.Vector3 = rotation.Vector3;
        }

        private void undoSetRotation(MyVector3 rotation)
        {
            var redoCommand = new RelayCommand<MyVector3>(SetRotation);
            MyVector3 parameter = new MyVector3();
            parameter.Vector3 = RotationEuler.Vector3;
            UndoRedoStack.AddUndoCommand(redoCommand, parameter);
            RotationEuler.Vector3 = rotation.Vector3;
        }
    }
}
