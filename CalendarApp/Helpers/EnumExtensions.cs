﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Helpers
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Return the display name for the provided enum.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string DisplayName(this Enum item)
        {
            var displayAttr = item.GetAttributeOfType<DisplayAttribute>();

            if (displayAttr != null)
            {
                return displayAttr.Name;
            }

            return item.ToString();
        }

        public static int DisplayOrder(this Enum item)
        {
            var displayAttr = item.GetAttributeOfType<DisplayAttribute>();

            if (displayAttr != null)
            {
                return displayAttr.Order;
            }

            return Convert.ToInt32(item);
        }
    }

    public static class EnumHelper
    {
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example>string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;</example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
