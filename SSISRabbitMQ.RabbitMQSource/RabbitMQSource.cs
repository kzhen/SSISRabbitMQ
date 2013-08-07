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

namespace SSISRabbitMQ.RabbitMQSource
{
  [DtsPipelineComponent(DisplayName = "RabbitMQ Source", 
    ComponentType = ComponentType.SourceAdapter, Description="Connection source for RabbitMQ")]
  public class RabbitMQSource : PipelineComponent
  {
    private IConnection rabbitConnection;

    private string queueName;

    public override void ProvideComponentProperties()
    {
      // Reset the component.
      base.RemoveAllInputsOutputsAndCustomProperties();
      ComponentMetaData.RuntimeConnectionCollection.RemoveAll();

      IDTSOutput100 output = ComponentMetaData.OutputCollection.New();
      output.Name = "Output";

      IDTSRuntimeConnection100 connection = ComponentMetaData.RuntimeConnectionCollection.New();
      connection.Name = "RABBITMQ";

      CreateColumns();
    }

    private void CreateColumns()
    {
      IDTSOutput100 output = ComponentMetaData.OutputCollection[0];

      output.OutputColumnCollection.RemoveAll();
      output.ExternalMetadataColumnCollection.RemoveAll();

      IDTSOutputColumn100 column1 = output.OutputColumnCollection.New();
      IDTSExternalMetadataColumn100 exColumn1 = output.ExternalMetadataColumnCollection.New();

      column1.Name = "MessageContents";
      column1.SetDataTypeProperties(DataType.DT_WSTR, 100, 0, 0, 0);
    }

    private IDTSRuntimeConnection100 connectionManager;

    public override void AcquireConnections(object transaction)
    {
      IDTSRuntimeConnection100 conn = ComponentMetaData.RuntimeConnectionCollection[0];
      conn.ConnectionManagerID = "something";
      this.connectionManager = conn;


      if (ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManager != null)
      {
        ConnectionManager connectionManager = Microsoft.SqlServer.Dts.Runtime.DtsConvert.GetWrapper(
          ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManager);

        RabbitMQConnectionManager.RabbitMQConnectionManager rabbitConnectionManager = connectionManager.InnerObject as RabbitMQConnectionManager.RabbitMQConnectionManager;

        if (rabbitConnectionManager == null)
          throw new Exception("Couldn't get the RabbitMQ connection manager, ");

        queueName = rabbitConnectionManager.QueueName;

        rabbitConnection = rabbitConnectionManager.AcquireConnection(transaction) as IConnection;
      }
    }

    public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
    {
      IDTSOutput100 output = ComponentMetaData.OutputCollection[0];
      PipelineBuffer buffer = buffers[0];

      IModel channel = rabbitConnection.CreateModel();
      QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);

      buffer.SetEndOfRowset();
    }
  }
}
