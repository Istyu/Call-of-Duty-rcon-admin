namespace codRconAdmin
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            IPbox=new TextBox();
            sendCMDbtn=new Button();
            PORTbox=new TextBox();
            rconPWbox=new TextBox();
            CMDbox=new TextBox();
            rconLoginBtn=new Button();
            cmdHintLbl=new Label();
            quitBtn=new Button();
            serverInfoList=new ListView();
            columnHeader1=new ColumnHeader();
            columnHeader2=new ColumnHeader();
            updSerInfoBtn=new Button();
            infoLbl=new Label();
            SuspendLayout();
            // 
            // IPbox
            // 
            IPbox.Location=new Point(12, 30);
            IPbox.Name="IPbox";
            IPbox.Size=new Size(138, 23);
            IPbox.TabIndex=0;
            // 
            // sendCMDbtn
            // 
            sendCMDbtn.Location=new Point(12, 186);
            sendCMDbtn.Name="sendCMDbtn";
            sendCMDbtn.Size=new Size(205, 23);
            sendCMDbtn.TabIndex=1;
            sendCMDbtn.Text="Execute command";
            sendCMDbtn.UseVisualStyleBackColor=true;
            sendCMDbtn.Click+=sendCMDbtn_Click;
            // 
            // PORTbox
            // 
            PORTbox.Location=new Point(156, 30);
            PORTbox.Name="PORTbox";
            PORTbox.Size=new Size(61, 23);
            PORTbox.TabIndex=2;
            // 
            // rconPWbox
            // 
            rconPWbox.Location=new Point(12, 59);
            rconPWbox.Name="rconPWbox";
            rconPWbox.Size=new Size(205, 23);
            rconPWbox.TabIndex=3;
            // 
            // CMDbox
            // 
            CMDbox.Location=new Point(12, 157);
            CMDbox.Name="CMDbox";
            CMDbox.Size=new Size(205, 23);
            CMDbox.TabIndex=4;
            // 
            // rconLoginBtn
            // 
            rconLoginBtn.Location=new Point(12, 88);
            rconLoginBtn.Name="rconLoginBtn";
            rconLoginBtn.Size=new Size(205, 23);
            rconLoginBtn.TabIndex=5;
            rconLoginBtn.Text="Rcon Login";
            rconLoginBtn.UseVisualStyleBackColor=true;
            rconLoginBtn.Click+=rconLoginBtn_Click;
            // 
            // cmdHintLbl
            // 
            cmdHintLbl.AutoSize=true;
            cmdHintLbl.Location=new Point(12, 139);
            cmdHintLbl.Name="cmdHintLbl";
            cmdHintLbl.Size=new Size(90, 15);
            cmdHintLbl.TabIndex=6;
            cmdHintLbl.Text="Command field";
            // 
            // quitBtn
            // 
            quitBtn.Location=new Point(12, 265);
            quitBtn.Name="quitBtn";
            quitBtn.Size=new Size(205, 23);
            quitBtn.TabIndex=7;
            quitBtn.Text="Quit";
            quitBtn.UseVisualStyleBackColor=true;
            quitBtn.Click+=quitBtn_Click;
            // 
            // serverInfoList
            // 
            serverInfoList.Anchor=AnchorStyles.Top|AnchorStyles.Right;
            serverInfoList.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            serverInfoList.Location=new Point(306, 30);
            serverInfoList.Name="serverInfoList";
            serverInfoList.Size=new Size(526, 258);
            serverInfoList.TabIndex=10;
            serverInfoList.UseCompatibleStateImageBehavior=false;
            serverInfoList.View=View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text="Variable";
            columnHeader1.Width=220;
            // 
            // columnHeader2
            // 
            columnHeader2.Text="Content";
            columnHeader2.Width=300;
            // 
            // updSerInfoBtn
            // 
            updSerInfoBtn.Location=new Point(12, 215);
            updSerInfoBtn.Name="updSerInfoBtn";
            updSerInfoBtn.Size=new Size(205, 23);
            updSerInfoBtn.TabIndex=11;
            updSerInfoBtn.Text="Refresh Serverinfo";
            updSerInfoBtn.UseVisualStyleBackColor=true;
            updSerInfoBtn.Click+=updSerInfoBtn_Click;
            // 
            // infoLbl
            // 
            infoLbl.Anchor=AnchorStyles.Top;
            infoLbl.AutoSize=true;
            infoLbl.ForeColor=SystemColors.ActiveBorder;
            infoLbl.Location=new Point(736, 9);
            infoLbl.Name="infoLbl";
            infoLbl.Size=new Size(96, 15);
            infoLbl.TabIndex=12;
            infoLbl.Text="IW rcon tool v1.0";
            // 
            // Form1
            // 
            AutoScaleDimensions=new SizeF(7F, 15F);
            AutoScaleMode=AutoScaleMode.Font;
            ClientSize=new Size(844, 301);
            Controls.Add(infoLbl);
            Controls.Add(updSerInfoBtn);
            Controls.Add(serverInfoList);
            Controls.Add(quitBtn);
            Controls.Add(cmdHintLbl);
            Controls.Add(rconLoginBtn);
            Controls.Add(CMDbox);
            Controls.Add(rconPWbox);
            Controls.Add(PORTbox);
            Controls.Add(sendCMDbtn);
            Controls.Add(IPbox);
            Icon=(Icon)resources.GetObject("$this.Icon");
            MaximumSize=new Size(860, 350);
            MinimumSize=new Size(850, 340);
            Name="Form1";
            Text="IW rcon tool";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox IPbox;
        private Button sendCMDbtn;
        private TextBox PORTbox;
        private TextBox rconPWbox;
        private TextBox CMDbox;
        private Button rconLoginBtn;
        private Label cmdHintLbl;
        private Button quitBtn;
        private ListView serverInfoList;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Button updSerInfoBtn;
        private Label infoLbl;
    }
}