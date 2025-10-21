using System.Drawing;
using System.Windows.Forms;

namespace UI.Formularios
{
    partial class LogsBitacoraForm
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabControl1;
        private TabPage tabLogs;
        private TabPage tabBitacora;
        private TextBox txtLogs;
        private DataGridView dgvBitacora;
        private Panel panelBotones;
        private Button btnActualizar;
        private Button btnLimpiarLogs;
        private Button btnExportar;
        private Button btnCerrar;
        private Panel panelFiltros;
        private Label lblFiltroFecha;
        private DateTimePicker dtpFiltroFecha;
        private Label lblFiltroUsuario;
        private TextBox txtFiltroUsuario;
        private Label lblFiltroModulo;
        private ComboBox cboFiltroModulo;
        private CheckBox chkSoloErrores;

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
            this.tabControl1 = new TabControl();
            this.tabLogs = new TabPage();
            this.tabBitacora = new TabPage();
            this.txtLogs = new TextBox();
            this.dgvBitacora = new DataGridView();
            this.panelBotones = new Panel();
            this.btnActualizar = new Button();
            this.btnLimpiarLogs = new Button();
            this.btnExportar = new Button();
            this.btnCerrar = new Button();
            this.panelFiltros = new Panel();
            this.lblFiltroFecha = new Label();
            this.dtpFiltroFecha = new DateTimePicker();
            this.lblFiltroUsuario = new Label();
            this.txtFiltroUsuario = new TextBox();
            this.lblFiltroModulo = new Label();
            this.cboFiltroModulo = new ComboBox();
            this.chkSoloErrores = new CheckBox();

