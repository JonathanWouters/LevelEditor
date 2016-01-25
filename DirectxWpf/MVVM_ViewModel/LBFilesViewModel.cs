using DirectxWpf.MVVM_Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;


namespace DirectxWpf.MVVM_ViewModel
{
    public class LBFilesViewModel: BaseViewModel
    {
        //*******************************************************//
        //                      FIELDS                           //
        //*******************************************************//
        private ObservableCollection<FileModel> _FilesList;
        private int _SelectedFilterIndex;
        private string _Filter;

        //*******************************************************//
        //                      PROPERTIES                       //
        //*******************************************************//
        public ObservableCollection<FileModel> FilesList
        {
            get
            {
                if (_FilesList == null)
                {
                    _FilesList = new ObservableCollection<FileModel>();
                }

                IEnumerable<FileModel> filterdList;
                switch (SelectedFilterIndex)
                {
                    case 0:
                        filterdList = from file in _FilesList
                                      where (file.Extension.ToLower() == ".ovm" ||
                                      file.Extension.ToLower() == ".xmlprefab") &&
                                      file.FileName.ToLower().Contains(Filter.ToLower())
                                      select file;
                        break;
                    case 1:
                        filterdList = from file in _FilesList
                                      where (file.Extension.ToLower() == ".ovpc" ||
                                      file.Extension.ToLower() == ".ovpt") &&
                                      file.FileName.ToLower().Contains(Filter.ToLower())
                                      select file;
                        break;
                    case 2:
                        filterdList = from file in _FilesList
                                      where (file.Extension.ToLower() == ".xmlmap" ||
                                      file.Extension.ToLower() == ".map") &&
                                      file.FileName.ToLower().Contains(Filter.ToLower())
                                      select file;
                        break;
                    default:
                        filterdList = from file in _FilesList
                                      where file.FileName.ToLower().Contains(Filter.ToLower())
                                      select file;
                        break;
                }

                return new ObservableCollection<FileModel>(filterdList);
            }
            set 
            {
                _FilesList = value;
                OnPropertyChanged("FilesList");
            }
        }
        public int SelectedFilterIndex 
        {
            get { return _SelectedFilterIndex; }
            set { _SelectedFilterIndex = value; OnPropertyChanged("SelectedFilterIndex"); OnPropertyChanged("FilesList"); } 
        }
        public string Filter
        {
            get {  if(_Filter == null) return ""; return _Filter; }
            set { _Filter = value; OnPropertyChanged("Filter"); OnPropertyChanged("FilesList"); }
        }

        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public void ChangeDirectory(TreeNode directory)
        {
            if (_FilesList == null)
                _FilesList = new ObservableCollection<FileModel>();

            _FilesList.Clear();
            if (directory != null)
            {
                if (Directory.Exists(directory.Path))
                {
                    var directoryInfo = new DirectoryInfo(directory.Path);
                    foreach (var file in directoryInfo.GetFiles())
                        _FilesList.Add(new FileModel(file.FullName));
                }
            }
            OnPropertyChanged("FilesList");
        }


        //*******************************************************//
        //                      COMMANDS                         //
        //*******************************************************//
        private RelayCommand<TreeNode> _ChangeDirectoryCommand;
        public RelayCommand<TreeNode> ChangeDirectoryCommand
        {
            get { return _ChangeDirectoryCommand ?? (_ChangeDirectoryCommand = new RelayCommand<TreeNode>(ChangeDirectory)); }
        }
    }
}
