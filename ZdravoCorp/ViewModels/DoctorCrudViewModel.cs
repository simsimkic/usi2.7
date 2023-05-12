using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using ZdravoCorp.DataAccess;
using ZdravoCorp.Models;
using ZdravoCorp.Views;
using ZdravoCorp.ViewModels.Filters;

namespace ZdravoCorp.ViewModels
{
    public class DoctorCrudViewModel : DoctorViewModel 
    {
        private readonly Doctor _doctor;

        private enum Selected { Patient, Examination, Nothing }
        private Selected _selected = Selected.Nothing;

        // combobox values
        public List<int> Hours { get; } = Enumerable.Range(0, 24).ToList();
        public List<int> Minutes { get; } = Enumerable.Range(0, 60).ToList();
        public List<int> Duration { get; } = Enumerable.Range(1, 241).ToList();

        // examinations in table
        private ICollectionView _examinationsView;
        public ICollectionView ExaminationsView
        {
            get => _examinationsView;
            set
            {
                if (_examinationsView == value) return;
                _examinationsView = value;
                OnPropertyChanged(nameof(ExaminationsView));
            }
        }
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

        private Examination _selectedExamination;
        public Examination SelectedExamination
        {
            get => _selectedExamination;
            set
            {
                if (value == _selectedExamination) return;
                _selectedExamination = value;
                OnPropertyChanged(nameof(SelectedExamination));
                _selected = Selected.Examination;
                UpdateFields();
            }
        }

        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                if (value == _selectedPatient) return;
                _selectedPatient = value;
                OnPropertyChanged(nameof(SelectedPatient));
                _selected = Selected.Patient;
                UpdateFields();
            }
        }

        private string _selectedFirstName;
        public string SelectedFirstName
        {
            get => _selectedFirstName;
            set
            {
                if (value == _selectedFirstName) return;
                _selectedFirstName = value;
                OnPropertyChanged(nameof(SelectedFirstName));
            }
        }

        private string _selectedLastName;
        public string SelectedLastName
        {
            get => _selectedLastName;
            set
            {
                if (value == _selectedLastName) return;
                _selectedLastName = value;
                OnPropertyChanged(nameof(SelectedLastName));
            }
        }

        // add new examination to all examinations
        public ICommand Create => new RelayCommand(CreateExamination);
        private void CreateExamination(object parameter)
        {
            bool isFieldEmpty = _selectedHour.Equals(null) || _selectedMinute.Equals(null) || _selectedDuration.Equals(null) || _examinationDate.Equals(null);
            if (isFieldEmpty)
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            foreach (var exam in ExaminationDAO.GetUserExaminations(SignedDoctor.Username))
            {
                var startTime = exam.TimeSlot.DateTime;
                var endTime = startTime.AddMinutes(exam.TimeSlot.Duration);
                var selectedStartTime = new DateTime(_examinationDate.Year, _examinationDate.Month, _examinationDate.Day, _selectedHour, _selectedMinute, 0);
                var selectedEndTime = selectedStartTime.AddMinutes(_selectedDuration);
                if ((selectedStartTime < startTime || selectedStartTime > endTime) &&
                    (selectedEndTime < startTime || selectedEndTime > endTime)) continue;
                MessageBox.Show("Time slot is not available");
                return;
            }

            MessageBox.Show("Successful created");

            var examination = new Examination(new TimeSlot(_examinationDate.Date.AddHours(_selectedHour).AddMinutes(_selectedMinute), _selectedDuration), _doctor.Username, _selectedUsername, _isOperationChecked, Examination.Status.Scheduled); ;
            ExaminationDAO.AddExamination(examination);
            _selected = Selected.Nothing;
            ResetFields();
        }

        public ICommand Open => new RelayCommand(OpenMedicalRecord);
        private void OpenMedicalRecord(object parameter)
        {
            var patientUsername = SelectedExamination.PatientUsername;
            var medicalRecord = new MedicalRecordViewModel(PatientDAO.GetPatientByUsername(patientUsername), MedicalRecordViewModel.MedicalRecordPermission.DoctorView);
            var medicalRecordWindow = new MedicalRecordWindow { DataContext = medicalRecord };
            medicalRecordWindow.ShowDialog();
        }   

        // get changed data from fields and update selected examination
        public ICommand Update => new RelayCommand(UpdateExamination);
        private void UpdateExamination(object parameter)
        {
            bool isFiedlEmpty = _selectedHour.Equals(null) || _selectedMinute.Equals(null) || _selectedDuration.Equals(null) || _examinationDate.Equals(null);
            if (isFiedlEmpty)
            {
                MessageBox.Show("Please fill all fields");
                return;
            }
            MessageBox.Show("Successful updated");
            SelectedExamination.TimeSlot =
                new TimeSlot(_examinationDate.Date.AddHours(_selectedHour).AddMinutes(_selectedMinute),
                    _selectedDuration);
            SelectedExamination.IsOperation = _isOperationChecked;
            ExaminationDAO.UpdateExamination(_selectedExamination);
            _selected = Selected.Nothing;
            _examinationsView.Refresh();
            ResetFields();
        }

        public ICommand Delete => new RelayCommand(DeleteExamination);
        private void DeleteExamination(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            MessageBox.Show("Successful deleted");
            _selected = Selected.Nothing;
            ExaminationDAO.DeleteExamination(_selectedExamination);
        }

        public ICommand Search => new RelayCommand(SearchExactDate);
        private void SearchExactDate(object parameter)
        {
            if (_examinationDate.Equals(null))
            {
                MessageBox.Show("Please select date");
                return;
            }
            _examinationsView.Refresh();
        }

        private bool _isExaminationChecked = true;
        public bool IsExaminationChecked
        {
            get => _isExaminationChecked;
            set
            {
                if (_isExaminationChecked == value) return;
                _isExaminationChecked = value;
                OnPropertyChanged(nameof(IsExaminationChecked));
                if (!value) return;
                SelectedDuration = 15;
            }
        }

        private bool _isOperationChecked;
        public bool IsOperationChecked
        {
            get => _isOperationChecked;
            set
            {
                if (_isOperationChecked == value) return;
                _isOperationChecked = value;
                OnPropertyChanged(nameof(IsOperationChecked));
                if (!value) return;
                SelectedDuration = 60;
            }
        }

        // filter examinations by next three days
        private bool _isNextThreeDaysChecked;
        public bool IsNextThreeDaysChecked
        {
            get => _isNextThreeDaysChecked;
            set
            {
                if (_isNextThreeDaysChecked == value) return;
                _isNextThreeDaysChecked = value;
                OnPropertyChanged(nameof(IsNextThreeDaysChecked));
                _examinationsView.Refresh();
            }
        }

        // filter examinations by exact date
        private bool _isExactDateChecked;
        public bool IsExactDateChecked
        {
            get => _isExactDateChecked;
            set
            {
                if (_isExactDateChecked == value) return;
                _isExactDateChecked = value;
                OnPropertyChanged(nameof(IsExactDateChecked));
                _examinationsView.Refresh();
            }
        }

        // default duration for examination is 15 minutes
        private int _selectedDuration = 15;
        public int SelectedDuration
        {
            get => _selectedDuration;
            set
            {
                if (_selectedDuration == value) return;
                _selectedDuration = value;
                OnPropertyChanged(nameof(SelectedDuration));
            }
        }

        private DateTime _examinationDate = DateTime.Now;
        public DateTime ExaminationDate
        {
            get => _examinationDate;
            set
            {
                if (_examinationDate == value.Date) return;
                _examinationDate = value.Date;
                OnPropertyChanged(nameof(ExaminationDate));
            }   
        }

        private int _selectedHour;
        public int SelectedHour
        {
            get => _selectedHour;
            set
            {
                if (_selectedHour == value) return;
                _selectedHour = value;
                OnPropertyChanged(nameof(SelectedHour));
            }
        }

        private int _selectedMinute;
        public int SelectedMinute
        {
            get => _selectedMinute;
            set
            {
                if (_selectedMinute == value) return;
                _selectedMinute = value;
                OnPropertyChanged(nameof(SelectedMinute));
            }
        }

        private DateTime _exactDate = DateTime.Now;
        public DateTime ExactDate
        {
            get => _exactDate;
            set
            {
                if (_exactDate == value) return;
                _exactDate = value;
                OnPropertyChanged(nameof(ExactDate));
            }
        }

        private string _selectedUsername;
        public string SelectedUsername
        {
            get => _selectedUsername;
            set
            {
                if (_selectedUsername == value) return;
                _selectedUsername = value;
                OnPropertyChanged(nameof(SelectedUsername));
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

        // if there is selected examination, enable button for updating and deleting examination
        private bool _isExaminationSelected;
        public bool IsExaminationSelected
        {
            get => _isExaminationSelected;
            set
            {
                if (_isExaminationSelected == value) return;
                _isExaminationSelected = value;
                OnPropertyChanged(nameof(IsExaminationSelected));
            }
        }

        public DoctorCrudViewModel()
        {
            _examinationsView = new ListCollectionView(ExaminationDAO.GetUserExaminations(SignedDoctor.Username));
            _patientsView = CollectionViewSource.GetDefaultView(PatientDAO.GetPatients());
            var examinationFilter = new ExaminationFilter(this);
            _examinationsView.Filter = obj =>
            {
                if (obj is not Examination examination) return false;
                return examinationFilter.MatchesDate(examination.TimeSlot.DateTime);
            };
        }

        // when select patient or examination, update fields to values of selected patient or examination
        private void UpdateFields()
        {
            if (_selectedExamination == null || _selectedPatient == null)
            {
                _selectedExamination ??= new Examination();
                _selectedPatient ??= new Patient();
                return;
            }
            ResetFields();
            switch (_selected)
            {
                case Selected.Patient:
                    SelectedFirstName = _selectedPatient.FirstName;
                    SelectedLastName = _selectedPatient.LastName;
                    SelectedUsername = _selectedPatient.Username;
                    IsPatientSelected = true;
                    IsExaminationSelected = false;
                    SelectedExamination = null;
                    _selectedExamination = new Examination();
                    break;
                case Selected.Examination:
                    var patient = PatientDAO.GetPatientByUsername(_selectedExamination.PatientUsername);
                    SelectedFirstName = patient.FirstName;
                    SelectedLastName = patient.LastName;
                    ExaminationDate = _selectedExamination.TimeSlot.DateTime;
                    SelectedHour = _selectedExamination.TimeSlot.DateTime.Hour;
                    SelectedMinute = _selectedExamination.TimeSlot.DateTime.Minute;
                    SelectedDuration = _selectedExamination.TimeSlot.Duration;
                    IsExaminationChecked = !_selectedExamination.IsOperation;
                    IsOperationChecked = _selectedExamination.IsOperation;
                    IsPatientSelected = false;
                    IsExaminationSelected = true;
                    SelectedPatient = null;
                    _selectedPatient = new Patient();
                    break;
                case Selected.Nothing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        // reset fields to default values
        private void ResetFields()
        {
            SelectedFirstName = string.Empty;
            SelectedLastName = string.Empty;
            SelectedUsername = string.Empty;
            ExaminationDate = DateTime.Now;
            SelectedHour = 0;
            SelectedMinute = 0;
            SelectedDuration = 15;
            IsExaminationChecked = true;
            IsOperationChecked = false;
        }
    }
}
