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
            this.lblNumeroPedido = new System.Windows.Forms.Label();
            this.lblNumeroPedidoValor = new System.Windows.Forms.Label();
            this.lblFechaPedido = new System.Windows.Forms.Label();
            this.lblFechaPedidoValor = new System.Windows.Forms.Label();
            this.lblEstadoPedido = new System.Windows.Forms.Label();
            this.lblEstadoPedidoValor = new System.Windows.Forms.Label();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
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
            this.grpAdjuntos = new System.Windows.Forms.GroupBox();
            this.tableAdjuntos = new System.Windows.Forms.TableLayoutPanel();
            this.lblAdjuntosInstrucciones = new System.Windows.Forms.Label();
            this.dgvAdjuntos = new System.Windows.Forms.DataGridView();
            this.panelAdjuntosAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAgregarAdjunto = new System.Windows.Forms.Button();
            this.btnDescargarAdjunto = new System.Windows.Forms.Button();
            this.btnEliminarAdjunto = new System.Windows.Forms.Button();
            this.grpPagos = new System.Windows.Forms.GroupBox();
            this.layoutPagos = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotalValor = new System.Windows.Forms.Label();
            this.lblPagado = new System.Windows.Forms.Label();
            this.lblPagadoValor = new System.Windows.Forms.Label();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.lblSaldoValor = new System.Windows.Forms.Label();
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
            this.grpAdjuntos.SuspendLayout();
            this.tableAdjuntos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdjuntos)).BeginInit();
            this.panelAdjuntosAcciones.SuspendLayout();
            this.grpPagos.SuspendLayout();
            this.layoutPagos.SuspendLayout();
            this.flowButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.grpGeneral, 0, 0);
            this.layoutMain.Controls.Add(this.grpDetalles, 0, 1);
            this.layoutMain.Controls.Add(this.grpAdjuntos, 0, 2);
            this.layoutMain.Controls.Add(this.grpPagos, 0, 3);
            this.layoutMain.Controls.Add(this.flowButtons, 0, 4);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(10, 10);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 5;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
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
            this.layoutGeneral.Controls.Add(this.lblNumeroPedido, 2, 0);
            this.layoutGeneral.Controls.Add(this.lblNumeroPedidoValor, 3, 0);
            this.layoutGeneral.Controls.Add(this.lblFechaPedido, 2, 1);
            this.layoutGeneral.Controls.Add(this.lblFechaPedidoValor, 3, 1);
            this.layoutGeneral.Controls.Add(this.lblEstadoPedido, 2, 2);
            this.layoutGeneral.Controls.Add(this.lblEstadoPedidoValor, 3, 2);
            this.layoutGeneral.Controls.Add(this.lblObservaciones, 0, 5);
            this.layoutGeneral.Controls.Add(this.txtObservaciones, 1, 5);
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
            this.cmbCliente.Size = new System.Drawing.Size(280, 21);
            this.cmbCliente.TabIndex = 1;
            // 
            // lblContacto
            // 
            this.lblContacto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblContacto.AutoSize = true;
            this.lblContacto.Location = new System.Drawing.Point(3, 38);
            this.lblContacto.Name = "lblContacto";
            this.lblContacto.Size = new System.Drawing.Size(134, 13);
            this.lblContacto.TabIndex = 2;
            this.lblContacto.Text = "sampleOrder.contact.name";
            // 
            // txtContacto
            // 
            this.txtContacto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContacto.Location = new System.Drawing.Point(143, 33);
            this.txtContacto.Name = "txtContacto";
            this.txtContacto.Size = new System.Drawing.Size(280, 20);
            this.txtContacto.TabIndex = 3;
            // 
            // lblEmail
            // 
            this.lblEmail.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(3, 68);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(132, 13);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "sampleOrder.contact.email";
            // 
            // txtEmail
            // 
            this.txtEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEmail.Location = new System.Drawing.Point(143, 63);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(280, 20);
            this.txtEmail.TabIndex = 5;
            // 
            // lblTelefono
            // 
            this.lblTelefono.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Location = new System.Drawing.Point(3, 92);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(132, 26);
            this.lblTelefono.TabIndex = 6;
            this.lblTelefono.Text = "sampleOrder.contact.phone";
            // 
            // txtTelefono
            // 
            this.txtTelefono.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTelefono.Location = new System.Drawing.Point(143, 93);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(280, 20);
            this.txtTelefono.TabIndex = 7;
            // 
            // lblDireccion
            // 
            this.lblDireccion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Location = new System.Drawing.Point(3, 122);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new System.Drawing.Size(129, 26);
            this.lblDireccion.TabIndex = 8;
            this.lblDireccion.Text = "sampleOrder.contact.address";
            // 
            // txtDireccion
            // 
            this.txtDireccion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDireccion.Location = new System.Drawing.Point(143, 123);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(280, 20);
            this.txtDireccion.TabIndex = 9;
            // 
            // lblNumeroPedido
            // 
            this.lblNumeroPedido.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNumeroPedido.AutoSize = true;
            this.lblNumeroPedido.Location = new System.Drawing.Point(429, 8);
            this.lblNumeroPedido.Name = "lblNumeroPedido";
            this.lblNumeroPedido.Size = new System.Drawing.Size(104, 13);
            this.lblNumeroPedido.TabIndex = 10;
            this.lblNumeroPedido.Text = "sampleOrder.number";
            // 
            // lblNumeroPedidoValor
            // 
            this.lblNumeroPedidoValor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNumeroPedidoValor.AutoSize = true;
            this.lblNumeroPedidoValor.Location = new System.Drawing.Point(569, 8);
            this.lblNumeroPedidoValor.Name = "lblNumeroPedidoValor";
            this.lblNumeroPedidoValor.Size = new System.Drawing.Size(10, 13);
            this.lblNumeroPedidoValor.TabIndex = 11;
            this.lblNumeroPedidoValor.Text = "-";
            // 
            // lblFechaPedido
            // 
            this.lblFechaPedido.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFechaPedido.AutoSize = true;
            this.lblFechaPedido.Location = new System.Drawing.Point(429, 38);
            this.lblFechaPedido.Name = "lblFechaPedido";
            this.lblFechaPedido.Size = new System.Drawing.Size(129, 13);
            this.lblFechaPedido.TabIndex = 12;
            this.lblFechaPedido.Text = "sampleOrder.created.date";
            // 
            // lblFechaPedidoValor
            // 
            this.lblFechaPedidoValor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFechaPedidoValor.AutoSize = true;
            this.lblFechaPedidoValor.Location = new System.Drawing.Point(569, 38);
            this.lblFechaPedidoValor.Name = "lblFechaPedidoValor";
            this.lblFechaPedidoValor.Size = new System.Drawing.Size(10, 13);
            this.lblFechaPedidoValor.TabIndex = 13;
            this.lblFechaPedidoValor.Text = "-";
            // 
            // lblEstadoPedido
            // 
            this.lblEstadoPedido.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEstadoPedido.AutoSize = true;
            this.lblEstadoPedido.Location = new System.Drawing.Point(429, 68);
            this.lblEstadoPedido.Name = "lblEstadoPedido";
            this.lblEstadoPedido.Size = new System.Drawing.Size(92, 13);
            this.lblEstadoPedido.TabIndex = 14;
            this.lblEstadoPedido.Text = "sampleOrder.state";
            // 
            // lblEstadoPedidoValor
            // 
            this.lblEstadoPedidoValor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEstadoPedidoValor.AutoSize = true;
            this.lblEstadoPedidoValor.Location = new System.Drawing.Point(569, 68);
            this.lblEstadoPedidoValor.Name = "lblEstadoPedidoValor";
            this.lblEstadoPedidoValor.Size = new System.Drawing.Size(10, 13);
            this.lblEstadoPedidoValor.TabIndex = 15;
            this.lblEstadoPedidoValor.Text = "-";
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Location = new System.Drawing.Point(3, 164);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(95, 13);
            this.lblObservaciones.TabIndex = 16;
            this.lblObservaciones.Text = "sampleOrder.notes";
            // 
            // txtObservaciones
            // 
            this.layoutGeneral.SetColumnSpan(this.txtObservaciones, 3);
            this.txtObservaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtObservaciones.Location = new System.Drawing.Point(143, 153);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservaciones.Size = new System.Drawing.Size(706, 35);
            this.txtObservaciones.TabIndex = 17;
            // 
            // grpDetalles
            // 
            this.grpDetalles.Controls.Add(this.dgvDetalles);
            this.grpDetalles.Controls.Add(this.flowDetalles);
            this.grpDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetalles.Location = new System.Drawing.Point(3, 219);
            this.grpDetalles.Name = "grpDetalles";
            this.grpDetalles.Size = new System.Drawing.Size(858, 146);
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
            this.dgvDetalles.Location = new System.Drawing.Point(3, 86);
            this.dgvDetalles.MultiSelect = false;
            this.dgvDetalles.Name = "dgvDetalles";
            this.dgvDetalles.ReadOnly = true;
            this.dgvDetalles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalles.Size = new System.Drawing.Size(852, 57);
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
            this.flowDetalles.Size = new System.Drawing.Size(852, 70);
            this.flowDetalles.TabIndex = 0;
            // 
            // btnAgregarDetalle
            // 
            this.btnAgregarDetalle.AutoSize = true;
            this.btnAgregarDetalle.Location = new System.Drawing.Point(3, 3);
            this.btnAgregarDetalle.Name = "btnAgregarDetalle";
            this.btnAgregarDetalle.Size = new System.Drawing.Size(125, 29);
            this.btnAgregarDetalle.TabIndex = 0;
            this.btnAgregarDetalle.Text = "sampleOrder.detail.add";
            this.btnAgregarDetalle.UseVisualStyleBackColor = true;
            this.btnAgregarDetalle.Click += new System.EventHandler(this.btnAgregarDetalle_Click);
            // 
            // btnEditarDetalle
            // 
            this.btnEditarDetalle.AutoSize = true;
            this.btnEditarDetalle.Location = new System.Drawing.Point(134, 3);
            this.btnEditarDetalle.Name = "btnEditarDetalle";
            this.btnEditarDetalle.Size = new System.Drawing.Size(124, 29);
            this.btnEditarDetalle.TabIndex = 1;
            this.btnEditarDetalle.Text = "sampleOrder.detail.edit";
            this.btnEditarDetalle.UseVisualStyleBackColor = true;
            this.btnEditarDetalle.Click += new System.EventHandler(this.btnEditarDetalle_Click);
            // 
            // btnEliminarDetalle
            // 
            this.btnEliminarDetalle.AutoSize = true;
            this.btnEliminarDetalle.Location = new System.Drawing.Point(264, 3);
            this.btnEliminarDetalle.Name = "btnEliminarDetalle";
            this.btnEliminarDetalle.Size = new System.Drawing.Size(136, 29);
            this.btnEliminarDetalle.TabIndex = 2;
            this.btnEliminarDetalle.Text = "sampleOrder.detail.delete";
            this.btnEliminarDetalle.UseVisualStyleBackColor = true;
            this.btnEliminarDetalle.Click += new System.EventHandler(this.btnEliminarDetalle_Click);
            // 
            // btnPedirFacturacion
            // 
            this.btnPedirFacturacion.AutoSize = true;
            this.btnPedirFacturacion.Location = new System.Drawing.Point(406, 3);
            this.btnPedirFacturacion.Name = "btnPedirFacturacion";
            this.btnPedirFacturacion.Size = new System.Drawing.Size(143, 29);
            this.btnPedirFacturacion.TabIndex = 3;
            this.btnPedirFacturacion.Text = "sampleOrder.request.billing";
            this.btnPedirFacturacion.UseVisualStyleBackColor = true;
            this.btnPedirFacturacion.Click += new System.EventHandler(this.btnPedirFacturacion_Click);
            // 
            // lblExtenderDias
            // 
            this.lblExtenderDias.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblExtenderDias.AutoSize = true;
            this.lblExtenderDias.Location = new System.Drawing.Point(555, 11);
            this.lblExtenderDias.Name = "lblExtenderDias";
            this.lblExtenderDias.Size = new System.Drawing.Size(126, 13);
            this.lblExtenderDias.TabIndex = 4;
            this.lblExtenderDias.Text = "sampleOrder.extend.days";
            // 
            // nudDiasExtension
            // 
            this.nudDiasExtension.Location = new System.Drawing.Point(687, 3);
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
            this.btnExtenderDias.Location = new System.Drawing.Point(3, 38);
            this.btnExtenderDias.Name = "btnExtenderDias";
            this.btnExtenderDias.Size = new System.Drawing.Size(132, 29);
            this.btnExtenderDias.TabIndex = 6;
            this.btnExtenderDias.Text = "sampleOrder.extend.due";
            this.btnExtenderDias.UseVisualStyleBackColor = true;
            this.btnExtenderDias.Click += new System.EventHandler(this.btnExtenderDias_Click);
            // 
            // grpAdjuntos
            // 
            this.grpAdjuntos.Controls.Add(this.tableAdjuntos);
            this.grpAdjuntos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAdjuntos.Location = new System.Drawing.Point(3, 371);
            this.grpAdjuntos.Name = "grpAdjuntos";
            this.grpAdjuntos.Size = new System.Drawing.Size(858, 146);
            this.grpAdjuntos.TabIndex = 2;
            this.grpAdjuntos.TabStop = false;
            this.grpAdjuntos.Text = "sampleOrder.group.invoices";
            // 
            // tableAdjuntos
            // 
            this.tableAdjuntos.ColumnCount = 1;
            this.tableAdjuntos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableAdjuntos.Controls.Add(this.lblAdjuntosInstrucciones, 0, 0);
            this.tableAdjuntos.Controls.Add(this.dgvAdjuntos, 0, 1);
            this.tableAdjuntos.Controls.Add(this.panelAdjuntosAcciones, 0, 2);
            this.tableAdjuntos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableAdjuntos.Location = new System.Drawing.Point(3, 16);
            this.tableAdjuntos.Name = "tableAdjuntos";
            this.tableAdjuntos.RowCount = 3;
            this.tableAdjuntos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAdjuntos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableAdjuntos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAdjuntos.Size = new System.Drawing.Size(852, 127);
            this.tableAdjuntos.TabIndex = 0;
            // 
            // lblAdjuntosInstrucciones
            // 
            this.lblAdjuntosInstrucciones.AutoSize = true;
            this.lblAdjuntosInstrucciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAdjuntosInstrucciones.Location = new System.Drawing.Point(3, 0);
            this.lblAdjuntosInstrucciones.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.lblAdjuntosInstrucciones.Name = "lblAdjuntosInstrucciones";
            this.lblAdjuntosInstrucciones.Size = new System.Drawing.Size(846, 13);
            this.lblAdjuntosInstrucciones.TabIndex = 0;
            this.lblAdjuntosInstrucciones.Text = "order.attachments.instructions";
            // 
            // dgvAdjuntos
            // 
            this.dgvAdjuntos.AllowUserToAddRows = false;
            this.dgvAdjuntos.AllowUserToDeleteRows = false;
            this.dgvAdjuntos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAdjuntos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdjuntos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAdjuntos.Location = new System.Drawing.Point(3, 22);
            this.dgvAdjuntos.MultiSelect = false;
            this.dgvAdjuntos.Name = "dgvAdjuntos";
            this.dgvAdjuntos.RowHeadersVisible = false;
            this.dgvAdjuntos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAdjuntos.Size = new System.Drawing.Size(846, 61);
            this.dgvAdjuntos.TabIndex = 1;
            // 
            // panelAdjuntosAcciones
            // 
            this.panelAdjuntosAcciones.AutoSize = true;
            this.panelAdjuntosAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelAdjuntosAcciones.Controls.Add(this.btnAgregarAdjunto);
            this.panelAdjuntosAcciones.Controls.Add(this.btnDescargarAdjunto);
            this.panelAdjuntosAcciones.Controls.Add(this.btnEliminarAdjunto);
            this.panelAdjuntosAcciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAdjuntosAcciones.Location = new System.Drawing.Point(3, 89);
            this.panelAdjuntosAcciones.Name = "panelAdjuntosAcciones";
            this.panelAdjuntosAcciones.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.panelAdjuntosAcciones.Size = new System.Drawing.Size(846, 35);
            this.panelAdjuntosAcciones.TabIndex = 2;
            this.panelAdjuntosAcciones.WrapContents = false;
            // 
            // btnAgregarAdjunto
            // 
            this.btnAgregarAdjunto.AutoSize = true;
            this.btnAgregarAdjunto.Location = new System.Drawing.Point(3, 9);
            this.btnAgregarAdjunto.Name = "btnAgregarAdjunto";
            this.btnAgregarAdjunto.Size = new System.Drawing.Size(123, 23);
            this.btnAgregarAdjunto.TabIndex = 0;
            this.btnAgregarAdjunto.Text = "order.attachments.add";
            this.btnAgregarAdjunto.UseVisualStyleBackColor = true;
            // 
            // btnDescargarAdjunto
            // 
            this.btnDescargarAdjunto.AutoSize = true;
            this.btnDescargarAdjunto.Location = new System.Drawing.Point(132, 9);
            this.btnDescargarAdjunto.Name = "btnDescargarAdjunto";
            this.btnDescargarAdjunto.Size = new System.Drawing.Size(151, 23);
            this.btnDescargarAdjunto.TabIndex = 1;
            this.btnDescargarAdjunto.Text = "order.attachments.download";
            this.btnDescargarAdjunto.UseVisualStyleBackColor = true;
            // 
            // btnEliminarAdjunto
            // 
            this.btnEliminarAdjunto.AutoSize = true;
            this.btnEliminarAdjunto.Location = new System.Drawing.Point(289, 9);
            this.btnEliminarAdjunto.Name = "btnEliminarAdjunto";
            this.btnEliminarAdjunto.Size = new System.Drawing.Size(140, 23);
            this.btnEliminarAdjunto.TabIndex = 2;
            this.btnEliminarAdjunto.Text = "order.attachments.remove";
            this.btnEliminarAdjunto.UseVisualStyleBackColor = true;
            // 
            // grpPagos
            // 
            this.grpPagos.Controls.Add(this.layoutPagos);
            this.grpPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPagos.Location = new System.Drawing.Point(3, 523);
            this.grpPagos.Name = "grpPagos";
            this.grpPagos.Size = new System.Drawing.Size(858, 80);
            this.grpPagos.TabIndex = 3;
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
            this.layoutPagos.Controls.Add(this.btnAgregarPago, 0, 1);
            this.layoutPagos.Controls.Add(this.btnEliminarPago, 2, 1);
            this.layoutPagos.Controls.Add(this.lstPagos, 4, 1);
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
            this.lblTotal.Location = new System.Drawing.Point(3, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(102, 25);
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
            this.lblTotalValor.Size = new System.Drawing.Size(43, 15);
            this.lblTotalValor.TabIndex = 1;
            this.lblTotalValor.Text = "$0,00";
            // 
            // lblPagado
            // 
            this.lblPagado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPagado.AutoSize = true;
            this.lblPagado.Location = new System.Drawing.Point(213, 0);
            this.lblPagado.Name = "lblPagado";
            this.lblPagado.Size = new System.Drawing.Size(102, 25);
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
            this.lblPagadoValor.Size = new System.Drawing.Size(43, 15);
            this.lblPagadoValor.TabIndex = 3;
            this.lblPagadoValor.Text = "$0,00";
            // 
            // lblSaldo
            // 
            this.lblSaldo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.Location = new System.Drawing.Point(423, 0);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(102, 25);
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
            this.lblSaldoValor.Size = new System.Drawing.Size(43, 15);
            this.lblSaldoValor.TabIndex = 5;
            this.lblSaldoValor.Text = "$0,00";
            // 
            // btnAgregarPago
            // 
            this.btnAgregarPago.AutoSize = true;
            this.layoutPagos.SetColumnSpan(this.btnAgregarPago, 2);
            this.btnAgregarPago.Location = new System.Drawing.Point(3, 28);
            this.btnAgregarPago.Name = "btnAgregarPago";
            this.btnAgregarPago.Size = new System.Drawing.Size(177, 26);
            this.btnAgregarPago.TabIndex = 6;
            this.btnAgregarPago.Text = "sampleOrder.payment.addPercent";
            this.btnAgregarPago.UseVisualStyleBackColor = true;
            this.btnAgregarPago.Click += new System.EventHandler(this.btnAgregarPago_Click);
            // 
            // btnEliminarPago
            // 
            this.btnEliminarPago.AutoSize = true;
            this.layoutPagos.SetColumnSpan(this.btnEliminarPago, 2);
            this.btnEliminarPago.Location = new System.Drawing.Point(213, 28);
            this.btnEliminarPago.Name = "btnEliminarPago";
            this.btnEliminarPago.Size = new System.Drawing.Size(157, 26);
            this.btnEliminarPago.TabIndex = 7;
            this.btnEliminarPago.Text = "sampleOrder.payment.remove";
            this.btnEliminarPago.UseVisualStyleBackColor = true;
            this.btnEliminarPago.Click += new System.EventHandler(this.btnEliminarPago_Click);
            // 
            // lstPagos
            // 
            this.layoutPagos.SetColumnSpan(this.lstPagos, 2);
            this.lstPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPagos.FormattingEnabled = true;
            this.lstPagos.Location = new System.Drawing.Point(423, 28);
            this.lstPagos.Name = "lstPagos";
            this.lstPagos.Size = new System.Drawing.Size(426, 30);
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
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
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
            this.grpAdjuntos.ResumeLayout(false);
            this.tableAdjuntos.ResumeLayout(false);
            this.tableAdjuntos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdjuntos)).EndInit();
            this.panelAdjuntosAcciones.ResumeLayout(false);
            this.panelAdjuntosAcciones.PerformLayout();
            this.grpPagos.ResumeLayout(false);
            this.layoutPagos.ResumeLayout(false);
            this.layoutPagos.PerformLayout();
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
        private System.Windows.Forms.Label lblNumeroPedido;
        private System.Windows.Forms.Label lblNumeroPedidoValor;
        private System.Windows.Forms.Label lblFechaPedido;
        private System.Windows.Forms.Label lblFechaPedidoValor;
        private System.Windows.Forms.Label lblEstadoPedido;
        private System.Windows.Forms.Label lblEstadoPedidoValor;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.TextBox txtObservaciones;
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
        private System.Windows.Forms.GroupBox grpAdjuntos;
        private System.Windows.Forms.TableLayoutPanel tableAdjuntos;
        private System.Windows.Forms.Label lblAdjuntosInstrucciones;
        private System.Windows.Forms.DataGridView dgvAdjuntos;
        private System.Windows.Forms.FlowLayoutPanel panelAdjuntosAcciones;
        private System.Windows.Forms.Button btnAgregarAdjunto;
        private System.Windows.Forms.Button btnDescargarAdjunto;
        private System.Windows.Forms.Button btnEliminarAdjunto;
        private System.Windows.Forms.GroupBox grpPagos;
        private System.Windows.Forms.TableLayoutPanel layoutPagos;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotalValor;
        private System.Windows.Forms.Label lblPagado;
        private System.Windows.Forms.Label lblPagadoValor;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblSaldoValor;
        private System.Windows.Forms.Button btnAgregarPago;
        private System.Windows.Forms.Button btnEliminarPago;
        private System.Windows.Forms.ListBox lstPagos;
        private System.Windows.Forms.FlowLayoutPanel flowButtons;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}