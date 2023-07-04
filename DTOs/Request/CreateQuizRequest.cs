using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class CreateQuizRequest
    {
        private int numberQuestion;
        private int time;
     
        [Required]
        public int NumberQuestion
        {
            get { return numberQuestion; }
            set
            {
                if (value != 10 && value != 20)
                {
                    throw new ValidationException("The NumberQuestion value must be either 10 or 20.");
                }
                numberQuestion = value;
            }
        }

        [Required]
        public int Time
        {
            get { return time; }
            set
            {
                if (NumberQuestion == 10 && value != 135)
                {
                    throw new ValidationException("The Time value must be 135 when NumberQuestion is 10.");
                }
                if (NumberQuestion == 20 && value != 270)
                {
                    throw new ValidationException("The Time value must be 270 when NumberQuestion is 20.");
                }
                time = value;
            }
        }
    }
}
