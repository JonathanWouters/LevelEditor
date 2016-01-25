using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectxWpf.MVVM_Model.Managers
{
    public static class NameManeger
    {
        public static Dictionary<string, int> NameDictionary;
        public static string GetNameFromPath(string path)
        {
            if (NameDictionary == null) NameDictionary = new Dictionary<string, int>();

            if (!NameDictionary.ContainsKey(Path.GetFileNameWithoutExtension(path)))
            {
                NameDictionary.Add(Path.GetFileNameWithoutExtension(path), 0);
            }
            else
            {
                NameDictionary[Path.GetFileNameWithoutExtension(path)] += 1;
            }

            return String.Format("{0}_{1}", Path.GetFileNameWithoutExtension(path), NameDictionary[Path.GetFileNameWithoutExtension(path)]);
        }

        public static string GetNameFromString(string str)
                {
            if (NameDictionary == null) NameDictionary = new Dictionary<string, int>();

            if (!NameDictionary.ContainsKey(str))
            {
                NameDictionary.Add(str, 0);
            }
            else
            {
                NameDictionary[str] += 1;
            }

            return String.Format("{0}_{1}", str, NameDictionary[str]);
        }
    }
}
