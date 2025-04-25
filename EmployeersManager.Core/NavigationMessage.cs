using EmployeersManager.Core.Enums;

namespace EmployeersManager.Core;

public class NavigationMessage(NavigationViewModel toNavigate) : NavigationMessage<object>(toNavigate, default)
{ }

public class NavigationMessage<T>(NavigationViewModel toNavigate, T? arg) where T : class
{
    public NavigationViewModel ToNavigate = toNavigate;
    public T? Arg { get; set; } = arg;
}
