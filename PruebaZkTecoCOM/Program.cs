using DotNetEnv;
using PruebaZkTecoCOM.Datos;
using PruebaZkTecoCOM.Reportes;
using PruebaZkTecoCOM.Servicios;
using zkemkeeper;

Env.Load();

try
{
    string cadenaConexion =
        Environment.GetEnvironmentVariable("CONNECTION_STRING")
        ?? throw new Exception("No se encontró CONNECTION_STRING en .env");

    var repositorio = new RepositorioRelojes(cadenaConexion);
    var conexion = new ConexionZKTeco();
    var todasLasMarcaciones = new List<MarcacionExcel>(); // NUEVO

    var relojes = await repositorio.ObtenerRelojesActivosAsync();

    Console.WriteLine($"Relojes activos encontrados: {relojes.Count}");

    if (relojes.Count == 0)
    {
        Console.WriteLine("No hay relojes activos registrados.");
        return;
    }

    foreach (var reloj in relojes)
    {
        Console.WriteLine("\n--------------------------------");
        Console.WriteLine($"Reloj: {reloj.Nombre}");
        Console.WriteLine($"IP: {reloj.IP}");

        CZKEM? zk = null;

        try
        {
            zk = conexion.Conectar(reloj);

            Console.WriteLine("Conexión exitosa.");

            // NUEVO
            bool lectura = zk.ReadGeneralLogData(reloj.NumeroMaquina);

            if (!lectura)
            {
                int error = 0;
                zk.GetLastError(ref error);

                Console.WriteLine($"No se pudieron leer las marcaciones. Error: {error}");
            }
            else
            {
                Console.WriteLine("Lectura de registros preparada correctamente.");

                string codigoEmpleado = "";
                int verificacion = 0;
                int tipoMarcacion = 0;

                int anio = 0;
                int mes = 0;
                int dia = 0;
                int hora = 0;
                int minuto = 0;
                int segundo = 0;
                int codigoTrabajo = 0;

                int cantidad = 0;

                while (zk.SSR_GetGeneralLogData(
                    reloj.NumeroMaquina,
                    out codigoEmpleado,
                    out verificacion,
                    out tipoMarcacion,
                    out anio,
                    out mes,
                    out dia,
                    out hora,
                    out minuto,
                    out segundo,
                    ref codigoTrabajo))
                {
                    cantidad++;

                    DateTime fechaHora = new DateTime(
                        anio, mes, dia, hora, minuto, segundo
                    );

                    // NUEVO
                    todasLasMarcaciones.Add(new MarcacionExcel
                    {
                        IdReloj = reloj.Id,
                        IdAgencia = reloj.IdAgencia,
                        NombreReloj = reloj.Nombre,
                        CodigoEmpleado = codigoEmpleado,
                        FechaHora = fechaHora,
                        Estado = tipoMarcacion == 0 ? "Entrada" :
                                 tipoMarcacion == 1 ? "Salida" : "Salida",
                        Verificacion = verificacion,
                        TipoMarcacion = tipoMarcacion
                    });

                    Console.WriteLine(
                        $"Agencia: {reloj.IdAgencia} | " +
                        $"Reloj: {reloj.Nombre} | " +
                        $"IP: {reloj.IP} | " +
                        $"Empleado: {codigoEmpleado} | " +
                        $"Fecha: {fechaHora:dd/MM/yyyy} | " +
                        $"Hora: {fechaHora:HH:mm:ss} | " +
                        $"Verificación: {verificacion} | " +
                        $"Tipo: {tipoMarcacion}"
                    );
                }

                Console.WriteLine($"Total de marcaciones leídas: {cantidad}");
            }
            // Hasta aqui

        }

        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
        }
        finally
        {
            if (zk != null)
            {
                conexion.Desconectar(zk);
                Console.WriteLine("Conexión cerrada.");
            }
        }
    }
    // NUEVO GENERAR ARCHIVO
    var generadorExcel = new GeneradorExcel();

    generadorExcel.Generar(todasLasMarcaciones);

    Console.WriteLine("\nArchivo Excel generado correctamente.");
    Console.WriteLine($"Total de marcaciones: {todasLasMarcaciones.Count}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error general: {ex.Message}");
}