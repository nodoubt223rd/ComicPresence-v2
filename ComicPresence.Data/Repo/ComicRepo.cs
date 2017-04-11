using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MicroOrm.Pocos.SqlGenerator;

using ComicPresence.Data.Interfaces;
using ComicPresence.Data.RepoModels;

namespace ComicPresence.Data.Repo
{
    public class ComicRepo : RepoBase<Comic>, IComicRepo
    {
        public ComicRepo(IDbConnection connection, ISqlGenerator<Comic> sqlGenerator)
            : base(connection, sqlGenerator)
        {
        }
    }
}
