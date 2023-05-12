using System.Windows;
using ZdravoCorp.ViewModels;
using ZdravoCorp.Models;
using ZdravoCorp.DataAccess;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using ZdravoCorp.Models.DAO;
using System.Numerics;

namespace ZdravoCorp.Views
{
    /// <summary>
    /// Interaction logic for Doctor.xaml
    /// </summary>
    public partial class DoctorView : Window
    {
        public DoctorView(Doctor doctor)
        {
            InitializeComponent();
            DoctorViewModel.SignedDoctor = doctor;           
            DoctorViewModel.PatientDAO = new PatientDAO();
            DoctorViewModel.ExaminationDAO = new ExaminationDAO();
        }

        public DoctorView()
        {
            Doctor doctor = new Doctor("doctor", "password", "Milos", "Milanovic", new DateTime(2002, 12, 9),
                Gender.Male, new ObservableCollection<Examination>(), Specialization.GeneralMedicine);
            InitializeComponent();
            DoctorViewModel.SignedDoctor = doctor;           
            DoctorViewModel.PatientDAO = new PatientDAO();
            DoctorViewModel.ExaminationDAO = new ExaminationDAO();
        }
    }
}