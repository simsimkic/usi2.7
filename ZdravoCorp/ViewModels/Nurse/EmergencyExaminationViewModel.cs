using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;
using ZdravoCorp.Views;

namespace ZdravoCorp.ViewModels.Nurse
{
    internal class EmergencyExaminationViewModel : ViewModelBase
    {
        private DoctorDAO _doctorDao = new DoctorDAO();
        private ExaminationDAO _examinationDao = new ExaminationDAO();
        private Patient _selectedPatient;
        public List<int> Duration { get; } = Enumerable.Range(1, 60).ToList();


        public EmergencyExaminationViewModel(Patient selectedPatient)
        {
            _selectedPatient = selectedPatient;
        }

        private Specialization _specialization;
        public Specialization Specialization
        {
            get
            {
                return _specialization;
            }
            set
            {
                _specialization = value;
                OnPropertyChanged(nameof(Specialization));
            }
        }
        private int _durationOfExamination = 15;
        public int DurationOfExamination
        {
            get
            {
                return _durationOfExamination;
            }
            set
            {
                _durationOfExamination = value;
                OnPropertyChanged(nameof(DurationOfExamination));
            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));

            }
        }

        public ICommand Submit => new RelayCommand(ScheduleExamination);
        private void ScheduleExamination(object parameter)
        {
            List<Doctor> qualifiedDoctors = _doctorDao.GetQualfiedDoctors(Specialization);
            //finds all upcoming doctors examinations
            List<Examination> allUpcomingDoctorsExaminations = GetUpcomingDoctorsExaminations(qualifiedDoctors);
            //finds available time slots for each doctor
            Dictionary<string, TimeSlot> availableTimeSlots = GetAvailableTimeSlots(allUpcomingDoctorsExaminations, qualifiedDoctors, _durationOfExamination);
            //finds earliest free time slot from all the doctors
            (string chosenDoctor, TimeSlot firstAvailableTimeSlot) = availableTimeSlots.OrderBy(x => x.Value.DateTime).FirstOrDefault();


            if (firstAvailableTimeSlot.DateTime > DateTime.Now.AddHours(2))
            {
                Dictionary<Examination, double> postponableExaminations = GetFirstFivePostponableExaminations(qualifiedDoctors, allUpcomingDoctorsExaminations, _durationOfExamination);

                var examinationsModel = new PostponeExaminationsViewModel(_examinationDao, postponableExaminations, _selectedPatient, _durationOfExamination, _isChecked);
                var examinationWindow = new PostponeExaminationsView { DataContext = examinationsModel };
                examinationWindow.ShowDialog();
            }
            else
            {
                Examination examination = new Examination(firstAvailableTimeSlot, chosenDoctor, _selectedPatient.Username, _isChecked, Examination.Status.Scheduled);
                _examinationDao.AddExamination(examination);
                Doctor doctor = _doctorDao.GetDoctorByUsername(chosenDoctor);
                string message = string.Format("Successfully added reservation\nDoctor: {0} {1}\nPatient: {2} {3}\nDate: {4}\nTime: {5}", doctor.FirstName, doctor.LastName, _selectedPatient.FirstName, _selectedPatient.LastName, firstAvailableTimeSlot.DateTime.ToString("dd-MM-yyyy"), firstAvailableTimeSlot.DateTime.ToString("HH:mm"));
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

        private Dictionary<Examination, double> GetFirstFivePostponableExaminations(List<Doctor> qualifiedDoctors, List<Examination> allUpcomingExaminations, int duration)
        {
            Dictionary<Examination, double> examinationsWithPostponedTimes = new Dictionary<Examination, double>();
            foreach (var doctor in qualifiedDoctors)
            {
                allUpcomingExaminations = allUpcomingExaminations.OrderBy(exam => exam.TimeSlot.DateTime).ToList();
                List<Examination> doctorExaminations = allUpcomingExaminations.Where(e => e.DoctorUsername == doctor.Username).ToList();
                for (int i = 0; i < doctorExaminations.Count; ++i)
                {
                    if (doctorExaminations[i].TimeSlot.DateTime < DateTime.Now.AddHours(2))
                    {
                        if (i != doctorExaminations.Count - 1)
                        {
                            TimeSlot possibleTimeSlot = new TimeSlot(doctorExaminations[i].TimeSlot.DateTime, duration);
                            if (possibleTimeSlot.IsOverlappingWith(doctorExaminations[i + 1].TimeSlot))
                            {
                                continue;
                            }
                        }

                        TimeSlot firstAvailableTimeSlot = _examinationDao.GetFirstAvailableTimeSlot(doctorExaminations, doctorExaminations[i].TimeSlot.Duration);
                        examinationsWithPostponedTimes[doctorExaminations[i]] = firstAvailableTimeSlot.DateTime.Subtract(doctorExaminations[i].TimeSlot.DateTime).TotalMinutes;
                    }
                }
            }
            return examinationsWithPostponedTimes.OrderBy(x => x.Value).Take(5).ToDictionary(x => x.Key, x => x.Value);
        }

        private List<Examination> GetUpcomingDoctorsExaminations(List<Doctor> doctors)
        {
            List<Examination> allDoctorsExaminations = new List<Examination>();

            foreach (var doctor in doctors)
            {
                List<Examination> doctorExaminations = _examinationDao.GetUpcomingUserExaminations(doctor.Username);
                allDoctorsExaminations.AddRange(doctorExaminations);
            }

            return allDoctorsExaminations;
        }

        private Dictionary<string, TimeSlot> GetAvailableTimeSlots(List<Examination> allExaminations, List<Doctor> qualifiedDoctors, int duration)
        {
            Dictionary<string, TimeSlot> availableTimeSlots = new Dictionary<string, TimeSlot>();

            foreach (var doctor in qualifiedDoctors)
            {

                List<Examination> doctorExaminations = allExaminations.Where(e => e.DoctorUsername == doctor.Username).ToList();
                TimeSlot firstAvailableTimeSlot = _examinationDao.GetFirstAvailableTimeSlot(doctorExaminations, duration);
                availableTimeSlots.Add(doctor.Username, firstAvailableTimeSlot);
            }

            return availableTimeSlots;
        }
    }
}
