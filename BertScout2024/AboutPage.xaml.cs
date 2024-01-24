using BertScout2024.Databases;

namespace BertScout2024;

public partial class AboutPage : ContentPage
{
    private readonly LocalDatabase db = new();
    
	public AboutPage()
	{
		InitializeComponent();
		LabelAboutDatabase.Text = $"Database path: {db.DatabaseDirPath}";
	}

	public void DarkModeButton_Clicked(object sender, EventArgs e)
	{
		
	}
}