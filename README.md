# Personajes de Sonic con ADO.NET y SQLite

Programa de consola en C# que muestra cómo acceder a una base de datos con **ADO.NET** y **SQLite**. Crea una base de datos, crea una tabla de personajes de Sonic, inserta tres registros y luego los consulta y los muestra en pantalla.

Lo desarrollé para la actividad de investigación *Introducción a ADO.NET y su aplicación en C#* de la asignatura Programación en C# con .NET (Corporación Universitaria Americana).

## Qué hace el programa

1. **Conecta** con la base de datos `sonicOG.db` mediante `SqliteConnection`. Si el archivo no existe, SQLite lo crea.
2. **Crea la tabla** `PersonajesSonic` con `CREATE TABLE IF NOT EXISTS`, ejecutada con `SqliteCommand`.
3. **Cuenta los registros** con `SELECT COUNT(*)` y `ExecuteScalar`.
4. **Inserta tres personajes** (Sonic, Tails y Amy) con `SqliteCommand` y parámetros, pero solo si la tabla está vacía. Así no se duplican al ejecutar el programa varias veces.
5. **Consulta y muestra** los registros con `SqliteDataReader`.

La conexión, los comandos y el lector se declaran con `using`, para que se cierren y liberen automáticamente.

### Estructura de la tabla

| Columna | Tipo | Descripción |
|---|---|---|
| `ID` | `INTEGER PRIMARY KEY AUTOINCREMENT` | Identificador único |
| `Nombre` | `TEXT NOT NULL` | Nombre del personaje |
| `Aparicion` | `INTEGER` | Año de su primera aparición |
| `Juego` | `TEXT` | Juego en el que apareció por primera vez |

## Requisitos

- [SDK de .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0)
- Visual Studio 2026 (opcional: también se puede ejecutar desde la terminal)
- Paquete NuGet [`Microsoft.Data.Sqlite`](https://www.nuget.org/packages/Microsoft.Data.Sqlite) 11.0.0. Ya está referenciado en el proyecto y se descarga solo al compilar.

No hace falta instalar SQLite ni ningún servidor de base de datos: el paquete trae el motor incluido.

## Cómo ejecutarlo

### Con Visual Studio

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/AlXstorCode/ADO.NET.git
   ```
2. Abrir `Ado.netone.slnx` en Visual Studio 2026.
3. Ejecutar con **Ctrl + F5**. Visual Studio restaura el paquete NuGet automáticamente.

### Desde la terminal

```bash
git clone https://github.com/AlXstorCode/ADO.NET.git
cd ADO.NET
dotnet run
```

## Resultado esperado

**Primera ejecución** (la base no existe todavía):

```text
Conexión abierta. Base de datos lista.
Tabla creada correctamente.

La tabla está vacía. Insertando personajes...
Registro insertado (1 fila): Sonic
Registro insertado (1 fila): Tails
Registro insertado (1 fila): Amy

Personajes de Sonic guardados en la base:
ID: 1, Nombre: Sonic, Aparición: 1991, Juego: Sonic the Hedgehog
ID: 2, Nombre: Tails, Aparición: 1992, Juego: Sonic the Hedgehog 2
ID: 3, Nombre: Amy, Aparición: 1993, Juego: Sonic the Hedgehog CD
```

**Ejecuciones siguientes:** los datos siguen guardados en el archivo, así que no se vuelven a insertar. Esto demuestra la persistencia.

```text
Ya hay 3 personajes guardados en la base de datos. No se insertan de nuevo

Personajes de Sonic guardados en la base:
ID: 1, Nombre: Sonic, Aparición: 1991, Juego: Sonic the Hedgehog
...
```

## Dónde queda la base de datos

La cadena de conexión `Data Source=sonicOG.db` usa una ruta relativa, así que el archivo se crea en la carpeta desde donde se ejecuta el programa:

- Con Visual Studio: `bin/Debug/net10.0/sonicOG.db`
- Con `dotnet run`: en la carpeta del proyecto

Para empezar de cero, basta con borrar `sonicOG.db`: el programa lo vuelve a crear en la siguiente ejecución.

## Autor

Owhen Geovanny Padilla Polanco. Ingeniería de Sistemas, Corporación Universitaria Americana.
