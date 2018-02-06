using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Batch_CreateFolder_HHT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string maxFolder = ConfigurationSettings.AppSettings["maxFolder"];
            string path = ConfigurationSettings.AppSettings["pathCreateFolder"];
            try
            {
                var spitPath = path.Split('\\').ToList();
                string mainDisk = spitPath[0].ToString();
                int folderNumber = Convert.ToInt32(maxFolder);
                string fullPath = string.Empty;
                string num = string.Empty;
                if (Directory.Exists(mainDisk))
                {
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                    for (int i = 0; i < folderNumber; i++)
                    {
                        if (i < 10)
                        {
                            num = i.ToString();
                            fullPath = path + "\\00" + num;
                        }
                        else if (i < 100)
                        {
                            num = i.ToString();
                            fullPath = path + "\\0" + num;
                        }
                        else
                        {
                            num = i.ToString();
                            fullPath = path + "\\" + num;
                        }
                        Directory.CreateDirectory(fullPath);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
