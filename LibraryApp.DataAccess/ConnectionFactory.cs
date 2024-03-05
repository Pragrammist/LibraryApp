using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace LibraryApp.DataAccess
{

    public interface IConnectionFactory
    {
        IDbConnection GetDbConnection();

        void Close();
    }
    public class ConnectionFactory : IConnectionFactory
    {

        IDbConnection _connection = null;

        public void Close()
        {
            _connection?.Close();
            _connection = null;
        }

        public IDbConnection GetDbConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

            if(_connection is null)
            {
                _connection = new SqlConnection(connectionString);
            }

            

            return _connection;
        }
    }
        
}
