namespace UI.Formularios
{
    partial class BackupRestoreForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.dgvBackups = new System.Windows.Forms.DataGridView();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.btnGenerarBackup = new System.Windows.Forms.Button();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lblUltimoBackup = new System.Windows.Forms.Label();
            this.lblDirectorio = new System.Windows.Forms.Label();
            this.lnkAbrirCarpeta = new System.Windows.Forms.LinkLabel();
            this.lblAyudaRestore = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackups)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(12, 9);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(236, 15);
            this.lblDescripcion.TabIndex = 0;
            this.lblDescripcion.Text = "Descripción del módulo de backup y restore";
            // 
            // dgvBackups
            // 
            this.dgvBackups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBackups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBackups.Location = new System.Drawing.Point(12, 83);
            this.dgvBackups.MultiSelect = false;
            this.dgvBackups.Name = "dgvBackups";
            this.dgvBackups.ReadOnly = true;
            this.dgvBackups.RowHeadersVisible = false;
            this.dgvBackups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBackups.Size = new System.Drawing.Size(760, 320);
            this.dgvBackups.TabIndex = 3;
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefrescar.Location = new System.Drawing.Point(12, 420);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(110, 28);
            this.btnRefrescar.TabIndex = 4;
            this.btnRefrescar.Text = "Actualizar";
            this.btnRefrescar.UseVisualStyleBackColor = true;
            // 
            // btnGenerarBackup
            // 
            this.btnGenerarBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGenerarBackup.Location = new System.Drawing.Point(132, 420);
            this.btnGenerarBackup.Name = "btnGenerarBackup";
            this.btnGenerarBackup.Size = new System.Drawing.Size(150, 28);
            this.btnGenerarBackup.TabIndex = 5;
            this.btnGenerarBackup.Text = "Generar backup";
            this.btnGenerarBackup.UseVisualStyleBackColor = true;
            // 
            // btnRestaurar
            // 
            this.btnRestaurar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRestaurar.Location = new System.Drawing.Point(292, 420);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(150, 28);
            this.btnRestaurar.TabIndex = 6;
            this.btnRestaurar.Text = "Restaurar backup";
            this.btnRestaurar.UseVisualStyleBackColor = true;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Location = new System.Drawing.Point(662, 420);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(110, 28);
            this.btnCerrar.TabIndex = 7;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            // 
            // lblUltimoBackup
            // 
            this.lblUltimoBackup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUltimoBackup.Location = new System.Drawing.Point(12, 406);
            this.lblUltimoBackup.Name = "lblUltimoBackup";
            this.lblUltimoBackup.Size = new System.Drawing.Size(760, 15);
            this.lblUltimoBackup.TabIndex = 8;
            this.lblUltimoBackup.Text = "Último backup:";
            // 
            // lblDirectorio
            // 
            this.lblDirectorio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDirectorio.Location = new System.Drawing.Point(12, 32);
            this.lblDirectorio.Name = "lblDirectorio";
            this.lblDirectorio.Size = new System.Drawing.Size(659, 18);
            this.lblDirectorio.TabIndex = 1;
            this.lblDirectorio.Text = "Directorio de backups:";
            // 
            // lnkAbrirCarpeta
            // 
            this.lnkAbrirCarpeta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkAbrirCarpeta.AutoSize = true;
            this.lnkAbrirCarpeta.Location = new System.Drawing.Point(677, 32);
            this.lnkAbrirCarpeta.Name = "lnkAbrirCarpeta";
            this.lnkAbrirCarpeta.Size = new System.Drawing.Size(95, 15);
            this.lnkAbrirCarpeta.TabIndex = 2;
            this.lnkAbrirCarpeta.TabStop = true;
            this.lnkAbrirCarpeta.Text = "Abrir carpeta...";
            // 
            // lblAyudaRestore
            // 
            this.lblAyudaRestore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAyudaRestore.Location = new System.Drawing.Point(12, 54);
            this.lblAyudaRestore.Name = "lblAyudaRestore";
            this.lblAyudaRestore.Size = new System.Drawing.Size(760, 23);
            this.lblAyudaRestore.TabIndex = 9;
            this.lblAyudaRestore.Text = "Seleccione un backup y presione Restaurar para recuperar la base de datos.";
            // 
            // BackupRestoreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.lblAyudaRestore);
            this.Controls.Add(this.lnkAbrirCarpeta);
            this.Controls.Add(this.lblDirectorio);
            this.Controls.Add(this.lblUltimoBackup);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnRestaurar);
            this.Controls.Add(this.btnGenerarBackup);
            this.Controls.Add(this.btnRefrescar);
            this.Controls.Add(this.dgvBackups);
            this.Controls.Add(this.lblDescripcion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BackupRestoreForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Backups";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackups)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.DataGridView dgvBackups;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.Button btnGenerarBackup;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label lblUltimoBackup;
        private System.Windows.Forms.Label lblDirectorio;
        private System.Windows.Forms.LinkLabel lnkAbrirCarpeta;
        private System.Windows.Forms.Label lblAyudaRestore;
    }
}