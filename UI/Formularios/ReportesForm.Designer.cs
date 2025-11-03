namespace UI.Formularios
{
    partial class ReportesForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel layoutPrincipal;
        private System.Windows.Forms.FlowLayoutPanel flowAcciones;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.TabControl tabReportes;
        private System.Windows.Forms.TabPage tabVentasPeriodo;
        private System.Windows.Forms.TabPage tabCategorias;
        private System.Windows.Forms.TabPage tabFacturacion;
        private System.Windows.Forms.TabPage tabPedidosCliente;
        private System.Windows.Forms.TabPage tabPedidosProveedor;
        private System.Windows.Forms.TabPage tabMejoresClientes;
        private System.Windows.Forms.TabPage tabClientesSaldo;
        private System.Windows.Forms.TableLayoutPanel layoutVentasPeriodo;
        private System.Windows.Forms.FlowLayoutPanel flowVentasPeriodo;
        private System.Windows.Forms.Label lblVentasTipo;
        private System.Windows.Forms.ComboBox cmbVentasTipo;
        private System.Windows.Forms.Label lblVentasDesde;
        private System.Windows.Forms.DateTimePicker dtpVentasDesde;
        private System.Windows.Forms.Label lblVentasHasta;
        private System.Windows.Forms.DateTimePicker dtpVentasHasta;
        private System.Windows.Forms.Label lblVentasMes;
        private System.Windows.Forms.DateTimePicker dtpVentasMes;
        private System.Windows.Forms.Label lblVentasAnio;
        private System.Windows.Forms.NumericUpDown nudVentasAnio;
        private System.Windows.Forms.Button btnVentasAplicar;
        private System.Windows.Forms.DataGridView dgvVentasPeriodo;
        private System.Windows.Forms.TableLayoutPanel layoutCategorias;
        private System.Windows.Forms.FlowLayoutPanel flowCategorias;
        private System.Windows.Forms.Label lblCategoriasPeriodo;
        private System.Windows.Forms.ComboBox cmbCategoriasPeriodo;
        private System.Windows.Forms.Label lblCategoriasMes;
        private System.Windows.Forms.DateTimePicker dtpCategoriasMes;
        private System.Windows.Forms.Label lblCategoriasAnio;
        private System.Windows.Forms.NumericUpDown nudCategoriasAnio;
        private System.Windows.Forms.Button btnCategoriasAplicar;
        private System.Windows.Forms.DataGridView dgvCategorias;
        private System.Windows.Forms.TableLayoutPanel layoutFacturacion;
        private System.Windows.Forms.FlowLayoutPanel flowFacturacion;
        private System.Windows.Forms.Label lblFacturacionPeriodo;
        private System.Windows.Forms.ComboBox cmbFacturacionPeriodo;
        private System.Windows.Forms.Label lblFacturacionMes;
        private System.Windows.Forms.DateTimePicker dtpFacturacionMes;
        private System.Windows.Forms.Label lblFacturacionAnio;
        private System.Windows.Forms.NumericUpDown nudFacturacionAnio;
        private System.Windows.Forms.Button btnFacturacionAplicar;
        private System.Windows.Forms.DataGridView dgvFacturacion;
        private System.Windows.Forms.TableLayoutPanel layoutPedidosCliente;
        private System.Windows.Forms.FlowLayoutPanel flowPedidosCliente;
        private System.Windows.Forms.Label lblPedidosClientePeriodo;
        private System.Windows.Forms.ComboBox cmbPedidosClientePeriodo;
        private System.Windows.Forms.Label lblPedidosClienteMes;
        private System.Windows.Forms.DateTimePicker dtpPedidosClienteMes;
        private System.Windows.Forms.Label lblPedidosClienteAnio;
        private System.Windows.Forms.NumericUpDown nudPedidosClienteAnio;
        private System.Windows.Forms.CheckBox chkPedidosClienteSaldo;
        private System.Windows.Forms.Button btnPedidosClienteAplicar;
        private System.Windows.Forms.DataGridView dgvPedidosCliente;
        private System.Windows.Forms.TableLayoutPanel layoutPedidosProveedor;
        private System.Windows.Forms.FlowLayoutPanel flowPedidosProveedor;
        private System.Windows.Forms.Label lblPedidosProveedorPeriodo;
        private System.Windows.Forms.ComboBox cmbPedidosProveedorPeriodo;
        private System.Windows.Forms.Label lblPedidosProveedorMes;
        private System.Windows.Forms.DateTimePicker dtpPedidosProveedorMes;
        private System.Windows.Forms.Label lblPedidosProveedorAnio;
        private System.Windows.Forms.NumericUpDown nudPedidosProveedorAnio;
        private System.Windows.Forms.Button btnPedidosProveedorAplicar;
        private System.Windows.Forms.DataGridView dgvPedidosProveedor;
        private System.Windows.Forms.TableLayoutPanel layoutMejoresClientes;
        private System.Windows.Forms.FlowLayoutPanel flowMejoresClientes;
        private System.Windows.Forms.Label lblMejoresClientesPeriodo;
        private System.Windows.Forms.ComboBox cmbMejoresClientesPeriodo;
        private System.Windows.Forms.Label lblMejoresClientesMes;
        private System.Windows.Forms.NumericUpDown nudMejoresClientesMes;
        private System.Windows.Forms.Label lblMejoresClientesAnio;
        private System.Windows.Forms.NumericUpDown nudMejoresClientesAnio;
        private System.Windows.Forms.Button btnMejoresClientesAplicar;
        private System.Windows.Forms.DataGridView dgvMejoresClientes;
        private System.Windows.Forms.TableLayoutPanel layoutClientesSaldo;
        private System.Windows.Forms.FlowLayoutPanel flowClientesSaldo;
        private System.Windows.Forms.Label lblClientesSaldoInfo;
        private System.Windows.Forms.DataGridView dgvClientesSaldo;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.layoutPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.flowAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.tabReportes = new System.Windows.Forms.TabControl();
            this.tabVentasPeriodo = new System.Windows.Forms.TabPage();
            this.layoutVentasPeriodo = new System.Windows.Forms.TableLayoutPanel();
            this.flowVentasPeriodo = new System.Windows.Forms.FlowLayoutPanel();
            this.lblVentasTipo = new System.Windows.Forms.Label();
            this.cmbVentasTipo = new System.Windows.Forms.ComboBox();
            this.lblVentasDesde = new System.Windows.Forms.Label();
            this.dtpVentasDesde = new System.Windows.Forms.DateTimePicker();
            this.lblVentasHasta = new System.Windows.Forms.Label();
            this.dtpVentasHasta = new System.Windows.Forms.DateTimePicker();
            this.lblVentasMes = new System.Windows.Forms.Label();
            this.dtpVentasMes = new System.Windows.Forms.DateTimePicker();
            this.lblVentasAnio = new System.Windows.Forms.Label();
            this.nudVentasAnio = new System.Windows.Forms.NumericUpDown();
            this.btnVentasAplicar = new System.Windows.Forms.Button();
            this.dgvVentasPeriodo = new System.Windows.Forms.DataGridView();
            this.tabCategorias = new System.Windows.Forms.TabPage();
            this.layoutCategorias = new System.Windows.Forms.TableLayoutPanel();
            this.flowCategorias = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCategoriasPeriodo = new System.Windows.Forms.Label();
            this.cmbCategoriasPeriodo = new System.Windows.Forms.ComboBox();
            this.lblCategoriasMes = new System.Windows.Forms.Label();
            this.dtpCategoriasMes = new System.Windows.Forms.DateTimePicker();
            this.lblCategoriasAnio = new System.Windows.Forms.Label();
            this.nudCategoriasAnio = new System.Windows.Forms.NumericUpDown();
            this.btnCategoriasAplicar = new System.Windows.Forms.Button();
            this.dgvCategorias = new System.Windows.Forms.DataGridView();
            this.tabFacturacion = new System.Windows.Forms.TabPage();
            this.layoutFacturacion = new System.Windows.Forms.TableLayoutPanel();
            this.flowFacturacion = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFacturacionPeriodo = new System.Windows.Forms.Label();
            this.cmbFacturacionPeriodo = new System.Windows.Forms.ComboBox();
            this.lblFacturacionMes = new System.Windows.Forms.Label();
            this.dtpFacturacionMes = new System.Windows.Forms.DateTimePicker();
            this.lblFacturacionAnio = new System.Windows.Forms.Label();
            this.nudFacturacionAnio = new System.Windows.Forms.NumericUpDown();
            this.btnFacturacionAplicar = new System.Windows.Forms.Button();
            this.dgvFacturacion = new System.Windows.Forms.DataGridView();
            this.tabPedidosCliente = new System.Windows.Forms.TabPage();
            this.layoutPedidosCliente = new System.Windows.Forms.TableLayoutPanel();
            this.flowPedidosCliente = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPedidosClientePeriodo = new System.Windows.Forms.Label();
            this.cmbPedidosClientePeriodo = new System.Windows.Forms.ComboBox();
            this.lblPedidosClienteMes = new System.Windows.Forms.Label();
            this.dtpPedidosClienteMes = new System.Windows.Forms.DateTimePicker();
            this.lblPedidosClienteAnio = new System.Windows.Forms.Label();
            this.nudPedidosClienteAnio = new System.Windows.Forms.NumericUpDown();
            this.chkPedidosClienteSaldo = new System.Windows.Forms.CheckBox();
            this.btnPedidosClienteAplicar = new System.Windows.Forms.Button();
            this.dgvPedidosCliente = new System.Windows.Forms.DataGridView();
            this.tabPedidosProveedor = new System.Windows.Forms.TabPage();
            this.layoutPedidosProveedor = new System.Windows.Forms.TableLayoutPanel();
            this.flowPedidosProveedor = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPedidosProveedorPeriodo = new System.Windows.Forms.Label();
            this.cmbPedidosProveedorPeriodo = new System.Windows.Forms.ComboBox();
            this.lblPedidosProveedorMes = new System.Windows.Forms.Label();
            this.dtpPedidosProveedorMes = new System.Windows.Forms.DateTimePicker();
            this.lblPedidosProveedorAnio = new System.Windows.Forms.Label();
            this.nudPedidosProveedorAnio = new System.Windows.Forms.NumericUpDown();
            this.btnPedidosProveedorAplicar = new System.Windows.Forms.Button();
            this.dgvPedidosProveedor = new System.Windows.Forms.DataGridView();
            this.tabMejoresClientes = new System.Windows.Forms.TabPage();
            this.layoutMejoresClientes = new System.Windows.Forms.TableLayoutPanel();
            this.flowMejoresClientes = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMejoresClientesPeriodo = new System.Windows.Forms.Label();
            this.cmbMejoresClientesPeriodo = new System.Windows.Forms.ComboBox();
            this.lblMejoresClientesMes = new System.Windows.Forms.Label();
            this.nudMejoresClientesMes = new System.Windows.Forms.NumericUpDown();
            this.lblMejoresClientesAnio = new System.Windows.Forms.Label();
            this.nudMejoresClientesAnio = new System.Windows.Forms.NumericUpDown();
            this.btnMejoresClientesAplicar = new System.Windows.Forms.Button();
            this.dgvMejoresClientes = new System.Windows.Forms.DataGridView();
            this.tabClientesSaldo = new System.Windows.Forms.TabPage();
            this.layoutClientesSaldo = new System.Windows.Forms.TableLayoutPanel();
            this.flowClientesSaldo = new System.Windows.Forms.FlowLayoutPanel();
            this.lblClientesSaldoInfo = new System.Windows.Forms.Label();
            this.dgvClientesSaldo = new System.Windows.Forms.DataGridView();
            this.layoutPrincipal.SuspendLayout();
            this.flowAcciones.SuspendLayout();
            this.tabReportes.SuspendLayout();
            this.tabVentasPeriodo.SuspendLayout();
            this.layoutVentasPeriodo.SuspendLayout();
            this.flowVentasPeriodo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVentasAnio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasPeriodo)).BeginInit();
            this.tabCategorias.SuspendLayout();
            this.layoutCategorias.SuspendLayout();
            this.flowCategorias.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCategoriasAnio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategorias)).BeginInit();
            this.tabFacturacion.SuspendLayout();
            this.layoutFacturacion.SuspendLayout();
            this.flowFacturacion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFacturacionAnio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFacturacion)).BeginInit();
            this.tabPedidosCliente.SuspendLayout();
            this.layoutPedidosCliente.SuspendLayout();
            this.flowPedidosCliente.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPedidosClienteAnio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidosCliente)).BeginInit();
            this.tabPedidosProveedor.SuspendLayout();
            this.layoutPedidosProveedor.SuspendLayout();
            this.flowPedidosProveedor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPedidosProveedorAnio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidosProveedor)).BeginInit();
            this.tabMejoresClientes.SuspendLayout();
            this.layoutMejoresClientes.SuspendLayout();
            this.flowMejoresClientes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMejoresClientesMes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMejoresClientesAnio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMejoresClientes)).BeginInit();
            this.tabClientesSaldo.SuspendLayout();
            this.layoutClientesSaldo.SuspendLayout();
            this.flowClientesSaldo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientesSaldo)).BeginInit();
            this.SuspendLayout();
            // layoutPrincipal
            this.layoutPrincipal.ColumnCount = 1;
            this.layoutPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPrincipal.Controls.Add(this.flowAcciones, 0, 0);
            this.layoutPrincipal.Controls.Add(this.tabReportes, 0, 1);
            this.layoutPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPrincipal.Location = new System.Drawing.Point(0, 0);
            this.layoutPrincipal.Name = "layoutPrincipal";
            this.layoutPrincipal.RowCount = 2;
            this.layoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPrincipal.Size = new System.Drawing.Size(1024, 768);
            this.layoutPrincipal.TabIndex = 0;
            // flowAcciones
            this.flowAcciones.AutoSize = true;
            this.flowAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowAcciones.Controls.Add(this.btnExportExcel);
            this.flowAcciones.Controls.Add(this.btnExportPdf);
            this.flowAcciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowAcciones.Location = new System.Drawing.Point(3, 3);
            this.flowAcciones.Name = "flowAcciones";
            this.flowAcciones.Size = new System.Drawing.Size(1018, 39);
            this.flowAcciones.TabIndex = 0;
            // btnExportExcel
            this.btnExportExcel.AutoSize = true;
            this.btnExportExcel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExportExcel.Location = new System.Drawing.Point(3, 3);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.btnExportExcel.Size = new System.Drawing.Size(108, 33);
            this.btnExportExcel.TabIndex = 0;
            this.btnExportExcel.Text = "Export Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // btnExportPdf
            this.btnExportPdf.AutoSize = true;
            this.btnExportPdf.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExportPdf.Location = new System.Drawing.Point(117, 3);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.btnExportPdf.Size = new System.Drawing.Size(95, 33);
            this.btnExportPdf.TabIndex = 1;
            this.btnExportPdf.Text = "Export PDF";
            this.btnExportPdf.UseVisualStyleBackColor = true;
            this.btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);
            // tabReportes
            this.tabReportes.Controls.Add(this.tabVentasPeriodo);
            this.tabReportes.Controls.Add(this.tabCategorias);
            this.tabReportes.Controls.Add(this.tabFacturacion);
            this.tabReportes.Controls.Add(this.tabPedidosCliente);
            this.tabReportes.Controls.Add(this.tabMejoresClientes);
            this.tabReportes.Controls.Add(this.tabClientesSaldo);
            this.tabReportes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReportes.Location = new System.Drawing.Point(3, 48);
            this.tabReportes.Name = "tabReportes";
            this.tabReportes.SelectedIndex = 0;
            this.tabReportes.Size = new System.Drawing.Size(1018, 717);
            this.tabReportes.TabIndex = 1;
            this.tabReportes.SelectedIndexChanged += new System.EventHandler(this.tabReportes_SelectedIndexChanged);
            // tabVentasPeriodo
            this.tabVentasPeriodo.Controls.Add(this.layoutVentasPeriodo);
            this.tabVentasPeriodo.Location = new System.Drawing.Point(4, 29);
            this.tabVentasPeriodo.Name = "tabVentasPeriodo";
            this.tabVentasPeriodo.Padding = new System.Windows.Forms.Padding(3);
            this.tabVentasPeriodo.Size = new System.Drawing.Size(1010, 684);
            this.tabVentasPeriodo.TabIndex = 0;
            this.tabVentasPeriodo.Text = "Ventas";
            this.tabVentasPeriodo.UseVisualStyleBackColor = true;
            // layoutVentasPeriodo
            this.layoutVentasPeriodo.ColumnCount = 1;
            this.layoutVentasPeriodo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutVentasPeriodo.Controls.Add(this.flowVentasPeriodo, 0, 0);
            this.layoutVentasPeriodo.Controls.Add(this.dgvVentasPeriodo, 0, 1);
            this.layoutVentasPeriodo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutVentasPeriodo.Location = new System.Drawing.Point(3, 3);
            this.layoutVentasPeriodo.Name = "layoutVentasPeriodo";
            this.layoutVentasPeriodo.RowCount = 2;
            this.layoutVentasPeriodo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layoutVentasPeriodo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutVentasPeriodo.Size = new System.Drawing.Size(1004, 678);
            this.layoutVentasPeriodo.TabIndex = 0;
            // flowVentasPeriodo
            this.flowVentasPeriodo.AutoSize = true;
            this.flowVentasPeriodo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowVentasPeriodo.Controls.Add(this.lblVentasTipo);
            this.flowVentasPeriodo.Controls.Add(this.cmbVentasTipo);
            this.flowVentasPeriodo.Controls.Add(this.lblVentasDesde);
            this.flowVentasPeriodo.Controls.Add(this.dtpVentasDesde);
            this.flowVentasPeriodo.Controls.Add(this.lblVentasHasta);
            this.flowVentasPeriodo.Controls.Add(this.dtpVentasHasta);
            this.flowVentasPeriodo.Controls.Add(this.lblVentasMes);
            this.flowVentasPeriodo.Controls.Add(this.dtpVentasMes);
            this.flowVentasPeriodo.Controls.Add(this.lblVentasAnio);
            this.flowVentasPeriodo.Controls.Add(this.nudVentasAnio);
            this.flowVentasPeriodo.Controls.Add(this.btnVentasAplicar);
            this.flowVentasPeriodo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowVentasPeriodo.Location = new System.Drawing.Point(3, 3);
            this.flowVentasPeriodo.Name = "flowVentasPeriodo";
            this.flowVentasPeriodo.Size = new System.Drawing.Size(998, 46);
            this.flowVentasPeriodo.TabIndex = 0;
            // lblVentasTipo
            this.lblVentasTipo.AutoSize = true;
            this.lblVentasTipo.Location = new System.Drawing.Point(3, 0);
            this.lblVentasTipo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblVentasTipo.Name = "lblVentasTipo";
            this.lblVentasTipo.Size = new System.Drawing.Size(92, 20);
            this.lblVentasTipo.TabIndex = 0;
            this.lblVentasTipo.Text = "Tipo período";
            // cmbVentasTipo
            this.cmbVentasTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVentasTipo.FormattingEnabled = true;
            this.cmbVentasTipo.Location = new System.Drawing.Point(101, 3);
            this.cmbVentasTipo.Name = "cmbVentasTipo";
            this.cmbVentasTipo.Size = new System.Drawing.Size(140, 28);
            this.cmbVentasTipo.TabIndex = 1;
            this.cmbVentasTipo.SelectedIndexChanged += new System.EventHandler(this.cmbVentasTipo_SelectedIndexChanged);
            // lblVentasDesde
            this.lblVentasDesde.AutoSize = true;
            this.lblVentasDesde.Location = new System.Drawing.Point(247, 0);
            this.lblVentasDesde.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblVentasDesde.Name = "lblVentasDesde";
            this.lblVentasDesde.Size = new System.Drawing.Size(52, 20);
            this.lblVentasDesde.TabIndex = 2;
            this.lblVentasDesde.Text = "Desde";
            // dtpVentasDesde
            this.dtpVentasDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpVentasDesde.Location = new System.Drawing.Point(305, 3);
            this.dtpVentasDesde.Name = "dtpVentasDesde";
            this.dtpVentasDesde.Size = new System.Drawing.Size(120, 26);
            this.dtpVentasDesde.TabIndex = 3;
            // lblVentasHasta
            this.lblVentasHasta.AutoSize = true;
            this.lblVentasHasta.Location = new System.Drawing.Point(431, 0);
            this.lblVentasHasta.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblVentasHasta.Name = "lblVentasHasta";
            this.lblVentasHasta.Size = new System.Drawing.Size(47, 20);
            this.lblVentasHasta.TabIndex = 4;
            this.lblVentasHasta.Text = "Hasta";
            // dtpVentasHasta
            this.dtpVentasHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpVentasHasta.Location = new System.Drawing.Point(484, 3);
            this.dtpVentasHasta.Name = "dtpVentasHasta";
            this.dtpVentasHasta.Size = new System.Drawing.Size(120, 26);
            this.dtpVentasHasta.TabIndex = 5;
            // lblVentasMes
            this.lblVentasMes.AutoSize = true;
            this.lblVentasMes.Location = new System.Drawing.Point(610, 0);
            this.lblVentasMes.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblVentasMes.Name = "lblVentasMes";
            this.lblVentasMes.Size = new System.Drawing.Size(37, 20);
            this.lblVentasMes.TabIndex = 6;
            this.lblVentasMes.Text = "Mes";
            // dtpVentasMes
            this.dtpVentasMes.CustomFormat = "yyyy-MM";
            this.dtpVentasMes.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpVentasMes.Location = new System.Drawing.Point(653, 3);
            this.dtpVentasMes.Name = "dtpVentasMes";
            this.dtpVentasMes.ShowUpDown = true;
            this.dtpVentasMes.Size = new System.Drawing.Size(110, 26);
            this.dtpVentasMes.TabIndex = 7;
            // lblVentasAnio
            this.lblVentasAnio.AutoSize = true;
            this.lblVentasAnio.Location = new System.Drawing.Point(769, 0);
            this.lblVentasAnio.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblVentasAnio.Name = "lblVentasAnio";
            this.lblVentasAnio.Size = new System.Drawing.Size(37, 20);
            this.lblVentasAnio.TabIndex = 8;
            this.lblVentasAnio.Text = "Año";
            // nudVentasAnio
            this.nudVentasAnio.Location = new System.Drawing.Point(812, 3);
            this.nudVentasAnio.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nudVentasAnio.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudVentasAnio.Name = "nudVentasAnio";
            this.nudVentasAnio.Size = new System.Drawing.Size(90, 26);
            this.nudVentasAnio.TabIndex = 9;
            this.nudVentasAnio.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // btnVentasAplicar
            this.btnVentasAplicar.AutoSize = true;
            this.btnVentasAplicar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnVentasAplicar.Location = new System.Drawing.Point(908, 3);
            this.btnVentasAplicar.Name = "btnVentasAplicar";
            this.btnVentasAplicar.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.btnVentasAplicar.Size = new System.Drawing.Size(87, 33);
            this.btnVentasAplicar.TabIndex = 10;
            this.btnVentasAplicar.Text = "Actualizar";
            this.btnVentasAplicar.UseVisualStyleBackColor = true;
            this.btnVentasAplicar.Click += new System.EventHandler(this.btnVentasAplicar_Click);
            // dgvVentasPeriodo
            this.dgvVentasPeriodo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVentasPeriodo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVentasPeriodo.Location = new System.Drawing.Point(3, 55);
            this.dgvVentasPeriodo.Name = "dgvVentasPeriodo";
            this.dgvVentasPeriodo.RowHeadersVisible = false;
            this.dgvVentasPeriodo.Size = new System.Drawing.Size(998, 620);
            this.dgvVentasPeriodo.TabIndex = 1;
            // tabCategorias
            this.tabCategorias.Controls.Add(this.layoutCategorias);
            this.tabCategorias.Location = new System.Drawing.Point(4, 29);
            this.tabCategorias.Name = "tabCategorias";
            this.tabCategorias.Padding = new System.Windows.Forms.Padding(3);
            this.tabCategorias.Size = new System.Drawing.Size(1010, 684);
            this.tabCategorias.TabIndex = 1;
            this.tabCategorias.Text = "Categorías";
            this.tabCategorias.UseVisualStyleBackColor = true;
            // layoutCategorias
            this.layoutCategorias.ColumnCount = 1;
            this.layoutCategorias.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutCategorias.Controls.Add(this.flowCategorias, 0, 0);
            this.layoutCategorias.Controls.Add(this.dgvCategorias, 0, 1);
            this.layoutCategorias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutCategorias.Location = new System.Drawing.Point(3, 3);
            this.layoutCategorias.Name = "layoutCategorias";
            this.layoutCategorias.RowCount = 2;
            this.layoutCategorias.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layoutCategorias.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutCategorias.Size = new System.Drawing.Size(1004, 678);
            this.layoutCategorias.TabIndex = 0;
            // flowCategorias
            this.flowCategorias.AutoSize = true;
            this.flowCategorias.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowCategorias.Controls.Add(this.lblCategoriasPeriodo);
            this.flowCategorias.Controls.Add(this.cmbCategoriasPeriodo);
            this.flowCategorias.Controls.Add(this.lblCategoriasMes);
            this.flowCategorias.Controls.Add(this.dtpCategoriasMes);
            this.flowCategorias.Controls.Add(this.lblCategoriasAnio);
            this.flowCategorias.Controls.Add(this.nudCategoriasAnio);
            this.flowCategorias.Controls.Add(this.btnCategoriasAplicar);
            this.flowCategorias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowCategorias.Location = new System.Drawing.Point(3, 3);
            this.flowCategorias.Name = "flowCategorias";
            this.flowCategorias.Size = new System.Drawing.Size(998, 40);
            this.flowCategorias.TabIndex = 0;
            // lblCategoriasPeriodo
            this.lblCategoriasPeriodo.AutoSize = true;
            this.lblCategoriasPeriodo.Location = new System.Drawing.Point(3, 0);
            this.lblCategoriasPeriodo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblCategoriasPeriodo.Name = "lblCategoriasPeriodo";
            this.lblCategoriasPeriodo.Size = new System.Drawing.Size(92, 20);
            this.lblCategoriasPeriodo.TabIndex = 0;
            this.lblCategoriasPeriodo.Text = "Tipo período";
            // cmbCategoriasPeriodo
            this.cmbCategoriasPeriodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoriasPeriodo.FormattingEnabled = true;
            this.cmbCategoriasPeriodo.Location = new System.Drawing.Point(101, 3);
            this.cmbCategoriasPeriodo.Name = "cmbCategoriasPeriodo";
            this.cmbCategoriasPeriodo.Size = new System.Drawing.Size(140, 28);
            this.cmbCategoriasPeriodo.TabIndex = 1;
            this.cmbCategoriasPeriodo.SelectedIndexChanged += new System.EventHandler(this.cmbCategoriasPeriodo_SelectedIndexChanged);
            // lblCategoriasMes
            this.lblCategoriasMes.AutoSize = true;
            this.lblCategoriasMes.Location = new System.Drawing.Point(247, 0);
            this.lblCategoriasMes.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblCategoriasMes.Name = "lblCategoriasMes";
            this.lblCategoriasMes.Size = new System.Drawing.Size(37, 20);
            this.lblCategoriasMes.TabIndex = 2;
            this.lblCategoriasMes.Text = "Mes";
            // dtpCategoriasMes
            this.dtpCategoriasMes.CustomFormat = "yyyy-MM";
            this.dtpCategoriasMes.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpCategoriasMes.Location = new System.Drawing.Point(290, 3);
            this.dtpCategoriasMes.Name = "dtpCategoriasMes";
            this.dtpCategoriasMes.ShowUpDown = true;
            this.dtpCategoriasMes.Size = new System.Drawing.Size(110, 26);
            this.dtpCategoriasMes.TabIndex = 3;
            // lblCategoriasAnio
            this.lblCategoriasAnio.AutoSize = true;
            this.lblCategoriasAnio.Location = new System.Drawing.Point(406, 0);
            this.lblCategoriasAnio.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblCategoriasAnio.Name = "lblCategoriasAnio";
            this.lblCategoriasAnio.Size = new System.Drawing.Size(37, 20);
            this.lblCategoriasAnio.TabIndex = 4;
            this.lblCategoriasAnio.Text = "Año";
            // nudCategoriasAnio
            this.nudCategoriasAnio.Location = new System.Drawing.Point(449, 3);
            this.nudCategoriasAnio.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nudCategoriasAnio.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudCategoriasAnio.Name = "nudCategoriasAnio";
            this.nudCategoriasAnio.Size = new System.Drawing.Size(90, 26);
            this.nudCategoriasAnio.TabIndex = 5;
            this.nudCategoriasAnio.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // btnCategoriasAplicar
            this.btnCategoriasAplicar.AutoSize = true;
            this.btnCategoriasAplicar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCategoriasAplicar.Location = new System.Drawing.Point(545, 3);
            this.btnCategoriasAplicar.Name = "btnCategoriasAplicar";
            this.btnCategoriasAplicar.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.btnCategoriasAplicar.Size = new System.Drawing.Size(87, 33);
            this.btnCategoriasAplicar.TabIndex = 6;
            this.btnCategoriasAplicar.Text = "Actualizar";
            this.btnCategoriasAplicar.UseVisualStyleBackColor = true;
            this.btnCategoriasAplicar.Click += new System.EventHandler(this.btnCategoriasAplicar_Click);
            // dgvCategorias
            this.dgvCategorias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategorias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCategorias.Location = new System.Drawing.Point(3, 49);
            this.dgvCategorias.Name = "dgvCategorias";
            this.dgvCategorias.RowHeadersVisible = false;
            this.dgvCategorias.Size = new System.Drawing.Size(998, 626);
            this.dgvCategorias.TabIndex = 1;
            // tabFacturacion
            this.tabFacturacion.Controls.Add(this.layoutFacturacion);
            this.tabFacturacion.Location = new System.Drawing.Point(4, 29);
            this.tabFacturacion.Name = "tabFacturacion";
            this.tabFacturacion.Padding = new System.Windows.Forms.Padding(3);
            this.tabFacturacion.Size = new System.Drawing.Size(1010, 684);
            this.tabFacturacion.TabIndex = 2;
            this.tabFacturacion.Text = "Facturación";
            this.tabFacturacion.UseVisualStyleBackColor = true;
            // layoutFacturacion
            this.layoutFacturacion.ColumnCount = 1;
            this.layoutFacturacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFacturacion.Controls.Add(this.flowFacturacion, 0, 0);
            this.layoutFacturacion.Controls.Add(this.dgvFacturacion, 0, 1);
            this.layoutFacturacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutFacturacion.Location = new System.Drawing.Point(3, 3);
            this.layoutFacturacion.Name = "layoutFacturacion";
            this.layoutFacturacion.RowCount = 2;
            this.layoutFacturacion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layoutFacturacion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFacturacion.Size = new System.Drawing.Size(1004, 678);
            this.layoutFacturacion.TabIndex = 0;
            // flowFacturacion
            this.flowFacturacion.AutoSize = true;
            this.flowFacturacion.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowFacturacion.Controls.Add(this.lblFacturacionPeriodo);
            this.flowFacturacion.Controls.Add(this.cmbFacturacionPeriodo);
            this.flowFacturacion.Controls.Add(this.lblFacturacionMes);
            this.flowFacturacion.Controls.Add(this.dtpFacturacionMes);
            this.flowFacturacion.Controls.Add(this.lblFacturacionAnio);
            this.flowFacturacion.Controls.Add(this.nudFacturacionAnio);
            this.flowFacturacion.Controls.Add(this.btnFacturacionAplicar);
            this.flowFacturacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowFacturacion.Location = new System.Drawing.Point(3, 3);
            this.flowFacturacion.Name = "flowFacturacion";
            this.flowFacturacion.Size = new System.Drawing.Size(998, 40);
            this.flowFacturacion.TabIndex = 0;
            // lblFacturacionPeriodo
            this.lblFacturacionPeriodo.AutoSize = true;
            this.lblFacturacionPeriodo.Location = new System.Drawing.Point(3, 0);
            this.lblFacturacionPeriodo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblFacturacionPeriodo.Name = "lblFacturacionPeriodo";
            this.lblFacturacionPeriodo.Size = new System.Drawing.Size(92, 20);
            this.lblFacturacionPeriodo.TabIndex = 0;
            this.lblFacturacionPeriodo.Text = "Tipo período";
            // cmbFacturacionPeriodo
            this.cmbFacturacionPeriodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFacturacionPeriodo.FormattingEnabled = true;
            this.cmbFacturacionPeriodo.Location = new System.Drawing.Point(101, 3);
            this.cmbFacturacionPeriodo.Name = "cmbFacturacionPeriodo";
            this.cmbFacturacionPeriodo.Size = new System.Drawing.Size(140, 28);
            this.cmbFacturacionPeriodo.TabIndex = 1;
            this.cmbFacturacionPeriodo.SelectedIndexChanged += new System.EventHandler(this.cmbFacturacionPeriodo_SelectedIndexChanged);
            // lblFacturacionMes
            this.lblFacturacionMes.AutoSize = true;
            this.lblFacturacionMes.Location = new System.Drawing.Point(247, 0);
            this.lblFacturacionMes.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblFacturacionMes.Name = "lblFacturacionMes";
            this.lblFacturacionMes.Size = new System.Drawing.Size(37, 20);
            this.lblFacturacionMes.TabIndex = 2;
            this.lblFacturacionMes.Text = "Mes";
            // dtpFacturacionMes
            this.dtpFacturacionMes.CustomFormat = "yyyy-MM";
            this.dtpFacturacionMes.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFacturacionMes.Location = new System.Drawing.Point(290, 3);
            this.dtpFacturacionMes.Name = "dtpFacturacionMes";
            this.dtpFacturacionMes.ShowUpDown = true;
            this.dtpFacturacionMes.Size = new System.Drawing.Size(110, 26);
            this.dtpFacturacionMes.TabIndex = 3;
            // lblFacturacionAnio
            this.lblFacturacionAnio.AutoSize = true;
            this.lblFacturacionAnio.Location = new System.Drawing.Point(406, 0);
            this.lblFacturacionAnio.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblFacturacionAnio.Name = "lblFacturacionAnio";
            this.lblFacturacionAnio.Size = new System.Drawing.Size(37, 20);
            this.lblFacturacionAnio.TabIndex = 4;
            this.lblFacturacionAnio.Text = "Año";
            // nudFacturacionAnio
            this.nudFacturacionAnio.Location = new System.Drawing.Point(449, 3);
            this.nudFacturacionAnio.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nudFacturacionAnio.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudFacturacionAnio.Name = "nudFacturacionAnio";
            this.nudFacturacionAnio.Size = new System.Drawing.Size(90, 26);
            this.nudFacturacionAnio.TabIndex = 5;
            this.nudFacturacionAnio.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // btnFacturacionAplicar
            this.btnFacturacionAplicar.AutoSize = true;
            this.btnFacturacionAplicar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnFacturacionAplicar.Location = new System.Drawing.Point(545, 3);
            this.btnFacturacionAplicar.Name = "btnFacturacionAplicar";
            this.btnFacturacionAplicar.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.btnFacturacionAplicar.Size = new System.Drawing.Size(87, 33);
            this.btnFacturacionAplicar.TabIndex = 6;
            this.btnFacturacionAplicar.Text = "Actualizar";
            this.btnFacturacionAplicar.UseVisualStyleBackColor = true;
            this.btnFacturacionAplicar.Click += new System.EventHandler(this.btnFacturacionAplicar_Click);
            // dgvFacturacion
            this.dgvFacturacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFacturacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFacturacion.Location = new System.Drawing.Point(3, 49);
            this.dgvFacturacion.Name = "dgvFacturacion";
            this.dgvFacturacion.RowHeadersVisible = false;
            this.dgvFacturacion.Size = new System.Drawing.Size(998, 626);
            this.dgvFacturacion.TabIndex = 1;
            // tabPedidosCliente
            this.tabPedidosCliente.Controls.Add(this.layoutPedidosCliente);
            this.tabPedidosCliente.Location = new System.Drawing.Point(4, 29);
            this.tabPedidosCliente.Name = "tabPedidosCliente";
            this.tabPedidosCliente.Padding = new System.Windows.Forms.Padding(3);
            this.tabPedidosCliente.Size = new System.Drawing.Size(1010, 684);
            this.tabPedidosCliente.TabIndex = 3;
            this.tabPedidosCliente.Text = "Pedidos cliente";
            this.tabPedidosCliente.UseVisualStyleBackColor = true;
            // layoutPedidosCliente
            this.layoutPedidosCliente.ColumnCount = 1;
            this.layoutPedidosCliente.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPedidosCliente.Controls.Add(this.flowPedidosCliente, 0, 0);
            this.layoutPedidosCliente.Controls.Add(this.dgvPedidosCliente, 0, 1);
            this.layoutPedidosCliente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPedidosCliente.Location = new System.Drawing.Point(3, 3);
            this.layoutPedidosCliente.Name = "layoutPedidosCliente";
            this.layoutPedidosCliente.RowCount = 2;
            this.layoutPedidosCliente.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layoutPedidosCliente.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPedidosCliente.Size = new System.Drawing.Size(1004, 678);
            this.layoutPedidosCliente.TabIndex = 0;
            // flowPedidosCliente
            this.flowPedidosCliente.AutoSize = true;
            this.flowPedidosCliente.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowPedidosCliente.Controls.Add(this.lblPedidosClientePeriodo);
            this.flowPedidosCliente.Controls.Add(this.cmbPedidosClientePeriodo);
            this.flowPedidosCliente.Controls.Add(this.lblPedidosClienteMes);
            this.flowPedidosCliente.Controls.Add(this.dtpPedidosClienteMes);
            this.flowPedidosCliente.Controls.Add(this.lblPedidosClienteAnio);
            this.flowPedidosCliente.Controls.Add(this.nudPedidosClienteAnio);
            this.flowPedidosCliente.Controls.Add(this.chkPedidosClienteSaldo);
            this.flowPedidosCliente.Controls.Add(this.btnPedidosClienteAplicar);
            this.flowPedidosCliente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPedidosCliente.Location = new System.Drawing.Point(3, 3);
            this.flowPedidosCliente.Name = "flowPedidosCliente";
            this.flowPedidosCliente.Size = new System.Drawing.Size(998, 46);
            this.flowPedidosCliente.TabIndex = 0;
            // lblPedidosClientePeriodo
            this.lblPedidosClientePeriodo.AutoSize = true;
            this.lblPedidosClientePeriodo.Location = new System.Drawing.Point(3, 0);
            this.lblPedidosClientePeriodo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblPedidosClientePeriodo.Name = "lblPedidosClientePeriodo";
            this.lblPedidosClientePeriodo.Size = new System.Drawing.Size(92, 20);
            this.lblPedidosClientePeriodo.TabIndex = 0;
            this.lblPedidosClientePeriodo.Text = "Tipo período";
            // cmbPedidosClientePeriodo
            this.cmbPedidosClientePeriodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPedidosClientePeriodo.FormattingEnabled = true;
            this.cmbPedidosClientePeriodo.Location = new System.Drawing.Point(101, 3);
            this.cmbPedidosClientePeriodo.Name = "cmbPedidosClientePeriodo";
            this.cmbPedidosClientePeriodo.Size = new System.Drawing.Size(140, 28);
            this.cmbPedidosClientePeriodo.TabIndex = 1;
            this.cmbPedidosClientePeriodo.SelectedIndexChanged += new System.EventHandler(this.cmbPedidosClientePeriodo_SelectedIndexChanged);
            // lblPedidosClienteMes
            this.lblPedidosClienteMes.AutoSize = true;
            this.lblPedidosClienteMes.Location = new System.Drawing.Point(247, 0);
            this.lblPedidosClienteMes.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblPedidosClienteMes.Name = "lblPedidosClienteMes";
            this.lblPedidosClienteMes.Size = new System.Drawing.Size(37, 20);
            this.lblPedidosClienteMes.TabIndex = 2;
            this.lblPedidosClienteMes.Text = "Mes";
            // dtpPedidosClienteMes
            this.dtpPedidosClienteMes.CustomFormat = "yyyy-MM";
            this.dtpPedidosClienteMes.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPedidosClienteMes.Location = new System.Drawing.Point(290, 3);
            this.dtpPedidosClienteMes.Name = "dtpPedidosClienteMes";
            this.dtpPedidosClienteMes.ShowUpDown = true;
            this.dtpPedidosClienteMes.Size = new System.Drawing.Size(110, 26);
            this.dtpPedidosClienteMes.TabIndex = 3;
            // lblPedidosClienteAnio
            this.lblPedidosClienteAnio.AutoSize = true;
            this.lblPedidosClienteAnio.Location = new System.Drawing.Point(406, 0);
            this.lblPedidosClienteAnio.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblPedidosClienteAnio.Name = "lblPedidosClienteAnio";
            this.lblPedidosClienteAnio.Size = new System.Drawing.Size(37, 20);
            this.lblPedidosClienteAnio.TabIndex = 4;
            this.lblPedidosClienteAnio.Text = "Año";
            // nudPedidosClienteAnio
            this.nudPedidosClienteAnio.Location = new System.Drawing.Point(449, 3);
            this.nudPedidosClienteAnio.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nudPedidosClienteAnio.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudPedidosClienteAnio.Name = "nudPedidosClienteAnio";
            this.nudPedidosClienteAnio.Size = new System.Drawing.Size(90, 26);
            this.nudPedidosClienteAnio.TabIndex = 5;
            this.nudPedidosClienteAnio.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // chkPedidosClienteSaldo
            this.chkPedidosClienteSaldo.AutoSize = true;
            this.chkPedidosClienteSaldo.Location = new System.Drawing.Point(545, 6);
            this.chkPedidosClienteSaldo.Name = "chkPedidosClienteSaldo";
            this.chkPedidosClienteSaldo.Size = new System.Drawing.Size(127, 24);
            this.chkPedidosClienteSaldo.TabIndex = 6;
            this.chkPedidosClienteSaldo.Text = "Solo con saldo";
            this.chkPedidosClienteSaldo.UseVisualStyleBackColor = true;
            // btnPedidosClienteAplicar
            this.btnPedidosClienteAplicar.AutoSize = true;
            this.btnPedidosClienteAplicar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPedidosClienteAplicar.Location = new System.Drawing.Point(678, 3);
            this.btnPedidosClienteAplicar.Name = "btnPedidosClienteAplicar";
            this.btnPedidosClienteAplicar.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.btnPedidosClienteAplicar.Size = new System.Drawing.Size(87, 33);
            this.btnPedidosClienteAplicar.TabIndex = 7;
            this.btnPedidosClienteAplicar.Text = "Actualizar";
            this.btnPedidosClienteAplicar.UseVisualStyleBackColor = true;
            this.btnPedidosClienteAplicar.Click += new System.EventHandler(this.btnPedidosClienteAplicar_Click);
            // dgvPedidosCliente
            this.dgvPedidosCliente.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPedidosCliente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPedidosCliente.Location = new System.Drawing.Point(3, 55);
            this.dgvPedidosCliente.Name = "dgvPedidosCliente";
            this.dgvPedidosCliente.RowHeadersVisible = false;
            this.dgvPedidosCliente.Size = new System.Drawing.Size(998, 620);
            this.dgvPedidosCliente.TabIndex = 1;
            // tabPedidosProveedor
            this.tabPedidosProveedor.Controls.Add(this.layoutPedidosProveedor);
            this.tabPedidosProveedor.Location = new System.Drawing.Point(4, 29);
            this.tabPedidosProveedor.Name = "tabPedidosProveedor";
            this.tabPedidosProveedor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPedidosProveedor.Size = new System.Drawing.Size(1010, 684);
            this.tabPedidosProveedor.TabIndex = 4;
            this.tabPedidosProveedor.Text = "Pedidos proveedor";
            this.tabPedidosProveedor.UseVisualStyleBackColor = true;
            // layoutPedidosProveedor
            this.layoutPedidosProveedor.ColumnCount = 1;
            this.layoutPedidosProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPedidosProveedor.Controls.Add(this.flowPedidosProveedor, 0, 0);
            this.layoutPedidosProveedor.Controls.Add(this.dgvPedidosProveedor, 0, 1);
            this.layoutPedidosProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPedidosProveedor.Location = new System.Drawing.Point(3, 3);
            this.layoutPedidosProveedor.Name = "layoutPedidosProveedor";
            this.layoutPedidosProveedor.RowCount = 2;
            this.layoutPedidosProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layoutPedidosProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPedidosProveedor.Size = new System.Drawing.Size(1004, 678);
            this.layoutPedidosProveedor.TabIndex = 0;
            // flowPedidosProveedor
            this.flowPedidosProveedor.AutoSize = true;
            this.flowPedidosProveedor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowPedidosProveedor.Controls.Add(this.lblPedidosProveedorPeriodo);
            this.flowPedidosProveedor.Controls.Add(this.cmbPedidosProveedorPeriodo);
            this.flowPedidosProveedor.Controls.Add(this.lblPedidosProveedorMes);
            this.flowPedidosProveedor.Controls.Add(this.dtpPedidosProveedorMes);
            this.flowPedidosProveedor.Controls.Add(this.lblPedidosProveedorAnio);
            this.flowPedidosProveedor.Controls.Add(this.nudPedidosProveedorAnio);
            this.flowPedidosProveedor.Controls.Add(this.btnPedidosProveedorAplicar);
            this.flowPedidosProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPedidosProveedor.Location = new System.Drawing.Point(3, 3);
            this.flowPedidosProveedor.Name = "flowPedidosProveedor";
            this.flowPedidosProveedor.Size = new System.Drawing.Size(998, 40);
            this.flowPedidosProveedor.TabIndex = 0;
            // lblPedidosProveedorPeriodo
            this.lblPedidosProveedorPeriodo.AutoSize = true;
            this.lblPedidosProveedorPeriodo.Location = new System.Drawing.Point(3, 0);
            this.lblPedidosProveedorPeriodo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblPedidosProveedorPeriodo.Name = "lblPedidosProveedorPeriodo";
            this.lblPedidosProveedorPeriodo.Size = new System.Drawing.Size(92, 20);
            this.lblPedidosProveedorPeriodo.TabIndex = 0;
            this.lblPedidosProveedorPeriodo.Text = "Tipo período";
            // cmbPedidosProveedorPeriodo
            this.cmbPedidosProveedorPeriodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPedidosProveedorPeriodo.FormattingEnabled = true;
            this.cmbPedidosProveedorPeriodo.Location = new System.Drawing.Point(101, 3);
            this.cmbPedidosProveedorPeriodo.Name = "cmbPedidosProveedorPeriodo";
            this.cmbPedidosProveedorPeriodo.Size = new System.Drawing.Size(140, 28);
            this.cmbPedidosProveedorPeriodo.TabIndex = 1;
            this.cmbPedidosProveedorPeriodo.SelectedIndexChanged += new System.EventHandler(this.cmbPedidosProveedorPeriodo_SelectedIndexChanged);
            // lblPedidosProveedorMes
            this.lblPedidosProveedorMes.AutoSize = true;
            this.lblPedidosProveedorMes.Location = new System.Drawing.Point(247, 0);
            this.lblPedidosProveedorMes.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblPedidosProveedorMes.Name = "lblPedidosProveedorMes";
            this.lblPedidosProveedorMes.Size = new System.Drawing.Size(37, 20);
            this.lblPedidosProveedorMes.TabIndex = 2;
            this.lblPedidosProveedorMes.Text = "Mes";
            // dtpPedidosProveedorMes
            this.dtpPedidosProveedorMes.CustomFormat = "yyyy-MM";
            this.dtpPedidosProveedorMes.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPedidosProveedorMes.Location = new System.Drawing.Point(290, 3);
            this.dtpPedidosProveedorMes.Name = "dtpPedidosProveedorMes";
            this.dtpPedidosProveedorMes.ShowUpDown = true;
            this.dtpPedidosProveedorMes.Size = new System.Drawing.Size(110, 26);
            this.dtpPedidosProveedorMes.TabIndex = 3;
            // lblPedidosProveedorAnio
            this.lblPedidosProveedorAnio.AutoSize = true;
            this.lblPedidosProveedorAnio.Location = new System.Drawing.Point(406, 0);
            this.lblPedidosProveedorAnio.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblPedidosProveedorAnio.Name = "lblPedidosProveedorAnio";
            this.lblPedidosProveedorAnio.Size = new System.Drawing.Size(37, 20);
            this.lblPedidosProveedorAnio.TabIndex = 4;
            this.lblPedidosProveedorAnio.Text = "Año";
            // nudPedidosProveedorAnio
            this.nudPedidosProveedorAnio.Location = new System.Drawing.Point(449, 3);
            this.nudPedidosProveedorAnio.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nudPedidosProveedorAnio.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudPedidosProveedorAnio.Name = "nudPedidosProveedorAnio";
            this.nudPedidosProveedorAnio.Size = new System.Drawing.Size(90, 26);
            this.nudPedidosProveedorAnio.TabIndex = 5;
            this.nudPedidosProveedorAnio.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // btnPedidosProveedorAplicar
            this.btnPedidosProveedorAplicar.AutoSize = true;
            this.btnPedidosProveedorAplicar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPedidosProveedorAplicar.Location = new System.Drawing.Point(545, 3);
            this.btnPedidosProveedorAplicar.Name = "btnPedidosProveedorAplicar";
            this.btnPedidosProveedorAplicar.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.btnPedidosProveedorAplicar.Size = new System.Drawing.Size(87, 33);
            this.btnPedidosProveedorAplicar.TabIndex = 6;
            this.btnPedidosProveedorAplicar.Text = "Actualizar";
            this.btnPedidosProveedorAplicar.UseVisualStyleBackColor = true;
            this.btnPedidosProveedorAplicar.Click += new System.EventHandler(this.btnPedidosProveedorAplicar_Click);
            // dgvPedidosProveedor
            this.dgvPedidosProveedor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPedidosProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPedidosProveedor.Location = new System.Drawing.Point(3, 49);
            this.dgvPedidosProveedor.Name = "dgvPedidosProveedor";
            this.dgvPedidosProveedor.RowHeadersVisible = false;
            this.dgvPedidosProveedor.Size = new System.Drawing.Size(998, 626);
            this.dgvPedidosProveedor.TabIndex = 1;
            // tabMejoresClientes
            this.tabMejoresClientes.Controls.Add(this.layoutMejoresClientes);
            this.tabMejoresClientes.Location = new System.Drawing.Point(4, 29);
            this.tabMejoresClientes.Name = "tabMejoresClientes";
            this.tabMejoresClientes.Padding = new System.Windows.Forms.Padding(3);
            this.tabMejoresClientes.Size = new System.Drawing.Size(1010, 684);
            this.tabMejoresClientes.TabIndex = 5;
            this.tabMejoresClientes.Text = "Mejores clientes";
            this.tabMejoresClientes.UseVisualStyleBackColor = true;
            // layoutMejoresClientes
            this.layoutMejoresClientes.ColumnCount = 1;
            this.layoutMejoresClientes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMejoresClientes.Controls.Add(this.flowMejoresClientes, 0, 0);
            this.layoutMejoresClientes.Controls.Add(this.dgvMejoresClientes, 0, 1);
            this.layoutMejoresClientes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMejoresClientes.Location = new System.Drawing.Point(3, 3);
            this.layoutMejoresClientes.Name = "layoutMejoresClientes";
            this.layoutMejoresClientes.RowCount = 2;
            this.layoutMejoresClientes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layoutMejoresClientes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMejoresClientes.Size = new System.Drawing.Size(1004, 678);
            this.layoutMejoresClientes.TabIndex = 0;
            // flowMejoresClientes
            this.flowMejoresClientes.AutoSize = true;
            this.flowMejoresClientes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowMejoresClientes.Controls.Add(this.lblMejoresClientesPeriodo);
            this.flowMejoresClientes.Controls.Add(this.cmbMejoresClientesPeriodo);
            this.flowMejoresClientes.Controls.Add(this.lblMejoresClientesMes);
            this.flowMejoresClientes.Controls.Add(this.nudMejoresClientesMes);
            this.flowMejoresClientes.Controls.Add(this.lblMejoresClientesAnio);
            this.flowMejoresClientes.Controls.Add(this.nudMejoresClientesAnio);
            this.flowMejoresClientes.Controls.Add(this.btnMejoresClientesAplicar);
            this.flowMejoresClientes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowMejoresClientes.Location = new System.Drawing.Point(3, 3);
            this.flowMejoresClientes.Name = "flowMejoresClientes";
            this.flowMejoresClientes.Size = new System.Drawing.Size(998, 40);
            this.flowMejoresClientes.TabIndex = 0;
            // lblMejoresClientesPeriodo
            this.lblMejoresClientesPeriodo.AutoSize = true;
            this.lblMejoresClientesPeriodo.Location = new System.Drawing.Point(3, 0);
            this.lblMejoresClientesPeriodo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblMejoresClientesPeriodo.Name = "lblMejoresClientesPeriodo";
            this.lblMejoresClientesPeriodo.Size = new System.Drawing.Size(92, 20);
            this.lblMejoresClientesPeriodo.TabIndex = 0;
            this.lblMejoresClientesPeriodo.Text = "Tipo período";
            // cmbMejoresClientesPeriodo
            this.cmbMejoresClientesPeriodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMejoresClientesPeriodo.FormattingEnabled = true;
            this.cmbMejoresClientesPeriodo.Location = new System.Drawing.Point(101, 3);
            this.cmbMejoresClientesPeriodo.Name = "cmbMejoresClientesPeriodo";
            this.cmbMejoresClientesPeriodo.Size = new System.Drawing.Size(140, 28);
            this.cmbMejoresClientesPeriodo.TabIndex = 1;
            this.cmbMejoresClientesPeriodo.SelectedIndexChanged += new System.EventHandler(this.cmbMejoresClientesPeriodo_SelectedIndexChanged);
            // lblMejoresClientesMes
            this.lblMejoresClientesMes.AutoSize = true;
            this.lblMejoresClientesMes.Location = new System.Drawing.Point(247, 0);
            this.lblMejoresClientesMes.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblMejoresClientesMes.Name = "lblMejoresClientesMes";
            this.lblMejoresClientesMes.Size = new System.Drawing.Size(37, 20);
            this.lblMejoresClientesMes.TabIndex = 2;
            this.lblMejoresClientesMes.Text = "Mes";
            // nudMejoresClientesMes
            this.nudMejoresClientesMes.Location = new System.Drawing.Point(290, 3);
            this.nudMejoresClientesMes.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nudMejoresClientesMes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMejoresClientesMes.Name = "nudMejoresClientesMes";
            this.nudMejoresClientesMes.Size = new System.Drawing.Size(90, 26);
            this.nudMejoresClientesMes.TabIndex = 3;
            this.nudMejoresClientesMes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // lblMejoresClientesAnio
            this.lblMejoresClientesAnio.AutoSize = true;
            this.lblMejoresClientesAnio.Location = new System.Drawing.Point(406, 0);
            this.lblMejoresClientesAnio.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblMejoresClientesAnio.Name = "lblMejoresClientesAnio";
            this.lblMejoresClientesAnio.Size = new System.Drawing.Size(37, 20);
            this.lblMejoresClientesAnio.TabIndex = 4;
            this.lblMejoresClientesAnio.Text = "Año";
            // nudMejoresClientesAnio
            this.nudMejoresClientesAnio.Location = new System.Drawing.Point(449, 3);
            this.nudMejoresClientesAnio.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nudMejoresClientesAnio.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudMejoresClientesAnio.Name = "nudMejoresClientesAnio";
            this.nudMejoresClientesAnio.Size = new System.Drawing.Size(90, 26);
            this.nudMejoresClientesAnio.TabIndex = 5;
            this.nudMejoresClientesAnio.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // btnMejoresClientesAplicar
            this.btnMejoresClientesAplicar.AutoSize = true;
            this.btnMejoresClientesAplicar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMejoresClientesAplicar.Location = new System.Drawing.Point(545, 3);
            this.btnMejoresClientesAplicar.Name = "btnMejoresClientesAplicar";
            this.btnMejoresClientesAplicar.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.btnMejoresClientesAplicar.Size = new System.Drawing.Size(87, 33);
            this.btnMejoresClientesAplicar.TabIndex = 6;
            this.btnMejoresClientesAplicar.Text = "Actualizar";
            this.btnMejoresClientesAplicar.UseVisualStyleBackColor = true;
            this.btnMejoresClientesAplicar.Click += new System.EventHandler(this.btnMejoresClientesAplicar_Click);
            // dgvMejoresClientes
            this.dgvMejoresClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMejoresClientes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMejoresClientes.Location = new System.Drawing.Point(3, 49);
            this.dgvMejoresClientes.Name = "dgvMejoresClientes";
            this.dgvMejoresClientes.RowHeadersVisible = false;
            this.dgvMejoresClientes.Size = new System.Drawing.Size(998, 626);
            this.dgvMejoresClientes.TabIndex = 1;
            // tabClientesSaldo
            this.tabClientesSaldo.Controls.Add(this.layoutClientesSaldo);
            this.tabClientesSaldo.Location = new System.Drawing.Point(4, 29);
            this.tabClientesSaldo.Name = "tabClientesSaldo";
            this.tabClientesSaldo.Padding = new System.Windows.Forms.Padding(3);
            this.tabClientesSaldo.Size = new System.Drawing.Size(1010, 684);
            this.tabClientesSaldo.TabIndex = 6;
            this.tabClientesSaldo.Text = "Clientes con saldo";
            this.tabClientesSaldo.UseVisualStyleBackColor = true;
            // layoutClientesSaldo
            this.layoutClientesSaldo.ColumnCount = 1;
            this.layoutClientesSaldo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutClientesSaldo.Controls.Add(this.flowClientesSaldo, 0, 0);
            this.layoutClientesSaldo.Controls.Add(this.dgvClientesSaldo, 0, 1);
            this.layoutClientesSaldo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutClientesSaldo.Location = new System.Drawing.Point(3, 3);
            this.layoutClientesSaldo.Name = "layoutClientesSaldo";
            this.layoutClientesSaldo.RowCount = 2;
            this.layoutClientesSaldo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layoutClientesSaldo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutClientesSaldo.Size = new System.Drawing.Size(1004, 678);
            this.layoutClientesSaldo.TabIndex = 0;
            // flowClientesSaldo
            this.flowClientesSaldo.AutoSize = true;
            this.flowClientesSaldo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowClientesSaldo.Controls.Add(this.lblClientesSaldoInfo);
            this.flowClientesSaldo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowClientesSaldo.Location = new System.Drawing.Point(3, 3);
            this.flowClientesSaldo.Name = "flowClientesSaldo";
            this.flowClientesSaldo.Size = new System.Drawing.Size(998, 26);
            this.flowClientesSaldo.TabIndex = 0;
            // lblClientesSaldoInfo
            this.lblClientesSaldoInfo.AutoSize = true;
            this.lblClientesSaldoInfo.Location = new System.Drawing.Point(3, 0);
            this.lblClientesSaldoInfo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblClientesSaldoInfo.Name = "lblClientesSaldoInfo";
            this.lblClientesSaldoInfo.Size = new System.Drawing.Size(109, 20);
            this.lblClientesSaldoInfo.TabIndex = 0;
            this.lblClientesSaldoInfo.Text = "Sin filtros extra";
            // dgvClientesSaldo
            this.dgvClientesSaldo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClientesSaldo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvClientesSaldo.Location = new System.Drawing.Point(3, 35);
            this.dgvClientesSaldo.Name = "dgvClientesSaldo";
            this.dgvClientesSaldo.RowHeadersVisible = false;
            this.dgvClientesSaldo.Size = new System.Drawing.Size(998, 640);
            this.dgvClientesSaldo.TabIndex = 1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.layoutPrincipal);
            this.Name = "ReportesForm";
            this.Text = "Reportes";
            this.Load += new System.EventHandler(this.ReportesForm_Load);
            this.layoutPrincipal.ResumeLayout(false);
            this.layoutPrincipal.PerformLayout();
            this.flowAcciones.ResumeLayout(false);
            this.flowAcciones.PerformLayout();
            this.tabReportes.ResumeLayout(false);
            this.tabVentasPeriodo.ResumeLayout(false);
            this.layoutVentasPeriodo.ResumeLayout(false);
            this.layoutVentasPeriodo.PerformLayout();
            this.flowVentasPeriodo.ResumeLayout(false);
            this.flowVentasPeriodo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVentasAnio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasPeriodo)).EndInit();
            this.tabCategorias.ResumeLayout(false);
            this.layoutCategorias.ResumeLayout(false);
            this.layoutCategorias.PerformLayout();
            this.flowCategorias.ResumeLayout(false);
            this.flowCategorias.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCategoriasAnio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategorias)).EndInit();
            this.tabFacturacion.ResumeLayout(false);
            this.layoutFacturacion.ResumeLayout(false);
            this.layoutFacturacion.PerformLayout();
            this.flowFacturacion.ResumeLayout(false);
            this.flowFacturacion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFacturacionAnio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFacturacion)).EndInit();
            this.tabPedidosCliente.ResumeLayout(false);
            this.layoutPedidosCliente.ResumeLayout(false);
            this.layoutPedidosCliente.PerformLayout();
            this.flowPedidosCliente.ResumeLayout(false);
            this.flowPedidosCliente.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPedidosClienteAnio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidosCliente)).EndInit();
            this.tabPedidosProveedor.ResumeLayout(false);
            this.layoutPedidosProveedor.ResumeLayout(false);
            this.layoutPedidosProveedor.PerformLayout();
            this.flowPedidosProveedor.ResumeLayout(false);
            this.flowPedidosProveedor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPedidosProveedorAnio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidosProveedor)).EndInit();
            this.tabMejoresClientes.ResumeLayout(false);
            this.layoutMejoresClientes.ResumeLayout(false);
            this.layoutMejoresClientes.PerformLayout();
            this.flowMejoresClientes.ResumeLayout(false);
            this.flowMejoresClientes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMejoresClientesMes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMejoresClientesAnio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMejoresClientes)).EndInit();
            this.tabClientesSaldo.ResumeLayout(false);
            this.layoutClientesSaldo.ResumeLayout(false);
            this.layoutClientesSaldo.PerformLayout();
            this.flowClientesSaldo.ResumeLayout(false);
            this.flowClientesSaldo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientesSaldo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}