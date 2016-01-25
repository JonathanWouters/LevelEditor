using DirectxWpf.MVVM_Model.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirectxWpf.MVVM_ViewModel
{
    public class SnapWindowViewModel:BaseViewModel
    {
        //*******************************************************//
        //                      FIELDS                           //
        //*******************************************************//
        private Gizmo _Gizmo;

        //*******************************************************//
        //                      PROPERTIES                       //
        //*******************************************************//  
        public Gizmo Gizmo { get { return _Gizmo; } set { _Gizmo = value; OnPropertyChanged("Gizmo"); } }


        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public SnapWindowViewModel()
        {
        }

        public void Close(Window parameter)
        {
            parameter.DialogResult = true;
        }


        //*******************************************************//
        //                      COMMANDS                         //
        //*******************************************************//
        private RelayCommand<Window> _CloseCommand;
        public RelayCommand<Window> CloseCommand
        {
            get { return _CloseCommand ?? (_CloseCommand = new RelayCommand<Window>(Close)); }
            private set { _CloseCommand = value; }
        }

    }
}
