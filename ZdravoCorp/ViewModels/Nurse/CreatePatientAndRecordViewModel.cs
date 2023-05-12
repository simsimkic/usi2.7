using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;
using ZdravoCorp.Storage;
using ZdravoCorp.Views;

namespace ZdravoCorp.ViewModels.Nurse
{
    internal class CreatePatientAndRecordViewModel : ViewModelBase
    {
        PatientDAO patientDao;
        public bool IsCreating;
        public CreatePatientAndRecordView CurrentWindow;
        public CreatePatientAndRecordViewModel(CreatePatientAndRecordView window, bool isCreating, PatientDAO patientDAO)
        {
            patientDao = patientDAO;
            CurrentWindow = window;
            IsCreating = isCreating;
            IsUpdatingPatient = !isCreating;

        }

        public CreatePatientAndRecordViewModel(CreatePatientAndRecordView window, bool isCreating, Patient patient, PatientDAO patientDAO)
        {
            patientDao = patientDAO;
            CurrentWindow = window;
            IsCreating = isCreating;
            IsUpdatingPatient = !isCreating;
            Name = patient.FirstName;
            Lastname = patient.LastName;
            Username = patient.Username;
            Gender = patient.Gender;
            BirthDate = patient.DateOfBirth;
            Password = patient.Password;
            Height = patient.PatientRecord.Height;
            Weight = patient.PatientRecord.Weight;
            MedicalHistory = string.Join(",", patient.PatientRecord.MedicalHistory);
            Allergens = string.Join(",", patient.PatientRecord.Allergens);
            //do medical history and send patient when updating and disable username changing
        }

        private bool _isUpdatingPatient;

        public bool IsUpdatingPatient
        {
            get { return _isUpdatingPatient; }
            set
            {
                _isUpdatingPatient = value;
                OnPropertyChanged(nameof(IsUpdatingPatient));
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _lastname;
        public string Lastname
        {
            get
            {
                return _lastname;
            }
            set
            {
                _lastname = value;
                OnPropertyChanged(nameof(Lastname));
            }
        }

        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private DateTime _birthDate = new DateTime(2000, 1, 1);
        public DateTime BirthDate
        {
            get
            {
                return _birthDate;
            }
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }

        private Gender _gender;
        public Gender Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        private double _height;
        public double Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private double _weight;
        public double Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        private string _medicalHistory;
        public string MedicalHistory
        {
            get
            {
                return _medicalHistory;
            }
            set
            {
                _medicalHistory = value;
                OnPropertyChanged(nameof(MedicalHistory));
            }
        }

        private string _allergens;
        public string Allergens
        {
            get
            {
                return _allergens;
            }
            set
            {
                _allergens = value;
                OnPropertyChanged(nameof(Allergens));
            }
        }

        public ICommand Submit => new RelayCommand(CreatePatientAndRecord);
        private void CreatePatientAndRecord(object parameter)
        {
            if (AreFieldsValid(IsCreating))
            {
                List<string> medicalHistory = new List<string>();
                List<string> allergens = new List<string>();
                if (_medicalHistory != null)
                {
                    medicalHistory = patientDao.ParseAllergensAndHistory(_medicalHistory);
                }
                if (_allergens != null)
                {
                    allergens = patientDao.ParseAllergensAndHistory(_allergens);
                }
                patientDao.CreatePatient(new Patient(_username, _password, _name, _lastname, _birthDate, _gender, new PatientRecord(_height, _weight, medicalHistory, allergens)));
                MessageBox.Show("Successfully saved patient", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                CurrentWindow.Close();

            }
        }



        private bool AreFieldsValid(bool isCreating)
        {
            if (_username == null || _lastname == null || _name == null || _password == null || _gender.Equals(null) || _height == 0 || _weight == 0)
            {
                MessageBox.Show("Please fill all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (isCreating)
            {
                if (patientDao.IsUsernameTaken(_username))
                {
                    MessageBox.Show("That username is already taken. Pick a different one.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;


        }
    }
}
