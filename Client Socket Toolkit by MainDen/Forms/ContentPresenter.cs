using System.Drawing;
using System.Windows.Forms;

namespace MainDen.ClientSocketToolkit
{
    public partial class ContentPresenter : Form
    {
        public ContentPresenter(string content, string title = null, FontFamily fontFamily = null)
        {
            InitializeComponent();
            rtbContent.Text = content ?? "";
            Text = title ?? "Content Presenter";
            rtbContent.Font = new Font(fontFamily ?? Font.FontFamily, Font.Size);
        }
    }
}
