namespace PruebaZkTecoCOM.Modelos;

public class Usuario
{
    public string CodigoEmpleado { get; set; } = "";

    public string Nombre { get; set; } = "";

    public string Password { get; set; } = "";

    public int Privilegio { get; set; }

    public bool Habilitado { get; set; }
}