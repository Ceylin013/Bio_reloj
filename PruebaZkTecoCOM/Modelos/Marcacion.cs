namespace PruebaZkTecoCOM.Modelos;

public class Marcacion
{
    public string CodigoEmpleado { get; set; } = "";

    public DateTime FechaHora { get; set; }

    public int TipoVerificacion { get; set; }

    public int EstadoMarcacion { get; set; }

    public int NumeroMaquina { get; set; }
}