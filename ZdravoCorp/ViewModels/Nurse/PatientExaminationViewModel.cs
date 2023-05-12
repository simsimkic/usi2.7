using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;
using ZdravoCorp.Views;

namespace ZdravoCorp.ViewModels.Nurse
{
    internal class PatientExaminationViewModel : ViewModelBase
    {
        private Patient _patient;
        public ExaminationDAO ExaminationDao;

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

        public PatientExaminationViewModel(ExaminationDAO examinationDao, Patient patient)
        {
            _patient = patient;
            ExaminationDao = examinationDao;
            Examinations = new ObservableCollection<Examination>(ExaminationDao.GetUserExaminations(_patient.Username) ?? new List<Examination>());
            _examinationsView = CollectionViewSource.GetDefaultView(Examinations);
        }
    }
}
