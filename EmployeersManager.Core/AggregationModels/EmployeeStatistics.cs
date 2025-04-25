namespace EmployeersManager.Core.AggregationModels
{
    public record EmployeeStatistics(int TotalEmployees, int ActiveEmployees, int TerminatedEmployees, decimal AverageSalary, double AverageDaysWorked, Dictionary<string,int> PositionsCount);
}
