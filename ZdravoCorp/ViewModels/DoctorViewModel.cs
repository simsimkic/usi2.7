using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using ZdravoCorp.DataAccess;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;
using ZdravoCorp.Services;
using ZdravoCorp.Views;

namespace ZdravoCorp.ViewModels
{
    public class DoctorViewModel : ViewModelBase
    {
        //public ICommand LogOut => new RelayCommand(OpenLoginWindow);
        //private void OpenLoginWindow(object parameter)
        //{
        //    LoginWindowView loginWindow = new LoginWindowView();
        //    loginWindow.Show();

        //    foreach (Window window in Application.Current.Windows)
        //    {
        //        if (window.DataContext == this)
        //        {
        //            window.Close();
        //        }
        //    }
        //}
        protected internal static Doctor SignedDoctor { get; set; }
        protected internal static ExaminationDAO ExaminationDAO { get; set; }
        protected internal static PatientDAO PatientDAO { get; set; }

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



