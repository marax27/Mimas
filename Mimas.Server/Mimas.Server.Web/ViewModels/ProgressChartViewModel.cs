namespace Mimas.Server.Web.ViewModels;

public class ProgressChartViewModel
{
    public int Dark { get; }
    public int Danger { get; }
    public int Warning { get; }
    public int Success { get; }

    public float DarkPercentage { get; private set; }
    public float DangerPercentage { get; private set; }
    public float WarningPercentage { get; private set; }
    public float SuccessPercentage { get; private set; }

    public ProgressChartViewModel(int dark, int danger, int warning, int success)
    {
        (Dark, Danger, Warning, Success) = (dark, danger, warning, success);

        var sum = dark + danger + warning + success;
        if (sum == 0) return;

        DarkPercentage = dark * 1f / sum;
        DangerPercentage = danger * 1f / sum;
        WarningPercentage = warning * 1f / sum;
        SuccessPercentage = success * 1f / sum;
    }

    public string GetDarkCssPercentage(float totalWidth) => $"{DarkPercentage * totalWidth}%";
    public string GetDangerCssPercentage(float totalWidth) => $"{DangerPercentage * totalWidth}%";
    public string GetWarningCssPercentage(float totalWidth) => $"{WarningPercentage * totalWidth}%";
    public string GetSuccessCssPercentage(float totalWidth) => $"{SuccessPercentage * totalWidth}%";
}