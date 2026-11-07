using zkemkeeper;
using PruebaZkTecoCOM.Modelos;

namespace PruebaZkTecoCOM.Servicios;

public class HuellaServicio
{
    private readonly CZKEM _zk;
    private readonly int _maquina;

    public HuellaServicio(CZKEM zk, int maquina = 1)
    {
        _zk = zk;
        _maquina = maquina;
    }

    public List<Huella> ConsultarHuellas()
    {
        List<Huella> huellas = new();

        Console.WriteLine("Preparando lectura de plantillas...");

        bool lectura = _zk.ReadAllTemplate(_maquina);

        Console.WriteLine($"Resultado de ReadAllTemplate: {lectura}");

        if (!lectura)
        {
            int error = 0;
            _zk.GetLastError(ref error);

            throw new Exception(
                $"No se pudieron leer las plantillas. Error: {error}"
            );
        }

        Console.WriteLine("Consultando lista de usuarios...");

        List<Usuario> usuarios = new UsuarioServicio(_zk, _maquina)
            .ConsultarUsuarios();

        Console.WriteLine($"Usuarios encontrados: {usuarios.Count}");

        int revisados = 0;

        foreach (Usuario usuario in usuarios)
        {
            Console.WriteLine(
                $"Revisando empleado {usuario.CodigoEmpleado}..."
            );

            for (int dedo = 0; dedo < 10; dedo++)
            {
                bool encontrada = _zk.GetUserTmpExStr(
                    _maquina,
                    usuario.CodigoEmpleado,
                    dedo,
                    out int flag,
                    out string plantilla,
                    out int longitud
                );

                if (encontrada && !string.IsNullOrEmpty(plantilla))
                {
                    huellas.Add(new Huella
                    {
                        CodigoEmpleado = usuario.CodigoEmpleado,
                        IndiceDedo = dedo,
                        Flag = flag,
                        Longitud = longitud,
                        Plantilla = plantilla
                    });

                    Console.WriteLine($"Huella encontrada: empleado {usuario.CodigoEmpleado}, dedo {dedo}");
                }
            }

            revisados++;
            Console.WriteLine($"Empleados recibidos: {revisados}");
        }

        return huellas;
    }
}