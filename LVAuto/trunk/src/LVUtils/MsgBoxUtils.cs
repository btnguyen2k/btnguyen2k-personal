using System.Windows.Forms;

namespace LVUtils
{
    public class MsgBoxUtils
    {
        /// <summary>
        /// Display an error message box
        /// </summary>
        /// <param name="message"></param>
        public static void ErrorBox(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Display a warning message box
        /// </summary>
        /// <param name="message"></param>
        public static void WarningBox(string message)
        {
            MessageBox.Show(message, "Lưu Ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
