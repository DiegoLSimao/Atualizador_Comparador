namespace Atualizador
{
    partial class frmPrincipal
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
            components = new System.ComponentModel.Container();
            txtStatus = new TextBox();
            tmrListaExecucao = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // txtStatus
            // 
            txtStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtStatus.Location = new Point(12, 12);
            txtStatus.Multiline = true;
            txtStatus.Name = "txtStatus";
            txtStatus.ScrollBars = ScrollBars.Both;
            txtStatus.Size = new Size(720, 303);
            txtStatus.TabIndex = 0;
            txtStatus.Click += txtStatus_Click;
            // 
            // tmrListaExecucao
            // 
            tmrListaExecucao.Enabled = true;
            tmrListaExecucao.Interval = 200;
            tmrListaExecucao.Tick += tmrListaExecucao_Tick;
            // 
            // frmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(744, 327);
            Controls.Add(txtStatus);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmPrincipal";
            ShowIcon = false;
            Text = "Atualizador";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtStatus;
        private System.Windows.Forms.Timer tmrListaExecucao;
    }
}
