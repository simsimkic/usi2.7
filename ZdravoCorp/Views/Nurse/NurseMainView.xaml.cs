using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.ViewModels.Nurse;

namespace ZdravoCorp.Views
{
    /// <summary>
    /// Interaction logic for NurseMainWindowView.xaml
    /// </summary>
    public partial class NurseMainView : Window
    {
        public NurseMainView()
        {
            InitializeComponent();
            DataContext = new NurseMainViewModel(this);

        }
    }
}
