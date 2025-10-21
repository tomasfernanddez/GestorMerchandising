using System.Drawing;
using System.Windows.Forms;
using UI.Localization;

namespace UI
{
    public partial class ABMProductosForm : Form
    {
        public ABMProductosForm()
        {
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(900, 600);
            Text = "abm.products.title".Traducir();
        }
    }
}