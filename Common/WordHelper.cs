using Microsoft.Office.Interop.Word;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows.Media;

namespace Common
{
    public static class WordHelper
    {
        public static string ExportStudentListInClass(string schoolYear,string className,List<StudyViewModel> students)
        {
            //try
            //{
                //Create an instance for word app  
                Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();

                //Set animation status for word application  
                winword.ShowAnimation = false;

                //Set status for word application is to be visible or not.  
                winword.Visible = false;

               
                //Create a new document  
                Microsoft.Office.Interop.Word.Document document = winword.Documents.Add();

                //adding text to document  
                document.Content.SetRange(0, 0); 
                //Add paragraph with Heading 1 style  
                Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add();
                para1.Range.Text = "LIST OF STUDENT IN "+schoolYear;
                object styleHeading1 = "Heading 1";
                //para1.set_Style("Heading 1");
                para1.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                para1.Range.Font.Name = "Arial";
                para1.Range.Font.Size = 16;
                para1.Range.Font.Bold = 1;
                para1.Range.InsertParagraphAfter();

                //Add paragraph with Heading 2 style  
                Microsoft.Office.Interop.Word.Paragraph para2 = document.Content.Paragraphs.Add();
                para2.Range.Text = "Class: "+className;
                object styleHeading2 = "Heading 2";
                //para2.set_Style("Heading 2");
                para2.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                para2.Range.Font.Name = "Arial";
                para2.Range.Font.Size = 14;
                para2.Range.Bold = 0;
                para2.Range.InsertParagraphAfter();

                //Create a 5X5 table and insert some dummy record  
                Microsoft.Office.Interop.Word.Table firstTable = document.Tables.Add(document.Paragraphs.Add().Range
                                                                , students.Count+1, 5);

                firstTable.Columns[1].PreferredWidth = winword.Application.CentimetersToPoints(1.5f);
                firstTable.Columns[2].PreferredWidth = winword.Application.CentimetersToPoints(5f);
                firstTable.Columns[3].PreferredWidth = winword.Application.CentimetersToPoints(2.4f);
                firstTable.Columns[4].PreferredWidth = winword.Application.CentimetersToPoints(1.8f);
                firstTable.Columns[5].PreferredWidth = winword.Application.CentimetersToPoints(5.6f);

                firstTable.Borders.Enable = 1;
                firstTable.Borders.OutsideLineWidth = WdLineWidth.wdLineWidth075pt;

                int index = -1;
                foreach (Row row in firstTable.Rows)
                {
                    foreach (Cell cell in row.Cells)
                    {
                        //Header row  
                        if (cell.RowIndex == 1)
                        {
                            if(cell.ColumnIndex == 1)
                            {
                                cell.Range.Text = "Index";
                            }
                            if(cell.ColumnIndex == 2)
                            {
                                cell.Range.Text = "Full name";
                            }
                            if (cell.ColumnIndex == 3)
                            {
                                cell.Range.Text = "Birth";
                            }
                            if (cell.ColumnIndex == 4)
                            {
                                cell.Range.Text = "Gender";
                            }
                            if (cell.ColumnIndex == 5)
                            {
                                cell.Range.Text = "Note";
                            }
                            cell.Range.Font.Bold = 1;
                            //other format properties goes here  
                            cell.Range.Font.Name = "Arial";
                            cell.Range.Font.Size = 9;
                            
                            //Center alignment for the Header cells  
                            cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                        }
                        //Data row  
                        else
                        {
                            if (cell.ColumnIndex == 1)
                            {
                                cell.Range.Text = students.ElementAt(index).IndexInClass.ToString();
                            }
                            if (cell.ColumnIndex == 2)
                            {
                                cell.Range.Text = students.ElementAt(index).StudentName.ToString();
                            }
                            if (cell.ColumnIndex == 3)
                            {
                                cell.Range.Text = students.ElementAt(index).StudentBirth.ToString();
                            }
                            if (cell.ColumnIndex == 4)
                            {
                                cell.Range.Text = students.ElementAt(index).StudentGender.ToString();
                            }
                            if (cell.ColumnIndex == 5)
                            {
                                cell.Range.Text = "";
                            }
                            cell.Range.Font.Size = 9;
                            cell.Range.Font.Name = "Arial";
                            cell.Range.Font.Bold = 0;
                        }
                    }
                        index++;
                    }

                //Save the document  
                string filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".docx";
                string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Source/Word/"), filename);
                document.SaveAs2(filePath);
                document.Close();
                document = null;
                winword.Quit();
                winword = null;

                return filePath;
          
            
        }
    }
}
