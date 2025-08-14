namespace ScreenFilterWinForms
{
    partial class MainForm
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
            this.checkboxOnOff = new System.Windows.Forms.CheckBox();
            this.TrackBrilhoControle = new System.Windows.Forms.TrackBar();
            this.numericIntensidadeBrilho = new System.Windows.Forms.NumericUpDown();
            this.btFechar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBrilhoControle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericIntensidadeBrilho)).BeginInit();
            this.SuspendLayout();
            // 
            // checkboxOnOff
            // 
            this.checkboxOnOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkboxOnOff.AutoSize = true;
            this.checkboxOnOff.Location = new System.Drawing.Point(12, 12);
            this.checkboxOnOff.Name = "checkboxOnOff";
            this.checkboxOnOff.Size = new System.Drawing.Size(50, 23);
            this.checkboxOnOff.TabIndex = 0;
            this.checkboxOnOff.Text = "On/Off";
            this.checkboxOnOff.UseVisualStyleBackColor = true;
            // 
            // TrackBrilhoControle
            // 
            this.TrackBrilhoControle.Location = new System.Drawing.Point(86, 15);
            this.TrackBrilhoControle.Name = "TrackBrilhoControle";
            this.TrackBrilhoControle.Size = new System.Drawing.Size(251, 45);
            this.TrackBrilhoControle.TabIndex = 1;
            // 
            // numericIntensidadeBrilho
            // 
            this.numericIntensidadeBrilho.BackColor = System.Drawing.Color.LightSteelBlue;
            this.numericIntensidadeBrilho.Location = new System.Drawing.Point(343, 15);
            this.numericIntensidadeBrilho.Name = "numericIntensidadeBrilho";
            this.numericIntensidadeBrilho.Size = new System.Drawing.Size(50, 20);
            this.numericIntensidadeBrilho.TabIndex = 2;
            // 
            // btFechar
            // 
            this.btFechar.BackColor = System.Drawing.Color.Transparent;
            this.btFechar.Location = new System.Drawing.Point(158, 58);
            this.btFechar.Name = "btFechar";
            this.btFechar.Size = new System.Drawing.Size(75, 23);
            this.btFechar.TabIndex = 3;
            this.btFechar.Text = "Fechar/Sair";
            this.btFechar.UseVisualStyleBackColor = false;
            this.btFechar.Click += new System.EventHandler(this.btFechar_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(410, 93);
            this.Controls.Add(this.btFechar);
            this.Controls.Add(this.numericIntensidadeBrilho);
            this.Controls.Add(this.TrackBrilhoControle);
            this.Controls.Add(this.checkboxOnOff);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.TrackBrilhoControle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericIntensidadeBrilho)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkboxOnOff; //liga ou desliga a aplicacao
        private System.Windows.Forms.TrackBar TrackBrilhoControle; //controla a intensidade do brilho
        private System.Windows.Forms.NumericUpDown numericIntensidadeBrilho; // intensidade do brilho, so que em numeros
        private System.Windows.Forms.Button btFechar; //fecha a aplicacao e para de rodar 
    }
}