using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Validations
{
    public class CustomDateRangeAttribute : ValidationAttribute
    {
        private readonly DateTime _minimumDate;

        public CustomDateRangeAttribute()
        {
            _minimumDate = new DateTime(2018, 1, 1);
        }

        public override bool IsValid(object value)
        {
            if (value is DateTime birthday)
            {
                return birthday < _minimumDate;
            }

            return false;
        }
    }
}
