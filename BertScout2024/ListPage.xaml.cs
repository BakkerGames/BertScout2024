using BertScout2024.Databases;
using BertScout2024.Models;
using System.Collections.ObjectModel;

namespace BertScout2024;

public partial class ListPage
{
    private readonly LocalDatabase db = new();

    private MatchItem matchItem = new();

    public ListPage()
    {
        InitializeComponent();
        MatchesListView.ItemsSource = matchItem.MatchesList;
        //        ListResults.Text = "";
        // MatchesList.Add(new MatchItem() { Match = "test" });
    }

    //    private async void ShowMatchesAsync()
    //    {
    //        ListResults.Text = "";
    //        List<TeamMatch> matches = await db.GetItemsAsync();
    //        foreach (TeamMatch item in matches
    //            .OrderBy(x => $"{x.MatchNumber,3}{x.MatchNumber,4}"))
    //        {
    //            if (ListResults.Text.Length > 0)
    //                ListResults.Text += "\r\n";
    //            ListResults.Text += $"Match {item.MatchNumber,4} - Team {item.TeamNumber,4} - {item.ScoutName}";
    //        }
    //    }
    private async void ShowMatchesAsync2()
    {
        // clear the current matches
        matchItem.Clear();

        List<TeamMatch> matches = await db.GetItemsAsync();
        foreach (TeamMatch item in matches
            .OrderBy(x => $"{x.MatchNumber,3}{x.MatchNumber,4}"))
        {
            matchItem.Add(new MatchItem() { Match = $"Match {item.MatchNumber,4} - Team {item.TeamNumber,4} - {item.ScoutName}" });
        }
    }

    private async void OpenMatchButton_Clicked(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string matchSub = btn.Text.Substring(6, 4);
        int match = int.Parse(matchSub);
        int team = int.Parse(btn.Text.Substring(18, btn.Text.IndexOf("-", 18) - 19));
        string name = btn.Text.Substring(25);

        Globals.item = await db.GetTeamMatchAsync(team, match);
        Globals.viewFormBody = true;

        await Shell.Current.GoToAsync("//MainPage");
    }

    private void VerticalStackLayout_SizeChanged(object sender, EventArgs e)
    {
//        ScrollResults.HeightRequest = cpListMatches.Height - ScrollResults.Y;
    }

    private void ShowMatchButton_Clicked(object sender, EventArgs e)
    {
//        ShowMatchesAsync();
        ShowMatchesAsync2();
       
    }
}

public class MatchItem
{
    public string Match { get; set; } = "";
    public ObservableCollection<MatchItem> MatchesList { get; set; } = new ObservableCollection<MatchItem>();

    public void Add(MatchItem m)
    {
        MatchesList.Add(m);
    }
    public void Clear()
    {
        MatchesList.Clear();
    }
}