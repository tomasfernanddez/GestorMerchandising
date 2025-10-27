namespace UI
{
    partial class PedidosMuestraForm
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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbNuevo = new System.Windows.Forms.ToolStripButton();
            this.tsbEditar = new System.Windows.Forms.ToolStripButton();
            this.tsbActualizar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPedirFacturacion = new System.Windows.Forms.ToolStripButton();
            this.tsbExtenderDias = new System.Windows.Forms.ToolStripButton();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.flowFiltros = new System.Windows.Forms.FlowLayoutPanel();
            this.lblClienteFiltro = new System.Windows.Forms.Label();
            this.cmbClienteFiltro = new System.Windows.Forms.ComboBox();
            this.lblEstadoFiltro = new System.Windows.Forms.Label();
            this.cmbEstadoFiltro = new System.Windows.Forms.ComboBox();
            this.chkFacturadoFiltro = new System.Windows.Forms.CheckBox();
            this.chkSaldoFiltro = new System.Windows.Forms.CheckBox();
            this.lblDiasExtension = new System.Windows.Forms.Label();
            this.nudDiasExtension = new System.Windows.Forms.NumericUpDown();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.dgvPedidos = new System.Windows.Forms.DataGridView();
            this.toolStrip.SuspendLayout();
            this.layoutMain.SuspendLayout();
            this.flowFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiasExtension)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidos)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNuevo,
            this.tsbEditar,
            this.tsbActualizar,
            this.toolStripSeparator1,
            this.tsbPedirFacturacion,
            this.tsbExtenderDias});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(884, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // tsbNuevo
            // 
            this.tsbNuevo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbNuevo.Name = "tsbNuevo";
            this.tsbNuevo.Size = new System.Drawing.Size(110, 22);
            this.tsbNuevo.Text = "sampleOrder.list.new";
            this.tsbNuevo.Click += new System.EventHandler(this.tsbNuevo_Click);
            // 
            // tsbEditar
            // 
            this.tsbEditar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbEditar.Name = "tsbEditar";
            this.tsbEditar.Size = new System.Drawing.Size(110, 22);
            this.tsbEditar.Text = "sampleOrder.list.edit";
            this.tsbEditar.Click += new System.EventHandler(this.tsbEditar_Click);
            // 
            // tsbActualizar
            // 
            this.tsbActualizar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbActualizar.Name = "tsbActualizar";
            this.tsbActualizar.Size = new System.Drawing.Size(74, 22);
            this.tsbActualizar.Text = "form.refresh";
            this.tsbActualizar.Click += new System.EventHandler(this.tsbActualizar_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbPedirFacturacion
            // 
            this.tsbPedirFacturacion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbPedirFacturacion.Name = "tsbPedirFacturacion";
            this.tsbPedirFacturacion.Size = new System.Drawing.Size(151, 22);
            this.tsbPedirFacturacion.Text = "sampleOrder.request.billing";
            this.tsbPedirFacturacion.Click += new System.EventHandler(this.tsbPedirFacturacion_Click);
            // 
            // tsbExtenderDias
            // 
            this.tsbExtenderDias.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbExtenderDias.Name = "tsbExtenderDias";
            this.tsbExtenderDias.Size = new System.Drawing.Size(129, 22);
            this.tsbExtenderDias.Text = "sampleOrder.extend.due";
            this.tsbExtenderDias.Click += new System.EventHandler(this.tsbExtenderDias_Click);
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.flowFiltros, 0, 0);
            this.layoutMain.Controls.Add(this.dgvPedidos, 0, 1);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 25);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 2;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(884, 636);
            this.layoutMain.TabIndex = 1;
            // 
            // flowFiltros
            // 
            this.flowFiltros.AutoSize = true;
            this.flowFiltros.Controls.Add(this.lblClienteFiltro);
            this.flowFiltros.Controls.Add(this.cmbClienteFiltro);
            this.flowFiltros.Controls.Add(this.lblEstadoFiltro);
            this.flowFiltros.Controls.Add(this.cmbEstadoFiltro);
            this.flowFiltros.Controls.Add(this.chkFacturadoFiltro);
            this.flowFiltros.Controls.Add(this.chkSaldoFiltro);
            this.flowFiltros.Controls.Add(this.lblDiasExtension);
            this.flowFiltros.Controls.Add(this.nudDiasExtension);
            this.flowFiltros.Controls.Add(this.btnFiltrar);
            this.flowFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowFiltros.Location = new System.Drawing.Point(3, 3);
            this.flowFiltros.Name = "flowFiltros";
            this.flowFiltros.Size = new System.Drawing.Size(878, 32);
            this.flowFiltros.TabIndex = 0;
            // 
            // lblClienteFiltro
            // 
            this.lblClienteFiltro.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblClienteFiltro.AutoSize = true;
            this.lblClienteFiltro.Location = new System.Drawing.Point(3, 9);
            this.lblClienteFiltro.Name = "lblClienteFiltro";
            this.lblClienteFiltro.Size = new System.Drawing.Size(94, 13);
            this.lblClienteFiltro.TabIndex = 0;
            this.lblClienteFiltro.Text = "sampleOrder.client";
            // 
            // cmbClienteFiltro
            // 
            this.cmbClienteFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClienteFiltro.FormattingEnabled = true;
            this.cmbClienteFiltro.Location = new System.Drawing.Point(103, 3);
            this.cmbClienteFiltro.Name = "cmbClienteFiltro";
            this.cmbClienteFiltro.Size = new System.Drawing.Size(180, 21);
            this.cmbClienteFiltro.TabIndex = 1;
            // 
            // lblEstadoFiltro
            // 
            this.lblEstadoFiltro.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEstadoFiltro.AutoSize = true;
            this.lblEstadoFiltro.Location = new System.Drawing.Point(289, 9);
            this.lblEstadoFiltro.Name = "lblEstadoFiltro";
            this.lblEstadoFiltro.Size = new System.Drawing.Size(97, 13);
            this.lblEstadoFiltro.TabIndex = 2;
            this.lblEstadoFiltro.Text = "sampleOrder.state";
            // 
            // cmbEstadoFiltro
            // 
            this.cmbEstadoFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstadoFiltro.FormattingEnabled = true;
            this.cmbEstadoFiltro.Location = new System.Drawing.Point(392, 3);
            this.cmbEstadoFiltro.Name = "cmbEstadoFiltro";
            this.cmbEstadoFiltro.Size = new System.Drawing.Size(160, 21);
            this.cmbEstadoFiltro.TabIndex = 3;
            // 
            // chkFacturadoFiltro
            // 
            this.chkFacturadoFiltro.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkFacturadoFiltro.AutoSize = true;
            this.chkFacturadoFiltro.Location = new System.Drawing.Point(558, 7);
            this.chkFacturadoFiltro.Name = "chkFacturadoFiltro";
            this.chkFacturadoFiltro.Size = new System.Drawing.Size(157, 17);
            this.chkFacturadoFiltro.TabIndex = 4;
            this.chkFacturadoFiltro.Text = "sampleOrder.invoiced.only";
            this.chkFacturadoFiltro.UseVisualStyleBackColor = true;
            // 
            // chkSaldoFiltro
            // 
            this.chkSaldoFiltro.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkSaldoFiltro.AutoSize = true;
            this.chkSaldoFiltro.Location = new System.Drawing.Point(721, 7);
            this.chkSaldoFiltro.Name = "chkSaldoFiltro";
            this.chkSaldoFiltro.Size = new System.Drawing.Size(153, 17);
            this.chkSaldoFiltro.TabIndex = 5;
            this.chkSaldoFiltro.Text = "sampleOrder.balance.only";
            this.chkSaldoFiltro.UseVisualStyleBackColor = true;
            // 
            // lblDiasExtension
            // 
            this.lblDiasExtension.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDiasExtension.AutoSize = true;
            this.lblDiasExtension.Location = new System.Drawing.Point(3, 39);
            this.lblDiasExtension.Name = "lblDiasExtension";
            this.lblDiasExtension.Size = new System.Drawing.Size(139, 13);
            this.lblDiasExtension.TabIndex = 6;
            this.lblDiasExtension.Text = "sampleOrder.extend.days";
            // 
            // nudDiasExtension
            // 
            this.nudDiasExtension.Location = new System.Drawing.Point(148, 35);
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
            this.nudDiasExtension.TabIndex = 7;
            this.nudDiasExtension.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.AutoSize = true;
            this.btnFiltrar.Location = new System.Drawing.Point(3, 62);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(85, 25);
            this.btnFiltrar.TabIndex = 8;
            this.btnFiltrar.Text = "form.filter";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // dgvPedidos
            // 
            this.dgvPedidos.AllowUserToAddRows = false;
            this.dgvPedidos.AllowUserToDeleteRows = false;
            this.dgvPedidos.AllowUserToResizeRows = false;
            this.dgvPedidos.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvPedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPedidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPedidos.Location = new System.Drawing.Point(3, 41);
            this.dgvPedidos.MultiSelect = false;
            this.dgvPedidos.Name = "dgvPedidos";
            this.dgvPedidos.ReadOnly = true;
            this.dgvPedidos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPedidos.Size = new System.Drawing.Size(878, 592);
            this.dgvPedidos.TabIndex = 1;
            this.dgvPedidos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPedidos_CellDoubleClick);
            // 
            // PedidosMuestraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.layoutMain);
            this.Controls.Add(this.toolStrip);
            this.MinimumSize = new System.Drawing.Size(900, 700);
            this.Name = "PedidosMuestraForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "sampleOrder.list.title";
            this.Load += new System.EventHandler(this.PedidosMuestraForm_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            this.flowFiltros.ResumeLayout(false);
            this.flowFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiasExtension)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbNuevo;
        private System.Windows.Forms.ToolStripButton tsbEditar;
        private System.Windows.Forms.ToolStripButton tsbActualizar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbPedirFacturacion;
        private System.Windows.Forms.ToolStripButton tsbExtenderDias;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.FlowLayoutPanel flowFiltros;
        private System.Windows.Forms.Label lblClienteFiltro;
        private System.Windows.Forms.ComboBox cmbClienteFiltro;
        private System.Windows.Forms.Label lblEstadoFiltro;
        private System.Windows.Forms.ComboBox cmbEstadoFiltro;
        private System.Windows.Forms.CheckBox chkFacturadoFiltro;
        private System.Windows.Forms.CheckBox chkSaldoFiltro;
        private System.Windows.Forms.Label lblDiasExtension;
        private System.Windows.Forms.NumericUpDown nudDiasExtension;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.DataGridView dgvPedidos;
    }
}