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
using ZdravoCorp.Models.DAO;
using ZdravoCorp.ViewModels;


namespace ZdravoCorp.ViewModels
{
    public class RecommendedExaminationViewModel : PatientViewModel
    {
        public ObservableCollection<Doctor> Doctors { get; set; } = new ObservableCollection<Doctor>();

        


        private ExaminationDAO _examinationDAO;
        private DoctorDAO _doctorDAO;
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
                ResetFields();
                OnPropertyChanged(nameof(SelectedDoctor));
                ;
                OnPropertyChanged(nameof(IsDoctorSelected));

                DoctorName = SelectedDoctor.FirstName;
                DoctorSurname = SelectedDoctor.LastName;
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



        

        private ObservableCollection<Examination> _closestExaminations = new ObservableCollection<Examination>();

        public ObservableCollection<Examination> ClosestExaminations
        {
            get => _closestExaminations;
            set
            {
                _closestExaminations = value;
                OnPropertyChanged(nameof(ClosestExaminations));
            }
        }


       
        private Examination _selectedExamination;
        public Examination SelectedExamination
        {
            get => _selectedExamination;
            set
            {
                if (_selectedExamination == value) return;
                _selectedExamination = value;
                OnPropertyChanged(nameof(SelectedExamination));

                
                _isExaminationSelected = true;

            }
        }

        private bool _isExaminationSelected = false;
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

        private ObservableCollection<int> _timeRange;
        public ObservableCollection<int> TimeRange
        {
            get => _timeRange;
            set
            {
                if (_timeRange == value) return;
                _timeRange = value;
                OnPropertyChanged(nameof(TimeRange));
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

        private int _selectedEndingHour;

        public int SelectedEndingHour
        {
            get => _selectedEndingHour;
            set
            {
                if (_selectedEndingHour == value) return;
                _selectedEndingHour = value;
                OnPropertyChanged(nameof(SelectedEndingHour));
            }
        }

        private int _selectedStartingHour;

        public int SelectedStartingHour
        {
            get => _selectedStartingHour;
            set
            {
                if (_selectedStartingHour == value) return;
                _selectedStartingHour = value;
                OnPropertyChanged(nameof(SelectedStartingHour));
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
              
            }
        }

       
        private bool _isDoctorChecked;
        public bool IsDoctorChecked
        {
            get => _isDoctorChecked;
            set
            {
                if (_isDoctorChecked == value) return;
                _isDoctorChecked = value;
                OnPropertyChanged(nameof(IsDoctorChecked));
               
            }
        }

        private bool _isTimeRangeChecked;
        public bool IsTimeRangeChecked
        {
            get => _isTimeRangeChecked;
            set
            {
                if (_isTimeRangeChecked == value) return;
                _isTimeRangeChecked = value;
                OnPropertyChanged(nameof(IsTimeRangeChecked));
                
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

        private string? _selectedUsername;

        public string? SelectedUsername
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
            if (SignedPatient == null)
            {
                MessageBox.Show("Required data not available");
                return;
            }

            if (_isExaminationSelected == false)
            {
                MessageBox.Show("Examination not selected!");
                return;
            }

            if (IsPatientBlocked())
            {
                MessageBox.Show("You have been blocked due to scheduling too many examinations or making too many changes");
                return;
            }

            if (!IsPatientBusy())
            {
                MessageBox.Show("Examination created successfully!");
                _examinationDAO.AddExamination(_selectedExamination);

            }
            else
            {
                MessageBox.Show("You already have an examination at that time!");
                return;
            }
        }

        public ICommand Find => new RelayCommand(FindClosestExaminations);
        private void FindClosestExaminations(object parameter)
        {
            if (SignedPatient == null)
            {
                MessageBox.Show("Required data not available");
                return;
            }



            bool isFieldEmpty = _selectedDoctor.Equals(null) || _selectedStartingHour.Equals(null) ||
                _selectedEndingHour.Equals(null) || _selectedDate.Equals(null);
            if (isFieldEmpty)
            {
                MessageBox.Show("Please fill all the fields");
                return;
            }

            FindClosestExaminations();
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
            DateTime startDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1);

            int examinationsScheduled =_examinationDAO.GetUserExaminations(SignedPatient.Username).Count(e => e.TimeSlot.DateTime >= startDate && e.TimeSlot.DateTime <= currentDate);

            return examinationsScheduled >= MaxExaminationsPerMonth || (_examinationsUpdated + _examinationsAdded) >= MaxUpdatesOrDeletesPerMonth;
        }

        public void FindClosestExaminations()
        {
            ClosestExaminations.Clear();

            List<Examination> closestExams = _examinationDAO.GetClosestExaminations(IsDoctorChecked, SelectedDoctor, SelectedStartingHour, SelectedEndingHour, SelectedDate, SignedPatient.Username);

            foreach (Examination exam in closestExams)
            {
                ClosestExaminations.Add(exam);
            }
        }
        private void InitializeTimeRange()
        {
            TimeRange = new ObservableCollection<int>();
            for (int i = 0; i < 24; i++)
            {
                TimeRange.Add(i);
            }
        }

        public static global::System.Int32 Duration => _duration;



        public RecommendedExaminationViewModel()
        {
            _examinationDAO = new ExaminationDAO();
            _doctorDAO = new DoctorDAO();
            DoctorsView = CollectionViewSource.GetDefaultView(_doctorDAO.GetDoctors());
           

            
            InitializeTimeRange();
        }


        
        private void UpdateFields()
        {
            ResetFields();

            if (SelectedDoctor != null)
            {
                SelectedFirstName = SelectedDoctor.FirstName;
                SelectedLastName = SelectedDoctor.LastName;
                SelectedUsername = SelectedDoctor.Username;
            }

            if (SelectedExamination != null)
            {
                var doctor = _doctorDAO.GetDoctorByUsername(SelectedExamination.DoctorUsername);
                SelectedFirstName = doctor.FirstName;
                SelectedLastName = doctor.LastName;
                SelectedDate = SelectedExamination.TimeSlot.DateTime;
                SelectedHour = SelectedExamination.TimeSlot.DateTime.Hour;
                SelectedMinute = SelectedExamination.TimeSlot.DateTime.Minute;
            }
        }

       
        private void ResetFields()
        {
            IsTimeRangeChecked = false; ;
            IsDoctorChecked = false;
            SelectedDate = DateTime.Now;
            SelectedStartingHour = 0;
            SelectedEndingHour = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected new void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

