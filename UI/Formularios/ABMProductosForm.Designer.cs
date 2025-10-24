namespace UI
{
    partial class ABMProductosForm
    {

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNuevo;
        private System.Windows.Forms.ToolStripButton tsbEditar;
        private System.Windows.Forms.ToolStripButton tsbActivar;
        private System.Windows.Forms.ToolStripButton tsbDesactivar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbActualizar;
        private System.Windows.Forms.ToolStripLabel tslBuscar;
        private System.Windows.Forms.ToolStripTextBox txtBuscar;
        private System.Windows.Forms.ToolStripLabel tslCategoria;
        private System.Windows.Forms.ToolStripComboBox cboCategorias;
        private System.Windows.Forms.ToolStripLabel tslProveedor;
        private System.Windows.Forms.ToolStripLabel tslEstado;
        private System.Windows.Forms.ToolStripComboBox cboEstado;
        private System.Windows.Forms.ToolStripComboBox cboProveedores;
        private System.Windows.Forms.DataGridView dgvProductos;

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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNuevo = new System.Windows.Forms.ToolStripButton();
            this.tsbEditar = new System.Windows.Forms.ToolStripButton();
            this.tsbActivar = new System.Windows.Forms.ToolStripButton();
            this.tsbDesactivar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbActualizar = new System.Windows.Forms.ToolStripButton();
            this.tslBuscar = new System.Windows.Forms.ToolStripLabel();
            this.txtBuscar = new System.Windows.Forms.ToolStripTextBox();
            this.tslCategoria = new System.Windows.Forms.ToolStripLabel();
            this.cboCategorias = new System.Windows.Forms.ToolStripComboBox();
            this.tslProveedor = new System.Windows.Forms.ToolStripLabel();
            this.cboProveedores = new System.Windows.Forms.ToolStripComboBox();
            this.tslEstado = new System.Windows.Forms.ToolStripLabel();
            this.cboEstado = new System.Windows.Forms.ToolStripComboBox();
            this.dgvProductos = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNuevo,
            this.tsbEditar,
            this.tsbActivar,
            this.tsbDesactivar,
            this.toolStripSeparator1,
            this.tsbActualizar,
            this.tslBuscar,
            this.txtBuscar,
            this.tslCategoria,
            this.cboCategorias,
            this.tslProveedor,
            this.cboProveedores,
            this.tslEstado,
            this.cboEstado});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(984, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNuevo
            // 
            this.tsbNuevo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbNuevo.Name = "tsbNuevo";
            this.tsbNuevo.Size = new System.Drawing.Size(46, 22);
            this.tsbNuevo.Text = "Nuevo";
            // 
            // tsbEditar
            // 
            this.tsbEditar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbEditar.Name = "tsbEditar";
            this.tsbEditar.Size = new System.Drawing.Size(41, 22);
            this.tsbEditar.Text = "Editar";
            // 
            // tsbActivar
            // 
            this.tsbActivar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbActivar.Name = "tsbActivar";
            this.tsbActivar.Size = new System.Drawing.Size(49, 22);
            this.tsbActivar.Text = "Activar";
            // 
            // tsbDesactivar
            // 
            this.tsbDesactivar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDesactivar.Name = "tsbDesactivar";
            this.tsbDesactivar.Size = new System.Drawing.Size(63, 22);
            this.tsbDesactivar.Text = "Desactivar";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbActualizar
            // 
            this.tsbActualizar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbActualizar.Name = "tsbActualizar";
            this.tsbActualizar.Size = new System.Drawing.Size(63, 22);
            this.tsbActualizar.Text = "Actualizar";
            // 
            // tslBuscar
            // 
            this.tslBuscar.Margin = new System.Windows.Forms.Padding(20, 1, 0, 2);
            this.tslBuscar.Name = "tslBuscar";
            this.tslBuscar.Size = new System.Drawing.Size(45, 22);
            this.tslBuscar.Text = "Buscar";
            // 
            // txtBuscar
            // 
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(160, 25);
            // 
            // tslCategoria
            // 
            this.tslCategoria.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.tslCategoria.Name = "tslCategoria";
            this.tslCategoria.Size = new System.Drawing.Size(61, 22);
            this.tslCategoria.Text = "Categoría";
            // 
            // cboCategorias
            // 
            this.cboCategorias.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategorias.Name = "cboCategorias";
            this.cboCategorias.Size = new System.Drawing.Size(160, 25);
            // 
            // tslProveedor
            // 
            this.tslProveedor.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.tslProveedor.Name = "tslProveedor";
            this.tslProveedor.Size = new System.Drawing.Size(59, 22);
            this.tslProveedor.Text = "Proveedor";
            // 
            // cboProveedores
            // 
            this.cboProveedores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProveedores.Name = "cboProveedores";
            this.cboProveedores.Size = new System.Drawing.Size(160, 25);
            //
            // tslEstado
            //
            this.tslEstado.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.tslEstado.Name = "tslEstado";
            this.tslEstado.Size = new System.Drawing.Size(40, 22);
            this.tslEstado.Text = "Estado";
            //
            // cboEstado
            //
            this.cboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEstado.Name = "cboEstado";
            this.cboEstado.Size = new System.Drawing.Size(160, 25);
            // 
            // dgvProductos
            // 
            this.dgvProductos.AllowUserToAddRows = false;
            this.dgvProductos.AllowUserToDeleteRows = false;
            this.dgvProductos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProductos.Location = new System.Drawing.Point(0, 25);
            this.dgvProductos.MultiSelect = false;
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.ReadOnly = true;
            this.dgvProductos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductos.Size = new System.Drawing.Size(984, 536);
            this.dgvProductos.TabIndex = 1;
            // 
            // ABMProductosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "ABMProductosForm";
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.dgvProductos);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ABMProductosForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Productos";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}