using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PopupBrowser
{
    sealed class Settings : ApplicationSettingsBase
    {
        public Settings(string settingsKey) : base(settingsKey) {}

        public bool IsNew => string.IsNullOrEmpty(this["Version"] as string);

        [UserScopedSetting()]
        public string Version
        {
            get => this["Version"] as string;
            set => this["Version"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("0, 0")]
        public Point WindowPos
        {
            get => (Point)this["WindowPos"];
            set => this["WindowPos"] = value;
        }

        [UserScopedSetting()]
        [DefaultSettingValue("0, 0")]
        public Size WindowSize
        {
            get => (Size)(this["WindowSize"]);
            set => this["WindowSize"] = value;
        }

        private double AsDouble(string key, int defaultValue)
        {
            double result;
            if (!Double.TryParse(this[key] as string, out result))
                return defaultValue;
            else
                return result;
        }
    }

    
}
