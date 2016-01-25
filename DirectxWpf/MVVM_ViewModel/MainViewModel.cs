using GalaSoft.MvvmLight.Command;
using DirectxWpf.MVVM_Model;
using DirectxWpf.MVVM_View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using SharpDX;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.IO;
using DirectxWpf.MVVM_Model.Models;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using DirectxWpf.MVVM_Model.XML;
using DirectxWpf.MVVM_Model.Managers;
using DirectxWpf.MVVM_Model.Components;
using DirectxWpf.Effects;


namespace DirectxWpf.MVVM_ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        //*******************************************************//
        //                      FIELDS                           //
        //*******************************************************//
        private DX10Viewport _Viewport10;
        private TRVGameObjectViewModel _GOTreeViewModel;
        private LBFilesViewModel _LBFilesViewModel;
        private TRVDirectoriesViewModel _TRVDirectoriesViewModel;
        private GameObjectManager _GOManager;
        private string _CurrentProjectPath;
        private bool _IsProjectLoaded;

        //*******************************************************//
        //                      PROPERTIES                       //
        //*******************************************************//        
        public DX10Viewport Viewport10
        {
            get {return _Viewport10;}
            private set { _Viewport10 = value; }
        }
        public TRVGameObjectViewModel GoTreeViewModel
        {
            get
            {
                return _GOTreeViewModel;
            }
            private set
            {
                _GOTreeViewModel = value;
                OnPropertyChanged("GoTreeViewModel");
            }
        }
        public LBFilesViewModel LBFilesViewModel
        {
            get { return _LBFilesViewModel; }
            private set { _LBFilesViewModel = value; OnPropertyChanged("LBFilesViewModel"); }
        }
        public TRVDirectoriesViewModel TRVDirectoriesViewModel
        {
            get { return _TRVDirectoriesViewModel; }
            private set { _TRVDirectoriesViewModel = value; OnPropertyChanged("TRVDirectoriesViewModel"); }
        }
        public GameObjectManager GoManager 
        { 
            get { return _GOManager; } 
            set { _GOManager = value; OnPropertyChanged("GoManager"); } 
        }
        public bool IsProjectLoaded 
        {
            get { return _IsProjectLoaded; }
            private set { _IsProjectLoaded = value; OnPropertyChanged("IsProjectLoaded"); } 
        }

        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public MainWindowViewModel()
        {
            IsProjectLoaded = false;

            Viewport10 = new DX10Viewport();
            GoManager = GameObjectManager.Instance();

            GoTreeViewModel = new TRVGameObjectViewModel();
            LBFilesViewModel = new LBFilesViewModel();
            TRVDirectoriesViewModel = new TRVDirectoriesViewModel();

            //Assign Listeners to events
            GoTreeViewModel.GameObjectReassigning += GoManager.OnReassigning;
            GoManager.ObjectAdded += GoTreeViewModel.OnGameObectAdded;
            GoManager.ObjectRemoved += GoTreeViewModel.OnGameObectRemoved;
        }

        private void ViewportGotFocus()
        {
            Viewport10.Camera.IsActive = true;
        }

        private void ViewportLostFocus()
        {
            Viewport10.Camera.IsActive = false;
        }

        private void SetGizmo(RadioButton r)
        {
            if ((string)r.Tag == "Translate")
            {
                if (r.IsChecked == true)
                {
                    Viewport10.Gizmo.Mode = GizmoMode.Translate;
                }
                else
                {
                    Viewport10.Gizmo.Mode = GizmoMode.None;
                }
            }

            if ((string)r.Tag == "Scale")
            {
                if (r.IsChecked == true)
                {
                    Viewport10.Gizmo.Mode = GizmoMode.Scale;
                }
                else
                {
                    Viewport10.Gizmo.Mode = GizmoMode.None;
                }
            }

            if ((string)r.Tag == "Rotate")
            {
                if (r.IsChecked == true)
                {
                    Viewport10.Gizmo.Mode = GizmoMode.Rotate;
                }
                else
                {
                    Viewport10.Gizmo.Mode = GizmoMode.None;
                }
            }
        }

        private void AddEmpty()
        {
            //Viewport10.AddEmpty();
            GameObject Empty = new GameObject();
            Empty.Name = NameManeger.GetNameFromString("Empty");
            UndoRedoStack.ClearRedoStack();
            GameObjectManager.Instance().AddGameObject(Empty);
        }
        private void ItemDoubleClicked(FileModel file)
        {
            if (file.Extension.ToLower() == ".ovm")
            {
                var obj = new GameObject();
                obj.Name = NameManeger.GetNameFromPath(file.FilePath);

                var modelcomp = new ModelComponent(file.FilePath);
                modelcomp.Shader = new PosNormColEffect();
                obj.AddComponent(modelcomp);
                UndoRedoStack.ClearRedoStack();
                GameObjectManager.Instance().AddGameObject(obj);
            }
            if (file.Extension.ToLower() == ".xmlprefab")
            {
                LoadPrefab(file.FilePath);
            }
            if (file.Extension.ToLower() == ".xmlmap")
            {
                GoManager.Clear();

                XMLLoader xmlLoader = new XMLLoader();
                xmlLoader.AddingObject += GoManager.OnAddingObject;
                xmlLoader.LoadXMLMap(file.FilePath,_CurrentProjectPath);
                UndoRedoStack.Clear();
            }
        }

        private void SaveXml()
        {
            XMLSaver xmlSaver = new XMLSaver();
            xmlSaver.SaveMap(GoManager.ObjectList.ToList<GameObject>(),_CurrentProjectPath);
        }

        private void LoadXml()
        {
            GoManager.Clear();

            XMLLoader xmlLoader = new XMLLoader();
            xmlLoader.AddingObject += GoManager.OnAddingObject;
            xmlLoader.LoadXMLMap(_CurrentProjectPath);
            UndoRedoStack.Clear();
        }

        public void OpenSnapWindow()
        {
            var dlg = new SnapWindow();
            SnapWindowViewModel model = dlg.DataContext as SnapWindowViewModel;
            model.Gizmo = Viewport10.Gizmo;

            var result = dlg.ShowDialog();
            if (result != true)
            {

                return;
            }
        }

        private void SavePrefab(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            XMLSaver xmlSaver = new XMLSaver();
            xmlSaver.SavePrefab(gameObject,_CurrentProjectPath);
        }

        private void LoadPrefab(string filePath)
        {
            XMLLoader xmlLoader = new XMLLoader();
            xmlLoader.AddingObject += GoManager.OnAddingObject;
            xmlLoader.LoadXMLPrefab(filePath,_CurrentProjectPath);
        }

        private void OpenProject() 
        { 
            // Create OpenFileDialog 
            OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".sln";
            dlg.Filter = "OverlordProject (*.sln)|*.sln";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                GoManager.Clear();
                // Open document 
               _CurrentProjectPath = dlg.FileName;
               _CurrentProjectPath = Path.GetDirectoryName(_CurrentProjectPath);

               if ( Directory.Exists(_CurrentProjectPath + "\\Resources") )
               {
                   TRVDirectoriesViewModel.LoadTree(_CurrentProjectPath + "\\Resources");
                   IsProjectLoaded = true;
                   UndoRedoStack.Clear();
               }
               else 
               {
                   MessageBox.Show("Resources folder not found !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                   _CurrentProjectPath = "";
                   IsProjectLoaded = false;
               }
            }


        }

        private void CloseProject() 
        {
            _CurrentProjectPath = "";
            IsProjectLoaded = false;
            GoManager.Clear();
            UndoRedoStack.Clear();
            LBFilesViewModel.ChangeDirectory(null);
            TRVDirectoriesViewModel.Clear();
        }

        private void CloseMap()
        {
            GoManager.Clear();
            UndoRedoStack.Clear();
        }

        private void Undo()
        {
            UndoRedoStack.Undo();
        }

        private void Redo()
        {
            UndoRedoStack.Redo();
        }

        //*******************************************************//
        //                      COMMANDS                         //
        //*******************************************************//
        private RelayCommand _viewportGotFocusCommand;
        public RelayCommand ViewportGotFocusCommand
        {
            get { return _viewportGotFocusCommand ?? (_viewportGotFocusCommand = new RelayCommand(ViewportGotFocus)); }
        }


        private RelayCommand _viewportLostFocusCommand;
        public RelayCommand ViewportLostFocusCommand
        {
            get { return _viewportLostFocusCommand ?? (_viewportLostFocusCommand = new RelayCommand(ViewportLostFocus)); }
        }


        private RelayCommand<RadioButton> _setGizmoCommand;
        public RelayCommand<RadioButton> SetGizmoCommand
        {
            get { return _setGizmoCommand ?? (_setGizmoCommand = new RelayCommand<RadioButton>(SetGizmo)); }
        }


        private RelayCommand _addEmptyCommand;
        public RelayCommand AddEmptyCommand
        {
            get { return _addEmptyCommand ?? (_addEmptyCommand = new RelayCommand(AddEmpty)); }
        }


        private RelayCommand<FileModel> _itemDoubleClickCommand;
        public RelayCommand<FileModel> ItemDoubleClickCommand
        {
            get { return _itemDoubleClickCommand ?? (_itemDoubleClickCommand = new RelayCommand<FileModel>(ItemDoubleClicked)); }
        }


        private RelayCommand _SaveXmlCommand;
        public RelayCommand SaveXmlCommand
        {
            get { return _SaveXmlCommand ?? (_SaveXmlCommand = new RelayCommand(SaveXml)); }
        }


        private RelayCommand _LoadXmlCommand;
        public RelayCommand LoadXmlCommand
        {
            get { return _LoadXmlCommand ?? (_LoadXmlCommand = new RelayCommand(LoadXml)); }
        }


        private RelayCommand _OpenSnapWindowCommand;
        public RelayCommand OpenSnapWindowCommand
        {
            get { return _OpenSnapWindowCommand ?? (_OpenSnapWindowCommand = new RelayCommand(OpenSnapWindow)); }
        }


        private RelayCommand<GameObject> _SavePrefabCommand;
        public RelayCommand<GameObject> SavePrefabCommand
        {
            get { return _SavePrefabCommand ?? (_SavePrefabCommand = new RelayCommand<GameObject>(SavePrefab)); }
        }


        private RelayCommand _OpenProjectCommand;
        public RelayCommand OpenProjectCommand
        {
            get { return _OpenProjectCommand ?? (_OpenProjectCommand = new RelayCommand(OpenProject)); }
        }


        private RelayCommand _CloseProjectCommand;
        public RelayCommand CloseProjectCommand
        {
            get { return _CloseProjectCommand ?? (_CloseProjectCommand = new RelayCommand(CloseProject)); }
        }


        private RelayCommand _CloseMapCommand;
        public RelayCommand CloseMapCommand
        {
            get { return _CloseMapCommand ?? (_CloseMapCommand = new RelayCommand(CloseMap)); }
        }

        private RelayCommand _UndoCommand;
        public RelayCommand UndoCommand
        {
            get { return _UndoCommand ?? (_UndoCommand = new RelayCommand(Undo)); }
        }

        private RelayCommand _RedoCommand;
        public RelayCommand RedoCommand
        {
            get { return _RedoCommand ?? (_RedoCommand = new RelayCommand(Redo)); }
        }
    }
}
