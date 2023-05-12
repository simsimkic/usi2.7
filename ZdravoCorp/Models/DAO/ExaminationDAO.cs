using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ZdravoCorp.Storage;


namespace ZdravoCorp.Models.DAO
{
    public class ExaminationDAO
    {
        private ExaminationStorage _examinationStorage;
        private List<Examination> _examinations;

        public ExaminationDAO()
        {
            _examinationStorage = new ExaminationStorage();
            _examinations = _examinationStorage.LoadExaminations();
        }

        public void ChangeExaminationStatusToReady(string username)
        {
            List<Examination> userExaminations = GetUpcomingUserExaminations(username).OrderBy(exam => exam.TimeSlot.DateTime).ToList();
            userExaminations[0].CurrentStatus = Examination.Status.Ready;
            _examinationStorage.SaveExaminations(_examinations);
        }
        public List<Examination> GetUserExaminations(string username)
        {
            List<Examination> userExaminations = new List<Examination>();
            foreach (var examination in _examinations)
            {
                if (examination.PatientUsername == username || examination.DoctorUsername == username)
                {
                    userExaminations.Add(examination);
                }
            }
            return userExaminations;
        }

        public List<Examination> GetUpcomingUserExaminations(string username)
        {
            List<Examination> upcomingExaminations = new List<Examination>();
            foreach (var examination in GetUserExaminations(username))
            {
                if (examination.TimeSlot.DateTime.AddMinutes(examination.TimeSlot.Duration).Subtract(DateTime.Now).TotalMinutes > 0)
                {
                    upcomingExaminations.Add(examination);
                }

            }
            return upcomingExaminations;
        }

        //Finds first available time slot in the list of examinations starting now
        public TimeSlot GetFirstAvailableTimeSlot(List<Examination> examinations, int duration)
        {
            TimeSlot timeSlot = new TimeSlot(DateTime.Now, duration);
            if (examinations.Count() == 0)
            {
                return timeSlot;
            }

            List<Examination> sortedExaminations = examinations.OrderBy(exam => exam.TimeSlot.DateTime).ToList();
            for (int i = 0; i < sortedExaminations.Count(); i++)
            {
                if (timeSlot.IsOverlappingWith(sortedExaminations[i].TimeSlot))
                {
                    //next time slot will start after examination ends
                    timeSlot.DateTime = sortedExaminations[i].TimeSlot.DateTime.AddMinutes(sortedExaminations[i].TimeSlot.Duration);
                }
                else
                {
                    return timeSlot;
                }

            }
            return timeSlot;
        }

        
        public void AddExamination(Examination examination)
        {
            _examinations.Add(examination);
            _examinationStorage.SaveExaminations(_examinations);
        }

        public void DeleteExamination(Examination examination)
        {
            _examinations.Remove(examination);
            _examinationStorage.SaveExaminations(_examinations);
        }

        public void UpdateExamination(Examination examination)
        {
            var index = _examinations.IndexOf(examination);
            _examinations[index] = examination;
            _examinationStorage.SaveExaminations(_examinations);
        }

        public void UpdateExaminations(List<Examination> examinations)
        {
            _examinations = examinations;
            _examinationStorage.SaveExaminations(_examinations);
        }
        public void DeleteExaminations()
        {
            _examinations.Clear();
            _examinationStorage.SaveExaminations(_examinations);
        }


        public List<TimeSlot> GetAvailableTimeSlots(List<Examination> scheduledExaminations, int startHour, int endHour, int slotDuration, int requiredSlots)
        {
            var availableTimeSlots = new List<TimeSlot>();
            var nextDay = DateTime.Now.Date.AddDays(1);
            var slotStart = nextDay.AddHours(startHour);
            var slotEnd = nextDay.AddHours(endHour).AddMinutes(-slotDuration);

            while (availableTimeSlots.Count < requiredSlots && slotStart <= slotEnd)
            {
                var proposedSlot = new TimeSlot(slotStart, slotDuration);
                var isOverlapping = scheduledExaminations.Any(exam => exam.TimeSlot.IsOverlappingWith(proposedSlot));

                if (!isOverlapping)
                {
                    availableTimeSlots.Add(proposedSlot);
                }

                slotStart = slotStart.AddMinutes(slotDuration);
            }

            return availableTimeSlots;
        }

