using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Dapper;

using ComicPresence.Common.Config;
using ComicPresence.Common.Logging;
using ComicPresence.Common.Security;

namespace ComicPresence.Common.Data
{
    /// <summary>
    /// Manages SQL queries and commands using a micro-ORM pattern. Singleton. 
    /// Must define connection strings in web/app.config file - configuration/connectionStrings/add
    /// </summary>
    public class SqlOrm
    {
        private const int cSqlCommandTimeout = 60;
        private const string cLoggingTypeName = "SqlOrm";
        private string _connStringName;

        public bool PerformanceLoggingEnabled
        {
            get
            {
                return AppSettingsMgr.Instance.GetValue<bool>(AppSettingId.PerformanceLoggingEnabled);
            }
        }

        public static SqlOrm Instance
        {
            get { return _instance.Value; }
        }

        private static readonly Lazy<SqlOrm> _instance = new Lazy<SqlOrm>(() => new SqlOrm(Constants.cPrimaryConnStringName), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        public static SqlOrm AppInstance
        {
            get { return _appInstance.Value; }
        }
        private static readonly Lazy<SqlOrm> _appInstance = new Lazy<SqlOrm>(() => new SqlOrm(Constants.cAppConnStringName), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        private SqlOrm(string connStringName)
        {
            _connStringName = connStringName;
        }

        /// <summary>
        /// Executes a stored procedure and returns a list of results
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parameters">An object with SQL parameters. Property names should match stored procedure parameter names. 
        /// Can be created as an anonymous object.</param>
        /// <param name="readOnly">Whether to use a connection/DB that is read-only (secondary)</param>
        /// <returns>A list of results. Column names are mapped to identically named properties (or via ColumnAttribute).</returns>
        public IList<T> QueryProc<T>(string procedureName, object parameters = null, bool readOnly = false, bool logPerf = true)
        {
            EnsureMapper<T>();

            CommandDefinition cmd = new CommandDefinition(procedureName, parameters, null, cSqlCommandTimeout, CommandType.StoredProcedure);

            return QueryProc<T>(cmd, readOnly, logPerf);
        }

        /// <summary>
        /// Overload of QueryProc&lt;T&gt;
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="readOnly"></param>
        public void ExecuteProc(string procedureName, object parameters = null, bool readOnly = false, bool logPerf = true)
        {
            CommandDefinition cmd = new CommandDefinition(procedureName, parameters, null, cSqlCommandTimeout, CommandType.StoredProcedure);

            ExecuteProc(cmd, readOnly, logPerf);
        }

        private void ExecuteProc(CommandDefinition cmd, bool readOnly = false, bool logPerf = true)
        {
            Stopwatch sw = null;
            if (PerformanceLoggingEnabled)
            {
                sw = new Stopwatch();
                sw.Start();
            }

            using (DbConnection conn = GetConnectionInternal(_connStringName, GetConnectionValue(_connStringName)))
            {
                Dapper.SqlMapper.Execute(conn, cmd);

                if (PerformanceLoggingEnabled)
                {
                    sw.Stop();
                    PerformanceLogMgr.Instance.Log(sw.ElapsedMilliseconds, cLoggingTypeName, nameof(ExecuteProc), cmd.CommandText);
                }
            }
        }

        /// <summary>
        /// Overload of QueryProcAsync&lt;T&gt;
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="readOnly"></param>
        public async Task ExecuteProcAsync(string procedureName, object parameters = null, bool readOnly = false)
        {
            CommandDefinition cmd = new CommandDefinition(procedureName, parameters, null, cSqlCommandTimeout, CommandType.StoredProcedure);

            await ExecuteProcAsync(cmd, readOnly).ConfigureAwait(false);
        }

        private async Task ExecuteProcAsync(CommandDefinition cmd, bool readOnly = false)
        {
            Stopwatch sw = null;
            if (PerformanceLoggingEnabled)
            {
                sw = new Stopwatch();
                sw.Start();
            }

            using (DbConnection conn = await GetConnectionInternalAsync(_connStringName, GetConnectionValue(_connStringName)).ConfigureAwait(false))
            {
                await Dapper.SqlMapper.ExecuteAsync(conn, cmd).ConfigureAwait(false);

                if (PerformanceLoggingEnabled)
                {
                    sw.Stop();
                    PerformanceLogMgr.Instance.Log(sw.ElapsedMilliseconds, cLoggingTypeName, nameof(ExecuteProcAsync), cmd.CommandText);
                }
            }
        }

        /// <summary>
        /// Async version of QueryProc. See QueryProc for details. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public async Task<IList<T>> QueryProcAsync<T>(string procedureName, object parameters = null, bool readOnly = false)
        {
            EnsureMapper<T>();

            CommandDefinition cmd = new CommandDefinition(procedureName, parameters, null, cSqlCommandTimeout, CommandType.StoredProcedure);

            return await QueryProcAsync<T>(cmd, readOnly).ConfigureAwait(false);
        }

        /// <summary>
        /// Similar to QueryProc<typeparamref name="T">T</typeparamref>, but retrieves multiple result sets. 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IList<IList<object>> QueryProcMultiple(string procedureName, object parameters, Type[] resultSetTypes, bool readOnly = false)
        {
            return QueryProcMultiple(procedureName, parameters, resultSetTypes.ToList(), readOnly);
        }

        /// <summary>
        /// Similar to QueryProc<typeparamref name="T">T</typeparamref>, but retrieves multiple result sets. 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IList<IList<object>> QueryProcMultiple(string procedureName, object parameters, IList<Type> resultSetTypes, bool readOnly = false)
        {
            if (resultSetTypes == null)
                throw new ArgumentNullException("resultSetTypes");

            foreach (Type t in resultSetTypes)
            {
                EnsureMapper(t);
            }

            CommandDefinition cmd = new CommandDefinition(procedureName, parameters, null, cSqlCommandTimeout, CommandType.StoredProcedure);

            return QueryProcMultiple(cmd, resultSetTypes, readOnly);
        }

        /// <summary>
        /// Similar to QueryProcAsync<typeparamref name="T">T</typeparamref>, but retrieves multiple result sets. 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public async Task<IList<IList<object>>> QueryProcMultipleAsync(string procedureName, object parameters, Type[] resultSetTypes, bool readOnly = false)
        {
            return await QueryProcMultipleAsync(procedureName, parameters, resultSetTypes.ToList(), readOnly).ConfigureAwait(false);
        }

        /// <summary>
        /// Similar to QueryProcAsync<typeparamref name="T">T</typeparamref>, but retrieves multiple result sets. 
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public async Task<IList<IList<object>>> QueryProcMultipleAsync(string procedureName, object parameters, IList<Type> resultSetTypes, bool readOnly = false)
        {
            if (resultSetTypes == null)
                throw new ArgumentNullException("resultSetTypes");

            foreach (Type t in resultSetTypes)
            {
                EnsureMapper(t);
            }

            CommandDefinition cmd = new CommandDefinition(procedureName, parameters, null, cSqlCommandTimeout, CommandType.StoredProcedure);

            return await QueryProcMultipleAsync(cmd, resultSetTypes, readOnly).ConfigureAwait(false);
        }

        private IList<T> QueryProc<T>(CommandDefinition cmd, bool readOnly, bool logPerf = true)
        {
            Stopwatch sw = null;
            if (logPerf && PerformanceLoggingEnabled)
            {
                sw = new Stopwatch();
                sw.Start();
            }

            DbConnection conn = null;

            try
            {
                conn = GetConnectionInternal(_connStringName, GetConnectionValue(_connStringName));

                IList<T> result = Dapper.SqlMapper.Query<T>(conn, cmd).ToList();

                if (logPerf && PerformanceLoggingEnabled)
                {
                    sw.Stop();
                    PerformanceLogMgr.Instance.Log(sw.ElapsedMilliseconds, cLoggingTypeName, "QueryProc<T>", cmd.CommandText);
                }

                return result;
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
            }
        }

        private async Task<IList<T>> QueryProcAsync<T>(CommandDefinition cmd, bool readOnly)
        {
            Stopwatch sw = null;
            if (PerformanceLoggingEnabled)
            {
                sw = new Stopwatch();
                sw.Start();
            }

            using (DbConnection conn = await GetConnectionInternalAsync(_connStringName, GetConnectionValue(_connStringName)).ConfigureAwait(false))
            {
                IList<T> results = (await Dapper.SqlMapper.QueryAsync<T>(conn, cmd).ConfigureAwait(false)).ToList();

                if (PerformanceLoggingEnabled)
                {
                    sw.Stop();
                    PerformanceLogMgr.Instance.Log(sw.ElapsedMilliseconds, cLoggingTypeName, "QueryProcAsync<T>", cmd.CommandText);
                }

                return results;
            }
        }

        private async Task<IList<IList<object>>> QueryProcMultipleAsync(CommandDefinition cmd, IList<Type> resultSetTypes, bool readOnly)
        {
            Stopwatch sw = null;
            if (PerformanceLoggingEnabled)
            {
                sw = new Stopwatch();
                sw.Start();
            }

            DbConnection conn = null;
            SqlMapper.GridReader gridReader = null;
            try
            {
                conn = await GetConnectionInternalAsync(_connStringName, GetConnectionValue(_connStringName)).ConfigureAwait(false);

                gridReader = await Dapper.SqlMapper.QueryMultipleAsync(conn, cmd).ConfigureAwait(false);

                IList<IList<object>> results = new List<IList<object>>();

                IList<Task<IEnumerable<object>>> resultsTasks = new List<Task<IEnumerable<object>>>();

                foreach (Type resultSetType in resultSetTypes)
                {
                    results.Add(gridReader.Read(resultSetType).ToList());
                }

                if (PerformanceLoggingEnabled)
                {
                    sw.Stop();
                    PerformanceLogMgr.Instance.Log(sw.ElapsedMilliseconds, cLoggingTypeName, "QueryProcMultipleAsync<T>", cmd.CommandText);
                }

                return results;
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
                if (gridReader != null)
                    gridReader.Dispose();
            }
        }

        private IList<IList<object>> QueryProcMultiple(CommandDefinition cmd, IList<Type> resultSetTypes, bool readOnly)
        {
            Stopwatch sw = null;
            if (PerformanceLoggingEnabled)
            {
                sw = new Stopwatch();
                sw.Start();
            }

            DbConnection conn = null;
            SqlMapper.GridReader gridReader = null;
            try
            {
                conn = GetConnectionInternalAsync(_connStringName, GetConnectionValue(_connStringName)).Result;
                gridReader = Dapper.SqlMapper.QueryMultiple(conn, cmd);

                IList<IList<object>> results = new List<IList<object>>();

                IList<Task<IEnumerable<object>>> resultsTasks = new List<Task<IEnumerable<object>>>();

                foreach (Type resultSetType in resultSetTypes)
                {
                    results.Add(gridReader.Read(resultSetType).ToList());
                }

                if (PerformanceLoggingEnabled)
                {
                    sw.Stop();
                    PerformanceLogMgr.Instance.Log(sw.ElapsedMilliseconds, cLoggingTypeName, "QueryProcMultiple<T>", cmd.CommandText);
                }

                return results;
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
                if (gridReader != null)
                    gridReader.Dispose();
            }
        }

        /// <summary>
        /// Note that the connection is disposable.
        /// </summary>
        /// <returns></returns>
        public async Task<DbConnection> GetConnectionAsync()
        {
            return await GetConnectionInternalAsync(_connStringName, GetConnectionValue(_connStringName)).ConfigureAwait(false);
        }

        private void EnsureMapper<T>()
        {
            Type t = typeof(T);
            EnsureMapper(t);
        }

        private void EnsureMapper(Type t)
        {
            if (!(Dapper.SqlMapper.GetTypeMap(t) is ColumnAttributeTypeMapper))
            {
                Dapper.SqlMapper.SetTypeMap(t,
                    new ColumnAttributeTypeMapper(t));
            }
        }

        /// <summary>
        /// Gets a connection to the main DB. Note that the connection is disposable.
        /// </summary>
        /// <returns></returns>
        protected async Task<DbConnection> GetPrimaryConnectionAsync()
        {
            return await GetConnectionInternalAsync(Constants.cPrimaryConnStringName, ConfigSettings.PrimaryConnStringName).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a connection to the secondary (read-only) DB. Note that the connection is disposable.
        /// </summary>
        /// <returns></returns>
        protected async Task<DbConnection> GetSecondaryConnection()
        {
            return await GetConnectionInternalAsync(Constants.cSecondaryConnStringName, ConfigSettings.SecondaryConnStringName).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a connection to the application DB
        /// </summary>
        /// <returns></returns>
        protected async Task<DbConnection> GetAppConnectionAsync()
        {
            return await GetConnectionInternalAsync(Constants.cAppConnStringName, ConfigSettings.AppConnStringName).ConfigureAwait(false);
        }

        /// <summary>
        /// Note that the connection is disposable.
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        protected async Task<DbConnection> GetConnectionInternalAsync(string connectionStringName, string connRegStr)
        {
            string connStr = GetEncryptedConnStr(connectionStringName, connRegStr);

            DbConnection sqlConnection = null;

            if (!string.IsNullOrEmpty(connStr))
            {
                sqlConnection = new StackExchange.Profiling.Data.ProfiledDbConnection(new SqlConnection(connStr), StackExchange.Profiling.MiniProfiler.Current);
                await sqlConnection.OpenAsync().ConfigureAwait(false);
            }
            else
            {
                throw new Common.Config.ConfigurationException(string.Format("Missing connection string {0}", connectionStringName));
            }

            return sqlConnection;
        }

        /// <summary>
        /// Note that the connection is disposable.
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        protected DbConnection GetConnectionInternal(string connectionStringName, string connRegStr)
        {
            string connStr = GetEncryptedConnStr(connectionStringName, connRegStr);

            DbConnection sqlConnection = null;

            if (!string.IsNullOrEmpty(connStr))
            {
                sqlConnection = new StackExchange.Profiling.Data.ProfiledDbConnection(new SqlConnection(connStr), StackExchange.Profiling.MiniProfiler.Current);
                sqlConnection.Open();
            }
            else
            {
                throw new Common.Config.ConfigurationException(String.Format("Missing connection string {0}", connectionStringName));
            }

            return sqlConnection;
        }

        protected string GetEncryptedConnStr(string connectionStringName, string connRegStr)
        {
            string valueName = "DbConn" + connectionStringName;

            string encryptionKey = Convert.ToBase64String(PadBytes(System.Text.Encoding.UTF8.GetBytes(ConfigSettings.ClusterAuthToken), Encryption.cKeyLengthBytes));
            return Encryption.DecryptString_Aes(connRegStr, encryptionKey);
        }

        private static byte[] PadBytes(byte[] partialBytes, int length)
        {
            byte[] key = new byte[length];

            Array.Clear(key, 0, key.Length);
            Array.Copy(partialBytes, key, Math.Min(partialBytes.Length, key.Length));

            return key;
        }

        private string GetConnectionValue(string connectionName)
        {
            switch(connectionName)
            {
                case Constants.cAppConnStringName:
                    return ConfigSettings.AppConnStringName;
                case Constants.cCoreConnStringName:
                    return ConfigSettings.CoreConnStringName;
                case Constants.cSecondaryConnStringName:
                    return ConfigSettings.SecondaryConnStringName;
                default:
                    return ConfigSettings.PrimaryConnStringName;
            }
        }

    }
}
