using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LunchTimeGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel mwvm = new MainWindowViewModel();
            this.DataContext = mwvm;
            RadTabControl.SelectedItem = mwvm.CurrentDay;
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RadTabControl_OnKeyDown(object sender, KeyEventArgs e)
        {
            UIElement_OnKeyDown(sender,e);
        }
    }
}
