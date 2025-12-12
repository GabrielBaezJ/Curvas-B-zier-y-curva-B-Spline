using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Curvas
{
    /// <summary>
    /// Clase para representar un nodo en las curvas B-Spline
    /// </summary>
    internal class NodoControl
    {
        public float X { get; set; }
        public float Y { get; set; }

        public NodoControl(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}, {Y})";
    }

    /// <summary>
    /// Interfaz para los diferentes algoritmos de B-Spline
    /// Permite modularidad y fácil reemplazo de implementaciones
    /// </summary>
    internal interface IAlgoritmoBSpline
    {
        /// <summary>
        /// Calcula los puntos de la curva B-Spline
        /// </summary>
        /// <param name="nodosControl">Lista de nodos de control</param>
        /// <param name="grado">Grado de la curva B-Spline (1=lineal, 2=cuadrática, 3=cúbica, etc)</param>
        /// <param name="resolucion">Número de puntos a calcular en la curva</param>
        /// <returns>Lista de puntos que forman la curva</returns>
        List<PointF> CalcularCurva(List<NodoControl> nodosControl, int grado, int resolucion);

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
    /// Algoritmo B-Spline Uniforme (Uniform B-Spline)
    /// Utiliza un vector de nodos uniforme para el cálculo
    /// </summary>
    internal class AlgoritmoBSplineUniforme : IAlgoritmoBSpline
    {
        public string Nombre => "B-Spline Uniforme";
        public string Descripcion => "B-Spline Uniforme - Cálculo con vector de nodos uniforme";

        /// <summary>
        /// Implementa el cálculo de curva B-Spline Uniforme
        /// </summary>
        public List<PointF> CalcularCurva(List<NodoControl> nodosControl, int grado, int resolucion)
        {
            ValidarNodosControl(nodosControl);
            ValidarGrado(grado, nodosControl.Count);
            ValidarResolucion(resolucion);

            List<PointF> curva = new List<PointF>();
            int n = nodosControl.Count - 1;

            // Crear vector de nodos uniforme
            List<float> vectorNodos = CrearVectorNodosUniforme(n, grado);

            // Parámetro t varía desde grado hasta n+1
            float tMin = grado;
            float tMax = n + 1;

            for (int i = 0; i <= resolucion; i++)
            {
                float t = tMin + (tMax - tMin) * (i / (float)resolucion);
                PointF punto = CalcularPuntoBSpline(nodosControl, vectorNodos, grado, t);
                curva.Add(punto);
            }

            return curva;
        }

        /// <summary>
        /// Crea un vector de nodos uniforme
        /// </summary>
        private List<float> CrearVectorNodosUniforme(int n, int p)
        {
            List<float> nodos = new List<float>();
            int m = n + p + 1;

            for (int i = 0; i <= m; i++)
            {
                nodos.Add(i);
            }

            return nodos;
        }

        /// <summary>
        /// Calcula un punto en la curva B-Spline usando la función base de Cox-de Boor
        /// </summary>
        private PointF CalcularPuntoBSpline(List<NodoControl> puntos, List<float> nodos, int p, float t)
        {
            int n = puntos.Count - 1;
            float x = 0, y = 0;

            for (int i = 0; i <= n; i++)
            {
                float N = FuncionBaseBSpline(i, p, t, nodos);
                x += N * puntos[i].X;
                y += N * puntos[i].Y;
            }

            return new PointF(x, y);
        }

        /// <summary>
        /// Función base B-Spline recursiva de Cox-de Boor
        /// N(i,p,t) = función base de grado p en el parámetro t
        /// </summary>
        private float FuncionBaseBSpline(int i, int p, float t, List<float> nodos)
        {
            if (p == 0)
            {
                // Caso base: función escalonada
                return (nodos[i] <= t && t < nodos[i + 1]) ? 1.0f : 0.0f;
            }

            float c1 = 0, c2 = 0;
            float den1 = nodos[i + p] - nodos[i];
            float den2 = nodos[i + p + 1] - nodos[i + 1];

            if (den1 != 0)
                c1 = (t - nodos[i]) / den1 * FuncionBaseBSpline(i, p - 1, t, nodos);

            if (den2 != 0)
                c2 = (nodos[i + p + 1] - t) / den2 * FuncionBaseBSpline(i + 1, p - 1, t, nodos);

            return c1 + c2;
        }

        private void ValidarNodosControl(List<NodoControl> nodos)
        {
            if (nodos == null || nodos.Count < 2)
                throw new ArgumentException("Se requieren al menos 2 nodos de control");

            if (nodos.Count > 15)
                throw new ArgumentException("Máximo 15 nodos de control permitidos");
        }

        private void ValidarGrado(int grado, int numNodos)
        {
            if (grado < 1)
                throw new ArgumentException("El grado debe ser al menos 1");

            if (grado >= numNodos)
                throw new ArgumentException($"El grado debe ser menor que el número de nodos ({numNodos})");

            if (grado > 5)
                throw new ArgumentException("El grado máximo permitido es 5");
        }

        private void ValidarResolucion(int resolucion)
        {
            if (resolucion < 10)
                throw new ArgumentException("La resolución debe ser mayor a 10");
        }
    }

    /// <summary>
    /// Algoritmo B-Spline No Uniforme (Non-Uniform B-Spline)
    /// Permite más control sobre la distribución de los puntos
    /// </summary>
    internal class AlgoritmoBSplineNoUniforme : IAlgoritmoBSpline
    {
        public string Nombre => "B-Spline No Uniforme";
        public string Descripcion => "B-Spline No Uniforme - Vector de nodos personalizado";

        public List<PointF> CalcularCurva(List<NodoControl> nodosControl, int grado, int resolucion)
        {
            ValidarNodosControl(nodosControl);
            ValidarGrado(grado, nodosControl.Count);
            ValidarResolucion(resolucion);

            List<PointF> curva = new List<PointF>();
            int n = nodosControl.Count - 1;

            // Crear vector de nodos no uniforme con distribución Centripetal
            List<float> vectorNodos = CrearVectorNodosNoUniforme(nodosControl, grado);

            float tMin = grado;
            float tMax = n + 1;

            for (int i = 0; i <= resolucion; i++)
            {
                float t = tMin + (tMax - tMin) * (i / (float)resolucion);
                PointF punto = CalcularPuntoBSpline(nodosControl, vectorNodos, grado, t);
                curva.Add(punto);
            }

            return curva;
        }

        /// <summary>
        /// Crea un vector de nodos no uniforme (Centripetal Chord Length)
        /// </summary>
        private List<float> CrearVectorNodosNoUniforme(List<NodoControl> puntos, int p)
        {
            List<float> nodos = new List<float>();
            int n = puntos.Count - 1;

            // Nodos al inicio
            for (int i = 0; i <= p; i++)
            {
                nodos.Add(0);
            }

            // Nodos interiores basados en distancia centripetal
            float[] distancias = new float[n + 1];
            distancias[0] = 0;

            for (int i = 1; i <= n; i++)
            {
                float dx = puntos[i].X - puntos[i - 1].X;
                float dy = puntos[i].Y - puntos[i - 1].Y;
                distancias[i] = distancias[i - 1] + (float)Math.Sqrt(Math.Sqrt(dx * dx + dy * dy));
            }

            for (int i = 1; i < n; i++)
            {
                float t = distancias[i] / distancias[n];
                nodos.Add(t * (n - p + 1));
            }

            // Nodos al final
            for (int i = 0; i <= p; i++)
            {
                nodos.Add(n - p + 1);
            }

            return nodos;
        }

        private PointF CalcularPuntoBSpline(List<NodoControl> puntos, List<float> nodos, int p, float t)
        {
            int n = puntos.Count - 1;
            float x = 0, y = 0;

            for (int i = 0; i <= n; i++)
            {
                float N = FuncionBaseBSpline(i, p, t, nodos);
                x += N * puntos[i].X;
                y += N * puntos[i].Y;
            }

            return new PointF(x, y);
        }

        private float FuncionBaseBSpline(int i, int p, float t, List<float> nodos)
        {
            if (p == 0)
            {
                return (nodos[i] <= t && t < nodos[i + 1]) ? 1.0f : 0.0f;
            }

            float c1 = 0, c2 = 0;
            float den1 = nodos[i + p] - nodos[i];
            float den2 = nodos[i + p + 1] - nodos[i + 1];

            if (den1 != 0)
                c1 = (t - nodos[i]) / den1 * FuncionBaseBSpline(i, p - 1, t, nodos);

            if (den2 != 0)
                c2 = (nodos[i + p + 1] - t) / den2 * FuncionBaseBSpline(i + 1, p - 1, t, nodos);

            return c1 + c2;
        }

        private void ValidarNodosControl(List<NodoControl> nodos)
        {
            if (nodos == null || nodos.Count < 2)
                throw new ArgumentException("Se requieren al menos 2 nodos de control");

            if (nodos.Count > 15)
                throw new ArgumentException("Máximo 15 nodos de control permitidos");
        }

        private void ValidarGrado(int grado, int numNodos)
        {
            if (grado < 1)
                throw new ArgumentException("El grado debe ser al menos 1");

            if (grado >= numNodos)
                throw new ArgumentException($"El grado debe ser menor que el número de nodos ({numNodos})");

            if (grado > 5)
                throw new ArgumentException("El grado máximo permitido es 5");
        }

        private void ValidarResolucion(int resolucion)
        {
            if (resolucion < 10)
                throw new ArgumentException("La resolución debe ser mayor a 10");
        }
    }

    /// <summary>
    /// Algoritmo NURBS (Non-Uniform Rational B-Spline)
    /// Versión racional que permite más flexibilidad en la forma de la curva
    /// </summary>
    internal class AlgoritmoNURBS : IAlgoritmoBSpline
    {
        public string Nombre => "NURBS";
        public string Descripcion => "NURBS - B-Spline Racional No Uniforme con pesos";

        public List<PointF> CalcularCurva(List<NodoControl> nodosControl, int grado, int resolucion)
        {
            ValidarNodosControl(nodosControl);
            ValidarGrado(grado, nodosControl.Count);
            ValidarResolucion(resolucion);

            List<PointF> curva = new List<PointF>();
            int n = nodosControl.Count - 1;

            // Crear vector de nodos uniforme
            List<float> vectorNodos = CrearVectorNodosUniforme(n, grado);

            // Pesos: 1.0 para puntos normales, mayor para puntos que queremos atraer más
            float[] pesos = new float[nodosControl.Count];
            for (int i = 0; i < pesos.Length; i++)
            {
                pesos[i] = 1.0f;
            }

            float tMin = grado;
            float tMax = n + 1;

            for (int i = 0; i <= resolucion; i++)
            {
                float t = tMin + (tMax - tMin) * (i / (float)resolucion);
                PointF punto = CalcularPuntoNURBS(nodosControl, vectorNodos, pesos, grado, t);
                curva.Add(punto);
            }

            return curva;
        }

        /// <summary>
        /// Crea un vector de nodos uniforme
        /// </summary>
        private List<float> CrearVectorNodosUniforme(int n, int p)
        {
            List<float> nodos = new List<float>();
            int m = n + p + 1;

            for (int i = 0; i <= m; i++)
            {
                nodos.Add(i);
            }

            return nodos;
        }

        /// <summary>
        /// Calcula un punto en la curva NURBS
        /// </summary>
        private PointF CalcularPuntoNURBS(List<NodoControl> puntos, List<float> nodos, float[] pesos, int p, float t)
        {
            int n = puntos.Count - 1;
            float numX = 0, numY = 0, den = 0;

            for (int i = 0; i <= n; i++)
            {
                float N = FuncionBaseBSpline(i, p, t, nodos);
                float Nw = N * pesos[i];
                numX += Nw * puntos[i].X;
                numY += Nw * puntos[i].Y;
                den += Nw;
            }

            if (den == 0) den = 1;
            return new PointF(numX / den, numY / den);
        }

        private float FuncionBaseBSpline(int i, int p, float t, List<float> nodos)
        {
            if (p == 0)
            {
                return (nodos[i] <= t && t < nodos[i + 1]) ? 1.0f : 0.0f;
            }

            float c1 = 0, c2 = 0;
            float den1 = nodos[i + p] - nodos[i];
            float den2 = nodos[i + p + 1] - nodos[i + 1];

            if (den1 != 0)
                c1 = (t - nodos[i]) / den1 * FuncionBaseBSpline(i, p - 1, t, nodos);

            if (den2 != 0)
                c2 = (nodos[i + p + 1] - t) / den2 * FuncionBaseBSpline(i + 1, p - 1, t, nodos);

            return c1 + c2;
        }

        private void ValidarNodosControl(List<NodoControl> nodos)
        {
            if (nodos == null || nodos.Count < 2)
                throw new ArgumentException("Se requieren al menos 2 nodos de control");

            if (nodos.Count > 15)
                throw new ArgumentException("Máximo 15 nodos de control permitidos");
        }

        private void ValidarGrado(int grado, int numNodos)
        {
            if (grado < 1)
                throw new ArgumentException("El grado debe ser al menos 1");

            if (grado >= numNodos)
                throw new ArgumentException($"El grado debe ser menor que el número de nodos ({numNodos})");

            if (grado > 5)
                throw new ArgumentException("El grado máximo permitido es 5");
        }

        private void ValidarResolucion(int resolucion)
        {
            if (resolucion < 10)
                throw new ArgumentException("La resolución debe ser mayor a 10");
        }
    }

    /// <summary>
    /// Controlador principal para el manejo de Curvas B-Spline
    /// Gestiona la selección de algoritmos, validación y cálculo de curvas
    /// </summary>
    internal class CBSpline
    {
        private List<IAlgoritmoBSpline> algoritmos;
        private IAlgoritmoBSpline algoritmoActual;

        public CBSpline()
        {
            // Inicializar lista de algoritmos disponibles
            algoritmos = new List<IAlgoritmoBSpline>
            {
                new AlgoritmoBSplineUniforme(),
                new AlgoritmoBSplineNoUniforme(),
                new AlgoritmoNURBS()
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
        /// Calcula la curva B-Spline con validación completa
        /// </summary>
        public List<PointF> CalcularCurva(List<NodoControl> nodosControl, int grado, int resolucion)
        {
            try
            {
                // Validar entrada del usuario
                ValidarEntrada(nodosControl, grado, resolucion);

                // Calcular la curva con el algoritmo actual
                return algoritmoActual.CalcularCurva(nodosControl, grado, resolucion);
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
        /// Valida que los nodos de control sean válidos
        /// </summary>
        private void ValidarEntrada(List<NodoControl> nodosControl, int grado, int resolucion)
        {
            // Validar nodos de control
            if (nodosControl == null)
                throw new ArgumentException("La lista de nodos de control no puede ser nula");

            if (nodosControl.Count < 2)
                throw new ArgumentException("Se requieren al menos 2 nodos de control");

            if (nodosControl.Count > 15)
                throw new ArgumentException("Máximo 15 nodos de control permitidos");

            // Validar cada nodo
            for (int i = 0; i < nodosControl.Count; i++)
            {
                if (nodosControl[i] == null)
                    throw new ArgumentException($"El nodo {i} es nulo");

                if (float.IsNaN(nodosControl[i].X) || float.IsNaN(nodosControl[i].Y))
                    throw new ArgumentException($"El nodo {i} contiene valores inválidos");

                if (float.IsInfinity(nodosControl[i].X) || float.IsInfinity(nodosControl[i].Y))
                    throw new ArgumentException($"El nodo {i} contiene valores infinitos");
            }

            // Validar grado
            if (grado < 1)
                throw new ArgumentException("El grado debe ser al menos 1");

            if (grado >= nodosControl.Count)
                throw new ArgumentException($"El grado debe ser menor que el número de nodos ({nodosControl.Count})");

            if (grado > 5)
                throw new ArgumentException("El grado máximo permitido es 5");

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
    }
}
