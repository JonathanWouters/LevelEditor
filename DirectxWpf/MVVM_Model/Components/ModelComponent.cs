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
using System.IO;
using DirectxWpf.Effects;
using System.ComponentModel;
using Format = SharpDX.DXGI.Format;
using DirectxWpf.MVVM_Model.Models;
using System.Windows;

namespace DirectxWpf.MVVM_Model.Components
{
    [Serializable]
    public class ModelComponent: IBaseComponent
    {
        //**********************************//
        //            FIELDS                //
        //**********************************//
        private string _filePath;
        private bool _isInitialized = false;

        //**********************************//
        //            PROPERTIES            //
        //**********************************//
        public GameObject GameObject { get; set; }
        public string FilePath { get { return _filePath; } private set {_filePath = value ;} }
        public PrimitiveTopology PrimitiveTopology { get; set; }
        public int VertexStride { get; set; }
        public int IndexCount { get; set; }
        public Buffer IndexBuffer { get; set; }
        public List<uint> IndexList { get; set; }
        public Buffer VertexBuffer { get; set; }
        public List<Vector3> VertexList { get; set; }
        public BoundingSphere BoundingSphere { get; set; }
        public IEffect Shader { get; set; }
        //**********************************//
        //            METHODS               //
        //**********************************//
        public ModelComponent(string filePath ) 
        {
            _isInitialized = false;
            _filePath = filePath;

        }
        public void Initialize(Device1 device)
        {
            if (_isInitialized)
                return;

             //Set Primitive Topology
            PrimitiveTopology = PrimitiveTopology.TriangleList;

            //Set VertexStride
            VertexStride = Marshal.SizeOf(typeof(VertexPosColNorm));

            //Read file
            ParseFile(device, _filePath);

            //Create the bounding Sphere
            BoundingSphere = BoundingSphere.FromPoints(VertexList.ToArray());
            _isInitialized = true;



        }

        public TransformComponent GetTransform()
        {
            if (GameObject == null)
            {
                Console.WriteLine("BaseComponent::GetTransform() > Failed to retrieve the TransformComponent. GameObject is NULL.");
                return null;
            }
            return GameObject.Transform;
        }
        public void Render(Device1 device)
        {
            if (!_isInitialized)
                Initialize(device);

            if (Shader != null)
            {
                if (!Shader.IsInitialized)
                    Shader.Initialize(device);

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
        public void Update(float deltaTime,Camera camera)
        {
            if (!_isInitialized)
                return;

            if (Shader != null)
            {
                Shader.SetWorld(GetTransform().World);
                Shader.SetWorldViewProjection(GetTransform().World * camera.ViewMatrix * camera.ProjectionMatrix);
                Shader.UpdateEffectVariable();
            }
        }

        public GameObject IsHitByMouseClick(System.Windows.Point mousePos, ViewportF viewFrustum, Camera camera, out float distance)
        {
            distance = float.MaxValue;

            if (!_isInitialized)
                return null;

            GameObject result = null;

            // Create the worldViewProjectionMatrix
            Matrix worldViewProjectionMatrix = GameObject.Transform.World * camera.ViewMatrix * camera.ProjectionMatrix;

            //Create a Ray from the mouse position
            Ray ray = Ray.GetPickRay((int)(mousePos.X), (int)(mousePos.Y), viewFrustum, worldViewProjectionMatrix);


            float closestHit = float.MaxValue;

            if (ray.Intersects(BoundingSphere))
            {
                //Check if the ray intersects with a triangle from the mesh
                for (int j = 0; j < IndexCount / 3; j++)
                {
                    Vector3 v0 = VertexList[(int)IndexList[j * 3]];
                    Vector3 v1 = VertexList[(int)IndexList[j * 3 + 1]];
                    Vector3 v2 = VertexList[(int)IndexList[j * 3 + 2]];

                    var ScaleMatrix = Matrix.Scaling(GetTransform().WorldScale.Vector3);

                    if (ray.Intersects(ref v0, ref v1, ref v2, out distance))
                    {
                        if (closestHit > distance * (ray.Direction * GetTransform().WorldScale.Vector3).Length())
                        {
                            closestHit = distance * (ray.Direction * GetTransform().WorldScale.Vector3).Length();
                            distance = float.MaxValue;
                        }
                        result = GameObject;
                    }
                }
            }

            distance = closestHit;
            return result;
        }

        private void ParseFile(Device1 device, string path)
        {
            var verts = new List<VertexPosColNorm>();
            IndexList = new List<uint>();

            uint vertCount = 0;

            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                int versionMaj = reader.ReadByte();
                int versionMin = reader.ReadByte();

                for (; ; )
                {
                    // block stuff
                    int blockId = reader.ReadByte();
                    if (blockId == 0)
                        break;
                    uint blockLength = reader.ReadUInt32();

                    if (blockId == 1)
                    {
                        //general
                        reader.ReadString();
                        vertCount = reader.ReadUInt32();
                        IndexCount = (int)reader.ReadUInt32();
                    }
                    else if (blockId == 2)
                    {
                        //positions
                        for (int i = 0; i < vertCount; i++)
                        {
                            float x = reader.ReadSingle();
                            float y = reader.ReadSingle();
                            float z = reader.ReadSingle();
                            verts.Add(new VertexPosColNorm(new Vector3(x, y, z) , Color.Gray, Vector3.Zero));
                        }
                    }
                    else if (blockId == 3)
                    {
                        //indices
                        for (int i = 0; i < IndexCount; i++)
                        {
                            IndexList.Add(reader.ReadUInt32());
                        }
                    }
                    else if (blockId == 4)
                    {
                        //normals
                        for (int i = 0; i < vertCount; i++)
                        {
                            float x = reader.ReadSingle();
                            float y = reader.ReadSingle();
                            float z = reader.ReadSingle();

                            var copy = verts[i];
                            verts[i] = new VertexPosColNorm(copy.Position, Color.Gray, new Vector3(x, y, z));
                        }
                    }
                    else if (blockId == 7)
                    {
                        //colors
                        for (int i = 0; i < vertCount; i++)
                        {
                            float r = reader.ReadSingle();
                            float g = reader.ReadSingle();
                            float b = reader.ReadSingle();
                            float a = reader.ReadSingle();

                            var copy = verts[i];
                            verts[i] = new VertexPosColNorm(copy.Position, new Color(r, g, b, a), copy.Normal);
                        }
                    }
                    else
                    {
                        reader.ReadBytes((int)blockLength);
                    }
                }
            }

            //CREATE vertex buffer
            if (VertexBuffer != null)
                VertexBuffer.Dispose();

            var vertBufferDescription = new BufferDescription
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Immutable,
                SizeInBytes = VertexStride * verts.Count
            };

            VertexBuffer = new Buffer(device, DataStream.Create(verts.ToArray(), false, false), vertBufferDescription);


            //CREATE index buffer
            if (IndexBuffer != null)
                IndexBuffer.Dispose();

            var intBufferDescription = new BufferDescription
            {
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Immutable,
                SizeInBytes = sizeof(uint) * IndexCount
            };

            IndexBuffer = new Buffer(device, DataStream.Create(IndexList.ToArray(), false, false), intBufferDescription);

            VertexList = new List<Vector3>();
            foreach (var vertex in verts)
            {
                VertexList.Add(vertex.Position);
            }
        }
    }
}
