using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;

namespace ZdravoCorp.ViewModels.Filters
{
    internal class PatientFilter
    {
        public string SearchText { get; set; }
        public string Gender { get; set; }
       

        public PatientFilter(string _searchText, string _gender)
        {
            SearchText = _searchText;
            Gender = _gender;
        }
        public bool MatchesSearchText(string fullName)
        {
           
            if (string.IsNullOrWhiteSpace(SearchText)) return true;
            return fullName.ToLower().Contains(SearchText.ToLower());
          
        }    
        public bool MatchesSelectedGender(Gender gender)
        {          
            if (Gender == "All") return true;
            return gender.ToString() == Gender;
           
        }
    }
}
