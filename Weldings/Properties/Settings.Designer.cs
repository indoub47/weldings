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
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.1.0.0")]
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
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\pc\\Documents\\Visual Studio 2017\\Projects\\Weldings\\DB\\pirmieji.txt")]
        public string PirmiejiSuvirinimaiFile {
            get {
                return ((string)(this["PirmiejiSuvirinimaiFile"]));
            }
            set {
                this["PirmiejiSuvirinimaiFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\pc\\Documents\\Visual Studio 2017\\Projects\\Weldings\\DB\\kiti.txt")]
        public string NepirmiejiSuvirinimaiFile {
            get {
                return ((string)(this["NepirmiejiSuvirinimaiFile"]));
            }
            set {
                this["NepirmiejiSuvirinimaiFile"] = value;
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
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\pc\\Desktop\\DataVerifyErrors.txt")]
        public string DataVerifyOutputFilePath {
            get {
                return ((string)(this["DataVerifyOutputFilePath"]));
            }
            set {
                this["DataVerifyOutputFilePath"] = value;
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
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int StartRow {
            get {
                return ((int)(this["StartRow"]));
            }
            set {
                this["StartRow"] = value;
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
        [global::System.Configuration.DefaultSettingValueAttribute("db_backup {0:yyyy-MM-dd H-mm-ss}")]
        public string DBBackupFilenameFormat {
            get {
                return ((string)(this["DBBackupFilenameFormat"]));
            }
            set {
                this["DBBackupFilenameFormat"] = value;
            }
        }
    }
}