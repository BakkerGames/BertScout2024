using BertScout2024.Databases;
using BertScout2024.Models;

namespace BertScout2024;

public partial class MainPage : ContentPage
{
    private readonly LocalDatabase db = new();

    private TeamMatch item = Globals.item;

    public MainPage()
    {
        InitializeComponent();
        CommentPicker.Items.Clear();
        ScorePicker.Items.Clear();

        foreach (string s in CommentList)
        {
            CommentPicker.Items.Add(s);
        }

        foreach (string s in ScoringList)
        {
            ScorePicker.Items.Add(s);
        }
    }
    private void MainPage_Loaded(object sender, EventArgs e)
    {
        if (Globals.viewFormBody)
        {
            TeamNumber.Text = Globals.item.TeamNumber.ToString();
            MatchNumber.Text = Globals.item.MatchNumber.ToString();
            ScoutName.Text = Globals.item.ScoutName;

            this.item = Globals.item;
            Globals.viewFormBody = false;

            Load_Match();
        }
    }

    private void Load_Match()
    {
        // show the values on the screen
        FillFields(item);
        // disable the top row while entering
        EnableTopRow(false);
    }
    IEnumerable<ConnectionProfile> profiles = Connectivity.Current.ConnectionProfiles;
    private async void Start_Clicked(object sender, EventArgs e)
    {
        if (Start.Text == "Start")
        {
            // check that all fields are valid
            if (!ValidateTeamNumber(TeamNumber.Text)) return;
            if (!ValidateMatchNumber(MatchNumber.Text)) return;

            // get integer values for later use
            var team = int.Parse(TeamNumber.Text);
            var match = int.Parse(MatchNumber.Text);

            // get existing record
            item = await db.GetTeamMatchAsync(team, match);

            // check they entered a scout name
            if (item == null && !ValidateScoutName(ScoutName.Text)) return;

            // update screen fields without leading zeros
            TeamNumber.Text = team.ToString();
            MatchNumber.Text = match.ToString();

            // delete the match
            if (ScoutName.Text == "DELETE")
            {
                bool answer = await DisplayAlert("Confirm", "Are you sure you want to delete this match?", "OK", "Cancel");
                if (answer)
                {
                    await db.DeleteTeamMatchAsync(team, match);
                }
                TeamNumber.Text = "";
                MatchNumber.Text = "";
                ScoutName.Text = "";
                // re-enable top row and focus on team number
                EnableTopRow(true);
                TeamNumber.Focus();
                return;
            }

            // if not found, create new record
            item ??= new()
            {
                TeamNumber = team,
                MatchNumber = match,
                ScoutName = ScoutName.Text,
                Comments = "",
            };

            Load_Match();
        }
        else if (Start.Text == "Save")
        {
            // store the screen fields in the record
            SaveFields(); 
            
            /*
            if (profiles.Contains(ConnectionProfile.WiFi) && false)
            {
                AirtablePage aPage = new AirtablePage();
                var uselessTask = Task.Run(() => aPage.AirtableSender());
                uselessTask.Wait();
                var useless = uselessTask.Result;
            }
            */

            // prepare for next match
            TeamNumber.Text = "";
            var match = int.Parse(MatchNumber.Text);
            var newMatch = Math.Min(match + 1, 999);
            MatchNumber.Text = newMatch.ToString();
            ClearAllFields();
            // re-enable top row and focus on team number
            EnableTopRow(true);
            TeamNumber.Focus();

           
        }
    }

    

    private void SaveFields()
    {
        // store the screen fields into the record
        StoreFields(item);

        item.Auto_Points = 0;
        item.Auto_Points += item.Auto_Amp * 2;
        item.Auto_Points += item.Auto_Speaker * 5;
        if (item.Auto_Leave) item.Auto_Points += 2;

        item.Tele_Points = 0;
        item.Tele_Points += item.Tele_Amp;
        item.Tele_Points += item.Tele_Speaker * 2;
        item.Tele_Points += item.Tele_Amped_Speaker * 5;

        item.Endgame_Points = 0;
        item.Endgame_Points += (item.Endgame_Trap ? 5 : 0);
        item.Endgame_Points += (item.Endgame_Parked ? 1 : 0);
        item.Endgame_Points += (item.Endgame_OnStage ? 3 : 0);
        item.Endgame_Points += (item.Endgame_Spotlit ? 1 : 0);
        item.Endgame_Points += (item.Endgame_Harmony ? 2 : 0);

        // save to database
        item.Changed = true;
        var taskSave = Task.Run(() => db.SaveItemAsync(item));
        taskSave.Wait();
        taskSave.Wait();
    }

