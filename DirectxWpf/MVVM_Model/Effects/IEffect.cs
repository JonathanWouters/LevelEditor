using SharpDX;
using SharpDX.Direct3D10;

namespace DirectxWpf.Effects
{
    public interface IEffect
    {
        bool IsInitialized { get; set; }
        EffectTechnique Technique { get; set; }
        Effect Effect { get; set; }
        InputLayout InputLayout { get; set; }

        void Initialize(Device1 device);
        void SetWorld(Matrix world);
        void SetWorldViewProjection(Matrix wvp);
        void SetLightDirection(Vector3 dir);
        void UpdateEffectVariable();
    }
}
