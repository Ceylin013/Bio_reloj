using System;
using DotNetEnv;
using zkteco_attendance_api;
using System.Linq;

Env.Load();
Console.WriteLine("=== PRUEBA DE CONEXIÓN CON ZKTECO K40 ===");

// Leer configuración
string? Ip = Environment.GetEnvironmentVariable("ZKTECO_IP");

string? Puerto = Environment.GetEnvironmentVariable("ZKTECO_PORT");

string? clave = Environment.GetEnvironmentVariable("ZKTECO_PASSWORD");

// Validar ip
if (string.IsNullOrWhiteSpace(Ip))
{
    Console.WriteLine("La ip no es válido.");
    return;
}

// Validar puerto
if (!int.TryParse(Puerto, out int puerto) ||
    puerto < 1 || puerto > 65535)
{
    Console.WriteLine("El puerto configurado no es válido.");
    return;
}

// Validar contraseña numérica
if (string.IsNullOrWhiteSpace(clave) ||
    !int.TryParse(clave, out int password) ||
    password < 0)
{
    Console.WriteLine(
        "Configura una contraseña numérica válida"
    );
    return;
}


var reloj = new ZkTeco(Ip, puerto);

reloj.NotifyCommandError += mensaje =>
{
    Console.WriteLine($"ERROR DEL RELOJ: {mensaje}");
};

bool conectado = false;

try
{
Console.WriteLine($"Intentando conectar con el reloj: {Ip}:{puerto}..");

conectado = reloj.Connect(password);

if (!conectado)
{
    Console.WriteLine("No se pudo establecer la conexión.");
    Console.WriteLine("Verifica la IP, el puerto, la red y la configuración del reloj.");
    return;
}

Console.WriteLine("¡Conexión exitosa!");

Console.WriteLine("\n=== PRUEBA DE LECTURA DE USUARIOS ===");

var usuarios = reloj.GetUsers();

if (usuarios == null)
{
    Console.WriteLine("La librería devolvió null al leer los usuarios.");
}
else
{
    Console.WriteLine($"Usuarios leídos: {usuarios.Count}");

    foreach (var usuario in usuarios.Take(5))
    {
        Console.WriteLine(
            $"ID: {usuario.UserId} | Nombre: {usuario.Name}"
        );
    }
}

Console.WriteLine("\n=== DIAGNÓSTICO DE MARCACIONES ===");

var detalle = reloj.GetStorageDetails();

if (detalle == null)
{
    Console.WriteLine("No se pudo consultar el almacenamiento.");
}
else
{
    Console.WriteLine($"Registros informados: {detalle.Records}");
    Console.WriteLine($"Usuarios informados: {detalle.Users}");
    Console.WriteLine($"Huellas informadas: {detalle.Fingers}");
}

string? nombre = reloj.GetDeviceName();
string? ip = reloj.GetDeviceIp();
string? numeroSerie = reloj.GetDeviceSerial();
string? firmware = reloj.GetFirmwareVersion();

Console.WriteLine("\n=== INFORMACIÓN DEL DISPOSITIVO ===");
Console.WriteLine($"Nombre: {nombre ?? "No disponible"}");
Console.WriteLine($"IP: {ip ?? "No disponible"}");
Console.WriteLine($"Número de serie: {numeroSerie ?? "No disponible"}");
Console.WriteLine($"Firmware: {firmware ?? "No disponible"}");

var almacenamiento = reloj.GetStorageDetails();

if (almacenamiento != null)
{
    Console.WriteLine("\n=== ALMACENAMIENTO DEL RELOJ ===");
    Console.WriteLine($"Usuarios registrados: {almacenamiento.Users}");
    Console.WriteLine($"Marcaciones almacenadas: {almacenamiento.Records}");
    Console.WriteLine($"Huellas almacenadas: {almacenamiento.Fingers}");
}

}
catch (Exception ex)
{
Console.WriteLine($"Error: {ex.Message}");
}
finally
{
if (conectado)
{
try
{
reloj.Disconnect();
Console.WriteLine("\nConexión cerrada.");
}
catch (Exception ex)
{
Console.WriteLine($"Error al desconectar: {ex.Message}");
}
}
}