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
using ZdravoCorp.Models.DAO;
using ZdravoCorp.ViewModels.Nurse;

namespace ZdravoCorp.Views
{
    /// <summary>
    /// Interaction logic for CreatePatientAndRecordView.xaml
    /// </summary>
    public partial class CreatePatientAndRecordView : Window
    {
        public CreatePatientAndRecordView(bool isCreate, Patient patient, PatientDAO patientDAO)
        {
            InitializeComponent();
            DataContext = new CreatePatientAndRecordViewModel(this, isCreate, patient, patientDAO);
        }
        public CreatePatientAndRecordView(bool isCreate, PatientDAO patientDAO)
        {
            InitializeComponent();
            DataContext = new CreatePatientAndRecordViewModel(this, isCreate, patientDAO);
        }

        private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsLetter(c))
                {
                    e.Handled = true; // set Handled to true to prevent non-letter input
                    break;
                }
            }
        }
        private void PreviewNumberInputHandler(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != "." ||
                ((TextBox)sender).Text.Contains(".") && e.Text == ".")
            {
                e.Handled = true;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }
    }
    
}
