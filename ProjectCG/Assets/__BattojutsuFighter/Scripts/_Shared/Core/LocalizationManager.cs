using MinbGamesLib;

public class LocalizationManager : BaseLocalizationManager<LocalizationManager>
{
    protected override ILocalization GetLocalization(string key)
    {
        return null; // MEMO: 추후 세팅해야 함 
    }
}