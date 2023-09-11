using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calc_Toan
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form mainForm = new frmToan();

            mainForm.StartPosition = FormStartPosition.CenterScreen; // Đặt vị trí chính giữa màn hình

            Application.Run(mainForm);
        }
    }
}
