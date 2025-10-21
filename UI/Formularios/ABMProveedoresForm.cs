using System.Drawing;
using System.Windows.Forms;
using UI.Localization;

namespace UI
{
    public partial class ABMProveedoresForm : Form
    {
        public ABMProveedoresForm()
        {
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(900, 600);
            Text = "abm.suppliers.title".Traducir();
        }
    }
}