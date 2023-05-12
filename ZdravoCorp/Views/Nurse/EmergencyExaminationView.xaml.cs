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
using ZdravoCorp.Models;
using ZdravoCorp.ViewModels.Nurse;

namespace ZdravoCorp.Views
{
    /// <summary>
    /// Interaction logic for DoctorSpecializationView.xaml
    /// </summary>
    public partial class EmergencyExaminationView : Window
    {
        public EmergencyExaminationView(Patient patient)
        {
            InitializeComponent();
            DataContext = new EmergencyExaminationViewModel(patient);
        }
    }
}
