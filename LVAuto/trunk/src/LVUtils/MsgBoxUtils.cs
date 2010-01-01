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
        /// Display an information message box
        /// </summary>
        /// <param name="message"></param>
        public static void InfoBox(string message)
        {
            MessageBox.Show(message, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
