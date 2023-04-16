using Models;
using Models.ViewModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public static class ExcelHelper
    {
        public static List<TeacherViewModel> ImportTeacherExcel(string userId,HttpPostedFileBase file)
        {
            //save file excel
            string filename = "teacher" + userId + DateTime.Now.ToString("yyyyMMddHHmmssffff") + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Source/Excel/"),filename);
            file.SaveAs(filePath);

            //Read file excel
            List<TeacherViewModel> list = new List<TeacherViewModel>();
            using(ExcelPackage package = new ExcelPackage(filePath))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var sheet = package.Workbook.Worksheets[0];

                list = GetList<TeacherViewModel>(sheet);

                return list;
            }

            return null;
        }

        public static List<StudentViewModel> ImportStudentHelper(string userId, HttpPostedFileBase file)
        {
            //save file excel
            string filename = "student"+ userId + DateTime.Now.ToString("yyyyMMddHHmmssffff") + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Source/Excel/"), filename);
            file.SaveAs(filePath);

            //Read file excel
            List<StudentViewModel> list = new List<StudentViewModel>();
            using (ExcelPackage package = new ExcelPackage(filePath))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var sheet = package.Workbook.Worksheets[0];

                list = GetList<StudentViewModel>(sheet);

                return list;
            }

            return null;
        }

        private static List<T> GetList<T> (ExcelWorksheet sheet)
        {
            List<T> list = new List<T>();
            var columnInfo = Enumerable.Range(1, sheet.Dimension.Columns).ToList().Select(n =>
                new { Index = n, ColumnName = sheet.Cells[1, n].Value.ToString() }
            );
            for(int row=2; row<= sheet.Dimension.Rows; row++)
            {
                T obj = (T)Activator.CreateInstance(typeof(T)); // get general object;
                foreach(var prop in typeof(T).GetProperties())
                {
                    if (prop.Name == "IDUser") continue;
                    var check = columnInfo.SingleOrDefault(c => c.ColumnName == prop.Name);

                    if(check != null)
                    {
                        int col = check.Index;
                        var val = sheet.Cells[row, col].Value;
                        var propType = prop.PropertyType;
                        prop.SetValue(obj, Convert.ChangeType(val, propType));
                    }
                    else
                    {
                        continue;
                    }

                }
                list.Add(obj);
            }

            return list;
        }

        
    }
}
