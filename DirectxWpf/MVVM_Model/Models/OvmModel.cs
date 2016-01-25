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

namespace DirectxWpf.MVVM_Model.Models
{
    public class OvmModel : IModel, INotifyPropertyChanged
    {
        private string _path;

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


        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        public static Dictionary<string, int> NameDictionary;
        public BoundingSphere BoundingSphere { get; set; }

        public OvmModel(string path)
        {
            SetName(path);

            _path = path;
            Scale = new MyVector3(1, 1, 1) ;
            Rotation = new MyVector3();
            Position = new MyVector3();
        }

        private void SetName(string path)
        {
            if (NameDictionary == null) NameDictionary = new Dictionary<string, int>();

            if (!NameDictionary.ContainsKey(Path.GetFileNameWithoutExtension(path)))
            {
                NameDictionary.Add(Path.GetFileNameWithoutExtension(path), 0);
            }
            else
            {
                NameDictionary[Path.GetFileNameWithoutExtension(path)] += 1;
            }

            Name = String.Format("{0}_{1}", Path.GetFileNameWithoutExtension(path), NameDictionary[Path.GetFileNameWithoutExtension(path)]);
        }

        public Matrix GetWorldMatrix()
        {
            var worldMat = Matrix.Identity;
            worldMat *= Matrix.Scaling(Scale.Vector3);
            worldMat *= Matrix.RotationYawPitchRoll(MathUtil.DegreesToRadians(Rotation.Y),
                                                       MathUtil.DegreesToRadians(Rotation.X),
                                                       MathUtil.DegreesToRadians(Rotation.Z));
            worldMat *= Matrix.Translation(Position.Vector3);

            return worldMat;
        }
        public void Create(Device1 device)
        {
            //Set Primitive Topology
            PrimitiveTopology = PrimitiveTopology.TriangleList;

            //Set VertexStride
            VertexStride = Marshal.SizeOf(typeof(VertexPosColNorm));

            //Read file
            ParseFile(device, _path);

            //Create the bounding Sphere
            BoundingSphere = BoundingSphere.FromPoints(VertexList.ToArray());

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
                device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer,VertexStride, 0));

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
            if ( ray.Intersects(BoundingSphere) )
            {
                //Check if the ray intersects with a triangle from the mesh
                for (int j = 0; j < IndexCount / 3; j++)
                {
                    Vector3 v0 = VertexList[(int)IndexList[j * 3]];
                    Vector3 v1 = VertexList[(int)IndexList[j * 3 + 1]];
                    Vector3 v2 = VertexList[(int)IndexList[j * 3 + 2]];

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
                            verts.Add(new VertexPosColNorm(new Vector3(x, y, z) * Scale.Vector3, Color.Gray, Vector3.Zero));
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
