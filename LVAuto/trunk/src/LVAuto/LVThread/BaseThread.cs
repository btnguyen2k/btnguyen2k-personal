using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace LVAuto.LVThread
{
    /// <summary>
    /// Base class for all "Auto Thread" classes.
    /// </summary>
    public abstract class BaseThread
    {
        protected Label Message;
        protected Thread InThread;
        public bool IsRunning = false;
        public int SleepTime = 60000;

        /// <summary>
        /// Constructs a new BaseThread object
        /// </summary>
        /// <param name="labelMessage"></param>
        public BaseThread(Label labelMessage)
        {
            this.Message = labelMessage;
        }

        /// <summary>
        /// Writes a log message to the "Log" tab.
        /// </summary>
        /// <param name="msg"></param>
        protected void WriteLog(string msg)
        {
            LVForm.FrmMain.LVFRMMAIN.WriteLog(msg);
        }

        /// <summary>
        /// Sub-class overrides this method to actually implement the business logic.
        /// </summary>
        protected abstract void Run();

        /// <summary>
        /// Starts auto task
        /// </summary>
        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                InThread = new Thread(new ThreadStart(Run));
                IsRunning = true;
                InThread.Start();
            }
        }

        /// <summary>
        /// Stops auto task
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                InThread.Abort();
                InThread.Join();
                Message.ForeColor = System.Drawing.Color.Blue;
                Message.Text = "Đã dừng bởi người sử dụng";
                IsRunning = false;
            }
        }

        delegate void SetTextCallback(string text);

        /// <summary>
        /// Updates the label message
        /// </summary>
        /// <param name="str"></param>
        public void SetText(String str)
        {
            if (this.Message.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Message.Invoke(d, new object[] { str });
            }
            else
            {
                this.Message.Text = str;
            }
        }
    } //end class
} //end namespace
