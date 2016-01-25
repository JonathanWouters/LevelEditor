using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectxWpf.MVVM_Model
{
    public class MyVector3 :  INotifyPropertyChanged
    {
        //*******************************************************//
        //                      FIELDS                           //
        //*******************************************************//
        private Vector3 _vector;


        //*******************************************************//
        //                      PROPERTIES                       //
        //*******************************************************// 
        public float X { get { return _vector.X; } set { _vector = new Vector3(value,_vector.Y,_vector.Z); OnPropertyChanged("X"); } }
        public float Y { get { return _vector.Y; } set { _vector = new Vector3(_vector.X, value, _vector.Z); OnPropertyChanged("Y"); } }
        public float Z { get { return _vector.Z; } set { _vector = new Vector3(_vector.X, _vector.Y, value); OnPropertyChanged("Z"); } }


        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public Vector3 Vector3
        {
            get { return _vector; }
            set 
            {
                _vector = value;
                X = value.X; 
                Y = value.Y; 
                Z = value.Z;
                OnPropertyChanged("X");
                OnPropertyChanged("Y");
                OnPropertyChanged("Z");
            }

        }

        public MyVector3() { }

        public MyVector3(float x, float y, float z) 
        {
            X = x;
            Y = y;
            Z = z;
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
