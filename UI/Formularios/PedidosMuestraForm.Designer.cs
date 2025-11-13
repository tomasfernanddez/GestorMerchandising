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
            this.tsbCancelar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPedirFacturacion = new System.Windows.Forms.ToolStripButton();
            this.tsbExtenderDias = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tslBuscar = new System.Windows.Forms.ToolStripLabel();
            this.txtBuscar = new System.Windows.Forms.ToolStripTextBox();
            this.btnBuscar = new System.Windows.Forms.ToolStripButton();
            this.tslEstado = new System.Windows.Forms.ToolStripLabel();
            this.cmbEstado = new System.Windows.Forms.ToolStripComboBox();
            this.tslSaldo = new System.Windows.Forms.ToolStripLabel();
            this.cmbSaldo = new System.Windows.Forms.ToolStripComboBox();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.dgvPedidos = new System.Windows.Forms.DataGridView();
            this.toolStrip.SuspendLayout();
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidos)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNuevo,
            this.tsbEditar,
            this.tsbActualizar,
            this.tsbCancelar,
            this.toolStripSeparator1,
            this.tsbPedirFacturacion,
            this.tsbExtenderDias,
            this.toolStripSeparator2,
            this.tslBuscar,
            this.txtBuscar,
            this.btnBuscar,
            this.tslEstado,
            this.cmbEstado,
            this.tslSaldo,
            this.cmbSaldo});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(884, 29);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // tsbNuevo
            // 
            this.tsbNuevo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbNuevo.Name = "tsbNuevo";
            this.tsbNuevo.Size = new System.Drawing.Size(122, 26);
            this.tsbNuevo.Text = "sampleOrder.list.new";
            this.tsbNuevo.Click += new System.EventHandler(this.tsbNuevo_Click);
            // 
            // tsbEditar
            // 
            this.tsbEditar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbEditar.Name = "tsbEditar";
            this.tsbEditar.Size = new System.Drawing.Size(120, 26);
            this.tsbEditar.Text = "sampleOrder.list.edit";
            this.tsbEditar.Click += new System.EventHandler(this.tsbEditar_Click);
            // 
            // tsbActualizar
            // 
            this.tsbActualizar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbActualizar.Name = "tsbActualizar";
            this.tsbActualizar.Size = new System.Drawing.Size(76, 26);
            this.tsbActualizar.Text = "form.refresh";
            this.tsbActualizar.Click += new System.EventHandler(this.tsbActualizar_Click);
            // 
            // tsbCancelar
            // 
            this.tsbCancelar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCancelar.Name = "tsbCancelar";
            this.tsbCancelar.Size = new System.Drawing.Size(115, 26);
            this.tsbCancelar.Text = "order.cancel.button";
            this.tsbCancelar.Click += new System.EventHandler(this.tsbCancelar_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // tsbPedirFacturacion
            // 
            this.tsbPedirFacturacion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbPedirFacturacion.Name = "tsbPedirFacturacion";
            this.tsbPedirFacturacion.Size = new System.Drawing.Size(157, 26);
            this.tsbPedirFacturacion.Text = "sampleOrder.request.billing";
            this.tsbPedirFacturacion.Click += new System.EventHandler(this.tsbPedirFacturacion_Click);
            // 
            // tsbExtenderDias
            // 
            this.tsbExtenderDias.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbExtenderDias.Name = "tsbExtenderDias";
            this.tsbExtenderDias.Size = new System.Drawing.Size(141, 26);
            this.tsbExtenderDias.Text = "sampleOrder.extend.due";
            this.tsbExtenderDias.Click += new System.EventHandler(this.tsbExtenderDias_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 29);
            // 
            // tslBuscar
            // 
            this.tslBuscar.Name = "tslBuscar";
            this.tslBuscar.Size = new System.Drawing.Size(70, 26);
            this.tslBuscar.Text = "form.search";
            // 
            // txtBuscar
            // 
            this.txtBuscar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(160, 23);
            // 
            // btnBuscar
            // 
            this.btnBuscar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(64, 19);
            this.btnBuscar.Text = "form.filter";
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // tslEstado
            // 
            this.tslEstado.Name = "tslEstado";
            this.tslEstado.Size = new System.Drawing.Size(103, 15);
            this.tslEstado.Text = "sampleOrder.state";
            // 
            // cmbEstado
            // 
            this.cmbEstado.AutoSize = false;
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(140, 23);
            // 
            // tslSaldo
            // 
            this.tslSaldo.Name = "tslSaldo";
            this.tslSaldo.Size = new System.Drawing.Size(172, 15);
            this.tslSaldo.Text = "sampleOrder.summary.balance";
            // 
            // cmbSaldo
            // 
            this.cmbSaldo.AutoSize = false;
            this.cmbSaldo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSaldo.Name = "cmbSaldo";
            this.cmbSaldo.Size = new System.Drawing.Size(120, 23);
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.dgvPedidos, 0, 0);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 29);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 1;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(884, 632);
            this.layoutMain.TabIndex = 1;
            // 
            // dgvPedidos
            // 
            this.dgvPedidos.AllowUserToAddRows = false;
            this.dgvPedidos.AllowUserToDeleteRows = false;
            this.dgvPedidos.AllowUserToResizeRows = false;
            this.dgvPedidos.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvPedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPedidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPedidos.Location = new System.Drawing.Point(3, 3);
            this.dgvPedidos.MultiSelect = false;
            this.dgvPedidos.Name = "dgvPedidos";
            this.dgvPedidos.ReadOnly = true;
            this.dgvPedidos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPedidos.Size = new System.Drawing.Size(878, 626);
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbNuevo;
        private System.Windows.Forms.ToolStripButton tsbEditar;
        private System.Windows.Forms.ToolStripButton tsbActualizar;
        private System.Windows.Forms.ToolStripButton tsbCancelar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbPedirFacturacion;
        private System.Windows.Forms.ToolStripButton tsbExtenderDias;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel tslBuscar;
        private System.Windows.Forms.ToolStripTextBox txtBuscar;
        private System.Windows.Forms.ToolStripButton btnBuscar;
        private System.Windows.Forms.ToolStripLabel tslEstado;
        private System.Windows.Forms.ToolStripComboBox cmbEstado;
        private System.Windows.Forms.ToolStripLabel tslSaldo;
        private System.Windows.Forms.ToolStripComboBox cmbSaldo;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.DataGridView dgvPedidos;
    }
}