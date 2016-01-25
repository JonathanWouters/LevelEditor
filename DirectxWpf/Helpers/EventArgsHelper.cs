using DirectxWpf.MVVM_Model;
using DirectxWpf.MVVM_ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectxWpf.Helpers
{
    public class GameObjectEventArgs : EventArgs
    {
        public GameObject GameObject { get; set; }
    }

    public class GOReassigningEventArgs : EventArgs
    {
        public GameObject GameObject { get; set; }
        public GameObject NewParent { get; set; }
    }

    public class ElementReassigningEventArgs : EventArgs
    {
        public TRVGameObjectElementViewModel Element { get; set; }
        public TRVGameObjectElementViewModel NewParent { get; set; }
    }
}
