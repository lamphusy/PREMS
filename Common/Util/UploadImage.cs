using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Util
{
    public class UploadImage
    {
        public static string UploadOneImage(HttpPostedFileBase file,string path, string name )
        {
            if (file != null)
            {
                var ext = Path.GetExtension(file.FileName);
                string myfile = name + ext;
                // đường dẫn lưu vào database
                var url = path + myfile;
               
                //đường dẫn để lưu tạo file trên ổ cứng
                var path2 = Path.Combine(HttpContext.Current.Server.MapPath(path), myfile);
                if (File.Exists(path2))
                {
                    File.Delete(path2);
                }
                file.SaveAs(path2);

                return url;
            }
            return null;
        }

        public static void DeleteImage(string path)
        {
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }
        }
    }
}
