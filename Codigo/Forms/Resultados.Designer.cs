namespace DM_SIEMENS_VALIQC.Forms
{
    partial class Resultados
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Resultados));
            this.timerIntervalos = new System.Windows.Forms.Timer(this.components);
            this.lblIntervalos = new System.Windows.Forms.Label();
            this.tlpTopResul = new System.Windows.Forms.TableLayoutPanel();
            this.lblResultados = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.flpContenedorResul = new System.Windows.Forms.FlowLayoutPanel();
            this.pnelTopResul = new System.Windows.Forms.Panel();
            this.tlpTopResul.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pnelTopResul.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerIntervalos
            // 
            this.timerIntervalos.Tick += new System.EventHandler(this.timerIntervalos_Tick);
            // 
            // lblIntervalos
            // 
            this.lblIntervalos.BackColor = System.Drawing.Color.Transparent;
            this.lblIntervalos.Location = new System.Drawing.Point(699, 583);
            this.lblIntervalos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIntervalos.Name = "lblIntervalos";
            this.lblIntervalos.Size = new System.Drawing.Size(35, 15);
            this.lblIntervalos.TabIndex = 0;
            this.lblIntervalos.Text = "Timer";
            this.lblIntervalos.Visible = false;
            // 
            // tlpTopResul
            // 
            this.tlpTopResul.BackColor = System.Drawing.Color.White;
            this.tlpTopResul.ColumnCount = 2;
            this.tlpTopResul.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.46639F));
            this.tlpTopResul.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.53361F));
            this.tlpTopResul.Controls.Add(this.lblResultados, 1, 0);
            this.tlpTopResul.Controls.Add(this.pictureBox2, 0, 0);
            this.tlpTopResul.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTopResul.Location = new System.Drawing.Point(2, 2);
            this.tlpTopResul.Name = "tlpTopResul";
            this.tlpTopResul.RowCount = 1;
            this.tlpTopResul.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTopResul.Size = new System.Drawing.Size(707, 72);
            this.tlpTopResul.TabIndex = 13;
            // 
            // lblResultados
            // 
            this.lblResultados.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblResultados.AutoSize = true;
            this.lblResultados.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(81)))), ((int)(((byte)(252)))));
            this.lblResultados.Location = new System.Drawing.Point(289, 24);
            this.lblResultados.Name = "lblResultados";
            this.lblResultados.Size = new System.Drawing.Size(180, 23);
            this.lblResultados.TabIndex = 0;
            this.lblResultados.Text = "Cargar Resultados";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(229, 11);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(54, 50);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // flpContenedorResul
            // 
            this.flpContenedorResul.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpContenedorResul.AutoScroll = true;
            this.flpContenedorResul.BackColor = System.Drawing.Color.White;
            this.flpContenedorResul.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpContenedorResul.Font = new System.Drawing.Font("Century Gothic", 9.25F, System.Drawing.FontStyle.Bold);
            this.flpContenedorResul.Location = new System.Drawing.Point(24, 134);
            this.flpContenedorResul.Name = "flpContenedorResul";
            this.flpContenedorResul.Size = new System.Drawing.Size(709, 446);
            this.flpContenedorResul.TabIndex = 16;
            this.flpContenedorResul.WrapContents = false;
            // 
            // pnelTopResul
            // 
            this.pnelTopResul.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnelTopResul.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(138)))), ((int)(((byte)(226)))));
            this.pnelTopResul.Controls.Add(this.tlpTopResul);
            this.pnelTopResul.Location = new System.Drawing.Point(24, 27);
            this.pnelTopResul.Name = "pnelTopResul";
            this.pnelTopResul.Padding = new System.Windows.Forms.Padding(2);
            this.pnelTopResul.Size = new System.Drawing.Size(711, 76);
            this.pnelTopResul.TabIndex = 0;
            // 
            // Resultados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(758, 606);
            this.Controls.Add(this.pnelTopResul);
            this.Controls.Add(this.lblIntervalos);
            this.Controls.Add(this.flpContenedorResul);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Resultados";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Terminal";
            this.Load += new System.EventHandler(this.Terminal_Load);
            this.tlpTopResul.ResumeLayout(false);
            this.tlpTopResul.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pnelTopResul.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerIntervalos;
        private System.Windows.Forms.Label lblIntervalos;
        private System.Windows.Forms.TableLayoutPanel tlpTopResul;
        private System.Windows.Forms.Label lblResultados;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.FlowLayoutPanel flpContenedorResul;
        private System.Windows.Forms.Panel pnelTopResul;
    }
}