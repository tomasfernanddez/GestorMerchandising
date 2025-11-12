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
            this.layoutContenido = new System.Windows.Forms.TableLayoutPanel();
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
            this.grpDetalles = new System.Windows.Forms.GroupBox();
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
            this.tablePagos = new System.Windows.Forms.TableLayoutPanel();
            this.flowPagoResumen = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMontoPagado = new System.Windows.Forms.Label();
            this.lblMontoPagadoValor = new System.Windows.Forms.Label();
            this.lstPagos = new System.Windows.Forms.ListBox();
            this.flowPagoAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAgregarPago = new System.Windows.Forms.Button();
            this.btnDeshacerPago = new System.Windows.Forms.Button();
            this.grpFacturas = new System.Windows.Forms.GroupBox();
            this.tableAdjuntos = new System.Windows.Forms.TableLayoutPanel();
            this.lblAdjuntosInstrucciones = new System.Windows.Forms.Label();
            this.dgvAdjuntos = new System.Windows.Forms.DataGridView();
            this.panelAdjuntosAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAgregarAdjunto = new System.Windows.Forms.Button();
            this.btnDescargarAdjunto = new System.Windows.Forms.Button();
            this.btnEliminarAdjunto = new System.Windows.Forms.Button();
            this.gbHistorialEstados = new System.Windows.Forms.GroupBox();
            this.lvHistorialEstados = new System.Windows.Forms.ListView();
            this.columnFecha = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEstado = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnComentario = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnCancelarPedido = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.layoutContenido.SuspendLayout();
            this.tableGeneral.SuspendLayout();
            this.panelFechaEntrega.SuspendLayout();
            this.grpDetalles.SuspendLayout();
            this.tableDetalles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).BeginInit();
            this.panelDetallesBotones.SuspendLayout();
            this.panelResumen.SuspendLayout();
            this.tablePagos.SuspendLayout();
            this.flowPagoResumen.SuspendLayout();
            this.flowPagoAcciones.SuspendLayout();
            this.grpFacturas.SuspendLayout();
            this.tableAdjuntos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdjuntos)).BeginInit();
            this.panelAdjuntosAcciones.SuspendLayout();
            this.gbHistorialEstados.SuspendLayout();
            this.panelAcciones.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(984, 624);
            this.tabControl.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.layoutContenido);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(8);
            this.tabGeneral.Size = new System.Drawing.Size(976, 598);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "Datos";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // layoutContenido
            // 
            this.layoutContenido.ColumnCount = 1;
            this.layoutContenido.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutContenido.Controls.Add(this.tableGeneral, 0, 0);
            this.layoutContenido.Controls.Add(this.grpDetalles, 0, 1);
            this.layoutContenido.Controls.Add(this.grpFacturas, 0, 2);
            this.layoutContenido.Controls.Add(this.gbHistorialEstados, 0, 3);
            this.layoutContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutContenido.Location = new System.Drawing.Point(8, 8);
            this.layoutContenido.Name = "layoutContenido";
            this.layoutContenido.RowCount = 4;
            this.layoutContenido.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutContenido.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutContenido.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutContenido.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutContenido.Size = new System.Drawing.Size(960, 582);
            this.layoutContenido.TabIndex = 0;
            // 
            // tableGeneral
            // 
            this.tableGeneral.AutoSize = true;
            this.tableGeneral.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
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
            this.tableGeneral.Controls.Add(this.lblOC, 0, 5);
            this.tableGeneral.Controls.Add(this.txtOC, 1, 5);
            this.tableGeneral.Controls.Add(this.lblContacto, 0, 6);
            this.tableGeneral.Controls.Add(this.txtContacto, 1, 6);
            this.tableGeneral.Controls.Add(this.lblEmail, 0, 7);
            this.tableGeneral.Controls.Add(this.txtEmail, 1, 7);
            this.tableGeneral.Controls.Add(this.lblTelefono, 0, 8);
            this.tableGeneral.Controls.Add(this.txtTelefono, 1, 8);
            this.tableGeneral.Controls.Add(this.lblDireccionEntrega, 0, 9);
            this.tableGeneral.Controls.Add(this.txtDireccionEntrega, 1, 9);
            this.tableGeneral.Controls.Add(this.lblNumeroRemito, 0, 10);
            this.tableGeneral.Controls.Add(this.txtNumeroRemito, 1, 10);
            this.tableGeneral.Controls.Add(this.lblObservaciones, 0, 11);
            this.tableGeneral.Controls.Add(this.txtObservaciones, 1, 11);
            this.tableGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableGeneral.Location = new System.Drawing.Point(3, 3);
            this.tableGeneral.Name = "tableGeneral";
            this.tableGeneral.RowCount = 12;
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableGeneral.Size = new System.Drawing.Size(954, 0);
            this.tableGeneral.TabIndex = 0;
            // 
            // lblNumeroPedido
            // 
            this.lblNumeroPedido.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNumeroPedido.AutoSize = true;
            this.lblNumeroPedido.Location = new System.Drawing.Point(3, 7);
            this.lblNumeroPedido.Name = "lblNumeroPedido";
            this.lblNumeroPedido.Size = new System.Drawing.Size(80, 13);
            this.lblNumeroPedido.TabIndex = 0;
            this.lblNumeroPedido.Text = "Número Pedido";
            // 
            // txtNumeroPedido
            // 
            this.txtNumeroPedido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNumeroPedido.Location = new System.Drawing.Point(289, 3);
            this.txtNumeroPedido.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.txtNumeroPedido.Name = "txtNumeroPedido";
            this.txtNumeroPedido.ReadOnly = true;
            this.txtNumeroPedido.Size = new System.Drawing.Size(665, 20);
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
            this.cmbCliente.Location = new System.Drawing.Point(289, 31);
            this.cmbCliente.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(665, 21);
            this.cmbCliente.TabIndex = 3;
            // 
            // lblTipoPago
            // 
            this.lblTipoPago.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTipoPago.AutoSize = true;
            this.lblTipoPago.Location = new System.Drawing.Point(3, 63);
            this.lblTipoPago.Name = "lblTipoPago";
            this.lblTipoPago.Size = new System.Drawing.Size(70, 13);
            this.lblTipoPago.TabIndex = 4;
            this.lblTipoPago.Text = "Tipo de pago";
            // 
            // cmbTipoPago
            // 
            this.cmbTipoPago.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTipoPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoPago.FormattingEnabled = true;
            this.cmbTipoPago.Location = new System.Drawing.Point(289, 59);
            this.cmbTipoPago.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.cmbTipoPago.Name = "cmbTipoPago";
            this.cmbTipoPago.Size = new System.Drawing.Size(665, 21);
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
            this.cmbEstadoPedido.Location = new System.Drawing.Point(289, 87);
            this.cmbEstadoPedido.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.cmbEstadoPedido.Name = "cmbEstadoPedido";
            this.cmbEstadoPedido.Size = new System.Drawing.Size(665, 21);
            this.cmbEstadoPedido.TabIndex = 7;
            // 
            // lblFechaEntrega
            // 
            this.lblFechaEntrega.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFechaEntrega.AutoSize = true;
            this.lblFechaEntrega.Location = new System.Drawing.Point(3, 121);
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
            this.panelFechaEntrega.Location = new System.Drawing.Point(289, 115);
            this.panelFechaEntrega.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.panelFechaEntrega.Name = "panelFechaEntrega";
            this.panelFechaEntrega.Size = new System.Drawing.Size(665, 26);
            this.panelFechaEntrega.TabIndex = 9;
            this.panelFechaEntrega.WrapContents = false;
            // 
            // chkFechaEntrega
            // 
            this.chkFechaEntrega.AutoSize = true;
            this.chkFechaEntrega.Location = new System.Drawing.Point(3, 3);
            this.chkFechaEntrega.Name = "chkFechaEntrega";
            this.chkFechaEntrega.Size = new System.Drawing.Size(64, 17);
            this.chkFechaEntrega.TabIndex = 0;
            this.chkFechaEntrega.Text = "Habilitar";
            this.chkFechaEntrega.UseVisualStyleBackColor = true;
            this.chkFechaEntrega.CheckedChanged += new System.EventHandler(this.chkFechaEntrega_CheckedChanged);
            // 
            // dtpFechaEntrega
            // 
            this.dtpFechaEntrega.Enabled = false;
            this.dtpFechaEntrega.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaEntrega.Location = new System.Drawing.Point(73, 3);
            this.dtpFechaEntrega.Name = "dtpFechaEntrega";
            this.dtpFechaEntrega.Size = new System.Drawing.Size(140, 20);
            this.dtpFechaEntrega.TabIndex = 1;
            // 
            // lblOC
            // 
            this.lblOC.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblOC.AutoSize = true;
            this.lblOC.Location = new System.Drawing.Point(3, 151);
            this.lblOC.Name = "lblOC";
            this.lblOC.Size = new System.Drawing.Size(22, 13);
            this.lblOC.TabIndex = 10;
            this.lblOC.Text = "OC";
            // 
            // txtOC
            // 
            this.txtOC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOC.Location = new System.Drawing.Point(289, 147);
            this.txtOC.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.txtOC.Name = "txtOC";
            this.txtOC.Size = new System.Drawing.Size(665, 20);
            this.txtOC.TabIndex = 11;
            // 
            // lblContacto
            // 
            this.lblContacto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblContacto.AutoSize = true;
            this.lblContacto.Location = new System.Drawing.Point(3, 179);
            this.lblContacto.Name = "lblContacto";
            this.lblContacto.Size = new System.Drawing.Size(50, 13);
            this.lblContacto.TabIndex = 12;
            this.lblContacto.Text = "Contacto";
            // 
            // txtContacto
            // 
            this.txtContacto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContacto.Location = new System.Drawing.Point(289, 175);
            this.txtContacto.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.txtContacto.Name = "txtContacto";
            this.txtContacto.Size = new System.Drawing.Size(665, 20);
            this.txtContacto.TabIndex = 13;
            // 
            // lblEmail
            // 
            this.lblEmail.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(3, 207);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(35, 13);
            this.lblEmail.TabIndex = 14;
            this.lblEmail.Text = "E-mail";
            // 
            // txtEmail
            // 
            this.txtEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEmail.Location = new System.Drawing.Point(289, 203);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(665, 20);
            this.txtEmail.TabIndex = 15;
            // 
            // lblTelefono
            // 
            this.lblTelefono.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Location = new System.Drawing.Point(3, 235);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(49, 13);
            this.lblTelefono.TabIndex = 16;
            this.lblTelefono.Text = "Teléfono";
            // 
            // txtTelefono
            // 
            this.txtTelefono.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTelefono.Location = new System.Drawing.Point(289, 231);
            this.txtTelefono.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(665, 20);
            this.txtTelefono.TabIndex = 17;
            // 
            // lblDireccionEntrega
            // 
            this.lblDireccionEntrega.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDireccionEntrega.AutoSize = true;
            this.lblDireccionEntrega.Location = new System.Drawing.Point(3, 263);
            this.lblDireccionEntrega.Name = "lblDireccionEntrega";
            this.lblDireccionEntrega.Size = new System.Drawing.Size(106, 13);
            this.lblDireccionEntrega.TabIndex = 18;
            this.lblDireccionEntrega.Text = "Dirección de entrega";
            // 
            // txtDireccionEntrega
            // 
            this.txtDireccionEntrega.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDireccionEntrega.Location = new System.Drawing.Point(289, 259);
            this.txtDireccionEntrega.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.txtDireccionEntrega.Name = "txtDireccionEntrega";
            this.txtDireccionEntrega.Size = new System.Drawing.Size(665, 20);
            this.txtDireccionEntrega.TabIndex = 19;
            // 
            // lblNumeroRemito
            // 
            this.lblNumeroRemito.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNumeroRemito.AutoSize = true;
            this.lblNumeroRemito.Location = new System.Drawing.Point(3, 291);
            this.lblNumeroRemito.Name = "lblNumeroRemito";
            this.lblNumeroRemito.Size = new System.Drawing.Size(80, 13);
            this.lblNumeroRemito.TabIndex = 20;
            this.lblNumeroRemito.Text = "Números remito";
            // 
            // txtNumeroRemito
            // 
            this.txtNumeroRemito.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNumeroRemito.Location = new System.Drawing.Point(289, 287);
            this.txtNumeroRemito.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.txtNumeroRemito.Name = "txtNumeroRemito";
            this.txtNumeroRemito.Size = new System.Drawing.Size(665, 20);
            this.txtNumeroRemito.TabIndex = 21;
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Location = new System.Drawing.Point(3, 329);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(78, 13);
            this.lblObservaciones.TabIndex = 22;
            this.lblObservaciones.Text = "Observaciones";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtObservaciones.Location = new System.Drawing.Point(289, 315);
            this.txtObservaciones.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservaciones.Size = new System.Drawing.Size(665, 264);
            this.txtObservaciones.TabIndex = 23;
            // 
            // grpDetalles
            // 
            this.grpDetalles.Controls.Add(this.tableDetalles);
            this.grpDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetalles.Location = new System.Drawing.Point(3, 3);
            this.grpDetalles.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.grpDetalles.Name = "grpDetalles";
            this.grpDetalles.Padding = new System.Windows.Forms.Padding(8);
            this.grpDetalles.Size = new System.Drawing.Size(954, 283);
            this.grpDetalles.TabIndex = 1;
            this.grpDetalles.TabStop = false;
            this.grpDetalles.Text = "Productos";
            // 
            // tableDetalles
            // 
            this.tableDetalles.ColumnCount = 1;
            this.tableDetalles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDetalles.Controls.Add(this.dgvDetalles, 0, 0);
            this.tableDetalles.Controls.Add(this.panelDetallesBotones, 0, 1);
            this.tableDetalles.Controls.Add(this.panelResumen, 0, 2);
            this.tableDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableDetalles.Location = new System.Drawing.Point(8, 24);
            this.tableDetalles.Name = "tableDetalles";
            this.tableDetalles.RowCount = 3;
            this.tableDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableDetalles.Size = new System.Drawing.Size(938, 251);
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
            this.dgvDetalles.Size = new System.Drawing.Size(932, 125);
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
            this.panelDetallesBotones.Location = new System.Drawing.Point(3, 134);
            this.panelDetallesBotones.Name = "panelDetallesBotones";
            this.panelDetallesBotones.Size = new System.Drawing.Size(932, 34);
            this.panelDetallesBotones.TabIndex = 1;
            // 
            // btnAgregarDetalle
            // 
            this.btnAgregarDetalle.AutoSize = true;
            this.btnAgregarDetalle.Location = new System.Drawing.Point(834, 3);
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
            this.btnEditarDetalle.Location = new System.Drawing.Point(733, 3);
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
            this.btnEliminarDetalle.Location = new System.Drawing.Point(632, 3);
            this.btnEliminarDetalle.Name = "btnEliminarDetalle";
            this.btnEliminarDetalle.Size = new System.Drawing.Size(95, 23);
            this.btnEliminarDetalle.TabIndex = 2;
            this.btnEliminarDetalle.Text = "Eliminar";
            this.btnEliminarDetalle.UseVisualStyleBackColor = true;
            this.btnEliminarDetalle.Click += new System.EventHandler(this.btnEliminarDetalle_Click);
            // 
            // panelResumen
            // 
            this.panelResumen.ColumnCount = 5;
            this.panelResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.panelResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.panelResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.panelResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19F));
            this.panelResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.panelResumen.Controls.Add(this.lblTotalSinIva, 0, 0);
            this.panelResumen.Controls.Add(this.lblTotalSinIvaValor, 0, 1);
            this.panelResumen.Controls.Add(this.lblMontoIva, 1, 0);
            this.panelResumen.Controls.Add(this.lblMontoIvaValor, 1, 1);
            this.panelResumen.Controls.Add(this.lblTotalConIva, 2, 0);
            this.panelResumen.Controls.Add(this.lblTotalConIvaValor, 2, 1);
            this.panelResumen.Controls.Add(this.lblSaldoPendiente, 3, 0);
            this.panelResumen.Controls.Add(this.lblSaldoPendienteValor, 3, 1);
            this.panelResumen.Controls.Add(this.tablePagos, 4, 0);
            this.panelResumen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelResumen.Location = new System.Drawing.Point(3, 174);
            this.panelResumen.Name = "panelResumen";
            this.panelResumen.RowCount = 2;
            this.panelResumen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.panelResumen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.panelResumen.Size = new System.Drawing.Size(932, 74);
            this.panelResumen.TabIndex = 2;
            // 
            // lblTotalSinIva
            // 
            this.lblTotalSinIva.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblTotalSinIva.AutoSize = true;
            this.lblTotalSinIva.Location = new System.Drawing.Point(58, 17);
            this.lblTotalSinIva.Name = "lblTotalSinIva";
            this.lblTotalSinIva.Size = new System.Drawing.Size(67, 13);
            this.lblTotalSinIva.TabIndex = 0;
            this.lblTotalSinIva.Text = "Total sin IVA";
            // 
            // lblTotalSinIvaValor
            // 
            this.lblTotalSinIvaValor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTotalSinIvaValor.AutoSize = true;
            this.lblTotalSinIvaValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalSinIvaValor.Location = new System.Drawing.Point(71, 29);
            this.lblTotalSinIvaValor.Name = "lblTotalSinIvaValor";
            this.lblTotalSinIvaValor.Size = new System.Drawing.Size(40, 17);
            this.lblTotalSinIvaValor.TabIndex = 1;
            this.lblTotalSinIvaValor.Text = "$0,0";
            // 
            // lblMontoIva
            // 
            this.lblMontoIva.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblMontoIva.AutoSize = true;
            this.lblMontoIva.Location = new System.Drawing.Point(252, 17);
            this.lblMontoIva.Name = "lblMontoIva";
            this.lblMontoIva.Size = new System.Drawing.Size(57, 13);
            this.lblMontoIva.TabIndex = 2;
            this.lblMontoIva.Text = "Monto IVA";
            // 
            // lblMontoIvaValor
            // 
            this.lblMontoIvaValor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMontoIvaValor.AutoSize = true;
            this.lblMontoIvaValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblMontoIvaValor.Location = new System.Drawing.Point(261, 29);
            this.lblMontoIvaValor.Name = "lblMontoIvaValor";
            this.lblMontoIvaValor.Size = new System.Drawing.Size(40, 17);
            this.lblMontoIvaValor.TabIndex = 3;
            this.lblMontoIvaValor.Text = "$0,0";
            // 
            // lblTotalConIva
            // 
            this.lblTotalConIva.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblTotalConIva.AutoSize = true;
            this.lblTotalConIva.Location = new System.Drawing.Point(433, 17);
            this.lblTotalConIva.Name = "lblTotalConIva";
            this.lblTotalConIva.Size = new System.Drawing.Size(72, 13);
            this.lblTotalConIva.TabIndex = 4;
            this.lblTotalConIva.Text = "Total con IVA";
            // 
            // lblTotalConIvaValor
            // 
            this.lblTotalConIvaValor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTotalConIvaValor.AutoSize = true;
            this.lblTotalConIvaValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalConIvaValor.Location = new System.Drawing.Point(449, 29);
            this.lblTotalConIvaValor.Name = "lblTotalConIvaValor";
            this.lblTotalConIvaValor.Size = new System.Drawing.Size(40, 17);
            this.lblTotalConIvaValor.TabIndex = 5;
            this.lblTotalConIvaValor.Text = "$0,0";
            // 
            // lblSaldoPendiente
            // 
            this.lblSaldoPendiente.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblSaldoPendiente.AutoSize = true;
            this.lblSaldoPendiente.Location = new System.Drawing.Point(614, 17);
            this.lblSaldoPendiente.Name = "lblSaldoPendiente";
            this.lblSaldoPendiente.Size = new System.Drawing.Size(84, 13);
            this.lblSaldoPendiente.TabIndex = 6;
            this.lblSaldoPendiente.Text = "Saldo pendiente";
            // 
            // lblSaldoPendienteValor
            // 
            this.lblSaldoPendienteValor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblSaldoPendienteValor.AutoSize = true;
            this.lblSaldoPendienteValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblSaldoPendienteValor.Location = new System.Drawing.Point(636, 29);
            this.lblSaldoPendienteValor.Name = "lblSaldoPendienteValor";
            this.lblSaldoPendienteValor.Size = new System.Drawing.Size(40, 17);
            this.lblSaldoPendienteValor.TabIndex = 7;
            this.lblSaldoPendienteValor.Text = "$0,0";
            //
            // tablePagos
            //
            this.tablePagos.AutoSize = true;
            this.tablePagos.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tablePagos.ColumnCount = 1;
            this.tablePagos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePagos.Controls.Add(this.flowPagoResumen, 0, 0);
            this.tablePagos.Controls.Add(this.lstPagos, 0, 1);
            this.tablePagos.Controls.Add(this.flowPagoAcciones, 0, 2);
            this.tablePagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePagos.Location = new System.Drawing.Point(667, 0);
            this.tablePagos.Margin = new System.Windows.Forms.Padding(0);
            this.tablePagos.Name = "tablePagos";
            this.panelResumen.SetRowSpan(this.tablePagos, 2);
            this.tablePagos.RowCount = 3;
            this.tablePagos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePagos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePagos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePagos.Size = new System.Drawing.Size(280, 74);
            this.tablePagos.TabIndex = 8;
            //
            // flowPagoResumen
            //
            this.flowPagoResumen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowPagoResumen.AutoSize = true;
            this.flowPagoResumen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowPagoResumen.BackColor = System.Drawing.Color.Transparent;
            this.flowPagoResumen.Controls.Add(this.lblMontoPagado);
            this.flowPagoResumen.Controls.Add(this.lblMontoPagadoValor);
            this.flowPagoResumen.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowPagoResumen.Location = new System.Drawing.Point(46, 3);
            this.flowPagoResumen.Margin = new System.Windows.Forms.Padding(0);
            this.flowPagoResumen.Name = "flowPagoResumen";
            this.flowPagoResumen.Padding = new System.Windows.Forms.Padding(6, 6, 6, 0);
            this.flowPagoResumen.Size = new System.Drawing.Size(187, 27);
            this.flowPagoResumen.TabIndex = 0;
            this.flowPagoResumen.WrapContents = false;
            // 
            // lblMontoPagado
            // 
            this.lblMontoPagado.AutoSize = true;
            this.lblMontoPagado.Location = new System.Drawing.Point(6, 6);
            this.lblMontoPagado.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.lblMontoPagado.Name = "lblMontoPagado";
            this.lblMontoPagado.Size = new System.Drawing.Size(90, 13);
            this.lblMontoPagado.TabIndex = 0;
            this.lblMontoPagado.Text = "order.paidAmount";
            // 
            // lblMontoPagadoValor
            // 
            this.lblMontoPagadoValor.AutoSize = true;
            this.lblMontoPagadoValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblMontoPagadoValor.Location = new System.Drawing.Point(112, 6);
            this.lblMontoPagadoValor.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblMontoPagadoValor.Name = "lblMontoPagadoValor";
            this.lblMontoPagadoValor.Size = new System.Drawing.Size(43, 15);
            this.lblMontoPagadoValor.TabIndex = 1;
            this.lblMontoPagadoValor.Text = "$0,00";
            // 
            // lstPagos
            // 
            this.lstPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPagos.FormattingEnabled = true;
            this.lstPagos.IntegralHeight = false;
            this.lstPagos.Location = new System.Drawing.Point(3, 33);
            this.lstPagos.Name = "lstPagos";
            this.lstPagos.SelectionMode = System.Windows.Forms.SelectionMode.One;
            this.lstPagos.Size = new System.Drawing.Size(274, 8);
            this.lstPagos.TabIndex = 1;
            // 
            // flowPagoAcciones
            // 
            this.flowPagoAcciones.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowPagoAcciones.AutoSize = true;
            this.flowPagoAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowPagoAcciones.BackColor = System.Drawing.Color.Transparent;
            this.flowPagoAcciones.Controls.Add(this.btnAgregarPago);
            this.flowPagoAcciones.Controls.Add(this.btnDeshacerPago);
            this.flowPagoAcciones.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowPagoAcciones.Location = new System.Drawing.Point(37, 44);
            this.flowPagoAcciones.Margin = new System.Windows.Forms.Padding(0);
            this.flowPagoAcciones.Name = "flowPagoAcciones";
            this.flowPagoAcciones.Padding = new System.Windows.Forms.Padding(6, 3, 6, 6);
            this.flowPagoAcciones.Size = new System.Drawing.Size(206, 28);
            this.flowPagoAcciones.TabIndex = 2;
            this.flowPagoAcciones.WrapContents = false;
            // 
            // btnAgregarPago
            // 
            this.btnAgregarPago.AutoSize = true;
            this.btnAgregarPago.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAgregarPago.Location = new System.Drawing.Point(6, 3);
            this.btnAgregarPago.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.btnAgregarPago.Name = "btnAgregarPago";
            this.btnAgregarPago.Size = new System.Drawing.Size(142, 23);
            this.btnAgregarPago.TabIndex = 3;
            this.btnAgregarPago.Text = "order.payment.addPercent";
            this.btnAgregarPago.UseVisualStyleBackColor = true;
            // 
            // btnDeshacerPago
            // 
            this.btnDeshacerPago.AutoSize = true;
            this.btnDeshacerPago.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDeshacerPago.Location = new System.Drawing.Point(153, 3);
            this.btnDeshacerPago.Margin = new System.Windows.Forms.Padding(0);
            this.btnDeshacerPago.Name = "btnDeshacerPago";
            this.btnDeshacerPago.Size = new System.Drawing.Size(119, 23);
            this.btnDeshacerPago.TabIndex = 4;
            this.btnDeshacerPago.Text = "order.payment.cancel";
            this.btnDeshacerPago.UseVisualStyleBackColor = true;
            // 
            // grpFacturas
            // 
            this.grpFacturas.Controls.Add(this.tableAdjuntos);
            this.grpFacturas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpFacturas.Location = new System.Drawing.Point(3, 292);
            this.grpFacturas.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.grpFacturas.Name = "grpFacturas";
            this.grpFacturas.Padding = new System.Windows.Forms.Padding(8);
            this.grpFacturas.Size = new System.Drawing.Size(954, 138);
            this.grpFacturas.TabIndex = 2;
            this.grpFacturas.TabStop = false;
            this.grpFacturas.Text = "order.facturas";
            //
            // tableAdjuntos
            //
            this.tableAdjuntos.ColumnCount = 1;
            this.tableAdjuntos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableAdjuntos.Controls.Add(this.lblAdjuntosInstrucciones, 0, 0);
            this.tableAdjuntos.Controls.Add(this.dgvAdjuntos, 0, 1);
            this.tableAdjuntos.Controls.Add(this.panelAdjuntosAcciones, 0, 2);
            this.tableAdjuntos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableAdjuntos.Location = new System.Drawing.Point(8, 24);
            this.tableAdjuntos.Name = "tableAdjuntos";
            this.tableAdjuntos.RowCount = 3;
            this.tableAdjuntos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAdjuntos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableAdjuntos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAdjuntos.Size = new System.Drawing.Size(938, 106);
            this.tableAdjuntos.TabIndex = 0;
            //
            // lblAdjuntosInstrucciones
            //
            this.lblAdjuntosInstrucciones.AutoSize = true;
            this.lblAdjuntosInstrucciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAdjuntosInstrucciones.Location = new System.Drawing.Point(3, 0);
            this.lblAdjuntosInstrucciones.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.lblAdjuntosInstrucciones.Name = "lblAdjuntosInstrucciones";
            this.lblAdjuntosInstrucciones.Size = new System.Drawing.Size(954, 13);
            this.lblAdjuntosInstrucciones.TabIndex = 0;
            this.lblAdjuntosInstrucciones.Text = "order.facturas.instructions";
            //
            // dgvAdjuntos
            //
            this.dgvAdjuntos.AllowUserToAddRows = false;
            this.dgvAdjuntos.AllowUserToDeleteRows = false;
            this.dgvAdjuntos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAdjuntos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdjuntos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAdjuntos.Location = new System.Drawing.Point(3, 19);
            this.dgvAdjuntos.MultiSelect = false;
            this.dgvAdjuntos.Name = "dgvAdjuntos";
            this.dgvAdjuntos.RowHeadersVisible = false;
            this.dgvAdjuntos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAdjuntos.Size = new System.Drawing.Size(932, 56);
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
            this.panelAdjuntosAcciones.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.panelAdjuntosAcciones.Location = new System.Drawing.Point(3, 81);
            this.panelAdjuntosAcciones.Name = "panelAdjuntosAcciones";
            this.panelAdjuntosAcciones.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.panelAdjuntosAcciones.Size = new System.Drawing.Size(932, 22);
            this.panelAdjuntosAcciones.TabIndex = 2;
            this.panelAdjuntosAcciones.WrapContents = false;
            //
            // btnAgregarAdjunto
            //
            this.btnAgregarAdjunto.AutoSize = true;
            this.btnAgregarAdjunto.Location = new System.Drawing.Point(3, 9);
            this.btnAgregarAdjunto.Name = "btnAgregarAdjunto";
            this.btnAgregarAdjunto.Size = new System.Drawing.Size(107, 23);
            this.btnAgregarAdjunto.TabIndex = 0;
            this.btnAgregarAdjunto.Text = "order.facturas.add";
            this.btnAgregarAdjunto.UseVisualStyleBackColor = true;
            //
            // btnDescargarAdjunto
            //
            this.btnDescargarAdjunto.AutoSize = true;
            this.btnDescargarAdjunto.Location = new System.Drawing.Point(116, 9);
            this.btnDescargarAdjunto.Name = "btnDescargarAdjunto";
            this.btnDescargarAdjunto.Size = new System.Drawing.Size(117, 23);
            this.btnDescargarAdjunto.TabIndex = 1;
            this.btnDescargarAdjunto.Text = "order.facturas.download";
            this.btnDescargarAdjunto.UseVisualStyleBackColor = true;
            //
            // btnEliminarAdjunto
            //
            this.btnEliminarAdjunto.AutoSize = true;
            this.btnEliminarAdjunto.Location = new System.Drawing.Point(239, 9);
            this.btnEliminarAdjunto.Name = "btnEliminarAdjunto";
            this.btnEliminarAdjunto.Size = new System.Drawing.Size(104, 23);
            this.btnEliminarAdjunto.TabIndex = 2;
            this.btnEliminarAdjunto.Text = "order.facturas.delete";
            this.btnEliminarAdjunto.UseVisualStyleBackColor = true;
            // 
            // gbHistorialEstados
            // 
            this.gbHistorialEstados.Controls.Add(this.lvHistorialEstados);
            this.gbHistorialEstados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbHistorialEstados.Location = new System.Drawing.Point(3, 436);
            this.gbHistorialEstados.Name = "gbHistorialEstados";
            this.gbHistorialEstados.Padding = new System.Windows.Forms.Padding(10, 6, 10, 10);
            this.gbHistorialEstados.Size = new System.Drawing.Size(954, 143);
            this.gbHistorialEstados.TabIndex = 3;
            this.gbHistorialEstados.TabStop = false;
            this.gbHistorialEstados.Text = "order.historial.estados";
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
            this.lvHistorialEstados.Size = new System.Drawing.Size(934, 114);
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
            this.columnComentario.Width = 540;
            // 
            // panelAcciones
            // 
            this.panelAcciones.AutoSize = true;
            this.panelAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelAcciones.Controls.Add(this.btnGuardar);
            this.panelAcciones.Controls.Add(this.btnCancelar);
            this.panelAcciones.Controls.Add(this.btnCancelarPedido);
            this.panelAcciones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAcciones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelAcciones.Location = new System.Drawing.Point(0, 624);
            this.panelAcciones.Name = "panelAcciones";
            this.panelAcciones.Size = new System.Drawing.Size(984, 37);
            this.panelAcciones.TabIndex = 1;
            // 
            // btnGuardar
            // 
            this.btnGuardar.AutoSize = true;
            this.btnGuardar.Location = new System.Drawing.Point(886, 3);
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
            this.btnCancelar.Location = new System.Drawing.Point(785, 3);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(95, 31);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            //
            // btnCancelarPedido
            //
            this.btnCancelarPedido.AutoSize = true;
            this.btnCancelarPedido.Location = new System.Drawing.Point(651, 3);
            this.btnCancelarPedido.Name = "btnCancelarPedido";
            this.btnCancelarPedido.Size = new System.Drawing.Size(128, 31);
            this.btnCancelarPedido.TabIndex = 2;
            this.btnCancelarPedido.Text = "Cancelar pedido";
            this.btnCancelarPedido.UseVisualStyleBackColor = true;
            this.btnCancelarPedido.Click += new System.EventHandler(this.btnCancelarPedido_Click);
            // 
            // PedidoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelAcciones);
            this.Name = "PedidoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pedido";
            this.Load += new System.EventHandler(this.PedidoForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.layoutContenido.ResumeLayout(false);
            this.layoutContenido.PerformLayout();
            this.tableGeneral.ResumeLayout(false);
            this.tableGeneral.PerformLayout();
            this.panelFechaEntrega.ResumeLayout(false);
            this.panelFechaEntrega.PerformLayout();
            this.grpDetalles.ResumeLayout(false);
            this.tableDetalles.ResumeLayout(false);
            this.tableDetalles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalles)).EndInit();
            this.panelDetallesBotones.ResumeLayout(false);
            this.panelDetallesBotones.PerformLayout();
            this.panelResumen.ResumeLayout(false);
            this.panelResumen.PerformLayout();
            this.tablePagos.ResumeLayout(false);
            this.tablePagos.PerformLayout();
            this.flowPagoResumen.ResumeLayout(false);
            this.flowPagoResumen.PerformLayout();
            this.flowPagoAcciones.ResumeLayout(false);
            this.flowPagoAcciones.PerformLayout();
            this.grpFacturas.ResumeLayout(false);
            this.tableAdjuntos.ResumeLayout(false);
            this.tableAdjuntos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdjuntos)).EndInit();
            this.panelAdjuntosAcciones.ResumeLayout(false);
            this.panelAdjuntosAcciones.PerformLayout();
            this.gbHistorialEstados.ResumeLayout(false);
            this.panelAcciones.ResumeLayout(false);
            this.panelAcciones.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
#endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TableLayoutPanel layoutContenido;
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
        private System.Windows.Forms.GroupBox grpDetalles;
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
        private System.Windows.Forms.TableLayoutPanel tablePagos;
        private System.Windows.Forms.FlowLayoutPanel flowPagoResumen;
        private System.Windows.Forms.Label lblMontoPagado;
        private System.Windows.Forms.Label lblMontoPagadoValor;
        private System.Windows.Forms.ListBox lstPagos;
        private System.Windows.Forms.FlowLayoutPanel flowPagoAcciones;
        private System.Windows.Forms.Button btnAgregarPago;
        private System.Windows.Forms.Button btnDeshacerPago;
        private System.Windows.Forms.GroupBox grpFacturas;
        private System.Windows.Forms.TableLayoutPanel tableAdjuntos;
        private System.Windows.Forms.Label lblAdjuntosInstrucciones;
        private System.Windows.Forms.DataGridView dgvAdjuntos;
        private System.Windows.Forms.FlowLayoutPanel panelAdjuntosAcciones;
        private System.Windows.Forms.Button btnAgregarAdjunto;
        private System.Windows.Forms.Button btnDescargarAdjunto;
        private System.Windows.Forms.Button btnEliminarAdjunto;
        private System.Windows.Forms.GroupBox gbHistorialEstados;
        private System.Windows.Forms.ListView lvHistorialEstados;
        private System.Windows.Forms.ColumnHeader columnFecha;
        private System.Windows.Forms.ColumnHeader columnEstado;
        private System.Windows.Forms.ColumnHeader columnComentario;
        private System.Windows.Forms.FlowLayoutPanel panelAcciones;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnCancelarPedido;
    }
}