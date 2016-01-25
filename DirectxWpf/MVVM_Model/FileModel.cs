using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DirectxWpf.MVVM_Model
{
    public class FileModel
    {
        //*******************************************************//
        //                      PROPERTIES                       //
        //*******************************************************//  
        public string FilePath { get; set; }
        public string Extension { get { return Path.GetExtension(FilePath); } }
        public string NoDotExtension
        {
            get
            {
                if (Extension.Length > 0)
                {
                    if (Extension.ToUpper().Substring(1) == "XMLPREFAB")
                    {
                        return "PREFAB";
                    }
                    if (Extension.ToUpper().Substring(1) == "XMLMAP")
                    {
                        return "MAP";
                    }
                    return Extension.ToUpper().Substring(1);
                }
                return "";
            }
        }
        public string FileName { get { return Path.GetFileNameWithoutExtension(FilePath); } }


        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public FileModel() 
        { 
        }

        public FileModel(string path)
        {
            FilePath = path;
        }
    }
}