            this.tabControl1.SuspendLayout();
            this.tabLogs.SuspendLayout();
            this.tabBitacora.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).BeginInit();
            this.panelBotones.SuspendLayout();
            this.panelFiltros.SuspendLayout();
            this.SuspendLayout();

            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabLogs);
            this.tabControl1.Controls.Add(this.tabBitacora);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(1000, 600);
            this.tabControl1.TabIndex = 0;

            // 
            // tabLogs
            // 
            this.tabLogs.Controls.Add(this.txtLogs);
            this.tabLogs.Location = new Point(4, 22);
            this.tabLogs.Name = "tabLogs";
            this.tabLogs.Padding = new Padding(3);
            this.tabLogs.Size = new Size(992, 574);
            this.tabLogs.TabIndex = 0;
            this.tabLogs.Text = "Logs de Sistema";
            this.tabLogs.UseVisualStyleBackColor = true;

            // 
            // txtLogs
            // 
            this.txtLogs.BackColor = Color.Black;
            this.txtLogs.Dock = DockStyle.Fill;
            this.txtLogs.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.txtLogs.ForeColor = Color.LimeGreen;
            this.txtLogs.Location = new Point(3, 3);
            this.txtLogs.Multiline = true;
            this.txtLogs.Name = "txtLogs";
            this.txtLogs.ReadOnly = true;
            this.txtLogs.ScrollBars = ScrollBars.Both;
            this.txtLogs.Size = new Size(986, 568);
            this.txtLogs.TabIndex = 0;
            this.txtLogs.WordWrap = false;

            // 
            // tabBitacora
            // 
            this.tabBitacora.Controls.Add(this.dgvBitacora);
            this.tabBitacora.Controls.Add(this.panelFiltros);
            this.tabBitacora.Location = new Point(4, 22);
            this.tabBitacora.Name = "tabBitacora";
            this.tabBitacora.Padding = new Padding(3);
            this.tabBitacora.Size = new Size(992, 574);
            this.tabBitacora.TabIndex = 1;
            this.tabBitacora.Text = "Bitácora de Usuarios";
            this.tabBitacora.UseVisualStyleBackColor = true;

            // 
            // dgvBitacora
            // 
            this.dgvBitacora.AllowUserToAddRows = false;
            this.dgvBitacora.AllowUserToDeleteRows = false;
            this.dgvBitacora.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBitacora.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBitacora.Dock = DockStyle.Fill;
            this.dgvBitacora.Location = new Point(3, 53);
            this.dgvBitacora.MultiSelect = false;
            this.dgvBitacora.Name = "dgvBitacora";
            this.dgvBitacora.ReadOnly = true;
            this.dgvBitacora.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvBitacora.Size = new Size(986, 518);
            this.dgvBitacora.TabIndex = 1;

            // 
            // panelFiltros
            // 
            this.panelFiltros.Controls.Add(this.lblFiltroFecha);
            this.panelFiltros.Controls.Add(this.dtpFiltroFecha);
            this.panelFiltros.Controls.Add(this.lblFiltroUsuario);
            this.panelFiltros.Controls.Add(this.txtFiltroUsuario);
            this.panelFiltros.Controls.Add(this.lblFiltroModulo);
            this.panelFiltros.Controls.Add(this.cboFiltroModulo);
            this.panelFiltros.Controls.Add(this.chkSoloErrores);
            this.panelFiltros.Dock = DockStyle.Top;
            this.panelFiltros.Location = new Point(3, 3);
            this.panelFiltros.Name = "panelFiltros";
            this.panelFiltros.Size = new Size(986, 50);
            this.panelFiltros.TabIndex = 0;

            // 
            // lblFiltroFecha
            // 
            this.lblFiltroFecha.AutoSize = true;
            this.lblFiltroFecha.Location = new Point(10, 15);
            this.lblFiltroFecha.Name = "lblFiltroFecha";
            this.lblFiltroFecha.Size = new Size(41, 13);
            this.lblFiltroFecha.TabIndex = 0;
            this.lblFiltroFecha.Text = "Desde:";

            // 
            // dtpFiltroFecha
            // 
            this.dtpFiltroFecha.Format = DateTimePickerFormat.Short;
            this.dtpFiltroFecha.Location = new Point(60, 12);
            this.dtpFiltroFecha.Name = "dtpFiltroFecha";
            this.dtpFiltroFecha.Size = new Size(100, 20);
            this.dtpFiltroFecha.TabIndex = 1;

            // 
            // lblFiltroUsuario
            // 
            this.lblFiltroUsuario.AutoSize = true;
            this.lblFiltroUsuario.Location = new Point(180, 15);
            this.lblFiltroUsuario.Name = "lblFiltroUsuario";
            this.lblFiltroUsuario.Size = new Size(46, 13);
            this.lblFiltroUsuario.TabIndex = 2;
            this.lblFiltroUsuario.Text = "Usuario:";

            // 
            // txtFiltroUsuario
            // 
            this.txtFiltroUsuario.Location = new Point(230, 12);
            this.txtFiltroUsuario.Name = "txtFiltroUsuario";
            this.txtFiltroUsuario.Size = new Size(120, 20);
            this.txtFiltroUsuario.TabIndex = 3;

            // 
            // lblFiltroModulo
            // 
            this.lblFiltroModulo.AutoSize = true;
            this.lblFiltroModulo.Location = new Point(370, 15);
            this.lblFiltroModulo.Name = "lblFiltroModulo";
            this.lblFiltroModulo.Size = new Size(45, 13);
            this.lblFiltroModulo.TabIndex = 4;
            this.lblFiltroModulo.Text = "Módulo:";

            // 
            // cboFiltroModulo
            // 
            this.cboFiltroModulo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboFiltroModulo.FormattingEnabled = true;
            this.cboFiltroModulo.Location = new Point(420, 12);
            this.cboFiltroModulo.Name = "cboFiltroModulo";
            this.cboFiltroModulo.Size = new Size(120, 21);
            this.cboFiltroModulo.TabIndex = 5;

            // 
            // chkSoloErrores
            // 
            this.chkSoloErrores.AutoSize = true;
            this.chkSoloErrores.Location = new Point(560, 14);
            this.chkSoloErrores.Name = "chkSoloErrores";
            this.chkSoloErrores.Size = new Size(83, 17);
            this.chkSoloErrores.TabIndex = 6;
            this.chkSoloErrores.Text = "Solo errores";
            this.chkSoloErrores.UseVisualStyleBackColor = true;

            // 
            // panelBotones
            // 
            this.panelBotones.Controls.Add(this.btnActualizar);
            this.panelBotones.Controls.Add(this.btnLimpiarLogs);
            this.panelBotones.Controls.Add(this.btnExportar);
            this.panelBotones.Controls.Add(this.btnCerrar);
            this.panelBotones.Dock = DockStyle.Bottom;
            this.panelBotones.Location = new Point(0, 600);
            this.panelBotones.Name = "panelBotones";
            this.panelBotones.Size = new Size(1000, 50);
            this.panelBotones.TabIndex = 1;

            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new Point(12, 12);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new Size(90, 26);
            this.btnActualizar.TabIndex = 0;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;

            // 
            // btnLimpiarLogs
            // 
            this.btnLimpiarLogs.Location = new Point(115, 12);
            this.btnLimpiarLogs.Name = "btnLimpiarLogs";
            this.btnLimpiarLogs.Size = new Size(120, 26);
            this.btnLimpiarLogs.TabIndex = 1;
            this.btnLimpiarLogs.Text = "Limpiar Logs";
            this.btnLimpiarLogs.UseVisualStyleBackColor = true;

            // 
            // btnExportar
            // 
            this.btnExportar.Location = new Point(248, 12);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new Size(90, 26);
            this.btnExportar.TabIndex = 2;
            this.btnExportar.Text = "Exportar";
            this.btnExportar.UseVisualStyleBackColor = true;

            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnCerrar.DialogResult = DialogResult.Cancel;
            this.btnCerrar.Location = new Point(898, 12);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new Size(90, 26);
            this.btnCerrar.TabIndex = 3;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;

            // 
            // LogsBitacoraForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.btnCerrar;
            this.ClientSize = new Size(1000, 650);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelBotones);
            this.MinimizeBox = false;
            this.Name = "LogsBitacoraForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Logs del Sistema y Bitácora";

            this.tabControl1.ResumeLayout(false);
            this.tabLogs.ResumeLayout(false);
            this.tabLogs.PerformLayout();
            this.tabBitacora.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).EndInit();
            this.panelBotones.ResumeLayout(false);
            this.panelFiltros.ResumeLayout(false);
            this.panelFiltros.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}