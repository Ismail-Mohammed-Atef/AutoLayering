using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using WpfCadApi.Context;
using RibbonButton = Autodesk.Windows.RibbonButton;
using RibbonControl = Autodesk.Windows.RibbonControl;
using RibbonTab = Autodesk.Windows.RibbonTab;

namespace WpfCadApi.Services
{
    public class Ribbon : IExtensionApplication
    {
        public const string RibbonTitle = "GI UNION";
        public const string RibbonId = "10 10";
        public void Initialize()
        {
            CreateRibbon();
        }

        public void Terminate()
        {
            //throw new NotImplementedException();
        }

        [CommandMethod("RibbonCreator")]
        public void CreateRibbon()
        {
            RibbonControl ribbon = ComponentManager.Ribbon;
            if (ribbon != null)
            {
                RibbonTab rtab = ribbon.FindTab(Ribbon.RibbonId);
                if (rtab != null)
                {
                    ribbon.Tabs.Remove(rtab);
                }

                rtab = new RibbonTab();
                rtab.Title = Ribbon.RibbonTitle;
                rtab.Id = Ribbon.RibbonId;
                ribbon.Tabs.Add(rtab);
                AddContentToTab(rtab);
            }
        }

        private void AddContentToTab(RibbonTab rtab)
        {
            rtab.Panels.Add(AddPanelOne());
        }
        private static RibbonPanel AddPanelOne()
        {
            RibbonPanelSource rps = new RibbonPanelSource();
            rps.Title = "Filter Add-ins";
            RibbonPanel rp = new RibbonPanel();
            rp.Source = rps;
            RibbonButton rci = new RibbonButton();
            rci.Name = "GI";
            rps.DialogLauncher = rci;

            var addinAssembly = typeof(Ribbon).Assembly;
            var layerObjectFilter = new LayerObjectFilter();
            RibbonButton btnPythonshell = new RibbonButton
            {
                Orientation = Orientation.Vertical,
                AllowInStatusBar = true,
                Size = RibbonItemSize.Large,
                Name = "GI1",
                ShowText = true,
                ShowImage = true,
                Text = "Element-Layer Filter",
                Description = "Add selected elements to selected layer in one click",
                CommandHandler = new RelayCommand(param => layerObjectFilter.Execute())
            };

            try
            {
                string? assemblyName = addinAssembly.GetName().Name;
                var uri = new Uri($"pack://application:,,,/{assemblyName};component/Resources/link.png");
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = uri;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                // Assign images
                btnPythonshell.LargeImage = bitmap;
                btnPythonshell.Image = bitmap; 
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"Image load failed: {ex.Message}\nPath attempted: Resources/link.png");
            }          

            rps.Items.Add(btnPythonshell);
            return rp;
        }

       
    }
}