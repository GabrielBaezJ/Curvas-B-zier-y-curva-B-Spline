using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Curvas
{
    /// <summary>
    /// Clase base para la validación de puntos de control
    /// </summary>
    internal class PuntoControl
    {
        public float X { get; set; }
        public float Y { get; set; }

        public PuntoControl(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}, {Y})";
    }

    /// <summary>
    /// Interfaz para los diferentes algoritmos de Curvas de Bézier
    /// Permite modularidad y fácil reemplazo de implementaciones
    /// </summary>
    internal interface IAlgoritmoBezier
    {
        /// <summary>
        /// Calcula los puntos de la curva de Bézier
        /// </summary>
        /// <param name="puntosControl">Lista de puntos de control</param>
        /// <param name="resolucion">Número de puntos a calcular en la curva</param>
        /// <returns>Lista de puntos que forman la curva</returns>
        List<PointF> CalcularCurva(List<PuntoControl> puntosControl, int resolucion);

        /// <summary>
        /// Nombre descriptivo del algoritmo
        /// </summary>
        string Nombre { get; }

        /// <summary>
        /// Descripción del algoritmo
        /// </summary>
        string Descripcion { get; }
    }

    /// <summary>
    /// Algoritmo de Bézier usando el método de Casteljau
    /// Este es el método más preciso y eficiente para calcular curvas de Bézier
    /// </summary>
    internal class AlgoritmoCasteljau : IAlgoritmoBezier
    {
        public string Nombre => "De Casteljau";
        public string Descripcion => "Método de De Casteljau - Cálculo recursivo de puntos intermedios";

        /// <summary>
        /// Implementa el algoritmo de De Casteljau para calcular puntos en la curva de Bézier
        /// Funciona mediante interpolación recursiva de puntos de control
        /// </summary>
        public List<PointF> CalcularCurva(List<PuntoControl> puntosControl, int resolucion)
        {
            ValidarPuntosControl(puntosControl);
            ValidarResolucion(resolucion);

            List<PointF> curva = new List<PointF>();

            for (int i = 0; i <= resolucion; i++)
            {
                float t = (float)i / resolucion;
                PointF punto = CalcularPuntoCasteljau(puntosControl, t);
                curva.Add(punto);
            }

            return curva;
        }

        /// <summary>
        /// Calcula un punto en la curva para un valor t específico usando De Casteljau
        /// </summary>
        private PointF CalcularPuntoCasteljau(List<PuntoControl> puntos, float t)
        {
            // Crear copia de los puntos para trabajar recursivamente
            List<PointF> puntosTrabajo = puntos.Cast<PuntoControl>()
                .Select(p => new PointF(p.X, p.Y))
                .ToList();

            // Reducir iterativamente hasta obtener un punto
            while (puntosTrabajo.Count > 1)
            {
                List<PointF> nuevosPuntos = new List<PointF>();

                for (int i = 0; i < puntosTrabajo.Count - 1; i++)
                {
                    float x = (1 - t) * puntosTrabajo[i].X + t * puntosTrabajo[i + 1].X;
                    float y = (1 - t) * puntosTrabajo[i].Y + t * puntosTrabajo[i + 1].Y;
                    nuevosPuntos.Add(new PointF(x, y));
                }

                puntosTrabajo = nuevosPuntos;
            }

            return puntosTrabajo[0];
        }

        private void ValidarPuntosControl(List<PuntoControl> puntos)
        {
            if (puntos == null || puntos.Count < 2)
                throw new ArgumentException("Se requieren al menos 2 puntos de control");
        }

        private void ValidarResolucion(int resolucion)
        {
            if (resolucion < 10)
                throw new ArgumentException("La resolución debe ser mayor a 10");
        }
    }

    /// <summary>
    /// Algoritmo de Bézier usando polinomios de Bernstein
    /// Utiliza la fórmula matemática directa de Bernstein
    /// </summary>
    internal class AlgoritmoBernstein : IAlgoritmoBezier
    {
        public string Nombre => "Polinomios de Bernstein";
        public string Descripcion => "Método de Polinomios de Bernstein - Cálculo directo mediante fórmula";

        /// <summary>
        /// Implementa el cálculo de curva de Bézier mediante polinomios de Bernstein
        /// Formula: B(t) = Σ(i=0 to n) C(n,i) * (1-t)^(n-i) * t^i * Pi
        /// </summary>
        public List<PointF> CalcularCurva(List<PuntoControl> puntosControl, int resolucion)
        {
            ValidarPuntosControl(puntosControl);
            ValidarResolucion(resolucion);

            List<PointF> curva = new List<PointF>();
            int n = puntosControl.Count - 1;

            for (int i = 0; i <= resolucion; i++)
            {
                float t = (float)i / resolucion;
                float x = 0, y = 0;

                for (int j = 0; j <= n; j++)
                {
                    float bernstein = CalcularPolinomioBernstein(n, j, t);
                    x += bernstein * puntosControl[j].X;
                    y += bernstein * puntosControl[j].Y;
                }

                curva.Add(new PointF(x, y));
            }

            return curva;
        }

        /// <summary>
        /// Calcula el valor del polinomio de Bernstein B(n, i, t)
        /// </summary>
        private float CalcularPolinomioBernstein(int n, int i, float t)
        {
            float binomio = CalcularCoeficienteBinomio(n, i);
            float factor1 = (float)Math.Pow(1 - t, n - i);
            float factor2 = (float)Math.Pow(t, i);
            return binomio * factor1 * factor2;
        }

        /// <summary>
        /// Calcula el coeficiente binomio C(n, k)
        /// </summary>
        private float CalcularCoeficienteBinomio(int n, int k)
        {
            if (k > n) return 0;
            if (k == 0 || k == n) return 1;

            float resultado = 1;
            for (int i = 0; i < k; i++)
            {
                resultado = resultado * (n - i) / (i + 1);
            }
            return resultado;
        }

        private void ValidarPuntosControl(List<PuntoControl> puntos)
        {
            if (puntos == null || puntos.Count < 2)
                throw new ArgumentException("Se requieren al menos 2 puntos de control");
        }

        private void ValidarResolucion(int resolucion)
        {
            if (resolucion < 10)
                throw new ArgumentException("La resolución debe ser mayor a 10");
        }
    }

    /// <summary>
    /// Algoritmo de Bézier usando interpolación lineal simple
    /// Variante más básica pero aún efectiva
    /// </summary>
    internal class AlgoritmoInterpolacionLineal : IAlgoritmoBezier
    {
        public string Nombre => "Interpolación Lineal";
        public string Descripcion => "Método de Interpolación Lineal - Aproximación simplificada";

        /// <summary>
        /// Implementa el cálculo de curva de Bézier mediante interpolación lineal iterativa
        /// </summary>
        public List<PointF> CalcularCurva(List<PuntoControl> puntosControl, int resolucion)
        {
            ValidarPuntosControl(puntosControl);
            ValidarResolucion(resolucion);

            List<PointF> curva = new List<PointF>();

            for (int i = 0; i <= resolucion; i++)
            {
                float t = (float)i / resolucion;
                PointF punto = InterpolerRecursivamente(puntosControl, t);
                curva.Add(punto);
            }

            return curva;
        }

        /// <summary>
        /// Interpola recursivamente entre puntos de control
        /// </summary>
        private PointF InterpolerRecursivamente(List<PuntoControl> puntos, float t)
        {
            if (puntos.Count == 1)
            {
                return new PointF(puntos[0].X, puntos[0].Y);
            }

            List<PuntoControl> puntosMedios = new List<PuntoControl>();

            for (int i = 0; i < puntos.Count - 1; i++)
            {
                float x = (1 - t) * puntos[i].X + t * puntos[i + 1].X;
                float y = (1 - t) * puntos[i].Y + t * puntos[i + 1].Y;
                puntosMedios.Add(new PuntoControl(x, y));
            }

            return InterpolerRecursivamente(puntosMedios, t);
        }

        private void ValidarPuntosControl(List<PuntoControl> puntos)
        {
            if (puntos == null || puntos.Count < 2)
                throw new ArgumentException("Se requieren al menos 2 puntos de control");
        }

        private void ValidarResolucion(int resolucion)
        {
            if (resolucion < 10)
                throw new ArgumentException("La resolución debe ser mayor a 10");
        }
    }

    /// <summary>
    /// Controlador principal para el manejo de Curvas de Bézier
    /// Gestiona la selección de algoritmos, validación y cálculo de curvas
    /// </summary>
    internal class CCurvasBéizer
    {
        private List<IAlgoritmoBezier> algoritmos;
        private IAlgoritmoBezier algoritmoActual;

        public CCurvasBéizer()
        {
            // Inicializar lista de algoritmos disponibles
            algoritmos = new List<IAlgoritmoBezier>
            {
                new AlgoritmoCasteljau(),
                new AlgoritmoBernstein(),
                new AlgoritmoInterpolacionLineal()
            };

            // Establecer algoritmo por defecto
            algoritmoActual = algoritmos[0];
        }

        /// <summary>
        /// Obtiene la lista de nombres de algoritmos disponibles
        /// </summary>
        public List<string> ObtenerNombresAlgoritmos()
        {
            return algoritmos.Select(a => a.Nombre).ToList();
        }

        /// <summary>
        /// Obtiene las descripciones de todos los algoritmos
        /// </summary>
        public List<string> ObtenerDescripciones()
        {
            return algoritmos.Select(a => a.Descripcion).ToList();
        }

        /// <summary>
        /// Selecciona un algoritmo por su nombre
        /// </summary>
        public bool SeleccionarAlgoritmo(string nombreAlgoritmo)
        {
            try
            {
                var algoritmo = algoritmos.FirstOrDefault(a => a.Nombre == nombreAlgoritmo);
                if (algoritmo != null)
                {
                    algoritmoActual = algoritmo;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Calcula la curva de Bézier con validación completa
        /// </summary>
        public List<PointF> CalcularCurva(List<PuntoControl> puntosControl, int resolucion)
        {
            try
            {
                // Validar entrada del usuario
                ValidarEntrada(puntosControl, resolucion);

                // Calcular la curva con el algoritmo actual
                return algoritmoActual.CalcularCurva(puntosControl, resolucion);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Error en validación: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al calcular la curva: {ex.Message}");
            }
        }

        /// <summary>
        /// Valida que los puntos de control sean válidos
        /// </summary>
        private void ValidarEntrada(List<PuntoControl> puntosControl, int resolucion)
        {
            // Validar puntos de control
            if (puntosControl == null)
                throw new ArgumentException("La lista de puntos de control no puede ser nula");

            if (puntosControl.Count < 2)
                throw new ArgumentException("Se requieren al menos 2 puntos de control");

            if (puntosControl.Count > 10)
                throw new ArgumentException("Máximo 10 puntos de control permitidos");

            // Validar cada punto
            for (int i = 0; i < puntosControl.Count; i++)
            {
                if (puntosControl[i] == null)
                    throw new ArgumentException($"El punto {i} es nulo");

                if (float.IsNaN(puntosControl[i].X) || float.IsNaN(puntosControl[i].Y))
                    throw new ArgumentException($"El punto {i} contiene valores inválidos");

                if (float.IsInfinity(puntosControl[i].X) || float.IsInfinity(puntosControl[i].Y))
                    throw new ArgumentException($"El punto {i} contiene valores infinitos");
            }

            // Validar resolución
            if (resolucion < 10)
                throw new ArgumentException("La resolución debe ser al menos 10 puntos");

            if (resolucion > 1000)
                throw new ArgumentException("La resolución máxima es 1000 puntos");
        }

        /// <summary>
        /// Obtiene el nombre del algoritmo actual
        /// </summary>
        public string ObtenerAlgoritmoActual()
        {
            return algoritmoActual.Nombre;
        }

        /// <summary>
        /// Obtiene la descripción del algoritmo actual
        /// </summary>
        public string ObtenerDescripcionActual()
        {
            return algoritmoActual.Descripcion;
        }

        /// <summary>
        /// Helper para habilitar DoubleBuffered en un Control mediante reflexión
        /// Evita parpadeo en controles que no exponen la propiedad públicamente
        /// </summary>
        public static void HabilitarDoubleBuffer(Control ctrl)
        {
            if (ctrl == null) return;
            try
            {
                typeof(Control).InvokeMember(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null,
                    ctrl,
                    new object[] { true }
                );
            }
            catch
            {
                // Ignorar errores de reflexión
            }
        }
    }
}
