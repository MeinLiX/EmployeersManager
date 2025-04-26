using EmployeersManager.Core.Interfaces;
using System.Windows.Controls;

namespace EmployeersManager.Dialog
{
    public partial class ExportImportDialog : UserControl, IDialogView
    {
        public ExportImportDialog()
        {
            InitializeComponent();
        }

        public IDialogView BindViewModel(IDialogViewModel viewModel)
        {
            DataContext = viewModel;
            return this;
        }
    }
}
