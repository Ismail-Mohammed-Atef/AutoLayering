using System.Collections.Generic;
using System.Windows;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using WpfCadApi.View;
using static WpfCadApi.Model.AuctoCadContext;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace WpfCadApi.Model
{
    public class AuctoCadContext
    {

        private static List<string> graphicalObjects = new List<string>();
        public static int GetCountOfElement(string objectName)
        {
            return GetCountOfLayeredElement(objectName);
        }
        public static int GetCountOfLayeredElement(string objName, string layerName = "")
        {
            if (layerName == null)
            {
                layerName = "null";
            }
            int count = 0;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;

                // First, get all block definition names
                foreach (ObjectId blockId in blockTable)
                {
                    BlockTableRecord btr = transaction.GetObject(blockId, OpenMode.ForRead) as BlockTableRecord;


                    if (!btr.IsAnonymous && !btr.IsLayout && !btr.Name.StartsWith("*") && btr.Name == objName)
                    {
                        foreach (ObjectId entId in btr)
                        {
                            Entity ent = transaction.GetObject(entId, OpenMode.ForRead) as Entity;

                            if (ent != null && ent.Layer.Contains(layerName))
                            {
                                count++;
                            }
                        }
                    }
                }

                BlockTableRecord modelSpace = transaction.GetObject(database.CurrentSpaceId, OpenMode.ForRead) as BlockTableRecord;

                foreach (ObjectId objectId in modelSpace)
                {
                    Entity entity = transaction.GetObject(objectId, OpenMode.ForRead) as Entity;
                    if (entity != null && entity.Layer.Contains(layerName))
                    {
                        if (entity.GetType().Name == objName)
                        {

                            count++;
                        }
                    }
                }

                transaction.Commit();
            }

            return count;
        }
        public static List<string> GetAllGraphicalObjects()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                BlockTable? blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;

                // First, get all block definition names
                foreach (ObjectId blockId in blockTable)
                {
                    BlockTableRecord? btr = transaction.GetObject(blockId, OpenMode.ForRead) as BlockTableRecord;

                    if (!btr.IsAnonymous && !btr.IsLayout && !btr.Name.StartsWith("*"))
                    {
                        
                        graphicalObjects.Add(btr.Name);
                    }
                }

                BlockTableRecord? modelSpace = transaction.GetObject(database.CurrentSpaceId, OpenMode.ForRead) as BlockTableRecord;

                foreach (ObjectId objectId in modelSpace)
                {
                    Entity? entity = transaction.GetObject(objectId, OpenMode.ForRead) as Entity;
                    if (entity != null)
                    {
                        if (entity is Line)
                        {
                            graphicalObjects.Add(entity.GetType().Name);
                        }
                        else if (entity is Circle)
                        {
                            graphicalObjects.Add(entity.GetType().Name);
                        }
                        else if (entity is Dimension)
                        {
                            graphicalObjects.Add(entity.GetType().Name);
                        }
                        else if (entity is DBText)
                        {
                            graphicalObjects.Add(entity.GetType().Name);
                        }
                        else if (entity is Hatch)
                        {
                            graphicalObjects.Add(entity.GetType().Name);
                        }
                    }
                }

                transaction.Commit();
            }

            return graphicalObjects.Distinct().ToList();
        }

        public static List<string> GetAllLayersInAutoCad()
        {
            List<string> CadLayerNames = new List<string>();

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor editor = doc.Editor;
            Database database = doc.Database;

            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                try
                {

                    LayerTable? LayersInAutoCad = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;

                    foreach (var item in LayersInAutoCad)
                    {
                        LayerTableRecord? ltr = transaction.GetObject(item, OpenMode.ForRead) as LayerTableRecord;

                        CadLayerNames.Add(ltr.Name);
                    }

                    transaction.Commit();
                }
                catch
                {

                }
            }

            return CadLayerNames;

        }
        public static void AddObjectToLayer(string layerName, string objectName)
        {
            int countOfChangedElements = 0;

            // Get the active document
            Document document = Application.DocumentManager.MdiActiveDocument;
            if (document == null)
            {
                Application.ShowAlertDialog("No active document found. Please open a drawing.");
                return;
            }

            Database database = document.Database;
            Editor editor = document.Editor;

            // Lock the document to allow modifications
            using (DocumentLock docLock = document.LockDocument())
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    try
                    {
                        BlockTable? blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord? modelSpace = transaction.GetObject(database.CurrentSpaceId, OpenMode.ForRead) as BlockTableRecord;

                        int objectsMoved = 0;

                        // Move block references to the specified layer
                        foreach (ObjectId blockId in blockTable)
                        {
                            BlockTableRecord? btr = transaction.GetObject(blockId, OpenMode.ForRead) as BlockTableRecord;

                            if (!btr.IsAnonymous && !btr.IsLayout && !btr.Name.StartsWith("*"))
                            {
                                if (btr.Name == objectName)
                                {
                                    btr.UpgradeOpen();

                                    foreach (ObjectId entId in btr)
                                    {
                                        Entity? ent = transaction.GetObject(entId, OpenMode.ForWrite) as Entity;
                                        if (ent != null)
                                        {
                                            ent.Layer = layerName;
                                            objectsMoved++;
                                            countOfChangedElements++;
                                        }
                                        else
                                        {
                                            editor.WriteMessage($"\nFailed to open entity with ID {entId}. It may not be a valid entity.");
                                        }
                                    }
                                }
                            }
                        }

                        // Move individual entities in model space to the specified layer
                        foreach (ObjectId objectId in modelSpace)
                        {
                            Entity? entity = transaction.GetObject(objectId, OpenMode.ForRead) as Entity;
                            
                            // Upgrade to write mode
                            entity.UpgradeOpen();

                            bool moved = false;
                            if (entity.GetType().Name == objectName)
                            {
                                entity.Layer = layerName;
                                moved = true;
                            }

                            if (moved)
                            {
                                objectsMoved++;
                                countOfChangedElements++;
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show($"Number Of Changed Elements = {countOfChangedElements}");
                        // Provide feedback
                        if (objectsMoved > 0)
                        {
                            editor.WriteMessage($"\nMoved {objectsMoved} {objectName}(s) to layer '{layerName}'");
                        }
                        else
                        {
                            editor.WriteMessage($"\nNo {objectName} objects found in Model Space");
                        }
                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception ex)
                    {
                        editor.WriteMessage($"\nAutoCAD Error: {ex.Message}");
                        transaction.Abort();
                    }
                    catch (System.Exception ex)
                    {
                        editor.WriteMessage($"\nSystem Error: {ex.Message}");
                        transaction.Abort();
                    }
                }
            }

            editor.Regen();
        }

        //[CommandMethod("RunThePlugin")]
        //public void RunThePlugin()
        //{
        //    MainWindow window = new MainWindow();

        //    Application.ShowModalWindow(window);

        //}
    }
}
