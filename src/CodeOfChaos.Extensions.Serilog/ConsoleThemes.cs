// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Ansi;
using Serilog.Sinks.SystemConsole.Themes;

namespace CodeOfChaos.Extensions.Serilog;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ConsoleThemes {
    public static readonly AnsiConsoleTheme AnnaSasDevTheme = new(
        new Dictionary<ConsoleThemeStyle, string> {
            [ConsoleThemeStyle.Text] = AnsiCodes.RgbForegroundColor(AnsiColorStore.White),
            [ConsoleThemeStyle.SecondaryText] = AnsiCodes.RgbForegroundColor(AnsiColorStore.Silver),
            [ConsoleThemeStyle.TertiaryText] = AnsiCodes.RgbForegroundColor(AnsiColorStore.Gray),
            [ConsoleThemeStyle.Invalid] = AnsiCodes.RgbForegroundColor(AnsiColorStore.Gold),
            [ConsoleThemeStyle.Null] = AnsiCodes.RgbForegroundColor(AnsiColorStore.Coral),
            [ConsoleThemeStyle.Name] = AnsiCodes.RgbForegroundColor(AnsiColorStore.Slategray),
            [ConsoleThemeStyle.String] = AnsiCodes.RgbForegroundColor(AnsiColorStore.Aqua),
            [ConsoleThemeStyle.Number] = AnsiCodes.RgbForegroundColor(AnsiColorStore.Mediumpurple),
            [ConsoleThemeStyle.Boolean] = AnsiCodes.RgbForegroundColor(AnsiColorStore.Coral),
            [ConsoleThemeStyle.Scalar] = AnsiCodes.RgbForegroundColor(AnsiColorStore.Coral),
            [ConsoleThemeStyle.LevelVerbose] = AnsiCodes.RgbForegroundColor(AnsiColorStore.Silver),
            [ConsoleThemeStyle.LevelDebug] = AnsiCodes.RgbForegroundColor(AnsiColorStore.Rose),
            [ConsoleThemeStyle.LevelInformation] = AnsiCodes.RgbForegroundColor(AnsiColorStore.White),
            [ConsoleThemeStyle.LevelWarning] = AnsiCodes.RgbForegroundColor(AnsiColorStore.Gold),
            [ConsoleThemeStyle.LevelError] = AnsiCodes.RgbForegroundColor(AnsiColorStore.White) + AnsiCodes.RgbBackgroundColor(AnsiColorStore.Rose),
            [ConsoleThemeStyle.LevelFatal] = AnsiCodes.RgbForegroundColor(AnsiColorStore.White) + AnsiCodes.RgbBackgroundColor(AnsiColorStore.Maroon)
        }
    );
}
