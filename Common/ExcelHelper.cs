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
        public static List<TeacherViewModel> ImportTeacherExcel(HttpPostedFileBase file)
        {
            //save file excel
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + Path.GetExtension(file.FileName);
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

        private static List<T> GetList<T> (ExcelWorksheet sheet)
        {
            List<T> list = new List<T>();
            var columnInfo = Enumerable.Range(1, sheet.Dimension.Columns).ToList().Select(n =>
                new { Index = n, ColumnName = sheet.Cells[1, n].Value.ToString() }
            );
            for(int row=2; row< sheet.Dimension.Rows; row++)
            {
                T obj = (T)Activator.CreateInstance(typeof(T)); // get general object;
                foreach(var prop in typeof(T).GetProperties())
                {
                    int col = columnInfo.SingleOrDefault(c => c.ColumnName == prop.Name).Index;
                    var val = sheet.Cells[row, col].Value;
                    var propType = prop.PropertyType;
                    prop.SetValue(obj, Convert.ChangeType(val,propType));
                }
                list.Add(obj);
            }

            return list;
        }

        
    }
}
