﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;

namespace MarkSheetCreator
{
    public partial class Form1 : Form
    {

        private static string _dataTableFilePath;
        private static string _marksheettemplatefilepath;
        private static string _completedMarksheetsfilepath;
        private static string publicCompletedMarksheetSaveLocation;
        private static List<object> fullDataTableList;
        private static CheckBox publicMarkSheetNameFieldCheck;

        public static List<int> ListofChosenValues = new List<int>();
        public static List<string> ListofChosenValuesText = new List<string>();
        public static List<string> ListofChosenCells = new List<string>();
        public static List<string> ListofCheckedBoxes = new List<string>();
        public static StudentDataSheet PublicDataSheetForCopying = new StudentDataSheet();

        public static CheckBox PublicMarkSheetNameFieldCheck
        {
            get
            {
                return publicMarkSheetNameFieldCheck;
            }
            set
            {
                publicMarkSheetNameFieldCheck = value;
            }
        }
        public static string PublicCompletedMarksheetSaveLocation
        {
            get
            {
                return publicCompletedMarksheetSaveLocation;
            }
            set
            {
                publicCompletedMarksheetSaveLocation = value;
            }
        }
        public static List<object> PublicFullDataTableList
        {
            get
            {
                return fullDataTableList;
            }
            set
            {
                fullDataTableList = value;
            }
        }
        public static string PublicCompletedMarkSheetsFilepath
        {
            get
            {
                return _completedMarksheetsfilepath;
            }
            set
            {
                _completedMarksheetsfilepath = value;
            }
        }
        public static string PublicMarkSheetTemplateFilepath
        {
            get
            {
                return _marksheettemplatefilepath;
            }
            set
            {
                _marksheettemplatefilepath = value;
            }
        }
        public static string PublicDataTableFilePath
        {
            get
            {
                return _dataTableFilePath;
            }
            set
            {
                _dataTableFilePath = value;
            }
        }
        public MarksheetTemplate StudentWorkingMarkSheetTemplate { get; set; }
        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        public void BrowseDataTable_Click(object sender, EventArgs e)
        {
            string dataTableContent = string.Empty;
            string dataTablefilePath = string.Empty;
            
            
            comboBox1.Leave += new System.EventHandler(comboBox1_leave);
            textBox1.Leave += new System.EventHandler(textBox1_Leave);
            checkBox1.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);

