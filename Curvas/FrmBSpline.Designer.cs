namespace Curvas
{
    partial class FrmBSpline
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelCanvas = new System.Windows.Forms.Panel();
            this.panelControles = new System.Windows.Forms.Panel();
            this.labelAlgoritmo = new System.Windows.Forms.Label();
            this.comboBoxAlgoritmos = new System.Windows.Forms.ComboBox();
            this.labelDescripcion = new System.Windows.Forms.Label();
            this.textBoxDescripcion = new System.Windows.Forms.TextBox();
            this.labelGrado = new System.Windows.Forms.Label();
            this.numericUpDownGrado = new System.Windows.Forms.NumericUpDown();
            this.labelResolucion = new System.Windows.Forms.Label();
            this.numericUpDownResolucion = new System.Windows.Forms.NumericUpDown();
            this.checkBoxAnimar = new System.Windows.Forms.CheckBox();
            this.buttonAgregar = new System.Windows.Forms.Button();
            this.buttonLimpiar = new System.Windows.Forms.Button();
            this.buttonDibujar = new System.Windows.Forms.Button();
            this.labelX = new System.Windows.Forms.Label();
            this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
            this.labelY = new System.Windows.Forms.Label();
            this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
            this.listBoxNodos = new System.Windows.Forms.ListBox();
            this.labelNodos = new System.Windows.Forms.Label();
            this.buttonEliminar = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();

            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGrado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownResolucion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();

            this.panelCanvas.BackColor = System.Drawing.Color.White;
            this.panelCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCanvas.Location = new System.Drawing.Point(12, 12);
            this.panelCanvas.Name = "panelCanvas";
            this.panelCanvas.Size = new System.Drawing.Size(600, 450);
            this.panelCanvas.TabIndex = 0;
            this.panelCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.panelCanvas_Paint);
            this.panelCanvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelCanvas_MouseClick);
            this.panelCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelCanvas_MouseDown);
            this.panelCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelCanvas_MouseMove);
            this.panelCanvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelCanvas_MouseUp);

            this.panelControles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelControles.Location = new System.Drawing.Point(618, 12);
            this.panelControles.Name = "panelControles";
            this.panelControles.Size = new System.Drawing.Size(320, 450);
            this.panelControles.TabIndex = 1;

            this.labelAlgoritmo.AutoSize = true;
            this.labelAlgoritmo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelAlgoritmo.Location = new System.Drawing.Point(10, 15);
            this.labelAlgoritmo.Name = "labelAlgoritmo";
            this.labelAlgoritmo.Text = "Algoritmo:";

            this.comboBoxAlgoritmos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlgoritmos.FormattingEnabled = true;
            this.comboBoxAlgoritmos.Location = new System.Drawing.Point(10, 34);
            this.comboBoxAlgoritmos.Name = "comboBoxAlgoritmos";
            this.comboBoxAlgoritmos.Size = new System.Drawing.Size(295, 21);
            this.comboBoxAlgoritmos.TabIndex = 1;
            this.comboBoxAlgoritmos.SelectedIndexChanged += new System.EventHandler(this.comboBoxAlgoritmos_SelectedIndexChanged);

            this.labelDescripcion.AutoSize = true;
            this.labelDescripcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.labelDescripcion.Location = new System.Drawing.Point(10, 58);
            this.labelDescripcion.Name = "labelDescripcion";
            this.labelDescripcion.Text = "Descripción:";

            this.textBoxDescripcion.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBoxDescripcion.Location = new System.Drawing.Point(10, 75);
            this.textBoxDescripcion.Multiline = true;
            this.textBoxDescripcion.Name = "textBoxDescripcion";
            this.textBoxDescripcion.ReadOnly = true;
            this.textBoxDescripcion.Size = new System.Drawing.Size(295, 50);
            this.textBoxDescripcion.TabIndex = 3;
            this.textBoxDescripcion.WordWrap = true;

            this.labelGrado.AutoSize = true;
            this.labelGrado.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelGrado.Location = new System.Drawing.Point(10, 130);
            this.labelGrado.Name = "labelGrado";
            this.labelGrado.Text = "Grado de la Curva:";

            this.numericUpDownGrado.Location = new System.Drawing.Point(10, 149);
            this.numericUpDownGrado.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numericUpDownGrado.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            this.numericUpDownGrado.Value = new decimal(new int[] { 3, 0, 0, 0 });
            this.numericUpDownGrado.Name = "numericUpDownGrado";
            this.numericUpDownGrado.Size = new System.Drawing.Size(295, 20);
            this.numericUpDownGrado.TabIndex = 5;

            this.labelResolucion.AutoSize = true;
            this.labelResolucion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelResolucion.Location = new System.Drawing.Point(10, 175);
            this.labelResolucion.Name = "labelResolucion";
            this.labelResolucion.Text = "Resolución:";

            this.numericUpDownResolucion.Location = new System.Drawing.Point(10, 194);
            this.numericUpDownResolucion.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            this.numericUpDownResolucion.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.numericUpDownResolucion.Value = new decimal(new int[] { 100, 0, 0, 0 });
            this.numericUpDownResolucion.Name = "numericUpDownResolucion";
            this.numericUpDownResolucion.Size = new System.Drawing.Size(295, 20);
            this.numericUpDownResolucion.TabIndex = 6;

            this.checkBoxAnimar.AutoSize = true;
            this.checkBoxAnimar.Location = new System.Drawing.Point(10, 220);
            this.checkBoxAnimar.Name = "checkBoxAnimar";
            this.checkBoxAnimar.Size = new System.Drawing.Size(115, 17);
            this.checkBoxAnimar.TabIndex = 7;
            this.checkBoxAnimar.Text = "Guardar rastros";
            this.checkBoxAnimar.UseVisualStyleBackColor = true;

            this.labelNodos.AutoSize = true;
            this.labelNodos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelNodos.Location = new System.Drawing.Point(10, 245);
            this.labelNodos.Name = "labelNodos";
            this.labelNodos.Text = "Nodos de Control:";

            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(10, 264);
            this.labelX.Name = "labelX";
            this.labelX.Text = "X:";

            this.numericUpDownX.Location = new System.Drawing.Point(30, 264);
            this.numericUpDownX.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.numericUpDownX.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            this.numericUpDownX.Name = "numericUpDownX";
            this.numericUpDownX.Size = new System.Drawing.Size(125, 20);
            this.numericUpDownX.TabIndex = 8;

            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(160, 264);
            this.labelY.Name = "labelY";
            this.labelY.Text = "Y:";

            this.numericUpDownY.Location = new System.Drawing.Point(180, 264);
            this.numericUpDownY.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.numericUpDownY.Maximum = new decimal(new int[] { 450, 0, 0, 0 });
            this.numericUpDownY.Name = "numericUpDownY";
            this.numericUpDownY.Size = new System.Drawing.Size(125, 20);
            this.numericUpDownY.TabIndex = 9;

            this.buttonAgregar.BackColor = System.Drawing.Color.LimeGreen;
            this.buttonAgregar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.buttonAgregar.ForeColor = System.Drawing.Color.White;
            this.buttonAgregar.Location = new System.Drawing.Point(10, 290);
            this.buttonAgregar.Name = "buttonAgregar";
            this.buttonAgregar.Size = new System.Drawing.Size(145, 30);
            this.buttonAgregar.TabIndex = 10;
            this.buttonAgregar.Text = "Agregar Nodo";
            this.buttonAgregar.UseVisualStyleBackColor = false;
            this.buttonAgregar.Click += new System.EventHandler(this.buttonAgregar_Click);

            this.buttonEliminar.BackColor = System.Drawing.Color.Red;
            this.buttonEliminar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.buttonEliminar.ForeColor = System.Drawing.Color.White;
            this.buttonEliminar.Location = new System.Drawing.Point(160, 290);
            this.buttonEliminar.Name = "buttonEliminar";
            this.buttonEliminar.Size = new System.Drawing.Size(145, 30);
            this.buttonEliminar.TabIndex = 11;
            this.buttonEliminar.Text = "Eliminar";
            this.buttonEliminar.UseVisualStyleBackColor = false;
            this.buttonEliminar.Click += new System.EventHandler(this.buttonEliminar_Click);

            this.listBoxNodos.Location = new System.Drawing.Point(10, 326);
            this.listBoxNodos.Name = "listBoxNodos";
            this.listBoxNodos.Size = new System.Drawing.Size(295, 60);
            this.listBoxNodos.TabIndex = 12;

            this.buttonLimpiar.BackColor = System.Drawing.Color.Orange;
            this.buttonLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.buttonLimpiar.ForeColor = System.Drawing.Color.White;
            this.buttonLimpiar.Location = new System.Drawing.Point(10, 391);
            this.buttonLimpiar.Name = "buttonLimpiar";
            this.buttonLimpiar.Size = new System.Drawing.Size(145, 30);
            this.buttonLimpiar.TabIndex = 13;
            this.buttonLimpiar.Text = "Limpiar Todo";
            this.buttonLimpiar.UseVisualStyleBackColor = false;
            this.buttonLimpiar.Click += new System.EventHandler(this.buttonLimpiar_Click);

            this.buttonDibujar.BackColor = System.Drawing.Color.Blue;
            this.buttonDibujar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.buttonDibujar.ForeColor = System.Drawing.Color.White;
            this.buttonDibujar.Location = new System.Drawing.Point(160, 391);
            this.buttonDibujar.Name = "buttonDibujar";
            this.buttonDibujar.Size = new System.Drawing.Size(145, 30);
            this.buttonDibujar.TabIndex = 14;
            this.buttonDibujar.Text = "Dibujar Curva";
            this.buttonDibujar.UseVisualStyleBackColor = false;
            this.buttonDibujar.Click += new System.EventHandler(this.buttonDibujar_Click);

            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 472);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(950, 22);
            this.statusStrip1.TabIndex = 2;

            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 494);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panelCanvas);
            this.Controls.Add(this.panelControles);
            this.panelControles.Controls.Add(this.buttonDibujar);
            this.panelControles.Controls.Add(this.buttonLimpiar);
            this.panelControles.Controls.Add(this.listBoxNodos);
            this.panelControles.Controls.Add(this.buttonEliminar);
            this.panelControles.Controls.Add(this.buttonAgregar);
            this.panelControles.Controls.Add(this.numericUpDownY);
            this.panelControles.Controls.Add(this.labelY);
            this.panelControles.Controls.Add(this.numericUpDownX);
            this.panelControles.Controls.Add(this.labelX);
            this.panelControles.Controls.Add(this.labelNodos);
            this.panelControles.Controls.Add(this.checkBoxAnimar);
            this.panelControles.Controls.Add(this.numericUpDownResolucion);
            this.panelControles.Controls.Add(this.labelResolucion);
            this.panelControles.Controls.Add(this.numericUpDownGrado);
            this.panelControles.Controls.Add(this.labelGrado);
            this.panelControles.Controls.Add(this.textBoxDescripcion);
            this.panelControles.Controls.Add(this.labelDescripcion);
            this.panelControles.Controls.Add(this.comboBoxAlgoritmos);
            this.panelControles.Controls.Add(this.labelAlgoritmo);
            this.Name = "FrmBSpline";
            this.Text = "Sistema de Curvas B-Spline";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmBSpline_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGrado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownResolucion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel panelCanvas;
        private System.Windows.Forms.Panel panelControles;
        private System.Windows.Forms.Label labelAlgoritmo;
        private System.Windows.Forms.ComboBox comboBoxAlgoritmos;
        private System.Windows.Forms.Label labelDescripcion;
        private System.Windows.Forms.TextBox textBoxDescripcion;
        private System.Windows.Forms.Label labelGrado;
        private System.Windows.Forms.NumericUpDown numericUpDownGrado;
        private System.Windows.Forms.Label labelResolucion;
        private System.Windows.Forms.NumericUpDown numericUpDownResolucion;
        private System.Windows.Forms.CheckBox checkBoxAnimar;
        private System.Windows.Forms.Label labelNodos;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.NumericUpDown numericUpDownX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.NumericUpDown numericUpDownY;
        private System.Windows.Forms.Button buttonAgregar;
        private System.Windows.Forms.Button buttonEliminar;
        private System.Windows.Forms.ListBox listBoxNodos;
        private System.Windows.Forms.Button buttonLimpiar;
        private System.Windows.Forms.Button buttonDibujar;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}