using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Contracts
{
    public static class ArrayHelper
    {
        public static long[] ConvertToListOfIds(string listOfIds)
        {
            if (string.IsNullOrWhiteSpace(listOfIds))
            {
                return new long[0];
            }

            string[] idStrings = listOfIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
            long[] ids = new long[idStrings.Length];


            for (int i = 0; i < idStrings.Length; i++)
            {
                if(long.TryParse(idStrings[i], out long id)){
                    ids[i] = id;

            }
                else
                {
                    return null;
                }
            }
            return ids;


        }

    }
}
