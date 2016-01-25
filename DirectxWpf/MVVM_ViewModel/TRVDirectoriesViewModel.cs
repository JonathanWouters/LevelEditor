using DirectxWpf.MVVM_Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectxWpf.MVVM_ViewModel
{
    public class TRVDirectoriesViewModel: BaseViewModel
    {
        //*******************************************************//
        //                      FIELDS                           //
        //*******************************************************//
        private TreeNode _Root;

        //*******************************************************//
        //                      PROPERTIES                       //
        //*******************************************************//       
        public TreeNode Root 
        {
            get { return _Root; } 
            set 
            {
                _Root = value;
                OnPropertyChanged("Root"); 
            } 
        }

        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public TRVDirectoriesViewModel() 
        {
            _Root = new TreeNode() { Title = "Root" };
        }

        public void LoadTree(string path)
        {
            if (Directory.Exists(path))
            {
                Root.Items.Clear();
                var rootDirectoryInfo = new DirectoryInfo(path);
                Root.Items.Add(CreateDirectoryNode(rootDirectoryInfo));
            }
        }

        public void Clear() 
        {
            Root.Items.Clear();      
        }

        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name, directoryInfo.FullName);
            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Items.Add(CreateDirectoryNode(directory));
            return directoryNode;
        }

        
    }
}
