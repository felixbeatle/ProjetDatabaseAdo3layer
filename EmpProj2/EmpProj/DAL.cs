using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace Data
{
    internal class Connect
    {
        private static String cliComConnectionString = GetConnectString();

        internal static String ConnectionString { get => cliComConnectionString; }

        private static String GetConnectString()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = "(local)";
            cs.InitialCatalog = "college1ens";
            cs.UserID = "sa";
            cs.Password = "Lookme1234";
            return cs.ConnectionString;
        }
    }

    internal class DataTables
    {
        private static SqlDataAdapter adapterStudents = InitAdapterstud();
        private static SqlDataAdapter adapterProg = InitAdapterProg();
        private static SqlDataAdapter adapterCourses = InitAdaptercourses();
        private static SqlDataAdapter adapterEnrollement = InitAdapterEnrollement();


        private static DataSet ds = InitDataSet();
       
        private static SqlDataAdapter InitAdapterstud()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Students ORDER BY StId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterProg()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Programs ORDER BY ProgId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }
        private static SqlDataAdapter InitAdaptercourses()
        {
            SqlDataAdapter adapter = new SqlDataAdapter(
                 "SELECT * FROM Courses ORDER BY CId",
                 Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.UpdateCommand = builder.GetUpdateCommand();

            return adapter;
        }
        private static SqlDataAdapter InitAdapterEnrollement()
        {
            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT * FROM Enrollments ORDER BY StId",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.UpdateCommand = builder.GetUpdateCommand();

            return adapter;
        }

       

        private static DataSet InitDataSet()
        {
            DataSet ds = new DataSet();
            loadProg(ds);
            loadStud(ds);
            loadCourses(ds);
            LoadEnrollements(ds);

            return ds;
        }

        private static void loadStud(DataSet ds)
        {


            adapterStudents.Fill(ds, "Students");

            ds.Tables["Students"].Columns["StId"].AllowDBNull = true;
            ds.Tables["Students"].Columns["StName"].AllowDBNull = true;

            ds.Tables["Students"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Students"].Columns["StId"]};
        }
        private static void loadProg(DataSet ds)
        {
            adapterProg.Fill(ds, "Programs");

            ds.Tables["Programs"].Columns["ProgId"].AllowDBNull = false;
            ds.Tables["Programs"].Columns["ProgName"].AllowDBNull = false;

            ds.Tables["Programs"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Programs"].Columns["ProgId"]};
        }
        private static void loadCourses(DataSet ds)
        {
            adapterCourses.Fill(ds, "Courses");

            ds.Tables["Courses"].Columns["CId"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["CName"].AllowDBNull = false;

            ds.Tables["Courses"].Columns["ProgId"].AllowDBNull = false;

            ds.Tables["Courses"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Courses"].Columns["CId"] };

            
        }
        private static void LoadEnrollements(DataSet ds)
        {
            adapterEnrollement.Fill(ds, "Enrollments");

            ds.Tables["Enrollments"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Enrollments"].Columns["CId"].AllowDBNull = false;

            ds.Tables["Enrollments"].Columns["FinalGrade"].AllowDBNull = true;

            ds.Tables["Enrollments"].PrimaryKey = new DataColumn[]
            {
        ds.Tables["Enrollments"].Columns["StId"],
        ds.Tables["Enrollments"].Columns["CId"]
            };

            ForeignKeyConstraint myFK03 = new ForeignKeyConstraint("MyFK03",
                new DataColumn[]{
            ds.Tables["Students"].Columns["StId"]
                },
                new DataColumn[] {
            ds.Tables["Enrollments"].Columns["StId"],
                }
            );
            myFK03.DeleteRule = Rule.Cascade;
            myFK03.UpdateRule = Rule.Cascade;
            ds.Tables["Enrollments"].Constraints.Add(myFK03);

            ForeignKeyConstraint myFK04 = new ForeignKeyConstraint("MyFK04",
              new DataColumn[]{
            ds.Tables["Courses"].Columns["CId"]
              },
              new DataColumn[] {
            ds.Tables["Enrollments"].Columns["CId"],
              }
            );
            myFK04.DeleteRule = Rule.None;
            myFK04.UpdateRule = Rule.None;
            ds.Tables["Enrollments"].Constraints.Add(myFK04);

        }

        internal static SqlDataAdapter getAdapterStudents
            ()
        {
            return adapterStudents;
        }
        internal static SqlDataAdapter getAdapterProg
            ()
        {
            return adapterProg;
        }
        internal static SqlDataAdapter getAdapterCourses()
        {
            return adapterCourses;
        }
        internal static SqlDataAdapter getAdapterEnrollements()
        {
            return adapterEnrollement;
        }
        internal static DataSet getDataSet()
        {
            return ds;
        }
    }

    internal class Students
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterStudents();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetStudent()
        {
            return ds.Tables["Students"];
        }

        internal static int UpdateStudents()
        {
            if (!ds.Tables["Students"].HasErrors)
            {
                return adapter.Update(ds.Tables["Students"]);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Programs
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterProg();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetPrograms()
        {
            return ds.Tables["Programs"];
        }

        internal static int UpdatePrograms()
        {
            if (!ds.Tables["Programs"].HasErrors)
            {
                return adapter.Update(ds.Tables["Programs"]);
            }
            else
            {
                return -1;
            }
        }
    }
    internal class Courses
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterCourses(); 
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable Getcourses()
        {
            return ds.Tables["Courses"];
        }

        internal static int UpdateCourses()
        {
            if (!ds.Tables["Courses"].HasErrors)
            {
                return adapter.Update(ds.Tables["Courses"]);
            }
            else
            {
                return -1;
            }
        }
    }
    internal class Enrollments
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterEnrollements();
        private static DataSet ds = DataTables.getDataSet();

        internal static DataTable GetEnrollments()
        {
            return ds.Tables["Enrollments"];
        }

        internal static DataTable GetEnrollmentsWithNames()
        {
            DataTable enrollments = ds.Tables["Enrollments"];
            DataTable students = ds.Tables["Students"];
            DataTable courses = ds.Tables["Courses"];
            DataTable Programs = ds.Tables["Programs"];
            var query = from enrollment in enrollments.AsEnumerable()
                        join student in students.AsEnumerable()
                        on enrollment.Field<string>("StId") equals student.Field<string>("StId")
                        join course in courses.AsEnumerable()
                        on enrollment.Field<string>("CId") equals course.Field<string>("CId")
                        join program in Programs.AsEnumerable()
                        on course.Field<string>("ProgId") equals program.Field<string>("ProgId")

                        select new
                        {
                            StId = enrollment.Field<string>("StId"),
                            StName = student.Field<string>("StName"),
                            CId = enrollment.Field<string>("CId"),
                            CName = course.Field<string>("CName"),
                            ProgId = program.Field<string>("ProgId"),
                            ProgName = program.Field<string>("ProgName"),
                            FinalGrade = enrollment.Field<Nullable<int>>("FinalGrade")
                        };

            DataTable result = new DataTable();
            result.Columns.Add("StId");
            result.Columns.Add("StName");
            result.Columns.Add("CId");
            result.Columns.Add("CName");
            result.Columns.Add("ProgId");
            result.Columns.Add("ProgName");
            result.Columns.Add("FinalGrade", typeof(int));

            foreach (var enrollment in query)
            {
                result.Rows.Add(enrollment.StId, enrollment.StName, enrollment.CId, enrollment.CName,enrollment.ProgId,enrollment.ProgName,
                               enrollment.FinalGrade ?? 0);
            }

            return result;
        }
        internal static int UpdateEnrollments()
        {
            if (!ds.Tables["Enrollments"].HasErrors)
            {
                return adapter.Update(ds.Tables["Enrollments"]);
            }
            else
            {
                return -1;
            }
        }

        internal static int InsertData(string[] a)
        {
            try
            {
                DataRow newRow = ds.Tables["Enrollments"].NewRow();
                newRow.SetField("StId", a[0]);
                newRow.SetField("CId", a[1]);
                ds.Tables["Enrollments"].Rows.Add(newRow);

                adapter.Update(ds.Tables["Enrollments"]);

                return 0;
            }
            catch (Exception)
            {
                EmpProj2.Form1.DALMessage("Insertion / Update rejected");
                return -1;
            }
        }


        internal static int UpdateData(string[] a)
        {
            
            try
            {
                DataRow enrollRow = ds.Tables["Enrollments"].AsEnumerable()
                    .SingleOrDefault(row =>
                        row.Field<string>("StId") == a[0]);

                if (enrollRow != null)
                {
                    enrollRow["CId"] = a[1];

                    adapter.Update(ds, "Enrollments");

                  

                    return 0;
                }
                else
                {
                    EmpProj2.Form1.DALMessage("Enrollment not found");
                    return -1;
                }
            }
            catch (Exception)
            {
                EmpProj2.Form1.DALMessage("Update rejected");
                return -1;
            }
        }

        internal static int DeleteData(List<string[]> lIds)
        {
            try
            {
                var enrollmentsToDelete = ds.Tables["Enrollments"].AsEnumerable()
                    .Where(row => lIds.Any(ids => row.Field<string>("StId") == ids[0] && row.Field<string>("CId") == ids[1]));

                int nonNullFinalGradesCount = enrollmentsToDelete.Count(row => !row.IsNull("FinalGrade"));

                if (nonNullFinalGradesCount > 0)
                {
                    EmpProj2.Form1.DALMessage("Final grade is already assigned. Cannot delete the enrollment.");
                    return -1;
                }
                else
                {
                    foreach (var enrollRow in enrollmentsToDelete.ToList())
                    {
                        enrollRow.Delete();
                    }
                }

                int rowsAffected = adapter.Update(ds.Tables["Enrollments"]);

                if (rowsAffected < enrollmentsToDelete.Count())
                {
                    EmpProj2.Form1.DALMessage("Not all deletions were successful.");
                    return -1;
                }

                return 0;
            }
            catch (Exception ex)
            {
                EmpProj2.Form1.DALMessage("Deletion error: " + ex.Message);
                return -1;
            }
        }




        internal static int UpdateEvaluation(string[] data)
        {
            try
            {
                DataSet ds = Data.DataTables.getDataSet();
                DataTable dt = ds.Tables["Enrollments"];

                DataRow enrollRow = dt.AsEnumerable()
                    .SingleOrDefault(row =>
                        row.Field<string>("StId") == data[0] &&
                        row.Field<string>("CId") == data[1]);

                if (enrollRow != null)
                {
                    string finalGradeStr = data[2];

                    if (!string.IsNullOrEmpty(finalGradeStr) &&
                        (!int.TryParse(finalGradeStr, out int finalGrade) || finalGrade < 0 || finalGrade > 100))
                    {
                        EmpProj2.Form1.BLLMessage("Invalid final grade. Please enter a valid integer between 0 and 100 or leave it empty for no grade.");
                        return -1;
                    }

                    enrollRow["FinalGrade"] = data[2];

                    adapter.Update(ds.Tables["Enrollments"]);

                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
