using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.BusinessObjects.Class
{
    public class CommonLib
    {
        public static int CInt(object expression)
        {
            if (expression == null || expression == DBNull.Value)
                return 0;
            string chuoi = expression.ToString();
            chuoi = chuoi.Replace(" ", "");
            if (int.TryParse(chuoi, out int testInt))
                return testInt;
            return 0;
        }

        public static string CString(object expression)
        {
            string chuoi = "";
            if (expression != null && expression != DBNull.Value)
            {
                chuoi = expression.ToString().Trim();
            }
            return chuoi;
        }
    }
}
