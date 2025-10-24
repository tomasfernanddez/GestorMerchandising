namespace UI
{
    partial class PedidoForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.tableGeneral = new System.Windows.Forms.TableLayoutPanel();
            this.lblNumeroPedido = new System.Windows.Forms.Label();
            this.txtNumeroPedido = new System.Windows.Forms.TextBox();
            this.lblCliente = new System.Windows.Forms.Label();
            this.cmbCliente = new System.Windows.Forms.ComboBox();
            this.lblTipoPago = new System.Windows.Forms.Label();
            this.cmbTipoPago = new System.Windows.Forms.ComboBox();
            this.lblEstadoPedido = new System.Windows.Forms.Label();
            this.cmbEstadoPedido = new System.Windows.Forms.ComboBox();
            this.lblFechaEntrega = new System.Windows.Forms.Label();
            this.panelFechaEntrega = new System.Windows.Forms.FlowLayoutPanel();
            this.chkFechaEntrega = new System.Windows.Forms.CheckBox();
            this.dtpFechaEntrega = new System.Windows.Forms.DateTimePicker();
            this.lblMontoPagado = new System.Windows.Forms.Label();
            this.nudMontoPagado = new System.Windows.Forms.NumericUpDown();
            this.lblFacturado = new System.Windows.Forms.Label();
            this.panelFactura = new System.Windows.Forms.FlowLayoutPanel();
            this.chkFacturado = new System.Windows.Forms.CheckBox();
            this.txtFactura = new System.Windows.Forms.TextBox();
            this.btnSeleccionarFactura = new System.Windows.Forms.Button();
            this.lblOC = new System.Windows.Forms.Label();
            this.txtOC = new System.Windows.Forms.TextBox();
            this.lblContacto = new System.Windows.Forms.Label();
            this.txtContacto = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.lblDireccionEntrega = new System.Windows.Forms.Label();
            this.txtDireccionEntrega = new System.Windows.Forms.TextBox();
            this.lblNumeroRemito = new System.Windows.Forms.Label();
            this.txtNumeroRemito = new System.Windows.Forms.TextBox();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.tabDetalles = new System.Windows.Forms.TabPage();
            this.tableDetalles = new System.Windows.Forms.TableLayoutPanel();
            this.dgvDetalles = new System.Windows.Forms.DataGridView();
            this.panelDetallesBotones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAgregarDetalle = new System.Windows.Forms.Button();
            this.btnEditarDetalle = new System.Windows.Forms.Button();
            this.btnEliminarDetalle = new System.Windows.Forms.Button();
            this.panelResumen = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotalSinIva = new System.Windows.Forms.Label();
            this.lblTotalSinIvaValor = new System.Windows.Forms.Label();
            this.lblMontoIva = new System.Windows.Forms.Label();
            this.lblMontoIvaValor = new System.Windows.Forms.Label();
            this.lblTotalConIva = new System.Windows.Forms.Label();
            this.lblTotalConIvaValor = new System.Windows.Forms.Label();
            this.lblSaldoPendiente = new System.Windows.Forms.Label();
            this.lblSaldoPendienteValor = new System.Windows.Forms.Label();
            this.tabNotas = new System.Windows.Forms.TabPage();
            this.tableNotas = new System.Windows.Forms.TableLayoutPanel();
            this.gbHistorialEstados = new System.Windows.Forms.GroupBox();
            this.lvHistorialEstados = new System.Windows.Forms.ListView();
            this.columnFecha = new System.Windows.Forms.ColumnHeader();
            this.columnEstado = new System.Windows.Forms.ColumnHeader();
            this.columnComentario = new System.Windows.Forms.ColumnHeader();
            this.gbNotas = new System.Windows.Forms.GroupBox();
            this.tableNotasInternas = new System.Windows.Forms.TableLayoutPanel();
            this.lstNotas = new System.Windows.Forms.ListBox();
            this.panelAgregarNota = new System.Windows.Forms.FlowLayoutPanel();
            this.txtNuevaNota = new System.Windows.Forms.TextBox();
            this.btnAgregarNota = new System.Windows.Forms.Button();
            this.panelAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tableGeneral.SuspendLayout();
            this.panelFechaEntrega.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMontoPagado)).BeginInit();
            this.panelFactura.SuspendLayout();
            this.tabDetalles.SuspendLayout();
            this.tableDetalles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).BeginInit();
            this.panelDetallesBotones.SuspendLayout();
            this.panelResumen.SuspendLayout();
            this.tabNotas.SuspendLayout();
            this.tableNotas.SuspendLayout();
            this.gbHistorialEstados.SuspendLayout();
            this.gbNotas.SuspendLayout();
            this.tableNotasInternas.SuspendLayout();
            this.panelAgregarNota.SuspendLayout();
            this.panelAcciones.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabDetalles);
            this.tabControl.Controls.Add(this.tabNotas);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(960, 600);
            this.tabControl.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.tableGeneral);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(8);
            this.tabGeneral.Size = new System.Drawing.Size(952, 574);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "Datos Generales";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // tableGeneral
            // 
            this.tableGeneral.ColumnCount = 2;
            this.tableGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableGeneral.Controls.Add(this.lblNumeroPedido, 0, 0);
            this.tableGeneral.Controls.Add(this.txtNumeroPedido, 1, 0);
            this.tableGeneral.Controls.Add(this.lblCliente, 0, 1);
            this.tableGeneral.Controls.Add(this.cmbCliente, 1, 1);
            this.tableGeneral.Controls.Add(this.lblTipoPago, 0, 2);
            this.tableGeneral.Controls.Add(this.cmbTipoPago, 1, 2);
            this.tableGeneral.Controls.Add(this.lblEstadoPedido, 0, 3);
            this.tableGeneral.Controls.Add(this.cmbEstadoPedido, 1, 3);
            this.tableGeneral.Controls.Add(this.lblFechaEntrega, 0, 4);
            this.tableGeneral.Controls.Add(this.panelFechaEntrega, 1, 4);
            this.tableGeneral.Controls.Add(this.lblMontoPagado, 0, 5);
            this.tableGeneral.Controls.Add(this.nudMontoPagado, 1, 5);
            this.tableGeneral.Controls.Add(this.lblFacturado, 0, 6);
            this.tableGeneral.Controls.Add(this.panelFactura, 1, 6);
            this.tableGeneral.Controls.Add(this.lblOC, 0, 7);
            this.tableGeneral.Controls.Add(this.txtOC, 1, 7);
            this.tableGeneral.Controls.Add(this.lblContacto, 0, 8);
            this.tableGeneral.Controls.Add(this.txtContacto, 1, 8);
            this.tableGeneral.Controls.Add(this.lblEmail, 0, 9);
            this.tableGeneral.Controls.Add(this.txtEmail, 1, 9);
            this.tableGeneral.Controls.Add(this.lblTelefono, 0, 10);
            this.tableGeneral.Controls.Add(this.txtTelefono, 1, 10);
            this.tableGeneral.Controls.Add(this.lblDireccionEntrega, 0, 11);
            this.tableGeneral.Controls.Add(this.txtDireccionEntrega, 1, 11);
            this.tableGeneral.Controls.Add(this.lblNumeroRemito, 0, 12);
            this.tableGeneral.Controls.Add(this.txtNumeroRemito, 1, 12);
            this.tableGeneral.Controls.Add(this.lblObservaciones, 0, 13);
            this.tableGeneral.Controls.Add(this.txtObservaciones, 1, 13);
            this.tableGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableGeneral.Location = new System.Drawing.Point(8, 8);
            this.tableGeneral.Name = "tableGeneral";
            this.tableGeneral.RowCount = 14;
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableGeneral.Size = new System.Drawing.Size(936, 558);
            this.tableGeneral.TabIndex = 0;
            // 
            // lblNumeroPedido
            // 
            this.lblNumeroPedido.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNumeroPedido.AutoSize = true;
            this.lblNumeroPedido.Location = new System.Drawing.Point(3, 7);
            this.lblNumeroPedido.Name = "lblNumeroPedido";
            this.lblNumeroPedido.Size = new System.Drawing.Size(73, 13);
            this.lblNumeroPedido.TabIndex = 0;
            this.lblNumeroPedido.Text = "Número Pedido";
            // 
            // txtNumeroPedido
            // 
            this.txtNumeroPedido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNumeroPedido.Location = new System.Drawing.Point(283, 3);
            this.txtNumeroPedido.Name = "txtNumeroPedido";
            this.txtNumeroPedido.ReadOnly = true;
            this.txtNumeroPedido.Size = new System.Drawing.Size(650, 20);
            this.txtNumeroPedido.TabIndex = 1;
            // 
            // lblCliente
            // 
            this.lblCliente.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCliente.AutoSize = true;
            this.lblCliente.Location = new System.Drawing.Point(3, 35);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(39, 13);
            this.lblCliente.TabIndex = 2;
            this.lblCliente.Text = "Cliente";
            // 
            // cmbCliente
            // 
            this.cmbCliente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCliente.FormattingEnabled = true;
            this.cmbCliente.Location = new System.Drawing.Point(283, 31);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(650, 21);
            this.cmbCliente.TabIndex = 3;
            // 
            // lblTipoPago
            // 
            this.lblTipoPago.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTipoPago.AutoSize = true;
            this.lblTipoPago.Location = new System.Drawing.Point(3, 63);
            this.lblTipoPago.Name = "lblTipoPago";
            this.lblTipoPago.Size = new System.Drawing.Size(66, 13);
            this.lblTipoPago.TabIndex = 4;
            this.lblTipoPago.Text = "Tipo de pago";
            // 
            // cmbTipoPago
            // 
            this.cmbTipoPago.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTipoPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoPago.FormattingEnabled = true;
            this.cmbTipoPago.Location = new System.Drawing.Point(283, 59);
            this.cmbTipoPago.Name = "cmbTipoPago";
            this.cmbTipoPago.Size = new System.Drawing.Size(650, 21);
            this.cmbTipoPago.TabIndex = 5;
            // 
            // lblEstadoPedido
            // 
            this.lblEstadoPedido.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEstadoPedido.AutoSize = true;
            this.lblEstadoPedido.Location = new System.Drawing.Point(3, 91);
            this.lblEstadoPedido.Name = "lblEstadoPedido";
            this.lblEstadoPedido.Size = new System.Drawing.Size(40, 13);
            this.lblEstadoPedido.TabIndex = 6;
            this.lblEstadoPedido.Text = "Estado";
            // 
            // cmbEstadoPedido
            // 
            this.cmbEstadoPedido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbEstadoPedido.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstadoPedido.FormattingEnabled = true;
            this.cmbEstadoPedido.Location = new System.Drawing.Point(283, 87);
            this.cmbEstadoPedido.Name = "cmbEstadoPedido";
            this.cmbEstadoPedido.Size = new System.Drawing.Size(650, 21);
            this.cmbEstadoPedido.TabIndex = 7;
            // 
            // lblFechaEntrega
            // 
            this.lblFechaEntrega.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFechaEntrega.AutoSize = true;
            this.lblFechaEntrega.Location = new System.Drawing.Point(3, 122);
            this.lblFechaEntrega.Name = "lblFechaEntrega";
            this.lblFechaEntrega.Size = new System.Drawing.Size(91, 13);
            this.lblFechaEntrega.TabIndex = 8;
            this.lblFechaEntrega.Text = "Fecha de entrega";
            // 
            // panelFechaEntrega
            // 
            this.panelFechaEntrega.AutoSize = true;
            this.panelFechaEntrega.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelFechaEntrega.Controls.Add(this.chkFechaEntrega);
            this.panelFechaEntrega.Controls.Add(this.dtpFechaEntrega);
            this.panelFechaEntrega.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFechaEntrega.Location = new System.Drawing.Point(283, 115);
            this.panelFechaEntrega.Name = "panelFechaEntrega";
            this.panelFechaEntrega.Size = new System.Drawing.Size(650, 26);
            this.panelFechaEntrega.TabIndex = 9;
            this.panelFechaEntrega.WrapContents = false;
            // 
            // chkFechaEntrega
            // 
            this.chkFechaEntrega.AutoSize = true;
            this.chkFechaEntrega.Location = new System.Drawing.Point(3, 3);
            this.chkFechaEntrega.Name = "chkFechaEntrega";
            this.chkFechaEntrega.Size = new System.Drawing.Size(68, 17);
            this.chkFechaEntrega.TabIndex = 0;
            this.chkFechaEntrega.Text = "Habilitar";
            this.chkFechaEntrega.UseVisualStyleBackColor = true;
            this.chkFechaEntrega.CheckedChanged += new System.EventHandler(this.chkFechaEntrega_CheckedChanged);
            // 
            // dtpFechaEntrega
            // 
            this.dtpFechaEntrega.Enabled = false;
            this.dtpFechaEntrega.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaEntrega.Location = new System.Drawing.Point(77, 3);
            this.dtpFechaEntrega.Name = "dtpFechaEntrega";
            this.dtpFechaEntrega.Size = new System.Drawing.Size(140, 20);
            this.dtpFechaEntrega.TabIndex = 1;
            // 
            // lblMontoPagado
            // 
            this.lblMontoPagado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMontoPagado.AutoSize = true;
            this.lblMontoPagado.Location = new System.Drawing.Point(3, 151);
            this.lblMontoPagado.Name = "lblMontoPagado";
            this.lblMontoPagado.Size = new System.Drawing.Size(75, 13);
            this.lblMontoPagado.TabIndex = 10;
            this.lblMontoPagado.Text = "Monto pagado";
            // 
            // nudMontoPagado
            // 
            this.nudMontoPagado.DecimalPlaces = 2;
            this.nudMontoPagado.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudMontoPagado.Location = new System.Drawing.Point(283, 147);
            this.nudMontoPagado.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudMontoPagado.Name = "nudMontoPagado";
            this.nudMontoPagado.Size = new System.Drawing.Size(160, 20);
            this.nudMontoPagado.TabIndex = 11;
            this.nudMontoPagado.ThousandsSeparator = true;
            // 
            // lblFacturado
            // 
            this.lblFacturado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFacturado.AutoSize = true;
            this.lblFacturado.Location = new System.Drawing.Point(3, 183);
            this.lblFacturado.Name = "lblFacturado";
            this.lblFacturado.Size = new System.Drawing.Size(54, 13);
            this.lblFacturado.TabIndex = 12;
            this.lblFacturado.Text = "Facturado";
            // 
            // panelFactura
            // 
            this.panelFactura.AutoSize = true;
            this.panelFactura.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelFactura.Controls.Add(this.chkFacturado);
            this.panelFactura.Controls.Add(this.txtFactura);
            this.panelFactura.Controls.Add(this.btnSeleccionarFactura);
            this.panelFactura.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFactura.Location = new System.Drawing.Point(283, 179);
            this.panelFactura.Name = "panelFactura";
            this.panelFactura.Size = new System.Drawing.Size(650, 26);
            this.panelFactura.TabIndex = 13;
            this.panelFactura.WrapContents = false;
            // 
            // chkFacturado
            // 
            this.chkFacturado.AutoSize = true;
            this.chkFacturado.Location = new System.Drawing.Point(3, 3);
            this.chkFacturado.Name = "chkFacturado";
            this.chkFacturado.Size = new System.Drawing.Size(70, 17);
            this.chkFacturado.TabIndex = 0;
            this.chkFacturado.Text = "Facturado";
            this.chkFacturado.UseVisualStyleBackColor = true;
            this.chkFacturado.CheckedChanged += new System.EventHandler(this.chkFacturado_CheckedChanged);
            // 
            // txtFactura
            // 
            this.txtFactura.Enabled = false;
            this.txtFactura.Location = new System.Drawing.Point(79, 3);
            this.txtFactura.Name = "txtFactura";
            this.txtFactura.Size = new System.Drawing.Size(320, 20);
            this.txtFactura.TabIndex = 1;
            // 
            // btnSeleccionarFactura
            // 
            this.btnSeleccionarFactura.Enabled = false;
            this.btnSeleccionarFactura.Location = new System.Drawing.Point(405, 3);
            this.btnSeleccionarFactura.Name = "btnSeleccionarFactura";
            this.btnSeleccionarFactura.Size = new System.Drawing.Size(110, 23);
            this.btnSeleccionarFactura.TabIndex = 2;
            this.btnSeleccionarFactura.Text = "Seleccionar";
            this.btnSeleccionarFactura.UseVisualStyleBackColor = true;
            this.btnSeleccionarFactura.Click += new System.EventHandler(this.btnSeleccionarFactura_Click);
            // 
            // lblOC
            // 
            this.lblOC.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblOC.AutoSize = true;
            this.lblOC.Location = new System.Drawing.Point(3, 215);
            this.lblOC.Name = "lblOC";
            this.lblOC.Size = new System.Drawing.Size(25, 13);
            this.lblOC.TabIndex = 14;
            this.lblOC.Text = "OC";
            // 
            // txtOC
            // 
            this.txtOC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOC.Location = new System.Drawing.Point(283, 211);
            this.txtOC.Name = "txtOC";
            this.txtOC.Size = new System.Drawing.Size(650, 20);
            this.txtOC.TabIndex = 15;
            // 
            // lblContacto
            // 
            this.lblContacto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblContacto.AutoSize = true;
            this.lblContacto.Location = new System.Drawing.Point(3, 243);
            this.lblContacto.Name = "lblContacto";
            this.lblContacto.Size = new System.Drawing.Size(50, 13);
            this.lblContacto.TabIndex = 16;
            this.lblContacto.Text = "Contacto";
            // 
            // txtContacto
            // 
            this.txtContacto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContacto.Location = new System.Drawing.Point(283, 239);
            this.txtContacto.Name = "txtContacto";
            this.txtContacto.Size = new System.Drawing.Size(650, 20);
            this.txtContacto.TabIndex = 17;
            // 
            // lblEmail
            // 
            this.lblEmail.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(3, 271);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(35, 13);
            this.lblEmail.TabIndex = 18;
            this.lblEmail.Text = "E-mail";
            // 
            // txtEmail
            // 
            this.txtEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEmail.Location = new System.Drawing.Point(283, 267);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(650, 20);
            this.txtEmail.TabIndex = 19;
            // 
            // lblTelefono
            // 
            this.lblTelefono.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Location = new System.Drawing.Point(3, 299);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(49, 13);
            this.lblTelefono.TabIndex = 20;
            this.lblTelefono.Text = "Teléfono";
            // 
            // txtTelefono
            // 
            this.txtTelefono.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTelefono.Location = new System.Drawing.Point(283, 295);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(650, 20);
            this.txtTelefono.TabIndex = 21;
            // 
            // lblDireccionEntrega
            // 
            this.lblDireccionEntrega.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDireccionEntrega.AutoSize = true;
            this.lblDireccionEntrega.Location = new System.Drawing.Point(3, 327);
            this.lblDireccionEntrega.Name = "lblDireccionEntrega";
            this.lblDireccionEntrega.Size = new System.Drawing.Size(104, 13);
            this.lblDireccionEntrega.TabIndex = 22;
            this.lblDireccionEntrega.Text = "Dirección de entrega";
            // 
            // txtDireccionEntrega
            // 
            this.txtDireccionEntrega.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDireccionEntrega.Location = new System.Drawing.Point(283, 323);
            this.txtDireccionEntrega.Name = "txtDireccionEntrega";
            this.txtDireccionEntrega.Size = new System.Drawing.Size(650, 20);
            this.txtDireccionEntrega.TabIndex = 23;
            // 
            // lblNumeroRemito
            // 
            this.lblNumeroRemito.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNumeroRemito.AutoSize = true;
            this.lblNumeroRemito.Location = new System.Drawing.Point(3, 355);
            this.lblNumeroRemito.Name = "lblNumeroRemito";
            this.lblNumeroRemito.Size = new System.Drawing.Size(87, 13);
            this.lblNumeroRemito.TabIndex = 24;
            this.lblNumeroRemito.Text = "Números remito";
            // 
            // txtNumeroRemito
            // 
            this.txtNumeroRemito.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNumeroRemito.Location = new System.Drawing.Point(283, 351);
            this.txtNumeroRemito.Name = "txtNumeroRemito";
            this.txtNumeroRemito.Size = new System.Drawing.Size(650, 20);
            this.txtNumeroRemito.TabIndex = 25;
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Location = new System.Drawing.Point(3, 422);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(78, 13);
            this.lblObservaciones.TabIndex = 26;
            this.lblObservaciones.Text = "Observaciones";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtObservaciones.Location = new System.Drawing.Point(283, 379);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservaciones.Size = new System.Drawing.Size(650, 176);
            this.txtObservaciones.TabIndex = 27;
            // 
            // tabDetalles
            // 
            this.tabDetalles.Controls.Add(this.tableDetalles);
            this.tabDetalles.Location = new System.Drawing.Point(4, 22);
            this.tabDetalles.Name = "tabDetalles";
            this.tabDetalles.Padding = new System.Windows.Forms.Padding(8);
            this.tabDetalles.Size = new System.Drawing.Size(952, 574);
            this.tabDetalles.TabIndex = 1;
            this.tabDetalles.Text = "Productos";
            this.tabDetalles.UseVisualStyleBackColor = true;
            // 
            // tableDetalles
            // 
            this.tableDetalles.ColumnCount = 1;
            this.tableDetalles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDetalles.Controls.Add(this.dgvDetalles, 0, 0);
            this.tableDetalles.Controls.Add(this.panelDetallesBotones, 0, 1);
            this.tableDetalles.Controls.Add(this.panelResumen, 0, 2);
            this.tableDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableDetalles.Location = new System.Drawing.Point(8, 8);
            this.tableDetalles.Name = "tableDetalles";
            this.tableDetalles.RowCount = 3;
            this.tableDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableDetalles.Size = new System.Drawing.Size(936, 558);
            this.tableDetalles.TabIndex = 0;
            // 
            // dgvDetalles
            // 
            this.dgvDetalles.AllowUserToAddRows = false;
            this.dgvDetalles.AllowUserToDeleteRows = false;
            this.dgvDetalles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetalles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetalles.Location = new System.Drawing.Point(3, 3);
            this.dgvDetalles.MultiSelect = false;
            this.dgvDetalles.Name = "dgvDetalles";
            this.dgvDetalles.ReadOnly = true;
            this.dgvDetalles.RowHeadersVisible = false;
            this.dgvDetalles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalles.Size = new System.Drawing.Size(930, 442);
            this.dgvDetalles.TabIndex = 0;
            // 
            // panelDetallesBotones
            // 
            this.panelDetallesBotones.AutoSize = true;
            this.panelDetallesBotones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelDetallesBotones.Controls.Add(this.btnAgregarDetalle);
            this.panelDetallesBotones.Controls.Add(this.btnEditarDetalle);
            this.panelDetallesBotones.Controls.Add(this.btnEliminarDetalle);
            this.panelDetallesBotones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetallesBotones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelDetallesBotones.Location = new System.Drawing.Point(3, 451);
            this.panelDetallesBotones.Name = "panelDetallesBotones";
            this.panelDetallesBotones.Size = new System.Drawing.Size(930, 34);
            this.panelDetallesBotones.TabIndex = 1;
            // 
            // btnAgregarDetalle
            // 
            this.btnAgregarDetalle.AutoSize = true;
            this.btnAgregarDetalle.Location = new System.Drawing.Point(832, 3);
            this.btnAgregarDetalle.Name = "btnAgregarDetalle";
            this.btnAgregarDetalle.Size = new System.Drawing.Size(95, 23);
            this.btnAgregarDetalle.TabIndex = 0;
            this.btnAgregarDetalle.Text = "Agregar";
            this.btnAgregarDetalle.UseVisualStyleBackColor = true;
            this.btnAgregarDetalle.Click += new System.EventHandler(this.btnAgregarDetalle_Click);
            // 
            // btnEditarDetalle
            // 
            this.btnEditarDetalle.AutoSize = true;
            this.btnEditarDetalle.Location = new System.Drawing.Point(731, 3);
            this.btnEditarDetalle.Name = "btnEditarDetalle";
            this.btnEditarDetalle.Size = new System.Drawing.Size(95, 23);
            this.btnEditarDetalle.TabIndex = 1;
            this.btnEditarDetalle.Text = "Editar";
            this.btnEditarDetalle.UseVisualStyleBackColor = true;
            this.btnEditarDetalle.Click += new System.EventHandler(this.btnEditarDetalle_Click);
            // 
            // btnEliminarDetalle
            // 
            this.btnEliminarDetalle.AutoSize = true;
            this.btnEliminarDetalle.Location = new System.Drawing.Point(630, 3);
            this.btnEliminarDetalle.Name = "btnEliminarDetalle";
            this.btnEliminarDetalle.Size = new System.Drawing.Size(95, 23);
            this.btnEliminarDetalle.TabIndex = 2;
            this.btnEliminarDetalle.Text = "Eliminar";
            this.btnEliminarDetalle.UseVisualStyleBackColor = true;
            this.btnEliminarDetalle.Click += new System.EventHandler(this.btnEliminarDetalle_Click);
            // 
            // panelResumen
            // 
            this.panelResumen.ColumnCount = 4;
            this.panelResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.panelResumen.Controls.Add(this.lblTotalSinIva, 0, 0);
            this.panelResumen.Controls.Add(this.lblTotalSinIvaValor, 0, 1);
            this.panelResumen.Controls.Add(this.lblMontoIva, 1, 0);
            this.panelResumen.Controls.Add(this.lblMontoIvaValor, 1, 1);
            this.panelResumen.Controls.Add(this.lblTotalConIva, 2, 0);
            this.panelResumen.Controls.Add(this.lblTotalConIvaValor, 2, 1);
            this.panelResumen.Controls.Add(this.lblSaldoPendiente, 3, 0);
            this.panelResumen.Controls.Add(this.lblSaldoPendienteValor, 3, 1);
            this.panelResumen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelResumen.Location = new System.Drawing.Point(3, 491);
            this.panelResumen.Name = "panelResumen";
            this.panelResumen.RowCount = 2;
            this.panelResumen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.panelResumen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.panelResumen.Size = new System.Drawing.Size(930, 64);
            this.panelResumen.TabIndex = 2;
            // 
            // lblTotalSinIva
            // 
            this.lblTotalSinIva.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblTotalSinIva.AutoSize = true;
            this.lblTotalSinIva.Location = new System.Drawing.Point(101, 12);
            this.lblTotalSinIva.Name = "lblTotalSinIva";
            this.lblTotalSinIva.Size = new System.Drawing.Size(120, 13);
            this.lblTotalSinIva.TabIndex = 0;
            this.lblTotalSinIva.Text = "Total sin IVA";
            // 
            // lblTotalSinIvaValor
            // 
            this.lblTotalSinIvaValor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTotalSinIvaValor.AutoSize = true;
            this.lblTotalSinIvaValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalSinIvaValor.Location = new System.Drawing.Point(141, 25);
            this.lblTotalSinIvaValor.Name = "lblTotalSinIvaValor";
            this.lblTotalSinIvaValor.Size = new System.Drawing.Size(41, 17);
            this.lblTotalSinIvaValor.TabIndex = 1;
            this.lblTotalSinIvaValor.Text = "$0,0";
            // 
            // lblMontoIva
            // 
            this.lblMontoIva.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblMontoIva.AutoSize = true;
            this.lblMontoIva.Location = new System.Drawing.Point(333, 12);
            this.lblMontoIva.Name = "lblMontoIva";
            this.lblMontoIva.Size = new System.Drawing.Size(86, 13);
            this.lblMontoIva.TabIndex = 2;
            this.lblMontoIva.Text = "Monto IVA";
            // 
            // lblMontoIvaValor
            // 
            this.lblMontoIvaValor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMontoIvaValor.AutoSize = true;
            this.lblMontoIvaValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblMontoIvaValor.Location = new System.Drawing.Point(373, 25);
            this.lblMontoIvaValor.Name = "lblMontoIvaValor";
            this.lblMontoIvaValor.Size = new System.Drawing.Size(41, 17);
            this.lblMontoIvaValor.TabIndex = 3;
            this.lblMontoIvaValor.Text = "$0,0";
            // 
            // lblTotalConIva
            // 
            this.lblTotalConIva.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblTotalConIva.AutoSize = true;
            this.lblTotalConIva.Location = new System.Drawing.Point(558, 12);
            this.lblTotalConIva.Name = "lblTotalConIva";
            this.lblTotalConIva.Size = new System.Drawing.Size(104, 13);
            this.lblTotalConIva.TabIndex = 4;
            this.lblTotalConIva.Text = "Total con IVA";
            // 
            // lblTotalConIvaValor
            // 
            this.lblTotalConIvaValor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTotalConIvaValor.AutoSize = true;
            this.lblTotalConIvaValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalConIvaValor.Location = new System.Drawing.Point(609, 25);
            this.lblTotalConIvaValor.Name = "lblTotalConIvaValor";
            this.lblTotalConIvaValor.Size = new System.Drawing.Size(41, 17);
            this.lblTotalConIvaValor.TabIndex = 5;
            this.lblTotalConIvaValor.Text = "$0,0";
            // 
            // lblSaldoPendiente
            // 
            this.lblSaldoPendiente.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblSaldoPendiente.AutoSize = true;
            this.lblSaldoPendiente.Location = new System.Drawing.Point(796, 12);
            this.lblSaldoPendiente.Name = "lblSaldoPendiente";
            this.lblSaldoPendiente.Size = new System.Drawing.Size(108, 13);
            this.lblSaldoPendiente.TabIndex = 6;
            this.lblSaldoPendiente.Text = "Saldo pendiente";
            // 
            // lblSaldoPendienteValor
            // 
            this.lblSaldoPendienteValor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblSaldoPendienteValor.AutoSize = true;
            this.lblSaldoPendienteValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblSaldoPendienteValor.Location = new System.Drawing.Point(847, 25);
            this.lblSaldoPendienteValor.Name = "lblSaldoPendienteValor";
            this.lblSaldoPendienteValor.Size = new System.Drawing.Size(41, 17);
            this.lblSaldoPendienteValor.TabIndex = 7;
            this.lblSaldoPendienteValor.Text = "$0,0";
            // 
            // tabNotas
            // 
            this.tabNotas.Controls.Add(this.tableNotas);
            this.tabNotas.Location = new System.Drawing.Point(4, 22);
            this.tabNotas.Name = "tabNotas";
            this.tabNotas.Padding = new System.Windows.Forms.Padding(8);
            this.tabNotas.Size = new System.Drawing.Size(952, 574);
            this.tabNotas.TabIndex = 2;
            this.tabNotas.Text = "Seguimiento";
            this.tabNotas.UseVisualStyleBackColor = true;
            // 
            // tableNotas
            // 
            this.tableNotas.ColumnCount = 1;
            this.tableNotas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableNotas.Controls.Add(this.gbHistorialEstados, 0, 0);
            this.tableNotas.Controls.Add(this.gbNotas, 0, 1);
            this.tableNotas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableNotas.Location = new System.Drawing.Point(8, 8);
            this.tableNotas.Name = "tableNotas";
            this.tableNotas.RowCount = 2;
            this.tableNotas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableNotas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableNotas.Size = new System.Drawing.Size(936, 558);
            this.tableNotas.TabIndex = 0;
            // 
            // gbHistorialEstados
            // 
            this.gbHistorialEstados.Controls.Add(this.lvHistorialEstados);
            this.gbHistorialEstados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbHistorialEstados.Location = new System.Drawing.Point(3, 3);
            this.gbHistorialEstados.Name = "gbHistorialEstados";
            this.gbHistorialEstados.Padding = new System.Windows.Forms.Padding(10, 6, 10, 10);
            this.gbHistorialEstados.Size = new System.Drawing.Size(930, 273);
            this.gbHistorialEstados.TabIndex = 0;
            this.gbHistorialEstados.TabStop = false;
            this.gbHistorialEstados.Text = "Historial de estados";
            // 
            // lvHistorialEstados
            // 
            this.lvHistorialEstados.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFecha,
            this.columnEstado,
            this.columnComentario});
            this.lvHistorialEstados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvHistorialEstados.FullRowSelect = true;
            this.lvHistorialEstados.HideSelection = false;
            this.lvHistorialEstados.Location = new System.Drawing.Point(10, 19);
            this.lvHistorialEstados.Name = "lvHistorialEstados";
            this.lvHistorialEstados.Size = new System.Drawing.Size(910, 244);
            this.lvHistorialEstados.TabIndex = 0;
            this.lvHistorialEstados.UseCompatibleStateImageBehavior = false;
            this.lvHistorialEstados.View = System.Windows.Forms.View.Details;
            // 
            // columnFecha
            // 
            this.columnFecha.Text = "Fecha";
            this.columnFecha.Width = 150;
            // 
            // columnEstado
            // 
            this.columnEstado.Text = "Estado";
            this.columnEstado.Width = 200;
            // 
            // columnComentario
            // 
            this.columnComentario.Text = "Comentario";
            this.columnComentario.Width = 520;
            // 
            // gbNotas
            // 
            this.gbNotas.Controls.Add(this.tableNotasInternas);
            this.gbNotas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbNotas.Location = new System.Drawing.Point(3, 282);
            this.gbNotas.Name = "gbNotas";
            this.gbNotas.Padding = new System.Windows.Forms.Padding(10, 6, 10, 10);
            this.gbNotas.Size = new System.Drawing.Size(930, 273);
            this.gbNotas.TabIndex = 1;
            this.gbNotas.TabStop = false;
            this.gbNotas.Text = "Notas internas";
            // 
            // tableNotasInternas
            // 
            this.tableNotasInternas.ColumnCount = 1;
            this.tableNotasInternas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableNotasInternas.Controls.Add(this.lstNotas, 0, 0);
            this.tableNotasInternas.Controls.Add(this.panelAgregarNota, 0, 1);
            this.tableNotasInternas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableNotasInternas.Location = new System.Drawing.Point(10, 19);
            this.tableNotasInternas.Name = "tableNotasInternas";
            this.tableNotasInternas.RowCount = 2;
            this.tableNotasInternas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableNotasInternas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableNotasInternas.Size = new System.Drawing.Size(910, 244);
            this.tableNotasInternas.TabIndex = 0;
            // 
            // lstNotas
            // 
            this.lstNotas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstNotas.FormattingEnabled = true;
            this.lstNotas.Location = new System.Drawing.Point(3, 3);
            this.lstNotas.Name = "lstNotas";
            this.lstNotas.Size = new System.Drawing.Size(904, 198);
            this.lstNotas.TabIndex = 0;
            // 
            // panelAgregarNota
            // 
            this.panelAgregarNota.AutoSize = true;
            this.panelAgregarNota.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelAgregarNota.Controls.Add(this.txtNuevaNota);
            this.panelAgregarNota.Controls.Add(this.btnAgregarNota);
            this.panelAgregarNota.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAgregarNota.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.panelAgregarNota.Location = new System.Drawing.Point(3, 207);
            this.panelAgregarNota.Name = "panelAgregarNota";
            this.panelAgregarNota.Size = new System.Drawing.Size(904, 34);
            this.panelAgregarNota.TabIndex = 1;
            // 
            // txtNuevaNota
            // 
            this.txtNuevaNota.Location = new System.Drawing.Point(3, 3);
            this.txtNuevaNota.Multiline = true;
            this.txtNuevaNota.Name = "txtNuevaNota";
            this.txtNuevaNota.Size = new System.Drawing.Size(760, 28);
            this.txtNuevaNota.TabIndex = 0;
            // 
            // btnAgregarNota
            // 
            this.btnAgregarNota.AutoSize = true;
            this.btnAgregarNota.Location = new System.Drawing.Point(769, 3);
            this.btnAgregarNota.Name = "btnAgregarNota";
            this.btnAgregarNota.Size = new System.Drawing.Size(132, 23);
            this.btnAgregarNota.TabIndex = 1;
            this.btnAgregarNota.Text = "Agregar nota";
            this.btnAgregarNota.UseVisualStyleBackColor = true;
            this.btnAgregarNota.Click += new System.EventHandler(this.btnAgregarNota_Click);
            // 
            // panelAcciones
            // 
            this.panelAcciones.AutoSize = true;
            this.panelAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelAcciones.Controls.Add(this.btnGuardar);
            this.panelAcciones.Controls.Add(this.btnCancelar);
            this.panelAcciones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAcciones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelAcciones.Location = new System.Drawing.Point(12, 612);
            this.panelAcciones.Name = "panelAcciones";
            this.panelAcciones.Size = new System.Drawing.Size(960, 37);
            this.panelAcciones.TabIndex = 1;
            // 
            // btnGuardar
            // 
            this.btnGuardar.AutoSize = true;
            this.btnGuardar.Location = new System.Drawing.Point(862, 3);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(95, 31);
            this.btnGuardar.TabIndex = 0;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.AutoSize = true;
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(761, 3);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(95, 31);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // PedidoForm
            // 
            this.AcceptButton = this.btnGuardar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelAcciones);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PedidoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pedido";
            this.Load += new System.EventHandler(this.PedidoForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tableGeneral.ResumeLayout(false);
            this.tableGeneral.PerformLayout();
            this.panelFechaEntrega.ResumeLayout(false);
            this.panelFechaEntrega.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMontoPagado)).EndInit();
            this.panelFactura.ResumeLayout(false);
            this.panelFactura.PerformLayout();
            this.tabDetalles.ResumeLayout(false);
            this.tableDetalles.ResumeLayout(false);
            this.tableDetalles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).EndInit();
            this.panelDetallesBotones.ResumeLayout(false);
            this.panelDetallesBotones.PerformLayout();
            this.panelResumen.ResumeLayout(false);
            this.panelResumen.PerformLayout();
            this.tabNotas.ResumeLayout(false);
            this.tableNotas.ResumeLayout(false);
            this.gbHistorialEstados.ResumeLayout(false);
            this.gbNotas.ResumeLayout(false);
            this.tableNotasInternas.ResumeLayout(false);
            this.tableNotasInternas.PerformLayout();
            this.panelAgregarNota.ResumeLayout(false);
            this.panelAgregarNota.PerformLayout();
            this.panelAcciones.ResumeLayout(false);
            this.panelAcciones.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabDetalles;
        private System.Windows.Forms.TableLayoutPanel tableGeneral;
        private System.Windows.Forms.Label lblNumeroPedido;
        private System.Windows.Forms.TextBox txtNumeroPedido;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.ComboBox cmbCliente;
        private System.Windows.Forms.Label lblTipoPago;
        private System.Windows.Forms.ComboBox cmbTipoPago;
        private System.Windows.Forms.Label lblEstadoPedido;
        private System.Windows.Forms.ComboBox cmbEstadoPedido;
        private System.Windows.Forms.Label lblFechaEntrega;
        private System.Windows.Forms.FlowLayoutPanel panelFechaEntrega;
        private System.Windows.Forms.CheckBox chkFechaEntrega;
        private System.Windows.Forms.DateTimePicker dtpFechaEntrega;
        private System.Windows.Forms.Label lblMontoPagado;
        private System.Windows.Forms.NumericUpDown nudMontoPagado;
        private System.Windows.Forms.Label lblFacturado;
        private System.Windows.Forms.FlowLayoutPanel panelFactura;
        private System.Windows.Forms.CheckBox chkFacturado;
        private System.Windows.Forms.TextBox txtFactura;
        private System.Windows.Forms.Button btnSeleccionarFactura;
        private System.Windows.Forms.Label lblOC;
        private System.Windows.Forms.TextBox txtOC;
        private System.Windows.Forms.Label lblContacto;
        private System.Windows.Forms.TextBox txtContacto;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.Label lblDireccionEntrega;
        private System.Windows.Forms.TextBox txtDireccionEntrega;
        private System.Windows.Forms.Label lblNumeroRemito;
        private System.Windows.Forms.TextBox txtNumeroRemito;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.TableLayoutPanel tableDetalles;
        private System.Windows.Forms.DataGridView dgvDetalles;
        private System.Windows.Forms.FlowLayoutPanel panelDetallesBotones;
        private System.Windows.Forms.Button btnAgregarDetalle;
        private System.Windows.Forms.Button btnEditarDetalle;
        private System.Windows.Forms.Button btnEliminarDetalle;
        private System.Windows.Forms.TableLayoutPanel panelResumen;
        private System.Windows.Forms.Label lblTotalSinIva;
        private System.Windows.Forms.Label lblTotalSinIvaValor;
        private System.Windows.Forms.Label lblMontoIva;
        private System.Windows.Forms.Label lblMontoIvaValor;
        private System.Windows.Forms.Label lblTotalConIva;
        private System.Windows.Forms.Label lblTotalConIvaValor;
        private System.Windows.Forms.Label lblSaldoPendiente;
        private System.Windows.Forms.Label lblSaldoPendienteValor;
        private System.Windows.Forms.TabPage tabNotas;
        private System.Windows.Forms.TableLayoutPanel tableNotas;
        private System.Windows.Forms.GroupBox gbHistorialEstados;
        private System.Windows.Forms.ListView lvHistorialEstados;
        private System.Windows.Forms.ColumnHeader columnFecha;
        private System.Windows.Forms.ColumnHeader columnEstado;
        private System.Windows.Forms.ColumnHeader columnComentario;
        private System.Windows.Forms.GroupBox gbNotas;
        private System.Windows.Forms.TableLayoutPanel tableNotasInternas;
        private System.Windows.Forms.ListBox lstNotas;
        private System.Windows.Forms.FlowLayoutPanel panelAgregarNota;
        private System.Windows.Forms.TextBox txtNuevaNota;
        private System.Windows.Forms.Button btnAgregarNota;
        private System.Windows.Forms.FlowLayoutPanel panelAcciones;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}