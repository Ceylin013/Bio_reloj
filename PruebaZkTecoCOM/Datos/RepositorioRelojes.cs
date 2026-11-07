using Microsoft.Data.SqlClient;
using PruebaZkTecoCOM.Configuracion;

namespace PruebaZkTecoCOM.Datos;

public class RepositorioRelojes
{
    private readonly string _cadenaConexion;

    public RepositorioRelojes(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    public async Task<List<ConfiguracionReloj>> ObtenerRelojesActivosAsync()
    {
        var relojes = new List<ConfiguracionReloj>();

        const string consulta = @"
            SELECT
                IdReloj,
                IdAgencia,
                Nombre,
                IP,
                Puerto,
                PasswordComunicacion,
                NumeroMaquina,
                Estado
            FROM dbo.Relojes
            WHERE Estado = 1;";

        await using var conexion = new SqlConnection(_cadenaConexion);
        await conexion.OpenAsync();

        await using var comando = new SqlCommand(consulta, conexion);
        await using var lector = await comando.ExecuteReaderAsync();

        while (await lector.ReadAsync())
        {
            relojes.Add(new ConfiguracionReloj
            {
                Id = Convert.ToInt32(lector["IdReloj"]),
                IdAgencia = Convert.ToInt32(lector["IdAgencia"]),
                Nombre = lector["Nombre"].ToString() ?? "",
                IP = lector["IP"].ToString() ?? "",
                Puerto = Convert.ToInt32(lector["Puerto"]),
                Password = Convert.ToInt32(lector["PasswordComunicacion"]),
                NumeroMaquina = Convert.ToInt32(lector["NumeroMaquina"]),
                Activo = Convert.ToBoolean(lector["Estado"])
            });
        }

        return relojes;
    }
}