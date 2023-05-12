using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using ZdravoCorp.Models.DAO;
using ZdravoCorp.ViewModels.Manager;
using ZdravoCorp.Views.Manager;
using ZdravoCorp.Views;
using System.Windows.Input;
using ZdravoCorp.Services;

namespace ZdravoCorp.ViewModels
{
    internal class LoginWindowViewModel : ViewModelBase
    {

        private DoctorDAO _doctorDAO = new DoctorDAO();
        private PatientDAO _patientDAO = new PatientDAO();
        private NotificationDAO _notificationDAO = new NotificationDAO();

        private Dictionary<string, string> _patients = new Dictionary<string, string>();
        private Dictionary<string, string> _doctors = new Dictionary<string, string>();
        public LoginWindowViewModel()
        {
            
            foreach (var doctor in _doctorDAO.GetDoctors())
            {
                _doctors.Add(doctor.Username, doctor.Password);
            }
            foreach (var patient in _patientDAO.GetPatients())
            {
                _patients.Add(patient.Username, patient.Password);
            }

            var orderService = new OrderService();
            orderService.CheckOrders();

            var transferService = new TransferService();
            transferService.CheckTransfers();

        }

        private string _username="";
        public string Username
        {
            get => _username;
            set
            {
                if (_username == value) return;
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private bool _errorVisibility = false;
        public bool ErrorVisibility
        {
            get => _errorVisibility;
            set
            {
                if (_errorVisibility == value) return;
                _errorVisibility = value;
                OnPropertyChanged(nameof(ErrorVisibility));
            }
        }

        private string _password="";
        public string Password
        {
            get => _password;
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                }
            }
        }

        public ICommand Login => new RelayCommand(LoginButtonClick);

        private void LoginButtonClick(object parameter)
        {
            if (_username == "nurse" && _password == "password")
            {
                NurseMainView nurseWindow = new NurseMainView();
                CloseWindow();
                nurseWindow.Show();
            }
            else if (_username == "manager" && _password == "password")
            {
                MainManagerWindow managerView = new MainManagerWindow()
                {
                    DataContext = new ManagerNavigationViewModel()
                };
                CloseWindow();
                managerView.Show();
            }
            else if (_doctors.ContainsKey(_username) && _doctors[_username] == _password)
            {
                if (_notificationDAO.HasNotification(_username))
                {
                    var notificationViewModel = new NotificationViewModel(_username);
                    var notificationView = new NotificationView() { DataContext = notificationViewModel };
                    notificationView.ShowDialog();
                }
                DoctorView doctorWindow = new DoctorView(_doctorDAO.GetDoctorByUsername(_username));
                CloseWindow();
                doctorWindow.Show();

            }
            else if (_patients.ContainsKey(_username) && _patients[_username] == _password)
            {
                if (_notificationDAO.HasNotification(_username))
                {
                    var notificationViewModel = new NotificationViewModel(_username);
                    var notificationView = new NotificationView() { DataContext = notificationViewModel };
                    notificationView.ShowDialog();
                }
                PatientView patientWindow = new PatientView(_patientDAO.GetPatientByUsername(_username));
                CloseWindow();
                patientWindow.Show();

            }
            else
            {
                // Show error message in MessageBox
                ErrorVisibility=true;
            }
        }

    }
}
