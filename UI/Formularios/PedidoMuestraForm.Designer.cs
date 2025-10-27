namespace UI
{
    partial class PedidoMuestraForm
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
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.layoutGeneral = new System.Windows.Forms.TableLayoutPanel();
            this.lblCliente = new System.Windows.Forms.Label();
            this.cmbCliente = new System.Windows.Forms.ComboBox();
            this.lblContacto = new System.Windows.Forms.Label();
            this.txtContacto = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.lblDireccion = new System.Windows.Forms.Label();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.lblFechaEntrega = new System.Windows.Forms.Label();
            this.dtpFechaEntrega = new System.Windows.Forms.DateTimePicker();
            this.lblFechaDevolucion = new System.Windows.Forms.Label();
            this.dtpFechaDevolucionEsperada = new System.Windows.Forms.DateTimePicker();
            this.lblEstadoPedido = new System.Windows.Forms.Label();
            this.cmbEstadoPedido = new System.Windows.Forms.ComboBox();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.chkFacturado = new System.Windows.Forms.CheckBox();
            this.txtFactura = new System.Windows.Forms.TextBox();
            this.btnSeleccionarFactura = new System.Windows.Forms.Button();
            this.grpDetalles = new System.Windows.Forms.GroupBox();
            this.dgvDetalles = new System.Windows.Forms.DataGridView();
            this.flowDetalles = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAgregarDetalle = new System.Windows.Forms.Button();
            this.btnEditarDetalle = new System.Windows.Forms.Button();
            this.btnEliminarDetalle = new System.Windows.Forms.Button();
            this.btnPedirFacturacion = new System.Windows.Forms.Button();
            this.lblExtenderDias = new System.Windows.Forms.Label();
            this.nudDiasExtension = new System.Windows.Forms.NumericUpDown();
            this.btnExtenderDias = new System.Windows.Forms.Button();
            this.grpPagos = new System.Windows.Forms.GroupBox();
            this.layoutPagos = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotalValor = new System.Windows.Forms.Label();
            this.lblPagado = new System.Windows.Forms.Label();
            this.lblPagadoValor = new System.Windows.Forms.Label();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.lblSaldoValor = new System.Windows.Forms.Label();
            this.lblPagoNuevo = new System.Windows.Forms.Label();
            this.nudPago = new System.Windows.Forms.NumericUpDown();
            this.btnAgregarPago = new System.Windows.Forms.Button();
            this.btnEliminarPago = new System.Windows.Forms.Button();
            this.lstPagos = new System.Windows.Forms.ListBox();
            this.flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.layoutMain.SuspendLayout();
            this.grpGeneral.SuspendLayout();
            this.layoutGeneral.SuspendLayout();
            this.grpDetalles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).BeginInit();
            this.flowDetalles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiasExtension)).BeginInit();
            this.grpPagos.SuspendLayout();
            this.layoutPagos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPago)).BeginInit();
            this.flowButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.grpGeneral, 0, 0);
            this.layoutMain.Controls.Add(this.grpDetalles, 0, 1);
            this.layoutMain.Controls.Add(this.grpPagos, 0, 2);
            this.layoutMain.Controls.Add(this.flowButtons, 0, 3);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(10, 10);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 4;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layoutMain.Size = new System.Drawing.Size(864, 641);
            this.layoutMain.TabIndex = 0;
            // 
            // grpGeneral
            // 
            this.grpGeneral.Controls.Add(this.layoutGeneral);
            this.grpGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpGeneral.Location = new System.Drawing.Point(3, 3);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(858, 210);
            this.grpGeneral.TabIndex = 0;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "sampleOrder.group.general";
            // 
            // layoutGeneral
            // 
            this.layoutGeneral.ColumnCount = 4;
            this.layoutGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.layoutGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.layoutGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutGeneral.Controls.Add(this.lblCliente, 0, 0);
            this.layoutGeneral.Controls.Add(this.cmbCliente, 1, 0);
            this.layoutGeneral.Controls.Add(this.lblContacto, 0, 1);
            this.layoutGeneral.Controls.Add(this.txtContacto, 1, 1);
            this.layoutGeneral.Controls.Add(this.lblEmail, 0, 2);
            this.layoutGeneral.Controls.Add(this.txtEmail, 1, 2);
            this.layoutGeneral.Controls.Add(this.lblTelefono, 0, 3);
            this.layoutGeneral.Controls.Add(this.txtTelefono, 1, 3);
            this.layoutGeneral.Controls.Add(this.lblDireccion, 0, 4);
            this.layoutGeneral.Controls.Add(this.txtDireccion, 1, 4);
            this.layoutGeneral.Controls.Add(this.lblFechaEntrega, 2, 0);
            this.layoutGeneral.Controls.Add(this.dtpFechaEntrega, 3, 0);
            this.layoutGeneral.Controls.Add(this.lblFechaDevolucion, 2, 1);
            this.layoutGeneral.Controls.Add(this.dtpFechaDevolucionEsperada, 3, 1);
            this.layoutGeneral.Controls.Add(this.lblEstadoPedido, 2, 2);
            this.layoutGeneral.Controls.Add(this.cmbEstadoPedido, 3, 2);
            this.layoutGeneral.Controls.Add(this.lblObservaciones, 0, 5);
            this.layoutGeneral.Controls.Add(this.txtObservaciones, 1, 5);
            this.layoutGeneral.Controls.Add(this.chkFacturado, 2, 3);
            this.layoutGeneral.Controls.Add(this.txtFactura, 3, 3);
            this.layoutGeneral.Controls.Add(this.btnSeleccionarFactura, 3, 4);
            this.layoutGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutGeneral.Location = new System.Drawing.Point(3, 16);
            this.layoutGeneral.Name = "layoutGeneral";
            this.layoutGeneral.RowCount = 6;
            this.layoutGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutGeneral.Size = new System.Drawing.Size(852, 191);
            this.layoutGeneral.TabIndex = 0;
            // 
            // lblCliente
            // 
            this.lblCliente.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCliente.AutoSize = true;
            this.lblCliente.Location = new System.Drawing.Point(3, 8);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(94, 13);
            this.lblCliente.TabIndex = 0;
            this.lblCliente.Text = "sampleOrder.client";
            // 
            // cmbCliente
            // 
            this.cmbCliente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCliente.FormattingEnabled = true;
            this.cmbCliente.Location = new System.Drawing.Point(143, 3);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(281, 21);
            this.cmbCliente.TabIndex = 1;
            // 
            // lblContacto
            // 
            this.lblContacto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblContacto.AutoSize = true;
            this.lblContacto.Location = new System.Drawing.Point(3, 38);
            this.lblContacto.Name = "lblContacto";
            this.lblContacto.Size = new System.Drawing.Size(135, 13);
            this.lblContacto.TabIndex = 2;
            this.lblContacto.Text = "sampleOrder.contact.name";
            // 
            // txtContacto
            // 
            this.txtContacto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContacto.Location = new System.Drawing.Point(143, 33);
            this.txtContacto.Name = "txtContacto";
            this.txtContacto.Size = new System.Drawing.Size(281, 20);
            this.txtContacto.TabIndex = 3;
            // 
            // lblEmail
            // 
            this.lblEmail.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(3, 68);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(137, 13);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "sampleOrder.contact.email";
            // 
            // txtEmail
            // 
            this.txtEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEmail.Location = new System.Drawing.Point(143, 63);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(281, 20);
            this.txtEmail.TabIndex = 5;
            // 
            // lblTelefono
            // 
            this.lblTelefono.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Location = new System.Drawing.Point(3, 98);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(143, 13);
            this.lblTelefono.TabIndex = 6;
            this.lblTelefono.Text = "sampleOrder.contact.phone";
            // 
            // txtTelefono
            // 
            this.txtTelefono.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTelefono.Location = new System.Drawing.Point(143, 93);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(281, 20);
            this.txtTelefono.TabIndex = 7;
            // 
            // lblDireccion
            // 
            this.lblDireccion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Location = new System.Drawing.Point(3, 128);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new System.Drawing.Size(145, 13);
            this.lblDireccion.TabIndex = 8;
            this.lblDireccion.Text = "sampleOrder.contact.address";
            // 
            // txtDireccion
            // 
            this.txtDireccion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDireccion.Location = new System.Drawing.Point(143, 123);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(281, 20);
            this.txtDireccion.TabIndex = 9;
            // 
            // lblFechaEntrega
            // 
            this.lblFechaEntrega.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFechaEntrega.AutoSize = true;
            this.lblFechaEntrega.Location = new System.Drawing.Point(430, 8);
            this.lblFechaEntrega.Name = "lblFechaEntrega";
            this.lblFechaEntrega.Size = new System.Drawing.Size(141, 13);
            this.lblFechaEntrega.TabIndex = 10;
            this.lblFechaEntrega.Text = "sampleOrder.delivery.date";
            // 
            // dtpFechaEntrega
            // 
            this.dtpFechaEntrega.Checked = false;
            this.dtpFechaEntrega.CustomFormat = "dd/MM/yyyy";
            this.dtpFechaEntrega.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFechaEntrega.Location = new System.Drawing.Point(570, 3);
            this.dtpFechaEntrega.Name = "dtpFechaEntrega";
            this.dtpFechaEntrega.ShowCheckBox = true;
            this.dtpFechaEntrega.Size = new System.Drawing.Size(200, 20);
            this.dtpFechaEntrega.TabIndex = 11;
            // 
            // lblFechaDevolucion
            // 
            this.lblFechaDevolucion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFechaDevolucion.AutoSize = true;
            this.lblFechaDevolucion.Location = new System.Drawing.Point(430, 38);
            this.lblFechaDevolucion.Name = "lblFechaDevolucion";
            this.lblFechaDevolucion.Size = new System.Drawing.Size(140, 13);
            this.lblFechaDevolucion.TabIndex = 12;
            this.lblFechaDevolucion.Text = "sampleOrder.return.expected";
            // 
            // dtpFechaDevolucionEsperada
            // 
            this.dtpFechaDevolucionEsperada.CustomFormat = "dd/MM/yyyy";
            this.dtpFechaDevolucionEsperada.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFechaDevolucionEsperada.Location = new System.Drawing.Point(570, 33);
            this.dtpFechaDevolucionEsperada.Name = "dtpFechaDevolucionEsperada";
            this.dtpFechaDevolucionEsperada.Size = new System.Drawing.Size(200, 20);
            this.dtpFechaDevolucionEsperada.TabIndex = 13;
            // 
            // lblEstadoPedido
            // 
            this.lblEstadoPedido.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEstadoPedido.AutoSize = true;
            this.lblEstadoPedido.Location = new System.Drawing.Point(430, 68);
            this.lblEstadoPedido.Name = "lblEstadoPedido";
            this.lblEstadoPedido.Size = new System.Drawing.Size(97, 13);
            this.lblEstadoPedido.TabIndex = 14;
            this.lblEstadoPedido.Text = "sampleOrder.state";
            // 
            // cmbEstadoPedido
            // 
            this.cmbEstadoPedido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbEstadoPedido.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstadoPedido.FormattingEnabled = true;
            this.cmbEstadoPedido.Location = new System.Drawing.Point(570, 63);
            this.cmbEstadoPedido.Name = "cmbEstadoPedido";
            this.cmbEstadoPedido.Size = new System.Drawing.Size(279, 21);
            this.cmbEstadoPedido.TabIndex = 15;
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Location = new System.Drawing.Point(3, 162);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(106, 13);
            this.lblObservaciones.TabIndex = 16;
            this.lblObservaciones.Text = "sampleOrder.notes";
            // 
            // txtObservaciones
            // 
            this.layoutGeneral.SetColumnSpan(this.txtObservaciones, 2);
            this.txtObservaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtObservaciones.Location = new System.Drawing.Point(143, 153);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservaciones.Size = new System.Drawing.Size(427, 35);
            this.txtObservaciones.TabIndex = 17;
            // 
            // chkFacturado
            // 
            this.chkFacturado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkFacturado.AutoSize = true;
            this.chkFacturado.Location = new System.Drawing.Point(430, 95);
            this.chkFacturado.Name = "chkFacturado";
            this.chkFacturado.Size = new System.Drawing.Size(132, 17);
            this.chkFacturado.TabIndex = 18;
            this.chkFacturado.Text = "sampleOrder.invoiced";
            this.chkFacturado.UseVisualStyleBackColor = true;
            // 
            // txtFactura
            // 
            this.txtFactura.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFactura.Location = new System.Drawing.Point(570, 93);
            this.txtFactura.Name = "txtFactura";
            this.txtFactura.ReadOnly = true;
            this.txtFactura.Size = new System.Drawing.Size(279, 20);
            this.txtFactura.TabIndex = 19;
            // 
            // btnSeleccionarFactura
            // 
            this.btnSeleccionarFactura.Location = new System.Drawing.Point(570, 123);
            this.btnSeleccionarFactura.Name = "btnSeleccionarFactura";
            this.btnSeleccionarFactura.Size = new System.Drawing.Size(120, 23);
            this.btnSeleccionarFactura.TabIndex = 20;
            this.btnSeleccionarFactura.Text = "sampleOrder.invoice.select";
            this.btnSeleccionarFactura.UseVisualStyleBackColor = true;
            this.btnSeleccionarFactura.Click += new System.EventHandler(this.btnSeleccionarFactura_Click);
            // 
            // grpDetalles
            // 
            this.grpDetalles.Controls.Add(this.dgvDetalles);
            this.grpDetalles.Controls.Add(this.flowDetalles);
            this.grpDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetalles.Location = new System.Drawing.Point(3, 219);
            this.grpDetalles.Name = "grpDetalles";
            this.grpDetalles.Size = new System.Drawing.Size(858, 298);
            this.grpDetalles.TabIndex = 1;
            this.grpDetalles.TabStop = false;
            this.grpDetalles.Text = "sampleOrder.group.details";
            // 
            // dgvDetalles
            // 
            this.dgvDetalles.AllowUserToAddRows = false;
            this.dgvDetalles.AllowUserToDeleteRows = false;
            this.dgvDetalles.AllowUserToResizeRows = false;
            this.dgvDetalles.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvDetalles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetalles.Location = new System.Drawing.Point(3, 55);
            this.dgvDetalles.MultiSelect = false;
            this.dgvDetalles.Name = "dgvDetalles";
            this.dgvDetalles.ReadOnly = true;
            this.dgvDetalles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalles.Size = new System.Drawing.Size(852, 240);
            this.dgvDetalles.TabIndex = 1;
            this.dgvDetalles.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDetalles_CellDoubleClick);
            // 
            // flowDetalles
            // 
            this.flowDetalles.AutoSize = true;
            this.flowDetalles.Controls.Add(this.btnAgregarDetalle);
            this.flowDetalles.Controls.Add(this.btnEditarDetalle);
            this.flowDetalles.Controls.Add(this.btnEliminarDetalle);
            this.flowDetalles.Controls.Add(this.btnPedirFacturacion);
            this.flowDetalles.Controls.Add(this.lblExtenderDias);
            this.flowDetalles.Controls.Add(this.nudDiasExtension);
            this.flowDetalles.Controls.Add(this.btnExtenderDias);
            this.flowDetalles.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowDetalles.Location = new System.Drawing.Point(3, 16);
            this.flowDetalles.Name = "flowDetalles";
            this.flowDetalles.Size = new System.Drawing.Size(852, 39);
            this.flowDetalles.TabIndex = 0;
            // 
            // btnAgregarDetalle
            // 
            this.btnAgregarDetalle.AutoSize = true;
            this.btnAgregarDetalle.Location = new System.Drawing.Point(3, 3);
            this.btnAgregarDetalle.Name = "btnAgregarDetalle";
            this.btnAgregarDetalle.Size = new System.Drawing.Size(110, 29);
            this.btnAgregarDetalle.TabIndex = 0;
            this.btnAgregarDetalle.Text = "sampleOrder.detail.add";
            this.btnAgregarDetalle.UseVisualStyleBackColor = true;
            this.btnAgregarDetalle.Click += new System.EventHandler(this.btnAgregarDetalle_Click);
            // 
            // btnEditarDetalle
            // 
            this.btnEditarDetalle.AutoSize = true;
            this.btnEditarDetalle.Location = new System.Drawing.Point(119, 3);
            this.btnEditarDetalle.Name = "btnEditarDetalle";
            this.btnEditarDetalle.Size = new System.Drawing.Size(110, 29);
            this.btnEditarDetalle.TabIndex = 1;
            this.btnEditarDetalle.Text = "sampleOrder.detail.edit";
            this.btnEditarDetalle.UseVisualStyleBackColor = true;
            this.btnEditarDetalle.Click += new System.EventHandler(this.btnEditarDetalle_Click);
            // 
            // btnEliminarDetalle
            // 
            this.btnEliminarDetalle.AutoSize = true;
            this.btnEliminarDetalle.Location = new System.Drawing.Point(235, 3);
            this.btnEliminarDetalle.Name = "btnEliminarDetalle";
            this.btnEliminarDetalle.Size = new System.Drawing.Size(110, 29);
            this.btnEliminarDetalle.TabIndex = 2;
            this.btnEliminarDetalle.Text = "sampleOrder.detail.delete";
            this.btnEliminarDetalle.UseVisualStyleBackColor = true;
            this.btnEliminarDetalle.Click += new System.EventHandler(this.btnEliminarDetalle_Click);
            // 
            // btnPedirFacturacion
            // 
            this.btnPedirFacturacion.AutoSize = true;
            this.btnPedirFacturacion.Location = new System.Drawing.Point(351, 3);
            this.btnPedirFacturacion.Name = "btnPedirFacturacion";
            this.btnPedirFacturacion.Size = new System.Drawing.Size(140, 29);
            this.btnPedirFacturacion.TabIndex = 3;
            this.btnPedirFacturacion.Text = "sampleOrder.request.billing";
            this.btnPedirFacturacion.UseVisualStyleBackColor = true;
            this.btnPedirFacturacion.Click += new System.EventHandler(this.btnPedirFacturacion_Click);
            // 
            // lblExtenderDias
            // 
            this.lblExtenderDias.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblExtenderDias.AutoSize = true;
            this.lblExtenderDias.Location = new System.Drawing.Point(497, 10);
            this.lblExtenderDias.Name = "lblExtenderDias";
            this.lblExtenderDias.Size = new System.Drawing.Size(139, 13);
            this.lblExtenderDias.TabIndex = 4;
            this.lblExtenderDias.Text = "sampleOrder.extend.days";
            // 
            // nudDiasExtension
            // 
            this.nudDiasExtension.Location = new System.Drawing.Point(642, 8);
            this.nudDiasExtension.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudDiasExtension.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDiasExtension.Name = "nudDiasExtension";
            this.nudDiasExtension.Size = new System.Drawing.Size(60, 20);
            this.nudDiasExtension.TabIndex = 5;
            this.nudDiasExtension.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnExtenderDias
            // 
            this.btnExtenderDias.AutoSize = true;
            this.btnExtenderDias.Location = new System.Drawing.Point(708, 3);
            this.btnExtenderDias.Name = "btnExtenderDias";
            this.btnExtenderDias.Size = new System.Drawing.Size(120, 29);
            this.btnExtenderDias.TabIndex = 6;
            this.btnExtenderDias.Text = "sampleOrder.extend.due";
            this.btnExtenderDias.UseVisualStyleBackColor = true;
            this.btnExtenderDias.Click += new System.EventHandler(this.btnExtenderDias_Click);
            // 
            // grpPagos
            // 
            this.grpPagos.Controls.Add(this.layoutPagos);
            this.grpPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPagos.Location = new System.Drawing.Point(3, 523);
            this.grpPagos.Name = "grpPagos";
            this.grpPagos.Size = new System.Drawing.Size(858, 80);
            this.grpPagos.TabIndex = 2;
            this.grpPagos.TabStop = false;
            this.grpPagos.Text = "sampleOrder.group.payments";
            // 
            // layoutPagos
            // 
            this.layoutPagos.ColumnCount = 6;
            this.layoutPagos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.layoutPagos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutPagos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.layoutPagos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutPagos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.layoutPagos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPagos.Controls.Add(this.lblTotal, 0, 0);
            this.layoutPagos.Controls.Add(this.lblTotalValor, 1, 0);
            this.layoutPagos.Controls.Add(this.lblPagado, 2, 0);
            this.layoutPagos.Controls.Add(this.lblPagadoValor, 3, 0);
            this.layoutPagos.Controls.Add(this.lblSaldo, 4, 0);
            this.layoutPagos.Controls.Add(this.lblSaldoValor, 5, 0);
            this.layoutPagos.Controls.Add(this.lblPagoNuevo, 0, 1);
            this.layoutPagos.Controls.Add(this.nudPago, 1, 1);
            this.layoutPagos.Controls.Add(this.btnAgregarPago, 2, 1);
            this.layoutPagos.Controls.Add(this.btnEliminarPago, 3, 1);
            this.layoutPagos.Controls.Add(this.lstPagos, 5, 1);
            this.layoutPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPagos.Location = new System.Drawing.Point(3, 16);
            this.layoutPagos.Name = "layoutPagos";
            this.layoutPagos.RowCount = 2;
            this.layoutPagos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.layoutPagos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPagos.Size = new System.Drawing.Size(852, 61);
            this.layoutPagos.TabIndex = 0;
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(3, 6);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(127, 13);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "sampleOrder.summary.total";
            // 
            // lblTotalValor
            // 
            this.lblTotalValor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTotalValor.AutoSize = true;
            this.lblTotalValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalValor.Location = new System.Drawing.Point(113, 5);
            this.lblTotalValor.Name = "lblTotalValor";
            this.lblTotalValor.Size = new System.Drawing.Size(44, 15);
            this.lblTotalValor.TabIndex = 1;
            this.lblTotalValor.Text = "$0,00";
            // 
            // lblPagado
            // 
            this.lblPagado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPagado.AutoSize = true;
            this.lblPagado.Location = new System.Drawing.Point(213, 6);
            this.lblPagado.Name = "lblPagado";
            this.lblPagado.Size = new System.Drawing.Size(126, 13);
            this.lblPagado.TabIndex = 2;
            this.lblPagado.Text = "sampleOrder.summary.paid";
            // 
            // lblPagadoValor
            // 
            this.lblPagadoValor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPagadoValor.AutoSize = true;
            this.lblPagadoValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblPagadoValor.Location = new System.Drawing.Point(323, 5);
            this.lblPagadoValor.Name = "lblPagadoValor";
            this.lblPagadoValor.Size = new System.Drawing.Size(44, 15);
            this.lblPagadoValor.TabIndex = 3;
            this.lblPagadoValor.Text = "$0,00";
            // 
            // lblSaldo
            // 
            this.lblSaldo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.Location = new System.Drawing.Point(423, 6);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(142, 13);
            this.lblSaldo.TabIndex = 4;
            this.lblSaldo.Text = "sampleOrder.summary.balance";
            // 
            // lblSaldoValor
            // 
            this.lblSaldoValor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSaldoValor.AutoSize = true;
            this.lblSaldoValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblSaldoValor.Location = new System.Drawing.Point(533, 5);
            this.lblSaldoValor.Name = "lblSaldoValor";
            this.lblSaldoValor.Size = new System.Drawing.Size(44, 15);
            this.lblSaldoValor.TabIndex = 5;
            this.lblSaldoValor.Text = "$0,00";
            // 
            // lblPagoNuevo
            // 
            this.lblPagoNuevo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPagoNuevo.AutoSize = true;
            this.lblPagoNuevo.Location = new System.Drawing.Point(3, 37);
            this.lblPagoNuevo.Name = "lblPagoNuevo";
            this.lblPagoNuevo.Size = new System.Drawing.Size(146, 13);
            this.lblPagoNuevo.TabIndex = 6;
            this.lblPagoNuevo.Text = "sampleOrder.payment.amount";
            // 
            // nudPago
            // 
            this.nudPago.DecimalPlaces = 2;
            this.nudPago.Location = new System.Drawing.Point(113, 28);
            this.nudPago.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudPago.Name = "nudPago";
            this.nudPago.Size = new System.Drawing.Size(94, 20);
            this.nudPago.TabIndex = 7;
            this.nudPago.ThousandsSeparator = true;
            // 
            // btnAgregarPago
            // 
            this.btnAgregarPago.AutoSize = true;
            this.btnAgregarPago.Location = new System.Drawing.Point(213, 28);
            this.btnAgregarPago.Name = "btnAgregarPago";
            this.btnAgregarPago.Size = new System.Drawing.Size(110, 26);
            this.btnAgregarPago.TabIndex = 8;
            this.btnAgregarPago.Text = "sampleOrder.payment.add";
            this.btnAgregarPago.UseVisualStyleBackColor = true;
            this.btnAgregarPago.Click += new System.EventHandler(this.btnAgregarPago_Click);
            // 
            // btnEliminarPago
            // 
            this.btnEliminarPago.AutoSize = true;
            this.btnEliminarPago.Location = new System.Drawing.Point(323, 28);
            this.btnEliminarPago.Name = "btnEliminarPago";
            this.btnEliminarPago.Size = new System.Drawing.Size(110, 26);
            this.btnEliminarPago.TabIndex = 9;
            this.btnEliminarPago.Text = "sampleOrder.payment.remove";
            this.btnEliminarPago.UseVisualStyleBackColor = true;
            this.btnEliminarPago.Click += new System.EventHandler(this.btnEliminarPago_Click);
            // 
            // lstPagos
            // 
            this.lstPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPagos.FormattingEnabled = true;
            this.lstPagos.Location = new System.Drawing.Point(533, 28);
            this.lstPagos.Name = "lstPagos";
            this.lstPagos.Size = new System.Drawing.Size(316, 30);
            this.lstPagos.TabIndex = 10;
            // 
            // flowButtons
            // 
            this.flowButtons.AutoSize = true;
            this.flowButtons.Controls.Add(this.btnGuardar);
            this.flowButtons.Controls.Add(this.btnCancelar);
            this.flowButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowButtons.Location = new System.Drawing.Point(3, 609);
            this.flowButtons.Name = "flowButtons";
            this.flowButtons.Size = new System.Drawing.Size(858, 29);
            this.flowButtons.TabIndex = 3;
            // 
            // btnGuardar
            // 
            this.btnGuardar.AutoSize = true;
            this.btnGuardar.Location = new System.Drawing.Point(780, 3);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 23);
            this.btnGuardar.TabIndex = 0;
            this.btnGuardar.Text = "form.save";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.AutoSize = true;
            this.btnCancelar.Location = new System.Drawing.Point(699, 3);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "form.cancel";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // PedidoMuestraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.layoutMain);
            this.MinimumSize = new System.Drawing.Size(900, 700);
            this.Name = "PedidoMuestraForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "sampleOrder.new.title";
            this.Load += new System.EventHandler(this.PedidoMuestraForm_Load);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            this.grpGeneral.ResumeLayout(false);
            this.layoutGeneral.ResumeLayout(false);
            this.layoutGeneral.PerformLayout();
            this.grpDetalles.ResumeLayout(false);
            this.grpDetalles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).EndInit();
            this.flowDetalles.ResumeLayout(false);
            this.flowDetalles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiasExtension)).EndInit();
            this.grpPagos.ResumeLayout(false);
            this.layoutPagos.ResumeLayout(false);
            this.layoutPagos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPago)).EndInit();
            this.flowButtons.ResumeLayout(false);
            this.flowButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.GroupBox grpGeneral;
        private System.Windows.Forms.TableLayoutPanel layoutGeneral;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.ComboBox cmbCliente;
        private System.Windows.Forms.Label lblContacto;
        private System.Windows.Forms.TextBox txtContacto;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.Label lblDireccion;
        private System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.Label lblFechaEntrega;
        private System.Windows.Forms.DateTimePicker dtpFechaEntrega;
        private System.Windows.Forms.Label lblFechaDevolucion;
        private System.Windows.Forms.DateTimePicker dtpFechaDevolucionEsperada;
        private System.Windows.Forms.Label lblEstadoPedido;
        private System.Windows.Forms.ComboBox cmbEstadoPedido;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.CheckBox chkFacturado;
        private System.Windows.Forms.TextBox txtFactura;
        private System.Windows.Forms.Button btnSeleccionarFactura;
        private System.Windows.Forms.GroupBox grpDetalles;
        private System.Windows.Forms.DataGridView dgvDetalles;
        private System.Windows.Forms.FlowLayoutPanel flowDetalles;
        private System.Windows.Forms.Button btnAgregarDetalle;
        private System.Windows.Forms.Button btnEditarDetalle;
        private System.Windows.Forms.Button btnEliminarDetalle;
        private System.Windows.Forms.Button btnPedirFacturacion;
        private System.Windows.Forms.Label lblExtenderDias;
        private System.Windows.Forms.NumericUpDown nudDiasExtension;
        private System.Windows.Forms.Button btnExtenderDias;
        private System.Windows.Forms.GroupBox grpPagos;
        private System.Windows.Forms.TableLayoutPanel layoutPagos;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotalValor;
        private System.Windows.Forms.Label lblPagado;
        private System.Windows.Forms.Label lblPagadoValor;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblSaldoValor;
        private System.Windows.Forms.Label lblPagoNuevo;
        private System.Windows.Forms.NumericUpDown nudPago;
        private System.Windows.Forms.Button btnAgregarPago;
        private System.Windows.Forms.Button btnEliminarPago;
        private System.Windows.Forms.ListBox lstPagos;
        private System.Windows.Forms.FlowLayoutPanel flowButtons;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}