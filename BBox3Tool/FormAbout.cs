using System.Diagnostics;
using System.Windows.Forms;

namespace BBox3Tool
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();

            labelVersion.Text += " " + Application.ProductVersion;

            LinkLabel.Link linkUserbase = new LinkLabel.Link();
            linkUserbase.LinkData = "http://userbase.be/forum/viewtopic.php?f=43&t=43902";
            linkLabelUserbase.Links.Add(linkUserbase);

            LinkLabel.Link linkADSLBC = new LinkLabel.Link();
            linkADSLBC.LinkData = "http://forum.adsl-bc.org/viewtopic.php?f=8&t=90952";
            linkLabelADSLBC.Links.Add(linkADSLBC);
        }

        private void linkLabelUserbase_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start((string) e.Link.LinkData);
        }

        private void linkLabelADSLBC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start((string) e.Link.LinkData);
        }
    }
}
