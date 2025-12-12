using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Curvas
{
    /// <summary>
    /// Formulario principal para la visualización y manipulación de Curvas B-Spline
    /// Proporciona una interfaz gráfica intuitiva con interactividad en vivo
    /// </summary>
    public partial class FrmBSpline : Form
    {
        private CBSpline controladorCurvas;
        private List<NodoControl> nodosControl = new List<NodoControl>();
        private List<PointF> puntoCurva = new List<PointF>();
        private List<List<PointF>> rastrosCurva = new List<List<PointF>>();
        private bool curvaCalculada = false;
        
        // Variables para la manipulación interactiva
        private int nodoSeleccionado = -1;
        private bool arrastrando = false;
        private const int RADIO_SELECCION = 8;

        public FrmBSpline()
        {
            InitializeComponent();
            controladorCurvas = new CBSpline();
        }

        /// <summary>
        /// Inicializa el formulario con los algoritmos disponibles
        /// </summary>
        private void FrmBSpline_Load(object sender, EventArgs e)
        {
            try
            {
                // Cargar algoritmos disponibles
                comboBoxAlgoritmos.DataSource = controladorCurvas.ObtenerNombresAlgoritmos();
                comboBoxAlgoritmos.SelectedIndex = 0;
                ActualizarDescripcion();
                ActualizarEstado("Sistema listo. Agregue nodos de control para comenzar o arrastre los nodos para moverlos en vivo.");
            }
            catch (Exception ex)
            {
                MostrarError("Error al inicializar el formulario: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento cuando se selecciona un algoritmo diferente
        /// </summary>
        private void comboBoxAlgoritmos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreAlgoritmo = comboBoxAlgoritmos.SelectedItem.ToString();
                if (controladorCurvas.SeleccionarAlgoritmo(nombreAlgoritmo))
                {
                    ActualizarDescripcion();
                    ActualizarEstado($"Algoritmo '{nombreAlgoritmo}' seleccionado");
                    RecalcularCurva();
                    panelCanvas.Invalidate();
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al cambiar algoritmo: " + ex.Message);
            }
        }

        /// <summary>
        /// Actualiza la descripción del algoritmo seleccionado
        /// </summary>
        private void ActualizarDescripcion()
        {
            try
            {
                textBoxDescripcion.Text = controladorCurvas.ObtenerDescripcionActual();
            }
            catch (Exception ex)
            {
                textBoxDescripcion.Text = "Error al obtener descripción: " + ex.Message;
            }
        }

        /// <summary>
        /// Evento del botón "Agregar Nodo"
        /// Valida e incorpora un nuevo nodo de control
        /// </summary>
        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener valores
                float x = (float)numericUpDownX.Value;
                float y = (float)numericUpDownY.Value;

                // Validar rangos
                if (x < 0 || x > 600)
                    throw new ArgumentException("X debe estar entre 0 y 600");

                if (y < 0 || y > 450)
                    throw new ArgumentException("Y debe estar entre 0 y 450");

                // Validar límite de nodos
                if (nodosControl.Count >= 15)
                    throw new ArgumentException("Máximo 15 nodos de control permitidos");

                // Agregar nodo
                NodoControl nodo = new NodoControl(x, y);
                nodosControl.Add(nodo);

                // Actualizar UI
                ActualizarListaNodos();
                RecalcularCurva();
                panelCanvas.Invalidate();
                ActualizarEstado($"Nodo agregado: ({x}, {y}). Total: {nodosControl.Count} nodos");
            }
            catch (Exception ex)
            {
                MostrarError("Error al agregar nodo: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento del botón "Eliminar Seleccionado"
        /// Elimina el nodo seleccionado de la lista
        /// </summary>
        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxNodos.SelectedIndex < 0)
                    throw new Exception("Por favor, seleccione un nodo para eliminar");

                int indice = listBoxNodos.SelectedIndex;
                nodosControl.RemoveAt(indice);

                ActualizarListaNodos();
                RecalcularCurva();
                panelCanvas.Invalidate();
                ActualizarEstado($"Nodo eliminado. Total: {nodosControl.Count} nodos");
            }
            catch (Exception ex)
            {
                MostrarError("Error al eliminar nodo: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento del botón "Limpiar Todo"
        /// Elimina todos los nodos de control y limpia el canvas
        /// </summary>
        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                nodosControl.Clear();
                puntoCurva.Clear();
                rastrosCurva.Clear();
                curvaCalculada = false;
                nodoSeleccionado = -1;
                ActualizarListaNodos();
                panelCanvas.Invalidate();
                ActualizarEstado("Canvas limpiado. Agregue nuevos nodos de control.");
            }
            catch (Exception ex)
            {
                MostrarError("Error al limpiar: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento del botón "Dibujar Curva"
        /// Calcula y dibuja la curva B-Spline con el algoritmo seleccionado
        /// </summary>
        private void buttonDibujar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar nodos
                if (nodosControl.Count < 2)
                    throw new Exception("Se requieren al menos 2 nodos de control");

                // Obtener parámetros
                int grado = (int)numericUpDownGrado.Value;
                int resolucion = (int)numericUpDownResolucion.Value;

                // Validar que el grado sea menor que el número de nodos
                if (grado >= nodosControl.Count)
                    throw new Exception($"El grado debe ser menor que el número de nodos ({nodosControl.Count})");

                // Calcular curva
                puntoCurva = controladorCurvas.CalcularCurva(nodosControl, grado, resolucion);
                curvaCalculada = true;
                
                // Agregar al rastro si está habilitado
                if (checkBoxAnimar.Checked)
                {
                    rastrosCurva.Add(new List<PointF>(puntoCurva));
                }

                // Redibujar
                panelCanvas.Invalidate();
                ActualizarEstado($"Curva B-Spline dibujada con {puntoCurva.Count} puntos usando {controladorCurvas.ObtenerAlgoritmoActual()}");
            }
            catch (Exception ex)
            {
                MostrarError("Error al calcular curva: " + ex.Message);
            }
        }

        /// <summary>
        /// Recalcula la curva automáticamente cuando cambian los nodos
        /// </summary>
        private void RecalcularCurva()
        {
            try
            {
                if (nodosControl.Count >= 2)
                {
                    int grado = (int)numericUpDownGrado.Value;
                    int resolucion = (int)numericUpDownResolucion.Value;

                    // Ajustar el grado si es necesario
                    if (grado >= nodosControl.Count)
                    {
                        grado = nodosControl.Count - 1;
                        numericUpDownGrado.Value = grado;
                    }

                    puntoCurva = controladorCurvas.CalcularCurva(nodosControl, grado, resolucion);
                    curvaCalculada = true;
                }
                else
                {
                    curvaCalculada = false;
                }
            }
            catch
            {
                curvaCalculada = false;
            }
        }

        /// <summary>
        /// Evento cuando se hace clic en el canvas
        /// Permite agregar nodos de control haciendo clic directamente en el canvas
        /// </summary>
        private void panelCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                int x = e.X;
                int y = e.Y;

                if (x < 0 || x > 600 || y < 0 || y > 450)
                    return;

                if (nodosControl.Count >= 15)
                {
                    MostrarError("Máximo 15 nodos de control permitidos");
                    return;
                }

                // Actualizar valores en los NumericUpDown
                numericUpDownX.Value = Math.Min(600, Math.Max(0, x));
                numericUpDownY.Value = Math.Min(450, Math.Max(0, y));

                // Agregar nodo automáticamente
                buttonAgregar_Click(null, null);
            }
            catch (Exception ex)
            {
                MostrarError("Error al agregar nodo desde canvas: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento cuando se presiona el botón del mouse en el canvas
        /// Detecta si se está seleccionando un nodo para arrastrarlo
        /// </summary>
        private void panelCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                int x = e.X;
                int y = e.Y;

                // Buscar si hay un nodo cerca del cursor
                nodoSeleccionado = -1;
                for (int i = 0; i < nodosControl.Count; i++)
                {
                    float dx = nodosControl[i].X - x;
                    float dy = nodosControl[i].Y - y;
                    float distancia = (float)Math.Sqrt(dx * dx + dy * dy);

                    if (distancia <= RADIO_SELECCION)
                    {
                        nodoSeleccionado = i;
                        arrastrando = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error en MouseDown: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento cuando se mueve el mouse en el canvas
        /// Si se está arrastrando un nodo, lo mueve en vivo
        /// </summary>
        private void panelCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (arrastrando && nodoSeleccionado >= 0)
                {
                    int x = Math.Min(600, Math.Max(0, e.X));
                    int y = Math.Min(450, Math.Max(0, e.Y));

                    // Actualizar la posición del nodo
                    nodosControl[nodoSeleccionado].X = x;
                    nodosControl[nodoSeleccionado].Y = y;

                    // Actualizar la lista visual
                    ActualizarListaNodos();

                    // Recalcular y redibujar la curva en tiempo real
                    RecalcularCurva();
                    panelCanvas.Invalidate();

                    // Actualizar estado
                    ActualizarEstado($"Moviendo nodo {nodoSeleccionado}: ({x}, {y})");
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error en MouseMove: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento cuando se suelta el botón del mouse en el canvas
        /// Finaliza el arrastre del nodo
        /// </summary>
        private void panelCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (arrastrando && nodoSeleccionado >= 0)
                {
                    arrastrando = false;
                    ActualizarEstado($"Nodo {nodoSeleccionado} posicionado en ({nodosControl[nodoSeleccionado].X}, {nodosControl[nodoSeleccionado].Y})");
                    nodoSeleccionado = -1;
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error en MouseUp: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento de pintura del canvas
        /// Dibuja los nodos de control, rastros y la curva calculada
        /// </summary>
        private void panelCanvas_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Dibujar grid de referencia
                DibujarGrid(e.Graphics);

                // Dibujar rastros anteriores de curvas (si están habilitados)
                DibujarRastros(e.Graphics);

                // Dibujar curva actual si está calculada
                if (curvaCalculada && puntoCurva.Count > 1)
                {
                    DibujarCurva(e.Graphics);
                }

                // Dibujar nodos de control
                DibujarNodosControl(e.Graphics);
            }
            catch (Exception ex)
            {
                e.Graphics.DrawString("Error al dibujar: " + ex.Message, 
                    new Font("Arial", 10), Brushes.Red, 10, 10);
            }
        }

        /// <summary>
        /// Dibuja una grilla de referencia en el canvas
        /// </summary>
        private void DibujarGrid(Graphics g)
        {
            Pen penGrid = new Pen(Color.LightGray, 0.5f);
            
            // Líneas verticales cada 50 píxeles
            for (int x = 0; x <= 600; x += 50)
            {
                g.DrawLine(penGrid, x, 0, x, 450);
            }

            // Líneas horizontales cada 50 píxeles
            for (int y = 0; y <= 450; y += 50)
            {
                g.DrawLine(penGrid, 0, y, 600, y);
            }
            
            penGrid.Dispose();
        }

        /// <summary>
        /// Dibuja los rastros de curvas anteriores con transparencia
        /// </summary>
        private void DibujarRastros(Graphics g)
        {
            // Dibujar rastros con colores más débiles
            Color[] coloresRastro = {
                Color.FromArgb(100, 173, 216, 230),  // Azul claro
                Color.FromArgb(100, 144, 238, 144),  // Verde claro
                Color.FromArgb(100, 255, 192, 203),  // Rosa claro
                Color.FromArgb(100, 255, 255, 153)   // Amarillo claro
            };

            for (int i = 0; i < rastrosCurva.Count; i++)
            {
                Color color = coloresRastro[i % coloresRastro.Length];
                Pen penRastro = new Pen(color, 1);

                List<PointF> rastro = rastrosCurva[i];
                for (int j = 0; j < rastro.Count - 1; j++)
                {
                    g.DrawLine(penRastro, rastro[j], rastro[j + 1]);
                }

                penRastro.Dispose();
            }
        }

        /// <summary>
        /// Dibuja los nodos de control en el canvas
        /// </summary>
        private void DibujarNodosControl(Graphics g)
        {
            const int radioControl = 6;

            // Dibujar línea de polígono de control
            if (nodosControl.Count > 1)
            {
                Pen penPoligono = new Pen(Color.Purple, 2);
                for (int i = 0; i < nodosControl.Count - 1; i++)
                {
                    g.DrawLine(penPoligono, 
                        nodosControl[i].X, nodosControl[i].Y,
                        nodosControl[i + 1].X, nodosControl[i + 1].Y);
                }
                penPoligono.Dispose();
            }

            // Dibujar nodos de control
            for (int i = 0; i < nodosControl.Count; i++)
            {
                // Highlight si es el nodo seleccionado
                Color colorNodo = (i == nodoSeleccionado) ? Color.Yellow : Color.Red;
                Brush brushControl = new SolidBrush(colorNodo);
                
                g.FillEllipse(brushControl, 
                    nodosControl[i].X - radioControl, 
                    nodosControl[i].Y - radioControl,
                    radioControl * 2, radioControl * 2);

                // Borde del nodo
                Pen penBorde = new Pen(Color.DarkRed, 2);
                g.DrawEllipse(penBorde,
                    nodosControl[i].X - radioControl,
                    nodosControl[i].Y - radioControl,
                    radioControl * 2, radioControl * 2);

                // Dibujar número del nodo
                g.DrawString(i.ToString(), new Font("Arial", 8, FontStyle.Bold), 
                    Brushes.White, nodosControl[i].X - 2, nodosControl[i].Y - 2);

                brushControl.Dispose();
                penBorde.Dispose();
            }
        }

        /// <summary>
        /// Dibuja la curva B-Spline calculada
        /// </summary>
        private void DibujarCurva(Graphics g)
        {
            Pen penCurva = new Pen(Color.DarkBlue, 3);
            penCurva.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            penCurva.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            
            for (int i = 0; i < puntoCurva.Count - 1; i++)
            {
                g.DrawLine(penCurva, puntoCurva[i], puntoCurva[i + 1]);
            }
            
            penCurva.Dispose();
        }

        /// <summary>
        /// Actualiza la lista de nodos mostrada en el ListBox
        /// </summary>
        private void ActualizarListaNodos()
        {
            listBoxNodos.Items.Clear();
            for (int i = 0; i < nodosControl.Count; i++)
            {
                listBoxNodos.Items.Add($"N{i}: ({nodosControl[i].X:F0}, {nodosControl[i].Y:F0})");
            }
        }

        /// <summary>
        /// Actualiza el texto de estado en la barra de estado
        /// </summary>
        private void ActualizarEstado(string mensaje)
        {
            toolStripStatusLabel.Text = mensaje;
        }

        /// <summary>
        /// Muestra un mensaje de error al usuario
        /// </summary>
        private void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ActualizarEstado("Error: " + mensaje);
        }
    }
}
