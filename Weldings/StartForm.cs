using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System.Threading;

namespace Weldings
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void btnVerifyData_Click(object sender, EventArgs e)
        {
            SheetsService service = GSheetsConnector.Connect();
            Spreadsheets.Operator[] operators = SpreadsheetsData.Operators;
            Spreadsheets.SheetsRanges allRanges = SpreadsheetsData.AllRanges;
            
            using (StreamWriter sw = new StreamWriter(Properties.Settings.Default.DataVerifyOutputFilePath))
           {
                try
                {
                    sw.Write(Controller.fetchConvertVerify(service, operators, allRanges));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
               MessageBox.Show("Done.\nDabar bus atidarytas failas su išvardintomis problemomis.");
               Process.Start(Properties.Settings.Default.DataVerifyOutputFilePath);
           }
        }

        private void btnWriteData_Click(object sender, EventArgs e)
        {
            //int countPirmieji = DBUpdater.DoPirmieji();
            //int countNepirmieji = DBUpdater.DoNepirmieji();
            ///MessageBox.Show(string.Format("Įrašyta į duomenų baze:\n{0} - pirmųjų patikrinimų\n{1} - ne pirmųjų patikrinimų", countPirmieji, countNepirmieji));
            this.btnWriteData.Enabled = false;
        }
    }
}
