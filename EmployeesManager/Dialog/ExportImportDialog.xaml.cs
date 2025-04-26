using EmployeesManager.Core.Interfaces;
using System.Windows.Controls;

namespace EmployeesManager.Dialog
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
