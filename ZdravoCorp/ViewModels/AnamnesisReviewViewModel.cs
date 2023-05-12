using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.DataAccess;
using ZdravoCorp.Models;
using ZdravoCorp.ViewModels;
using ZdravoCorp.Models.DAO;
using System.Runtime.CompilerServices;
using ZdravoCorp.Views;

namespace ZdravoCorp.ViewModels
{
    public class AnamnesisReviewViewModel : PatientViewModel
    {
        private readonly DoctorDAO _doctorDAO;
        private ObservableCollection<AnamnesisReview> _anamnesisReviews;
        private string _selectedSortOption;
        private string _searchKeyword;

        public ICommand OpenMedicalRecordCommand => new RelayCommand(OpenMedicalRecord);
        public ICommand SortCommand => new RelayCommand(SortAnamnesisReviews);
        public ICommand SearchCommand => new RelayCommand(SearchAnamnesisReviews);

        public ObservableCollection<string> SortOptions { get; }
        public ICollectionView AnamnesisReviewsView { get; }

        public ObservableCollection<AnamnesisReview> AnamnesisReviews
        {
            get => _anamnesisReviews;
            set
            {
                _anamnesisReviews = value;
                OnPropertyChanged();
            }
        }

        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                _selectedSortOption = value;
                OnPropertyChanged(nameof(SelectedSortOption));
                SortCommand.Execute(null);
            }
        }

        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged();
            }
        }

        public AnamnesisReviewViewModel()
        {
            _doctorDAO = new DoctorDAO();
            _anamnesisReviews = new ObservableCollection<AnamnesisReview>();
            SortOptions = new ObservableCollection<string> { "Doctor", "Specialization", "Date" };

            LoadAnamnesisReviews();
            AnamnesisReviewsView = CollectionViewSource.GetDefaultView(AnamnesisReviews);
        }

        private void LoadAnamnesisReviews()
        {
            foreach (var anamnesis in SignedPatient.PatientRecord.Anamnesis)
            {
                var doctor = _doctorDAO.FindDoctorForAnamnesis(anamnesis.Date, SignedPatient.Username);
                if (doctor != null)
                {
                    AnamnesisReviews.Add(new AnamnesisReview(anamnesis, doctor));
                }
            }
        }

        private void OpenMedicalRecord(object parameter)
        {
            var patientDAO = new PatientDAO();
            var medicalRecord = new MedicalRecordViewModel(patientDAO.GetPatientByUsername(SignedPatient.Username), MedicalRecordViewModel.MedicalRecordPermission.PatientView);
            var medicalRecordWindow = new MedicalRecordWindow { DataContext = medicalRecord };
            medicalRecordWindow.ShowDialog();
        }

        private void SortAnamnesisReviews(object parameter)
        {
            switch (SelectedSortOption)
            {
                case "Doctor":
                    AnamnesisReviews = new ObservableCollection<AnamnesisReview>(AnamnesisReviews.OrderBy(x => x.DoctorUsername));
                    break;
                case "Specialization":
                    AnamnesisReviews = new ObservableCollection<AnamnesisReview>(AnamnesisReviews.OrderBy(x => x.DoctorSpecialization));
                    break;
                case "Date":
                    AnamnesisReviews = new ObservableCollection<AnamnesisReview>(AnamnesisReviews.OrderBy(x => x.ExaminationDate));
                    break;
            }
        }

        private void SearchAnamnesisReviews(object parameter)
        {
            if (string.IsNullOrWhiteSpace(SearchKeyword))
            {
                AnamnesisReviewsView.Filter = null;
            }
            else
            {
                AnamnesisReviewsView.Filter = anamnesisReviewObj =>
                {
                    AnamnesisReview review = anamnesisReviewObj as AnamnesisReview;
                    return review.SymptomsAndConclusion.ToLower().Contains(SearchKeyword.ToLower());
                };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}