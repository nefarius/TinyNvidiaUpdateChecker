﻿namespace TinyNvidiaUpdateChecker.Forms
{
    partial class ConfigurationForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            groupBox1 = new System.Windows.Forms.GroupBox();
            minimalCheckBox = new System.Windows.Forms.CheckBox();
            updateCheckBox = new System.Windows.Forms.CheckBox();
            cancelButton = new System.Windows.Forms.Button();
            saveButton = new System.Windows.Forms.Button();
            groupBox2 = new System.Windows.Forms.GroupBox();
            sdRadioButton = new System.Windows.Forms.RadioButton();
            grdRadioButton = new System.Windows.Forms.RadioButton();
            label1 = new System.Windows.Forms.Label();
            multiGpuGroupBox = new System.Windows.Forms.GroupBox();
            resetGpuButton = new System.Windows.Forms.Button();
            groupBox4 = new System.Windows.Forms.GroupBox();
            experimentalCheckBox = new System.Windows.Forms.CheckBox();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            multiGpuGroupBox.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(minimalCheckBox);
            groupBox1.Controls.Add(updateCheckBox);
            groupBox1.Location = new System.Drawing.Point(12, 43);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(246, 97);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "General";
            // 
            // minimalCheckBox
            // 
            minimalCheckBox.AutoSize = true;
            minimalCheckBox.Location = new System.Drawing.Point(12, 56);
            minimalCheckBox.Name = "minimalCheckBox";
            minimalCheckBox.Size = new System.Drawing.Size(184, 24);
            minimalCheckBox.TabIndex = 2;
            minimalCheckBox.Text = "Enable minimal install?";
            minimalCheckBox.UseVisualStyleBackColor = true;
            // 
            // updateCheckBox
            // 
            updateCheckBox.AutoSize = true;
            updateCheckBox.Location = new System.Drawing.Point(12, 26);
            updateCheckBox.Name = "updateCheckBox";
            updateCheckBox.Size = new System.Drawing.Size(228, 24);
            updateCheckBox.TabIndex = 1;
            updateCheckBox.Text = "Check for updates on startup?";
            updateCheckBox.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.Location = new System.Drawing.Point(195, 417);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(63, 29);
            cancelButton.TabIndex = 8;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // saveButton
            // 
            saveButton.Location = new System.Drawing.Point(95, 417);
            saveButton.Name = "saveButton";
            saveButton.Size = new System.Drawing.Size(94, 29);
            saveButton.TabIndex = 7;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(sdRadioButton);
            groupBox2.Controls.Add(grdRadioButton);
            groupBox2.Location = new System.Drawing.Point(12, 146);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(246, 97);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Driver type";
            // 
            // sdRadioButton
            // 
            sdRadioButton.AutoSize = true;
            sdRadioButton.Location = new System.Drawing.Point(12, 56);
            sdRadioButton.Name = "sdRadioButton";
            sdRadioButton.Size = new System.Drawing.Size(117, 24);
            sdRadioButton.TabIndex = 4;
            sdRadioButton.TabStop = true;
            sdRadioButton.Text = "Studio Driver";
            sdRadioButton.UseVisualStyleBackColor = true;
            // 
            // grdRadioButton
            // 
            grdRadioButton.AutoSize = true;
            grdRadioButton.Location = new System.Drawing.Point(12, 26);
            grdRadioButton.Name = "grdRadioButton";
            grdRadioButton.Size = new System.Drawing.Size(219, 24);
            grdRadioButton.TabIndex = 3;
            grdRadioButton.TabStop = true;
            grdRadioButton.Text = "Game Ready Driver (default)";
            grdRadioButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(141, 20);
            label1.TabIndex = 0;
            label1.Text = "Configuration Menu";
            // 
            // multiGpuGroupBox
            // 
            multiGpuGroupBox.Controls.Add(resetGpuButton);
            multiGpuGroupBox.Enabled = false;
            multiGpuGroupBox.Location = new System.Drawing.Point(12, 249);
            multiGpuGroupBox.Name = "multiGpuGroupBox";
            multiGpuGroupBox.Size = new System.Drawing.Size(246, 94);
            multiGpuGroupBox.TabIndex = 3;
            multiGpuGroupBox.TabStop = false;
            multiGpuGroupBox.Text = "Multi GPU Setup";
            // 
            // resetGpuButton
            // 
            resetGpuButton.Location = new System.Drawing.Point(12, 26);
            resetGpuButton.Name = "resetGpuButton";
            resetGpuButton.Size = new System.Drawing.Size(140, 51);
            resetGpuButton.TabIndex = 5;
            resetGpuButton.Text = "Reset GPU choice\r\n(requires restart)";
            resetGpuButton.UseVisualStyleBackColor = true;
            resetGpuButton.Click += resetGpuButton_Click;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(experimentalCheckBox);
            groupBox4.Location = new System.Drawing.Point(12, 349);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(246, 58);
            groupBox4.TabIndex = 6;
            groupBox4.TabStop = false;
            groupBox4.Text = "Experimental";
            // 
            // experimentalCheckBox
            // 
            experimentalCheckBox.AutoSize = true;
            experimentalCheckBox.Location = new System.Drawing.Point(12, 26);
            experimentalCheckBox.Name = "experimentalCheckBox";
            experimentalCheckBox.Size = new System.Drawing.Size(222, 24);
            experimentalCheckBox.TabIndex = 6;
            experimentalCheckBox.Text = "Use experimental data repo?";
            toolTip1.SetToolTip(experimentalCheckBox, "Uses an experimental GPU metadata repo. This resolves issues with eGPUs and TNUC not able to identify GPUs by name.\r\nNOTE: Release description is not implemented");
            experimentalCheckBox.UseVisualStyleBackColor = true;
            // 
            // ConfigurationForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(270, 458);
            Controls.Add(groupBox4);
            Controls.Add(multiGpuGroupBox);
            Controls.Add(label1);
            Controls.Add(groupBox2);
            Controls.Add(saveButton);
            Controls.Add(cancelButton);
            Controls.Add(groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.Execut‌​ablePath);
            Name = "ConfigurationForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Configuration Menu";
            Load += ConfigurationForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            multiGpuGroupBox.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox minimalCheckBox;
        private System.Windows.Forms.CheckBox updateCheckBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton sdRadioButton;
        private System.Windows.Forms.RadioButton grdRadioButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox multiGpuGroupBox;
        private System.Windows.Forms.Button resetGpuButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox experimentalCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}