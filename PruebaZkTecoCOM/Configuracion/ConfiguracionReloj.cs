namespace PruebaZkTecoCOM.Configuracion;

public class ConfiguracionReloj
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string IP { get; set; } = "";
    public int Puerto { get; set; }
    public int Password { get; set; }
    public int NumeroMaquina { get; set; } = 1;
    public int IdAgencia { get; set; }
    public bool Activo { get; set; } = true;
}