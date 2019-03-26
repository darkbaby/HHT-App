﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FSBT_HHT_DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BackupHistory> BackupHistories { get; set; }
        public virtual DbSet<BackupHistoryDetail> BackupHistoryDetails { get; set; }
        public virtual DbSet<ConfigBackupTable> ConfigBackupTables { get; set; }
        public virtual DbSet<ConfigFileAS400> ConfigFileAS400 { get; set; }
        public virtual DbSet<ConfigFileDetailAS400> ConfigFileDetailAS400 { get; set; }
        public virtual DbSet<ConfigFileFormat> ConfigFileFormats { get; set; }
        public virtual DbSet<ConfigFileFormatDetail> ConfigFileFormatDetails { get; set; }
        public virtual DbSet<ConfigReport> ConfigReports { get; set; }
        public virtual DbSet<ConfigReport_bak> ConfigReport_bak { get; set; }
        public virtual DbSet<ConfigUserGroup> ConfigUserGroups { get; set; }
        public virtual DbSet<ConfigUserGroupPermission> ConfigUserGroupPermissions { get; set; }
        public virtual DbSet<ConfigUserGroupReport> ConfigUserGroupReports { get; set; }
        public virtual DbSet<ConfigUserGroupScreen> ConfigUserGroupScreens { get; set; }
        public virtual DbSet<DownloadLocation> DownloadLocations { get; set; }
        public virtual DbSet<DownloadSKU> DownloadSKUs { get; set; }
        public virtual DbSet<LogError> LogErrors { get; set; }
        public virtual DbSet<MasterBarcode> MasterBarcodes { get; set; }
        public virtual DbSet<MasterBranch> MasterBranches { get; set; }
        public virtual DbSet<MasterBrand> MasterBrands { get; set; }
        public virtual DbSet<MasterPack> MasterPacks { get; set; }
        public virtual DbSet<MasterPlant> MasterPlants { get; set; }
        public virtual DbSet<MasterReport> MasterReports { get; set; }
        public virtual DbSet<MasterReport_bak> MasterReport_bak { get; set; }
        public virtual DbSet<MasterScreen> MasterScreens { get; set; }
        public virtual DbSet<MasterStorageLocation> MasterStorageLocations { get; set; }
        public virtual DbSet<MasterUnit> MasterUnits { get; set; }
        public virtual DbSet<MastMCHLevel> MastMCHLevels { get; set; }
        public virtual DbSet<MastSAP_Barcode> MastSAP_Barcode { get; set; }
        public virtual DbSet<MastSAP_RegularPrice> MastSAP_RegularPrice { get; set; }
        public virtual DbSet<SystemSetting> SystemSettings { get; set; }
        public virtual DbSet<TempFileAcknowledgeBarDetail> TempFileAcknowledgeBarDetails { get; set; }
        public virtual DbSet<TempFileAcknowledgeSKUDetail> TempFileAcknowledgeSKUDetails { get; set; }
        public virtual DbSet<TempFileBarcodeDetail> TempFileBarcodeDetails { get; set; }
        public virtual DbSet<TempFileBarcodeError> TempFileBarcodeErrors { get; set; }
        public virtual DbSet<TempFileBarcodeLineError> TempFileBarcodeLineErrors { get; set; }
        public virtual DbSet<TempFileRegularPriceDetail> TempFileRegularPriceDetails { get; set; }
        public virtual DbSet<TempFileRegularPriceError> TempFileRegularPriceErrors { get; set; }
        public virtual DbSet<TempFileRegularPriceLineError> TempFileRegularPriceLineErrors { get; set; }
        public virtual DbSet<TempFileSKUDetail> TempFileSKUDetails { get; set; }
        public virtual DbSet<TempFileSKUError> TempFileSKUErrors { get; set; }
        public virtual DbSet<TempFileSKULineError> TempFileSKULineErrors { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<MastSAP_SKU> MastSAP_SKU { get; set; }
        public virtual DbSet<TempFileExportDetail> TempFileExportDetails { get; set; }
        public virtual DbSet<MasterSerialNumber> MasterSerialNumbers { get; set; }
        public virtual DbSet<HHTStocktaking> HHTStocktakings { get; set; }
        public virtual DbSet<tmpHHTStocktaking> tmpHHTStocktakings { get; set; }
        public virtual DbSet<LogSystem> LogSystems { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<MasterSKU> MasterSKUs { get; set; }
        public virtual DbSet<tmp_MasterMapping> tmp_MasterMapping { get; set; }
    }
}
