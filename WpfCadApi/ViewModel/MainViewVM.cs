using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfCadApi.Model;
using WpfCadApi.Services;

namespace WpfCadApi.ViewModel
{
    public class MainViewVM : INotifyPropertyChanged
    {
        #region Constructor
        public MainViewVM()
        {

            AutoCadLayers = AuctoCadContext.GetAllLayersInAutoCad();
            AutoCadObjects = AuctoCadContext.GetAllGraphicalObjects();
            CreateBtnCmd = new MyCommand(ExcuteCmd, CanExcuteCmd);

            CountOfElements = 0;
            CountOfLayeredElements = 0;
        }
        #endregion



        #region Properties
        public List<string> AutoCadLayers { get; set; } = new List<string>();
        private string _selectedLayer { get; set; }
        public string SelectedLayer
        {
            get { return _selectedLayer; }
            set
            {
                _selectedLayer = value;
                OnPropertyChanger();
                CountOfElements = AuctoCadContext.GetCountOfElement(SelectedObject);
                CountOfLayeredElements = AuctoCadContext.GetCountOfLayeredElement(SelectedObject, SelectedLayer);
            }

        }
        public List<string> AutoCadObjects { get; set; } = new List<string>();
        private string _selectedObject { get; set; }
        public string SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                _selectedObject = value;
                OnPropertyChanger();
                CountOfElements = AuctoCadContext.GetCountOfElement(SelectedObject);
                CountOfLayeredElements = AuctoCadContext.GetCountOfLayeredElement(SelectedObject, SelectedLayer);
            }

        }

        private int _countOfElements;
        public int CountOfElements
        {
            get => _countOfElements;
            private set
            {
                if (_countOfElements != value)
                {
                    _countOfElements = value;
                    OnPropertyChanger();
                }
            }
        }
        private int _countOfLayeredElements;
        public int CountOfLayeredElements
        {
            get => _countOfLayeredElements;
            private set
            {
                if (_countOfLayeredElements != value)
                {
                    _countOfLayeredElements = value;
                    OnPropertyChanger();

                }
            }
        }
        #endregion



        #region Methods
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanger([CallerMemberName] string Name = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(Name));

        }

        public void ExcuteCmd(object paremeter)
        {
            AuctoCadContext.AddObjectToLayer(SelectedLayer, SelectedObject);
            CountOfElements = AuctoCadContext.GetCountOfElement(SelectedObject);
            CountOfLayeredElements = AuctoCadContext.GetCountOfLayeredElement(SelectedObject, SelectedLayer);


        }

        public bool CanExcuteCmd(object parameter)
        {
            return true;
        }
        #endregion



        #region Commands
        public MyCommand CreateBtnCmd { get; set; }

        #endregion




    }
}
