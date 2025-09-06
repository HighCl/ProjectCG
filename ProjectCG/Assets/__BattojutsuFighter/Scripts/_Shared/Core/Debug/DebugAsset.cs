using UnityEngine;
using System;
using System.Linq;
using MinbGamesLib;

[CreateAssetMenu(fileName = "DebugAsset", menuName = "Debug/DebugAsset")]
public class DebugAsset : BaseDebugAsset<DebugAsset>
{
    #region << =========== PROTECTED METHODS =========== >>

    protected override void InitCustomTags()
    {
        foreach (CustomDebugType type in Enum.GetValues(typeof(CustomDebugType)))
        {
            DebugTag<CustomDebugType> customTag = new DebugTag<CustomDebugType>(type, GetCustomIconMapper());

            var existingSetting = _tagSettings.FirstOrDefault(s =>
                s.TagName == customTag.Name && s.TagValue == customTag.Value);

            if (existingSetting == null)
            {
                _tagSettings.Add(DebugTagSetting.Create(customTag));
            }
            else
            {
                existingSetting.UpdateIcon(customTag.Icon);
            }
        }
    }

    private static Func<CustomDebugType, string> GetCustomIconMapper()
    {
        return type => type switch
        {
            _ => "🏷️"
        };
    }

    #endregion
}