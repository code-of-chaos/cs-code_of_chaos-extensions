// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Serilog.Sinks.SystemConsole.Themes;
using CodeOfChaos.Ansi;

namespace CodeOfChaos.Extensions.Serilog;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ConsoleThemes {
    public static readonly AnsiConsoleTheme AnnaSasDevTheme = new(
        new Dictionary<ConsoleThemeStyle, string> {
            [ConsoleThemeStyle.Text] = AnsiColor.AsFore("white"),
            [ConsoleThemeStyle.SecondaryText] = AnsiColor.AsFore("silver"),
            [ConsoleThemeStyle.TertiaryText] = AnsiColor.AsFore("gray"),
            [ConsoleThemeStyle.Invalid] = AnsiColor.AsFore("gold"),
            [ConsoleThemeStyle.Null] = AnsiColor.AsFore("coral"),
            [ConsoleThemeStyle.Name] = AnsiColor.AsFore("slategray"),
            [ConsoleThemeStyle.String] = AnsiColor.AsFore("aqua"),
            [ConsoleThemeStyle.Number] = AnsiColor.AsFore("mediumpurple"),
            [ConsoleThemeStyle.Boolean] = AnsiColor.AsFore("coral"),
            [ConsoleThemeStyle.Scalar] = AnsiColor.AsFore("coral"),
            [ConsoleThemeStyle.LevelVerbose] = AnsiColor.AsFore("silver"),
            [ConsoleThemeStyle.LevelDebug] = AnsiColor.AsFore("rose"),
            [ConsoleThemeStyle.LevelInformation] = AnsiColor.AsFore("white"),
            [ConsoleThemeStyle.LevelWarning] = AnsiColor.AsFore("gold"),
            [ConsoleThemeStyle.LevelError] = AnsiColor.AsFore("white") + AnsiColor.AsBack("rose"),
            [ConsoleThemeStyle.LevelFatal] = AnsiColor.AsFore("white") + AnsiColor.AsBack("maroon")
        });
}
