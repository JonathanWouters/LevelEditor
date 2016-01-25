using DirectxWpf.MVVM_Model.Components;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DirectxWpf.MVVM_Model.XML
{
    public class XMLSaver
    {
        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public XMLSaver() 
        { 
        
        }

        public void SaveMap(List<GameObject> objectList, string currentProjectFolder)
        {            
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xmlMap";
            saveFileDialog.Filter = "Map Files (*.xmlMap)|*.xmlMap";
            
            if (saveFileDialog.ShowDialog() == true)
            {

                using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    using (XmlTextWriter w = new XmlTextWriter(fs, Encoding.UTF8))
                    {
                        w.Formatting = Formatting.Indented;
                        w.Indentation = 4;
                        w.WriteStartDocument();
                        w.WriteStartElement("Map");
                        w.WriteStartElement("GameObjects");

                        saveGameObjects(objectList, w,currentProjectFolder);
                        
                        w.WriteEndElement();
                        w.WriteEndElement();
                        w.WriteEndDocument();
                    }
                }
            }
        }

        public void SavePrefab(GameObject gameObject, string currentProjectFolder) 
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xmlPrefab";
            saveFileDialog.Filter = "Prefab Files (*.xmlPrefab)|*.xmlPrefab|Other Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    using (XmlTextWriter w = new XmlTextWriter(fs, Encoding.UTF8))
                    {
                        w.Formatting = Formatting.Indented;
                        w.Indentation = 4;
                        w.WriteStartDocument();
                        w.WriteStartElement("Prefab");
                            w.WriteStartElement("GameObjects");
                                w.WriteStartElement("GameObject");
                                w.WriteAttributeString("Name", gameObject.Name);

                                SaveComponents(gameObject, w,currentProjectFolder);
                                saveGameObjects(gameObject.Children.ToList<GameObject>(), w,currentProjectFolder);

                                w.WriteEndElement();
                           w.WriteEndElement();
                        w.WriteEndElement();
                        w.WriteEndDocument();
                    }
                }
            }            
        }

        private void saveGameObjects(List<GameObject> objectList, XmlTextWriter w,string currentProjectFolder) 
        { 
            foreach (var gameObject in objectList)
            {
                w.WriteStartElement("GameObject");
                w.WriteAttributeString("Name", gameObject.Name);
                
                SaveComponents(gameObject, w,currentProjectFolder);
                
                saveGameObjects(gameObject.Children.ToList<GameObject>(),w,currentProjectFolder);
                w.WriteEndElement();
            }        
        }

        private void SaveComponents(GameObject gameObject, XmlTextWriter w,string currentProjectFolder) 
        {
            w.WriteStartElement("Components");
            foreach (var component in gameObject.Components)
            {
                if (component.GetType() == typeof(TransformComponent))
                {
                    var transform = component as TransformComponent;
                    w.WriteStartElement("TransformComponent");

                    w.WriteStartElement("Position");
                    w.WriteStartElement("X");
                    w.WriteString(transform.Position.X.ToString(CultureInfo.InvariantCulture));
                    w.WriteEndElement();
                    w.WriteStartElement("Y");
                    w.WriteString(transform.Position.Y.ToString(CultureInfo.InvariantCulture));
                    w.WriteEndElement();
                    w.WriteStartElement("Z");
                    w.WriteString(transform.Position.Z.ToString(CultureInfo.InvariantCulture));
                    w.WriteEndElement();
                    w.WriteEndElement();

                    w.WriteStartElement("Rotation");
                    w.WriteStartElement("X");
                    w.WriteString(transform.RotationEuler.X.ToString(CultureInfo.InvariantCulture));
                    w.WriteEndElement();
                    w.WriteStartElement("Y");
                    w.WriteString(transform.RotationEuler.Y.ToString(CultureInfo.InvariantCulture));
                    w.WriteEndElement();
                    w.WriteStartElement("Z");
                    w.WriteString(transform.RotationEuler.Z.ToString(CultureInfo.InvariantCulture));
                    w.WriteEndElement();
                    w.WriteEndElement();

                    w.WriteStartElement("Scale");
                    w.WriteStartElement("X");
                    w.WriteString(transform.Scale.X.ToString(CultureInfo.InvariantCulture));
                    w.WriteEndElement();
                    w.WriteStartElement("Y");
                    w.WriteString(transform.Scale.Y.ToString(CultureInfo.InvariantCulture));
                    w.WriteEndElement();
                    w.WriteStartElement("Z");
                    w.WriteString(transform.Scale.Z.ToString(CultureInfo.InvariantCulture));
                    w.WriteEndElement();
                    w.WriteEndElement();

                    w.WriteEndElement();
                }

                if (component.GetType() == typeof(ModelComponent))
                {
                    var modelComponent = component as ModelComponent;
                    w.WriteStartElement("ModelComponent");
                    w.WriteStartElement("FilePath");
                    string relativePath = FaultTolerantRelativePath(modelComponent.FilePath, currentProjectFolder);
                    w.WriteString(relativePath);
                    w.WriteEndElement();
                    w.WriteEndElement();

                }
            }
            w.WriteEndElement();            
        }

        private string FaultTolerantRelativePath(string absolutePath, string basePath)
        {
            if (absolutePath == null || basePath == null)
                return null;

            absolutePath = absolutePath.Replace(System.IO.Path.DirectorySeparatorChar, '\\');
            basePath = basePath.Replace(System.IO.Path.DirectorySeparatorChar, '\\');

            if (!basePath.EndsWith("\\"))
                basePath += "\\";


            if (absolutePath.Length < basePath.Length)
                throw new ArgumentException("absolutePath.Length < basePath.Length ? This can't be. You mixed up absolute and base path.");

            string resultingPath = absolutePath.Substring(basePath.Length);
            resultingPath = resultingPath.Replace('\\', System.IO.Path.DirectorySeparatorChar);

            return resultingPath;
        }
    }
}
