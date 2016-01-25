using DirectxWpf.Effects;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D10;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Buffer = SharpDX.Direct3D10.Buffer;

namespace DirectxWpf.MVVM_Model.Models
{
    public interface IModel
    {
        PrimitiveTopology PrimitiveTopology { get; set; }
        int VertexStride { get; set; }
        int IndexCount { get; set; }
        Buffer IndexBuffer { get; set; }
        List<uint> IndexList { get; set; }
        Buffer VertexBuffer { get; set; }
        List<Vector3> VertexList { get; set; }
        IEffect Shader { get; set; }
        MyVector3 Position { get; set ;}
        MyVector3 Rotation { get; set; }
        MyVector3 Scale { get; set; }
        BoundingSphere BoundingSphere { get; set; }
        string Name { get; set; }

        void Create(Device1 device);
        Matrix GetWorldMatrix();
        void Render(Device1 device);
        void Update(Camera camera);

        bool IsHitByRay(Ray ray, out float distance);

    }
}
