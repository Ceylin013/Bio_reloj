using Microsoft.Data.SqlClient;

namespace Biometrico.Web.Datos;

public class ConexionBD
{
    private readonly string _cadenaConexion;

    public ConexionBD(IConfiguration configuration)
    {
        _cadenaConexion = configuration.GetConnectionString("BioReloj")
            ?? throw new InvalidOperationException(
                "No se encontró la cadena de conexión BioReloj.");
    }

    public async Task<bool> ProbarConexionAsync()
    {
        await using var conexion = new SqlConnection(_cadenaConexion);

        await conexion.OpenAsync();

        return conexion.State == System.Data.ConnectionState.Open;
    }
}