using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UI.Formularios
{
    partial class ABMUsuariosForm
    {
        private IContainer components = null;

        // Declaración de controles
        private ToolStrip toolStrip1;
        private ToolStripButton tsbNuevo;
        private ToolStripButton tsbEditar;
        private ToolStripButton tsbCambiarPassword;
        private ToolStripButton tsbBloquear;
        private ToolStripButton tsbActivar;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tsbActualizar;
        private ToolStripButton tsbMostrarBloqueados;  // NUEVO
        private ToolStripButton tsbMostrarInactivos;   // NUEVO
        private DataGridView dgvUsuarios;
        private BindingSource bsUsuarios;
        private Panel panelBotones;
        private Button btnCerrar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.toolStrip1 = new ToolStrip();
            this.tsbNuevo = new ToolStripButton();
            this.tsbEditar = new ToolStripButton();
            this.tsbCambiarPassword = new ToolStripButton();
            this.tsbBloquear = new ToolStripButton();
            this.tsbActivar = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.tsbActualizar = new ToolStripButton();
            this.tsbMostrarBloqueados = new ToolStripButton();  // NUEVO
            this.tsbMostrarInactivos = new ToolStripButton();   // NUEVO
            this.dgvUsuarios = new DataGridView();
            this.bsUsuarios = new BindingSource(this.components);
            this.panelBotones = new Panel();
            this.btnCerrar = new Button();

            this.toolStrip1.SuspendLayout();
            ((ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            ((ISupportInitialize)(this.bsUsuarios)).BeginInit();
            this.panelBotones.SuspendLayout();
            this.SuspendLayout();

            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] {
            this.tsbNuevo,
            this.tsbEditar,
            this.tsbCambiarPassword,
            this.tsbBloquear,
            this.tsbActivar,
            this.toolStripSeparator1,
            this.tsbActualizar,
            new ToolStripSeparator(),           // Separador extra
            this.tsbMostrarBloqueados,         // NUEVO
            this.tsbMostrarInactivos});        // NUEVO
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(1000, 25);
            this.toolStrip1.TabIndex = 0;

            // 
            // tsbNuevo
            // 
            this.tsbNuevo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsbNuevo.Name = "tsbNuevo";
            this.tsbNuevo.Size = new Size(46, 22);
            this.tsbNuevo.Text = "Nuevo";

            // 
            // tsbEditar
            // 
            this.tsbEditar.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsbEditar.Name = "tsbEditar";
            this.tsbEditar.Size = new Size(41, 22);
            this.tsbEditar.Text = "Editar";

            // 
            // tsbCambiarPassword
            // 
            this.tsbCambiarPassword.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsbCambiarPassword.Name = "tsbCambiarPassword";
            this.tsbCambiarPassword.Size = new Size(110, 22);
            this.tsbCambiarPassword.Text = "Cambiar Contraseña";

            // 
            // tsbBloquear
            // 
            this.tsbBloquear.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsbBloquear.Name = "tsbBloquear";
            this.tsbBloquear.Size = new Size(120, 22);
            this.tsbBloquear.Text = "Bloquear/Desbloquear";

            // 
            // tsbActivar
            // 
            this.tsbActivar.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsbActivar.Name = "tsbActivar";
            this.tsbActivar.Size = new Size(110, 22);
            this.tsbActivar.Text = "Activar/Desactivar";

            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 25);

            // 
            // tsbActualizar
            // 
            this.tsbActualizar.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsbActualizar.Name = "tsbActualizar";
            this.tsbActualizar.Size = new Size(63, 22);
            this.tsbActualizar.Text = "Actualizar";

            // 
            // tsbMostrarBloqueados - NUEVO
            // 
            this.tsbMostrarBloqueados.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsbMostrarBloqueados.Name = "tsbMostrarBloqueados";
            this.tsbMostrarBloqueados.Size = new Size(120, 22);
            this.tsbMostrarBloqueados.Text = "Mostrar Bloqueados";

            // 
            // tsbMostrarInactivos - NUEVO
            // 
            this.tsbMostrarInactivos.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsbMostrarInactivos.Name = "tsbMostrarInactivos";
            this.tsbMostrarInactivos.Size = new Size(110, 22);
            this.tsbMostrarInactivos.Text = "Mostrar Inactivos";

            // 
            // dgvUsuarios
            // 
            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.AllowUserToDeleteRows = false;
            this.dgvUsuarios.AutoGenerateColumns = false;
            this.dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsuarios.DataSource = this.bsUsuarios;
            this.dgvUsuarios.Dock = DockStyle.Fill;
            this.dgvUsuarios.Location = new Point(0, 25);
            this.dgvUsuarios.MultiSelect = false;
            this.dgvUsuarios.Name = "dgvUsuarios";
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.Size = new Size(1000, 525);
            this.dgvUsuarios.TabIndex = 1;

            // 
            // panelBotones
            // 
            this.panelBotones.Controls.Add(this.btnCerrar);
            this.panelBotones.Dock = DockStyle.Bottom;
            this.panelBotones.Location = new Point(0, 550);
            this.panelBotones.Name = "panelBotones";
            this.panelBotones.Size = new Size(1000, 50);
            this.panelBotones.TabIndex = 2;

            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnCerrar.DialogResult = DialogResult.Cancel;
            this.btnCerrar.Location = new Point(898, 12);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new Size(90, 26);
            this.btnCerrar.TabIndex = 0;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;

            // 
            // ABMUsuariosForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.btnCerrar;
            this.ClientSize = new Size(1000, 600);
            this.Controls.Add(this.dgvUsuarios);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panelBotones);
            this.Name = "ABMUsuariosForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Gestión de Usuarios";

            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((ISupportInitialize)(this.dgvUsuarios)).EndInit();
            ((ISupportInitialize)(this.bsUsuarios)).EndInit();
            this.panelBotones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}