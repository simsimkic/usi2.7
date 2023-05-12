using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.VisualBasic;
using ZdravoCorp.DataAccess;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;
using ZdravoCorp.ViewModels.Nurse;
using ZdravoCorp.Views;

namespace ZdravoCorp.ViewModels
{
    internal class MedicalRecordViewModel : ViewModelBase
    {
        public ExaminationDAO examinationDao = new ExaminationDAO();
        private PatientDAO _patientDao = new PatientDAO();
        private readonly Patient _selectedPatient;
        public bool InputFields { get; set; }
        public bool AddDeleteAnamnese { get; set; }
        public bool UpdateAnamnese { get; set; }
        public bool IsSaveCloseButtonVisible { get; set; }
        public bool IsFinishButtonVisible { get; set; }

        private MedicalRecordPermission _medicalRecordPermission;
        public enum MedicalRecordPermission
        {
            DoctorView,
            DoctorViewUpdate,
            DoctorViewAddUpdateDelete,
            Nurse,
            PatientView
        }
        public string Name => _selectedPatient.FirstName;
        public string Surname => _selectedPatient.LastName;
        public DateTime DateOfBirth => _selectedPatient.DateOfBirth;
        public Gender Gender => _selectedPatient.Gender;
        private ICollectionView _medicalHistoryView;
        public ICollectionView MedicalHistoryView
        {
            get => _medicalHistoryView;
            set
            {
                if (_medicalHistoryView == value) return;
                _medicalHistoryView = value;
                OnPropertyChanged(nameof(MedicalHistoryView));
            }
        }
        private ICollectionView _allergensView;
        public ICollectionView AllergensView
        {
            get => _allergensView;
            set
            {
                if (_allergensView == value) return;
                _allergensView = value;
                OnPropertyChanged(nameof(AllergensView));
            }
        }
        private ICollectionView _anamnesisView;
        public ICollectionView AnamnesisView
        {
            get => _anamnesisView;
            set
            {
                if (_anamnesisView == value) return;
                _anamnesisView = value;
                OnPropertyChanged(nameof(AnamnesisView));
                
            }
        }


        private double _weight;
        public double Weight
        {
            get => _weight;
            set
            {
                if (_weight == value) return;
                _weight = value;
                OnPropertyChanged(nameof(Weight));
                _selectedPatient.PatientRecord.Weight = _weight;
            }
        }
        private double _height;
        public double Height
        {
            get => _height;
            set
            {
                if (_height == value) return;
                _height = value;
                OnPropertyChanged(nameof(Height));
                _selectedPatient.PatientRecord.Height = _height;
            }
        }

        private string _selectedMedicalHistory;
        public string SelectedMedicalHistory
        {
            get => _selectedMedicalHistory;
            set
            {
                if (_selectedMedicalHistory == value) return;
                _selectedMedicalHistory = value;
                OnPropertyChanged(nameof(SelectedMedicalHistory));
                if (_selectedMedicalHistory == null) return;
                MedicalHistoryInput = _selectedMedicalHistory;
            }
        }

        private string _medicalHistoryInput;
        public string MedicalHistoryInput
        {
            get => _medicalHistoryInput;
            set
            {
                if (_medicalHistoryInput == value) return;
                _medicalHistoryInput = value;
                OnPropertyChanged(nameof(MedicalHistoryInput));
            }
        }

        private string _selectedAllergen;
        public string SelectedAllergen
        {
            get => _selectedAllergen;
            set
            {
                if (_selectedAllergen == value) return;
                _selectedAllergen = value;
                OnPropertyChanged(nameof(SelectedAllergen));
                if (_selectedAllergen == null) return;
                AllergenInput = _selectedAllergen;
            }
        }
        private string _allergenInput;
        public string AllergenInput
        {
            get => _allergenInput;
            set
            {
                if (_allergenInput == value) return;
                _allergenInput = value;
                OnPropertyChanged(nameof(AllergenInput));
            }
        }

