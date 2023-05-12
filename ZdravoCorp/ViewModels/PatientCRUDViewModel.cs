using System;
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
using ZdravoCorp.Models.DAO;
using ZdravoCorp.ViewModels;
using System.Collections.Generic;


namespace ZdravoCorp.ViewModels
{
    public class PatientCRUDViewModel : PatientViewModel
    {
        public ObservableCollection<Doctor> Doctors { get; set; }


        private ExaminationDAO _examinationDAO;
        private DoctorDAO _doctorDAO;

        private enum Selected { Doctor, Examination, Nothing }
        private Selected _selected = Selected.Nothing;

        public ObservableCollection<Examination> Examinations { get; set; }
        public ObservableCollection<TimeSlot> TimeSlots { get; set; }

        public List<int> Hours { get; } = Enumerable.Range(0, 24).ToList();
        public List<int> Minutes { get; } = Enumerable.Range(0, 60).ToList();


        private const int MaxExaminationsPerMonth = 8;
        private const int MaxUpdatesOrDeletesPerMonth = 5;
        private int _examinationsUpdated;
        private int _examinationsAdded;
        private const int _duration = 15;

        private Doctor _selectedDoctor;
        public Doctor SelectedDoctor
        {
            get => _selectedDoctor;
            set
            {
                if (_selectedDoctor == value) return;
                _selectedDoctor = value;
                OnPropertyChanged(nameof(SelectedDoctor));
                IsExaminationSelected = false;
                OnPropertyChanged(nameof(IsExaminationSelected));
                IsDoctorSelected = true;
                OnPropertyChanged(nameof(IsDoctorSelected));
                UpdateFields();
            }
        }

        private ICollectionView _doctorsView;
        public ICollectionView DoctorsView
        {
            get => _doctorsView;
            set
            {
                if (_doctorsView == value) return;
                _doctorsView = value;
                OnPropertyChanged(nameof(DoctorsView));
            }
        }

        //public bool IsDoctorSelected => SelectedDoctor != null;

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