    private void CommentPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CommentPicker.SelectedIndex < 0)
            return;
        if (Comments.Text == null)
            Comments.Text = "";
        else if (Comments.Text.Length > 0 && !Comments.Text.EndsWith(' '))
            Comments.Text += " ";
        Comments.Text += CommentPicker.SelectedItem.ToString() + " ";
        CommentPicker.SelectedIndex = -1;
        SaveFields();
    }

    // Autonomous

    private void ButtonAutoSpeakerMinus_Clicked(object sender, EventArgs e)
    {
        if
            (item.Auto_Speaker > 0)
        {
            item.Auto_Speaker--;
            LabelAutoSpeaker.Text = item.Auto_Speaker.ToString();
            SaveFields();
        }
    }

    private void ScorePicker_Picked(object sender, EventArgs e) {
        if (ScorePicker.SelectedIndex < 0)
            return;
        if (Comments.Text == null)
            Comments.Text = "";
        else if (Comments.Text.Length > 0 && !Comments.Text.EndsWith(' '))
            Comments.Text += " ";
        Comments.Text += " - " + ScorePicker.SelectedItem.ToString() + " ";
        CommentPicker.SelectedIndex = -1;
        SaveFields();
    }
    private void ButtonAutoSpeakerPlus_Clicked(object sender, EventArgs e)
    {
        item.Auto_Speaker++;
        LabelAutoSpeaker.Text = item.Auto_Speaker.ToString();
        SaveFields();
    }

    private void ButtonAutoAmpMinus_Clicked(object sender, EventArgs e)
    {
        if
            (item.Auto_Amp > 0)
        {
            item.Auto_Amp--;
            LabelAutoAmp.Text = item.Auto_Amp.ToString();
            SaveFields();
        }
    }
    private void ButtonAutoAmpPlus_Clicked(object sender, EventArgs e)
    {
        item.Auto_Amp++;
        LabelAutoAmp.Text = item.Auto_Amp.ToString();
        SaveFields();
    }

    private void ButtonAutoLeave_Clicked(object sender, EventArgs e)
    {
        item.Auto_Leave = !item.Auto_Leave;
        ButtonAutoLeave.BackgroundColor = (item.Auto_Leave ? Colors.Green : Colors.Gray);
        SaveFields();
    }

    // Teleop

    private void ButtonTeleSpeakerMinus_Clicked(object sender, EventArgs e)
    {
        if
            (item.Tele_Speaker > 0)
        {
            item.Tele_Speaker--;
            LabelTeleSpeaker.Text = item.Tele_Speaker.ToString();
            SaveFields();
        }
    }
    private void ButtonTeleSpeakerPlus_Clicked(object sender, EventArgs e)
    {
        item.Tele_Speaker++;
        LabelTeleSpeaker.Text = item.Tele_Speaker.ToString();
        SaveFields();
    }

    private void ButtonTeleAmpMinus_Clicked(object sender, EventArgs e)
    {
        if
            (item.Tele_Amp > 0)
        {
            item.Tele_Amp--;
            LabelTeleAmp.Text = item.Tele_Amp.ToString();
            SaveFields();
        }
    }
    private void ButtonTeleAmpPlus_Clicked(object sender, EventArgs e)
    {
        item.Tele_Amp++;
        LabelTeleAmp.Text = item.Tele_Amp.ToString();
        SaveFields();
    }

    private void ButtonTeleAmplifiedMinus_Clicked(object sender, EventArgs e)
    {
        if (item.Tele_Amped_Speaker > 0)
        {
            item.Tele_Amped_Speaker--;
            LabelTeleAmplified.Text = item.Tele_Amped_Speaker.ToString();
            SaveFields();
        }
    }
    private void ButtonTeleAmplifiedPlus_Clicked(object sender, EventArgs e)
    {
        item.Tele_Amped_Speaker++;
        LabelTeleAmplified.Text = item.Tele_Amped_Speaker.ToString();
        SaveFields();
    }

    private void ButtonTeleCoopertition_Clicked(object sender, EventArgs e)
    {
        item.Tele_Coop = !item.Tele_Coop;
        ButtonTeleCoopertition.BackgroundColor = (item.Tele_Coop ? Colors.Green : Colors.Gray);
        SaveFields();
    }

    // Endgame

    private void ButtonEndgameParked_Clicked(object sender, EventArgs e)
    {
        SetButton_Parked(!item.Endgame_Parked);
        if (item.Endgame_Parked)
        {
            SetButton_OnStage(false);
            SetButton_Harmony(false);
            SetButton_Spotlit(false);
        }
        SaveFields();
    }

    private void ButtonEndgameOnStage_Clicked(object sender, EventArgs e)
    {
        SetButton_OnStage(!item.Endgame_OnStage);
        if (item.Endgame_OnStage)
        {
            SetButton_Parked(false);
        }
        SaveFields();
    }

    private void ButtonEndgameHarmony_Clicked(object sender, EventArgs e)
    {
        SetButton_Harmony(!item.Endgame_Harmony);
        if (item.Endgame_Harmony)
        {
            SetButton_OnStage(true);
            SetButton_Parked(false);
        }
        SaveFields();
    }

    private void ButtonEndgameSpotlit_Clicked(object sender, EventArgs e)
    {
        SetButton_Spotlit(!item.Endgame_Spotlit);
        if (item.Endgame_Spotlit)
        {
            SetButton_OnStage(true);
            SetButton_Parked(false);
        }
        SaveFields();
    }

    private void ButtonEndgameTrap_Clicked(object sender, EventArgs e)
    {
        SetButton_Trap(!item.Endgame_Trap);
        SaveFields();
    }

    private void Comments_TextChanged(object sender, TextChangedEventArgs e)
    {
        item.Comments = Comments?.Text ?? "";
    }

    #region ButtonEvents

    private void SetButton_Parked(bool value)
    {
        item.Endgame_Parked = value;
        ButtonEndgameParked.BackgroundColor = (value ? Colors.Green : Colors.Gray);
    }

    private void SetButton_OnStage(bool value)
    {
        item.Endgame_OnStage = value;
        ButtonEndgameOnStage.BackgroundColor = (value ? Colors.Green : Colors.Gray);
    }

    private void SetButton_Harmony(bool value)
    {
        item.Endgame_Harmony = value;
        ButtonEndgameHarmony.BackgroundColor = (value ? Colors.Green : Colors.Gray);
    }

    private void SetButton_Spotlit(bool value)
    {
        item.Endgame_Spotlit = value;
        ButtonEndgameSpotlit.BackgroundColor = (value ? Colors.Green : Colors.Gray);
    }

    private void SetButton_Trap(bool value)
    {
        item.Endgame_Trap = value;
        ButtonEndgameTrap.BackgroundColor = (value ? Colors.Green : Colors.Gray);
    }

    #endregion
}