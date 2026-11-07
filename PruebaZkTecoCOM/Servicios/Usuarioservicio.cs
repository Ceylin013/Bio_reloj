using zkemkeeper;
using PruebaZkTecoCOM.Modelos;

namespace PruebaZkTecoCOM.Servicios;

public class UsuarioServicio
{
    private readonly CZKEM _zk;
    private readonly int _maquina;

    public UsuarioServicio(CZKEM zk, int maquina = 1)
    {
        _zk = zk;
        _maquina = maquina;
    }

    // Consultar todos los usuarios del K40
    public List<Usuario> ConsultarUsuarios()
    {
        List<Usuario> usuarios = new();

        if (!_zk.ReadAllUserID(_maquina))
        {
            int error = 0;
            _zk.GetLastError(ref error);
            throw new Exception($"Error al leer usuarios: {error}");
        }

        while (_zk.SSR_GetAllUserInfo(
            _maquina,
            out string id,
            out string nombre,
            out string password,
            out int privilegio,
            out bool habilitado))
        {
            usuarios.Add(new Usuario
            {
                CodigoEmpleado = id,
                Nombre = nombre,
                Privilegio = privilegio,
                Habilitado = habilitado
            });
        }

        return usuarios;
    }

    // Registrar un usuario nuevo en el K40
    public bool RegistrarUsuario(Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario.CodigoEmpleado))
            throw new ArgumentException("El número de empleado es obligatorio.");

        if (string.IsNullOrWhiteSpace(usuario.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");

        List<Usuario> usuarios = ConsultarUsuarios();

        if (usuarios.Any(u => u.CodigoEmpleado == usuario.CodigoEmpleado))
            throw new Exception("El usuario ya existe en el K40.");

        bool resultado = _zk.SSR_SetUserInfo(
            _maquina,
            usuario.CodigoEmpleado,
            usuario.Nombre,
            "",
            usuario.Privilegio,
            usuario.Habilitado
        );

        if (!resultado)
        {
            int error = 0;
            _zk.GetLastError(ref error);
            throw new Exception($"No se pudo registrar el usuario. Error: {error}");
        }

        return true;
    }
}