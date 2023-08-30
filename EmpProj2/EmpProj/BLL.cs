
using System.Data;

namespace Business
{
    class student
    {
        internal static int UpdateSTudents()
        {
            DataSet ds = Data.DataTables.getDataSet();

            DataTable dt = ds.Tables["Students"]
                              .GetChanges(DataRowState.Added | DataRowState.Modified);


            return Data.Students.UpdateStudents();

        }
    }

    class Programs
    {
        internal static int UpdatePrograms()
        {
            DataSet ds = Data.DataTables.getDataSet();

            DataTable dt = ds.Tables["Programs"]
                              .GetChanges(DataRowState.Added | DataRowState.Modified);

            return Data.Programs.UpdatePrograms();

        }
    }

    class Courses
    {

        internal static int UpdateCourses()
        {
            DataSet ds = Data.DataTables.getDataSet();

            DataTable dt = ds.Tables["Courses"]
                              .GetChanges(DataRowState.Added | DataRowState.Modified);

            return Data.Courses.UpdateCourses();

        }

    }
    class Enrollements
    {

        internal static int Updateenrollement()
        {
            DataSet ds = Data.DataTables.getDataSet();

            DataTable dt = ds.Tables["Enrollments"]
                              .GetChanges(DataRowState.Added | DataRowState.Modified);

            return Data.Enrollments.UpdateEnrollments();

        }

    }
}
