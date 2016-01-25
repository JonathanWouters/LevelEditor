using SharpDX;
using SharpDX.Direct3D10;
using Format = SharpDX.DXGI.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectxWpf.Effects;
using DaeSharpWpf;
using DirectxWpf.MVVM_Model;
using DirectxWpf.MVVM_Model.Managers;
using GalaSoft.MvvmLight.Command;

namespace DirectxWpf.MVVM_Model.Models
{
    public enum GizmoMode
    {
        None,
        Translate,
        Rotate,
        Scale
    }

    public class Gizmo
    {
        public List<IModel> TranslationGizmo { get; private set; }
        public List<IModel> RotationGizmo { get; private set; }
        public List<IModel> ScaleGizmo { get; private set; }
        public GizmoMode Mode { get; set; }
        public int SelectedAxis { get; set; }

        private MyVector3 _StartPosition, _StartRotation, _StartScale;
        private Vector3 _HitPosition;
        private Vector3 _LastIntersectionPosition;
        private System.Windows.Point _lastMousePos;
        private Vector3 _Position;
        public Vector3 Position { 
            get { return _Position; } 
            set 
            {
                _Position = value;
                foreach (var model in TranslationGizmo)
                {
                    model.Position.Vector3 = value;
                }
                foreach (var model in RotationGizmo)
                {
                    model.Position.Vector3 = value;
                }
                foreach (var model in ScaleGizmo)
                {
                    model.Position.Vector3 = value;
                }
            } 
        }

        public float TranslationSnapValue { get; set; }
        public float RotationSnapValue { get; set; }
        public float ScaleSnapValue { get; set; }
        public float ScaleSensitivity{get;set;}


        public Gizmo(Device1 device) 
        {
            Mode = GizmoMode.None;
            SelectedAxis = -1;
            TranslationSnapValue = 1.0f;
            RotationSnapValue = 5.0f;
            ScaleSnapValue = 0.1f;
            ScaleSensitivity = 0.2f;

            _StartPosition = null;
            _StartScale = null;
            _StartRotation = null;

            // Translation Gizmo
            TranslationGizmo = new List<IModel>();
            var XArrow = new OvmModel("Resources//Arrow.ovm");
            XArrow.Create(device);
            XArrow.Shader = new ColorShader(new Color(255, 0, 0, 255));
            TranslationGizmo.Add(XArrow);

            var YArrow = new OvmModel("Resources//Arrow.ovm");
            YArrow.Create(device);
            YArrow.Shader = new ColorShader(new Color(0, 255, 0, 255));
            YArrow.Rotation.Z = 90;
            TranslationGizmo.Add(YArrow);

            var ZArrow = new OvmModel("Resources//Arrow.ovm");
            ZArrow.Create(device);
            ZArrow.Shader = new ColorShader(new Color(0, 0, 255, 255));
            ZArrow.Rotation.Y = 90;
            TranslationGizmo.Add(ZArrow);

            // Rotation Gizmo
            RotationGizmo = new List<IModel>();
            var XRotatinGizmo = new OvmModel("Resources//RotationGizmo.ovm");
            XRotatinGizmo.Create(device);
            XRotatinGizmo.Shader = new ColorShader(new Color(255, 0, 0, 255));
            RotationGizmo.Add(XRotatinGizmo);

            var YRotatinGizmo = new OvmModel("Resources//RotationGizmo.ovm");
            YRotatinGizmo.Create(device);
            YRotatinGizmo.Shader = new ColorShader(new Color(0, 255, 0, 255));
            YRotatinGizmo.Rotation.Z = 90;
            RotationGizmo.Add(YRotatinGizmo);

            var ZRotatinGizmo = new OvmModel("Resources//RotationGizmo.ovm");
            ZRotatinGizmo.Create(device);
            ZRotatinGizmo.Shader = new ColorShader(new Color(0, 0, 255, 255));
            ZRotatinGizmo.Rotation.Y = 90;
            RotationGizmo.Add(ZRotatinGizmo);

            // Scale Gizmo
            ScaleGizmo = new List<IModel>();
            var XScaleArrow = new OvmModel("Resources//ScaleArrow.ovm");
            XScaleArrow.Create(device);
            XScaleArrow.Shader = new ColorShader(new Color(255, 0, 0, 255));
            ScaleGizmo.Add(XScaleArrow);

            var YScaleArrow = new OvmModel("Resources//ScaleArrow.ovm");
            YScaleArrow.Create(device);
            YScaleArrow.Shader = new ColorShader(new Color(0, 255, 0, 255));
            YScaleArrow.Rotation.Z = 90;
            ScaleGizmo.Add(YScaleArrow);

            var ZScaleArrow = new OvmModel("Resources//ScaleArrow.ovm");
            ZScaleArrow.Create(device);
            ZScaleArrow.Shader = new ColorShader(new Color(0, 0, 255, 255));
            ZScaleArrow.Rotation.Y = 90;;
            ScaleGizmo.Add(ZScaleArrow);
        }

