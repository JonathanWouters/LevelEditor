using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectxWpf.Behavior
{
    public interface IDropable
    {
        Type DataType { get; }

        void Drop(object data);
    }
}
