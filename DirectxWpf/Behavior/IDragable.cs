using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectxWpf.Behavior
{
    public interface IDragable
    {
        Type DataType { get; }

        void Remove(object i);
    }
}
