using BertScout2024.Models;

namespace BertScout2024;

public partial class MainPage
{
    public void EnableTopRow(bool enable)
    {
        TeamNumber.IsEnabled = enable;
        MatchNumber.IsEnabled = enable;
        FormBody.IsVisible = !enable;
        Start.Text = enable ? "Start" : "Save";
        TeamNumber.TextColor = enable ? Colors.Black : Colors.Gray;
        MatchNumber.TextColor = enable ? Colors.Black : Colors.Gray;
    }

    public static bool ValidateTeamNumber(string teamNumber)
    {
        if (!int.TryParse(teamNumber, out int tNumber))
        {
            return false;
        }
        if (tNumber < 1)
        {
            return false;
        }
        return true;
    }

    public static bool ValidateMatchNumber(string matchNumber)
    {
        if (!int.TryParse(matchNumber, out int mNumber))
        {
            return false;
        }
        if (mNumber < 1)
        {
            return false;
        }
        return true;
    }

    public static bool ValidateScoutName(string scoutName)
    {
        if (string.IsNullOrWhiteSpace(scoutName))
        {
            return false;
        } else if (scoutName.ToLower() == "nft")
        {
            throw new SystemException("Crash!");
        }
        return true;
    }

    public void ClearAllFields()
    {
        Comments.Text = "";
    }

    private void FillFields(TeamMatch item)
    {
        LabelAutoSpeaker.Text = item.Auto_Speaker.ToString();
        LabelAutoAmp.Text = item.Auto_Amp.ToString();

        ButtonAutoLeave.BackgroundColor = item.Auto_Leave ? Colors.Green : Colors.Gray;

        LabelTeleSpeaker.Text = item.Tele_Speaker.ToString();
        LabelTeleAmp.Text = item.Tele_Amp.ToString();
        LabelTeleAmplified.Text = item.Tele_Amplified.ToString();

        ButtonEndgameParked.BackgroundColor = item.Endgame_Parked ? Colors.Green : Colors.Gray;
        ButtonEndgameOnStage.BackgroundColor = item.Endgame_OnStage ? Colors.Green : Colors.Gray;
        ButtonEndgameSpotlit.BackgroundColor = item.Endgame_Spotlit ? Colors.Green : Colors.Gray;
        ButtonEndgameHarmony.BackgroundColor = item.Endgame_Harmony ? Colors.Green : Colors.Gray;

        Comments.Text = item.Comments;
        CommentPicker.SelectedIndex = -1;
    }

    private void StoreFields(TeamMatch item)
    {
        if (string.IsNullOrWhiteSpace(item.ScoutName))
            item.ScoutName = ScoutName.Text;
        // everything else handled by Clicked/Changed events
    }
}