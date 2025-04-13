using Autodesk.AutoCAD.ApplicationServices;
using WpfCadApi.Services;
using WpfCadApi.View;

namespace WpfCadApi.Context
{
    public class LayerObjectFilter : ICadCommand
    {
        public void Execute()
        {
            RunThePlugin();
        }

        public void RunThePlugin()
        {
            MainWindow window = new MainWindow();
            Application.ShowModalWindow(window);
        }
    }
}