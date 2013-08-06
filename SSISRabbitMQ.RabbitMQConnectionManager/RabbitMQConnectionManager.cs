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
  [DtsConnection(ConnectionType = "RABBITMQ",
  DisplayName = "RabbitMQ Connection Manager",
  Description = "Connection manager for RabbitMQ")]
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

    public override Microsoft.SqlServer.Dts.Runtime.DTSExecResult Validate(Microsoft.SqlServer.Dts.Runtime.IDTSInfoEvents infoEvents)
    {
      if (string.IsNullOrEmpty(HostName))
      {
        return DTSExecResult.Failure;
      }
      else if (string.IsNullOrEmpty(VirtualHost))
      {
        return DTSExecResult.Failure;
      }
      else if (string.IsNullOrEmpty(UserName))
      {
        return DTSExecResult.Failure;
      }
      else if (string.IsNullOrEmpty(Password))
      {
        return DTSExecResult.Failure;
      }
      else if (Port <= 0)
      {
        return DTSExecResult.Failure;
      }

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

      var connection = connFactory.CreateConnection();

      return connection;
    }

    public override void ReleaseConnection(object connection)
    {
      if (connection != null)
      {
        ((IConnection)connection).Close();
      }
    }
  }
}
