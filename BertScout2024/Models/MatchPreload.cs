using System.Collections.ObjectModel;

namespace BertScout2024.Models;

public class MatchPreload
{
    public int Match { get; set; } = 0;
    public int Team { get; set; } = 0;

    public ObservableCollection<MatchPreload> PreloadList { get; set; } = [];

    public void Add(MatchPreload m)
    {
        PreloadList.Add(m);
    }
    public void Clear()
    {
        PreloadList.Clear();
    }
}
