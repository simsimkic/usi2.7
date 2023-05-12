using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.ViewModels;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Text.Json;
using ZdravoCorp.Helper;
using ZdravoCorp.Views;
using ZdravoCorp.DataAccess;
using System.ComponentModel;
using System.Windows.Markup;
using ZdravoCorp.Models.DAO;
using ZdravoCorp;



namespace ZdravoCorp.ViewModels
{



    public class PatientViewModel : ViewModelBase
    {
        protected internal static Patient? SignedPatient { get; set; }
        protected internal static ExaminationDAO? ExaminationDAO { get; set; }
        protected internal static DoctorDAO? DoctorDAO { get; set; }

        public ICommand LogOut => new RelayCommand(OpenLoginWindow);
        private void OpenLoginWindow(object parameter)
        {
            LoginWindowView loginWindow = new LoginWindowView();
            loginWindow.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                }
            }


        }



    }
}