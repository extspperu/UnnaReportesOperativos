using System;
using System.Collections.Generic;
using System.Text;

namespace Unna.OperationalReport.Tools.Comunes.Infraestructura.Dtos
{
    public class OperacionDto<T>
    {

        public bool Completado { get; }
        public CodigosOperacionDto Codigo { get; }
        public T? Resultado { get; }
        public List<string>? Mensajes { get; }

        public OperacionDto(CodigosOperacionDto codigo, string mensaje)
        {
            Codigo = codigo;
            Mensajes = new List<string>() { mensaje };
            Completado = false;
        }

        public OperacionDto(CodigosOperacionDto codigo, List<string>? mensajes)
        {
            Codigo = codigo;
            Mensajes = mensajes;
            Completado = false;
        }

        public OperacionDto(T resultado)
        {
            Completado = !(EqualityComparer<T>.Default.Equals(resultado, default));
            Mensajes = !Completado ? new List<string>() { "Sin resultado" } : null;
            Codigo = !Completado ? CodigosOperacionDto.ResultadoVacio : CodigosOperacionDto.Suceso;
            Resultado = resultado;
        }

        public OperacionDto(T resultado, CodigosOperacionDto codigo)
        {
            Completado = !(EqualityComparer<T>.Default.Equals(resultado, default));
            Mensajes = !Completado ? new List<string>() { "Sin resultado" } : null;
            Codigo = !Completado ? CodigosOperacionDto.ResultadoVacio : codigo;
            Resultado = resultado;
        }

    }
}
