using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SSISRabbitMQ.RabbitMQSource
{
  public partial class RabbitMQSourceUIForm : Form
  {
    private Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSComponentMetaData100 metaData;
    private IServiceProvider serviceProvider;
    private IDtsConnectionService connectionService;
    private CManagedComponentWrapper designTimeInstance;

    private class ConnectionManagerItem
    {
      public string ID;
      public string Name { get; set; }
      public RabbitMQConnectionManager.RabbitMQConnectionManager ConnManager { get; set; }

      public override string ToString()
      {
        return Name;
      }
    }

    public RabbitMQSourceUIForm()
    {
      InitializeComponent();
    }

    public RabbitMQSourceUIForm(Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSComponentMetaData100 metaData, IServiceProvider serviceProvider)
      : this()
    {
      this.metaData = metaData;
      this.serviceProvider = serviceProvider;
      this.connectionService = (IDtsConnectionService)serviceProvider.GetService(typeof(IDtsConnectionService));
      this.designTimeInstance = metaData.Instantiate();
    }

    private void RabbitMQSourceUIForm_Load(object sender, EventArgs e)
    {
      var connections = connectionService.GetConnections();

      var queueName = metaData.CustomPropertyCollection[0];
      txtQueueName.Text = queueName.Value;

      string connectionManagerId = string.Empty;

      var currentConnectionManager = this.metaData.RuntimeConnectionCollection[0];//.ConnectionManagerID;
      if (currentConnectionManager != null)
      {
        connectionManagerId = currentConnectionManager.ConnectionManagerID;
      }

      for (int i = 0; i < connections.Count; i++)
      {
        var conn = connections[i].InnerObject as RabbitMQConnectionManager.RabbitMQConnectionManager;

        if (conn != null)
        {
          var item = new ConnectionManagerItem()
          {
            Name = connections[i].Name,
            ConnManager = conn,
            ID = connections[i].ID
          };
          cbConnectionList.Items.Add(item);

          if (connections[i].ID.Equals(connectionManagerId))
          {
            cbConnectionList.SelectedIndex = i;
          }
        }
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrWhiteSpace(txtQueueName.Text))
      {
        designTimeInstance.SetComponentProperty("QueueName", txtQueueName.Text);
      }

      if (cbConnectionList.SelectedItem != null)
      {
        var item = (ConnectionManagerItem)cbConnectionList.SelectedItem;
        this.metaData.RuntimeConnectionCollection[0].ConnectionManagerID = item.ID;
      }

      this.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.Close();
    }

    private void btnNewConnectionManager_Click(object sender, EventArgs e)
    {
      System.Collections.ArrayList created = connectionService.CreateConnection("RABBITMQ");

      foreach (ConnectionManager cm in created)
      {
        var item = new ConnectionManagerItem()
        {
          Name = cm.Name,
          ConnManager = cm.InnerObject as RabbitMQConnectionManager.RabbitMQConnectionManager,
          ID = cm.ID
        };

        cbConnectionList.Items.Insert(0, item);
      }
    }
  }
}
