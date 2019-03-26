using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSBT_HHT_BLL
{
    public static class LocalParameter
    {
        public static string programPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.ProductName);
        public static string DBName = "STOCKTAKING_HHT.sdf";
        public static string validateDBName = "COMPUTER_NAME.sdf";
    }
}
