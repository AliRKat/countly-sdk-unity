using Plugins.CountlySDK;
using System.Collections.Generic;
using UnityEngine;

public class UserDetail : MonoBehaviour
{
    public void Increment()
    {
        Countly.Instance.UserProfile.Increment("Samurai");
    }

    public void IncrementBy()
    {
        Countly.Instance.UserProfile.IncrementBy("Silverhand", 2077);
    }

    public void SaveMax()
    {
        Countly.Instance.UserProfile.SaveMax("PowerLevel", 9001);
    }

    public void SaveMin()
    {
        Countly.Instance.UserProfile.SaveMin("BestTime", 23);
    }

    public void Multiply()
    {
        Countly.Instance.UserProfile.Multiply("CurrentScore", 2);
    }

    public void Pull()
    {
        Countly.Instance.UserProfile.Pull("Inventory", "Blackblade");
    }

    public void Push()
    {
        Countly.Instance.UserProfile.Push("Backpack", "Unity");
    }

    public void PushUnique()
    {
        Countly.Instance.UserProfile.PushUnique("OneTimeItems", "AccessCard");
    }

    public void Save()
    {
        Countly.Instance.UserProfile.Save();
    }

    public void SetOnce()
    {
        Countly.Instance.UserProfile.SetOnce("ChosenEnding", "Ending1");
    }

    public void SetProperties()
    {
        Dictionary<string, object> userProperties = new Dictionary<string, object> {
            { "name", "Johnny Silverhand" },
            { "username", "silverhand" },
            { "email", "info@samurai.com" },
            { "organization", "Samurai" },
            { "phone", "+1 555 123 4567" },
            { "picture", "https://static1.thegamerimages.com/wordpress/wp-content/uploads/2021/01/cyberpunk-2077-johnny-silverhand-glasses.jpg" },
            { "gender", "M" },
            { "byear", 1988 },
            { "Boolean", true },
            { "Integer", 34 },
            { "Float", 6.2f },
            { "Quote", "Wake up, Samurai. We have a city to burn." },
            { "SignificantYears", new [] { 2020, 2077, 2088 } },
            { "TruthValues", new [] { true, false, true } },
            { "SkillLevels", new [] { 1.5f, 2.5f, 3.5f } },
            { "FavoriteItems", new [] { "guitar", "revolver", "cyberarm" } },
            { "YearList", new List<int> { 2020, 2077, 2088 } },
            { "BooleanList", new List<bool> { true, false, true } },
            { "SkillLevelList", new List<float> { 1.5f, 2.5f, 3.5f } },
            { "FavoriteItemList", new List<string> { "guitar", "revolver", "cyberarm" } }
        };

        Countly.Instance.UserProfile.SetProperties(userProperties);
    }

    public void SetProperty()
    {
        Countly.Instance.UserProfile.SetProperty("Best Attribute", "Int");
    }
}
