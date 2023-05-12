using System;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Data;
using ZdravoCorp.Models.DAO;

namespace ZdravoCorp.Converters
{
    public class DoctorNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the patient with the given username
            DoctorDAO doctorDao = new DoctorDAO();
            var doctor = doctorDao.GetDoctorByUsername(value.ToString());

            // Return the patient's full name
            if (doctor != null)
                return doctor.FirstName + " " + doctor.LastName;
            return "Doctor";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            // Return the patient's username
            return value.ToString();
        }
    }
}

