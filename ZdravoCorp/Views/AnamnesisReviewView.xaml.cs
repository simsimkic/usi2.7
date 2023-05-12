using System.Linq;
using System.Numerics;
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
using ZdravoCorp.Models;
using ZdravoCorp.ViewModels;

namespace ZdravoCorp.Views
{
    /// <summary>
    /// Interaction logic for PatirntCrudView.xaml
    /// </summary>
    public partial class AnamnesisReviewView : UserControl
    {
        public AnamnesisReviewView()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is AnamnesisReviewViewModel viewModel)
            {
                viewModel.SortCommand.Execute(null);
            }
        }
    }
}
