using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Curvas
{
    /// <summary>
    /// Formulario principal para la visualización y manipulación de Curvas de Bézier
    /// Proporciona una interfaz gráfica intuitiva con interactividad en vivo
    /// </summary>
    public partial class FrmCurvasBéizer : Form
    {
        private CCurvasBéizer controladorCurvas;
        private List<PuntoControl> puntosControl = new List<PuntoControl>();
        private List<PointF> puntoCurva = new List<PointF>();
        private List<List<PointF>> rastrosCurva = new List<List<PointF>>();
        private bool curvaCalculada = false;
        
        // Variables para la manipulación interactiva
        private int puntoSeleccionado = -1;
        private bool arrastrando = false;
        private const int RADIO_SELECCION = 8;

        public FrmCurvasBéizer()
        {
            InitializeComponent();
            controladorCurvas = new CCurvasBéizer();
        }

        /// <summary>
        /// Inicializa el formulario con los algoritmos disponibles
        /// </summary>
        private void FrmCurvasBéizer_Load(object sender, EventArgs e)
        {
            try
            {
                // Cargar algoritmos disponibles
                comboBoxAlgoritmos.DataSource = controladorCurvas.ObtenerNombresAlgoritmos();
                comboBoxAlgoritmos.SelectedIndex = 0;
                ActualizarDescripcion();
                ActualizarEstado("Sistema listo. Agregue puntos de control para comenzar o arrastre los puntos para moverlos en vivo.");
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
        /// Evento del botón "Agregar Punto"
        /// Valida e incorpora un nuevo punto de control
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

                // Validar límite de puntos
                if (puntosControl.Count >= 10)
                    throw new ArgumentException("Máximo 10 puntos de control permitidos");

                // Agregar punto
                PuntoControl punto = new PuntoControl(x, y);
                puntosControl.Add(punto);

                // Actualizar UI
                ActualizarListaPuntos();
                RecalcularCurva();
                panelCanvas.Invalidate();
                ActualizarEstado($"Punto agregado: ({x}, {y}). Total: {puntosControl.Count} puntos");
            }
            catch (Exception ex)
            {
                MostrarError("Error al agregar punto: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento del botón "Eliminar Seleccionado"
        /// Elimina el punto seleccionado de la lista
        /// </summary>
        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxPuntos.SelectedIndex < 0)
                    throw new Exception("Por favor, seleccione un punto para eliminar");

                int indice = listBoxPuntos.SelectedIndex;
                puntosControl.RemoveAt(indice);

                ActualizarListaPuntos();
                RecalcularCurva();
                panelCanvas.Invalidate();
                ActualizarEstado($"Punto eliminado. Total: {puntosControl.Count} puntos");
            }
            catch (Exception ex)
            {
                MostrarError("Error al eliminar punto: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento del botón "Limpiar Todo"
        /// Elimina todos los puntos de control y limpia el canvas
        /// </summary>
        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                puntosControl.Clear();
                puntoCurva.Clear();
                rastrosCurva.Clear();
                curvaCalculada = false;
                puntoSeleccionado = -1;
                ActualizarListaPuntos();
                panelCanvas.Invalidate();
                ActualizarEstado("Canvas limpiado. Agregue nuevos puntos de control.");
            }
            catch (Exception ex)
            {
                MostrarError("Error al limpiar: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento del botón "Dibujar Curva"
        /// Calcula y dibuja la curva de Bézier con el algoritmo seleccionado
        /// </summary>
        private void buttonDibujar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar puntos
                if (puntosControl.Count < 2)
                    throw new Exception("Se requieren al menos 2 puntos de control");

                // Obtener resolución
                int resolucion = (int)numericUpDownResolucion.Value;

                // Calcular curva
                puntoCurva = controladorCurvas.CalcularCurva(puntosControl, resolucion);
                curvaCalculada = true;
                
                // Agregar al rastro si está habilitado
                if (checkBoxAnimar.Checked)
                {
                    rastrosCurva.Add(new List<PointF>(puntoCurva));
                }

                // Redibujar
                panelCanvas.Invalidate();
                ActualizarEstado($"Curva dibujada con {puntoCurva.Count} puntos usando {controladorCurvas.ObtenerAlgoritmoActual()}");
            }
            catch (Exception ex)
            {
                MostrarError("Error al calcular curva: " + ex.Message);
            }
        }

        /// <summary>
        /// Recalcula la curva automáticamente cuando cambian los puntos
        /// </summary>
        private void RecalcularCurva()
        {
            try
            {
                if (puntosControl.Count >= 2)
                {
                    int resolucion = (int)numericUpDownResolucion.Value;
                    puntoCurva = controladorCurvas.CalcularCurva(puntosControl, resolucion);
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
        /// Permite agregar puntos de control haciendo clic directamente en el canvas
        /// </summary>
        private void panelCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                // Convertir coordenadas del mouse a valores del NumericUpDown
                int x = e.X;
                int y = e.Y;

                if (x < 0 || x > 600 || y < 0 || y > 450)
                    return;

                if (puntosControl.Count >= 10)
                {
                    MostrarError("Máximo 10 puntos de control permitidos");
                    return;
                }

                // Actualizar valores en los NumericUpDown
                numericUpDownX.Value = Math.Min(600, Math.Max(0, x));
                numericUpDownY.Value = Math.Min(450, Math.Max(0, y));

                // Agregar punto automáticamente
                buttonAgregar_Click(null, null);
            }
            catch (Exception ex)
            {
                MostrarError("Error al agregar punto desde canvas: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento cuando se presiona el botón del mouse en el canvas
        /// Detecta si se está seleccionando un punto para arrastrarlo
        /// </summary>
        private void panelCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                int x = e.X;
                int y = e.Y;

                // Buscar si hay un punto cerca del cursor
                puntoSeleccionado = -1;
                for (int i = 0; i < puntosControl.Count; i++)
                {
                    float dx = puntosControl[i].X - x;
                    float dy = puntosControl[i].Y - y;
                    float distancia = (float)Math.Sqrt(dx * dx + dy * dy);

                    if (distancia <= RADIO_SELECCION)
                    {
                        puntoSeleccionado = i;
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
        /// Si se está arrastrando un punto, lo mueve en vivo
        /// </summary>
        private void panelCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (arrastrando && puntoSeleccionado >= 0)
                {
                    int x = Math.Min(600, Math.Max(0, e.X));
                    int y = Math.Min(450, Math.Max(0, e.Y));

                    // Actualizar la posición del punto
                    puntosControl[puntoSeleccionado].X = x;
                    puntosControl[puntoSeleccionado].Y = y;

                    // Actualizar la lista visual
                    ActualizarListaPuntos();

                    // Recalcular y redibujar la curva en tiempo real
                    RecalcularCurva();
                    panelCanvas.Invalidate();

                    // Actualizar estado
                    ActualizarEstado($"Moviendo punto {puntoSeleccionado}: ({x}, {y})");
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error en MouseMove: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento cuando se suelta el botón del mouse en el canvas
        /// Finaliza el arrastre del punto
        /// </summary>
        private void panelCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (arrastrando && puntoSeleccionado >= 0)
                {
                    arrastrando = false;
                    ActualizarEstado($"Punto {puntoSeleccionado} posicionado en ({puntosControl[puntoSeleccionado].X}, {puntosControl[puntoSeleccionado].Y})");
                    puntoSeleccionado = -1;
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error en MouseUp: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento de pintura del canvas
        /// Dibuja los puntos de control, rastros y la curva calculada
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

                // Dibujar puntos de control
                DibujarPuntosControl(e.Graphics);
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
        /// Dibuja los puntos de control en el canvas
        /// </summary>
        private void DibujarPuntosControl(Graphics g)
        {
            const int radioControl = 6;

            // Dibujar línea de polígono de control
            if (puntosControl.Count > 1)
            {
                Pen penPoligono = new Pen(Color.Green, 2);
                for (int i = 0; i < puntosControl.Count - 1; i++)
                {
                    g.DrawLine(penPoligono, 
                        puntosControl[i].X, puntosControl[i].Y,
                        puntosControl[i + 1].X, puntosControl[i + 1].Y);
                }
                penPoligono.Dispose();
            }

            // Dibujar puntos de control
            for (int i = 0; i < puntosControl.Count; i++)
            {
                // Highlight si es el punto seleccionado
                Color colorPunto = (i == puntoSeleccionado) ? Color.Yellow : Color.Red;
                Brush brushControl = new SolidBrush(colorPunto);
                
                g.FillEllipse(brushControl, 
                    puntosControl[i].X - radioControl, 
                    puntosControl[i].Y - radioControl,
                    radioControl * 2, radioControl * 2);

                // Borde del punto
                Pen penBorde = new Pen(Color.DarkRed, 2);
                g.DrawEllipse(penBorde,
                    puntosControl[i].X - radioControl,
                    puntosControl[i].Y - radioControl,
                    radioControl * 2, radioControl * 2);

                // Dibujar número del punto
                g.DrawString(i.ToString(), new Font("Arial", 8, FontStyle.Bold), 
                    Brushes.White, puntosControl[i].X - 2, puntosControl[i].Y - 2);

                brushControl.Dispose();
                penBorde.Dispose();
            }
        }

        /// <summary>
        /// Dibuja la curva de Bézier calculada
        /// </summary>
        private void DibujarCurva(Graphics g)
        {
            Pen penCurva = new Pen(Color.Blue, 3);
            penCurva.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            penCurva.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            
            for (int i = 0; i < puntoCurva.Count - 1; i++)
            {
                g.DrawLine(penCurva, puntoCurva[i], puntoCurva[i + 1]);
            }
            
            penCurva.Dispose();
        }

        /// <summary>
        /// Actualiza la lista de puntos mostrada en el ListBox
        /// </summary>
        private void ActualizarListaPuntos()
        {
            listBoxPuntos.Items.Clear();
            for (int i = 0; i < puntosControl.Count; i++)
            {
                listBoxPuntos.Items.Add($"P{i}: ({puntosControl[i].X:F0}, {puntosControl[i].Y:F0})");
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
