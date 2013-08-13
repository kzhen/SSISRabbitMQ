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
  [DtsPipelineComponent(IconResource = "SSISRabbitMQ.RabbitMQSource.Rabbit.ico",
    DisplayName = "RabbitMQ Source",
    ComponentType = ComponentType.SourceAdapter,
    Description = "Connection source for RabbitMQ",
    UITypeName = "SSISRabbitMQ.RabbitMQSource.RabbitMQSourceUI, SSISRabbitMQ.RabbitMQSource, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ac1c316408dd3955")]
  public class RabbitMQSource : PipelineComponent
  {
    private IConnection rabbitConnection;
    private IModel consumerChannel;
    private QueueingBasicConsumer queueConsumer;

    private RabbitMQConnectionManager.RabbitMQConnectionManager rabbitMqConnectionManager;

    private string queueName;
    private string consumerTag;

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
      connection.ConnectionManagerID = "RabbitMQ";

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
      column1.SetDataTypeProperties(DataType.DT_WSTR, 4000, 0, 0, 0);

      column2.Name = "RoutingKey";
      column2.SetDataTypeProperties(DataType.DT_WSTR, 100, 0, 0, 0);
    }

    public override DTSValidationStatus Validate()
    {
      bool cancel;
      string qName = ComponentMetaData.CustomPropertyCollection["QueueName"].Value;

      if (string.IsNullOrWhiteSpace(qName))
      {
        //Validate that the QueueName property is set
        ComponentMetaData.FireError(0, ComponentMetaData.Name, "The QueueName property must be set", "", 0, out cancel);
        return DTSValidationStatus.VS_ISBROKEN;
      }

      return base.Validate();
    }

    public override void AcquireConnections(object transaction)
    {
      if (ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManager != null)
      {
        ConnectionManager connectionManager = Microsoft.SqlServer.Dts.Runtime.DtsConvert.GetWrapper(
          ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManager);

        this.rabbitMqConnectionManager = connectionManager.InnerObject as RabbitMQConnectionManager.RabbitMQConnectionManager;

        if (this.rabbitMqConnectionManager == null)
          throw new Exception("Couldn't get the RabbitMQ connection manager, ");

        this.queueName = ComponentMetaData.CustomPropertyCollection["QueueName"].Value;

        rabbitConnection = this.rabbitMqConnectionManager.AcquireConnection(transaction) as IConnection;
      }
    }

    public override void ReleaseConnections()
    {
      if (rabbitMqConnectionManager != null)
      {
        this.rabbitMqConnectionManager.ReleaseConnection(rabbitConnection);
      }
    }

    public override void PreExecute()
    {
      try
      {
        this.consumerChannel = rabbitConnection.CreateModel();
        this.consumerChannel.QueueDeclare(queueName, true, false, false, null);
        this.queueConsumer = new QueueingBasicConsumer(this.consumerChannel);
        this.consumerTag = consumerChannel.BasicConsume(queueName, true, queueConsumer);
      }
      catch (Exception)
      {
        ReleaseConnections();
        throw;
      }
    }

    public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
    {
      IDTSOutput100 output = ComponentMetaData.OutputCollection[0];
      PipelineBuffer buffer = buffers[0];

      object message;
      bool success;

      while (queueConsumer.IsRunning)
      {
        try
        {
          success = queueConsumer.Queue.Dequeue(100, out message);
        }
        catch (Exception)
        {
          break;
        }

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

    public override void Cleanup()
    {
      if (consumerChannel.IsOpen)
      {
        if (queueConsumer.IsRunning)
        {
          consumerChannel.BasicCancel(consumerTag);
        }
        consumerChannel.Close();
      }
      base.Cleanup();
    }
  }
}
