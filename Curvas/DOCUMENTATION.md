# Sistema de Curvas de Bézier
## Documentación Técnica

### Descripción General
Sistema interactivo para la visualización y manipulación de Curvas de Bézier implementado en C# con Windows Forms. Proporciona tres algoritmos diferentes para calcular y dibujar curvas de Bézier con una interfaz gráfica clara e intuitiva.

---

## Características Principales

### 1. **Algoritmos Implementados**

#### Algoritmo de De Casteljau
- **Nombre**: De Casteljau
- **Descripción**: Método de De Casteljau - Cálculo recursivo de puntos intermedios
- **Ventajas**: 
  - Más preciso numéricamente
  - Mejor para cálculos de puntos individuales
  - Ideal para aplicaciones interactivas
- **Complejidad**: O(n²) por punto calculado

#### Algoritmo de Polinomios de Bernstein
- **Nombre**: Polinomios de Bernstein
- **Descripción**: Método de Polinomios de Bernstein - Cálculo directo mediante fórmula
- **Ventajas**:
  - Fórmula matemática directa
  - Excelente para análisis teórico
  - Buen rendimiento general
- **Complejidad**: O(n²) por punto calculado

#### Algoritmo de Interpolación Lineal
- **Nombre**: Interpolación Lineal
- **Descripción**: Método de Interpolación Lineal - Aproximación simplificada
- **Ventajas**:
  - Más simple de comprender
  - Rápido para propósitos didácticos
  - Buena aproximación visual
- **Complejidad**: O(n) por punto calculado

### 2. **Características de la Interfaz Gráfica**

#### Panel de Controles
- **Selector de Algoritmo**: ComboBox con lista de algoritmos disponibles
- **Descripción del Algoritmo**: Área de texto con explicación del algoritmo seleccionado
- **Control de Resolución**: NumericUpDown para ajustar la densidad de puntos (10-1000)
- **Entrada de Coordenadas**: NumericUpDown para X (0-600) e Y (0-450)
- **Botones de Acción**:
  - Agregar Punto (Verde)
  - Eliminar Seleccionado (Rojo)
  - Limpiar Todo (Naranja)
  - Dibujar Curva (Azul)
- **Lista de Puntos**: ListBox que muestra todos los puntos de control agregados
- **Barra de Estado**: Muestra mensajes de estado y error

#### Canvas de Dibujo
- Panel blanco de 600x450 píxeles
- **Características de Visualización**:
  - Grilla de referencia cada 50 píxeles
  - Puntos de control dibujados como círculos rojos
  - Polígono de control (líneas verdes conectando puntos)
  - Curva de Bézier dibujada en azul
  - Números identificadores en cada punto de control

---

## Validación y Manejo de Errores

### Validaciones de Entrada

#### Puntos de Control
- **Mínimo**: 2 puntos requeridos
- **Máximo**: 10 puntos permitidos
- **Rango X**: 0-600 píxeles
- **Rango Y**: 0-450 píxeles
- **Validación de Valores**: Se rechazan valores nulos, NaN o infinitos

#### Resolución
- **Mínimo**: 10 puntos en la curva
- **Máximo**: 1000 puntos en la curva
- **Validación**: Se restringe automáticamente en el NumericUpDown

### Manejo de Excepciones
- Try-catch en todos los eventos del usuario
- Mensajes de error descriptivos
- Actualización de barra de estado con errores
- La aplicación no se cierra inesperadamente

---

## Estructura de Clases

### PuntoControl
Clase que representa un punto de control 2D.
```csharp
public class PuntoControl
{
    public float X { get; set; }
    public float Y { get; set; }
    public PuntoControl(float x, float y)
}
```

### IAlgoritmoBezier (Interfaz)
Define la estructura para los algoritmos de Bézier.
```csharp
internal interface IAlgoritmoBezier
{
    List<PointF> CalcularCurva(List<PuntoControl> puntosControl, int resolucion);
    string Nombre { get; }
    string Descripcion { get; }
}
```

### Clases de Algoritmos
- `AlgoritmoCasteljau`: Implementa el método de De Casteljau
- `AlgoritmoBernstein`: Implementa polinomios de Bernstein
- `AlgoritmoInterpolacionLineal`: Implementa interpolación lineal

### CCurvasBéizer (Controlador Principal)
Gestiona:
- Lista de algoritmos disponibles
- Selección del algoritmo actual
- Validación de entrada completa
- Cálculo de curvas

### FrmCurvasBéizer (Formulario Principal)
Interfaz gráfica que:
- Maneja eventos del usuario
- Dibuja puntos y curvas
- Actualiza el estado de la aplicación
- Gestiona la lista de puntos

---

## Guía de Uso

### Paso 1: Seleccionar Algoritmo
1. Use el ComboBox "Algoritmo:" para elegir entre:
   - De Casteljau
   - Polinomios de Bernstein
   - Interpolación Lineal
2. La descripción se actualiza automáticamente

### Paso 2: Agregar Puntos de Control
**Método 1 - Entrada Manual:**
1. Ingrese coordenadas X (0-600) e Y (0-450)
2. Haga clic en "Agregar Punto"

