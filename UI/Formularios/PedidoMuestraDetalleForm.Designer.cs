namespace UI
{
    partial class PedidoMuestraDetalleForm
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
            this.lblProducto = new System.Windows.Forms.Label();
            this.cmbProducto = new System.Windows.Forms.ComboBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.lblPrecio = new System.Windows.Forms.Label();
            this.nudPrecio = new System.Windows.Forms.NumericUpDown();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.nudCantidad = new System.Windows.Forms.NumericUpDown();
            this.lblDevolucion = new System.Windows.Forms.Label();
            this.dtpDevolucion = new System.Windows.Forms.DateTimePicker();
            this.flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrecio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).BeginInit();
            this.flowButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.lblProducto, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbProducto, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblEstado, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbEstado, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblPrecio, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.nudPrecio, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblCantidad, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.nudCantidad, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblDevolucion, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.dtpDevolucion, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(414, 160);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblProducto
            // 
            this.lblProducto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProducto.AutoSize = true;
            this.lblProducto.Location = new System.Drawing.Point(3, 8);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(94, 13);
            this.lblProducto.TabIndex = 0;
            this.lblProducto.Text = "sampleOrder.detail.product";
            // 
            // cmbProducto
            // 
            this.cmbProducto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProducto.FormattingEnabled = true;
            this.cmbProducto.Location = new System.Drawing.Point(169, 3);
            this.cmbProducto.Name = "cmbProducto";
            this.cmbProducto.Size = new System.Drawing.Size(242, 21);
            this.cmbProducto.TabIndex = 1;
            // 
            // lblEstado
            // 
            this.lblEstado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(3, 38);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(93, 13);
            this.lblEstado.TabIndex = 2;
            this.lblEstado.Text = "sampleOrder.detail.state";
            // 
            // cmbEstado
            // 
            this.cmbEstado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Location = new System.Drawing.Point(169, 33);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(242, 21);
            this.cmbEstado.TabIndex = 3;
            // 
            // lblPrecio
            // 
            this.lblPrecio.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPrecio.AutoSize = true;
            this.lblPrecio.Location = new System.Drawing.Point(3, 68);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new System.Drawing.Size(93, 13);
            this.lblPrecio.TabIndex = 4;
            this.lblPrecio.Text = "sampleOrder.detail.price";
            // 
            // nudPrecio
            // 
            this.nudPrecio.DecimalPlaces = 2;
            this.nudPrecio.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudPrecio.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudPrecio.Name = "nudPrecio";
            this.nudPrecio.Size = new System.Drawing.Size(120, 20);
            this.nudPrecio.TabIndex = 5;
            this.nudPrecio.ThousandsSeparator = true;
            // 
            // lblCantidad
            // 
            this.lblCantidad.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Location = new System.Drawing.Point(3, 98);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(104, 13);
            this.lblCantidad.TabIndex = 6;
            this.lblCantidad.Text = "sampleOrder.detail.quantity";
            // 
            // nudCantidad
            // 
            this.nudCantidad.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudCantidad.Location = new System.Drawing.Point(169, 93);
            this.nudCantidad.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCantidad.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCantidad.Name = "nudCantidad";
            this.nudCantidad.Size = new System.Drawing.Size(60, 20);
            this.nudCantidad.TabIndex = 7;
            this.nudCantidad.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblDevolucion
            // 
            this.lblDevolucion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDevolucion.AutoSize = true;
            this.lblDevolucion.Location = new System.Drawing.Point(3, 128);
            this.lblDevolucion.Name = "lblDevolucion";
            this.lblDevolucion.Size = new System.Drawing.Size(124, 13);
            this.lblDevolucion.TabIndex = 8;
            this.lblDevolucion.Text = "sampleOrder.detail.returnDate";
            // 
            // dtpDevolucion
            // 
            this.dtpDevolucion.Checked = false;
            this.dtpDevolucion.CustomFormat = "dd/MM/yyyy";
            this.dtpDevolucion.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDevolucion.Location = new System.Drawing.Point(169, 123);
            this.dtpDevolucion.Name = "dtpDevolucion";
            this.dtpDevolucion.ShowCheckBox = true;
            this.dtpDevolucion.Size = new System.Drawing.Size(200, 20);
            this.dtpDevolucion.TabIndex = 9;
            // 
            // flowButtons
            // 
            this.flowButtons.AutoSize = true;
            this.flowButtons.Controls.Add(this.btnGuardar);
            this.flowButtons.Controls.Add(this.btnCancelar);
            this.flowButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowButtons.Location = new System.Drawing.Point(10, 177);
            this.flowButtons.Name = "flowButtons";
            this.flowButtons.Size = new System.Drawing.Size(414, 35);
            this.flowButtons.TabIndex = 1;
            // 
            // btnGuardar
            // 
            this.btnGuardar.AutoSize = true;
            this.btnGuardar.Location = new System.Drawing.Point(321, 3);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(90, 29);
            this.btnGuardar.TabIndex = 0;
            this.btnGuardar.Text = "form.save";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.AutoSize = true;
            this.btnCancelar.Location = new System.Drawing.Point(225, 3);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(90, 29);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "form.cancel";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // PedidoMuestraDetalleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 222);
            this.Controls.Add(this.flowButtons);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PedidoMuestraDetalleForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.PedidoMuestraDetalleForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrecio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).EndInit();
            this.flowButtons.ResumeLayout(false);
            this.flowButtons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.ComboBox cmbProducto;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.NumericUpDown nudPrecio;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.NumericUpDown nudCantidad;
        private System.Windows.Forms.Label lblDevolucion;
        private System.Windows.Forms.DateTimePicker dtpDevolucion;
        private System.Windows.Forms.FlowLayoutPanel flowButtons;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}