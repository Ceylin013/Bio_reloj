using zkemkeeper;
using PruebaZkTecoCOM.Modelos;

namespace PruebaZkTecoCOM.Servicios;

public class AsistenciaServicio
{
    private readonly CZKEM _zk;
    private readonly int _maquina;

    public AsistenciaServicio(CZKEM zk, int maquina = 1)
    {
        _zk = zk;
        _maquina = maquina;
    }

    public List<Marcacion> ConsultarMarcaciones()
    {
        List<Marcacion> marcaciones = new();

        bool lectura = _zk.ReadGeneralLogData(_maquina);

        if (!lectura)
        {
            int error = 0;
            _zk.GetLastError(ref error);

            throw new Exception(
                $"No se pudieron leer las marcaciones. Error: {error}"
            );
        }
        int workCode = 0;

        while (_zk.SSR_GetGeneralLogData(
            _maquina,
            out string codigoEmpleado,
            out int verificacion,
            out int estado,
            out int anio,
            out int mes,
            out int dia,
            out int hora,
            out int minuto,
            out int segundo,
            ref workCode))
        {
            DateTime fechaHora = new DateTime(
                anio, mes, dia, hora, minuto, segundo
            );

            marcaciones.Add(new Marcacion
            {
                CodigoEmpleado = codigoEmpleado,
                FechaHora = fechaHora,
                TipoVerificacion = verificacion,
                EstadoMarcacion = estado,
                NumeroMaquina = _maquina
            });
        }

        return marcaciones;
    }
}