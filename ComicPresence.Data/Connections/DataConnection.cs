using System;
using System.Data;
using System.Data.SqlClient;

using ComicPresence.Common.Config;
using ComicPresence.Common.Security;

namespace ComicPresence.Data.Connections
{
    public class DataConnection : IDisposable
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        private IDbConnection _connection;

        /// <summary>
        /// 
        /// </summary>
        protected IDbConnection Connection
        {
            get
            {
                if (_connection.State != ConnectionState.Open && _connection.State != ConnectionState.Connecting)
                    _connection.Open();

                return new SqlConnection(GetEncryptedConnStr("PrimaryAuthToken"));
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public DataConnection(IDbConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Close the connection if this is open
        /// </summary>
        public void Dispose()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
                _connection.Close();
        }

        protected string GetEncryptedConnStr(string connectionStringName)
        {

            string valueName = connectionStringName;

            string encryptionKey = Convert.ToBase64String(PadBytes(System.Text.Encoding.UTF8.GetBytes(ConfigSettings.ClusterAuthToken), Encryption.cKeyLengthBytes));

            return Encryption.DecryptString_Aes(valueName, encryptionKey);
        }

        private static byte[] PadBytes(byte[] partialBytes, int length)
        {
            byte[] key = new byte[length];

            Array.Clear(key, 0, key.Length);
            Array.Copy(partialBytes, key, Math.Min(partialBytes.Length, key.Length));

            return key;
        }
    }
}
