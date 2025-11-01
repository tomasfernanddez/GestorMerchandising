namespace UI.Formularios
{
    partial class ReportesForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel layoutPrincipal;
        private System.Windows.Forms.GroupBox grpFiltros;
        private System.Windows.Forms.FlowLayoutPanel flowFiltros;
        private System.Windows.Forms.Label lblDiasLimite;
        private System.Windows.Forms.NumericUpDown nudDiasLimite;
        private System.Windows.Forms.Label lblMesComparativo;
        private System.Windows.Forms.DateTimePicker dtpMesComparativo;
        private System.Windows.Forms.Label lblAnioComparativo;
        private System.Windows.Forms.NumericUpDown nudAnioComparativo;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Button btnExportRaw;
        private System.Windows.Forms.TabControl tabPrincipal;
        private System.Windows.Forms.TabPage tabPageOperativos;
        private System.Windows.Forms.TabPage tabPageVentas;
        private System.Windows.Forms.TabPage tabPageFinancieros;
        private System.Windows.Forms.TabControl tabOperativos;
        private System.Windows.Forms.TabPage tabOperativosEstadoPage;
        private System.Windows.Forms.TabPage tabOperativosVencimientoPage;
        private System.Windows.Forms.TabPage tabOperativosDemorasPage;
        private System.Windows.Forms.TabPage tabOperativosMuestrasPage;
        private System.Windows.Forms.TabPage tabOperativosSaldoPage;
        private System.Windows.Forms.TabPage tabOperativosProduccionPage;
        private System.Windows.Forms.DataGridView dgvOperativosEstado;
        private System.Windows.Forms.DataGridView dgvOperativosLimite;
        private System.Windows.Forms.DataGridView dgvOperativosDemoras;
        private System.Windows.Forms.DataGridView dgvOperativosMuestras;
        private System.Windows.Forms.DataGridView dgvOperativosSaldo;
        private System.Windows.Forms.DataGridView dgvOperativosProduccion;
        private System.Windows.Forms.TabControl tabVentas;
        private System.Windows.Forms.TabPage tabVentasMensualesPage;
        private System.Windows.Forms.TabPage tabVentasTrimestralesPage;
        private System.Windows.Forms.TabPage tabVentasAnualesPage;
        private System.Windows.Forms.TabPage tabVentasComparativasPage;
        private System.Windows.Forms.TabPage tabVentasRankingPage;
        private System.Windows.Forms.TabPage tabVentasCategoriasPage;
        private System.Windows.Forms.DataGridView dgvVentasMensuales;
        private System.Windows.Forms.DataGridView dgvVentasTrimestrales;
        private System.Windows.Forms.DataGridView dgvVentasAnuales;
        private System.Windows.Forms.DataGridView dgvVentasComparativas;
        private System.Windows.Forms.DataGridView dgvVentasRanking;
        private System.Windows.Forms.DataGridView dgvVentasCategorias;
        private System.Windows.Forms.TabControl tabFinancieros;
        private System.Windows.Forms.TabPage tabFinanzasFacturacionPage;
        private System.Windows.Forms.TabPage tabFinanzasCuentasPage;
        private System.Windows.Forms.TabPage tabFinanzasPagosPage;
        private System.Windows.Forms.TabPage tabFinanzasProyeccionPage;
        private System.Windows.Forms.DataGridView dgvFinanzasFacturacion;
        private System.Windows.Forms.DataGridView dgvFinanzasCuentas;
        private System.Windows.Forms.DataGridView dgvFinanzasPagos;
        private System.Windows.Forms.DataGridView dgvFinanzasProyeccion;

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
            this.components = new System.ComponentModel.Container();
            this.layoutPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.grpFiltros = new System.Windows.Forms.GroupBox();
            this.flowFiltros = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDiasLimite = new System.Windows.Forms.Label();
            this.nudDiasLimite = new System.Windows.Forms.NumericUpDown();
            this.lblMesComparativo = new System.Windows.Forms.Label();
            this.dtpMesComparativo = new System.Windows.Forms.DateTimePicker();
            this.lblAnioComparativo = new System.Windows.Forms.Label();
            this.nudAnioComparativo = new System.Windows.Forms.NumericUpDown();
            this.lblDesde = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.btnExportRaw = new System.Windows.Forms.Button();
            this.tabPrincipal = new System.Windows.Forms.TabControl();
            this.tabPageOperativos = new System.Windows.Forms.TabPage();
            this.tabOperativos = new System.Windows.Forms.TabControl();
            this.tabOperativosEstadoPage = new System.Windows.Forms.TabPage();
            this.dgvOperativosEstado = new System.Windows.Forms.DataGridView();
            this.tabOperativosVencimientoPage = new System.Windows.Forms.TabPage();
            this.dgvOperativosLimite = new System.Windows.Forms.DataGridView();
            this.tabOperativosDemorasPage = new System.Windows.Forms.TabPage();
            this.dgvOperativosDemoras = new System.Windows.Forms.DataGridView();
            this.tabOperativosMuestrasPage = new System.Windows.Forms.TabPage();
            this.dgvOperativosMuestras = new System.Windows.Forms.DataGridView();
            this.tabOperativosSaldoPage = new System.Windows.Forms.TabPage();
            this.dgvOperativosSaldo = new System.Windows.Forms.DataGridView();
            this.tabOperativosProduccionPage = new System.Windows.Forms.TabPage();
            this.dgvOperativosProduccion = new System.Windows.Forms.DataGridView();
            this.tabPageVentas = new System.Windows.Forms.TabPage();
            this.tabVentas = new System.Windows.Forms.TabControl();
            this.tabVentasMensualesPage = new System.Windows.Forms.TabPage();
            this.dgvVentasMensuales = new System.Windows.Forms.DataGridView();
            this.tabVentasTrimestralesPage = new System.Windows.Forms.TabPage();
            this.dgvVentasTrimestrales = new System.Windows.Forms.DataGridView();
            this.tabVentasAnualesPage = new System.Windows.Forms.TabPage();
            this.dgvVentasAnuales = new System.Windows.Forms.DataGridView();
            this.tabVentasComparativasPage = new System.Windows.Forms.TabPage();
            this.dgvVentasComparativas = new System.Windows.Forms.DataGridView();
            this.tabVentasRankingPage = new System.Windows.Forms.TabPage();
            this.dgvVentasRanking = new System.Windows.Forms.DataGridView();
            this.tabVentasCategoriasPage = new System.Windows.Forms.TabPage();
            this.dgvVentasCategorias = new System.Windows.Forms.DataGridView();
            this.tabPageFinancieros = new System.Windows.Forms.TabPage();
            this.tabFinancieros = new System.Windows.Forms.TabControl();
            this.tabFinanzasFacturacionPage = new System.Windows.Forms.TabPage();
            this.dgvFinanzasFacturacion = new System.Windows.Forms.DataGridView();
            this.tabFinanzasCuentasPage = new System.Windows.Forms.TabPage();
            this.dgvFinanzasCuentas = new System.Windows.Forms.DataGridView();
            this.tabFinanzasPagosPage = new System.Windows.Forms.TabPage();
            this.dgvFinanzasPagos = new System.Windows.Forms.DataGridView();
            this.tabFinanzasProyeccionPage = new System.Windows.Forms.TabPage();
            this.dgvFinanzasProyeccion = new System.Windows.Forms.DataGridView();
            this.layoutPrincipal.SuspendLayout();
            this.grpFiltros.SuspendLayout();
            this.flowFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiasLimite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAnioComparativo)).BeginInit();
            this.tabPrincipal.SuspendLayout();
            this.tabPageOperativos.SuspendLayout();
            this.tabOperativos.SuspendLayout();
            this.tabOperativosEstadoPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperativosEstado)).BeginInit();
            this.tabOperativosVencimientoPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperativosLimite)).BeginInit();
            this.tabOperativosDemorasPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperativosDemoras)).BeginInit();
            this.tabOperativosMuestrasPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperativosMuestras)).BeginInit();
            this.tabOperativosSaldoPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperativosSaldo)).BeginInit();
            this.tabOperativosProduccionPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperativosProduccion)).BeginInit();
            this.tabPageVentas.SuspendLayout();
            this.tabVentas.SuspendLayout();
            this.tabVentasMensualesPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasMensuales)).BeginInit();
            this.tabVentasTrimestralesPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasTrimestrales)).BeginInit();
            this.tabVentasAnualesPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasAnuales)).BeginInit();
            this.tabVentasComparativasPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasComparativas)).BeginInit();
            this.tabVentasRankingPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasRanking)).BeginInit();
            this.tabVentasCategoriasPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasCategorias)).BeginInit();
            this.tabPageFinancieros.SuspendLayout();
            this.tabFinancieros.SuspendLayout();
            this.tabFinanzasFacturacionPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinanzasFacturacion)).BeginInit();
            this.tabFinanzasCuentasPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinanzasCuentas)).BeginInit();
            this.tabFinanzasPagosPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinanzasPagos)).BeginInit();
            this.tabFinanzasProyeccionPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinanzasProyeccion)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutPrincipal
            // 
            this.layoutPrincipal.ColumnCount = 1;
            this.layoutPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPrincipal.Controls.Add(this.grpFiltros, 0, 0);
            this.layoutPrincipal.Controls.Add(this.tabPrincipal, 0, 1);
            this.layoutPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPrincipal.Location = new System.Drawing.Point(0, 0);
            this.layoutPrincipal.Name = "layoutPrincipal";
            this.layoutPrincipal.RowCount = 2;
            this.layoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPrincipal.Size = new System.Drawing.Size(1024, 768);
            this.layoutPrincipal.TabIndex = 0;
            // 
            // grpFiltros
            // 
            this.grpFiltros.Controls.Add(this.flowFiltros);
            this.grpFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpFiltros.Location = new System.Drawing.Point(3, 3);
            this.grpFiltros.Name = "grpFiltros";
            this.grpFiltros.Size = new System.Drawing.Size(1018, 84);
            this.grpFiltros.TabIndex = 0;
            this.grpFiltros.TabStop = false;
            this.grpFiltros.Text = "Parámetros";
            // 
            // flowFiltros
            // 
            this.flowFiltros.AutoSize = true;
            this.flowFiltros.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowFiltros.Location = new System.Drawing.Point(3, 16);
            this.flowFiltros.Name = "flowFiltros";
            this.flowFiltros.Padding = new System.Windows.Forms.Padding(8);
            this.flowFiltros.Size = new System.Drawing.Size(1012, 65);
            this.flowFiltros.TabIndex = 0;
            this.flowFiltros.WrapContents = true;
            // 
            // lblDiasLimite
            // 
            this.lblDiasLimite.AutoSize = true;
            this.lblDiasLimite.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.lblDiasLimite.Name = "lblDiasLimite";
            this.lblDiasLimite.Size = new System.Drawing.Size(63, 13);
            this.lblDiasLimite.TabIndex = 0;
            this.lblDiasLimite.Text = "Días límite";
            // 
            // nudDiasLimite
            // 
            this.nudDiasLimite.Margin = new System.Windows.Forms.Padding(5, 5, 15, 5);
            this.nudDiasLimite.Name = "nudDiasLimite";
            this.nudDiasLimite.Size = new System.Drawing.Size(60, 20);
            this.nudDiasLimite.TabIndex = 1;
            // 
            // lblMesComparativo
            // 
            this.lblMesComparativo.AutoSize = true;
            this.lblMesComparativo.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.lblMesComparativo.Name = "lblMesComparativo";
            this.lblMesComparativo.Size = new System.Drawing.Size(90, 13);
            this.lblMesComparativo.TabIndex = 2;
            this.lblMesComparativo.Text = "Mes comparativo";
            // 
            // dtpMesComparativo
            // 
            this.dtpMesComparativo.Margin = new System.Windows.Forms.Padding(5, 5, 15, 5);
            this.dtpMesComparativo.Name = "dtpMesComparativo";
            this.dtpMesComparativo.Size = new System.Drawing.Size(110, 20);
            this.dtpMesComparativo.TabIndex = 3;
            // 
            // lblAnioComparativo
            // 
            this.lblAnioComparativo.AutoSize = true;
            this.lblAnioComparativo.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.lblAnioComparativo.Name = "lblAnioComparativo";
            this.lblAnioComparativo.Size = new System.Drawing.Size(88, 13);
            this.lblAnioComparativo.TabIndex = 4;
            this.lblAnioComparativo.Text = "Año comparativo";
            // 
            // nudAnioComparativo
            // 
            this.nudAnioComparativo.Margin = new System.Windows.Forms.Padding(5, 5, 15, 5);
            this.nudAnioComparativo.Name = "nudAnioComparativo";
            this.nudAnioComparativo.Size = new System.Drawing.Size(80, 20);
            this.nudAnioComparativo.TabIndex = 5;
            // 
            // lblDesde
            // 
            this.lblDesde.AutoSize = true;
            this.lblDesde.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(38, 13);
            this.lblDesde.TabIndex = 6;
            this.lblDesde.Text = "Desde";
            // 
            // dtpDesde
            // 
            this.dtpDesde.Margin = new System.Windows.Forms.Padding(5, 5, 15, 5);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(120, 20);
            this.dtpDesde.TabIndex = 7;
            // 
            // lblHasta
            // 
            this.lblHasta.AutoSize = true;
            this.lblHasta.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(35, 13);
            this.lblHasta.TabIndex = 8;
            this.lblHasta.Text = "Hasta";
            // 
            // dtpHasta
            // 
            this.dtpHasta.Margin = new System.Windows.Forms.Padding(5, 5, 15, 5);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(120, 20);
            this.dtpHasta.TabIndex = 9;
            // 
            // btnActualizar
            // 
            this.btnActualizar.AutoSize = true;
            this.btnActualizar.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(75, 23);
            this.btnActualizar.TabIndex = 10;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.AutoSize = true;
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(130, 23);
            this.btnExportExcel.TabIndex = 11;
            this.btnExportExcel.Text = "Exportar Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.AutoSize = true;
            this.btnExportPdf.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(110, 23);
            this.btnExportPdf.TabIndex = 12;
            this.btnExportPdf.Text = "Exportar PDF";
            this.btnExportPdf.UseVisualStyleBackColor = true;
            this.btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);
            // 
            // btnExportRaw
            // 
            this.btnExportRaw.AutoSize = true;
            this.btnExportRaw.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnExportRaw.Name = "btnExportRaw";
            this.btnExportRaw.Size = new System.Drawing.Size(150, 23);
            this.btnExportRaw.TabIndex = 13;
            this.btnExportRaw.Text = "Exportar datos crudos";
            this.btnExportRaw.UseVisualStyleBackColor = true;
            this.btnExportRaw.Click += new System.EventHandler(this.btnExportRaw_Click);
            // 
            // Añadir controles al flow
            // 
            this.flowFiltros.Controls.Add(this.lblDiasLimite);
            this.flowFiltros.Controls.Add(this.nudDiasLimite);
            this.flowFiltros.Controls.Add(this.lblMesComparativo);
            this.flowFiltros.Controls.Add(this.dtpMesComparativo);
            this.flowFiltros.Controls.Add(this.lblAnioComparativo);
            this.flowFiltros.Controls.Add(this.nudAnioComparativo);
            this.flowFiltros.Controls.Add(this.lblDesde);
            this.flowFiltros.Controls.Add(this.dtpDesde);
            this.flowFiltros.Controls.Add(this.lblHasta);
            this.flowFiltros.Controls.Add(this.dtpHasta);
            this.flowFiltros.Controls.Add(this.btnActualizar);
            this.flowFiltros.Controls.Add(this.btnExportExcel);
            this.flowFiltros.Controls.Add(this.btnExportPdf);
            this.flowFiltros.Controls.Add(this.btnExportRaw);
            // 
            // tabPrincipal
            // 
            this.tabPrincipal.Controls.Add(this.tabPageOperativos);
            this.tabPrincipal.Controls.Add(this.tabPageVentas);
            this.tabPrincipal.Controls.Add(this.tabPageFinancieros);
            this.tabPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPrincipal.Location = new System.Drawing.Point(3, 93);
            this.tabPrincipal.Name = "tabPrincipal";
            this.tabPrincipal.SelectedIndex = 0;
            this.tabPrincipal.Size = new System.Drawing.Size(1018, 672);
            this.tabPrincipal.TabIndex = 1;
            // 
            // tabPageOperativos
            // 
            this.tabPageOperativos.Controls.Add(this.tabOperativos);
            this.tabPageOperativos.Location = new System.Drawing.Point(4, 22);
            this.tabPageOperativos.Name = "tabPageOperativos";
            this.tabPageOperativos.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOperativos.Size = new System.Drawing.Size(1010, 646);
            this.tabPageOperativos.TabIndex = 0;
            this.tabPageOperativos.Text = "Operativos";
            this.tabPageOperativos.UseVisualStyleBackColor = true;
            // 
            // tabOperativos
            // 
            this.tabOperativos.Controls.Add(this.tabOperativosEstadoPage);
            this.tabOperativos.Controls.Add(this.tabOperativosVencimientoPage);
            this.tabOperativos.Controls.Add(this.tabOperativosDemorasPage);
            this.tabOperativos.Controls.Add(this.tabOperativosMuestrasPage);
            this.tabOperativos.Controls.Add(this.tabOperativosSaldoPage);
            this.tabOperativos.Controls.Add(this.tabOperativosProduccionPage);
            this.tabOperativos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabOperativos.Location = new System.Drawing.Point(3, 3);
            this.tabOperativos.Name = "tabOperativos";
            this.tabOperativos.SelectedIndex = 0;
            this.tabOperativos.Size = new System.Drawing.Size(1004, 640);
            this.tabOperativos.TabIndex = 0;
            // 
            // tabOperativosEstadoPage
            // 
            this.tabOperativosEstadoPage.Controls.Add(this.dgvOperativosEstado);
            this.tabOperativosEstadoPage.Location = new System.Drawing.Point(4, 22);
            this.tabOperativosEstadoPage.Name = "tabOperativosEstadoPage";
            this.tabOperativosEstadoPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabOperativosEstadoPage.Size = new System.Drawing.Size(996, 614);
            this.tabOperativosEstadoPage.TabIndex = 0;
            this.tabOperativosEstadoPage.Text = "Por estado";
            this.tabOperativosEstadoPage.UseVisualStyleBackColor = true;
            // 
            // dgvOperativosEstado
            // 
            this.dgvOperativosEstado.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOperativosEstado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOperativosEstado.Location = new System.Drawing.Point(3, 3);
            this.dgvOperativosEstado.Name = "dgvOperativosEstado";
            this.dgvOperativosEstado.Size = new System.Drawing.Size(990, 608);
            this.dgvOperativosEstado.TabIndex = 0;
            // 
            // tabOperativosVencimientoPage
            // 
            this.tabOperativosVencimientoPage.Controls.Add(this.dgvOperativosLimite);
            this.tabOperativosVencimientoPage.Location = new System.Drawing.Point(4, 22);
            this.tabOperativosVencimientoPage.Name = "tabOperativosVencimientoPage";
            this.tabOperativosVencimientoPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabOperativosVencimientoPage.Size = new System.Drawing.Size(996, 614);
            this.tabOperativosVencimientoPage.TabIndex = 1;
            this.tabOperativosVencimientoPage.Text = "Próximos";
            this.tabOperativosVencimientoPage.UseVisualStyleBackColor = true;
            // 
            // dgvOperativosLimite
            // 
            this.dgvOperativosLimite.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOperativosLimite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOperativosLimite.Location = new System.Drawing.Point(3, 3);
            this.dgvOperativosLimite.Name = "dgvOperativosLimite";
            this.dgvOperativosLimite.Size = new System.Drawing.Size(990, 608);
            this.dgvOperativosLimite.TabIndex = 0;
            // 
            // tabOperativosDemorasPage
            // 
            this.tabOperativosDemorasPage.Controls.Add(this.dgvOperativosDemoras);
            this.tabOperativosDemorasPage.Location = new System.Drawing.Point(4, 22);
            this.tabOperativosDemorasPage.Name = "tabOperativosDemorasPage";
            this.tabOperativosDemorasPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabOperativosDemorasPage.Size = new System.Drawing.Size(996, 614);
            this.tabOperativosDemorasPage.TabIndex = 2;
            this.tabOperativosDemorasPage.Text = "Demorados";
            this.tabOperativosDemorasPage.UseVisualStyleBackColor = true;
            // 
            // dgvOperativosDemoras
            // 
            this.dgvOperativosDemoras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOperativosDemoras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOperativosDemoras.Location = new System.Drawing.Point(3, 3);
            this.dgvOperativosDemoras.Name = "dgvOperativosDemoras";
            this.dgvOperativosDemoras.Size = new System.Drawing.Size(990, 608);
            this.dgvOperativosDemoras.TabIndex = 0;
            // 
            // tabOperativosMuestrasPage
            // 
            this.tabOperativosMuestrasPage.Controls.Add(this.dgvOperativosMuestras);
            this.tabOperativosMuestrasPage.Location = new System.Drawing.Point(4, 22);
            this.tabOperativosMuestrasPage.Name = "tabOperativosMuestrasPage";
            this.tabOperativosMuestrasPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabOperativosMuestrasPage.Size = new System.Drawing.Size(996, 614);
            this.tabOperativosMuestrasPage.TabIndex = 3;
            this.tabOperativosMuestrasPage.Text = "Muestras";
            this.tabOperativosMuestrasPage.UseVisualStyleBackColor = true;
            // 
            // dgvOperativosMuestras
            // 
            this.dgvOperativosMuestras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOperativosMuestras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOperativosMuestras.Location = new System.Drawing.Point(3, 3);
            this.dgvOperativosMuestras.Name = "dgvOperativosMuestras";
            this.dgvOperativosMuestras.Size = new System.Drawing.Size(990, 608);
            this.dgvOperativosMuestras.TabIndex = 0;
            // 
            // tabOperativosSaldoPage
            // 
            this.tabOperativosSaldoPage.Controls.Add(this.dgvOperativosSaldo);
            this.tabOperativosSaldoPage.Location = new System.Drawing.Point(4, 22);
            this.tabOperativosSaldoPage.Name = "tabOperativosSaldoPage";
            this.tabOperativosSaldoPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabOperativosSaldoPage.Size = new System.Drawing.Size(996, 614);
            this.tabOperativosSaldoPage.TabIndex = 4;
            this.tabOperativosSaldoPage.Text = "Saldo";
            this.tabOperativosSaldoPage.UseVisualStyleBackColor = true;
            // 
            // dgvOperativosSaldo
            // 
            this.dgvOperativosSaldo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOperativosSaldo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOperativosSaldo.Location = new System.Drawing.Point(3, 3);
            this.dgvOperativosSaldo.Name = "dgvOperativosSaldo";
            this.dgvOperativosSaldo.Size = new System.Drawing.Size(990, 608);
            this.dgvOperativosSaldo.TabIndex = 0;
            // 
            // tabOperativosProduccionPage
            // 
            this.tabOperativosProduccionPage.Controls.Add(this.dgvOperativosProduccion);
            this.tabOperativosProduccionPage.Location = new System.Drawing.Point(4, 22);
            this.tabOperativosProduccionPage.Name = "tabOperativosProduccionPage";
            this.tabOperativosProduccionPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabOperativosProduccionPage.Size = new System.Drawing.Size(996, 614);
            this.tabOperativosProduccionPage.TabIndex = 5;
            this.tabOperativosProduccionPage.Text = "Producción";
            this.tabOperativosProduccionPage.UseVisualStyleBackColor = true;
            // 
            // dgvOperativosProduccion
            // 
            this.dgvOperativosProduccion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOperativosProduccion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOperativosProduccion.Location = new System.Drawing.Point(3, 3);
            this.dgvOperativosProduccion.Name = "dgvOperativosProduccion";
            this.dgvOperativosProduccion.Size = new System.Drawing.Size(990, 608);
            this.dgvOperativosProduccion.TabIndex = 0;
            // 
            // tabPageVentas
            // 
            this.tabPageVentas.Controls.Add(this.tabVentas);
            this.tabPageVentas.Location = new System.Drawing.Point(4, 22);
            this.tabPageVentas.Name = "tabPageVentas";
            this.tabPageVentas.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageVentas.Size = new System.Drawing.Size(1010, 646);
            this.tabPageVentas.TabIndex = 1;
            this.tabPageVentas.Text = "Ventas";
            this.tabPageVentas.UseVisualStyleBackColor = true;
            // 
            // tabVentas
            // 
            this.tabVentas.Controls.Add(this.tabVentasMensualesPage);
            this.tabVentas.Controls.Add(this.tabVentasTrimestralesPage);
            this.tabVentas.Controls.Add(this.tabVentasAnualesPage);
            this.tabVentas.Controls.Add(this.tabVentasComparativasPage);
            this.tabVentas.Controls.Add(this.tabVentasRankingPage);
            this.tabVentas.Controls.Add(this.tabVentasCategoriasPage);
            this.tabVentas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabVentas.Location = new System.Drawing.Point(3, 3);
            this.tabVentas.Name = "tabVentas";
            this.tabVentas.SelectedIndex = 0;
            this.tabVentas.Size = new System.Drawing.Size(1004, 640);
            this.tabVentas.TabIndex = 0;
            // 
            // tabVentasMensualesPage
            // 
            this.tabVentasMensualesPage.Controls.Add(this.dgvVentasMensuales);
            this.tabVentasMensualesPage.Location = new System.Drawing.Point(4, 22);
            this.tabVentasMensualesPage.Name = "tabVentasMensualesPage";
            this.tabVentasMensualesPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabVentasMensualesPage.Size = new System.Drawing.Size(996, 614);
            this.tabVentasMensualesPage.TabIndex = 0;
            this.tabVentasMensualesPage.Text = "Mensuales";
            this.tabVentasMensualesPage.UseVisualStyleBackColor = true;
            // 
            // dgvVentasMensuales
            // 
            this.dgvVentasMensuales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVentasMensuales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVentasMensuales.Location = new System.Drawing.Point(3, 3);
            this.dgvVentasMensuales.Name = "dgvVentasMensuales";
            this.dgvVentasMensuales.Size = new System.Drawing.Size(990, 608);
            this.dgvVentasMensuales.TabIndex = 0;
            // 
            // tabVentasTrimestralesPage
            // 
            this.tabVentasTrimestralesPage.Controls.Add(this.dgvVentasTrimestrales);
            this.tabVentasTrimestralesPage.Location = new System.Drawing.Point(4, 22);
            this.tabVentasTrimestralesPage.Name = "tabVentasTrimestralesPage";
            this.tabVentasTrimestralesPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabVentasTrimestralesPage.Size = new System.Drawing.Size(996, 614);
            this.tabVentasTrimestralesPage.TabIndex = 1;
            this.tabVentasTrimestralesPage.Text = "Trimestrales";
            this.tabVentasTrimestralesPage.UseVisualStyleBackColor = true;
            // 
            // dgvVentasTrimestrales
            // 
            this.dgvVentasTrimestrales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVentasTrimestrales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVentasTrimestrales.Location = new System.Drawing.Point(3, 3);
            this.dgvVentasTrimestrales.Name = "dgvVentasTrimestrales";
            this.dgvVentasTrimestrales.Size = new System.Drawing.Size(990, 608);
            this.dgvVentasTrimestrales.TabIndex = 0;
            // 
            // tabVentasAnualesPage
            // 
            this.tabVentasAnualesPage.Controls.Add(this.dgvVentasAnuales);
            this.tabVentasAnualesPage.Location = new System.Drawing.Point(4, 22);
            this.tabVentasAnualesPage.Name = "tabVentasAnualesPage";
            this.tabVentasAnualesPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabVentasAnualesPage.Size = new System.Drawing.Size(996, 614);
            this.tabVentasAnualesPage.TabIndex = 2;
            this.tabVentasAnualesPage.Text = "Anuales";
            this.tabVentasAnualesPage.UseVisualStyleBackColor = true;
            // 
            // dgvVentasAnuales
            // 
            this.dgvVentasAnuales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVentasAnuales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVentasAnuales.Location = new System.Drawing.Point(3, 3);
            this.dgvVentasAnuales.Name = "dgvVentasAnuales";
            this.dgvVentasAnuales.Size = new System.Drawing.Size(990, 608);
            this.dgvVentasAnuales.TabIndex = 0;
            // 
            // tabVentasComparativasPage
            // 
            this.tabVentasComparativasPage.Controls.Add(this.dgvVentasComparativas);
            this.tabVentasComparativasPage.Location = new System.Drawing.Point(4, 22);
            this.tabVentasComparativasPage.Name = "tabVentasComparativasPage";
            this.tabVentasComparativasPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabVentasComparativasPage.Size = new System.Drawing.Size(996, 614);
            this.tabVentasComparativasPage.TabIndex = 3;
            this.tabVentasComparativasPage.Text = "Comparativas";
            this.tabVentasComparativasPage.UseVisualStyleBackColor = true;
            // 
            // dgvVentasComparativas
            // 
            this.dgvVentasComparativas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVentasComparativas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVentasComparativas.Location = new System.Drawing.Point(3, 3);
            this.dgvVentasComparativas.Name = "dgvVentasComparativas";
            this.dgvVentasComparativas.Size = new System.Drawing.Size(990, 608);
            this.dgvVentasComparativas.TabIndex = 0;
            // 
            // tabVentasRankingPage
            // 
            this.tabVentasRankingPage.Controls.Add(this.dgvVentasRanking);
            this.tabVentasRankingPage.Location = new System.Drawing.Point(4, 22);
            this.tabVentasRankingPage.Name = "tabVentasRankingPage";
            this.tabVentasRankingPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabVentasRankingPage.Size = new System.Drawing.Size(996, 614);
            this.tabVentasRankingPage.TabIndex = 4;
            this.tabVentasRankingPage.Text = "Ranking";
            this.tabVentasRankingPage.UseVisualStyleBackColor = true;
            // 
            // dgvVentasRanking
            // 
            this.dgvVentasRanking.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVentasRanking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVentasRanking.Location = new System.Drawing.Point(3, 3);
            this.dgvVentasRanking.Name = "dgvVentasRanking";
            this.dgvVentasRanking.Size = new System.Drawing.Size(990, 608);
            this.dgvVentasRanking.TabIndex = 0;
            // 
            // tabVentasCategoriasPage
            // 
            this.tabVentasCategoriasPage.Controls.Add(this.dgvVentasCategorias);
            this.tabVentasCategoriasPage.Location = new System.Drawing.Point(4, 22);
            this.tabVentasCategoriasPage.Name = "tabVentasCategoriasPage";
            this.tabVentasCategoriasPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabVentasCategoriasPage.Size = new System.Drawing.Size(996, 614);
            this.tabVentasCategoriasPage.TabIndex = 5;
            this.tabVentasCategoriasPage.Text = "Categorías";
            this.tabVentasCategoriasPage.UseVisualStyleBackColor = true;
            // 
            // dgvVentasCategorias
            // 
            this.dgvVentasCategorias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVentasCategorias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVentasCategorias.Location = new System.Drawing.Point(3, 3);
            this.dgvVentasCategorias.Name = "dgvVentasCategorias";
            this.dgvVentasCategorias.Size = new System.Drawing.Size(990, 608);
            this.dgvVentasCategorias.TabIndex = 0;
            // 
            // tabPageFinancieros
            // 
            this.tabPageFinancieros.Controls.Add(this.tabFinancieros);
            this.tabPageFinancieros.Location = new System.Drawing.Point(4, 22);
            this.tabPageFinancieros.Name = "tabPageFinancieros";
            this.tabPageFinancieros.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFinancieros.Size = new System.Drawing.Size(1010, 646);
            this.tabPageFinancieros.TabIndex = 2;
            this.tabPageFinancieros.Text = "Financieros";
            this.tabPageFinancieros.UseVisualStyleBackColor = true;
            // 
            // tabFinancieros
            // 
            this.tabFinancieros.Controls.Add(this.tabFinanzasFacturacionPage);
            this.tabFinancieros.Controls.Add(this.tabFinanzasCuentasPage);
            this.tabFinancieros.Controls.Add(this.tabFinanzasPagosPage);
            this.tabFinancieros.Controls.Add(this.tabFinanzasProyeccionPage);
            this.tabFinancieros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFinancieros.Location = new System.Drawing.Point(3, 3);
            this.tabFinancieros.Name = "tabFinancieros";
            this.tabFinancieros.SelectedIndex = 0;
            this.tabFinancieros.Size = new System.Drawing.Size(1004, 640);
            this.tabFinancieros.TabIndex = 0;
            // 
            // tabFinanzasFacturacionPage
            // 
            this.tabFinanzasFacturacionPage.Controls.Add(this.dgvFinanzasFacturacion);
            this.tabFinanzasFacturacionPage.Location = new System.Drawing.Point(4, 22);
            this.tabFinanzasFacturacionPage.Name = "tabFinanzasFacturacionPage";
            this.tabFinanzasFacturacionPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabFinanzasFacturacionPage.Size = new System.Drawing.Size(996, 614);
            this.tabFinanzasFacturacionPage.TabIndex = 0;
            this.tabFinanzasFacturacionPage.Text = "Facturación";
            this.tabFinanzasFacturacionPage.UseVisualStyleBackColor = true;
            // 
            // dgvFinanzasFacturacion
            // 
            this.dgvFinanzasFacturacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFinanzasFacturacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFinanzasFacturacion.Location = new System.Drawing.Point(3, 3);
            this.dgvFinanzasFacturacion.Name = "dgvFinanzasFacturacion";
            this.dgvFinanzasFacturacion.Size = new System.Drawing.Size(990, 608);
            this.dgvFinanzasFacturacion.TabIndex = 0;
            // 
            // tabFinanzasCuentasPage
            // 
            this.tabFinanzasCuentasPage.Controls.Add(this.dgvFinanzasCuentas);
            this.tabFinanzasCuentasPage.Location = new System.Drawing.Point(4, 22);
            this.tabFinanzasCuentasPage.Name = "tabFinanzasCuentasPage";
            this.tabFinanzasCuentasPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabFinanzasCuentasPage.Size = new System.Drawing.Size(996, 614);
            this.tabFinanzasCuentasPage.TabIndex = 1;
            this.tabFinanzasCuentasPage.Text = "Cuentas";
            this.tabFinanzasCuentasPage.UseVisualStyleBackColor = true;
            // 
            // dgvFinanzasCuentas
            // 
            this.dgvFinanzasCuentas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFinanzasCuentas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFinanzasCuentas.Location = new System.Drawing.Point(3, 3);
            this.dgvFinanzasCuentas.Name = "dgvFinanzasCuentas";
            this.dgvFinanzasCuentas.Size = new System.Drawing.Size(990, 608);
            this.dgvFinanzasCuentas.TabIndex = 0;
            // 
            // tabFinanzasPagosPage
            // 
            this.tabFinanzasPagosPage.Controls.Add(this.dgvFinanzasPagos);
            this.tabFinanzasPagosPage.Location = new System.Drawing.Point(4, 22);
            this.tabFinanzasPagosPage.Name = "tabFinanzasPagosPage";
            this.tabFinanzasPagosPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabFinanzasPagosPage.Size = new System.Drawing.Size(996, 614);
            this.tabFinanzasPagosPage.TabIndex = 2;
            this.tabFinanzasPagosPage.Text = "Pagos";
            this.tabFinanzasPagosPage.UseVisualStyleBackColor = true;
            // 
            // dgvFinanzasPagos
            // 
            this.dgvFinanzasPagos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFinanzasPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFinanzasPagos.Location = new System.Drawing.Point(3, 3);
            this.dgvFinanzasPagos.Name = "dgvFinanzasPagos";
            this.dgvFinanzasPagos.Size = new System.Drawing.Size(990, 608);
            this.dgvFinanzasPagos.TabIndex = 0;
            // 
            // tabFinanzasProyeccionPage
            // 
            this.tabFinanzasProyeccionPage.Controls.Add(this.dgvFinanzasProyeccion);
            this.tabFinanzasProyeccionPage.Location = new System.Drawing.Point(4, 22);
            this.tabFinanzasProyeccionPage.Name = "tabFinanzasProyeccionPage";
            this.tabFinanzasProyeccionPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabFinanzasProyeccionPage.Size = new System.Drawing.Size(996, 614);
            this.tabFinanzasProyeccionPage.TabIndex = 3;
            this.tabFinanzasProyeccionPage.Text = "Proyección";
            this.tabFinanzasProyeccionPage.UseVisualStyleBackColor = true;
            // 
            // dgvFinanzasProyeccion
            // 
            this.dgvFinanzasProyeccion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFinanzasProyeccion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFinanzasProyeccion.Location = new System.Drawing.Point(3, 3);
            this.dgvFinanzasProyeccion.Name = "dgvFinanzasProyeccion";
            this.dgvFinanzasProyeccion.Size = new System.Drawing.Size(990, 608);
            this.dgvFinanzasProyeccion.TabIndex = 0;
            // 
            // ReportesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.layoutPrincipal);
            this.MinimumSize = new System.Drawing.Size(960, 600);
            this.Name = "ReportesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reportes";
            this.Load += new System.EventHandler(this.ReportesForm_Load);
            this.layoutPrincipal.ResumeLayout(false);
            this.grpFiltros.ResumeLayout(false);
            this.grpFiltros.PerformLayout();
            this.flowFiltros.ResumeLayout(false);
            this.flowFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiasLimite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAnioComparativo)).EndInit();
            this.tabPrincipal.ResumeLayout(false);
            this.tabPageOperativos.ResumeLayout(false);
            this.tabOperativos.ResumeLayout(false);
            this.tabOperativosEstadoPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperativosEstado)).EndInit();
            this.tabOperativosVencimientoPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperativosLimite)).EndInit();
            this.tabOperativosDemorasPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperativosDemoras)).EndInit();
            this.tabOperativosMuestrasPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperativosMuestras)).EndInit();
            this.tabOperativosSaldoPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperativosSaldo)).EndInit();
            this.tabOperativosProduccionPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperativosProduccion)).EndInit();
            this.tabPageVentas.ResumeLayout(false);
            this.tabVentas.ResumeLayout(false);
            this.tabVentasMensualesPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasMensuales)).EndInit();
            this.tabVentasTrimestralesPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasTrimestrales)).EndInit();
            this.tabVentasAnualesPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasAnuales)).EndInit();
            this.tabVentasComparativasPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasComparativas)).EndInit();
            this.tabVentasRankingPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasRanking)).EndInit();
            this.tabVentasCategoriasPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasCategorias)).EndInit();
            this.tabPageFinancieros.ResumeLayout(false);
            this.tabFinancieros.ResumeLayout(false);
            this.tabFinanzasFacturacionPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinanzasFacturacion)).EndInit();
            this.tabFinanzasCuentasPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinanzasCuentas)).EndInit();
            this.tabFinanzasPagosPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinanzasPagos)).EndInit();
            this.tabFinanzasProyeccionPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinanzasProyeccion)).EndInit();
            this.ResumeLayout(false);
        }
    }
}