        //public bool IsExaminationSelected => SelectedExamination != null;
        private Examination _selectedExamination;
        public Examination SelectedExamination
        {
            get => _selectedExamination;
            set
            {
                if (_selectedExamination == value) return;
                _selectedExamination = value;
                OnPropertyChanged(nameof(SelectedExamination));
                IsDoctorSelected = false;
                OnPropertyChanged(nameof(IsDoctorSelected));
                IsExaminationSelected = true;
                OnPropertyChanged(nameof(IsExaminationSelected));
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

        public bool IsCreateVisible
        {
            get => IsDoctorSelected && !IsExaminationSelected;
        }

        public bool IsUpdateVisible
        {
            get => !IsDoctorSelected && IsExaminationSelected;
        }


        private bool _isUsernameSelected;

        public bool IsUsernameSelected
        {
            get => _isUsernameSelected;
            set
            {
                if (_isUsernameSelected == value) return;
                _isUsernameSelected = value;
                OnPropertyChanged(nameof(IsUsernameSelected));

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
        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate == value) return;
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
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


        private string? _doctorUsername;

        public string? DoctorUsername
        {
            get => _doctorUsername;
            set
            {
                if (_doctorUsername == value) return;
                _doctorUsername = value;
                OnPropertyChanged(nameof(DoctorUsername));
            }
        }

        private string? _doctorName;

        public string? DoctorName
        {
            get => _doctorName;
            set
            {
                if (_doctorName == value) return;
                _doctorName = value;
                OnPropertyChanged(nameof(DoctorName));
            }
        }

        private string? _doctorSurname;

        public string? DoctorSurname
        {
            get => _doctorSurname;
            set
            {
                if (_doctorSurname == value) return;
                _doctorSurname = value;
                OnPropertyChanged(nameof(DoctorSurname));
            }
        }

        private bool _isDoctorSelected;
        public bool IsDoctorSelected
        {
            get => _isDoctorSelected;
            set
            {
                if (_isDoctorSelected == value) return;
                _isDoctorSelected = value;
                OnPropertyChanged(nameof(IsDoctorSelected));
                OnPropertyChanged(nameof(IsCreateVisible));
                OnPropertyChanged(nameof(IsUpdateVisible));
            }
        }

        private bool _isExaminationSelected;
        public bool IsExaminationSelected
        {
            get => _isExaminationSelected;
            set
            {
                if (_isExaminationSelected == value) return;
                _isExaminationSelected = value;
                OnPropertyChanged(nameof(IsExaminationSelected));
                OnPropertyChanged(nameof(IsCreateVisible));
                OnPropertyChanged(nameof(IsUpdateVisible));
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


        public ICommand Create => new RelayCommand(CreateExamination);
        private void CreateExamination(object parameter)
        {
            if (SignedPatient == null || _selectedDoctor == null)
            {
                MessageBox.Show("Required data not available");
                return;
            }


            if (IsPatientBusy())
            {
                return;
            }


            bool isFieldEmpty = _selectedHour.Equals(null) || _selectedDate.Equals(null) || _selectedMinute.Equals(null);
            if (isFieldEmpty)
            {
                MessageBox.Show("Please fill all fields");
                return;
            }
           

            if (IsDoctorBusy())
            {
                return;
            }

            if (IsPatientBlocked())
            {
                MessageBox.Show("You have been blocked due to scheduling too many examinations or making too many changes");
                return;
            }

            MessageBox.Show("Successful created");
            var examination = new Examination(new TimeSlot(_selectedDate.Date.AddHours(_selectedHour).AddMinutes(_selectedMinute), _duration), _selectedDoctor.Username, SignedPatient.Username);
            Examinations.Add(examination);
            _examinationDAO.AddExamination(examination);
            _selected = Selected.Nothing;
            ResetFields();
            _examinationsAdded++;
            _examinationsView.Refresh();
        }

        public ICommand Update => new RelayCommand(UpdateExamination);

        private void UpdateExamination(object parameter)
        {
            bool isFieldEmpty = _selectedHour.Equals(null) || _selectedMinute.Equals(null) || _selectedDate.Equals(null) || _selectedDoctor == null;
            if (isFieldEmpty)
            {
                MessageBox.Show("Please fill all fields");
                return;
            }
            if (IsDoctorBusy())
            {
              
                return;
            }

            if (IsPatientBusy())
            {
               
                return;
            }

            if (IsPatientBlocked())
            {
                MessageBox.Show("You have been blocked due to scheduling too many examinations or making too many changes");
                return;
            }

            // Check if the change is being made at least one day before the examination
            if (DateTime.Now.AddDays(1) >= SelectedExamination.TimeSlot.DateTime)
            {
                MessageBox.Show("Examination can only be changed at least one day before the start");
                return;
            }

            MessageBox.Show("Successful updated");
            SelectedExamination.TimeSlot =
                 new TimeSlot(_selectedDate.Date.AddHours(_selectedHour).AddMinutes(_selectedMinute),
                     Duration);
            _examinationDAO.UpdateExamination(_selectedExamination);
            _selected = Selected.Nothing;
            _examinationsView.Refresh();
            ResetFields();
            _examinationsUpdated++;
            _examinationsView.Refresh();
        }

        public ICommand Delete => new RelayCommand(DeleteExamination);

        private void DeleteExamination(object parameter)
        {
            if (SelectedExamination == null)
            {
                MessageBox.Show("Please select examination");
                return;
            }

            // Check if the deletion is being made at least one day before the examination
            if (DateTime.Now.AddDays(1) >= SelectedExamination.TimeSlot.DateTime)
            {
                MessageBox.Show("Examination can only be deleted at least one day before the start");
                return;
            }

            if (IsPatientBlocked())
            {
                MessageBox.Show("You have been blocked due to scheduling too many examinations or making too many changes");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            if (SelectedExamination == null)
            {
                MessageBox.Show("Please select examination");
                return;
            }
            MessageBox.Show("Successful deleted");
            _selected = Selected.Nothing;
            _examinationDAO.DeleteExamination(_selectedExamination);
            Examinations.Remove(_selectedExamination); // Add this line to remove the examination from the Examinations collection
            _examinationsView.Refresh();
        }

        private bool IsDoctorBusy()
        {
            foreach (var exam in _examinationDAO.GetUserExaminations(_selectedDoctor.Username))
            {
                var startTime = exam.TimeSlot.DateTime;
                var endTime = startTime.AddMinutes(exam.TimeSlot.Duration);
                var selectedStartTime = new DateTime(_selectedDate.Year, _selectedDate.Month, _selectedDate.Day, _selectedHour, _selectedMinute, 0);
                var selectedEndTime = selectedStartTime.AddMinutes(Duration);
                if ((selectedStartTime < startTime || selectedStartTime > endTime) &&
                    (selectedEndTime < startTime || selectedEndTime > endTime)) continue;
                MessageBox.Show("Time slot is not available");
                return true;
            };
            return false;
        }

        private bool IsPatientBusy()
        {
            foreach (var exam in _examinationDAO.GetUserExaminations(SignedPatient.Username))
            {
                var startTime = exam.TimeSlot.DateTime;
                var endTime = startTime.AddMinutes(exam.TimeSlot.Duration);
                var selectedStartTime = new DateTime(_selectedDate.Year, _selectedDate.Month, _selectedDate.Day, _selectedHour, _selectedMinute, 0);
                var selectedEndTime = selectedStartTime.AddMinutes(Duration);
                if ((selectedStartTime < startTime || selectedStartTime > endTime) &&
                    (selectedEndTime < startTime || selectedEndTime > endTime)) continue;
                MessageBox.Show("Time slot is not available");
                return true;
            }
            return false;
        }

        private bool IsPatientBlocked()
        {
            DateTime currentDate = DateTime.Now;
            DateTime startDate = currentDate.AddMonths(-1);

            int examinationsScheduled =_examinationDAO.GetUserExaminations(SignedPatient.Username).Count(e => e.TimeSlot.DateTime >= startDate && e.TimeSlot.DateTime <= currentDate);

            return examinationsScheduled >= MaxExaminationsPerMonth || (_examinationsUpdated + _examinationsAdded) >= MaxUpdatesOrDeletesPerMonth;
        }


        public static global::System.Int32 Duration => _duration;

        public PatientCRUDViewModel()
        {
            _examinationDAO=new ExaminationDAO();
            _doctorDAO=new DoctorDAO();

            Doctors = new ObservableCollection<Doctor>();
            TimeSlots = new ObservableCollection<TimeSlot>();
            Examinations = new ObservableCollection<Examination>(_examinationDAO.GetUserExaminations(SignedPatient.Username));

            ExaminationsView = CollectionViewSource.GetDefaultView(Examinations);
            _doctorsView = CollectionViewSource.GetDefaultView(_doctorDAO.GetDoctors());
            _examinationsUpdated = 0;
            _examinationsAdded = 0;


        }
        // load all examinations from examinations.json file and add to Examinations 
        private void UpdateFields()
        {
            if (_selectedExamination == null || _selectedDoctor == null)
            {
                _selectedExamination ??= new Examination();
                _selectedDoctor ??= new Doctor();
                return;
            }
            ResetFields();
            switch (_selected)
            {
                case Selected.Doctor:
                    SelectedFirstName = _selectedDoctor.FirstName;
                    SelectedLastName = _selectedDoctor.LastName;
                    SelectedUsername = _selectedDoctor.Username;
                    SelectedExamination = null;
                    _selectedExamination = new Examination();
                    break;
                case Selected.Examination:
                    var doctor = DoctorDAO.GetDoctorByUsername(_selectedExamination.DoctorUsername);
                    SelectedFirstName = doctor.FirstName;
                    SelectedLastName = doctor.LastName;
                    SelectedDate = _selectedExamination.TimeSlot.DateTime;
                    SelectedHour = _selectedExamination.TimeSlot.DateTime.Hour;
                    SelectedMinute = _selectedExamination.TimeSlot.DateTime.Minute;
                    SelectedDoctor = null;
                    _selectedDoctor = new Doctor();
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
            SelectedDate = DateTime.Now;
            SelectedHour = 0;
            SelectedMinute = 0;
        }
    }
}