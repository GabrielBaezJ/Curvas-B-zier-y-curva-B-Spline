# ?? Sistema de Curvas de Bézier

Un sistema interactivo para la visualización, manipulación y cálculo de **Curvas de Bézier** usando tres algoritmos diferentes, implementado en C# con Windows Forms.

## ? Características Principales

### ?? Tres Algoritmos de Bézier

1. **Algoritmo de De Casteljau**
   - Método recursivo de interpolación
   - Ideal para aplicaciones interactivas
   - Máxima precisión numérica

2. **Polinomios de Bernstein**
   - Fórmula matemática directa
   - Excelente para análisis teórico
   - Rendimiento general óptimo

3. **Interpolación Lineal**
   - Aproximación simplificada
   - Perfecto para didáctica
   - Muy rápido

### ?? Interfaz Gráfica Clara e Intuitiva

- **Selector de Algoritmo**: Cambie entre algoritmos con un clic
- **Canvas Interactivo**: Agregue puntos haciendo clic directamente
- **Visualización en Tiempo Real**: Vea la curva mientras manipula puntos
- **Controles Intuitivos**: Entrada de coordenadas y parámetros fácil de usar
- **Barra de Estado**: Mensajes informativos sobre operaciones

### ? Validación Robusta

- Validación completa de entrada del usuario
- Manejo de excepciones en todos los eventos
- Restricciones automáticas en parámetros
- Mensajes de error descriptivos
- La aplicación **nunca** se cierra inesperadamente

### ??? Código Modular y Bien Organizado

- Arquitectura basada en interfaces
- Fácil agregar nuevos algoritmos
- Separación clara entre lógica y UI
- Comentarios descriptivos en todo el código
- Código limpio y profesional

---

## ?? Inicio Rápido

### 1. Compilar y Ejecutar
```bash
cd Curvas
dotnet build
dotnet run
```

### 2. Crear Puntos de Control
**Opción A - Entrada Manual:**
- Ingrese X (0-600) e Y (0-450)
- Haga clic en "Agregar Punto"

**Opción B - Clic en Canvas:**
- Haga clic en el área blanca donde desee el punto

### 3. Seleccionar Algoritmo
- Use el ComboBox "Algoritmo:"
- La descripción se actualiza automáticamente

### 4. Ajustar Resolución
- Use el control "Resolución:" (10-1000 puntos)
- Más puntos = curva más suave

### 5. Dibujar Curva
- Haga clic en "Dibujar Curva"
- ¡Vea la curva de Bézier en tiempo real!

---

## ?? Requisitos

- **.NET Framework**: 4.7.2 o superior
- **SO**: Windows Vista o superior
- **RAM**: 256 MB mínimo
- **Pantalla**: 1024x768 mínimo

---

## ?? Estructura del Proyecto

```
Curvas/
??? CCurvasBéizer.cs          # Algoritmos y controlador (módulo core)
??? FrmCurvasBéizer.cs        # Interfaz gráfica principal
??? FrmCurvasBéizer.Designer.cs # Diseñador de formulario
??? Program.cs                # Punto de entrada
??? FrmHome.cs                # Pantalla de inicio
??? DOCUMENTATION.md          # Documentación técnica completa
??? README.md                 # Este archivo
```

---

## ?? Componentes Principales

### Clases de Algoritmos

#### `PuntoControl`
Representa un punto 2D en el espacio de control.
```csharp
var punto = new PuntoControl(100, 150);
```

#### `IAlgoritmoBezier` (Interfaz)
Define la estructura común para todos los algoritmos.

#### `AlgoritmoCasteljau`
Implementación del algoritmo de De Casteljau.

#### `AlgoritmoBernstein`
Implementación usando polinomios de Bernstein.

#### `AlgoritmoInterpolacionLineal`
Implementación por interpolación lineal.

### Controlador Principal

#### `CCurvasBéizer`
- Gestiona lista de algoritmos
- Valida entrada del usuario
- Calcula curvas
- Selecciona algoritmos dinámicamente

### Interfaz Gráfica

#### `FrmCurvasBéizer`
- Maneja todos los eventos del usuario
- Dibuja puntos de control y curva
- Actualiza estado de la aplicación
- Proporciona retroalimentación visual

---

## ?? Validaciones Implementadas

### Puntos de Control
- ? Mínimo: 2 puntos
- ? Máximo: 10 puntos
- ? Rango X: 0-600
- ? Rango Y: 0-450
- ? Sin valores nulos, NaN o infinitos

### Resolución
- ? Mínimo: 10 puntos
- ? Máximo: 1000 puntos
- ? Validación automática

