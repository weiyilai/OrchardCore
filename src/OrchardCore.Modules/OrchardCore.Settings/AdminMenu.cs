using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using OrchardCore.Settings.Drivers;

namespace OrchardCore.Settings;

public sealed class AdminMenu : AdminNavigationProvider
{
    private static readonly RouteValueDictionary _routeValues = new()
    {
        { "area", "OrchardCore.Settings" },
        { "groupId", DefaultSiteSettingsDisplayDriver.GroupId },
    };

    internal readonly IStringLocalizer S;

    public AdminMenu(IStringLocalizer<AdminMenu> stringLocalizer)
    {
        S = stringLocalizer;
    }

    protected override ValueTask BuildAsync(NavigationBuilder builder)
    {

        if (NavigationHelper.UseLegacyFormat())
        {
            builder
                .Add(S["Configuration"], NavigationConstants.AdminMenuConfigurationPosition, configuration => configuration
                    .AddClass("menu-configuration")
                    .Id("configuration")
                    .Add(S["Settings"], "1", settings => settings
                        .Add(S["General"], "1", entry => entry
                            .AddClass("general")
                            .Id("general")
                            .Action("Index", "Admin", _routeValues)
                            .Permission(SettingsPermissions.ManageGroupSettings)
                            .LocalNav()
                        ),
                    priority: 1)
                );

            return ValueTask.CompletedTask;
        }

        builder
            .Add(S["Tools"], "after.50", tools => tools
                .Id("tools")
                .AddClass("tools")
            , priority: 1)
            .Add(S["Settings"], "after.100", settings => settings
                .Id("settings")
                .AddClass("settings")
                .Add(S["General"], "before", general => general
                    .AddClass("general")
                    .Id("general")
                    .Action("Index", "Admin", _routeValues)
                    .Permission(SettingsPermissions.ManageGroupSettings)
                    .LocalNav()
                )
            , priority: 1);

        return ValueTask.CompletedTask;
    }
}
