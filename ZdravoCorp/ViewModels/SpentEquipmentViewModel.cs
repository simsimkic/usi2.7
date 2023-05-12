using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.ViewModels
{
    public class SpentEquipmentViewModel : ViewModelBase
    {
		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		private int _spent;
		public int Spent
		{
			get
			{
				return _spent;
			}
			set
			{
				_spent = value;
				OnPropertyChanged(nameof(Spent));
			}
		}

		private int _available;
		public int Available
		{
			get
			{
				return _available;
			}
			set
			{
				_available = value;
				OnPropertyChanged(nameof(Available));
			}
		}

        public SpentEquipmentViewModel(string name, int spent, int available)
        {
			_name = name;
			_spent = spent;
			_available = available;
        }
    }
}
