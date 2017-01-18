using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrmBlog.Helpers
{
    public static class GeneralHelperExtension
    {
        //public static Nullable<T> ParseNullable<T>(this string txt, Func<string, Nullable<T>> func) where T : struct
        //{
        //    if (!string.IsNullOrEmpty(txt))
        //        return func(txt);
        //    else
        //        return null;
        //}
        public static void ConditionalAddRange<T>(this List<T> list, List<T> appendList,string primary_key_property)
        {
            foreach (var v in appendList)
            {
                object obj = v.GetType().GetProperty(primary_key_property).GetValue(v, null);
                bool ekle = true;
                foreach (var p in list)
                {
                    object src = p.GetType().GetProperty(primary_key_property).GetValue(p, null);
                    if (src.ToString() == obj.ToString())
                        ekle = false;
                }
                if(ekle)
                    list.Add(v);
            }
        }
        public static T ParseStruct<T>(this string txt, Func<string, T> func) where T : struct
        {
            if (string.IsNullOrEmpty(txt))
                return default(T);
            else
                return func(txt);
        }
       
        public static Nullable<T> ParseNullable<T>(this object obj, Func<object, Nullable<T>> func) where T : struct
        {
            if (obj!=null)
                return func(obj);
            else
                return null;
        }
    }
}