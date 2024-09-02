using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StudentManagementSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string StudentsFile = "students.txt";

        void ClearInputFields()
        {
            txtStudentID.Text = string.Empty;
            txtName.Text = string.Empty;
            txtAge.Text = string.Empty;
            txtMajor.Text = string.Empty;
        }

        void AddNewStudentToFile(string StudentRecord)
        {
            File.AppendAllText(StudentsFile, StudentRecord + Environment.NewLine);
        }
        void UpdateFile()
        {
            List <string> lines = new List<string>();

            foreach (ListViewItem item in listView_Students.Items)
            {
                lines.Add($"{item.Text},{item.SubItems[1].Text},{item.SubItems[2].Text},{item.SubItems[3].Text}");
            }
            File.WriteAllLines(StudentsFile,lines);

        }
        void LoadStudentsRecordsFromFileToListView()
        {
            if (File.Exists(StudentsFile))
            {
                string[] lines = File.ReadAllLines(StudentsFile);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    ListViewItem item = new ListViewItem(parts[0]);
                    item.SubItems.Add(parts[1]);
                    item.SubItems.Add(parts[2]);
                    item.SubItems.Add(parts[3]);
                    item.ImageIndex = 0;

                    listView_Students.Items.Add(item);
                }
            }
        }

        bool IsStudentWithThisIDExist(string StudentID)
        {
            foreach(ListViewItem item in listView_Students.Items)
            {
                if (item.Text == StudentID)
                    return true;
            }
            return false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string StudentId = txtStudentID.Text;
            string Name = txtName.Text;
            string Age = txtAge.Text;
            string Major = txtMajor.Text;
            
            if (string.IsNullOrEmpty(StudentId) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Age) || string.IsNullOrEmpty(Major))
            {
                MessageBox.Show("All Fields are Required!","Alert", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if (IsStudentWithThisIDExist(StudentId))
            {
                MessageBox.Show("Student ID Already Exists!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //create list view item
            ListViewItem item = new ListViewItem(StudentId);
            item.SubItems.Add(Name);
            item.SubItems.Add(Age);
            item.SubItems.Add(Major);
            item.ImageIndex = 0;

            // add to listView
            listView_Students.Items.Add(item);

            // Save to file 
            AddNewStudentToFile($"{StudentId},{Name},{Age},{Major}");

            ClearInputFields();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtStudentID.Text) || string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtAge.Text) || string.IsNullOrEmpty(txtMajor.Text))
            {
                MessageBox.Show("All Fields are Required!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (listView_Students.Items.Count < 0)
            {
                MessageBox.Show("Please Select a Student to Update", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //change the record of the selected index
            ListViewItem SelectedItem = listView_Students.Items[0];
            SelectedItem.SubItems[0].Text = txtStudentID.Text;
            SelectedItem.SubItems[1].Text = txtName.Text;
            SelectedItem.SubItems[2].Text = txtAge.Text;
            SelectedItem.SubItems[3].Text = txtMajor.Text;

            UpdateFile();
            ClearInputFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView_Students.Items.Count < 0)
            {
                MessageBox.Show("Please Select a Student to Delete", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            listView_Students.Items.Remove(listView_Students.SelectedItems[0]);
            UpdateFile();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchByID.Text))
            {
                MessageBox.Show("Please Enter a Student ID to Search", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (ListViewItem item in listView_Students.Items)
            {
                if (item.Text == txtSearchByID.Text)
                {
                    string record = $"{item.SubItems[0].Text},{item.SubItems[1].Text},{item.SubItems[2].Text},{item.SubItems[3].Text}";
                    MessageBox.Show($"Student Found!\n\nThe Record: \n[{record}]", "Student Found!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            MessageBox.Show("Student Not Found!", "Not Found!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtStudentID.Text = string.Empty;
            txtName.Text = string.Empty;
            txtAge.Text = string.Empty;
            txtMajor.Text = string.Empty;
            txtSearchByID.Text = string.Empty;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //load records from file when the form loads
            LoadStudentsRecordsFromFileToListView();
        }

        private void rbDetails_CheckedChanged(object sender, EventArgs e)
        {
            listView_Students.View = View.Details;
        }

        private void rbLargeIcon_CheckedChanged(object sender, EventArgs e)
        {
            listView_Students.View = View.LargeIcon;
        }

        private void rbSmallIcon_CheckedChanged(object sender, EventArgs e)
        {
            listView_Students.View = View.SmallIcon;
        }

        private void rbList_CheckedChanged(object sender, EventArgs e)
        {
            listView_Students.View = View.List;
        }

        private void rbTile_CheckedChanged(object sender, EventArgs e)
        {
            listView_Students.View = View.Tile;
        }
    }
}
