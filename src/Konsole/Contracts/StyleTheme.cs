﻿using System;
using System.Linq;
using static System.ConsoleColor;

namespace Konsole
{
    public class StyleTheme
    {
        public StyleTheme(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));
            if (code.Length != 13) throw new ArgumentOutOfRangeException("code must be exactly 13 characters long.");
            var style = new Style(code);
            Active = style;
            Disabled = style;
            Inactive = style;
        }



        public static StyleTheme[] GetStyleThemes()
        {
            return Style.GetStyles().Select(s => s.ToTheme()).ToArray();
        }

        // can globally override the default theme.
        public static Func<StyleTheme> GlobalDefault = () => new StyleTheme(White, Black);
        public static StyleTheme Default
        {
            get { return GlobalDefault(); }
        }

        public override string ToString()
        {
            return $"{Active.Body.Foreground} on {Active.Body.Background} [{Active.ToString()}]";
        }

        public StyleTheme(Style active, Style inactive, Style disabled = null)
        {
            Active = active;
            Inactive = inactive;
            Disabled = disabled ?? inactive;
        }

        public StyleTheme(ConsoleColor foreground, ConsoleColor background)
        {
            Active = new Style(foreground, background);
            Inactive = Active;
            Disabled = Active;
        }
        public StyleTheme(ConsoleColor foreground, ConsoleColor background, LineThickNess thickness)
        {
            Active = new Style(foreground, background, thickness);
            Inactive = Active;
            Disabled = Active;
        }

        public StyleTheme()
        {
            //Active = Style
        }

        public Style Active { get; }
        public Style Inactive { get; }
        public Style Disabled { get; }

        public StyleTheme WithTitle(ConsoleColor foreground, ConsoleColor background)
        {
            return WithTitle(new Colors(foreground, background));
        }
        public StyleTheme WithTitle(Colors activeColors)
        {
            return new StyleTheme(
                Active.WithTitle(activeColors),
                Inactive.WithTitle(activeColors),
                Disabled.WithTitle(activeColors.ToSelectedItem())
            );
        }

        public StyleTheme WithTitle(Colors active, Colors inactive, Colors disabled)
        {
            return new StyleTheme(
                Active.WithTitle(active),
                Inactive.WithTitle(inactive), 
                Disabled.WithTitle(disabled)
            );
        }

        public StyleTheme WithActive(Style active)
        {
            return new StyleTheme(active, Inactive, Disabled);
        }

        public StyleTheme WithThickness(LineThickNess thickness)
        {
            return new StyleTheme(
                Active.WithThickness(thickness),
                Inactive.WithThickness(thickness),
                Disabled.WithThickness(thickness)
            );
        }

        public StyleTheme WithForeground(ConsoleColor color)
        {
            return new StyleTheme(
                Active.WithForeground(color),
                Inactive.WithForeground(color),
                Disabled.WithForeground(color)
            );
        }
        public StyleTheme WithColor(Colors colors)
        {
            return new StyleTheme(
                Active.WithColors(colors),
                Inactive.WithColors(colors),
                Disabled.WithColors(colors)
            );
        }


        public StyleTheme WithInactive(Style inactive)
        {
            return new StyleTheme(Active, inactive, Disabled);
        }
        public StyleTheme WithDisabled(Style disabled)
        {
            return new StyleTheme(Active, Inactive, disabled);
        }
    }

     

    public static class StyleThemeExtensions
    {
        public static Style GetActive(this StyleTheme src, ControlStatus status)
        {
            switch (status)
            {
                case ControlStatus.Active:
                    return src.Active;
                case ControlStatus.Inactive:
                    return src.Inactive;
                case ControlStatus.Disabled:
                    return src.Disabled;
                case ControlStatus.Undefined:
                    return src.Active;
                default:
                    return src.Active;
            }
        }
    }

}