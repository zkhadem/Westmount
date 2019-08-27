using System;
using System.Windows;
using Microsoft.Win32;
using System.Data;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using System.ComponentModel;

namespace Westmount
{
    public partial class MainWindow : System.Windows.Window /*INotifyPropertyChanged*/
    {
        //private string _SearchTerm = "";
        //public string SearchTerm
        //{
        //    get { return _SearchTerm; }
        //    set
        //    {
        //        if (_SearchTerm != value)
        //        {
        //            _SearchTerm = value;
        //            NotifyPropertyChanged("SearchTerm");
        //            NotifyPropertyChanged("SearchResults");
        //        }
        //    }
        //}

        ////private DataTable table = new DataTable();
        ////private DataTable _SearchResults;
        ////public DataTable SearchResults
        ////{
        ////    get
        ////    {
        ////        _SearchResults = table.Where((w) => w.Contains(_SearchTerm)).ToDataTable();
        ////        return _SearchResults;
        ////    }
        ////}
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.DefaultExt = ".xlsx";
            openfile.Filter = "(.xlsx)|*.xlsx";


            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                //Static File From Base Path...........
                //Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "TestExcel.xlsx", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                //Dynamic File Using Uploader...........
                Workbook excelBook = excelApp.Workbooks.Open(openfile.FileName.ToString(), 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Worksheet excelSheet = (Worksheet)excelBook.Sheets[1];
                Range excelRange = excelSheet.UsedRange;

                int rowCnt = excelRange.Rows.Count;
                int colCnt = excelRange.Columns.Count;
                string strCellData = "";
                double douCellData;

                DataTable table;

                table = new DataTable();

                for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                {
                    string strColumn = "";
                    strColumn = (string)(excelRange.Cells[1, colCnt] as Range).Value2;
                    table.Columns.Add(strColumn, typeof(string));
                }

                for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
                {
                    string strData = "";
                    for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                    {
                        if ((excelRange.Cells[rowCnt, colCnt] as Range).Value2 is string)
                        {
                            strCellData = (excelRange.Cells[rowCnt, colCnt] as Range).Value2;
                            strData += strCellData + "|";
                        }
                        else if ((excelRange.Cells[rowCnt, colCnt] as Range).Value2 is double)
                        {
                            douCellData = (excelRange.Cells[rowCnt, colCnt] as Range).Value2;
                            strData += douCellData.ToString() + "|";
                        }
                        else
                        {
                            strData += "" + "|";
                        }
                    }
                    strData = strData.Remove(strData.Length - 1, 1);
                    table.Rows.Add(strData.Split('|'));
                }

                dtGrid.ItemsSource = table.DefaultView;
                excelBook.Close(true, null, null);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                Console.ReadLine();
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            DateTime? date = picker.SelectedDate;

            if (date == null)
            {
                this.Title = "No date";
            }
            else
            {
                this.Title = date.Value.ToShortDateString();
            }
        }
        //public event PropertyChangedEventHandler PropertyChanged;
        //private void NotifyPropertyChanged(string propertyName)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}