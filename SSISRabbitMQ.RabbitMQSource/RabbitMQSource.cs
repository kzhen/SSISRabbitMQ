using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SSISRabbitMQ.RabbitMQSource
{
  [DtsPipelineComponent(DisplayName = "RabbitMQ Source",
    ComponentType = ComponentType.SourceAdapter, Description = "Connection source for RabbitMQ")]
  public class RabbitMQSource : PipelineComponent
  {
    private IConnection rabbitConnection;
    private string queueName;

    public override void ReleaseConnections()
    {
      if (rabbitMqConnectionManager != null)
      {
        this.rabbitMqConnectionManager.ReleaseConnection(rabbitConnection);
      }
    }

    public override void ProvideComponentProperties()
    {
      // Reset the component.
      base.RemoveAllInputsOutputsAndCustomProperties();
      ComponentMetaData.RuntimeConnectionCollection.RemoveAll();

      IDTSOutput100 output = ComponentMetaData.OutputCollection.New();
      output.Name = "Output";

      IDTSCustomProperty100 queueName = ComponentMetaData.CustomPropertyCollection.New();
      queueName.Name = "QueueName";
      queueName.Description = "The name of the RabbitMQ queue to read messages from";

      IDTSRuntimeConnection100 connection = ComponentMetaData.RuntimeConnectionCollection.New();
      connection.Name = "RabbitMQ";

      CreateColumns();
    }

    private void CreateColumns()
    {
      IDTSOutput100 output = ComponentMetaData.OutputCollection[0];

      output.OutputColumnCollection.RemoveAll();
      output.ExternalMetadataColumnCollection.RemoveAll();

      IDTSOutputColumn100 column1 = output.OutputColumnCollection.New();
      IDTSExternalMetadataColumn100 exColumn1 = output.ExternalMetadataColumnCollection.New();

      IDTSOutputColumn100 column2 = output.OutputColumnCollection.New();
      IDTSExternalMetadataColumn100 exColumn2 = output.ExternalMetadataColumnCollection.New();

      column1.Name = "MessageContents";
      column1.SetDataTypeProperties(DataType.DT_WSTR, 100, 0, 0, 0);

      column2.Name = "RoutingKey";
      column2.SetDataTypeProperties(DataType.DT_WSTR, 100, 0, 0, 0);
    }

    private IDTSRuntimeConnection100 connectionManager;
    private RabbitMQConnectionManager.RabbitMQConnectionManager rabbitMqConnectionManager;

    public override void AcquireConnections(object transaction)
    {
      IDTSRuntimeConnection100 conn = ComponentMetaData.RuntimeConnectionCollection[0];
      conn.ConnectionManagerID = "RabbitMQ";
      this.connectionManager = conn;


      if (ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManager != null)
      {
        ConnectionManager connectionManager = Microsoft.SqlServer.Dts.Runtime.DtsConvert.GetWrapper(
          ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManager);

        this.rabbitMqConnectionManager = connectionManager.InnerObject as RabbitMQConnectionManager.RabbitMQConnectionManager;

        this.queueName = ComponentMetaData.CustomPropertyCollection["QueueName"].Value;

        if (this.rabbitMqConnectionManager == null)
          throw new Exception("Couldn't get the RabbitMQ connection manager, ");

        rabbitConnection = this.rabbitMqConnectionManager.AcquireConnection(transaction) as IConnection;
      }
    }

    public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
    {
      IDTSOutput100 output = ComponentMetaData.OutputCollection[0];
      PipelineBuffer buffer = buffers[0];

      IModel channel = rabbitConnection.CreateModel();
      channel.QueueDeclare(queueName, true, false, false, null);
      QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);

      string consumerTag = channel.BasicConsume(queueName, true, consumer);

      object message;

      while (true)
      {
        bool success = consumer.Queue.Dequeue(100, out message);

        if (success)
        {
          BasicDeliverEventArgs e = (BasicDeliverEventArgs)message;

          var messageContent = System.Text.Encoding.UTF8.GetString(e.Body);

          buffer.AddRow();
          buffer[0] = messageContent;
          buffer[1] = e.RoutingKey;
        }
        else
        {
          break;
        }
      }

      buffer.SetEndOfRowset();
    }
  }
}
