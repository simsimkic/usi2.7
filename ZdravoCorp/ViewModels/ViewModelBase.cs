using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.DataAccess;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;

namespace ZdravoCorp.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged, IValueConverter, ICommand
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool CanExecute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }


        public event EventHandler? CanExecuteChanged;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the patient with the given username
            PatientDAO patientDao = new PatientDAO();
            var patient = patientDao.GetPatientByUsername(value.ToString());

            // Return the patient's full name
            return patient.FirstName + " " + patient.LastName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
