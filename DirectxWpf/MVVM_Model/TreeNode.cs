using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DirectxWpf.MVVM_Model
{
    public class TreeNode: INotifyPropertyChanged
    {
        //*******************************************************//
        //                      FIELDS                           //
        //*******************************************************//
        private string _Title;
        private string _Path;


        //*******************************************************//
        //                      PROPERTIES                       //
        //*******************************************************//  
        public string Title 
        { 
            get { return _Title; }
            set { _Title = value; OnPropertyChanged("Title"); } 
        }
        public string Path 
        { 
            get { return _Path; } 
            set { _Path = value; OnPropertyChanged("Path"); OnPropertyChanged("Extension"); } 
        }
        public ObservableCollection<TreeNode> Items { get; set; }


        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public TreeNode()
        {
            this.Items = new ObservableCollection<TreeNode>();
        }
        public TreeNode( string title,string path)
        {
            this.Items = new ObservableCollection<TreeNode>();
            Title = title;
            Path = path;
        }

        //*******************************************************//
        //                      EVENTS                           //
        //*******************************************************//
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
