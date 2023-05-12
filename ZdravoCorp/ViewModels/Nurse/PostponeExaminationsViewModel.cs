using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ZdravoCorp.Models.DAO;
using ZdravoCorp.Models;
using System.Windows.Input;
using System.Windows;
using ZdravoCorp.Views;
using System.Numerics;

namespace ZdravoCorp.ViewModels.Nurse
{
    internal class PostponeExaminationsViewModel : ViewModelBase
    {
        private DoctorDAO _doctorDao = new DoctorDAO();
        private PatientDAO _patientDao = new PatientDAO();
        private Dictionary<Examination, double> _postponableExaminations;
        private Patient _patient;
        private int _duration;
        private bool _isChecked;
        private ExaminationDAO _examinationDao;
        private NotificationDAO _notificationDAO = new NotificationDAO();

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
        private ObservableCollection<Examination> _examinations;
        public ObservableCollection<Examination> Examinations
        {
            get { return _examinations; }
            set
            {
                _examinations = value;
                OnPropertyChanged(nameof(Examinations));

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
                IsExaminationSelected = true;
            }
        }

        // if there is selected patient, enable button for opening medical record and enable button for adding examination
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

        public PostponeExaminationsViewModel(ExaminationDAO examinationDao, Dictionary<Examination, double> postponableExaminations, Patient patient, int duration, bool isChecked)
        {
            _postponableExaminations = postponableExaminations;
            var examinations = postponableExaminations.Keys.ToList();
            _patient = patient;
            _examinationDao = examinationDao;
            Examinations = new ObservableCollection<Examination>(examinations);
            _examinationsView = CollectionViewSource.GetDefaultView(Examinations);
            _duration = duration;
            _isChecked = isChecked;
        }

        public ICommand Postpone => new RelayCommand(PostponeAndScheduleEmergency);
        private void PostponeAndScheduleEmergency(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to postpone this examination?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _examinationDao.DeleteExamination(_selectedExamination);
                //dodaj duration kao parametar konstruktora
                //proveri za timeslotove
                Examination examination = new Examination(new TimeSlot(_selectedExamination.TimeSlot.DateTime, _duration), _selectedExamination.DoctorUsername, _patient.Username, _isChecked, Examination.Status.Scheduled);
                _examinationDao.AddExamination(examination);
                _selectedExamination.TimeSlot.DateTime = _selectedExamination.TimeSlot.DateTime.AddMinutes(_postponableExaminations[_selectedExamination]);
                _examinationDao.AddExamination(_selectedExamination);
                Doctor doctor = _doctorDao.GetDoctorByUsername(_selectedExamination.DoctorUsername);
                Patient patient = _patientDao.GetPatientByUsername(_selectedExamination.PatientUsername);
                string message = string.Format("Postponed examination:\n\tDoctor: {0} {1}\n\tPatient: {2} {3}\n\tDate: {4}\n\tDuration: {5}\n\n" +
                                                "Emergency examination:\n\tDoctor: {0} {1}\n\tPatient: {6} {7}\n\tDate: {8}\n\tDuration: {9}",
                    doctor.FirstName, doctor.LastName, patient.FirstName, patient.LastName, _selectedExamination.TimeSlot.DateTime.ToString("dd-MM-yyyy HH:mm"),
                    _selectedExamination.TimeSlot.Duration, _patient.FirstName, _patient.LastName,
                    examination.TimeSlot.DateTime.ToString("dd-MM-yyyy HH:mm"), examination.TimeSlot.Duration);

                Notification doctorNotification = new Notification(message, doctor.Username);
                Notification patientNotification = new Notification(string.Format("Postponed examination:\nDoctor: {0} {1}\nPatient: {2} {3}\nFrom: {4}\nTo: {5}",
                    doctor.FirstName, doctor.LastName, patient.FirstName, patient.LastName, examination.TimeSlot.DateTime.ToString("dd-MM-yyyy HH:mm"),
                    _selectedExamination.TimeSlot.DateTime.ToString("dd-MM-yyyy HH:mm")), patient.Username);

                _notificationDAO.AddNotification(doctorNotification);
                _notificationDAO.AddNotification(patientNotification);

                MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

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
