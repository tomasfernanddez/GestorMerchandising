using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    partial class ABMProveedoresForm
    {
        private IContainer components = null;
        private ToolStrip toolStrip1;
        private ToolStripButton tsbNuevo;
        private ToolStripButton tsbEditar;
        private ToolStripButton tsbEliminar;
        private ToolStripButton tsbActivar;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel lblBuscarRazon;
        private ToolStripTextBox txtBuscarRazon;
        private ToolStripLabel lblFiltroTipo;
        private ToolStripComboBox cboFiltroTipo;
        private ToolStripLabel lblFiltroEstado;
        private ToolStripComboBox cboFiltroEstado;
        private ToolStripButton tsbActualizar;
        private DataGridView dgvProveedores;
        private BindingSource bsProveedores;

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
            this.tsbEliminar = new System.Windows.Forms.ToolStripButton();
            this.tsbActivar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblBuscarRazon = new System.Windows.Forms.ToolStripLabel();
            this.txtBuscarRazon = new System.Windows.Forms.ToolStripTextBox();
            this.lblFiltroTipo = new System.Windows.Forms.ToolStripLabel();
            this.cboFiltroTipo = new System.Windows.Forms.ToolStripComboBox();
            this.lblFiltroEstado = new System.Windows.Forms.ToolStripLabel();
            this.cboFiltroEstado = new System.Windows.Forms.ToolStripComboBox();
            this.tsbActualizar = new System.Windows.Forms.ToolStripButton();
            this.dgvProveedores = new System.Windows.Forms.DataGridView();
            this.bsProveedores = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProveedores)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsProveedores)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNuevo,
            this.tsbEditar,
            this.tsbEliminar,
            this.tsbActivar,
            this.toolStripSeparator1,
            this.lblBuscarRazon,
            this.txtBuscarRazon,
            this.lblFiltroTipo,
            this.cboFiltroTipo,
            this.lblFiltroEstado,
            this.cboFiltroEstado,
            this.tsbActualizar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1000, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNuevo
            // 
            this.tsbNuevo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbNuevo.Name = "tsbNuevo";
            this.tsbNuevo.Size = new System.Drawing.Size(46, 22);
            this.tsbNuevo.Text = "abm.common.new";
            // 
            // tsbEditar
            // 
            this.tsbEditar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbEditar.Name = "tsbEditar";
            this.tsbEditar.Size = new System.Drawing.Size(41, 22);
            this.tsbEditar.Text = "abm.common.edit";
            // 
            // tsbEliminar
            // 
            this.tsbEliminar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbEliminar.Name = "tsbEliminar";
            this.tsbEliminar.Size = new System.Drawing.Size(54, 22);
            this.tsbEliminar.Text = "abm.common.delete";
            // 
            // tsbActivar
            // 
            this.tsbActivar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbActivar.Name = "tsbActivar";
            this.tsbActivar.Size = new System.Drawing.Size(102, 22);
            this.tsbActivar.Text = "supplier.tool.activate";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblBuscarRazon
            // 
            this.lblBuscarRazon.Name = "lblBuscarRazon";
            this.lblBuscarRazon.Size = new System.Drawing.Size(107, 22);
            this.lblBuscarRazon.Text = "supplier.filter.razon";
            // 
            // txtBuscarRazon
            // 
            this.txtBuscarRazon.AutoSize = false;
            this.txtBuscarRazon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBuscarRazon.Name = "txtBuscarRazon";
            this.txtBuscarRazon.Size = new System.Drawing.Size(160, 25);
            // 
            // lblFiltroTipo
            // 
            this.lblFiltroTipo.Name = "lblFiltroTipo";
            this.lblFiltroTipo.Size = new System.Drawing.Size(105, 22);
            this.lblFiltroTipo.Text = "supplier.filter.type";
            // 
            // cboFiltroTipo
            // 
            this.cboFiltroTipo.AutoSize = false;
            this.cboFiltroTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltroTipo.Name = "cboFiltroTipo";
            this.cboFiltroTipo.Size = new System.Drawing.Size(150, 25);
            // 
            // lblFiltroEstado
            // 
            this.lblFiltroEstado.Name = "lblFiltroEstado";
            this.lblFiltroEstado.Size = new System.Drawing.Size(119, 22);
            this.lblFiltroEstado.Text = "supplier.filter.status";
            // 
            // cboFiltroEstado
            // 
            this.cboFiltroEstado.AutoSize = false;
            this.cboFiltroEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltroEstado.Name = "cboFiltroEstado";
            this.cboFiltroEstado.Size = new System.Drawing.Size(120, 25);
            // 
            // tsbActualizar
            // 
            this.tsbActualizar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbActualizar.Name = "tsbActualizar";
            this.tsbActualizar.Size = new System.Drawing.Size(63, 22);
            this.tsbActualizar.Text = "abm.common.refresh";
            // 
            // dgvProveedores
            // 
            this.dgvProveedores.AllowUserToAddRows = false;
            this.dgvProveedores.AllowUserToDeleteRows = false;
            this.dgvProveedores.AutoGenerateColumns = false;
            this.dgvProveedores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProveedores.DataSource = this.bsProveedores;
            this.dgvProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProveedores.Location = new System.Drawing.Point(0, 25);
            this.dgvProveedores.MultiSelect = false;
            this.dgvProveedores.Name = "dgvProveedores";
            this.dgvProveedores.ReadOnly = true;
            this.dgvProveedores.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProveedores.Size = new System.Drawing.Size(1000, 575);
            this.dgvProveedores.TabIndex = 1;
            // 
            // ABMProveedoresForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.dgvProveedores);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ABMProveedoresForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "abm.suppliers.title";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProveedores)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsProveedores)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}