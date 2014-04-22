# Tarea 1 - Nicolás Gómez

## Instrucciones:
1. Abrir en Visual Studio
2. Ejecutar
3. Por consola, colocar path absoluto al archivo de configuración (si es que se especifica)

## Valores por defecto:
Los valores por defecto se especifican en la clase Config, y son los siguientes:

1. generaciones = 100000;
2. poblacion = 40;
3. crossover = 0.5;
4. mutacion = 0.1;
5. presupuesto_max = 600000000;

## Repositorio:
https://github.com/muZk/T1-EVOL (subida 53 minutos tarde)

### Adicionales:
- Hay TestUnitarios para los métodos base de la solución.
- Función objetivo: Maximizar dinero asignado a los postulantes.
- Función fitness: suma del dinero asignado.
- Representación: string de "trits", 0 representa "NADA", 1 representa "MEDIA BECA", y 3 "BECA COMPLETA".
- Inicialmente se crea un schema con las restricciones básicas.
- Selección por torneo (subconjuntos de tamaño 2). El torneo tenía 10% de probabilidad de elegir el mejor en vez del peor, para así agregar variabilidad.
- Lo de considerar a los de "mejor nota, menor ingreso" se consideró para cuando en el torneo peliaran dos soluciones distintas con igual fitness.
- La mutación es sobre un gen al azar, y mantiene al cromosoma válido (según el schema).
- Los cromosomas que no cumplen con alguna de las restricciones de nota o de ingresos son penalizados por un factor igual a: (número de restricciones no cumplidas del cromosoma) / (número de restricciones totales del cromosoma)
- Los cromosomas que no cumplen con la restricción de presupuesto, son reparados. La reparación finaliza cuando el cromosoma es válido. La reparación va sacando a los que menos aportan a la función fitness, hasta que cumple con la restricción de resupuesto.