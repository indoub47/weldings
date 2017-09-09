using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Weldings.Properties;

namespace Weldings
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
            bgw.WorkerReportsProgress = true;
            bgw.WorkerSupportsCancellation = true;
            bgw.DoWork += new DoWorkEventHandler(bgwDoWork);
            bgw.ProgressChanged += new ProgressChangedEventHandler(bgwProgressChanged);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwRunWorkerCompleted);
            ControllerVerify.ProgressUpdated += RespondToProgressUpdate;
            ControllerUpdate.ProgressUpdated += RespondToProgressUpdate;
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            loadSettings();
        }

        private void btnVerifyData_Click(object sender, EventArgs e)
        {
            if (bgw.IsBusy != true)
            {
                Object[] argument = { "verify" };
                bgw.RunWorkerAsync(argument);
            }
        }

        private void btnDBUpdate_Click(object sender, EventArgs e)
        {
            if (bgw.IsBusy != true)
            {
                Object[] argument = { "update" };
                bgw.RunWorkerAsync(argument);
            }
        }

        private void bgwDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            txbProgress.Text = string.Empty;
            string argument = ((object[])e.Argument)[0].ToString();
            switch (argument)
            {
                case "verify":
                    doDataVerifyWork(worker, e);
                    break;
                case "update":
                    doDBUpdateWork(worker, e);
                    break;
            }
        }

        private void RespondToProgressUpdate(Object sender, ProgressChangedEventArgs e)
        {
            bgw.ReportProgress(e.ProgressPercentage, e.UserState);
            // Šitas invokina bgwProgessChanged.
            // Jeigu mėginti naudoti bgwProgressChanged tiesiogiai, 
            // meta exception, kad mėginama manipuliuoti UI iš BGW thread
        }

        // This event handler updates the progress text box.
        private void bgwProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.txbProgress.Text != string.Empty)
                this.txbProgress.AppendText("\r\n");
            if (e.ProgressPercentage == 0)
            {
                txbProgress.AppendText(" - " + e.UserState.ToString());
            }
            else
            {
                txbProgress.AppendText(e.ProgressPercentage.ToString() + ". " + e.UserState.ToString());
            }
        }

        // This event handler deals with the results of the
        // background operation.
        private void bgwRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                txbProgress.AppendText("\r\nAn error occured.");
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                txbProgress.AppendText("\r\nCanceled.");
            }
            else if (e.Result != null)
            {
                txbProgress.AppendText("\r\n" + e.Result.ToString());
            }
            else
            {
                txbProgress.AppendText("\r\n something went wrong.");
            }

            // čia sutvarkyti - nes skirtingi dalykai turi būti atlikti, priklausomai nuo to, kuris bgw dirbo
            btnDBUpdate.Enabled = true;
        }



        private string createFileName(string format, string whatFile, string extension = "")
        {
            string fn = string.Empty;
            if (extension == "")
            {
                extension = StartFormMessages.Default.OutputFileExtension;
            }

            try
            {
                fn = Path.Combine(
                    Settings.Default.OutputPath,
                    string.Format(format, DateTime.Now, DateTime.Now) + extension
                    );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(
                        StartFormMessages.Default.BadFileNameFormatMessage,
                        whatFile, format) + ex.Message,
                    StartFormMessages.Default.BadStringFormatErrorTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                LogWriter.Log(ex);
                fn = string.Empty;
            }
            return fn;
        }

        private void checkOutputFolder()
        {
            if (!Directory.Exists(Settings.Default.OutputPath))
            {
                Directory.CreateDirectory(Settings.Default.OutputPath);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {                
                worker.CancelAsync();
            }
            else
            {
                Application.Exit();
            }            
        }


        /// <summary>
        /// Checks the ability to create and write to a file in the supplied directory.
        /// </summary>
        /// <param name="directory">String representing the directory path to check.</param>
        /// <returns>True if successful; otherwise false.</returns>
        private static bool dirIsWritable(string directory)
        {
            const string TEMP_FILE = "tempFile.tmp";
            bool success = false;
            string fullPath = Path.Combine(directory, TEMP_FILE);

            if (Directory.Exists(directory))
            {
                try
                {
                    using (FileStream fs = new FileStream(fullPath, FileMode.Create,
                                                                    FileAccess.Write))
                    {
                        fs.WriteByte(0xff);
                    }

                    if (File.Exists(fullPath))
                    {                        
                        success = true;
                        try
                        {
                            File.Delete(fullPath);
                        }
                        catch
                        {
                            // neleido ištrinti tmp, tai ir chuj s nim
                        }
                    }                    
                }
                catch (Exception)
                {
                    success = false;
                }
            }

            return success;
        }
    }
}
