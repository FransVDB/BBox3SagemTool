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
        public static void SetLabelTextFromThread(Label label, string text)
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
        public static void SetPanelVisibilityFromThread(Panel panel, bool visible)
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
        public static void SetButtonEnabledFromThread(Button button, bool enabled)
        {
            button.Invoke((MethodInvoker)delegate
            {
                button.Enabled = enabled;
            });
        }

        /// <summary>
        /// Set button text from within a thread
        /// </summary>
        /// <param name="button">Button</param>
        /// <param name="text">Button text</param>
        public static void SetButtonTextFromThread(Button button, string text)
        {
            button.Invoke((MethodInvoker)delegate
            {
                button.Text = text;
            });
        }

        /// <summary>
        /// Add loader icon to certain panel from within a thread
        /// </summary>
        /// <param name="panel">Parent panel</param>
        /// <param name="loader">Loader icon</param>
        public static void AddLoaderToPanel(Panel panel, PictureBox loader)
        {
            panel.Invoke((MethodInvoker)delegate
            {
                panel.Controls.Add(loader);
            });
        }

        /// <summary>
        /// Remove loader icon from certain panel from within a thread
        /// </summary>
        /// <param name="panel">Parent panel</param>
        /// <param name="loader">Loader icon</param>
        public static void RemoveLoaderFromPanel(Panel panel, PictureBox loader)
        {
            panel.Invoke((MethodInvoker)delegate
            {
                panel.Controls.Remove(loader);
            });
        }
    }
}
