namespace HHTRQDChonTuong
{
    partial class FormAHPPA
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
            this.dataGridViewHP = new System.Windows.Forms.DataGridView();
            this.dataGridViewTLVL = new System.Windows.Forms.DataGridView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.sqlCommand1 = new Microsoft.Data.SqlClient.SqlCommand();
            this.dataGridViewCSVC = new System.Windows.Forms.DataGridView();
            this.dataGridViewHDXH = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTLVL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCSVC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHDXH)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewHP
            // 
            this.dataGridViewHP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHP.Location = new System.Drawing.Point(12, 3);
            this.dataGridViewHP.Name = "dataGridViewHP";
            this.dataGridViewHP.RowHeadersWidth = 51;
            this.dataGridViewHP.RowTemplate.Height = 29;
            this.dataGridViewHP.Size = new System.Drawing.Size(82, 241);
            this.dataGridViewHP.TabIndex = 0;
            // 
            // dataGridViewTLVL
            // 
            this.dataGridViewTLVL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTLVL.Location = new System.Drawing.Point(100, 3);
            this.dataGridViewTLVL.Name = "dataGridViewTLVL";
            this.dataGridViewTLVL.RowHeadersWidth = 51;
            this.dataGridViewTLVL.RowTemplate.Height = 29;
            this.dataGridViewTLVL.Size = new System.Drawing.Size(82, 241);
            this.dataGridViewTLVL.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(188, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 29;
            this.dataGridView1.Size = new System.Drawing.Size(79, 241);
            this.dataGridView1.TabIndex = 2;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(273, 3);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.RowTemplate.Height = 29;
            this.dataGridView2.Size = new System.Drawing.Size(86, 241);
            this.dataGridView2.TabIndex = 3;
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(581, 291);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowHeadersWidth = 51;
            this.dataGridView3.RowTemplate.Height = 29;
            this.dataGridView3.Size = new System.Drawing.Size(547, 241);
            this.dataGridView3.TabIndex = 4;
            // 
            // sqlCommand1
            // 
            this.sqlCommand1.CommandTimeout = 30;
            this.sqlCommand1.Connection = null;
            this.sqlCommand1.Notification = null;
            this.sqlCommand1.Transaction = null;
            // 
            // dataGridViewCSVC
            // 
            this.dataGridViewCSVC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCSVC.Location = new System.Drawing.Point(451, 3);
            this.dataGridViewCSVC.Name = "dataGridViewCSVC";
            this.dataGridViewCSVC.RowHeadersWidth = 51;
            this.dataGridViewCSVC.RowTemplate.Height = 29;
            this.dataGridViewCSVC.Size = new System.Drawing.Size(448, 241);
            this.dataGridViewCSVC.TabIndex = 5;
            // 
            // dataGridViewHDXH
            // 
            this.dataGridViewHDXH.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHDXH.Location = new System.Drawing.Point(68, 291);
            this.dataGridViewHDXH.Name = "dataGridViewHDXH";
            this.dataGridViewHDXH.RowHeadersWidth = 51;
            this.dataGridViewHDXH.RowTemplate.Height = 29;
            this.dataGridViewHDXH.Size = new System.Drawing.Size(494, 219);
            this.dataGridViewHDXH.TabIndex = 6;
            // 
            // FormAHPPA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1255, 595);
            this.Controls.Add(this.dataGridViewHDXH);
            this.Controls.Add(this.dataGridViewCSVC);
            this.Controls.Add(this.dataGridView3);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.dataGridViewTLVL);
            this.Controls.Add(this.dataGridViewHP);
            this.Name = "FormAHPPA";
            this.Text = "FormAHPPA";
            this.Load += new System.EventHandler(this.FormAHPPA_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTLVL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCSVC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHDXH)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewHP;
        private System.Windows.Forms.DataGridView dataGridViewTLVL;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView3;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand1;
        private System.Windows.Forms.DataGridView dataGridViewCSVC;
        private System.Windows.Forms.DataGridView dataGridViewHDXH;
    }
}