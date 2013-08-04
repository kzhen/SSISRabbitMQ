using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Dts.Runtime;
using RabbitMQ.Client;

namespace SSISRabbitMQ.RabbitMQConnectionManager
{
  [DtsConnection(ConnectionType = "SQLCS",
  DisplayName = "SqlConnectionManager (CS)",
  Description = "Connection manager for Sql Server 1")]
  public class RabbitMQConnectionManager : ConnectionManagerBase
  {
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
    public string VirtualHost { get; set; }

    public RabbitMQConnectionManager()
    {
      HostName = "localhost";
      UserName = "guest";
      Password = "guest";
      Port = 5672;
      VirtualHost = "/";
    }

    private IConnection connection;

    public override Microsoft.SqlServer.Dts.Runtime.DTSExecResult Validate(Microsoft.SqlServer.Dts.Runtime.IDTSInfoEvents infoEvents)
    {
      return DTSExecResult.Success;
    }

    public override object AcquireConnection(object txn)
    {
      ConnectionFactory connFactory = new ConnectionFactory()
      {
        UserName = UserName,
        HostName = HostName,
        Password = Password,
        Port = Port,
        VirtualHost = VirtualHost
      };

      connection = connFactory.CreateConnection();

      return connection;
    }

    public override void ReleaseConnection(object connection)
    {
      ((IConnection)connection).Close();
    }
  }
}