            using (OpenFileDialog GetStudentDataTable = new OpenFileDialog())
            {
                 GetStudentDataTable.Title = "Please choose the student data table";
                 GetStudentDataTable.InitialDirectory = "c:\\";
                 GetStudentDataTable.Filter = "Excel files(*.xls;*.xlsx)|*.xls;*.xlsx";
                 GetStudentDataTable.FilterIndex = 2;
                 GetStudentDataTable.RestoreDirectory = true;
                 if (GetStudentDataTable.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                 {
                     dataTablefilePath = GetStudentDataTable.FileName;
                     label2.Text = GetStudentDataTable.FileName;
                 }    
            }
            _dataTableFilePath = dataTablefilePath;
            PublicDataSheetForCopying.ExcelStudentDataSheet();
            comboBox1.DataSource = StudentDataSheet.ListofDataSheetValues;
        }
        private void BrowseMarkSheetTemplate_Click(object sender, EventArgs e)
        {
            var MarkSheetTemplateContent = string.Empty;
            var MarkSheetTemplatefilePath = string.Empty;
            
            
            using (OpenFileDialog MarkSheetTemplate = new OpenFileDialog())
            {
                MarkSheetTemplate.Title = "Please choose the student data table";
                MarkSheetTemplate.InitialDirectory = "c:\\";
                MarkSheetTemplate.Filter = "Excel files(*.xls;*.xlsx)|*.xls;*.xlsx";
                MarkSheetTemplate.FilterIndex = 2;
                MarkSheetTemplate.RestoreDirectory = true;
                if (MarkSheetTemplate.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MarkSheetTemplatefilePath = MarkSheetTemplate.FileName;
                    label4.Text = MarkSheetTemplate.FileName;
                }
            }
            _marksheettemplatefilepath = MarkSheetTemplatefilePath;
            StudentWorkingMarkSheetTemplate = new MarksheetTemplate();
            StudentWorkingMarkSheetTemplate.ExcelMarkSheetTemplate();
        }
        private void Confirmation_Click(object sender, EventArgs e)
        {
            
            
            MarksheetGenerator MarkSheetConstructor = new MarksheetGenerator();
            MarkSheetConstructor.GetChosenColumnsAndData();
            MarkSheetConstructor.CloseExcel();
        }
        public void button5_Click(object sender, EventArgs e)
        {
            int comboboxnameint = 0;
            //Combo Box creation
            ComboBox ComboBoxOnClick = new ComboBox();
            comboboxnameint += 1;
            ComboBoxOnClick.Name = comboboxnameint.ToString();
            ComboBoxOnClick.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxOnClick.Text = "DropDown" + comboboxnameint.ToString();
            ComboBoxOnClick.BindingContext = new BindingContext();
            ComboBoxOnClick.DataSource = StudentDataSheet.ListofDataSheetValues;
            ComboBoxOnClick.Size = new System.Drawing.Size(178, 21);
            ComboBoxOnClick.Left = 41;
            int Comboboxtop = 15;
            if (this.Controls.OfType<ComboBox>().Count() > 0)
                Comboboxtop = this.Controls.OfType<ComboBox>().Last().Top + 25;
            ComboBoxOnClick.Top = Comboboxtop;
            this.Controls.Add(ComboBoxOnClick);
            string selectedItem = ComboBoxOnClick.Items[ComboBoxOnClick.SelectedIndex].ToString();
            ComboBoxOnClick.SelectedIndexChanged += new System.EventHandler(ComboBoxOnClick_SelectedIndexChanged);
            //Combo Box creation end

            //Name field check box creation
            /*
            CheckBox marksheetNameFieldCheck = new CheckBox();
            checkboxnameint += 1;
            marksheetNameFieldCheck.Name = "checkBox" + checkboxnameint.ToString();
            string _strCheckBoxName = marksheetNameFieldCheck.Name;
            marksheetNameFieldCheck.BindingContext = new BindingContext();
            marksheetNameFieldCheck.Size = new System.Drawing.Size(15, 14);
            marksheetNameFieldCheck.Left = 234;
            int Checkboxtop = 5;
            if (this.Controls.OfType<CheckBox>().Count() > 0)
                Checkboxtop = this.Controls.OfType<CheckBox>().Last().Top + 25;
            marksheetNameFieldCheck.Top = Checkboxtop;
            this.Controls.Add(marksheetNameFieldCheck);
            marksheetNameFieldCheck.BindingContext = new BindingContext();
            publicMarkSheetNameFieldCheck = marksheetNameFieldCheck;
            marksheetNameFieldCheck.CheckedChanged += new System.EventHandler(marksheetNameFieldCheck_CheckedChanged);
            //Name field check box creation
            */

            //Cell reference textbox creation
            TextBox marksheetCellForField = new TextBox();
            
            marksheetCellForField.Left = 293;
            int Textboxtop = 5;
            if (this.Controls.OfType<TextBox>().Count() > 0)
                Textboxtop = this.Controls.OfType<TextBox>().Last().Top + 25;
            marksheetCellForField.Top = Textboxtop;
            this.Controls.Add(marksheetCellForField);
            marksheetCellForField.TextChanged += new System.EventHandler(marksheetCellForField_TextChanged);
            marksheetCellForField.Leave += new System.EventHandler(marksheetCellForField_Leave);
            //Cell reference textbox creation
        }
        private void comboBox1_leave(object sender, EventArgs e)
        {
            ListofChosenValues.Add(comboBox1.SelectedIndex);
            ListofChosenValuesText.Add(comboBox1.SelectedItem.ToString());
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            TextBox textBox1 = (TextBox)sender;
            ListofChosenCells.Add(textBox1.Text);
        }
        private void marksheetCellForField_Leave(object sender, EventArgs e)
        {
            TextBox marksheetCellForField = (TextBox)sender;
            ListofChosenCells.Add(marksheetCellForField.Text);
        }
        public void ComboBoxOnClick_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboBoxOnClick = (ComboBox)sender;
            ListofChosenValues.Add(ComboBoxOnClick.SelectedIndex);
            ListofChosenValuesText.Add(ComboBoxOnClick.SelectedItem.ToString());
        }
        private void button6_Click(object sender, EventArgs e)
        {
            
            
            var MarkSheetCompletedfilePath = string.Empty;
            using (FolderBrowserDialog SetMarksheetSaveLocation = new FolderBrowserDialog())
            {
                SetMarksheetSaveLocation.RootFolder = Environment.SpecialFolder.Desktop;
                if (SetMarksheetSaveLocation.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MarkSheetCompletedfilePath = SetMarksheetSaveLocation.SelectedPath;
                    publicCompletedMarksheetSaveLocation = MarkSheetCompletedfilePath;
                }
            }
            if (ListofChosenValuesText != null || ListofChosenValuesText != null)
            {
                string NameField = ListofChosenValuesText[0];
                string MarkerField = ListofChosenValuesText[1];
                _completedMarksheetsfilepath = MarkSheetCompletedfilePath;
                string fileName = "[" + NameField + "]" + ", " + "[" + MarkerField + "]" + ".xlsx";
                _completedMarksheetsfilepath = System.IO.Path.Combine(MarkSheetCompletedfilePath, fileName);
                textBox2.Text = _completedMarksheetsfilepath;
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
            
            button1.Enabled = !string.IsNullOrWhiteSpace(textBox2.Text);       
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            
            
            if (StudentDataSheet.ExcelAppDataSheet != null)
            {
                StudentDataSheet.ExcelAppDataSheet.Quit();
            }
            if (MarksheetTemplate.ExcelAppMarksheet != null)
            {
                MarksheetTemplate.ExcelAppMarksheet.Quit();
            }
            this.Close();
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (StudentDataSheet.ExcelAppDataSheet != null)
            { 
                StudentDataSheet.ExcelAppDataSheet.Quit();
            }
            if (MarksheetTemplate.ExcelAppMarksheet != null)
            { 
                MarksheetTemplate.ExcelAppMarksheet.Quit();
            }
            base.OnFormClosed(e);
        }
        private void Form1_FormClosing(object sender, EventArgs e)
        {
            StudentDataSheet.StudentDataWorkbookPublic.Close(0);
            StudentDataSheet.ExcelAppDataSheet.Quit();
            MarksheetTemplate.MarksheetTemplatePublic.Close(0);
            MarksheetTemplate.ExcelAppMarksheet.Quit();
        }
        private void marksheetCellForField_TextChanged(object sender, EventArgs e)
        {

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void marksheetNameFieldCheck_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
            
        }
        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            
        }
        private void comboBox1_DataSheetHeadingList1(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {
            
            
        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        private void DataTablePathName_TextChanged(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click_1(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
