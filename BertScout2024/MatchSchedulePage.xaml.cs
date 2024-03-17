using BertScout2024.Databases;
using BertScout2024.Models;

namespace BertScout2024;

public partial class MatchSchedulePage : ContentPage
{
    private readonly LocalDatabase db = new();

    private MatchPreload matchPreload = new();

	public MatchSchedulePage()
	{
		InitializeComponent();
		MatchesPreloadView.ItemsSource = matchPreload.PreloadList;
    }
}