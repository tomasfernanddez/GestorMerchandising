using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    partial class ABMPedidosForm
    {
        private IContainer components = null;
        private ToolStrip toolStrip1;
        private ToolStripButton tsbNuevo;
        private ToolStripButton tsbEditar;
        private ToolStripButton tsbActualizar;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel tslBuscar;
        private ToolStripTextBox txtBuscar;
        private ToolStripButton tsbBuscar;
        private GroupBox grpFiltros;
        private Label lblFiltroCliente;
        private ComboBox cboFiltroCliente;
        private Label lblFiltroEstado;
        private ComboBox cboFiltroEstado;
        private CheckBox chkSoloVencidos;
        private Button btnFiltrar;
        private Button btnLimpiarFiltros;
        private DataGridView dgvPedidos;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNuevo = new System.Windows.Forms.ToolStripButton();
            this.tsbEditar = new System.Windows.Forms.ToolStripButton();
            this.tsbActualizar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tslBuscar = new System.Windows.Forms.ToolStripLabel();
            this.txtBuscar = new System.Windows.Forms.ToolStripTextBox();
            this.tsbBuscar = new System.Windows.Forms.ToolStripButton();
            this.grpFiltros = new System.Windows.Forms.GroupBox();
            this.lblFiltroCliente = new System.Windows.Forms.Label();
            this.cboFiltroCliente = new System.Windows.Forms.ComboBox();
            this.lblFiltroEstado = new System.Windows.Forms.Label();
            this.cboFiltroEstado = new System.Windows.Forms.ComboBox();
            this.chkSoloVencidos = new System.Windows.Forms.CheckBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnLimpiarFiltros = new System.Windows.Forms.Button();
            this.dgvPedidos = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            this.grpFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidos)).BeginInit();
            this.SuspendLayout();
            //
            // toolStrip1
            //
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNuevo,
            this.tsbEditar,
            this.tsbActualizar,
            this.toolStripSeparator1,
            this.tslBuscar,
            this.txtBuscar,
            this.tsbBuscar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1200, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            //
            // tsbNuevo
            //
            this.tsbNuevo.Text = "Nuevo";
            this.tsbNuevo.Name = "tsbNuevo";
            //
            // tsbEditar
            //
            this.tsbEditar.Text = "Editar";
            this.tsbEditar.Name = "tsbEditar";
            //
            // tsbActualizar
            //
            this.tsbActualizar.Text = "Actualizar";
            this.tsbActualizar.Name = "tsbActualizar";
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            //
            // tslBuscar
            //
            this.tslBuscar.Name = "tslBuscar";
            this.tslBuscar.Size = new System.Drawing.Size(100, 22);
            this.tslBuscar.Text = "Buscar (Nro):";
            //
            // txtBuscar
            //
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(150, 25);
            //
            // tsbBuscar
            //
            this.tsbBuscar.Text = "Buscar";
            this.tsbBuscar.Name = "tsbBuscar";
            //
            // grpFiltros
            //
            this.grpFiltros.Controls.Add(this.lblFiltroCliente);
            this.grpFiltros.Controls.Add(this.cboFiltroCliente);
            this.grpFiltros.Controls.Add(this.lblFiltroEstado);
            this.grpFiltros.Controls.Add(this.cboFiltroEstado);
            this.grpFiltros.Controls.Add(this.chkSoloVencidos);
            this.grpFiltros.Controls.Add(this.btnFiltrar);
            this.grpFiltros.Controls.Add(this.btnLimpiarFiltros);
            this.grpFiltros.Location = new System.Drawing.Point(12, 35);
            this.grpFiltros.Name = "grpFiltros";
            this.grpFiltros.Size = new System.Drawing.Size(1176, 80);
            this.grpFiltros.TabIndex = 1;
            this.grpFiltros.TabStop = false;
            this.grpFiltros.Text = "Filtros";
            //
            // lblFiltroCliente
            //
            this.lblFiltroCliente.Location = new System.Drawing.Point(15, 25);
            this.lblFiltroCliente.Name = "lblFiltroCliente";
            this.lblFiltroCliente.Size = new System.Drawing.Size(80, 22);
            this.lblFiltroCliente.TabIndex = 0;
            this.lblFiltroCliente.Text = "order.client";
            this.lblFiltroCliente.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cboFiltroCliente
            //
            this.cboFiltroCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltroCliente.FormattingEnabled = true;
            this.cboFiltroCliente.Location = new System.Drawing.Point(95, 25);
            this.cboFiltroCliente.Name = "cboFiltroCliente";
            this.cboFiltroCliente.Size = new System.Drawing.Size(300, 21);
            this.cboFiltroCliente.TabIndex = 1;
            //
            // lblFiltroEstado
            //
            this.lblFiltroEstado.Location = new System.Drawing.Point(415, 25);
            this.lblFiltroEstado.Name = "lblFiltroEstado";
            this.lblFiltroEstado.Size = new System.Drawing.Size(80, 22);
            this.lblFiltroEstado.TabIndex = 2;
            this.lblFiltroEstado.Text = "order.status";
            this.lblFiltroEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cboFiltroEstado
            //
            this.cboFiltroEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltroEstado.FormattingEnabled = true;
            this.cboFiltroEstado.Location = new System.Drawing.Point(495, 25);
            this.cboFiltroEstado.Name = "cboFiltroEstado";
            this.cboFiltroEstado.Size = new System.Drawing.Size(250, 21);
            this.cboFiltroEstado.TabIndex = 3;
            //
            // chkSoloVencidos
            //
            this.chkSoloVencidos.Location = new System.Drawing.Point(765, 25);
            this.chkSoloVencidos.Name = "chkSoloVencidos";
            this.chkSoloVencidos.Size = new System.Drawing.Size(150, 22);
            this.chkSoloVencidos.TabIndex = 4;
            this.chkSoloVencidos.Text = "Solo Vencidos";
            this.chkSoloVencidos.UseVisualStyleBackColor = true;
            //
            // btnFiltrar
            //
            this.btnFiltrar.Location = new System.Drawing.Point(935, 23);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(100, 25);
            this.btnFiltrar.TabIndex = 5;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            //
            // btnLimpiarFiltros
            //
            this.btnLimpiarFiltros.Location = new System.Drawing.Point(1045, 23);
            this.btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            this.btnLimpiarFiltros.Size = new System.Drawing.Size(115, 25);
            this.btnLimpiarFiltros.TabIndex = 6;
            this.btnLimpiarFiltros.Text = "Limpiar Filtros";
            this.btnLimpiarFiltros.UseVisualStyleBackColor = true;
            //
            // dgvPedidos
            //
            this.dgvPedidos.AllowUserToAddRows = false;
            this.dgvPedidos.AllowUserToDeleteRows = false;
            this.dgvPedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPedidos.Location = new System.Drawing.Point(12, 125);
            this.dgvPedidos.Name = "dgvPedidos";
            this.dgvPedidos.ReadOnly = true;
            this.dgvPedidos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPedidos.Size = new System.Drawing.Size(1176, 545);
            this.dgvPedidos.TabIndex = 2;
            //
            // ABMPedidosForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 680);
            this.Controls.Add(this.dgvPedidos);
            this.Controls.Add(this.grpFiltros);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ABMPedidosForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Pedidos";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.grpFiltros.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
