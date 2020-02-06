using System;
using System.ComponentModel;

namespace Poker_WebApp.Utilities
{
    public static class EnumExtentions
    {
        public static string Description(this Enum val)
        {
            var valType = val.GetType();
            var valInfo = valType.GetMember(val.ToString());
            var attributes = valInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
