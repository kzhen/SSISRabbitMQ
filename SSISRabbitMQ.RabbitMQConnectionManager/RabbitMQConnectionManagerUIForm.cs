using Microsoft.SqlServer.Dts.Runtime;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SSISRabbitMQ.RabbitMQConnectionManager
{
  public partial class RabbitMQConnectionManagerUIForm : Form
  {
    private ConnectionManager connectionManager;
    private IServiceProvider serviceProvider;

    public RabbitMQConnectionManagerUIForm()
    {
      InitializeComponent();
    }

    public RabbitMQConnectionManagerUIForm(Microsoft.SqlServer.Dts.Runtime.ConnectionManager connectionManager, IServiceProvider serviceProvider)
      : this()
    {
      this.connectionManager = connectionManager;
      this.serviceProvider = serviceProvider;
      
      SetFormValuesFromConnectionManager();
    }

    private void SetFormValuesFromConnectionManager()
    {
      string hostname = connectionManager.Properties["HostName"].GetValue(connectionManager).ToString();
      string username = connectionManager.Properties["UserName"].GetValue(connectionManager).ToString();
      string password = connectionManager.Properties["Password"].GetValue(connectionManager).ToString();
      string virtualhost = connectionManager.Properties["VirtualHost"].GetValue(connectionManager).ToString();
      string port = connectionManager.Properties["Port"].GetValue(connectionManager).ToString();

      if (!string.IsNullOrWhiteSpace(hostname))
      {
        txtHost.Text = hostname;
      }
      if (!string.IsNullOrWhiteSpace(username))
      {
        txtUserName.Text = username;
      }
      if (!string.IsNullOrWhiteSpace(password))
      {
        txtPassword.Text = password;
      }
      if (!string.IsNullOrWhiteSpace(virtualhost))
      {
        txtVirtualHost.Text = virtualhost;
      }
      if (!string.IsNullOrWhiteSpace(port))
      {
        nmPort.Text = port;
      }
    }

    private void UpdateConnectionFromControls()
    {
      int port = Convert.ToInt32(nmPort.Value);

      connectionManager.Properties["HostName"].SetValue(connectionManager, txtHost.Text);
      connectionManager.Properties["UserName"].SetValue(connectionManager, txtUserName.Text);
      connectionManager.Properties["Password"].SetValue(connectionManager, txtPassword.Text);
      connectionManager.Properties["VirtualHost"].SetValue(connectionManager, txtVirtualHost.Text);
      connectionManager.Properties["Port"].SetValue(connectionManager, port);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      UpdateConnectionFromControls(); 

      this.DialogResult = DialogResult.OK;

      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;

      this.Close();
    }

    private void btnTestConnection_Click(object sender, EventArgs e)
    {
      IConnection connection = null;

      try
      {
        ConnectionFactory rabbitMqConnectionFactory = new ConnectionFactory()
        {
          HostName = txtHost.Text,
          VirtualHost = txtVirtualHost.Text,
          UserName = txtUserName.Text,
          Password = txtPassword.Text,
          Port = Convert.ToInt32(nmPort.Value)
        };

        connection = rabbitMqConnectionFactory.CreateConnection();

        if (connection != null && connection.IsOpen)
        {
          MessageBox.Show("Test connection verified", "RabbitMQ Connection Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
          MessageBox.Show("Test connection failed", "RabbitMQ Connection Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        connection.Close();
      }
      catch (Exception ex)
      {
        MessageBox.Show("Test connection failed!" + Environment.NewLine + ex.Message, "RabbitMQ Connection Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);

        if (connection != null && connection.IsOpen)
        {
          connection.Close();
        }
        else if (connection != null)
        {
          connection.Dispose();
        }
      }
    }
  }
}
