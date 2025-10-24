namespace UI
{
    partial class PedidoLogoForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTecnica = new System.Windows.Forms.Label();
            this.cmbTecnica = new System.Windows.Forms.ComboBox();
            this.lblUbicacion = new System.Windows.Forms.Label();
            this.cmbUbicacion = new System.Windows.Forms.ComboBox();
            this.lblProveedor = new System.Windows.Forms.Label();
            this.cmbProveedor = new System.Windows.Forms.ComboBox();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.nudCantidad = new System.Windows.Forms.NumericUpDown();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.panelBotones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).BeginInit();
            this.panelBotones.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.Controls.Add(this.lblTecnica, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbTecnica, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblUbicacion, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbUbicacion, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblProveedor, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbProveedor, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblCantidad, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.nudCantidad, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblDescripcion, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtDescripcion, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(460, 162);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblTecnica
            // 
            this.lblTecnica.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTecnica.AutoSize = true;
            this.lblTecnica.Location = new System.Drawing.Point(3, 7);
            this.lblTecnica.Name = "lblTecnica";
            this.lblTecnica.Size = new System.Drawing.Size(46, 13);
            this.lblTecnica.TabIndex = 0;
            this.lblTecnica.Text = "Técnica";
            // 
            // cmbTecnica
            // 
            this.cmbTecnica.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTecnica.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTecnica.FormattingEnabled = true;
            this.cmbTecnica.Location = new System.Drawing.Point(164, 3);
            this.cmbTecnica.Name = "cmbTecnica";
            this.cmbTecnica.Size = new System.Drawing.Size(293, 21);
            this.cmbTecnica.TabIndex = 1;
            // 
            // lblUbicacion
            // 
            this.lblUbicacion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUbicacion.AutoSize = true;
            this.lblUbicacion.Location = new System.Drawing.Point(3, 35);
            this.lblUbicacion.Name = "lblUbicacion";
            this.lblUbicacion.Size = new System.Drawing.Size(55, 13);
            this.lblUbicacion.TabIndex = 2;
            this.lblUbicacion.Text = "Ubicación";
            // 
            // cmbUbicacion
            // 
            this.cmbUbicacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbUbicacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUbicacion.FormattingEnabled = true;
            this.cmbUbicacion.Location = new System.Drawing.Point(164, 31);
            this.cmbUbicacion.Name = "cmbUbicacion";
            this.cmbUbicacion.Size = new System.Drawing.Size(293, 21);
            this.cmbUbicacion.TabIndex = 3;
            // 
            // lblProveedor
            // 
            this.lblProveedor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProveedor.AutoSize = true;
            this.lblProveedor.Location = new System.Drawing.Point(3, 63);
            this.lblProveedor.Name = "lblProveedor";
            this.lblProveedor.Size = new System.Drawing.Size(56, 13);
            this.lblProveedor.TabIndex = 4;
            this.lblProveedor.Text = "Proveedor";
            // 
            // cmbProveedor
            // 
            this.cmbProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProveedor.FormattingEnabled = true;
            this.cmbProveedor.Location = new System.Drawing.Point(164, 59);
            this.cmbProveedor.Name = "cmbProveedor";
            this.cmbProveedor.Size = new System.Drawing.Size(293, 21);
            this.cmbProveedor.TabIndex = 5;
            // 
            // lblCantidad
            // 
            this.lblCantidad.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Location = new System.Drawing.Point(3, 91);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(49, 13);
            this.lblCantidad.TabIndex = 6;
            this.lblCantidad.Text = "Cantidad";
            // 
            // nudCantidad
            // 
            this.nudCantidad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudCantidad.Location = new System.Drawing.Point(164, 87);
            this.nudCantidad.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudCantidad.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCantidad.Name = "nudCantidad";
            this.nudCantidad.Size = new System.Drawing.Size(293, 20);
            this.nudCantidad.TabIndex = 7;
            this.nudCantidad.ThousandsSeparator = true;
            this.nudCantidad.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(3, 158);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(63, 13);
            this.lblDescripcion.TabIndex = 8;
            this.lblDescripcion.Text = "Descripción";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescripcion.Location = new System.Drawing.Point(164, 115);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescripcion.Size = new System.Drawing.Size(293, 72);
            this.txtDescripcion.TabIndex = 9;
            // 
            // panelBotones
            // 
            this.panelBotones.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBotones.AutoSize = true;
            this.panelBotones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBotones.Controls.Add(this.btnAceptar);
            this.panelBotones.Controls.Add(this.btnCancelar);
            this.panelBotones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelBotones.Location = new System.Drawing.Point(278, 180);
            this.panelBotones.Name = "panelBotones";
            this.panelBotones.Size = new System.Drawing.Size(194, 29);
            this.panelBotones.TabIndex = 1;
            // 
            // btnAceptar
            // 
            this.btnAceptar.AutoSize = true;
            this.btnAceptar.Location = new System.Drawing.Point(116, 3);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.AutoSize = true;
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(35, 3);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // PedidoLogoForm
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(484, 221);
            this.Controls.Add(this.panelBotones);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PedidoLogoForm";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Logo";
            this.Load += new System.EventHandler(this.PedidoLogoForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).EndInit();
            this.panelBotones.ResumeLayout(false);
            this.panelBotones.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblTecnica;
        private System.Windows.Forms.ComboBox cmbTecnica;
        private System.Windows.Forms.Label lblUbicacion;
        private System.Windows.Forms.ComboBox cmbUbicacion;
        private System.Windows.Forms.Label lblProveedor;
        private System.Windows.Forms.ComboBox cmbProveedor;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.NumericUpDown nudCantidad;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.FlowLayoutPanel panelBotones;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}