using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calc_Toan
{
    public partial class frmHistory : Form
    {
        private List<String> historyList;

        private void historyEmpty()
        {
            // Chỉnh căn lề của văn bản hiện tại (đang được chọn) vào chính giữa
            rtbHistory.SelectionAlignment = HorizontalAlignment.Center;

            rtbHistory.SelectionFont = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Regular);
            rtbHistory.AppendText("Không có nhật ký");

            btnClear.Hide();
        }

        public frmHistory(List<String> historyList)
        {
            InitializeComponent();

            this.historyList = historyList;

            if (historyList.Count == 0)
            {
                historyEmpty();
            }
            else
            {
                // Chỉnh căn lề của văn bản hiện tại (đang được chọn) sang bên phải
                rtbHistory.SelectionAlignment = HorizontalAlignment.Right;

                // Đặt lề nội dung cho đoạn văn bản hiện tại
                rtbHistory.SelectionIndent = 25; // Lề trái
                rtbHistory.SelectionRightIndent = 25; // Lề phải

                // cập nhật nhật ký
                for (int i = 0; i < historyList.Count; i += 2)
                {
                    rtbHistory.SelectionFont = new System.Drawing.Font("Arial", 18, System.Drawing.FontStyle.Regular);
                    rtbHistory.AppendText(historyList[i] + "\n");

                    rtbHistory.SelectionFont = new System.Drawing.Font("Arial", 20, System.Drawing.FontStyle.Regular);
                    rtbHistory.SelectionColor = Color.FromArgb(45, 122, 187);
                    rtbHistory.AppendText(historyList[i + 1] + "\n");

                    rtbHistory.AppendText("\n");

                }
            }
        }

        private void btnCalculator_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn chắc chắn muốn xoá nhật ký ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                // Người dùng đã chọn "OK", thực hiện hành động tương ứng ở đây
                this.historyList.Clear();
                rtbHistory.Clear();
                historyEmpty();
            }
        }
    }
}
