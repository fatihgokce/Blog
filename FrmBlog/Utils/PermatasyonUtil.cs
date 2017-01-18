using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrmBlog.Utils
{
    public class PermatasyonUtil
    {
        static List<List<T>> ConCat<T>(List<T> ary, List<T> ary2)
        {
            List<List<T>> ret = new List<List<T>>();

            foreach (var item in ary2)
            {
                List<T> temp = new List<T>();
                ary.ForEach(x => temp.Add(x));
                temp.Add(item);
                ret.Add(temp);
            }
            return ret;
        }
        static void RemoveDuplicate<T>(List<List<T>> ary) 
        {

            for (var i = 0; i < ary.Count; i++)
            {
                var item1 = ary[i];
                for (var j = i + 1; j < ary.Count; j++)
                {
                    var item2 = ary[j];
                    bool flag = true;
                    for (int index = 0; index < item1.Count; index++)
                    {
                        if (item1[index].Equals(item2[index]))
                            continue;
                        else
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        if (i < j)
                            ary.RemoveAt(i);
                        else
                            ary.RemoveAt(j);

                        i = 0;
                        break;
                    }
                }
            }
           
        }
        public static List<List<T>> Permutasyon<T>(List<T> source, int size)
        {          
            if (source.Count == size)
            {                
                return new List<List<T>> { source };
            }
            else
            {
                List<List<T>> retVals = new List<List<T>>();
                List<T> ary = new List<T>();
                List<T> tempS = source;
                tempS = tempS.Where(x => !x.Equals(source[0])).ToList();
                ary.Add(source[0]);
                for (int j = 1; j < size - 1; j++)
                {
                    ary.Add(source[j]);
                    tempS = tempS.Where(x => !x.Equals(source[j])).ToList();
                }
                retVals.AddRange(ConCat(ary, tempS));               
                if (size > 2)
                    retVals.AddRange(Permutasyon(source.Where(x => !x.Equals(source[1])).ToList(), size));
                retVals.AddRange(Permutasyon(source.Where(x => !x.Equals(source[0])).ToList(), size));
                RemoveDuplicate(retVals);
                return retVals;
            }
            

        }
    }
}