namespace SSISRabbitMQ.RabbitMQConnectionManager
{
  partial class RabbitMQConnectionManagerUIForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RabbitMQConnectionManagerUIForm));
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.nmPort = new System.Windows.Forms.NumericUpDown();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.txtUserName = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.txtVirtualHost = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.txtHost = new System.Windows.Forms.TextBox();
      this.btnTestConnection = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nmPort)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.nmPort);
      this.groupBox1.Controls.Add(this.label5);
      this.groupBox1.Controls.Add(this.label4);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.txtPassword);
      this.groupBox1.Controls.Add(this.txtUserName);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.txtVirtualHost);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.txtHost);
      this.groupBox1.Location = new System.Drawing.Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(319, 161);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      // 
      // nmPort
      // 
      this.nmPort.Location = new System.Drawing.Point(112, 127);
      this.nmPort.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.nmPort.Name = "nmPort";
      this.nmPort.Size = new System.Drawing.Size(91, 20);
      this.nmPort.TabIndex = 10;
      this.nmPort.Value = new decimal(new int[] {
            5672,
            0,
            0,
            0});
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(19, 129);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(26, 13);
      this.label5.TabIndex = 9;
      this.label5.Text = "Port";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(19, 103);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(53, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Password";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(19, 76);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(57, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "UserName";
      // 
      // txtPassword
      // 
      this.txtPassword.Location = new System.Drawing.Point(112, 100);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.Size = new System.Drawing.Size(189, 20);
      this.txtPassword.TabIndex = 5;
      // 
      // txtUserName
      // 
      this.txtUserName.Location = new System.Drawing.Point(112, 73);
      this.txtUserName.Name = "txtUserName";
      this.txtUserName.Size = new System.Drawing.Size(189, 20);
      this.txtUserName.TabIndex = 4;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(19, 49);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(61, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Virtual Host";
      // 
      // txtVirtualHost
      // 
      this.txtVirtualHost.Location = new System.Drawing.Point(112, 46);
      this.txtVirtualHost.Name = "txtVirtualHost";
      this.txtVirtualHost.Size = new System.Drawing.Size(189, 20);
      this.txtVirtualHost.TabIndex = 2;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(19, 22);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(60, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Host Name";
      // 
      // txtHost
      // 
      this.txtHost.Location = new System.Drawing.Point(112, 19);
      this.txtHost.Name = "txtHost";
      this.txtHost.Size = new System.Drawing.Size(189, 20);
      this.txtHost.TabIndex = 0;
      // 
      // btnTestConnection
      // 
      this.btnTestConnection.Location = new System.Drawing.Point(12, 188);
      this.btnTestConnection.Name = "btnTestConnection";
      this.btnTestConnection.Size = new System.Drawing.Size(103, 23);
      this.btnTestConnection.TabIndex = 16;
      this.btnTestConnection.Text = "&Test Connection";
      this.btnTestConnection.UseVisualStyleBackColor = true;
      this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(256, 188);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 15;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point(174, 189);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 14;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // RabbitMQConnectionManagerUIForm
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(343, 223);
      this.Controls.Add(this.btnTestConnection);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.groupBox1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "RabbitMQConnectionManagerUIForm";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "RabbitMQ Connection Manager";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nmPort)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.NumericUpDown nmPort;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.TextBox txtUserName;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtVirtualHost;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtHost;
    private System.Windows.Forms.Button btnTestConnection;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
  }
}