        public List<IModel> GetCurrentGizmoModels() 
        {
            switch (Mode)
            {
                case GizmoMode.None:
                    return null;

                case GizmoMode.Translate:
                    return TranslationGizmo;

                case GizmoMode.Rotate:
                    return RotationGizmo;

                case GizmoMode.Scale:
                    return ScaleGizmo;

                default:
                    return null;
            }       
        }
        public void Render(Device1 device) 
        {
            switch (Mode)
            {
                case GizmoMode.None:
                    break;

                case GizmoMode.Translate:
                    foreach (var model in TranslationGizmo)
                    {
                        model.Render(device);
                    }
                    break;

                case GizmoMode.Rotate:
                    foreach (var model in RotationGizmo)
                    {
                        model.Render(device);
                    }
                    break;

                case GizmoMode.Scale:
                    foreach (var model in ScaleGizmo)
                    {
                        model.Render(device);
                    }
                    break;

                default:
                    break;
            }


        }

        private void UpdateModels(Camera camera)
        {
            foreach (var model in TranslationGizmo)
            {
                // Keep the gizmo the same size regardless of camera distance
                float cameraObjectDistance = Vector3.Distance(camera.Position, model.Position.Vector3);
                float worldSize = 2 * (float)Math.Tan((double)(camera.FOV / 2.0)) * cameraObjectDistance;
                float scale = 0.05f * worldSize;
                model.Scale.Vector3 = Vector3.One * scale;

                model.Update(camera);

            }
            foreach (var model in RotationGizmo)
            {
                // Keep the gizmo the same size regardless of camera distance
                float cameraObjectDistance = Vector3.Distance(camera.Position, model.Position.Vector3);
                float worldSize = 2 * (float)Math.Tan((double)(camera.FOV / 2.0)) * cameraObjectDistance;
                float scale = 0.04f * worldSize;
                model.Scale.Vector3 = Vector3.One * scale;

                model.Update(camera);
            }
            foreach (var model in ScaleGizmo)
            {
                // Keep the gizmo the same size regardless of camera distance
                float cameraObjectDistance = Vector3.Distance(camera.Position, model.Position.Vector3);
                float worldSize = 2 * (float)Math.Tan((double)(camera.FOV / 2.0)) * cameraObjectDistance;
                float scale = 0.05f * worldSize;
                model.Scale.Vector3 = Vector3.One * scale;

                model.Update(camera);
            }

        }
        private Vector3 CalculateTransformDelta(Plane plane,Ray ray,Camera camera)
        {
	        float intersectionDistance = float.MaxValue;
	        Vector3 intersectPosition = Vector3.Zero;

            if (ray.Intersects(ref plane, out intersectionDistance))
	        {
		        intersectPosition = (camera.Position + (ray.Direction * intersectionDistance));

	        }
		    if (_LastIntersectionPosition == Vector3.Zero)
                _LastIntersectionPosition = intersectPosition;

	        return intersectPosition - _LastIntersectionPosition;
        }

