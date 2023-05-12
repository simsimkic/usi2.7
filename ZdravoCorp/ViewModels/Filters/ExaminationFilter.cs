using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;

namespace ZdravoCorp.ViewModels.Filters
{
    public class ExaminationFilter
    {
        private readonly DoctorCrudViewModel _doctorCrudViewModel;
        private readonly DoctorScheduleViewModel _doctorScheduleViewModel;

        public ExaminationFilter(DoctorCrudViewModel doctorCrudViewModel)
        {
            _doctorCrudViewModel = doctorCrudViewModel;
        }
        public ExaminationFilter(DoctorScheduleViewModel doctorScheduleViewModel)
        {
            _doctorScheduleViewModel = doctorScheduleViewModel;
        }

        public bool MatchesDate(DateTime examinationDate)
        {
            if (_doctorCrudViewModel.IsNextThreeDaysChecked)
            {
                var today = DateTime.Today;
                var threeDaysFromNow = today.AddDays(2);
                return examinationDate >= today && examinationDate <= threeDaysFromNow;
            }

            if (!_doctorCrudViewModel.IsExactDateChecked) return true;
            {
                return examinationDate.Date == _doctorCrudViewModel.ExactDate.Date;
            }
        }
        public bool MatchesSearchText(string fullName)
        {
            if (string.IsNullOrWhiteSpace(_doctorScheduleViewModel.SearchText)) return true;
            return fullName.ToLower().Contains(_doctorScheduleViewModel.SearchText.ToLower());
        }
        public bool MatchesSelectedStatus(string status)
        {
            if (_doctorScheduleViewModel.SelectedStatus == "All") return true;
            return status == _doctorScheduleViewModel.SelectedStatus;
        }
        public bool MatchesCheckedType(bool type)
        {
            if (_doctorScheduleViewModel.IsExaminationChecked && !type) return true;
            if (_doctorScheduleViewModel.IsOperationChecked && type) return true;
            return _doctorScheduleViewModel is { IsExaminationChecked: false, IsOperationChecked: false };
        }
        public bool MatchesDateSchedule(DateTime examinationDate)
        {
            if (_doctorScheduleViewModel.IsNextThreeDaysChecked)
            {
                var today = DateTime.Today;
                var threeDaysFromNow = today.AddDays(2);
                return examinationDate >= today && examinationDate <= threeDaysFromNow;
            }

            if (!_doctorScheduleViewModel.IsExactDateChecked) return true;
            {
                return examinationDate.Date == _doctorScheduleViewModel.ExactDate.Date;
            }
        }
    }
}
