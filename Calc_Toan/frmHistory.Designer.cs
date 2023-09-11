
namespace Calc_Toan
{
    partial class frmHistory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHistory));
            this.lbLine = new System.Windows.Forms.Label();
            this.rtbHistory = new System.Windows.Forms.RichTextBox();
            this.btnCalculator = new ePOSOne.btnProduct.Button_WOC();
            this.btnClear = new ePOSOne.btnProduct.Button_WOC();
            this.SuspendLayout();
            // 
            // lbLine
            // 
            this.lbLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbLine.Location = new System.Drawing.Point(22, 58);
            this.lbLine.Name = "lbLine";
            this.lbLine.Size = new System.Drawing.Size(320, 1);
            this.lbLine.TabIndex = 3;
            // 
            // rtbHistory
            // 
            this.rtbHistory.BackColor = System.Drawing.Color.Black;
            this.rtbHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbHistory.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.rtbHistory.ForeColor = System.Drawing.Color.White;
            this.rtbHistory.Location = new System.Drawing.Point(2, 76);
            this.rtbHistory.Name = "rtbHistory";
            this.rtbHistory.ReadOnly = true;
            this.rtbHistory.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rtbHistory.Size = new System.Drawing.Size(361, 542);
            this.rtbHistory.TabIndex = 5;
            this.rtbHistory.Text = "";
            // 
            // btnCalculator
            // 
            this.btnCalculator.BackgroundImage = global::Calc_Toan.Properties.Resources.calculator_icon;
            this.btnCalculator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalculator.BorderColor = System.Drawing.Color.Empty;
            this.btnCalculator.ButtonColor = System.Drawing.Color.Empty;
            this.btnCalculator.FlatAppearance.BorderSize = 0;
            this.btnCalculator.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCalculator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalculator.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalculator.Location = new System.Drawing.Point(25, 15);
            this.btnCalculator.Name = "btnCalculator";
            this.btnCalculator.OnHoverBorderColor = System.Drawing.Color.Empty;
            this.btnCalculator.OnHoverButtonColor = System.Drawing.Color.Empty;
            this.btnCalculator.OnHoverTextColor = System.Drawing.Color.Empty;
            this.btnCalculator.Size = new System.Drawing.Size(30, 30);
            this.btnCalculator.TabIndex = 4;
            this.btnCalculator.TextColor = System.Drawing.Color.Empty;
            this.btnCalculator.UseVisualStyleBackColor = true;
            this.btnCalculator.Click += new System.EventHandler(this.btnCalculator_Click);
            // 
            // btnClear
            // 
            this.btnClear.BorderColor = System.Drawing.Color.Empty;
            this.btnClear.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(37)))));
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(25, 634);
            this.btnClear.Name = "btnClear";
            this.btnClear.OnHoverBorderColor = System.Drawing.Color.Empty;
            this.btnClear.OnHoverButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClear.OnHoverTextColor = System.Drawing.Color.White;
            this.btnClear.Size = new System.Drawing.Size(317, 50);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "Xoá nhật ký";
            this.btnClear.TextColor = System.Drawing.Color.White;
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(366, 696);
            this.Controls.Add(this.rtbHistory);
            this.Controls.Add(this.btnCalculator);
            this.Controls.Add(this.lbLine);
            this.Controls.Add(this.btnClear);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmHistory";
            this.Text = "History";
            this.ResumeLayout(false);

        }

        #endregion

        private ePOSOne.btnProduct.Button_WOC btnClear;
        private System.Windows.Forms.Label lbLine;
        private ePOSOne.btnProduct.Button_WOC btnCalculator;
        private System.Windows.Forms.RichTextBox rtbHistory;
    }
}