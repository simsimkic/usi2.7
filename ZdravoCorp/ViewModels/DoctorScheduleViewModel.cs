using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Models;
using ZdravoCorp.ViewModels.Filters;
using ZdravoCorp.Views;

namespace ZdravoCorp.ViewModels
{
    public class DoctorScheduleViewModel : DoctorViewModel
    {
        private readonly List<string> _status = new() { "All", "Scheduled", "Ready", "Finished" };
        public List<string> Status => _status;

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus == value) return;
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
                _examinationsView.Refresh();
            }
        }
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
        private Examination _selectedExamination;
        public Examination SelectedExamination
        {
            get => _selectedExamination;
            set
            {
                if (value == _selectedExamination) return;
                _selectedExamination = value;
                OnPropertyChanged(nameof(SelectedExamination));
            }
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
                _examinationsView.Refresh();
            }
        }
        private bool _isNextThreeDaysChecked;
        public bool IsNextThreeDaysChecked
        {
            get => _isNextThreeDaysChecked;
            set
            {
                if (_isNextThreeDaysChecked == value) return;
                _isNextThreeDaysChecked = value;
                OnPropertyChanged(nameof(IsNextThreeDaysChecked));
                if (_isExactDateChecked) IsExactDateChecked = false;
                _examinationsView.Refresh();
            }
        }

        private bool _isExactDateChecked;
        public bool IsExactDateChecked
        {
            get => _isExactDateChecked;
            set
            {
                if (_isExactDateChecked == value) return;
                _isExactDateChecked = value;
                OnPropertyChanged(nameof(IsExactDateChecked));
                if (_isNextThreeDaysChecked) IsNextThreeDaysChecked = false;
                _examinationsView.Refresh();
            }
        }
        private bool _isExaminationChecked;
        public bool IsExaminationChecked
        {
            get => _isExaminationChecked;
            set
            {
                if (_isExaminationChecked == value) return;
                _isExaminationChecked = value;
                OnPropertyChanged(nameof(IsExaminationChecked));
                if (_isOperationChecked) IsOperationChecked = false;
                _examinationsView.Refresh();
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
                if (_isExaminationChecked) IsExaminationChecked = false;
                _examinationsView.Refresh();
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
                _examinationsView.Refresh();
            }
        }

        public ICommand StartExamination => new RelayCommand(OpenMedicalRecord);
        private void OpenMedicalRecord(object parameter)
        {
            var patientUsername = SelectedExamination.PatientUsername;
            _selectedExamination.CurrentStatus = Examination.Status.Finished;
            ExaminationDAO.UpdateExamination(_selectedExamination);
            _examinationsView.Refresh();
            var medicalRecord = new MedicalRecordViewModel(PatientDAO.GetPatientByUsername(patientUsername), MedicalRecordViewModel.MedicalRecordPermission.DoctorViewAddUpdateDelete);
            var medicalRecordWindow = new MedicalRecordWindow { DataContext = medicalRecord };
            medicalRecordWindow.ShowDialog();
        }
        public DoctorScheduleViewModel()
        {
            _examinationsView = new ListCollectionView(ExaminationDAO.GetUserExaminations(SignedDoctor.Username));
            var examinationFilter = new ExaminationFilter(this);
            CreateFilter(examinationFilter);

        }

        private void CreateFilter(ExaminationFilter examinationFilter)
        {
            _examinationsView.Filter = obj =>
            {
                if (obj is not Examination examination) return false;
                return examinationFilter.MatchesDateSchedule(examination.TimeSlot.DateTime) &&
                       examinationFilter.MatchesSelectedStatus(examination.CurrentStatus.ToString()) &&
                       examinationFilter.MatchesCheckedType(examination.IsOperation) &&
                       examinationFilter.MatchesSearchText(PatientDAO.GetPatientByUsername(examination.PatientUsername)
                           .ToString());
            };
        }
    }
}
