using BertScout2024.Databases;
using BertScout2024.Models;

namespace BertScout2024;

public partial class MainPage : ContentPage
{
    private readonly LocalDatabase db = new();

    private TeamMatch item = new();

    public MainPage()
    {
        InitializeComponent();
        CommentPicker.Items.Clear();
        foreach (string s in CommentList)
        {
            CommentPicker.Items.Add(s);
        }
    }

    private async void Start_Clicked(object sender, EventArgs e)
    {
        if (Start.Text == "Start")
        {
            // check that all fields are valid
            if (!ValidateTeamNumber(TeamNumber.Text)) return;
            if (!ValidateMatchNumber(MatchNumber.Text)) return;
            if (!ValidateScoutName(ScoutName.Text)) return;
            // get integer values for later use
            var team = int.Parse(TeamNumber.Text);
            var match = int.Parse(MatchNumber.Text);
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
            // get existing record
            item = await db.GetTeamMatchAsync(team, match);
            // if not found, create new record
            item ??= new()
            {
                TeamNumber = team,
                MatchNumber = match,
                ScoutName = ScoutName.Text,
                Comments = "",
            };
            // show the values on the screen
            FillFields(item);
            // disable the top row while entering
            EnableTopRow(false);
        }
        else if (Start.Text == "Save")
        {
            // store the screen fields in the record
            SaveFields();
            // prepare for next match
            TeamNumber.Text = "";
            var match = int.Parse(MatchNumber.Text);
            MatchNumber.Text = (match + 1).ToString();
            ClearAllFields();
            // re-enable top row and focus on team number
            EnableTopRow(true);
            TeamNumber.Focus();
        }
    }

    private void SaveFields()
    {
        // store the screen fields in the record
        StoreFields(item);
        // save to database
        item.Changed = true;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        db.SaveItemAsync(item);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    private void CommentPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CommentPicker.SelectedIndex < 0)
            return;
        if (Comments.Text == null)
            Comments.Text = "";
        else if (Comments.Text.Length > 0)
            Comments.Text += " ";
        Comments.Text += CommentPicker.SelectedItem.ToString();
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
        switch (item.Auto_Leave)
        {
            case false:
                ButtonAutoLeave.BackgroundColor = Colors.Gray;
                break;
            case true:
                ButtonAutoLeave.BackgroundColor = Colors.Green;
                break;
        }
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
        if (item.Tele_Amplified > 0)
        {
            item.Tele_Amplified--;
            LabelTeleAmplified.Text = item.Tele_Amplified.ToString();
            SaveFields();
        }
    }
    private void ButtonTeleAmplifiedPlus_Clicked(object sender, EventArgs e)
    {
        if (item.Tele_Amplified < 3)
        {
            item.Tele_Amplified++;
            LabelTeleAmplified.Text = item.Tele_Amplified.ToString();
            SaveFields();

        }
    }

    private void ButtonTeleCoopertition_Clicked(object sender, EventArgs e)
    {
        item.Tele_Coop = !item.Tele_Coop;
        switch (item.Tele_Coop)
        {
            case false:
                ButtonTeleCoopertition.BackgroundColor = Colors.Gray;
                break;
            case true:
                ButtonTeleCoopertition.BackgroundColor = Colors.Green;
                break;
        }
        SaveFields();
    }

    // Endgame

    private void ButtonEndgameParked_Clicked(object sender, EventArgs e)
    {
        item.Endgame_Parked = !item.Endgame_Parked;
        switch (item.Endgame_Parked)
        {
            case false:
                ButtonEndgameParked.BackgroundColor = Colors.Gray;
                break;
            case true:
                ButtonEndgameParked.BackgroundColor = Colors.Green;
                break;
        }
        SaveFields();
    }

    private void ButtonEndgameOnStage_Clicked(object sender, EventArgs e)
    {
        item.Endgame_OnStage = !item.Endgame_OnStage;
        switch (item.Endgame_OnStage)
        {
            case false:
                ButtonEndgameOnStage.BackgroundColor = Colors.Gray;
                break;
            case true:
                ButtonEndgameOnStage.BackgroundColor = Colors.Green;
                break;
        }
        SaveFields();
    }

    private void ButtonEndgameHarmony_Clicked(object sender, EventArgs e)
    {
        item.Endgame_Harmony = !item.Endgame_Harmony;
        switch (item.Endgame_Harmony)
        {
            case false:
                ButtonEndgameHarmony.BackgroundColor = Colors.Gray;
                break;
            case true:
                ButtonEndgameHarmony.BackgroundColor = Colors.Green;
                break;
        }
        SaveFields();
    }

    private void ButtonEndgameSpotlit_Clicked(object sender, EventArgs e)
    {
        item.Endgame_Spotlit = !item.Endgame_Spotlit;
        switch (item.Endgame_Spotlit)
        {
            case false:
                ButtonEndgameSpotlit.BackgroundColor = Colors.Gray;
                break;
            case true:
                ButtonEndgameSpotlit.BackgroundColor = Colors.Green;
                break;
        }
        SaveFields();
    }

    private void Comments_TextChanged(object sender, TextChangedEventArgs e)
    {
        item.Comments = Comments?.Text ?? "";
    }
}