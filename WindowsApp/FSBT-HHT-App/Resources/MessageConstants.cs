using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT.HHT.App.Resources
{
    public class MessageConstants
    {
        #region Title
        public static string TitleError = "Error";
        public static string TitleInfomation = "Infomation";
        public static string TitleWarning = "Warning";
        public static string TitleConfirm = "Confirm";
        
        #endregion

        #region SysHhtForm
        public static string Cannotconnecttodatabase = "Cannot connect to database.";
        public static string CannotconnecttoHandhelddatabase = "Cannot connect to Handheld database.";
        public static string Invaliddevice = "Invalid device.";
        public static string Nodeviceconnected = "No device connected.";
        public static string PleaseenterIPaddress = "Please enter IP address.";
        public static string DoyouwanttodeleteuploadeddataonHHT = "Upload completed. Do you want to delete uploaded data on HHT?";
        public static string UploadComplete = "Upload Completed";
        public static string Deleteuploadeddatasuccessful = "Delete uploaded data successful.";
        public static string TitleDeleteuploadeddatasuccessful = "Delete uploaded data successful";
        public static string Deleteuploadeddatafail = "Delete uploaded data fail.";
        public static string DeleteuploadeddatafailFailtransfertohht = "Delete uploaded data fail. Fail transfer to hht.";
        public static string DeleteuploadeddatafailNodeviceconnected = "Delete uploaded data fail. No device connected.";
        public static string UploadNotComplete = "Upload Not Completed!";
        public static string Pleasechooselocationbeforeupload = "Please choose location before upload.";
        public static string Recentlyuploadeddatahasduplicatewithpreviousuploadeddata = "Recently uploaded data has duplicate with previous uploaded data.@How do you want to upload?";
        public static string DownloadComplete = "Download Completed!";
        public static string DownloadCompleteDeleteAllTable = "Download Completed. Delete data in tb_m_location table and tb_m_sku table successful";
        public static string DownloadCompleteButsavingdownloadlogisfail = "Download Completed! But saving download log is fail.";
        public static string DownloadNotCompleteFailtotransferfiletoHandheld = "Download Not Complete! Fail to transfer file to Handheld.";
        public static string DownloadNotComplete = "Download Not Complete!";
        public static string DownloadNotCompleteFailtodownloadSKUMaster = "Download Not Complete! Fail to download SKU Master.";
        public static string DownloadNotCompleteFailtodownloadLocation = "Download Not Complete! Fail to download Location.";
        public static string DownloadNotCompleteFailtodownloadUnit = "Download Not Complete! Fail to download Unit.";
        public static string FaildeleteSKUMasterdataindatabase = "Fail delete SKU Master data in database!";
        public static string NoDataToDownload = "No data to download.";
        public static string FaildeleteLocationdatainHandhelddatabase = "Fail delete Location data in Handheld database!";
        public static string NoLocationdatafound = "No Location data found.";
        public static string NoSKUdataFound = "No SKU data found.";
        public static string PleasEnterLocationFormOrLocationTo = "Please enter Location Form or Location To.";
        public static string NoDiffTransactionData = "No different transaction data.";

        #endregion

        #region EditQTYForm
        public static string NoAuditdatafound = "No Audit data found.";
        public static string LocationBarcodeQuantityDescriptionCannotbenull = "Sub Location,Barcode and Quantity can not be null.";
        public static string NewRecordSKUCodeCannotbenull = "SKUCode cannot be null in all new records.";
        public static string ConversionCounterCannotbenull = "Conversion Counter can not be null.";
        public static string Nochangeddata = "No changed data.";
        public static string Nodatadeleterecordreport = "No data for generate report, please try again.";
        public static string cannotgeneratereport = "can’t generate report, please try again.";
        public static string Savecomplete = "Save completed.";
        public static string CannotsaveEditQtydatatodatabase = "Can’t save Edit Qty data to database, please try again.";
        public static string Cannotbenull = " can not null.";
        public static string BarcodenotexistinMaster = "Barcode is not exist in Master.";
        public static string SerialnotexistinMaster = "Serial Number is not exist in Master.";
        public static string LocationnotexistinMaster = "Location is not exist in Master.";
        public static string LocationandBarcodecannotbeduplicate = "Location and Barcode can not be duplicate.";
        public static string SeialNumbernotexistinMaster = "Serial Number is not exist in Master.";
        public static string DoyouwanttochangeScanmode = "Current data will be lost Do you want to continue ?";
        public static string DuplicateSerialNumber = "Serial Number can not be duplicate";
        #endregion

        #region UserManagementForm
        public static string Youhavenotsavecurrentdata = "You have not save current data, save now?";
        public static string TitleSaveEdit = "Save Edit";
        public static string Connectivityissuesoccurred = "Connectivity issues occurred, please try again.";
        public static string cannotsaveuserinformationtodatabase = "can’t save user information to database, please try again.";
        public static string Updatedatacompleted = "Update data completed.";
        public static string Alreadyhavethisuserinserver = "Already have this user in server.";
        public static string Usernamecannotnull = "Username can not null.";
        public static string Usernametooshort = "Username too short.";
        public static string Usernametoolong = "Username too long.";
        public static string Usernamecannotduplicate = "Username can not duplicate.";
        public static string Passwordcannotnull = "Password can not null.";
        public static string Passwordtooshort = "Password too short.";
        public static string Passwordtoolong = "Password too long.";
        public static string Firstnamecannotnull = "Firstname can not null.";
        public static string Lastnamecannotnull = "Lastname can not null.";
        public static string Enablecannotnull = "Enable can not null.";
        public static string Lockcannotnull = "Lock can not null";
        public static string Doyouwanttodeleteuser = "Do you want to delete user ";
        public static string TitleDeleteUser = "Delete User";
        public static string cannotdeleteusernamefromdatabase = "can’t delete username from database, please try again.";
        public static string Nouserfound = "No user found";

        #endregion

        #region UserGroupManagementForm
        public static string Nodatachangedsaveactionisnothappened = "No data changed, save action is not happened.";
        public static string cannotupdatedataindatabase = "can't update data in database, please try again.";
        public static string OneDoyouwanttosavechanges = "(1) Do you want to save changes?";
        public static string TwoDoyouwanttosavechanges = "(2) Do you want to save changes?";
        public static string AnyDoyouwanttosavechanges = "(any) Do you want to save changes?";
        public static string TitleAlert = "Alert";
        public static string Noscreenfound = "No screen found.";
        public static string Usermusthaveagroup = "User must have a group, so you can't remove any user from any group but you can change it.";
        #endregion

        #region SystemSettingsForm
        public static string Dataupdated = "Data updated.";
        public static string cannotsaveinformationtodatabase = "can’t save information to database, please try again.";
        public static string MaxLoginFailcannotbeblank = "Max Login Fail can’t be blank.";
        public static string MaxLoginFailmustbenumberonly = "Max Login Fail must be number only.";
        public static string MaxLoginFailcannotbelessthan1 = "Max Login Fail can’t be less than 1.";
        public static string ComputerIDcannotbeblank = "Computer ID can’t be blank.";
        public static string ComputerIDistoolong = "Computer ID is too long.";
        public static string ComputerNamecannotbeblank = "Computer Name can’t be blank.";
        public static string ComputerNameistoolong = "Computer Name is too long.";
        public static string CountDatecannotbeblank = "Count Date can’t be blank.";
        public static string CountDatecannotmorethan2099 = "Count Date Year cannot set more than 2099AD or 2642BE";
        public static string MCHLevelcannotbeblank = "MCH Level can’t be blank.";
        public static string Plantcannotduplicate = "Plant can't duplicate.";
        #endregion

        #region PrintBarCodeForm
        public static string LocationFrommustbenumberonly = "Location From must be number only.";
        public static string LocationTomustbenumberonly = "Location To must be number only.";
        public static string Sectioncodemustbenumberonly = "Section code must be number only.";
        public static string Departmentcodemustbenumberonly = "Department code must be number only.";
        public static string Nosectiondatafound = "No section data found.";
        #endregion

        #region LocationMasterForm
        public static string Youhavenotsavecurrentdatasavenow = "You have not save current data, save now?";
        public static string Departmentcodecannotbenull = "Department code cannot be null.";
        public static string Sectioncodecannotbenull = "Section Code/Brand Code cannot be null.";
        public static string Sectiontypecannotbenull = "Section type cannot be null.";
        public static string Sectionnamecannotbenull = " Section Name/Brand Name cannot be null.";
        public static string LocationFromcannotbenull = "LocationFrom cannot be null.";
        public static string LocationTocannotbenull = "LocationTo cannot be null.";
        public static string CountSheetcannotbenull = "CountSheet cannot be null.";
        public static string StorageLocationcannotbenull = "StorageLocation cannot be null.";
        public static string Plantcannotbenull = "Plant cannot be null.";
        public static string DuplicatesectioncodeDoyouwanttoreplacesection = "Duplicate section code. Do you want to replace section code now ?";
        public static string Cannotsavesectiondatatodatabase = "Cannot save data to database, please try again.";
        public static string StorageLocationisnotexists = "StorageLocation is not exists in master storage location.";
        public static string Plantisnotexists = "Plant is not exists in master plant.";
        public static string Countsheetisnotexists = "Countsheet is not exists in master";
        public static string Brandisnotexists = "Brand is not exists in master brand.";
        public static string Sectionisnotexists = "Section is not exists in master brand.";
        public static string StorageLocationisnumberonly = "StorageLocation is number only.";
        public static string SectionCodeisnumberonly = "Section Code is number only.";
        public static string LocationFromisnumberonly = "LocationFrom is number only.";
        public static string LocationToisnumberonly = "LocationTo is number only.";
        public static string Departmentisnumberonly = "Department Code is number only.";
        public static string Doyouwanttodeleteallsectionbelow = "Do you want to delete all section below ?";
        public static string Allsectiondatahasbeendeleted = "All section data has been deleted.";
        public static string Errorclearall = "Error clear all";
        public static string Doyouwanttosaveallsectionbelow = "Do you want to save all data below ?";
        public static string Doyouwanttoclearallsectionbelow = "Do you want to clear all data in grid below ?";
        public static string SavesectiondatacompletebutCannotexporttextfile  = "Save data to database completed but Can’t export text file, please try again.";
        public static string Nodatachanged = "No data changed.";
        public static string DeptCodemustbe3digits = "Department Code must be 3 digits.";
        public static string SectionCodemustbe5digits = "Section Code must be 5 digits.";
        public static string StorageLocationmustbe4digits ="Storage Location must be 4 digits";
        public static string SectionTypecannotbeduplicate = "Section Type cannot be duplicate.";
        public static string SectionCodecannotbeduplicate = "Section Code cannot be duplicate.";
        public static string StorageLocationcannotbeduplicate = "Storage Location Code cannot be duplicate.";
        public static string Plantcannotbeduplicate = "Plant cannot be duplicate.";
        public static string Countsheetcannotbeduplicate = "Count Sheet cannot be duplicate.";
        public static string LocationFrommustbe5digits = "LocationFrom must be 5 digits.";
        public static string LocationFromcannotbeduplicate = "LocationFrom cannot be duplicate.";
        public static string LocationFormmustequealorlessthanLocationTo = "LocationForm must equeal or less than LocationTo.";
        public static string LocationTomustbe5digits = "LocationTo must be 5 digits.";
        public static string LocationTocannotbeduplicate = "LocationTo cannot be duplicate.";
        public static string LocationTomustequalormorethanLocationForm = "LocationTo must equal or more than LocationForm.";
        public static string Importcomplete = "Import completed.";
        public static string Cannotimportsectiondatatodatabase = "Can’t import section data to database, please try again.";
        public static string Cannotloadtemplate = "Cannot load template, please try again.";
        public static string Oldsectiondatawillbedeletedafterthisaction = "Old section data will be deleted after this action. Do you want to continue?";
        public static string ClearGrid = "Do you want to clear data in gridview?";
        public static string LocationFromToIncorrect = "Cannot save duplication Location From/To, please verify.";
        public static string ExportLocationFromToIncorrect = "Cannot export duplication Location From/To, please verify.";
        public static string PlantAndCountsheetIncorrect = "Plant and Countsheet are incorrect, please verify.";
        public static string PlantIncorrect = "Plant is incorrect, please verify.";
        public static string CountsheetIncorrect = "Countsheet is incorrect, please verify.";
        public static string Duplicatesectioncode = "Duplicate section code, please verify. ";
        #endregion

        #region GentextFileForm
        public static string Sectioncodeshouldnotlessthan5digits = "Section code should not less than 5 digits.";
        public static string LocationFromshouldnotlessthan5digits = "Location From should not less than 5 digits.";
        public static string LocationToshouldnotlessthan5digits = "Location To should not less than 5 digits.";
        public static string Exportcomplete = "Export completed.";
        public static string PleaseSelectCountSheet = "Please select at least one count sheet.";
        public static string Cannotexporttextfile  = "Can’t export text file, please try again.";
        public static string Storetypecannotnull = "Store type cannot null.";
        public static string Filenamecannotnull = "File name cannot null.";
        public static string Nodata = "No data";
        public static string Cannotaddtextfile = "Can’t add text file, please try again.";
        public static string Mergecomplete = "Merge completed.";
        public static string Cannotmergetextfile = "Can’t merge text file, please try again.";
        public static string NoTextFile = "No Text File";
        public static string Doyouwanttoclearallfilebelow = "Do you want to clear all file below ?";
        public static string DifferentFileType = "Added a new file is different file type, please try again.";
        public static string PathNotExists = "Your path does not exist.";
        public static string ConditionExportNotMatch = "Condition is not match with file export.";
        public static string PleaseenterPassword = "Please enter Password.";
        public static string PasswordWrong = "Wrong Password.";
        #endregion

        #region DownloadMasterForm
        public static string Pleaseuploadfile = "Please upload file.";
        public static string WrongFormatFileEmptyfield = "Wrong Format File : Empty field.";
        public static string WrongFormatFileTextinnumberfield = "Wrong Format File : Text in number field.";
        public static string CannotLoadDataToDatabase = "Cannot Load Data To Database.";
        public static string WrongFormatData = "Wrong Format Data.";
        public static string Doyouwanttoclearalldata = "Do you want to clear all data.";
        public static string ClearDataSuccessful = "Clear Data Successful.";
        public static string CannotClearData = "Cannot Clear Data.";
        public static string NoDatafound = "No Data found.";
        public static string CannotBrowse = "Cannot Browse.";
        public static string CannotconnectSFTP = "Cannot connect to SFTP Server.";
        #endregion

        #region BackupRestoreForm
        public static string CannotrestoreOnly1recordselectedforrestore = "Can't restore, Only 1 record selected for restore.";
        public static string CannotrestorePleaseselectrecordforrestore = "Can't restore, Please select record for restore.";
        public static string Restoresuccessful = "Restore successful.";
        public static string CannotrestoredataPleasetryagain = "Can't restore data, Please try again.";
        public static string Cannotcreatebackupfilepathisinvalid = "Can't not create backup file, path is invalid.";
        public static string Cannotcreatebackupfileerrorexportcsvfile = "Can't not create backup file, error export csv file.";
        public static string Createbackupfilesuccessfull = "Create backup file successfull.";
        public static string BackupNamemustnotbeNullorBlank = "Backup Name must not be Null or Blank.";
        public static string CannotdeletebackupfilewithbackupID = "Can't not delete backup file with backupID : ";
        public static string Deletebackupdatasuccessfull = "Delete backup data successfull.";
        public static string Cannotdeletebackupdata = "Can't delete backup data. Please try again.";
        public static string CannotdeletebackupdataPleaseselectrecordfordelete = "Can't delete backup data, Please select record for delete.";
        public static string Doyouwanttodeletecurrentdata = "Do you want to delete current data ?";
        public static string Doyouwanttodeletebackupdata = "Do you want to delete backup data ?";
        public static string TitleClearDataConfirmation = "Clear Data Confirmation";
        public static string Cleardatasuccessful = "Clear data successful.";
        public static string Cannotcleardata = "Can't clear data, Please try again.";

        #endregion

        #region ReportPrintForm
        public static string TheseDepartmentCodedonotexistOpen = "These Department Code do not exist in system. (";
        public static string TheseSectionCodedonotexistOpen = "These Section Code do not exist in system. (";
        public static string Close = ")";
        public static string PleaseinsertPlantCode = "Please insert Plant Code.";
        public static string PleaseinsertCountSheet ="Please insert Count Sheet.";
        public static string PleaseinsertMCHLevel1 = "Please insert MCH Level 1.";
        public static string PleaseinsertMCHLevel2 = "Please insert MCH Level 2.";
        public static string PleaseinsertMCHLevel3 = "Please insert MCH Level 3.";
        public static string PleaseinsertMCHLevel4 = "Please insert MCH Level 4.";
        public static string PleaseinsertStoregeLocation = "Please insert Storege Location.";
        public static string PleaseinsertSectionCode = "Please insert Section Code.";     
        public static string TheLocationFromhastobelessthanLocationTo = "The Location From has to be less than Location To.";
        public static string TheseLocationCodedonotexistinsystem = "These Location Code do not exist in system. (";
        public static string PleaseinsertLocationFromorLocationTo = "Please insert Location From or Location To.";
        public static string PleaseinsertLocation = "Please insert Location.";
        public static string TheseBrandCodedonotexistinsystem = "These Brand Code do not exist in system. (";
        public static string PleaseinsertBrandCode = "Please insert Brand Code.";
        public static string TheseBarcodedonotexistinsystem = "These Barcode do not exist in system. (";
        public static string PleaseinsertBarcode = "Please insert Barcode.";
        public static string PleaseselectStoreType = "Please select Store Type.";
        public static string PleaseselectDiffType = "Please select Diff Type.";
        public static string PleaseselectCorrectDelete = "Please select Correct/Delete.";
        public static string Nodataforgeneratereport = "No data for generate report, please try again.";
        public static string PleaseselectStoreTypeFrontOrBack = "Please select Store Type Front or Back Only.";
        public static string PleaseselectStoreTypeFrontAndBack = "Please select Store Type Front and Back or Warehouse or Fresh Food Only.";
        #endregion

        #region Login
        public static string PleaseenterUsernamePassword = "Please enter Username/Password.";
        public static string UsernamePasswordmustatleast4letters = "Username/Password must at least 4 letters.";
        public static string CannotOpenProgram = "This application is currently running. Please close application in task manager then, start this application again.";
        #endregion
    }
}
