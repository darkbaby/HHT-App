using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FSBT.HHT.App.UI
{
    public partial class Loading_Screen : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        #region "Declare Variable"
        private static string _message2 = string.Empty;
        private string _message = string.Empty;
        #endregion

        #region "Properties"

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        public static string Message2
        {
            get { return _message2; }
            set { _message2 = value; }
        }
        #endregion

        #region "Constructor"
        public Loading_Screen()
        {
            InitializeComponent();
        }

        private void Loading_Screen_Load(object sender, EventArgs e)
        {
            this.Text = Message2;
            lblMessage.Text = Message2;
        }

        //Delegate for cross thread call to close 
        private delegate void CloseDelegate();

        //The type of form to be displayed as the splash screen. 
        private static Loading_Screen splashForm;
        private static bool threadCall;
        private static Thread thread;

        static public void ShowSplashScreen(string message = "Loading... Please waiting")
        {
            try
            {
                Message2 = message;

                // Make sure it is only launched once. 
                if (splashForm != null || threadCall)
                    return;
                thread = new Thread(new ThreadStart(Loading_Screen.ShowForm));
                thread.IsBackground = true;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                threadCall = true;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        static private void ShowForm()
        {
            try
            {
                splashForm = new Loading_Screen();
                Application.Run(splashForm);
                threadCall = false;
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Exception : {0}", ex.StackTrace));
            }
        }

        static public void CloseForm()
        {
            try
            {
                if (splashForm != null)
                {
                    if (splashForm.InvokeRequired)splashForm.Invoke(new CloseDelegate(Loading_Screen.CloseFormInternal));
                    splashForm = null;
                }
                else if (threadCall)
                {
                    thread.Abort();
                    threadCall = false;
                }
            }
            catch (Exception ex)
            {
                //log.Error(String.Format("Exception : {0}", ex.StackTrace));
                log.Error(String.Format("Exception : {0}", ex == null ? string.Empty : ex.Message));
            }
        }

        static private void CloseFormInternal()
        {
            try
            {
                splashForm.Close();
            }
            catch (Exception ex)
            {
                //log.Error(String.Format("Exception : {0}", ex.StackTrace));
                log.Error(String.Format("Exception : {0}", ex.Message));
            }
        }

        #endregion

        private void lblMessage_Click(object sender, EventArgs e)
        {

        }

    }
}