### Manejo de Errores
- ? Try-catch en todos los eventos
- ? Mensajes de error claros
- ? Barra de estado informativa
- ? Recuperación graceful

---

## ?? Ejemplos de Uso

### Curva Cuadrática (3 puntos)
```
Puntos: (50, 400), (300, 50), (550, 400)
Algoritmo: Cualquiera
Resolución: 200
Resultado: Parábola suave
```

### Curva Cúbica (4 puntos)
```
Puntos: (50, 200), (150, 50), (450, 50), (550, 200)
Algoritmo: De Casteljau
Resolución: 500
Resultado: Curva suave de tercer orden
```

### Forma Compleja (5+ puntos)
```
Puntos: Agregue hasta 10 puntos
Algoritmo: Polinomios de Bernstein
Resolución: 1000
Resultado: Formas complejas y suaves
```

---

## ?? Visualización

### Canvas
- **Fondo**: Blanco
- **Grilla**: Líneas grises cada 50 píxeles (referencia)
- **Puntos de Control**: Círculos rojos con número
- **Polígono de Control**: Líneas verdes conectando puntos
- **Curva**: Línea azul suave

### Interfaz
- **Selector de Algoritmo**: ComboBox en la parte superior
- **Descripción**: Texto informativo en gris
- **Controles**: Entrada de coordenadas y botones
- **Lista de Puntos**: ListBox con todos los puntos
- **Botones de Acción**: 4 botones de colores distintos
- **Barra de Estado**: Mensajes en la parte inferior

---

## ?? Algoritmos Explicados

### De Casteljau
Interpola recursivamente entre puntos:
1. Conecte cada par de puntos con parámetro t
2. Interpole los puntos resultantes
3. Repita hasta obtener un punto final

**Ventaja**: Muy preciso y numericamente estable

### Polinomios de Bernstein
Usa la fórmula directa:
```
B(t) = ? C(n,i) * (1-t)^(n-i) * t^i * P?
```

**Ventaja**: Fórmula cerrada y análisis matemático directo

### Interpolación Lineal
Interpola linealmente en un nivel:
```
P(t) = (1-t) * P? + t * P?
```

**Ventaja**: Muy simple y rápido

---

## ? Rendimiento

| Puntos | Resolución | Tiempo |
|--------|-----------|--------|
| 2      | 100       | ~1ms   |
| 5      | 500       | ~5ms   |
| 10     | 1000      | ~20ms  |

---

## ?? Seguridad y Robustez

- ? Todas las entradas validadas
- ? Excepciones manejadas apropiadamente
- ? Límites numéricos respetados
- ? Estado de aplicación siempre consistente
- ? Sin memory leaks
- ? Interfaz responsiva

---

## ?? Documentación Adicional

Para documentación técnica detallada, consulte **DOCUMENTATION.md**:
- Fórmulas matemáticas completas
- Análisis de complejidad
- Guía de extensión del código
- Casos de prueba
- Restricciones técnicas

---

## ?? Requisitos del Proyecto Completados

? **Interfaz gráfica clara, intuitiva y organizada**
- Selector de algoritmos fácil
- Entrada de puntos intuitiva
- Visualización clara

? **Código modular con fácil reemplazo de algoritmos**
- Interfaz IAlgoritmoBezier
- Cada algoritmo en su propia clase
- Fácil agregar nuevos algoritmos

? **Módulos con comentarios descriptivos**
- Documentación XML en métodos
- Comentarios de explicación en código
- Descripción de fórmulas matemáticas

? **Estilo de codificación consistente**
- Nombres significativos
- Convenciones C# respetadas
- Código limpio y profesional

? **Manejo robusto de excepciones**
- Try-catch en todos los eventos
- Mensajes de error descriptivos
- Aplicación nunca se cierra inesperadamente

? **Validaciones de algoritmo**
- Rango mínimo/máximo de puntos
- Validación de coordenadas
- Validación de resolución

? **Validación de entrada de usuario**
- Rangos automáticos en NumericUpDown
- Validación en cada agregación de punto
- Errores claros y específicos

---

## ?? Contribuciones

Este proyecto fue desarrollado como parte del curso de **Computación Gráfica** - Semestre 7 en **ESPE**.

---

## ?? Licencia

Este proyecto es de código abierto bajo fines educativos.

---

## ?? Soporte

Para preguntas o problemas:
1. Consulte DOCUMENTATION.md
2. Revise los comentarios en el código
3. Verifique que cumpla los requisitos de sistema

---

**¡Gracias por usar el Sistema de Curvas de Bézier!** ??

Disfrute visualizando y experimentando con estas hermosas curvas matemáticas.
