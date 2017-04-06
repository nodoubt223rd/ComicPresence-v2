using System;
using System.Collections.Generic;
using System.Data;

namespace ComicPresence.Common.Data
{
    /// <summary>
    /// Table-valued parameters
    /// </summary>
    public static class TvpUtils
    {
        /// <summary>
        /// Creates a DataTable that can be used with SqlOrm for TVPs
        /// </summary>
        /// <param name="ints"></param>
        /// <returns></returns>
        public static DataTable GetTvpForListOfInt(IEnumerable<int> ints)
        {
            DataTable dt = new DataTable();
            dt.TableName = "[cp].[ListOfInt]";

            dt.Columns.Add(new DataColumn("value", typeof(int)));

            foreach (int i in ints)
            {
                dt.Rows.Add(i);
            }

            return dt;
        }

        public static DataTable GetTvpForListOfDateTime2(IEnumerable<DateTime> dateTimes)
        {
            DataTable dt = new DataTable();
            dt.TableName = "[cp].[ListOfDatetime2]";

            dt.Columns.Add(new DataColumn("value", typeof(DateTime)));

            foreach (DateTime dateTime in dateTimes)
            {
                dt.Rows.Add(dateTime);
            }

            return dt;
        }
    }
}
