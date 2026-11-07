using zkemkeeper;
using PruebaZkTecoCOM.Configuracion;

namespace PruebaZkTecoCOM.Servicios;

public class ConexionZKTeco
{
    public CZKEM Conectar(ConfiguracionReloj configuracion)
    {
        CZKEM zk = new CZKEM();

        try
        {
            zk.SetCommPassword(configuracion.Password);

            bool conectado = zk.Connect_Net(
                configuracion.IP,
                configuracion.Puerto
            );

            if (!conectado)
            {
                int error = 0;
                zk.GetLastError(ref error);

                throw new Exception(
                    $"No se pudo conectar con el reloj {configuracion.Nombre}. Error: {error}"
                );
            }

            return zk;
        }
        catch
        {
            try
            {
                zk.Disconnect();
            }
            catch
            {
                // Evita que un error al desconectar oculte el error original.
            }

            throw;
        }
    }

    public void Desconectar(CZKEM zk)
    {
        zk.Disconnect();
    }
}