using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InputManager = DirectxWpf.MVVM_Model.Managers.InputManager;


namespace DirectxWpf.MVVM_Model
{
    public class Camera
    {
        //*******************************************************//
        //                      FIELDS                           //
        //*******************************************************//
        private float _TotalPitch;
        private float _TotalYaw;
        private float _MoveSpeed;
        private float _SpeedMultiplier;
        private Vector3 _position;
        private Vector3 _rotation;
        private Vector3 _scale;


        //*******************************************************//
        //                      PROPERTIES                       //
        //*******************************************************//
        public Vector3 Up { get; private set; }
        public Vector3 Right { get; private set; }
        public Vector3 Forward { get; private set; }
       
        public Vector3 Position { get { return _position; } set { _position = value; UpdateTransforms(); } }
        public Vector3 Rotation { get { return _rotation; } set { _rotation = value; UpdateTransforms(); } }
        public Vector3 Scale { get { return _scale; } set { _scale = value; UpdateTransforms(); } }
       
        public Matrix WorldMatrix { get; private set; }
        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }
        public Matrix ViewInverseMatrix { get; private set; }
        public Matrix ViewProjectionMatrix { get; private set; }
        public Matrix ViewProjectionInverseMatrix { get; private set; }

        public float FarPlane { get; set; }
        public float NearPlane { get; set; }
        public float FOV { get; set; }
        public float OrthographicSize { get; set; }
        public bool PerspectiveProjection { get; set; }

        public bool IsActive {get; set; }


        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public Camera()
        {   
            _MoveSpeed = 75;
            _SpeedMultiplier = 2.5f;

            Scale =new Vector3(1, 1, 1);
            Rotation = new Vector3(0, 0, 0);
            Position = new Vector3(0, 0, -100);

            ViewMatrix = Matrix.Identity;
            ProjectionMatrix = Matrix.Identity;
            ViewInverseMatrix = Matrix.Identity;
            ViewProjectionMatrix = Matrix.Identity;
            ViewProjectionInverseMatrix = Matrix.Identity;

            FOV = MathUtil.DegreesToRadians( 25 );
            NearPlane = 0.1f;
            FarPlane = 2500.0f;
            OrthographicSize = 100.0f;
            PerspectiveProjection = true;

            IsActive = true;
        }

        private void UpdateTransforms() 
        {
            //Calculate World Matrix
            //**********************
            WorldMatrix = Matrix.Identity;
            WorldMatrix *= Matrix.Scaling(Scale);
            WorldMatrix *= Matrix.RotationX(Rotation.X);
            WorldMatrix *= Matrix.RotationY(Rotation.Y);
            WorldMatrix *= Matrix.RotationZ(Rotation.Z);
            WorldMatrix *= Matrix.Translation(Position);


            Matrix rotMat = Matrix.Identity;
            rotMat *= Matrix.RotationX(Rotation.X);
            rotMat *= Matrix.RotationY(Rotation.Y);
            rotMat *= Matrix.RotationZ(Rotation.Z);

            Forward = Vector3.TransformCoordinate(new Vector3(0, 0, 1), rotMat);
            Right = Vector3.TransformCoordinate(new Vector3(1, 0, 0), rotMat);
            Up= Vector3.Cross(Forward, Right);
        }

        public void Update(float deltaTime, float viewportWidth, float viewportHeight  ) 
        {
	      // Matrix projectionMat, viewMat, viewInvMat, viewProjectionInvMat;
            if (IsActive)
            {
                KeyInput(deltaTime);
                if (InputManager.IsRightMouseButtonDown())
                {
                    _TotalYaw += InputManager.GetMouseDirection().X * MathUtil.PiOverFour * deltaTime;
                    _TotalPitch += InputManager.GetMouseDirection().Y * MathUtil.PiOverFour * deltaTime;
                }


            }
            if (PerspectiveProjection)
            {
                ProjectionMatrix = Matrix.PerspectiveFovLH(FOV, viewportWidth / viewportHeight, NearPlane, FarPlane);
            }
            else
            {
                float viewWidth = (OrthographicSize > 0) ? OrthographicSize * viewportWidth / viewportHeight : viewportWidth;
                float viewHeight = (OrthographicSize > 0) ? OrthographicSize : viewportHeight;
                ProjectionMatrix = Matrix.OrthoLH(viewWidth, viewHeight, NearPlane, FarPlane);
            }

            Rotation = new Vector3(_TotalPitch, _TotalYaw, 0);
            ViewMatrix = Matrix.LookAtLH(Position,Position+ Forward, Up);
	        ViewInverseMatrix= Matrix.Invert(ViewMatrix);
        }

        private void KeyInput(float deltaTime) 
        {
            float multiplier = 1;
            if (InputManager.IsKeyDown(Key.LeftShift))
            {
                multiplier = _SpeedMultiplier;
            }
            if (InputManager.IsKeyDown(Key.Z))
            {
                Position += Forward * _MoveSpeed * multiplier * deltaTime;
            }
            if (InputManager.IsKeyDown(Key.S))
            {
                Position -= Forward * _MoveSpeed * multiplier * deltaTime;
            }
            if (InputManager.IsKeyDown(Key.Q))
            {
                Position -= Right * _MoveSpeed * multiplier * deltaTime;
            }
            if (InputManager.IsKeyDown(Key.D))
            {
                Position += Right * _MoveSpeed * multiplier * deltaTime;
            }

             
        }
    }
}
