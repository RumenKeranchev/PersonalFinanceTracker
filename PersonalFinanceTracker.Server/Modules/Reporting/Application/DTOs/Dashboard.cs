namespace PersonalFinanceTracker.Server.Modules.Reporting.Application.DTOs
{
    public record Dashboard(IEnumerable<string> Labels, IEnumerable<DashboardDataset> Datasets);
    
    public record PointDashboard(IEnumerable<DashboardPointDataset> Datasets);

    public record DashboardDataset(string Label, IEnumerable<decimal> Data);

    public record DashboardPointDataset(string Label, IEnumerable<DatasetPoint> Data);

    public record DatasetPoint(object X, object Y);
}
