using Miracle.FileZilla.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateUserFTP
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string path = ConfigurationSettings.AppSettings["pathCreateFolder"];
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                Directory.CreateDirectory(path);

                var fileZillaApi = new FileZillaApi();
                fileZillaApi.Connect("");
                var accountSettings = fileZillaApi.GetAccountSettings();

                var user = new User
                {
                    UserName = "admin",
                    SharedFolders = new List<SharedFolder>()
                {
                    new SharedFolder()
                    {
                        Directory = path,
                        AccessRights = AccessRights.DirList | AccessRights.DirCreate | AccessRights.DirSubdirs | AccessRights.DirDelete |
                            AccessRights.FileRead | AccessRights.FileWrite | AccessRights.FileDelete | AccessRights.FileAppend |
                            AccessRights.IsHome
                    }
                }
                };
                user.AssignPassword("admin", fileZillaApi.ProtocolVersion);
                accountSettings.Users.Add(user);
                fileZillaApi.SetAccountSettings(accountSettings);
            }
            catch(Exception ex)
            {

            }
            
        }

    }
}
