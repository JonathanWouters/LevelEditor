using SharpDX.Direct3D10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectxWpf.MVVM_Model.Components
{
    public interface IBaseComponent
    {
        GameObject GameObject { get; set; }
        TransformComponent GetTransform();
        void Update(float deltaTime, Camera camera);
        void Render(Device1 device);

    }
}
