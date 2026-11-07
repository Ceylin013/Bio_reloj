using ClosedXML.Excel;

namespace PruebaZkTecoCOM.Reportes;

public class MarcacionExcel
{
    public int IdReloj { get; set; }
    public int IdAgencia { get; set; }
    public string NombreReloj { get; set; } = "";
    public string CodigoEmpleado { get; set; } = "";
    public DateTime FechaHora { get; set; }
    public string Estado { get; set; } = ""; // NUEVO
    public int Verificacion { get; set; }
    public int TipoMarcacion { get; set; }
}

public class GeneradorExcel
{
    public void Generar(List<MarcacionExcel> marcaciones)
    {
        using var libro = new XLWorkbook();
        var hoja = libro.Worksheets.Add("Marcaciones");

        string[] encabezados =
        {
            "ID Reloj",
            "ID Agencia",
            "Nombre del reloj",
            "Código empleado",
            "Fecha",
            "Hora",
            "Estado",
            "Verificación",
            "Tipo de marcación"
        };

        for (int i = 0; i < encabezados.Length; i++)
        {
            hoja.Cell(1, i + 1).Value = encabezados[i];
        }

        var encabezado = hoja.Range(1, 1, 1, encabezados.Length);
        encabezado.Style.Font.Bold = true;
        encabezado.Style.Fill.BackgroundColor = XLColor.LightBlue;

        int fila = 2;

        foreach (var marcacion in marcaciones)
        {
            hoja.Cell(fila, 1).Value = marcacion.IdReloj;
            hoja.Cell(fila, 2).Value = marcacion.IdAgencia;
            hoja.Cell(fila, 3).Value = marcacion.NombreReloj;
            hoja.Cell(fila, 4).Value = marcacion.CodigoEmpleado;
            hoja.Cell(fila, 5).Value = marcacion.FechaHora.Date;
            hoja.Cell(fila, 6).Value = marcacion.FechaHora.TimeOfDay;
            hoja.Cell(fila, 7).Value = marcacion.Estado;
            hoja.Cell(fila, 8).Value = marcacion.Verificacion;
            hoja.Cell(fila, 9).Value = marcacion.TipoMarcacion;

            fila++;
        }

        hoja.Column(5).Style.DateFormat.Format = "dd/MM/yyyy";
        hoja.Column(6).Style.DateFormat.Format = "HH:mm:ss";
        hoja.Columns().AdjustToContents();

        libro.SaveAs("Marcaciones_ultimas.xlsx");
    }
}