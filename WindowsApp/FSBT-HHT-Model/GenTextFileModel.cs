using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_Model
{
    public class GenTextFileModel
    {
        public string HHTName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string LocationCode { get; set; }
        public int RecordAmount { get; set; }
        public int ScanMode { get; set; }
        public string DepartmentCode { get; set; }
    }
    public class FileModel
    {
        public int fileCount { get; set; }
        public string fileName { get; set; }
        public int fileID { get; set; }
    }

    public class FileMod
    {
        public int fileCount { get; set; }
        public string fileName { get; set; }
    }

    public class MeargeFileFirstRecord
    {
        public string Computer { get; set; }
        public string Record { get; set; }
        public string QtyFront { get; set; }
        public string QtyBack { get; set; }
        public string QtyStockPcs { get; set; }
        public string QtyStockPck { get; set; }
        public string QtyWTPcs { get; set; }
        public string QtyWTG { get; set; }
    }

    public class FileModelDetail
    {
        public int FileDetailID { get; set; }
        public int FileID { get; set; }
        public int Index { get; set; }
        public string PrimaryKey { get; set; }
        public string Field { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
        public int DecPos { get; set; }
        public int StartPos { get; set; }
        public int EndPos { get; set; }
        public string Default { get; set; }
    }
}
