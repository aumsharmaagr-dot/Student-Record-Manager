
using System;

namespace StudentRecordManagerC_
{
    public class Student
    {
        public int RollNo { get; set; }

        public string Name { get; set; }

        public DateTime DOB { get; set; }

        public int Marks { get; set; }
        public int CalculateAge()
        {
            int age = DateTime.Now.Year - DOB.Year;

            if (DOB.Date > DateTime.Now.Date.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
