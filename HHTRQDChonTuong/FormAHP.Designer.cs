namespace HHTRQDChonTuong
{
    partial class FormAHP
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
            this.dataGridViewTrAHP = new System.Windows.Forms.DataGridView();
            this.dataGridViewTC = new System.Windows.Forms.DataGridView();
            this.textBoxSaiSo = new System.Windows.Forms.TextBox();
            this.buttonTT = new System.Windows.Forms.Button();
            this.sqlCommand1 = new Microsoft.Data.SqlClient.SqlCommand();
            this.textBoxCI = new System.Windows.Forms.TextBox();
            this.textBoxCR = new System.Windows.Forms.TextBox();
            this.buttonKT = new System.Windows.Forms.Button();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.dataGridView5 = new System.Windows.Forms.DataGridView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView6 = new System.Windows.Forms.DataGridView();
            this.dataGridView7 = new System.Windows.Forms.DataGridView();
            this.textBoxLmax = new System.Windows.Forms.TextBox();
            this.textBoxCSNQ = new System.Windows.Forms.TextBox();
            this.textBoxCSCR = new System.Windows.Forms.TextBox();
            this.dataGridView8 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTrAHP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView8)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewTrAHP
            // 
            this.dataGridViewTrAHP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTrAHP.Location = new System.Drawing.Point(931, 0);
            this.dataGridViewTrAHP.Name = "dataGridViewTrAHP";
            this.dataGridViewTrAHP.RowHeadersWidth = 51;
            this.dataGridViewTrAHP.RowTemplate.Height = 29;
            this.dataGridViewTrAHP.Size = new System.Drawing.Size(739, 321);
            this.dataGridViewTrAHP.TabIndex = 0;
            // 
            // dataGridViewTC
            // 
            this.dataGridViewTC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTC.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTC.Name = "dataGridViewTC";
            this.dataGridViewTC.RowHeadersWidth = 51;
            this.dataGridViewTC.RowTemplate.Height = 29;
            this.dataGridViewTC.Size = new System.Drawing.Size(498, 321);
            this.dataGridViewTC.TabIndex = 1;
            this.dataGridViewTC.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTC_CellContentClick);
            this.dataGridViewTC.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTC_CellValueChanged);
            // 
            // textBoxSaiSo
            // 
            this.textBoxSaiSo.Location = new System.Drawing.Point(0, 655);
            this.textBoxSaiSo.Name = "textBoxSaiSo";
            this.textBoxSaiSo.Size = new System.Drawing.Size(242, 27);
            this.textBoxSaiSo.TabIndex = 2;
            this.textBoxSaiSo.Visible = false;
            // 
            // buttonTT
            // 
            this.buttonTT.Location = new System.Drawing.Point(404, 693);
            this.buttonTT.Name = "buttonTT";
            this.buttonTT.Size = new System.Drawing.Size(94, 29);
            this.buttonTT.TabIndex = 5;
            this.buttonTT.Text = "Tiếp theo";
            this.buttonTT.UseVisualStyleBackColor = true;
            this.buttonTT.Click += new System.EventHandler(this.buttonTT_Click);
            // 
            // sqlCommand1
            // 
            this.sqlCommand1.CommandTimeout = 30;
            this.sqlCommand1.Connection = null;
            this.sqlCommand1.Notification = null;
            this.sqlCommand1.Transaction = null;
            // 
            // textBoxCI
            // 
            this.textBoxCI.Location = new System.Drawing.Point(12, 581);
            this.textBoxCI.Name = "textBoxCI";
            this.textBoxCI.Size = new System.Drawing.Size(215, 27);
            this.textBoxCI.TabIndex = 6;
            this.textBoxCI.Visible = false;
            // 
            // textBoxCR
            // 
            this.textBoxCR.Location = new System.Drawing.Point(305, 581);
            this.textBoxCR.Name = "textBoxCR";
            this.textBoxCR.Size = new System.Drawing.Size(232, 27);
            this.textBoxCR.TabIndex = 7;
            this.textBoxCR.Visible = false;
            // 
            // buttonKT
            // 
            this.buttonKT.Location = new System.Drawing.Point(583, 677);
            this.buttonKT.Name = "buttonKT";
            this.buttonKT.Size = new System.Drawing.Size(229, 29);
            this.buttonKT.TabIndex = 8;
            this.buttonKT.Text = "Kiểm tra độ nhất quán";
            this.buttonKT.UseVisualStyleBackColor = true;
            this.buttonKT.Visible = false;
            this.buttonKT.Click += new System.EventHandler(this.buttonKT_Click);
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(0, 327);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowHeadersWidth = 51;
            this.dataGridView3.RowTemplate.Height = 29;
            this.dataGridView3.Size = new System.Drawing.Size(498, 95);
            this.dataGridView3.TabIndex = 9;
            this.dataGridView3.Visible = false;
            // 
            // dataGridView4
            // 
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Location = new System.Drawing.Point(509, 351);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.RowHeadersWidth = 51;
            this.dataGridView4.RowTemplate.Height = 29;
            this.dataGridView4.Size = new System.Drawing.Size(81, 291);
            this.dataGridView4.TabIndex = 10;
            this.dataGridView4.Visible = false;
            this.dataGridView4.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView4_CellContentClick);
            // 
            // dataGridView5
            // 
            this.dataGridView5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView5.Location = new System.Drawing.Point(613, 351);
            this.dataGridView5.Name = "dataGridView5";
            this.dataGridView5.RowHeadersWidth = 51;
            this.dataGridView5.RowTemplate.Height = 29;
            this.dataGridView5.Size = new System.Drawing.Size(101, 291);
            this.dataGridView5.TabIndex = 11;
            this.dataGridView5.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(720, 375);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 29;
            this.dataGridView1.Size = new System.Drawing.Size(202, 269);
            this.dataGridView1.TabIndex = 12;
            this.dataGridView1.Visible = false;
            // 
            // dataGridView6
            // 
            this.dataGridView6.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView6.Location = new System.Drawing.Point(945, 359);
            this.dataGridView6.Name = "dataGridView6";
            this.dataGridView6.RowHeadersWidth = 51;
            this.dataGridView6.RowTemplate.Height = 29;
            this.dataGridView6.Size = new System.Drawing.Size(186, 283);
            this.dataGridView6.TabIndex = 13;
            this.dataGridView6.Visible = false;
            // 
            // dataGridView7
            // 
            this.dataGridView7.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView7.Location = new System.Drawing.Point(1154, 363);
            this.dataGridView7.Name = "dataGridView7";
            this.dataGridView7.RowHeadersWidth = 51;
            this.dataGridView7.RowTemplate.Height = 29;
            this.dataGridView7.Size = new System.Drawing.Size(215, 285);
            this.dataGridView7.TabIndex = 14;
            this.dataGridView7.Visible = false;
            // 
            // textBoxLmax
            // 
            this.textBoxLmax.Location = new System.Drawing.Point(818, 677);
            this.textBoxLmax.Name = "textBoxLmax";
            this.textBoxLmax.Size = new System.Drawing.Size(168, 27);
            this.textBoxLmax.TabIndex = 15;
            this.textBoxLmax.Visible = false;
            // 
            // textBoxCSNQ
            // 
            this.textBoxCSNQ.Location = new System.Drawing.Point(992, 679);
            this.textBoxCSNQ.Name = "textBoxCSNQ";
            this.textBoxCSNQ.Size = new System.Drawing.Size(162, 27);
            this.textBoxCSNQ.TabIndex = 16;
            this.textBoxCSNQ.Visible = false;
            // 
            // textBoxCSCR
            // 
            this.textBoxCSCR.Location = new System.Drawing.Point(1160, 679);
            this.textBoxCSCR.Name = "textBoxCSCR";
            this.textBoxCSCR.Size = new System.Drawing.Size(214, 27);
            this.textBoxCSCR.TabIndex = 17;
            // 
            // dataGridView8
            // 
            this.dataGridView8.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView8.Location = new System.Drawing.Point(504, 0);
            this.dataGridView8.Name = "dataGridView8";
            this.dataGridView8.RowHeadersWidth = 51;
            this.dataGridView8.RowTemplate.Height = 29;
            this.dataGridView8.Size = new System.Drawing.Size(418, 321);
            this.dataGridView8.TabIndex = 18;
            // 
            // FormAHP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1682, 753);
            this.Controls.Add(this.dataGridView8);
            this.Controls.Add(this.textBoxCSCR);
            this.Controls.Add(this.textBoxCSNQ);
            this.Controls.Add(this.textBoxLmax);
            this.Controls.Add(this.dataGridView7);
            this.Controls.Add(this.dataGridView6);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.dataGridView5);
            this.Controls.Add(this.dataGridView4);
            this.Controls.Add(this.dataGridView3);
            this.Controls.Add(this.buttonKT);
            this.Controls.Add(this.textBoxCR);
            this.Controls.Add(this.textBoxCI);
            this.Controls.Add(this.buttonTT);
            this.Controls.Add(this.textBoxSaiSo);
            this.Controls.Add(this.dataGridViewTC);
            this.Controls.Add(this.dataGridViewTrAHP);
            this.Name = "FormAHP";
            this.Text = "FormAHP";
            this.Load += new System.EventHandler(this.FormAHP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTrAHP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView8)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewTrAHP;
        private System.Windows.Forms.DataGridView dataGridViewTC;
        private System.Windows.Forms.TextBox textBoxSaiSo;
        private System.Windows.Forms.Button buttonTT;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand1;
        private System.Windows.Forms.TextBox textBoxCI;
        private System.Windows.Forms.TextBox textBoxCR;
        private System.Windows.Forms.Button buttonKT;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.DataGridView dataGridView5;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView6;
        private System.Windows.Forms.DataGridView dataGridView7;
        private System.Windows.Forms.TextBox textBoxLmax;
        private System.Windows.Forms.TextBox textBoxCSNQ;
        private System.Windows.Forms.TextBox textBoxCSCR;
        private System.Windows.Forms.DataGridView dataGridView8;
    }
}