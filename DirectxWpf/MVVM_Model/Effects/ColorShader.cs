using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectxWpf.MVVM_Model.Models;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D10;
using Device1 = SharpDX.Direct3D10.Device1;
using System.IO;

namespace DirectxWpf.Effects
{
    public class ColorShader : IEffect
    {
        private bool _IsInitialized;
        public bool IsInitialized { get { return _IsInitialized; } set { } }
        public EffectTechnique Technique { get; set; }
        public Effect Effect { get; set; }
        public InputLayout InputLayout { get; set; }

        public Color Color { get; set; }

        public void Initialize(Device1 device)
        {
            //Load Effect
            var shaderSource = File.ReadAllText("Resources\\ColorShader.fx");
            var shaderByteCode = ShaderBytecode.Compile(shaderSource, "fx_4_0", ShaderFlags.None, EffectFlags.None);
            Effect = new Effect(device, shaderByteCode);
            Technique = Effect.GetTechniqueByIndex(0);

            //InputLayout
            var pass = Technique.GetPassByIndex(0);
            InputLayout = new InputLayout(device, pass.Description.Signature, InputLayouts.PosNormCol);

            _IsInitialized = true;
        }
        public ColorShader()
        {
            _IsInitialized = false;
        }
        public ColorShader(Color c)
        {
            Color = c;
            _IsInitialized = false;
        }
        public ColorShader(ColorShader other) 
        {
            Color = other.Color;
            _IsInitialized = false;
        }

        public void SetWorld(Matrix world)
        {
            if (!IsInitialized)
                return;

            if (Effect != null)
                Effect.GetVariableBySemantic("WORLD").AsMatrix().SetMatrix(world);
        }

        public void SetWorldViewProjection(Matrix wvp)
        {
            if (!IsInitialized)
                return;

            if (Effect != null)
                Effect.GetVariableBySemantic("WORLDVIEWPROJECTION").AsMatrix().SetMatrix(wvp);
        }


        public void SetLightDirection(Vector3 dir)
        {
            if (!IsInitialized)
                return;

            if (Effect != null)
                Effect.GetVariableByName("gLightDirection").AsVector().Set(dir);
        }

        public void UpdateEffectVariable()
        {
            if (!IsInitialized)
                return;

            if (Effect != null)
                Effect.GetVariableByName("gColor").AsVector().Set(Color.ToVector4());
        }
    }
}
