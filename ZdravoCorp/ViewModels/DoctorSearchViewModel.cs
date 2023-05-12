using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Models;
using ZdravoCorp.ViewModels.Filters;
using ZdravoCorp.Views;

namespace ZdravoCorp.ViewModels
{
    internal class DoctorSearchViewModel : DoctorViewModel
    {
        private readonly List<string> _gender = new() { "All", "Male", "Female", "Other" };
        public List<string> Gender => _gender;

        private PatientFilter _patientFilter;

        // examinations in table
        private ICollectionView _patientsView;
        public ICollectionView PatientsView
        {
            get => _patientsView;
            set
            {
                if (_patientsView == value) return;
                _patientsView = value;
                OnPropertyChanged(nameof(PatientsView));
            }
        }

        // field for searching patients by name and last name
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
                _patientsView.Refresh();;
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
                _patientsView.Refresh();;
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
                IsPatientSelected = HasSelectedPatientExaminationByDoctor();
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

        public ICommand Open => new RelayCommand(OpenMedicalRecord);
        private void OpenMedicalRecord(object parameter)
        {
            var patientUsername = SelectedPatient.Username;
            var medicalRecord = new MedicalRecordViewModel(PatientDAO.GetPatientByUsername(patientUsername), MedicalRecordViewModel.MedicalRecordPermission.DoctorViewUpdate);
            var medicalRecordWindow = new MedicalRecordWindow { DataContext = medicalRecord };
            medicalRecordWindow.ShowDialog();
        }

        public DoctorSearchViewModel()
        {
            _patientsView = CollectionViewSource.GetDefaultView(PatientDAO.GetPatients());
            CreateFilter();
        }
        private void CreateFilter()
        {
            _patientFilter = new PatientFilter(SearchText,SelectedGender);
            _patientsView.Filter = obj =>
            {
                if (obj is not Patient patient) return false;
                return _patientFilter.MatchesSearchText(patient.ToString()) &&
                       _patientFilter.MatchesSelectedGender(patient.Gender);
            };
        }

        private bool HasSelectedPatientExaminationByDoctor()
        {
            return SelectedPatient != null && ExaminationDAO.GetUserExaminations(SignedDoctor.Username).Any(e => e.PatientUsername == SelectedPatient.Username && e.DoctorUsername == SignedDoctor.Username);
        }
    }
}
