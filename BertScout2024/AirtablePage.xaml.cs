using BertScout2024.Databases;
using BertScout2024.Models;

namespace BertScout2024;

public partial class AirtablePage
{
    private readonly LocalDatabase db = new();

    public AirtablePage()
    {
        InitializeComponent();
    }

    private async void AirtableSend_Clicked(object sender, EventArgs e)
    {
        try
        {
            AirtableSend.IsEnabled = false;
            AirtableUpdatedLabel.Text = "Sending, please wait...";
            AirtableResults.Text = "";
            InvalidateMeasure();
            Task task = DisplayAlert("Sending", "Sending data to Airtable - Please Wait", "OK");
            List<TeamMatch> matches = await db.GetItemsAsync();
            var count = await AirtableService.AirtableSendRecords(matches);
            var showS = (count == 1) ? "" : "s";
            AirtableUpdatedLabel.Text = $"{count} record{showS} sent to Airtable";
            foreach (TeamMatch item in matches
                .OrderBy(x => $"{x.MatchNumber,3}{x.TeamNumber,5}"))
            {
                if (item.Changed)
                {
                    item.Changed = false;
                    await db.SaveItemAsync(item);
                    if (AirtableResults.Text.Length > 0)
                        AirtableResults.Text += "\r\n";
                    AirtableResults.Text += $"Match {item.MatchNumber,4} - Team {item.TeamNumber,5}";
                }
            }
        }
        catch (Exception ex)
        {
            AirtableResults.Text = ex.Message;
        }
        finally
        {
            AirtableSend.IsEnabled = true;
        }
    }

    private void VerticalStackLayout_SizeChanged(object sender, EventArgs e)
    {
        ScrollResults.HeightRequest = cp.Height - ScrollResults.Y;
    }
}