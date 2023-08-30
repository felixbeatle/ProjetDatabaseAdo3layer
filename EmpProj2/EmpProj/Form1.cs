using System;
using System.Collections.Generic;
using System.Data;

using System.Windows.Forms;

namespace EmpProj2
{
    public partial class Form1 : Form
    {
        internal enum Grids
        {
           Students,
              programs,
                enrollements,
                courses
        }

        internal static Form1 current;

        private Grids grid;

        public Form1()
        {
            current = this;
            InitializeComponent();           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new Form2();
            Form2.current.Visible = false;
            
            dataGridView1.Dock = DockStyle.Fill;
        }

        private void StudentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.Students;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource1.DataSource = Data.Students.GetStudent();
            bindingSource1.Sort = "StId";
            dataGridView1.DataSource = bindingSource1;

            dataGridView1.Columns["StName"].HeaderText = "name";
            dataGridView1.Columns["StId"].DisplayIndex = 0;
            dataGridView1.Columns["ProgId"].DisplayIndex = 1;

        }

        private void ProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.programs;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataTable programsData = Data.Programs.GetPrograms();

            if (programsData != null && programsData.Rows.Count > 0)
            {
                bindingSource2 = new BindingSource(programsData, null);
                bindingSource2.Sort = "ProgId";
                dataGridView1.DataSource = bindingSource2;

                dataGridView1.Columns["ProgId"].DisplayIndex = 0;
                dataGridView1.Columns["ProgName"].DisplayIndex = 1;
            }
            else
            {
                MessageBox.Show("No data found for Programs.");
            }
        }

        private void EnrollementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.enrollements;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Enabled = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;



            DataTable enrollmentsData = Data.Enrollments.GetEnrollmentsWithNames();

            if (enrollmentsData != null && enrollmentsData.Rows.Count > 0)
            {
                bindingSource4 = new BindingSource(enrollmentsData, null);

                dataGridView1.DataSource = bindingSource4;

                dataGridView1.Columns["StId"].HeaderText = "Student ID";
                dataGridView1.Columns["StName"].HeaderText = "Student Name";
                dataGridView1.Columns["CId"].HeaderText = "Course ID";
                dataGridView1.Columns["CName"].HeaderText = "Course Name";
                dataGridView1.Columns["ProgId"].HeaderText = "Program ID";
                dataGridView1.Columns["ProgName"].HeaderText = "Program Name";
                dataGridView1.Columns["FinalGrade"].HeaderText = "Final Grade";
                

                dataGridView1.Columns["StId"].ReadOnly = true;
                dataGridView1.Columns["CId"].ReadOnly = true;
                dataGridView1.Columns["ProgId"].ReadOnly = true;
                dataGridView1.Columns["StName"].ReadOnly = true;
            }
            else
            {
                MessageBox.Show("No data found for Enrollments.");
            }
        }
        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            Business.student.UpdateSTudents();
        }

        private void bindingSource2_CurrentChanged(object sender, EventArgs e)
        {
            Business.Programs.UpdatePrograms();
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            
            Business.student.UpdateSTudents();
            Business.Programs.UpdatePrograms();
            Business.Courses.UpdateCourses();
            Business.Enrollements.Updateenrollement();
        }

        internal static void BLLMessage(string s)
        {
            MessageBox.Show("Business Layer: " + s);
        }

        internal static void DALMessage(string s)
        {
            MessageBox.Show("Data Layer: " + s);
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Start(Form2.Modes.INSERT, null);
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("A line must be selected for update");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected for update");
            }
            else
            {
                Form2.current.Start(Form2.Modes.UPDATE, c);
            }
        }

        private void evaluationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("A line must be selected for evaluation update");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected for update");
            }
            else
            {
                Form2.current.Start(Form2.Modes.EVALUATION, c);
            }
        }

        private void RefreshDataGridView()
        {
            switch (grid)
            {
                case Grids.Students:
                    bindingSource1.DataSource = Data.Students.GetStudent();
                    break;
                case Grids.programs:
                    DataTable programsData = Data.Programs.GetPrograms();
                    if (programsData != null && programsData.Rows.Count > 0)
                    {
                        bindingSource2 = new BindingSource(programsData, null);
                        dataGridView1.DataSource = bindingSource2;
                    }
                    else
                    {
                        MessageBox.Show("No data found for Programs.");
                    }
                    break;
                case Grids.enrollements:
                    DataTable enrollmentsData = Data.Enrollments.GetEnrollmentsWithNames();
                    if (enrollmentsData != null && enrollmentsData.Rows.Count > 0)
                    {
                        bindingSource4 = new BindingSource(enrollmentsData, null);
                        dataGridView1.DataSource = bindingSource4;
                    }
                    else
                    {
                        MessageBox.Show("No data found for Enrollments.");
                    }
                    break;
                case Grids.courses:
                    bindingSource3 = new BindingSource(Data.Courses.Getcourses(), null);
                    dataGridView1.DataSource = bindingSource3;
                    break;
                default:
                    break;
            }
        }


        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Impossible to insert / update");
            e.Cancel = false;  
        }
        private void Form2_InsertCompleted(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }


        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.courses;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource3 = new BindingSource(Data.Courses.Getcourses(), null);
            bindingSource3.Sort = "CId";
            dataGridView1.DataSource = bindingSource3;

            dataGridView1.Columns["CId"].DisplayIndex = 0;
            dataGridView1.Columns["CName"].DisplayIndex = 1;
            dataGridView1.Columns["ProgId"].DisplayIndex = 2;

        }

        private void deleteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dataGridView1.SelectedRows;

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("At least one line must be selected for deletion.");
                return;
            }

            List<string[]> rowsToDelete = new List<string[]>();

            foreach (DataGridViewRow selectedRow in selectedRows)
            {
                string stId = selectedRow.Cells["StId"].Value.ToString();
                string cId = selectedRow.Cells["CId"].Value.ToString();
                rowsToDelete.Add(new string[] { stId, cId });
            }

            int result = Data.Enrollments.DeleteData(rowsToDelete);

            if (result >= 0)
            {
                EmpProj2.Form1.DALMessage("Successfully deleted.");
            }
            else
            {
                EmpProj2.Form1.DALMessage("Deletion error.");
            }
        }

    }
}
