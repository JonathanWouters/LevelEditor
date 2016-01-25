using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D10;
using Buffer = SharpDX.Direct3D10.Buffer;
using System.Runtime.InteropServices;
using System.ComponentModel;
using DirectxWpf.Effects;
using Format = SharpDX.DXGI.Format;

namespace DirectxWpf.MVVM_Model.Models
{
    class BoxModel :IModel, INotifyPropertyChanged
    {
        public PrimitiveTopology PrimitiveTopology { get; set; }
        public int VertexStride { get; set; }
        public int IndexCount { get; set; }
        public Buffer IndexBuffer { get; set; }
        public List<uint> IndexList { get; set; }
        public Buffer VertexBuffer { get; set; }
        public List<Vector3> VertexList { get; set; }
        public IEffect Shader { get; set; }

        public MyVector3 Position { get; set; }
        public MyVector3 Rotation { get; set; }
        public MyVector3 Scale { get; set; }

        public BoundingSphere BoundingSphere { get; set; }

        private string _name;
        public string Name 
        { 
            get { return _name; } 
            set { _name = value; OnPropertyChanged("Name"); } 
        }
        public static int BOX_COUNT = 0;

        public BoxModel() 
        {
            Position = new MyVector3();
            Rotation = new MyVector3();
            Scale = new MyVector3(1, 1, 1);
            Name = "Box_" + BOX_COUNT;
            BOX_COUNT++;

        }
        public Matrix GetWorldMatrix()
        {
            var worldMat = Matrix.Identity;
            worldMat *= Matrix.Scaling(Scale.Vector3);
            worldMat *= Matrix.RotationX (MathUtil.DegreesToRadians( Rotation.X ) );
            worldMat *= Matrix.RotationY(MathUtil.DegreesToRadians( Rotation.Y ));
            worldMat *= Matrix.RotationZ(MathUtil.DegreesToRadians( Rotation.Z ));
            worldMat *= Matrix.Translation(Position.Vector3);

            return worldMat;
        }
        public void Create(Device1 device)
        {
            //Set Primitive Topology
            PrimitiveTopology = PrimitiveTopology.TriangleList;

            //Set VertexStride
            VertexStride = Marshal.SizeOf(typeof(VertexPosColNorm));

            //Create VertexBuffer
            CreateVertexBuffer(device);

            //CreateIndexBuffer
            CreateIndexBuffer(device);

            //Create the bounding sphere
            BoundingSphere = BoundingSphere.FromPoints(VertexList.ToArray());
        }
        private void CreateVertexBuffer(Device1 device)
        {
            var verts = new List<VertexPosColNorm>();

            Color col = Color.Red;
            Vector3 norm = new Vector3(0, 0, -1);
            //FRONT RED
            verts.Add(new VertexPosColNorm(new Vector3(-0.5f, -0.5f, -0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(-0.5f, 0.5f, -0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(0.5f, -0.5f, -0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(0.5f, 0.5f, -0.5f), col, norm));

            //BACK RED
            norm = new Vector3(0, 0, 1);
            verts.Add(new VertexPosColNorm(new Vector3(-0.5f, -0.5f, 0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(-0.5f, 0.5f, 0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(0.5f, -0.5f, 0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(0.5f, 0.5f, 0.5f), col, norm));

            //LEFT GREEN
            col = Color.Green;
            norm = new Vector3(-1, 0, 0);
            verts.Add(new VertexPosColNorm(new Vector3(-0.5f, -0.5f, -0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(-0.5f, 0.5f, -0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(-0.5f, -0.5f, 0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(-0.5f, 0.5f, 0.5f), col, norm));

            //RIGHT GREEN
            norm = new Vector3(1, 0, 0);
            verts.Add(new VertexPosColNorm(new Vector3(0.5f, -0.5f, -0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(0.5f, 0.5f, -0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(0.5f, -0.5f, 0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(0.5f, 0.5f, 0.5f), col, norm));

            //TOP BLUE
            col = Color.Blue;
            norm = new Vector3(0, 1, 0);
            verts.Add(new VertexPosColNorm(new Vector3(-0.5f, 0.5f, 0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(-0.5f, 0.5f, -0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(0.5f, 0.5f, 0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(0.5f, 0.5f, -0.5f), col, norm));

            //BOTTOM BLUE
            norm = new Vector3(0, -1, 0);
            verts.Add(new VertexPosColNorm(new Vector3(-0.5f, -0.5f, 0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(-0.5f, -0.5f, -0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(0.5f, -0.5f, 0.5f), col, norm));
            verts.Add(new VertexPosColNorm(new Vector3(0.5f, -0.5f, -0.5f), col, norm));

            //CREATE BUFFER
            if (VertexBuffer != null)
                VertexBuffer.Dispose();

            var bufferDescription = new BufferDescription
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Immutable,
                SizeInBytes = VertexStride * verts.Count
            };

            VertexBuffer = new Buffer(device, DataStream.Create(verts.ToArray(), false, false), bufferDescription);
           
            VertexList = new List<Vector3>();
            foreach (var vertex in verts)
            {
                VertexList.Add(vertex.Position);
            }

        }
        private void CreateIndexBuffer(Device1 device)
        {
            IndexList = new List<uint>()
            {
                0, 1, 2, 2, 1, 3,
                4, 6, 5, 5, 6, 7,
                8, 10, 9, 9, 10, 11,
                12, 13, 14, 14, 13, 15,
                16, 18, 17, 17, 18, 19,
                20, 21, 22, 22, 21, 23
            };
            IndexCount = IndexList.Count;

            //CREATE BUFFER
            if (IndexBuffer != null)
                IndexBuffer.Dispose();

            var bufferDescription = new BufferDescription
            {
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Immutable,
                SizeInBytes = sizeof(uint) * IndexCount
            };

            IndexBuffer = new Buffer(device, DataStream.Create(IndexList.ToArray(), false, false), bufferDescription);
        }
        public void Render(Device1 device)
        {
            if (Shader != null)
            {
                if (!Shader.IsInitialized)
                {
                    Shader.Initialize(device);
                }
                device.InputAssembler.InputLayout = Shader.InputLayout;
                device.InputAssembler.PrimitiveTopology = PrimitiveTopology;
                device.InputAssembler.SetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);
                device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, VertexStride, 0));

                for (int i = 0; i < Shader.Technique.Description.PassCount; ++i)
                {
                    Shader.Technique.GetPassByIndex(i).Apply();
                    device.DrawIndexed(IndexCount, 0, 0);
                }
            }
        }
        public void Update(Camera camera)
        {
            if (Shader != null)
            {
                Shader.SetWorld(GetWorldMatrix());
                Shader.SetWorldViewProjection(GetWorldMatrix() * camera.ViewMatrix * camera.ProjectionMatrix);
                Shader.UpdateEffectVariable();
            }
        }

        public bool IsHitByRay(Ray ray, out float distance)
        {
            bool result = false;
            distance = float.MaxValue;

            float closestHit = float.MaxValue;
            if (ray.Intersects(BoundingSphere))
            {
                //Check if the ray intersects with a triangle from the mesh
                for (int j = 0; j < IndexCount / 3; j++)
                {
                    Vector3 v0 = VertexList[(int)IndexList[j * 3]];
                    Vector3 v1 = VertexList[(int)IndexList[j * 3 + 1]];
                    Vector3 v2 = VertexList[(int)IndexList[j * 3 + 2]];

                    var ScaleMatrix = Matrix.Scaling(Scale.Vector3);

                    if (ray.Intersects(ref v0, ref v1, ref v2, out distance))
                    {
                        if (closestHit > distance * (ray.Direction * Scale.Vector3).Length())
                        {
                            closestHit = distance * (ray.Direction * Scale.Vector3).Length();
                            distance = float.MaxValue;
                        }
                        result = true;
                    }
                }
            }

            distance = closestHit;
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
