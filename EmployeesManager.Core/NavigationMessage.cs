using EmployeesManager.Core.Enums;

namespace EmployeesManager.Core;

public class NavigationMessage(ViewModelNavigation toNavigate) : NavigationMessage<object>(toNavigate, default)
{ }

public class NavigationMessage<T>(ViewModelNavigation toNavigate, T? arg) where T : class
{
    public ViewModelNavigation ToNavigate = toNavigate;
    public T? Arg { get; set; } = arg;
}
