using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace EmpProj2
{
    public partial class Form2 : Form
    {

        internal enum Modes
        {
            INSERT,
            UPDATE,
            EVALUATION
        }

        internal static Form2 current;

        private Modes mode = Modes.INSERT;

        private string[] EnrollementInitial; 

        public Form2()
        {
            current = this;
            InitializeComponent();
        }

        internal void Start(Modes m, DataGridViewSelectedRowCollection c)
        {
            mode = m;
            Text = "" + mode;

            comboBox1.DisplayMember = "StId";
            comboBox1.ValueMember = "StId";
            comboBox1.DataSource = Data.Students.GetStudent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;
            comboBox1.Enabled = true;

            comboBox2.DisplayMember = "CId";
            comboBox2.ValueMember = "CId";
            comboBox2.DataSource = Data.Courses.Getcourses();
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedIndex = 0;
            comboBox2.Enabled = true;

            if (((mode == Modes.UPDATE) || (mode == Modes.EVALUATION)) && (c != null))
            {
                comboBox1.SelectedValue = c[0].Cells["StId"].Value;
                comboBox2.SelectedValue = c[0].Cells["CId"].Value;

                EnrollementInitial = new string[] { c[0].Cells["StId"].Value.ToString(), c[0].Cells["CId"].Value.ToString() };
            }
            if (mode == Modes.UPDATE)
            {
                comboBox1.Enabled = false;
                comboBox2.Enabled = true;
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
            }
            if (mode == Modes.EVALUATION)
            {
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = false;
                string currentFinalGrade = c[0].Cells["FinalGrade"].Value.ToString();
                textBox3.Text = currentFinalGrade;
            }


            ShowDialog();
        }
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                var selectedValue = comboBox1.SelectedValue;
                if (selectedValue != null)
                {
                    var stId = selectedValue;
                    var a = from r in Data.Students.GetStudent().AsEnumerable()
                            where r.Field<string>("StId") == stId
                            select new { Name = r.Field<string>("StName") };
                    textBox1.Text = a.SingleOrDefault()?.Name;
                }
                else
                {
                    textBox1.Text = "";
                }
            }
        }
        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                var selectedValue = comboBox2.SelectedValue;
                if (selectedValue != null)
                {
                    var CiD = selectedValue;
                    var a = from r in Data.Courses.Getcourses().AsEnumerable()
                            where r.Field<string>("CId") == CiD
                            select new { Name = r.Field<string>("CName") };
                    textBox2.Text = a.SingleOrDefault()?.Name;
                }
                else
                {
                    textBox2.Text = "";
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            int r = -1;

            try
            {
                if (mode == Modes.INSERT)
                {
                    string selectedStId = (string)comboBox1.SelectedValue;
                    string selectedCId = (string)comboBox2.SelectedValue;

                    DataRowView selectedStudent = (DataRowView)comboBox1.SelectedItem;
                    DataRowView selectedCourse = (DataRowView)comboBox2.SelectedItem;
                    string studentProgramId = selectedStudent["ProgId"].ToString();
                    string courseProgramId = selectedCourse["ProgId"].ToString();

                    if (studentProgramId != courseProgramId)
                    {
                        MessageBox.Show("Error: The selected student and course do not belong to the same program.");
                        return;
                    }

                    int result = Data.Enrollments.InsertData(new string[] { selectedStId, selectedCId });
                    if (result >= 0)
                    {
                        MessageBox.Show("Insertion successful");
                        Close();
                    }
                   
                
                }
                else if (mode == Modes.UPDATE)
                {
                    string selectedStId = (string)comboBox1.SelectedValue;
                    string selectedCId = (string)comboBox2.SelectedValue;
                    r = Data.Enrollments.UpdateData(new string[] { selectedStId, selectedCId });
                    if (r == 0)
                    {
                        MessageBox.Show("Update successful.");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Error occurred during update: " + r.ToString());
                    }
                }
                else if (mode == Modes.EVALUATION)
                {
                    string selectedStId = (string)comboBox1.SelectedValue;
                    string selectedCId = (string)comboBox2.SelectedValue;

                    string finalGrade = textBox3.Text;

                    r = Data.Enrollments.UpdateEvaluation(new string[] { selectedStId, selectedCId, finalGrade });

                    if (r == 0)
                    {
                        MessageBox.Show("Evaluation update successful.");
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}

