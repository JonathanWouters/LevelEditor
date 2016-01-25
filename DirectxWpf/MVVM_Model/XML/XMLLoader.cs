using DirectxWpf.Effects;
using DirectxWpf.Helpers;
using DirectxWpf.MVVM_Model.Components;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace DirectxWpf.MVVM_Model.XML
{
    public class XMLLoader
    {
        //*******************************************************//
        //                      METHODS                          //
        //*******************************************************//
        public XMLLoader()
        {
        }
       
        public void LoadXMLMap(string currentProjectPath)
        {
            // Create OpenFileDialog 
            OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".xmlMap";
            dlg.Filter = "Map Files (*.xmlMap)|*.xmlMap|xml Files (*.xml)|*.xml|txt Files (*.txt)|*.txt|Other Files (*.*)|*.*";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                LoadXMLMap(filename, currentProjectPath);

            }
        }

        public void LoadXMLMap(string path, string currentProjectPath)
        {
            XElement file = XElement.Load(path);
            if (file.Name != "Map")
            {
                MessageBox.Show("XML element <Map> not found.", "XMLLoader Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            XElement GameObjects = file.Element("GameObjects");
            if (GameObjects == null)
            {
                MessageBox.Show("XML element <GameObjects> not found in <Map>.", "XMLLoader Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (GameObjects.Elements("GameObject") != null)
            {
                foreach (XElement gameObject in GameObjects.Elements("GameObject"))
                {
                    OnAddingObject(LoadGameObject(gameObject, currentProjectPath));
                }
            }       
        }
        
        public void LoadXMLPrefab(string fileName, string currentProjectPath)
        {
            // Open document 
            XElement file = XElement.Load(fileName);
            if (file.Name != "Prefab")
            {
                MessageBox.Show("XML element <Prefab> not found.", "XMLLoader Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            XElement GameObjects = file.Element("GameObjects");
            if (GameObjects == null)
            {
                MessageBox.Show("XML element <GameObjects> not found in <Prefab>.", "XMLLoader Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (GameObjects.Elements("GameObject") != null)
            {
                foreach (XElement gameObject in GameObjects.Elements("GameObject"))
                {
                    OnAddingObject(LoadGameObject(gameObject, currentProjectPath));
                }
            }
        }

        private GameObject LoadGameObject(XElement xElement, string currentProjectPath)
        {
            GameObject gameObject = new GameObject();
            if((string)xElement.Attribute("Name") != null)
                gameObject.Name = (string)xElement.Attribute("Name");

            LoadComponents( gameObject, xElement.Element("Components"),currentProjectPath );

            foreach (XElement childObject in xElement.Elements("GameObject"))
            {
                gameObject.AddChild( LoadGameObject(childObject,currentProjectPath),false );
            }
 
            return gameObject;
        }

        private void LoadComponents(GameObject gameobject, XElement components, string currentProjectPath) 
        {
            if (components == null)
            {
                //Warn user that filename was not found
                MessageBox.Show(String.Format("{0}: No components found", gameobject.Name), "XMLLoader Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            XElement xTranformComponent = components.Element("TransformComponent");
            if (xTranformComponent != null)
            {
                XElement xPosition = xTranformComponent.Element("Position");
                if (xPosition != null)
                {
                    float parseOut;
                    XElement xX = xPosition.Element("X");
                    if (xX != null)
                    {
                        if (!float.TryParse((string)xX.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out parseOut)) 
                        {
                            MessageBox.Show(String.Format("Parse error in Transform component: {0}", gameobject.Name), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            parseOut = 0;
                        }
                        gameobject.Transform.Position.X = parseOut;
                    }

                    XElement xY = xPosition.Element("Y");
                    if (xY != null)
                    {
                        if (!float.TryParse((string)xY.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out parseOut))
                        {
                            MessageBox.Show(String.Format("Parse error in Transform component: {0}", gameobject.Name), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            parseOut = 0;
                        }
                        gameobject.Transform.Position.Y = parseOut;
                    }

                    XElement xZ = xPosition.Element("Z");
                    if (xZ != null)
                    {
                        if (!float.TryParse((string)xZ.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out parseOut))
                        {
                            MessageBox.Show(String.Format("Parse error in Transform component: {0}", gameobject.Name), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            parseOut = 0;
                        }
                        gameobject.Transform.Position.Z = parseOut;
                    }
                }

                XElement xRotation = xTranformComponent.Element("Rotation");
                if (xRotation != null)
                {
                    float parseOut;
                    XElement xX = xRotation.Element("X");
                    if (xX != null)
                    {
                        if (!float.TryParse((string)xX.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out parseOut))
                        {
                            MessageBox.Show(String.Format("Parse error in Transform component: {0}", gameobject.Name), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            parseOut = 0;
                        }
                        gameobject.Transform.RotationEuler.X = parseOut;
                    }

                    XElement xY = xRotation.Element("Y");
                    if (xY != null)
                    {
                        if (!float.TryParse((string)xY.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out parseOut))
                        {
                            MessageBox.Show(String.Format("Parse error in Transform component: {0}", gameobject.Name), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            parseOut = 0;
                        }
                        gameobject.Transform.RotationEuler.Y = parseOut;
                    }

                    XElement xZ = xRotation.Element("Z");
                    if (xZ != null)
                    {
                        if (!float.TryParse((string)xZ.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out parseOut))
                        {
                            MessageBox.Show(String.Format("Parse error in Transform component: {0}", gameobject.Name), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            parseOut = 0;
                        }
                        gameobject.Transform.RotationEuler.Z = parseOut;
                    }
                }

                XElement xScale = xTranformComponent.Element("Scale");
                if (xScale != null)
                {
                    float parseOut;
                    XElement xX = xScale.Element("X");
                    if (xX != null)
                    {
                        if (!float.TryParse((string)xX.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out parseOut))
                        {
                            MessageBox.Show(String.Format("Parse error in Transform component: {0}", gameobject.Name), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            parseOut = 0;
                        }
                        gameobject.Transform.Scale.X = parseOut;
                    }

                    XElement xY = xScale.Element("Y");
                    if (xY != null)
                    {
                        if (!float.TryParse((string)xY.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out parseOut))
                        {
                            MessageBox.Show(String.Format("Parse error in Transform component: {0}", gameobject.Name), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            parseOut = 0;
                        }
                        gameobject.Transform.Scale.Y = parseOut;
                    }

                    XElement xZ = xScale.Element("Z");
                    if (xZ != null)
                    {
                        if (!float.TryParse((string)xZ.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out parseOut))
                        {
                            MessageBox.Show(String.Format("Parse error in Transform component: {0}", gameobject.Name), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            parseOut = 0;
                        }
                        gameobject.Transform.Scale.Z = parseOut;
                    }
                }
            }
            else
            {
                MessageBox.Show(String.Format("{0}: No TransformComponent found", gameobject.Name), "XMLLoader Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            XElement xModelComponent = components.Element("ModelComponent");
            if (xModelComponent != null)
            {
                XElement xFilePath = xModelComponent.Element("FilePath");
                if (xFilePath != null)
                {
                    string fullFilePath = currentProjectPath + "\\" + (string)xFilePath.Value;
                    if (!File.Exists(fullFilePath))
                    {
                        MessageBox.Show("ModelComponent: File does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    ModelComponent modelComponent = new ModelComponent(fullFilePath);
                    modelComponent.Shader = new PosNormColEffect();
                    gameobject.AddComponent(modelComponent);
                }
                else 
                {
                    //Warn user that filename was not found
                    MessageBox.Show(String.Format("{0}: ModelComponent was found but no filename", gameobject.Name), "XMLLoader Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }    
        }


        //*******************************************************//
        //                      EVENTS                           //
        //*******************************************************//
        public event EventHandler<GameObjectEventArgs> AddingObject;
        protected virtual void OnAddingObject(GameObject gameObject)
        {
            if (AddingObject != null)
                AddingObject(this, new GameObjectEventArgs() { GameObject = gameObject });
        }
    }

}
