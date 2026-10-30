namespace Session06_task
{

    using System;
    using System.Collections.Generic;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Program
    {
        class Student
        {
            public int StudentId { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public List<Course> Courses { get; set; } = new List<Course>();

            public Student(int id, string name, int age)
            {
                StudentId = id;
                Name = name;
                Age = age;
            }

            public bool Enroll(Course course)
            {
                if (Courses.Any(c => c.CourseId == course.CourseId))
                    return false;

                Courses.Add(course);
                return true;
            }

            public string PrintDetails()
            {
                string names = Courses.Count == 0
                    ? "No courses"
                    : string.Join(", ", Courses.Select(c => c.Title));

                return $"ID: {StudentId} | Name: {Name} | Age: {Age} | Courses: {names}";
            }
        }

        class Instructor
        {
            public int InstructorId { get; set; }
            public string Name { get; set; }
            public string Specialization { get; set; }

            public Instructor(int id, string name, string specialization)
            {
                InstructorId = id;
                Name = name;
                Specialization = specialization;
            }

            public string PrintDetails()
            {
                return $"ID: {InstructorId} | Name: {Name} | Specialization: {Specialization}";
            }
        }

        class Course
        {
            public int CourseId { get; set; }
            public string Title { get; set; }
            public Instructor Instructor { get; set; }

            public Course(int id, string title, Instructor instructor)
            {
                CourseId = id;
                Title = title;
                Instructor = instructor;
            }

            public string PrintDetails()
            {
                return $"ID: {CourseId} | Title: {Title} | Instructor: {Instructor.Name}";
            }
        }

        class StudentManager
        {
            public List<Student> Students { get; set; } = new List<Student>();
            public List<Course> Courses { get; set; } = new List<Course>();
            public List<Instructor> Instructors { get; set; } = new List<Instructor>();

            public Student FindStudent(int id) =>
                Students.FirstOrDefault(s => s.StudentId == id);

            public Course FindCourse(int id) =>
                Courses.FirstOrDefault(c => c.CourseId == id);

            public Instructor FindInstructor(int id) =>
                Instructors.FirstOrDefault(i => i.InstructorId == id);

            public bool AddStudent(Student s)
            {
                if (FindStudent(s.StudentId) != null) return false;
                Students.Add(s);
                return true;
            }

            public bool AddInstructor(Instructor i)
            {
                if (FindInstructor(i.InstructorId) != null) return false;
                Instructors.Add(i);
                return true;
            }

            public bool AddCourse(Course c)
            {
                if (FindCourse(c.CourseId) != null ||
                    FindInstructor(c.Instructor.InstructorId) == null)
                    return false;

                Courses.Add(c);
                return true;
            }

            public bool EnrollStudentInCourse(int studentId, int courseId)
            {
                Student s = FindStudent(studentId);
                Course c = FindCourse(courseId);

                if (s == null || c == null) return false;
                return s.Enroll(c);
            }

            public bool UpdateStudent(int id, string name, int age)
            {
                Student s = FindStudent(id);
                if (s == null) return false;

                s.Name = name;
                s.Age = age;
                return true;
            }

            public bool DeleteStudent(int id)
            {
                Student s = FindStudent(id);
                if (s == null) return false;

                Students.Remove(s);
                return true;
            }

            public bool IsStudentEnrolled(int studentId, int courseId)
            {
                Student s = FindStudent(studentId);
                return s != null && s.Courses.Any(c => c.CourseId == courseId);
            }

            public Instructor FindInstructorByCourseName(string title)
            {
                Course c = Courses.FirstOrDefault(
                    c => c.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

                return c?.Instructor;
            }

            public Student FindStudentByName(string name) =>
                Students.FirstOrDefault(s =>
                    s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            public Course FindCourseByName(string title) =>
                Courses.FirstOrDefault(c =>
                    c.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            public void ShowStudents()
            {
                if (Students.Count == 0) Console.WriteLine("No students found.");
                foreach (Student s in Students) Console.WriteLine(s.PrintDetails());
            }

            public void ShowCourses()
            {
                if (Courses.Count == 0) Console.WriteLine("No courses found.");
                foreach (Course c in Courses) Console.WriteLine(c.PrintDetails());
            }

            public void ShowInstructors()
            {
                if (Instructors.Count == 0) Console.WriteLine("No instructors found.");
                foreach (Instructor i in Instructors) Console.WriteLine(i.PrintDetails());
            }
        }

        // Read a number from the user
        static int ReadInt(string message)
        {
            int number;
            Console.Write(message);

            while (!int.TryParse(Console.ReadLine(), out number))
                Console.Write("Enter a valid number: ");

            return number;
        }

        static string ReadText(string message)
        {
            Console.Write(message);
            return Console.ReadLine() ?? "";
        }

        static void Main()
        {
            StudentManager school = new StudentManager();

            while (true)
            {
                Console.WriteLine("\n=== Student Management System ===");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Add Instructor");
                Console.WriteLine("3. Add Course");
                Console.WriteLine("4. Enroll Student");
                Console.WriteLine("5. Show Students");
                Console.WriteLine("6. Show Courses");
                Console.WriteLine("7. Show Instructors");
                Console.WriteLine("8. Find Student");
                Console.WriteLine("9. Find Course");
                Console.WriteLine("10. Exit");
                Console.WriteLine("11. Check Enrollment");
                Console.WriteLine("12. Find Instructor by Course");
                Console.WriteLine("13. Update Student");
                Console.WriteLine("14. Delete Student");

                int choice = ReadInt("Choose an option: ");

                switch (choice)
                {
                    case 1:
                        int sid = ReadInt("Student ID: ");
                        string sname = ReadText("Name: ");
                        int age = ReadInt("Age: ");

                        if (age <= 0)
                            Console.WriteLine("Invalid age.");
                        else if (school.AddStudent(new Student(sid, sname, age)))
                            Console.WriteLine("Student added successfully.");
                        else
                            Console.WriteLine("Student ID already exists.");
                        break;

                    case 2:
                        int iid = ReadInt("Instructor ID: ");
                        string iname = ReadText("Name: ");
                        string spec = ReadText("Specialization: ");

                        Console.WriteLine(school.AddInstructor(new Instructor(iid, iname, spec))
                            ? "Instructor added successfully."
                            : "Instructor ID already exists.");
                        break;

                    case 3:
                        int cid = ReadInt("Course ID: ");
                        string title = ReadText("Course title: ");
                        school.ShowInstructors();

                        int teacherId = ReadInt("Instructor ID: ");
                        Instructor teacher = school.FindInstructor(teacherId);

                        if (teacher == null)
                            Console.WriteLine("Instructor not found.");
                        else
                            Console.WriteLine(school.AddCourse(new Course(cid, title, teacher))
                                ? "Course added successfully."
                                : "Course ID already exists.");
                        break;

                    case 4:
                        int studentId = ReadInt("Student ID: ");
                        int courseId = ReadInt("Course ID: ");

                        Console.WriteLine(school.EnrollStudentInCourse(studentId, courseId)
                            ? "Student enrolled successfully."
                            : "Enrollment failed. Check IDs or existing enrollment.");
                        break;

                    case 5:
                        school.ShowStudents();
                        break;

                    case 6:
                        school.ShowCourses();
                        break;

                    case 7:
                        school.ShowInstructors();
                        break;

                    case 8:
                        string studentSearch = ReadText("Search by (1) ID or (2) Name: ");
                        Student foundStudent = studentSearch == "1"
                            ? school.FindStudent(ReadInt("Student ID: "))
                            : studentSearch == "2"
                                ? school.FindStudentByName(ReadText("Student name: "))
                                : null;

                        Console.WriteLine(foundStudent?.PrintDetails() ?? "Student not found.");
                        break;

                    case 9:
                        string courseSearch = ReadText("Search by (1) ID or (2) Name: ");
                        Course foundCourse = courseSearch == "1"
                            ? school.FindCourse(ReadInt("Course ID: "))
                            : courseSearch == "2"
                                ? school.FindCourseByName(ReadText("Course title: "))
                                : null;

                        Console.WriteLine(foundCourse?.PrintDetails() ?? "Course not found.");
                        break;

                    case 10:
                        Console.WriteLine("Goodbye!");
                        return;

                    case 11:
                        int checkSid = ReadInt("Student ID: ");
                        int checkCid = ReadInt("Course ID: ");

                        Console.WriteLine(school.IsStudentEnrolled(checkSid, checkCid)
                            ? "The student is enrolled."
                            : "The student is not enrolled.");
                        break;

                    case 12:
                        string searchTitle = ReadText("Course title: ");
                        Instructor foundInstructor =
                            school.FindInstructorByCourseName(searchTitle);

                        Console.WriteLine(foundInstructor?.Name ?? "Course or instructor not found.");
                        break;

                    case 13:
                        int updateId = ReadInt("Student ID to update: ");
                        string newName = ReadText("New name: ");
                        int newAge = ReadInt("New age: ");

                        if (newAge <= 0)
                            Console.WriteLine("Invalid age.");
                        else
                            Console.WriteLine(school.UpdateStudent(updateId, newName, newAge)
                                ? "Student updated successfully."
                                : "Student not found.");
                        break;

                    case 14:
                        int deleteId = ReadInt("Student ID to delete: ");

                        Console.WriteLine(school.DeleteStudent(deleteId)
                            ? "Student deleted successfully."
                            : "Student not found.");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }
    }
}
