namespace UI
{
    partial class PedidoDetalleForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblProducto = new System.Windows.Forms.Label();
            this.cmbProducto = new System.Windows.Forms.ComboBox();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.cmbCategoria = new System.Windows.Forms.ComboBox();
            this.lblProveedor = new System.Windows.Forms.Label();
            this.cmbProveedor = new System.Windows.Forms.ComboBox();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.nudCantidad = new System.Windows.Forms.NumericUpDown();
            this.lblPrecio = new System.Windows.Forms.Label();
            this.nudPrecio = new System.Windows.Forms.NumericUpDown();
            this.lblEstado = new System.Windows.Forms.Label();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.lblFechaLimite = new System.Windows.Forms.Label();
            this.panelFecha = new System.Windows.Forms.FlowLayoutPanel();
            this.chkFechaLimite = new System.Windows.Forms.CheckBox();
            this.dtpFechaLimite = new System.Windows.Forms.DateTimePicker();
            this.lblFicha = new System.Windows.Forms.Label();
            this.chkFicha = new System.Windows.Forms.CheckBox();
            this.lblNotas = new System.Windows.Forms.Label();
            this.txtNotas = new System.Windows.Forms.TextBox();
            this.gbLogos = new System.Windows.Forms.GroupBox();
            this.dgvLogos = new System.Windows.Forms.DataGridView();
            this.panelLogosBotones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAgregarLogo = new System.Windows.Forms.Button();
            this.btnEditarLogo = new System.Windows.Forms.Button();
            this.btnEliminarLogo = new System.Windows.Forms.Button();
            this.panelBotones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrecio)).BeginInit();
            this.panelFecha.SuspendLayout();
            this.gbLogos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogos)).BeginInit();
            this.panelLogosBotones.SuspendLayout();
            this.panelBotones.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.Controls.Add(this.lblProducto, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbProducto, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCategoria, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbCategoria, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblProveedor, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbProveedor, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblCantidad, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.nudCantidad, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblPrecio, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.nudPrecio, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblEstado, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.cmbEstado, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblFechaLimite, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.panelFecha, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblFicha, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.chkFicha, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblNotas, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.txtNotas, 1, 8);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(680, 264);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblProducto
            // 
            this.lblProducto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProducto.AutoSize = true;
            this.lblProducto.Location = new System.Drawing.Point(3, 7);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(50, 13);
            this.lblProducto.TabIndex = 0;
            this.lblProducto.Text = "Producto";
            // 
            // cmbProducto
            // 
            this.cmbProducto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbProducto.FormattingEnabled = true;
            this.cmbProducto.Location = new System.Drawing.Point(241, 3);
            this.cmbProducto.Name = "cmbProducto";
            this.cmbProducto.Size = new System.Drawing.Size(436, 21);
            this.cmbProducto.TabIndex = 1;
            this.cmbProducto.SelectedIndexChanged += new System.EventHandler(this.cmbProducto_SelectedIndexChanged);
            this.cmbProducto.TextUpdate += new System.EventHandler(this.cmbProducto_TextUpdate);
            this.cmbProducto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbProducto_KeyDown);
            this.cmbProducto.Leave += new System.EventHandler(this.cmbProducto_Leave);
            // 
            // lblCategoria
            // 
            this.lblCategoria.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.Location = new System.Drawing.Point(3, 35);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new System.Drawing.Size(54, 13);
            this.lblCategoria.TabIndex = 2;
            this.lblCategoria.Text = "Categoría";
            // 
            // cmbCategoria
            // 
            this.cmbCategoria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoria.FormattingEnabled = true;
            this.cmbCategoria.Location = new System.Drawing.Point(241, 31);
            this.cmbCategoria.Name = "cmbCategoria";
            this.cmbCategoria.Size = new System.Drawing.Size(436, 21);
            this.cmbCategoria.TabIndex = 3;
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
            this.cmbProveedor.Location = new System.Drawing.Point(241, 59);
            this.cmbProveedor.Name = "cmbProveedor";
            this.cmbProveedor.Size = new System.Drawing.Size(436, 21);
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
            this.nudCantidad.Location = new System.Drawing.Point(241, 87);
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
            this.nudCantidad.Size = new System.Drawing.Size(436, 20);
            this.nudCantidad.TabIndex = 7;
            this.nudCantidad.ThousandsSeparator = true;
            this.nudCantidad.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblPrecio
            // 
            this.lblPrecio.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPrecio.AutoSize = true;
            this.lblPrecio.Location = new System.Drawing.Point(3, 119);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new System.Drawing.Size(67, 13);
            this.lblPrecio.TabIndex = 8;
            this.lblPrecio.Text = "Precio s/IVA";
            // 
            // nudPrecio
            // 
            this.nudPrecio.DecimalPlaces = 2;
            this.nudPrecio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudPrecio.Location = new System.Drawing.Point(241, 115);
            this.nudPrecio.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudPrecio.Name = "nudPrecio";
            this.nudPrecio.Size = new System.Drawing.Size(436, 20);
            this.nudPrecio.TabIndex = 9;
            this.nudPrecio.ThousandsSeparator = true;
            // 
            // lblEstado
            // 
            this.lblEstado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(3, 147);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(40, 13);
            this.lblEstado.TabIndex = 10;
            this.lblEstado.Text = "Estado";
            // 
            // cmbEstado
            // 
            this.cmbEstado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Location = new System.Drawing.Point(241, 143);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(436, 21);
            this.cmbEstado.TabIndex = 11;
            // 
            // lblFechaLimite
            // 
            this.lblFechaLimite.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFechaLimite.AutoSize = true;
            this.lblFechaLimite.Location = new System.Drawing.Point(3, 177);
            this.lblFechaLimite.Name = "lblFechaLimite";
            this.lblFechaLimite.Size = new System.Drawing.Size(65, 13);
            this.lblFechaLimite.TabIndex = 12;
            this.lblFechaLimite.Text = "Fecha límite";
            // 
            // panelFecha
            // 
            this.panelFecha.AutoSize = true;
            this.panelFecha.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelFecha.Controls.Add(this.chkFechaLimite);
            this.panelFecha.Controls.Add(this.dtpFechaLimite);
            this.panelFecha.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFecha.Location = new System.Drawing.Point(241, 171);
            this.panelFecha.Name = "panelFecha";
            this.panelFecha.Size = new System.Drawing.Size(436, 26);
            this.panelFecha.TabIndex = 13;
            this.panelFecha.WrapContents = false;
            // 
            // chkFechaLimite
            // 
            this.chkFechaLimite.AutoSize = true;
            this.chkFechaLimite.Location = new System.Drawing.Point(3, 3);
            this.chkFechaLimite.Name = "chkFechaLimite";
            this.chkFechaLimite.Size = new System.Drawing.Size(64, 17);
            this.chkFechaLimite.TabIndex = 0;
            this.chkFechaLimite.Text = "Habilitar";
            this.chkFechaLimite.UseVisualStyleBackColor = true;
            this.chkFechaLimite.CheckedChanged += new System.EventHandler(this.chkFechaLimite_CheckedChanged);
            // 
            // dtpFechaLimite
            // 
            this.dtpFechaLimite.Enabled = false;
            this.dtpFechaLimite.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaLimite.Location = new System.Drawing.Point(73, 3);
            this.dtpFechaLimite.Name = "dtpFechaLimite";
            this.dtpFechaLimite.Size = new System.Drawing.Size(140, 20);
            this.dtpFechaLimite.TabIndex = 1;
            // 
            // lblFicha
            // 
            this.lblFicha.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFicha.AutoSize = true;
            this.lblFicha.Location = new System.Drawing.Point(3, 207);
            this.lblFicha.Name = "lblFicha";
            this.lblFicha.Size = new System.Drawing.Size(84, 13);
            this.lblFicha.TabIndex = 14;
            this.lblFicha.Text = "Ficha aplicación";
            // 
            // chkFicha
            // 
            this.chkFicha.AutoSize = true;
            this.chkFicha.Location = new System.Drawing.Point(241, 203);
            this.chkFicha.Name = "chkFicha";
            this.chkFicha.Size = new System.Drawing.Size(15, 14);
            this.chkFicha.TabIndex = 15;
            this.chkFicha.UseVisualStyleBackColor = true;
            // 
            // lblNotas
            // 
            this.lblNotas.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNotas.AutoSize = true;
            this.lblNotas.Location = new System.Drawing.Point(3, 239);
            this.lblNotas.Name = "lblNotas";
            this.lblNotas.Size = new System.Drawing.Size(35, 13);
            this.lblNotas.TabIndex = 18;
            this.lblNotas.Text = "Notas";
            // 
            // txtNotas
            // 
            this.txtNotas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotas.Location = new System.Drawing.Point(241, 231);
            this.txtNotas.Multiline = true;
            this.txtNotas.Name = "txtNotas";
            this.txtNotas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotas.Size = new System.Drawing.Size(436, 30);
            this.txtNotas.TabIndex = 19;
            // 
            // gbLogos
            // 
            this.gbLogos.Controls.Add(this.dgvLogos);
            this.gbLogos.Controls.Add(this.panelLogosBotones);
            this.gbLogos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLogos.Location = new System.Drawing.Point(12, 276);
            this.gbLogos.Name = "gbLogos";
            this.gbLogos.Padding = new System.Windows.Forms.Padding(10, 6, 10, 10);
            this.gbLogos.Size = new System.Drawing.Size(680, 234);
            this.gbLogos.TabIndex = 1;
            this.gbLogos.TabStop = false;
            this.gbLogos.Text = "Logos";
            // 
            // dgvLogos
            // 
            this.dgvLogos.AllowUserToAddRows = false;
            this.dgvLogos.AllowUserToDeleteRows = false;
            this.dgvLogos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLogos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLogos.Location = new System.Drawing.Point(10, 19);
            this.dgvLogos.MultiSelect = false;
            this.dgvLogos.Name = "dgvLogos";
            this.dgvLogos.ReadOnly = true;
            this.dgvLogos.RowHeadersVisible = false;
            this.dgvLogos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogos.Size = new System.Drawing.Size(660, 176);
            this.dgvLogos.TabIndex = 0;
            // 
            // panelLogosBotones
            // 
            this.panelLogosBotones.AutoSize = true;
            this.panelLogosBotones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelLogosBotones.Controls.Add(this.btnAgregarLogo);
            this.panelLogosBotones.Controls.Add(this.btnEditarLogo);
            this.panelLogosBotones.Controls.Add(this.btnEliminarLogo);
            this.panelLogosBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLogosBotones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelLogosBotones.Location = new System.Drawing.Point(10, 195);
            this.panelLogosBotones.Name = "panelLogosBotones";
            this.panelLogosBotones.Size = new System.Drawing.Size(660, 29);
            this.panelLogosBotones.TabIndex = 1;
            // 
            // btnAgregarLogo
            // 
            this.btnAgregarLogo.AutoSize = true;
            this.btnAgregarLogo.Location = new System.Drawing.Point(558, 3);
            this.btnAgregarLogo.Name = "btnAgregarLogo";
            this.btnAgregarLogo.Size = new System.Drawing.Size(99, 23);
            this.btnAgregarLogo.TabIndex = 0;
            this.btnAgregarLogo.Text = "Agregar logo";
            this.btnAgregarLogo.UseVisualStyleBackColor = true;
            this.btnAgregarLogo.Click += new System.EventHandler(this.btnAgregarLogo_Click);
            // 
            // btnEditarLogo
            // 
            this.btnEditarLogo.AutoSize = true;
            this.btnEditarLogo.Location = new System.Drawing.Point(453, 3);
            this.btnEditarLogo.Name = "btnEditarLogo";
            this.btnEditarLogo.Size = new System.Drawing.Size(99, 23);
            this.btnEditarLogo.TabIndex = 1;
            this.btnEditarLogo.Text = "Editar logo";
            this.btnEditarLogo.UseVisualStyleBackColor = true;
            this.btnEditarLogo.Click += new System.EventHandler(this.btnEditarLogo_Click);
            // 
            // btnEliminarLogo
            // 
            this.btnEliminarLogo.AutoSize = true;
            this.btnEliminarLogo.Location = new System.Drawing.Point(348, 3);
            this.btnEliminarLogo.Name = "btnEliminarLogo";
            this.btnEliminarLogo.Size = new System.Drawing.Size(99, 23);
            this.btnEliminarLogo.TabIndex = 2;
            this.btnEliminarLogo.Text = "Eliminar logo";
            this.btnEliminarLogo.UseVisualStyleBackColor = true;
            this.btnEliminarLogo.Click += new System.EventHandler(this.btnEliminarLogo_Click);
            // 
            // panelBotones
            // 
            this.panelBotones.AutoSize = true;
            this.panelBotones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBotones.Controls.Add(this.btnAceptar);
            this.panelBotones.Controls.Add(this.btnCancelar);
            this.panelBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBotones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelBotones.Location = new System.Drawing.Point(12, 510);
            this.panelBotones.Name = "panelBotones";
            this.panelBotones.Size = new System.Drawing.Size(680, 35);
            this.panelBotones.TabIndex = 2;
            // 
            // btnAceptar
            // 
            this.btnAceptar.AutoSize = true;
            this.btnAceptar.Location = new System.Drawing.Point(587, 3);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(90, 29);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.AutoSize = true;
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(491, 3);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(90, 29);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // PedidoDetalleForm
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(704, 557);
            this.Controls.Add(this.gbLogos);
            this.Controls.Add(this.panelBotones);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PedidoDetalleForm";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Detalle de pedido";
            this.Load += new System.EventHandler(this.PedidoDetalleForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrecio)).EndInit();
            this.panelFecha.ResumeLayout(false);
            this.panelFecha.PerformLayout();
            this.gbLogos.ResumeLayout(false);
            this.gbLogos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogos)).EndInit();
            this.panelLogosBotones.ResumeLayout(false);
            this.panelLogosBotones.PerformLayout();
            this.panelBotones.ResumeLayout(false);
            this.panelBotones.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.ComboBox cmbProducto;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.ComboBox cmbCategoria;
        private System.Windows.Forms.Label lblProveedor;
        private System.Windows.Forms.ComboBox cmbProveedor;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.NumericUpDown nudCantidad;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.NumericUpDown nudPrecio;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Label lblFechaLimite;
        private System.Windows.Forms.FlowLayoutPanel panelFecha;
        private System.Windows.Forms.CheckBox chkFechaLimite;
        private System.Windows.Forms.DateTimePicker dtpFechaLimite;
        private System.Windows.Forms.Label lblFicha;
        private System.Windows.Forms.CheckBox chkFicha;
        private System.Windows.Forms.Label lblNotas;
        private System.Windows.Forms.TextBox txtNotas;
        private System.Windows.Forms.GroupBox gbLogos;
        private System.Windows.Forms.DataGridView dgvLogos;
        private System.Windows.Forms.FlowLayoutPanel panelLogosBotones;
        private System.Windows.Forms.Button btnAgregarLogo;
        private System.Windows.Forms.Button btnEditarLogo;
        private System.Windows.Forms.Button btnEliminarLogo;
        private System.Windows.Forms.FlowLayoutPanel panelBotones;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}