**Método 2 - Clic en Canvas:**
1. Haga clic directamente en el area blanca
2. Los puntos se agregarán con las coordenadas del clic

### Paso 3: Gestionar Puntos
- **Ver Puntos**: Se muestran en el ListBox con formato "P[n]: (x, y)"
- **Eliminar**: Seleccione un punto en la lista y haga clic "Eliminar Seleccionado"
- **Limpiar Todo**: Borre todos los puntos con un clic

### Paso 4: Ajustar Resolución
- Use "Resolución:" para controlar la suavidad de la curva
- Valores bajos (10-50): Curva menos suave pero más rápido
- Valores altos (500-1000): Curva muy suave pero más lento

### Paso 5: Dibujar Curva
1. Asegúrese de tener al menos 2 puntos
2. Haga clic en "Dibujar Curva"
3. La curva se dibujará en azul en el canvas

---

## Fórmulas Matemáticas

### De Casteljau
Interpolación recursiva de puntos intermedios:
```
Para cada t ? [0, 1]:
  P?¹ = (1-t)P? + tP?
  P?¹ = (1-t)P? + tP?
  ...
  P?² = (1-t)P?¹ + tP?¹
  ...
```

### Polinomios de Bernstein
```
B(t) = ?(i=0 to n) C(n,i) * (1-t)^(n-i) * t^i * P?

Donde:
  C(n,i) = n! / (i! * (n-i)!)  (coeficiente binomio)
  P? = punto de control i
  t ? [0, 1]
```

### Interpolación Lineal
```
P(t) = (1-t) * P? + t * P?
```

---

## Organización del Código

### Modularidad
- Cada algoritmo está en su propia clase
- Interfaz `IAlgoritmoBezier` permite fácil extensión
- Controlador `CCurvasBéizer` centraliza la lógica
- Formulario `FrmCurvasBéizer` solo maneja UI

### Estilo de Codificación
- Nombres significativos de variables y métodos
- Comentarios descriptivos en cada clase y método
- Uso de convenciones PascalCase para clases/métodos
- Documentación XML para métodos públicos

### Manejo de Excepciones
- Validaciones al inicio de métodos públicos
- Mensajes de error claros y específicos
- Recuperación graceful de errores
- Nunca permite estado inconsistente

---

## Restricciones y Limitaciones

### Técnicas
- Máximo 10 puntos de control
- Máximo 1000 puntos en la curva
- Resolución mínima de 10 puntos
- Canvas de 600x450 píxeles

### Matemáticas
- Coordenadas solo en enteros (0-600 para X, 0-450 para Y)
- Precisión de float (32 bits)
- Parámetro t ? [0, 1]

---

## Ejemplos de Uso

### Ejemplo 1: Curva Simple (2 puntos)
1. Algoritmo: Cualquiera
2. Puntos: (100, 100) y (500, 350)
3. Resolución: 50
4. Resultado: Línea recta (esperado con 2 puntos)

### Ejemplo 2: Curva Cuadrática (3 puntos)
1. Algoritmo: De Casteljau
2. Puntos: (50, 400), (300, 50), (550, 400)
3. Resolución: 200
4. Resultado: Parábola

### Ejemplo 3: Curva Cúbica (4 puntos)
1. Algoritmo: Polinomios de Bernstein
2. Puntos: (50, 200), (150, 50), (450, 50), (550, 200)
3. Resolución: 500
4. Resultado: Curva suave de tercera orden

---

## Rendimiento

### Complejidad Temporal
- Cálculo de curva: O(n × m²)
  - n = número de puntos en la curva (resolución)
  - m = número de puntos de control

### Complejidad Espacial
- O(n) para almacenar la curva calculada
- O(m) para almacenar puntos de control

### Casos de Prueba
| Puntos | Resolución | Tiempo Est. |
|--------|-----------|------------|
| 2      | 100       | ~1ms       |
| 5      | 500       | ~5ms       |
| 10     | 1000      | ~20ms      |

---

## Extensiones Futuras

### Posibles Mejoras
1. Agregar más algoritmos (Slerp, B-splines)
2. Exportar curvas a archivos (SVG, PNG)
3. Importar puntos desde archivo
4. Animación de construcción de la curva
5. Cálculo de longitud de la curva
6. Derivadas y curvaturas
7. Soportar múltiples curvas simultáneamente
8. Zoom y pan en el canvas

---

## Requisitos del Sistema

### Software
- .NET Framework 4.7.2 o superior
- Windows Vista o superior
- 20 MB de espacio libre

### Hardware
- Procesador de 1 GHz
- 256 MB RAM
- Pantalla de 1024x768 mínimo

---

## Licencia y Autoría

**Proyecto**: Sistema de Curvas de Bézier
**Versión**: 1.0
**Asignatura**: Computación Gráfica
**Semestre**: 7
**Universidad**: ESPE

---

## Contacto y Soporte

Para preguntas o sugerencias sobre este sistema, consulte la documentación del código o contacte al desarrollador.