        private Anamnesis _selectedAnamnese;
        public Anamnesis SelectedAnamnese
        {
            get => _selectedAnamnese;
            set
            {
                if (_selectedAnamnese == value) return;
                _selectedAnamnese = value;
                OnPropertyChanged(nameof(SelectedAnamnese));
                if (_selectedAnamnese == null) return;
                AnamneseDate = _selectedAnamnese.Date;
                SymptomsInput = _selectedAnamnese.Symptoms;
                ConclusionInput = _selectedAnamnese.Conclusion;
            }
        }

        private DateTime _anamneseDate = DateTime.Now;
        public DateTime AnamneseDate
        {
            get => _anamneseDate;
            set
            {
                if (_anamneseDate == value) return;
                _anamneseDate = value;
                OnPropertyChanged(nameof(AnamneseDate));
            }
        }

        private string _symptomsInput;
        public string SymptomsInput
        {
            get => _symptomsInput;
            set
            {
                if (_symptomsInput == value) return;
                _symptomsInput = value;
                OnPropertyChanged(nameof(SymptomsInput));
            }
        }

        private string _conclusionInput;
        public string ConclusionInput
        {
            get => _conclusionInput;
            set
            {
                if (_conclusionInput == value) return;
                _conclusionInput = value;
                OnPropertyChanged(nameof(ConclusionInput));
            }
        }

        private bool _isConclusionEnabled;
        public bool IsConclusionEnabled
        {
            get => _isConclusionEnabled;
            set
            {
                if (_isConclusionEnabled == value) return;
                _isConclusionEnabled = value;
                OnPropertyChanged(nameof(IsConclusionEnabled));
            }
        }

