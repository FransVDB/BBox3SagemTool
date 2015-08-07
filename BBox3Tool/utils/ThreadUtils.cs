using System.Windows.Forms;

namespace BBox3Tool.utils
{
    public class ThreadUtils
    {
        /// <summary>
        /// Set label text from within a thread
        /// </summary>
        /// <param name="label">Label</param>
        /// <param name="text">Text to be set</param>
        public static void setLabelTextFromThread(Label label, string text)
        {
            label.Invoke((MethodInvoker)delegate
            {
                label.Text = text;
            });
        }

        /// <summary>
        /// Set panel visibility from within a thread
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <param name="visible">Visible or not</param>
        public static void setPanelVisibilityFromThread(Panel panel, bool visible)
        {
            panel.Invoke((MethodInvoker)delegate
            {
                panel.Visible = visible;
            });
        }

        /// <summary>
        /// Set button enabled or disabled from within a thread
        /// </summary>
        /// <param name="button">Button</param>
        /// <param name="enabled">Enabled or not</param>
        public static void setButtonEnabledFromThread(Button button, bool enabled)
        {
            button.Invoke((MethodInvoker)delegate
            {
                button.Enabled = enabled;
            });
        }
    }
}
