using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace StudentRecordManagerC_
{
    internal class Program
    {
        static List<Student> students = new List<Student>();

        static void Main(string[] args)
        {
            LoadRecords();

            while (true)
            {
                Console.WriteLine("\n===== STUDENT RECORD MANAGER =====");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. View Students");
                Console.WriteLine("3. Search Student");
                Console.WriteLine("4. Delete Student");
                Console.WriteLine("5. Save Records");
                Console.WriteLine("6. Exit");

                Console.Write("\nEnter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid Input!");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        AddStudent();
                        break;

                    case 2:
                        ViewStudents();
                        break;

                    case 3:
                        SearchStudent();
                        break;

                    case 4:
                        DeleteStudent();
                        break;

                    case 5:
                        SaveRecords();
                        break;

                    case 6:
                        SaveRecords();
                        Console.WriteLine("Thank You for Using Student Record Manager!");
                        Console.WriteLine("Program Exited Successfully.");
                        return;

                    default:
                        Console.WriteLine("Invalid Choice!");
                        break;
                }
            }
        }

        // ADD STUDENT
        static void AddStudent()
        {
            Student s = new Student();

            Console.Write("\nEnter Roll Number: ");

            if (!int.TryParse(Console.ReadLine(), out int rollNo))
            {
                Console.WriteLine("Invalid Roll Number!");
                return;
            }

            if (students.Exists(x => x.RollNo == rollNo))
            {
                Console.WriteLine("Roll Number Already Exists!");
                return;
            }

            s.RollNo = rollNo;

            Console.Write("Enter Student Name: ");
            s.Name = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(s.Name))
            {
                Console.WriteLine("Name Cannot Be Empty!");
                return;
            }

            Console.Write("Enter Date of Birth (dd-MM-yyyy): ");

            if (!DateTime.TryParseExact(
                Console.ReadLine(),
                "dd-MM-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime dob))
            {
                Console.WriteLine("Invalid Date of Birth!");
                return;
            }

            if (dob.Date > DateTime.Today)
            {
                Console.WriteLine("Invalid Date of Birth!");
                return;
            }

            s.DOB = dob;

            Console.Write("Enter Student Marks: ");

            if (!int.TryParse(Console.ReadLine(), out int marks) ||
                marks < 0 || marks > 100)
            {
                Console.WriteLine("Marks Must Be Between 0 and 100!");
                return;
            }

            s.Marks = marks;

            students.Add(s);

            Console.WriteLine("Student Added Successfully!");
            SaveRecords();
        }

        // VIEW STUDENTS
        static void ViewStudents()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("No Student Records Found!");
                return;
            }

            foreach (Student s in students)
            {
                Console.WriteLine("\n----------------------");
                Console.WriteLine("Roll Number: " + s.RollNo);
                Console.WriteLine("Name: " + s.Name);
                Console.WriteLine("Date of Birth: " + s.DOB.ToString("dd-MM-yyyy"));
                Console.WriteLine("Age: " + s.CalculateAge());
                Console.WriteLine("Marks: " + s.Marks);
                Console.WriteLine("----------------------");
            }
        }

        // SEARCH STUDENT
        static void SearchStudent()
        {
            Console.Write("\nEnter Student Name: ");

            string name = (Console.ReadLine() ?? "").Trim();

            bool found = false;

            foreach (Student s in students)
            {
                if (s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("\nStudent Found!");
                    Console.WriteLine("Roll No: " + s.RollNo);
                    Console.WriteLine("Name: " + s.Name);
                    Console.WriteLine("DOB: " + s.DOB.ToString("dd-MM-yyyy"));
                    Console.WriteLine("Age: " + s.CalculateAge());
                    Console.WriteLine("Marks: " + s.Marks);

                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Student Not Found!");
            }
        }

        // DELETE STUDENT
        static void DeleteStudent()
        {
            Console.Write("\nEnter Roll Number to Delete: ");

            if (!int.TryParse(Console.ReadLine(), out int rollNo))
            {
                Console.WriteLine("Invalid Roll Number!");
                return;
            }

            Student? student = students.Find(s => s.RollNo == rollNo);

            if (student != null)
            {
                students.Remove(student);
                SaveRecords();

                Console.WriteLine("Student Deleted Successfully!");
            }
            else
            {
                Console.WriteLine("Student Not Found!");
            }
        }

        // SAVE RECORDS
        static void SaveRecords()
        {
            try
            {
                List<string> records = new List<string>();

                foreach (Student s in students)
                {
                    string record = s.RollNo + "|" +
                                    s.Name + "|" +
                                    s.DOB.ToString("dd-MM-yyyy") + "|" +
                                    s.Marks;

                    records.Add(record);
                }

                File.WriteAllLines("students.txt", records);

                Console.WriteLine("Records Saved Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Saving Records: " + ex.Message);
            }
        }

        // LOAD RECORDS
        static void LoadRecords()
        {
            try
            {
                if (File.Exists("students.txt"))
                {
                    students.Clear();

                    string[] records = File.ReadAllLines("students.txt");

                    foreach (string record in records)
                    {
                        string[] data = record.Split('|');

                        if (data.Length == 4 &&
                            int.TryParse(data[0], out int rollNo) &&
                            DateTime.TryParseExact(
                                data[2],
                                "dd-MM-yyyy",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out DateTime dob) &&
                            int.TryParse(data[3], out int marks) &&
                            marks >= 0 && marks <= 100)
                        {
                            Student s = new Student();

                            s.RollNo = rollNo;
                            s.Name = data[1];
                            s.DOB = dob;
                            s.Marks = marks;

                            students.Add(s);
                        }
                    }

                    Console.WriteLine("Records Loaded Successfully!");
                }
                else
                {
                    Console.WriteLine("No Previous Records Found!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Loading Records: " + ex.Message);
            }
        }
    }
}