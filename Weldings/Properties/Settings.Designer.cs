﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Weldings.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(" PT-{0}/{1}/{2:d}")]
        public string PapildomoPastaba {
            get {
                return ((string)(this["PapildomoPastaba"]));
            }
            set {
                this["PapildomoPastaba"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4")]
        public string Ifas {
            get {
                return ((string)(this["Ifas"]));
            }
            set {
                this["Ifas"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;")]
        public string OleDbConnectionString {
            get {
                return ((string)(this["OleDbConnectionString"]));
            }
            set {
                this["OleDbConnectionString"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\pc\\Documents\\Visual Studio 2017\\Projects\\Weldings\\DB\\Def2010-IF4.mdb")]
        public string AccessDbPath {
            get {
                return ((string)(this["AccessDbPath"]));
            }
            set {
                this["AccessDbPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Google Sheets API .NET")]
        public string ApplicationName {
            get {
                return ((string)(this["ApplicationName"]));
            }
            set {
                this["ApplicationName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("./data/spreadsheets.json")]
        public string SpreadsheetsJsonFilePath {
            get {
                return ((string)(this["SpreadsheetsJsonFilePath"]));
            }
            set {
                this["SpreadsheetsJsonFilePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("yyyy-MM-dd")]
        public string DateFormat {
            get {
                return ((string)(this["DateFormat"]));
            }
            set {
                this["DateFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:/WeldingOutput")]
        public string OutputPath {
            get {
                return ((string)(this["OutputPath"]));
            }
            set {
                this["OutputPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("db_backup {0:yyyy-MM-dd} {1:H-mm-ss}")]
        public string DBBackupFilenameFormat {
            get {
                return ((string)(this["DBBackupFilenameFormat"]));
            }
            set {
                this["DBBackupFilenameFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CheckDateIfReal {
            get {
                return ((bool)(this["CheckDateIfReal"]));
            }
            set {
                this["CheckDateIfReal"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("14")]
        public float AllowedDayCount {
            get {
                return ((float)(this["AllowedDayCount"]));
            }
            set {
                this["AllowedDayCount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("data_verify_info {0:yyyy-MM-dd} {1:H-mm-ss}")]
        public string DataVerifyFileNameFormat {
            get {
                return ((string)(this["DataVerifyFileNameFormat"]));
            }
            set {
                this["DataVerifyFileNameFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("db_update_info {0:yyyy-MM-dd} {1:H-mm-ss}")]
        public string DbUpdateFileNameFormat {
            get {
                return ((string)(this["DbUpdateFileNameFormat"]));
            }
            set {
                this["DbUpdateFileNameFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("./data/Spreadsheets.json")]
        public string SpreadsheetsJsonPath {
            get {
                return ((string)(this["SpreadsheetsJsonPath"]));
            }
            set {
                this["SpreadsheetsJsonPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("db_update_report {0:yyyy-MM-dd} {1:H-mm-ss}")]
        public string UpdateReportFileNameFormat {
            get {
                return ((string)(this["UpdateReportFileNameFormat"]));
            }
            set {
                this["UpdateReportFileNameFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("proxy.adm.lg")]
        public string Proxy {
            get {
                return ((string)(this["Proxy"]));
            }
            set {
                this["Proxy"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8080")]
        public int Port {
            get {
                return ((int)(this["Port"]));
            }
            set {
                this["Port"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("client_secret.json")]
        public string ClientSecretFile {
            get {
                return ((string)(this["ClientSecretFile"]));
            }
            set {
                this["ClientSecretFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".credentials/sheets.googleapis.com-dotnet.json")]
        public string CredentialDir {
            get {
                return ((string)(this["CredentialDir"]));
            }
            set {
                this["CredentialDir"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool AbortOnFailedRollback {
            get {
                return ((bool)(this["AbortOnFailedRollback"]));
            }
            set {
                this["AbortOnFailedRollback"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CreateDBBackup {
            get {
                return ((bool)(this["CreateDBBackup"]));
            }
            set {
                this["CreateDBBackup"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("log.txt")]
        public string LogFileName {
            get {
                return ((string)(this["LogFileName"]));
            }
            set {
                this["LogFileName"] = value;
            }
        }
    }
}
