using ApkCenterAdminApi.Models;
using System;
using System.Collections.Generic;

namespace ApkCenterAdminApi.Src
{
    public class MatchHelper
    {

        public bool Contains<T>(List<T> listValues, T value)
        {
            for (int i = 0; i < listValues.Count; i++)
            {
                if (listValues[i].Equals(value))
                {
                    return true;
                }
                continue;
            }

            return false;
        }
        public bool Contains<T>(T[] listValues, T value)
        {
            for (int i = 0; i < listValues.Length; i++)
            {
                if (listValues[i].Equals(value))
                {
                    return true;
                }
                continue;
            }

            return false;
        }

        public int Matched(List<AppModel> listValues, AppModel value)
        {
            for (int i = 0; i < listValues.Count; i++)
            {
                if (listValues[i].Title == value.Title)
                {
                    return i;
                }
                continue;
            }

            return -1;
        }
        public int Matched<T>(List<T> listValues, T value)
        {
            for (int i = 0; i < listValues.Count; i++)
            {
                if (listValues[i].Equals(value))
                {
                    return i;
                }
                continue;
            }

            return -1;
        }


    }
}