        public List<Examination> GetClosestExaminations(bool prioritizeDoctor, Doctor doctor, int startHour, int endHour, DateTime lastPossibleDate, string patientUsername, int duration = 15, int recommendationsCount = 3)
        {
            return prioritizeDoctor
                ? GetRecommendExaminationsByDoctor(doctor, startHour, endHour, lastPossibleDate, patientUsername, false, duration, recommendationsCount)
                : GetRecommendExaminationsByTime(startHour, endHour, lastPossibleDate, patientUsername,doctor, duration, recommendationsCount);
        }

        public List<Examination> GetRecommendExaminationsByTime(int startHour, int endHour, DateTime lastPossibleDate, string patientUsername, Doctor preferredDoctor, int duration = 15, int recommendationsCount = 3)
        {
            var recommendedExaminations = new List<Examination>();
            var allDoctors = new DoctorDAO().GetDoctors();
            var availableTimeSlots = GetAvailableTimeSlots(_examinations, startHour, endHour, duration, recommendationsCount);

            foreach (var slot in availableTimeSlots)
            {
                if (!IsDoctorOccupiedInSlot(preferredDoctor, slot))
                {
                    recommendedExaminations.Add(CreateExamination(preferredDoctor.Username, slot, patientUsername));
                    if (recommendedExaminations.Count >= recommendationsCount)
                    {
                        break;
                    }
                }
                else
                {
                    foreach (var doctor in allDoctors.Where(d => d.Username != preferredDoctor.Username))
                    {
                        if (!IsDoctorOccupiedInSlot(doctor, slot))
                        {
                            recommendedExaminations.Add(CreateExamination(doctor.Username, slot, patientUsername));
                            if (recommendedExaminations.Count >= recommendationsCount)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return recommendedExaminations;
        }

        private bool IsDoctorOccupiedInSlot(Doctor doctor, TimeSlot slot)
        {
            return GetUserExaminations(doctor.Username).Any(exam => exam.TimeSlot.IsOverlappingWith(slot));
        }

        public List<Examination> GetRecommendExaminationsByDoctor(Doctor doctor, int startHour, int endHour, DateTime lastPossibleDate, string patientUsername, bool flexible = false, int duration = 15, int recommendationsCount = 3)
        {
            var recommendedExaminations = new List<Examination>();
            var nextDay = DateTime.Now.Date.AddDays(1);

            while (recommendedExaminations.Count < recommendationsCount && nextDay <= lastPossibleDate)
            {
                var doctorExaminations = GetUserExaminations(doctor.Username).Where(exam => exam.DoctorUsername == doctor.Username).ToList();
                var availableTimeSlots = GetAvailableTimeSlots(doctorExaminations, startHour, endHour, duration, recommendationsCount - recommendedExaminations.Count);

                foreach (var slot in availableTimeSlots)
                {
                    recommendedExaminations.Add(CreateExamination(doctor.Username, slot, patientUsername));

                    if (recommendedExaminations.Count >= recommendationsCount)
                    {
                        break;
                    }
                }

                nextDay = nextDay.AddDays(1);
            }

            if (flexible && recommendedExaminations.Count < recommendationsCount)
            {
                var remainingRecommendations = recommendationsCount - recommendedExaminations.Count;
                var additionalExaminations = GetRecommendExaminationsByDoctor(doctor, 0, 24, lastPossibleDate, patientUsername, false, duration, remainingRecommendations);
                recommendedExaminations.AddRange(additionalExaminations);
            }

            return recommendedExaminations;
        }

        private Examination CreateExamination(string doctorUsername, TimeSlot slot, string patientUsername)
        {
            return new Examination
            {
                DoctorUsername = doctorUsername,
                TimeSlot = slot,
                PatientUsername = patientUsername
            };
        }

    }
}
