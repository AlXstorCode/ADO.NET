using Microsoft.Data.Sqlite;

// Programa de consola: guarda y consulta personajes de Sonic en una base de datos SQLite usando ADO.NET.

// 1. Conexión: SqliteConnection abre el archivo sonicOG.db y, si no existe, lo crea.
//    La ruta es relativa, por eso el .db queda junto al .exe (bin/Debug/net10.0).
//    "using var" cierra la conexión automáticamente al terminar el programa, aunque ocurra un error.
using var conexion = new SqliteConnection("Data Source=sonicOG.db");
conexion.Open();

Console.WriteLine("Conexión abierta. Base de datos lista.");

// 2. Crear la tabla: un SqliteCommand lleva la orden CREATE TABLE a SQLite.
//    IF NOT EXISTS evita el error cuando la tabla ya existe (desde la segunda ejecución).
//    Se usa ExecuteNonQuery porque esta orden no devuelve filas.
using var comando = conexion.CreateCommand();
comando.CommandText = """

    CREATE TABLE IF NOT EXISTS PersonajesSonic(

        ID INTEGER PRIMARY KEY AUTOINCREMENT,
        Nombre TEXT NOT NULL,
        Aparicion INTEGER,
        Juego TEXT);

    """;
comando.ExecuteNonQuery();
Console.WriteLine("Tabla creada correctamente.");

// 3. Contar los registros: ExecuteScalar devuelve un solo valor, el resultado de COUNT(*).
//    Llega como object, por eso se convierte a long.
using var contar = conexion.CreateCommand();
contar.CommandText = "SELECT COUNT(*) FROM PersonajesSonic";
long cantidad = (long)contar.ExecuteScalar();

// 4. Insertar solo si la tabla está vacía, para no duplicar los personajes en cada ejecución.
//    Los valores van como parámetros ($nombre, $aparicion, $juego) y no pegados en el texto SQL;
//    así SQLite los trata siempre como datos y se evita la inyección SQL.
//    Parameters.Clear() vacía los valores antes de cargar el siguiente personaje.
if (cantidad == 0)
{
    Console.WriteLine("\nLa tabla está vacía. Insertando personajes...");

    using var insertar = conexion.CreateCommand();
    insertar.CommandText = """
    INSERT INTO PersonajesSonic (Nombre, Aparicion, Juego)
    VALUES ($nombre, $aparicion, $juego);
    """;
    insertar.Parameters.AddWithValue("$nombre", "Sonic");
    insertar.Parameters.AddWithValue("$aparicion", 1991);
    insertar.Parameters.AddWithValue("$juego", "Sonic the Hedgehog");

    int filas = insertar.ExecuteNonQuery();
    Console.WriteLine($"Registro insertado ({filas} fila): Sonic");

    insertar.Parameters.Clear();

    insertar.Parameters.AddWithValue("$nombre", "Tails");
    insertar.Parameters.AddWithValue("$aparicion", 1992);
    insertar.Parameters.AddWithValue("$juego", "Sonic the Hedgehog 2");

    filas = insertar.ExecuteNonQuery();
    Console.WriteLine($"Registro insertado ({filas} fila): Tails");

    insertar.Parameters.Clear();

    insertar.Parameters.AddWithValue("$nombre", "Amy");
    insertar.Parameters.AddWithValue("$aparicion", 1993);
    insertar.Parameters.AddWithValue("$juego", "Sonic the Hedgehog CD");

    filas = insertar.ExecuteNonQuery();
    Console.WriteLine($"Registro insertado ({filas} fila): Amy");
}
else
{
    Console.WriteLine($"\nYa hay {cantidad} personajes guardados en la base de datos. No se insertan de nuevo");
}

// 5. Consultar y mostrar: ExecuteReader devuelve las filas del SELECT y SqliteDataReader las recorre.
//    Read() avanza a la siguiente fila y devuelve false cuando ya no quedan más.
//    Cada columna se lee por su índice, empezando en 0: 0 = ID, 1 = Nombre, 2 = Aparicion, 3 = Juego.
using var consultar = conexion.CreateCommand();
consultar.CommandText = "SELECT ID, Nombre, Aparicion, Juego FROM PersonajesSonic";

using var lector = consultar.ExecuteReader();

Console.WriteLine("\nPersonajes de Sonic guardados en la base:");
while (lector.Read())
{
    int id = lector.GetInt32(0);
    string nombre = lector.GetString(1);
    int aparicion = lector.GetInt32(2);
    string juego = lector.GetString(3);
    Console.WriteLine($"ID: {id}, Nombre: {nombre}, Aparición: {aparicion}, Juego: {juego}");
}