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
using ZdravoCorp.ViewModels;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;

namespace ZdravoCorp.Views
{
    /// <summary>
    /// Interaction logic for Doctor.xaml
    /// </summary>
    public partial class PatientView : Window
    {
        public PatientView(Patient patient)
        {
            PatientViewModel.SignedPatient = patient;
            PatientViewModel.DoctorDAO = new DoctorDAO();
            PatientViewModel.ExaminationDAO = new ExaminationDAO();
            InitializeComponent();
            DataContext = new PatientViewModel();
        }

        public PatientView()
        {
            Patient patient = new Patient("patient", "password", "Nikola", "Mitrovic", new DateTime(2002, 12, 9),
                Gender.Male, new PatientRecord(62, 177, null, null, null));

            PatientViewModel.SignedPatient = patient;
            PatientViewModel.DoctorDAO = new DoctorDAO();
            PatientViewModel.ExaminationDAO = new ExaminationDAO(); ;
            InitializeComponent();
        }
    }
}