        public void Update(Camera camera, ViewportF view, System.Windows.Point mousePos, GameObject linkedObject)
        {
             UpdateModels(camera);

            if (InputManager.IsLeftMouseButtonDown() && SelectedAxis != -1)
            {
                //Create a Ray from the mouse position
                Matrix worldViewProjectionMatrix = Matrix.Identity *  camera.ViewMatrix * camera.ProjectionMatrix;
                Ray ray = Ray.GetPickRay((int)(mousePos.X), (int)(mousePos.Y), view, worldViewProjectionMatrix);
                Plane plane;
                Matrix RotationMat = Matrix.Identity;
                switch (Mode)
                {
                    case GizmoMode.None:
                        break;
                    case GizmoMode.Translate:
                        if (_StartPosition == null)
                        {
                            _StartPosition = new MyVector3();
                            _StartPosition.Vector3 = linkedObject.Transform.Position.Vector3; ;
                        }
                        Vector3 translationDelta = Vector3.Zero;
                        switch (SelectedAxis)
                        {
                            case 0:
                                plane = new Plane(_HitPosition, camera.Forward);
                                translationDelta = CalculateTransformDelta(plane, ray, camera);
                                Console.WriteLine(_HitPosition);

                                translationDelta = new Vector3(translationDelta.X, 0, 0);
                                break;
                            case 1:
 
                                plane = new Plane(_HitPosition, camera.Forward);
                                translationDelta = CalculateTransformDelta(plane, ray, camera);
                                Console.WriteLine(_HitPosition);
                                translationDelta = new Vector3(0, translationDelta.Y, 0);
                                break;
                            case 2:
                                plane = new Plane(_HitPosition, Vector3.Up);
                                translationDelta = CalculateTransformDelta(plane, ray, camera);
                                translationDelta = new Vector3(0, 0, translationDelta.Z);
                                break;
                            default:
                                break;
                        }

                        var scaledTranslationDelta = new Vector3((int)(translationDelta.X / TranslationSnapValue) * TranslationSnapValue,
                                                                 (int)(translationDelta.Y / TranslationSnapValue) * TranslationSnapValue,
                                                                 (int)(translationDelta.Z / TranslationSnapValue) * TranslationSnapValue);

                        _LastIntersectionPosition += scaledTranslationDelta;
                        linkedObject.Transform.Position.Vector3 += scaledTranslationDelta;
                        break;

                    case GizmoMode.Rotate:
                        if (_StartRotation == null)
                        {
                            _StartRotation = new MyVector3();
                            _StartRotation.Vector3 = linkedObject.Transform.RotationEuler.Vector3; ;
                        }
                        Vector3 RotateDelta = Vector3.Zero;
                        Vector3 MouseDelta = Vector3.Zero;
                        switch (SelectedAxis)
                        {
                            case 0:
                                plane = new Plane(_HitPosition, camera.Forward);
                                RotateDelta = CalculateTransformDelta(plane, ray, camera);
                                MouseDelta = new Vector3(0, RotateDelta.Y, 0);
                                RotateDelta = new Vector3(RotateDelta.Y, 0, 0);

                                break;
                            case 1:
                                plane = new Plane(_HitPosition, camera.Forward);
                                RotateDelta = CalculateTransformDelta(plane, ray, camera);
                                MouseDelta = new Vector3(RotateDelta.X, 0, 0);
                                RotateDelta = new Vector3(0, -RotateDelta.X, 0);
                                break;
                            case 2:
                                plane = new Plane(_HitPosition, camera.Up);
                                RotateDelta = CalculateTransformDelta(plane, ray, camera);
                                MouseDelta = new Vector3(0, 0, RotateDelta.Z);
                                RotateDelta = new Vector3(0, 0, RotateDelta.Z);
                                break;
                            default:
                                break;
                        }

                        RotateDelta *= 10.0f;
                        var scaledRotationDelta = new Vector3((int)(RotateDelta.X / RotationSnapValue) * RotationSnapValue,
                                                                 (int)(RotateDelta.Y / RotationSnapValue) * RotationSnapValue,
                                                                 (int)(RotateDelta.Z / RotationSnapValue) * RotationSnapValue);
                        
                        _LastIntersectionPosition += MouseDelta;
                        linkedObject.Transform.RotationEuler.X += scaledRotationDelta.X;
                        linkedObject.Transform.RotationEuler.Y += scaledRotationDelta.Y;
                        linkedObject.Transform.RotationEuler.Z += scaledRotationDelta.Z;
                        break;
                    case GizmoMode.Scale:
                        Vector3 scaleDelta = Vector3.Zero;
                        if (_StartScale == null)
                        {
                            _StartScale = new MyVector3();
                            _StartScale.Vector3 = linkedObject.Transform.Scale.Vector3; ;
                        }
                        switch (SelectedAxis)
                        {
                            case 0:
                                plane = new Plane(_HitPosition, camera.Forward);
                                scaleDelta = CalculateTransformDelta(plane, ray, camera);
                                scaleDelta = new Vector3(scaleDelta.X, 0, 0);
                                break;
                            case 1:
                                plane = new Plane(_HitPosition, camera.Forward);
                                scaleDelta = CalculateTransformDelta(plane, ray, camera);
                                scaleDelta = new Vector3(0, scaleDelta.Y, 0);
                                break;
                            case 2:
                                plane = new Plane(_HitPosition, Vector3.Up);
                                scaleDelta = CalculateTransformDelta(plane, ray, camera);
                                scaleDelta = new Vector3(0, 0, scaleDelta.Z);
                                break;
                            default:
                                break;
                        }

                        var scaledScaleDelta = new Vector3((int)(scaleDelta.X / ScaleSnapValue) * ScaleSnapValue,
                                                           (int)(scaleDelta.Y / ScaleSnapValue) * ScaleSnapValue,
                                                           (int)(scaleDelta.Z / ScaleSnapValue) * ScaleSnapValue) ;

                        _LastIntersectionPosition += scaledScaleDelta;

                        RotationMat = Matrix.RotationQuaternion(linkedObject.Transform.Rotation);
                        scaledScaleDelta = Vector3.TransformCoordinate(scaledScaleDelta, RotationMat);

                        linkedObject.Transform.Scale.Vector3 += scaledScaleDelta * new Vector3(1,1,-1) ;
                        break;
                    default:
                        break;
                }

                
            }
            else 
            {
                if (SelectedAxis != -1 )
                {
                    switch (Mode)
                    {
                        case GizmoMode.None:
                            break;
                        case GizmoMode.Translate:
                                var UndoCommand = new RelayCommand<MyVector3>(linkedObject.Transform.SetPosition);
                                UndoRedoStack.AddUndoCommand(UndoCommand, _StartPosition);
                                UndoRedoStack.ClearRedoStack();
                                _StartPosition = null;
                            break;
                        case GizmoMode.Rotate:
                                var UndoRotateCommand = new RelayCommand<MyVector3>(linkedObject.Transform.SetRotation);
                                UndoRedoStack.AddUndoCommand(UndoRotateCommand, _StartRotation);
                                UndoRedoStack.ClearRedoStack();
                                _StartRotation = null;
                            break;
                        case GizmoMode.Scale:
                                var UndoScaleCommand = new RelayCommand<MyVector3>(linkedObject.Transform.SetScale);
                                UndoRedoStack.AddUndoCommand(UndoScaleCommand, _StartScale);
                                UndoRedoStack.ClearRedoStack();
                                _StartScale = null;
                            break;
                        default:
                            break;
                    }
                }
                SelectedAxis = -1;
                _LastIntersectionPosition = Vector3.Zero;
            }

        }
        public bool IsHit(Camera camera, ViewportF view, System.Windows.Point mousePos)
        {
            bool result = false;
            float distance = float.MaxValue;
            float closestDistance = float.MaxValue;

            if (GetCurrentGizmoModels() == null)
                return false;
            for (int i = 0; i < GetCurrentGizmoModels().Count; i++)
            {
                // Create the worldViewProjectionMatrix
                Matrix worldViewProjectionMatrix = GetCurrentGizmoModels()[i].GetWorldMatrix() * camera.ViewMatrix * camera.ProjectionMatrix;

                //Create a Ray from the mouse position
                Ray ray = Ray.GetPickRay((int)(mousePos.X), (int)(mousePos.Y), view, worldViewProjectionMatrix);

                if (GetCurrentGizmoModels()[i].IsHitByRay(ray, out distance))
                {
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        SelectedAxis = i;

                        var rotate = Matrix.Identity;
                        
                        rotate = Matrix.RotationYawPitchRoll(MathUtil.DegreesToRadians(GetCurrentGizmoModels()[i].Rotation.Y),
                                                                   MathUtil.DegreesToRadians(GetCurrentGizmoModels()[i].Rotation.X),
                                                                   MathUtil.DegreesToRadians(GetCurrentGizmoModels()[i].Rotation.Z));
                        
                        _HitPosition = camera.Position + Vector3.TransformCoordinate(ray.Direction * distance, rotate);
                        
                        _lastMousePos = mousePos;
                    }

                    result = true;
                }
            }                    
            return result;
        }
    }
}
