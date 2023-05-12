using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.DataAccess;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;
using ZdravoCorp.ViewModels.Filters;
using ZdravoCorp.Views;
using static ZdravoCorp.ViewModels.MedicalRecordViewModel;

namespace ZdravoCorp.ViewModels.Nurse
{
    internal class NurseMainViewModel : ViewModelBase
    {
        public PatientDAO patientDao = new PatientDAO();
        public NurseMainView CurrentWindow;
        private PatientFilter _patientFilter;

        private readonly List<string> _gender = new() { "All", "Male", "Female", "Other" };
        public List<string> Gender => _gender;


        private ICollectionView _patientsView;
        public ICollectionView PatientsView
        {
            get => _patientsView;
            set
            {
                _patientsView = value;
                OnPropertyChanged(nameof(PatientsView));
            }
        }

        private ObservableCollection<Patient> _patients;
        public ObservableCollection<Patient> Patients
        {
            get { return _patients; }
            set
            {
                _patients = value;
                OnPropertyChanged(nameof(Patients));
                PatientsView = CollectionViewSource.GetDefaultView(_patients);
                CreateFilter();
                PatientsView.Refresh();
            }
        }

        private void CreateFilter()
        {
            _patientFilter = new PatientFilter(SearchText, SelectedGender);
            _patientsView.Filter = obj =>
            {
                if (obj is not Patient patient) return false;
                return _patientFilter.MatchesSearchText(patient.ToString()) &&
                _patientFilter.MatchesSelectedGender(patient.Gender);
            };
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                _patientFilter.SearchText = SearchText;
                PatientsView.Refresh();
            }
        }

        private string _selectedGender;
        public string SelectedGender
        {
            get => _selectedGender;
            set
            {
                if (_selectedGender == value) return;
                _selectedGender = value;
                OnPropertyChanged(nameof(SelectedGender));
                _patientFilter.Gender = SelectedGender;
                PatientsView.Refresh();
            }
        }

        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                if (_selectedPatient == value) return;
                _selectedPatient = value;
                OnPropertyChanged(nameof(SelectedPatient));
                IsPatientSelected = true;
            }
        }

        // if there is selected patient, enable button for opening medical record and enable button for adding examination
        private bool _isPatientSelected;
        public bool IsPatientSelected
        {
            get => _isPatientSelected;
            set
            {
                if (_isPatientSelected == value) return;
                _isPatientSelected = value;
                OnPropertyChanged(nameof(IsPatientSelected));
            }
        }



        public NurseMainViewModel(NurseMainView currentWindow)
        {
            Patients = new ObservableCollection<Patient>(patientDao.GetPatients() ?? new List<Patient>());
            _selectedGender = "All";
            _patientsView = CollectionViewSource.GetDefaultView(_patients);
            CreateFilter();
            CurrentWindow = currentWindow;
        }



        public ICommand AddPatient => new RelayCommand(OpenCreatePatientWindow);

        private void OpenCreatePatientWindow(object parameter)
        {

            CreatePatientAndRecordView createPatientAndRecordView = new CreatePatientAndRecordView(true, patientDao);
            createPatientAndRecordView.ShowDialog();
            Patients = new ObservableCollection<Patient>(patientDao.GetPatients() ?? new List<Patient>());
            PatientsView.Refresh();
            _selectedPatient = null;
        }

        public ICommand UpdatePatient => new RelayCommand(OpenUpdatePatientWindow);
        private void OpenUpdatePatientWindow(object parameter)
        {
            if (_selectedPatient == null)
            {
                MessageBox.Show("Please select a patient first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string username = _selectedPatient.Username;
            Patient patient = patientDao.GetPatientByUsername(username);
            CreatePatientAndRecordView createPatientAndRecordView = new CreatePatientAndRecordView(false, patient, patientDao);
            createPatientAndRecordView.ShowDialog();
            Patients = new ObservableCollection<Patient>(patientDao.GetPatients() ?? new List<Patient>());
            PatientsView.Refresh();
            _selectedPatient = null;
        }

        public ICommand Delete => new RelayCommand(DeletePatient);
        private void DeletePatient(object parameter)
        {
            if (_selectedPatient == null)
            {
                MessageBox.Show("Please select a patient first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string username = _selectedPatient.Username;
            Patient patient = patientDao.GetPatientByUsername(username);
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this patient?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                patientDao.DeletePatient(patient);
                Patients = new ObservableCollection<Patient>(patientDao.GetPatients() ?? new List<Patient>());
                PatientsView.Refresh();
                _selectedPatient = null;
            }
            return;

        }


        public ICommand Open => new RelayCommand(OpenMedicalRecord);
        private void OpenMedicalRecord(object parameter)
        {
            if (_selectedPatient == null)
            {
                MessageBox.Show("Select a patient first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var patientUsername = SelectedPatient.Username;
            var medicalRecord = new MedicalRecordViewModel(patientDao.GetPatientByUsername(patientUsername), MedicalRecordPermission.Nurse);
            var medicalRecordWindow = new MedicalRecordWindow { DataContext = medicalRecord };
            medicalRecordWindow.ShowDialog();
        }

        public ICommand OpenSpecialization => new RelayCommand(OpenDoctorSpecialization);
        private void OpenDoctorSpecialization(object parameter)
        {
            if (_selectedPatient == null)
            {
                MessageBox.Show("Select a patient first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ExaminationDAO examinationDao = new ExaminationDAO();
            List<Examination> patientExaminations = examinationDao.GetUpcomingUserExaminations(SelectedPatient.Username);
            foreach (var examination in patientExaminations)
            {
                if (examination.TimeSlot.DateTime < DateTime.Now.AddHours(2))
                {
                    string message = string.Format("{0} {1} already has examination in {2}", SelectedPatient.FirstName, SelectedPatient.LastName, examination.TimeSlot.DateTime.ToString("HH:mm"));
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            EmergencyExaminationView emergencyExaminationView = new EmergencyExaminationView(SelectedPatient);
            emergencyExaminationView.ShowDialog();
        }

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