        //ADDITION BUTTONS
        public ICommand AddToMedicalHistory => new RelayCommand(AddSelectedMedicalHistory);
        private void AddSelectedMedicalHistory(object parameter)
        {
            if (String.IsNullOrEmpty(_medicalHistoryInput))
            {
                MessageBox.Show("Field must contain text", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _selectedPatient.PatientRecord.MedicalHistory.Add(_medicalHistoryInput);
            MedicalHistoryInput = string.Empty;
            _medicalHistoryView.Refresh();
        }
        public ICommand AddToAllergens => new RelayCommand(AddSelectedAllergen);
        private void AddSelectedAllergen(object parameter)
        {
            if (String.IsNullOrEmpty(_allergenInput))
            {
                MessageBox.Show("Field must contain text", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _selectedPatient.PatientRecord.Allergens.Add(_allergenInput);
            AllergenInput = string.Empty;
            _allergensView.Refresh();
        }
        public ICommand AddToAnamnesis => new RelayCommand(AddSelectedAnamnese);
        private void AddSelectedAnamnese(object parameter)
        {
            
            if (_medicalRecordPermission == MedicalRecordPermission.Nurse)
            {
                if (string.IsNullOrEmpty(_symptomsInput))
                {
                    MessageBox.Show("Field must contain text", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                List<Anamnesis> allAnamnesis = _selectedPatient.PatientRecord.Anamnesis;
                foreach (var anamnesis in allAnamnesis)
                {
                    if (DateTime.Now.Subtract(anamnesis.Date).TotalMinutes < 30)
                    {
                        MessageBox.Show("Invalid option. You added anemnesis recently.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                if (IsTimeForPatientAnamnesis(_selectedPatient))
                {
                    MessageBox.Show("Invalid time to start anamnesis for this patient", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    var patientExaminationsModel = new PatientExaminationViewModel(examinationDao,_selectedPatient);
                    var examinationWindow = new PatientExaminationView { DataContext = patientExaminationsModel };
                    examinationWindow.ShowDialog();
                    SymptomsInput = string.Empty;
                    return;
                }
            }
            MessageBox.Show("Successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            if (MedicalRecordPermission.Nurse == _medicalRecordPermission)
            {
                _conclusionInput = string.Empty;
                examinationDao.ChangeExaminationStatusToReady(_selectedPatient.Username);
            } 
            _selectedPatient.PatientRecord.Anamnesis.Add(new Anamnesis(DateTime.Now, _symptomsInput, _conclusionInput));
            SymptomsInput = string.Empty;
            ConclusionInput = string.Empty;
            _anamnesisView.Refresh();
        }

        //UPDATE BUTTONS
        public ICommand UpdateMedicalHistory => new RelayCommand(UpdateSelectedMedicalHistory);
        private void UpdateSelectedMedicalHistory(object parameter)
        {
            if (string.IsNullOrEmpty(_medicalHistoryInput))
            {
                MessageBox.Show("Field must contain text", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _selectedPatient.PatientRecord.MedicalHistory.Remove(_selectedMedicalHistory);
            _selectedPatient.PatientRecord.MedicalHistory.Add(_medicalHistoryInput);
            MedicalHistoryInput = string.Empty;
            _medicalHistoryView.Refresh();
        }
        public ICommand UpdateAllergens => new RelayCommand(UpdateSelectedAllergen);
        private void UpdateSelectedAllergen(object parameter)
        {
            if (string.IsNullOrEmpty(_allergenInput))
            {
                MessageBox.Show("Field must contain text", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _selectedPatient.PatientRecord.Allergens.Remove(_selectedAllergen);
            _selectedPatient.PatientRecord.Allergens.Add(_allergenInput);
            AllergenInput = string.Empty;
            _allergensView.Refresh();
        }
        public ICommand UpdateAnamnesis => new RelayCommand(UpdateSelectedAnamnese);
        private void UpdateSelectedAnamnese(object parameter)
        {
            MessageBox.Show("Successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _selectedPatient.PatientRecord.Anamnesis.Remove(_selectedAnamnese);
            _selectedPatient.PatientRecord.Anamnesis.Add(new Anamnesis(_anamneseDate, _symptomsInput, _conclusionInput));
            _anamnesisView.Refresh();
        }


        //DELETE BUTTONS
        public ICommand DeleteFromMedicalHistory => new RelayCommand(DeleteSelectedMedicalHistory);
        private void DeleteSelectedMedicalHistory(object parameter)
        {
            if (string.IsNullOrEmpty(_selectedMedicalHistory))
            {
                MessageBox.Show("Field doesn't contain existing condition.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            MessageBox.Show("Successfully deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _selectedPatient.PatientRecord.MedicalHistory.Remove(_selectedMedicalHistory);
            _medicalHistoryView.Refresh();
        }
        public ICommand DeleteFromAllergens => new RelayCommand(DeleteSelectedAllergen);
        private void DeleteSelectedAllergen(object parameter)
        {
            if (string.IsNullOrEmpty(_selectedAllergen))
            {
                MessageBox.Show("Field doesn't contain existing allergen.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            MessageBox.Show("Successfully deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _selectedPatient.PatientRecord.Allergens.Remove(_selectedAllergen);
            _allergensView.Refresh();
        }
        public ICommand DeleteFromAnamnesis => new RelayCommand(DeleteSelectedAnamnese);
        private void DeleteSelectedAnamnese(object parameter)
        {
            if (string.IsNullOrEmpty(_symptomsInput))
            {
                MessageBox.Show("Field must contain text", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            
            _selectedPatient.PatientRecord.Anamnesis.Remove(_selectedAnamnese);

            AnamneseDate = DateTime.Now;
            SymptomsInput = string.Empty;
            ConclusionInput = string.Empty;

            _anamnesisView.Refresh();
            MessageBox.Show("Successfully deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public ICommand ResetMedicalHistoryAndAllergensFields => new RelayCommand(MedicalHistoryAndAllergensFieldsToEmpty);
        private void MedicalHistoryAndAllergensFieldsToEmpty(object parameter)
        {
            MedicalHistoryInput = string.Empty;
            AllergenInput = string.Empty;
        }
        public ICommand ResetAnamneseFields => new RelayCommand(AnamneseFieldsToEmpty);
        private void AnamneseFieldsToEmpty(object parameter)
        {
            AnamneseDate = DateTime.Now;
            SymptomsInput = string.Empty;
            ConclusionInput = string.Empty;
        }

        public ICommand Save => new RelayCommand(SaveChanges);
        private void SaveChanges(object parameter)
        {
            MessageBox.Show("Successfully saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _patientDao.CreatePatient(_selectedPatient);
        }
        public ICommand Close => new RelayCommand(CancelChanges);
        private void CancelChanges(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Save changes before close!\nAre you sure you want to close?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                }
            }
        }
        public ICommand Finish => new RelayCommand(FinishExamination);
        private void FinishExamination(object parameter)
        {
            MessageBox.Show("Successfully finished", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _patientDao.CreatePatient(_selectedPatient);
            if (_medicalRecordPermission != MedicalRecordPermission.DoctorViewAddUpdateDelete) return;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext != this) continue;
                window.Close();
                if (_medicalRecordPermission != MedicalRecordPermission.DoctorViewAddUpdateDelete) continue;
                var doctorSpentEquipmentView = new DoctorSpentEquipmentView();
                doctorSpentEquipmentView.ShowDialog();
            }
        }

        public MedicalRecordViewModel(Patient selectedPatient, MedicalRecordPermission medicalRecordPermission)
        {
            if (medicalRecordPermission == MedicalRecordPermission.Nurse)
            {
                IsConclusionEnabled = false;
            }
            else{
                IsConclusionEnabled = true;
            }

            SetWindowAccessType(medicalRecordPermission);
            _selectedPatient = selectedPatient;
            _medicalRecordPermission = medicalRecordPermission;
            MedicalHistoryView = CollectionViewSource.GetDefaultView(_selectedPatient.PatientRecord.MedicalHistory);
            AllergensView = CollectionViewSource.GetDefaultView(_selectedPatient.PatientRecord.Allergens);
            AnamnesisView = CollectionViewSource.GetDefaultView(_selectedPatient.PatientRecord.Anamnesis);
            Weight = _selectedPatient.PatientRecord.Weight;
            Height = _selectedPatient.PatientRecord.Height;
        }

        // depending on who accesses the window, the desired appearance is created
        private void SetWindowAccessType(MedicalRecordPermission medicalRecordPermission)
        {
            switch (medicalRecordPermission)
            {
                case MedicalRecordPermission.DoctorView:
                    InputFields = false;
                    AddDeleteAnamnese = true;
                    UpdateAnamnese = true;
                    break;
                case MedicalRecordPermission.DoctorViewUpdate:
                    InputFields = true;
                    AddDeleteAnamnese = false;
                    UpdateAnamnese = true;
                    IsSaveCloseButtonVisible = true;
                    IsFinishButtonVisible = false;
                    break;
                case MedicalRecordPermission.DoctorViewAddUpdateDelete:
                    InputFields = true;
                    AddDeleteAnamnese = true;
                    UpdateAnamnese = true;
                    IsSaveCloseButtonVisible = false;
                    IsFinishButtonVisible = true;
                    break;
                case MedicalRecordPermission.Nurse:
                    InputFields = true;
                    AddDeleteAnamnese = true;
                    UpdateAnamnese = false;
                    IsSaveCloseButtonVisible = false;
                    IsFinishButtonVisible = true;
                    break;
                case MedicalRecordPermission.PatientView:
                    InputFields = false;
                    AddDeleteAnamnese = false;
                    UpdateAnamnese = false;
                    IsSaveCloseButtonVisible = false;
                    IsFinishButtonVisible = false;
                    break;

            }
        }

        public bool IsTimeForPatientAnamnesis(Patient patient)
        {
            List<Examination> patientExaminations = examinationDao.GetUpcomingUserExaminations(patient.Username);
            foreach (Examination examination in patientExaminations)
            {
                if (examination.TimeSlot.DateTime.Subtract(DateTime.Now).TotalMinutes < 15)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
